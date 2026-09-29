using System;
using System.Collections.Generic;
using System.Globalization;
using Promotion.Core.Contracts;

namespace DBCacheServer
{
    /// <summary>已保存的一筆優惠錢包操作。</summary>
    internal sealed class PromotionWalletRow
    {
        public string OperationKey;
        public long UserUID;
        public string BonusTaskId;
        public byte Action;
        public decimal Amount;
        public int Sequence;
        public byte Status;
        public int AttemptCount;
    }

    /// <summary>以 OperationKey 保存優惠錢包指令。不執行餘額異動。</summary>
    internal sealed class PromotionWalletStore
    {
        internal const string TableName = "PromotionWalletOperation";
        internal const byte StatusPending = 0;
        internal const byte StatusCompleted = 1;
        internal const byte StatusFailed = 2;
        internal const int RetentionDays = 90;

        static readonly string[] Fields =
        {
            "OperationKey", "UserUID", "BonusTaskId", "Action", "Amount",
            "Sequence", "Status", "AttemptCount", "LastError", "CreatedAt", "CompletedAt"
        };

        readonly MysqlAcess mysql;

        internal PromotionWalletStore(MysqlAcess mysql)
        {
            this.mysql = mysql ?? throw new ArgumentNullException(nameof(mysql));
        }

        internal bool Save(WalletInstructionDto instruction, int sequence)
        {
            if (!CanSave(instruction))
                return false;

            try
            {
                mysql.ExecuteParameterizedTransaction(context =>
                {
                    if (SelectForUpdate(context, instruction.OperationKey) != null)
                        return 0;
                    context.Insert(TableName, new Dictionary<string, object>
                    {
                        ["OperationKey"] = instruction.OperationKey,
                        ["UserUID"] = instruction.UserUID,
                        ["BonusTaskId"] = instruction.BonusTaskId,
                        ["Action"] = (byte)instruction.Action,
                        ["Amount"] = instruction.Amount,
                        ["Sequence"] = sequence,
                        ["Status"] = StatusPending,
                        ["AttemptCount"] = 0,
                        ["LastError"] = null,
                        ["CreatedAt"] = DateTime.Now,
                        ["CompletedAt"] = null
                    });
                    return 1;
                });
                return true;
            }
            catch (PromotionDataException ex) when (ex.Kind == PromotionDataErrorKind.DuplicateKey)
            {
                return true;
            }
        }

        internal PromotionWalletRow Get(string operationKey)
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> rows = mysql.SelectParameterizedV2(new MysqlSelectCommand(
                TableName, Fields,
                new[] { Equal("OperationKey", operationKey) },
                Array.Empty<MysqlOrder>(), 1, null));
            return rows.Count == 0 ? null : Map(rows[0]);
        }

        internal bool HasEarlierIncomplete(string bonusTaskId, int sequence)
        {
            if (sequence <= 0)
                return false;

            IReadOnlyList<IReadOnlyDictionary<string, object>> rows = mysql.SelectParameterizedV2(new MysqlSelectCommand(
                TableName, new[] { "Status" },
                new[]
                {
                    Equal("BonusTaskId", bonusTaskId),
                    new MysqlCondition("Sequence", MysqlComparisonOperator.LessThan, sequence)
                },
                new[] { new MysqlOrder("Sequence", MysqlSortDirection.Ascending) },
                20, null));
            if (rows.Count < sequence)
                return true;
            for (int i = 0; i < rows.Count; i++)
            {
                if (ToInt32(rows[i]["Status"]) != StatusCompleted)
                    return true;
            }
            return false;
        }

        internal IReadOnlyList<PromotionWalletRow> ListRetryable()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> rows = mysql.SelectParameterizedV2(new MysqlSelectCommand(
                TableName, Fields,
                new[]
                {
                    new MysqlCondition("Status", MysqlComparisonOperator.In, new List<byte> { StatusPending, StatusFailed })
                },
                new[]
                {
                    new MysqlOrder("BonusTaskId", MysqlSortDirection.Ascending),
                    new MysqlOrder("Sequence", MysqlSortDirection.Ascending)
                },
                200, null));
            List<PromotionWalletRow> list = new List<PromotionWalletRow>(rows.Count);
            for (int i = 0; i < rows.Count; i++)
                list.Add(Map(rows[i]));
            return list;
        }

        internal void MarkCompleted(string operationKey)
        {
            Mark(operationKey, StatusCompleted, null, true);
        }

        internal void MarkFailed(string operationKey, string error)
        {
            Mark(operationKey, StatusFailed, error, false);
        }

        internal void DeleteExpiredCompleted()
        {
            DateTime cutoff = DateTime.Now.AddDays(-RetentionDays);
            while (true)
            {
                long deleted = mysql.DeleteParameterizedV2(new MysqlDeleteCommand(
                    TableName,
                    new[]
                    {
                        Equal("Status", StatusCompleted),
                        new MysqlCondition("CompletedAt", MysqlComparisonOperator.LessThan, cutoff)
                    },
                    new[] { new MysqlOrder("CompletedAt", MysqlSortDirection.Ascending) },
                    1000));
                if (deleted < 1000)
                    return;
            }
        }

        internal static bool CanSave(WalletInstructionDto instruction)
        {
            return instruction != null
                && !string.IsNullOrWhiteSpace(instruction.OperationKey)
                && !string.IsNullOrWhiteSpace(instruction.BonusTaskId)
                && instruction.UserUID > 0
                && Enum.IsDefined(typeof(WalletAction), instruction.Action)
                && instruction.OperationKey == instruction.BonusTaskId + ":" + instruction.Action.ToString();
        }

        void Mark(string operationKey, byte status, string error, bool completed)
        {
            mysql.ExecuteParameterizedTransaction(context =>
            {
                IReadOnlyDictionary<string, object> row = SelectForUpdate(context, operationKey);
                if (row == null || ToInt32(row["Status"]) == StatusCompleted)
                    return 0;
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    ["Status"] = status,
                    ["AttemptCount"] = ToInt32(row["AttemptCount"]) + 1,
                    ["LastError"] = TrimError(error)
                };
                if (completed)
                    data["CompletedAt"] = DateTime.Now;
                context.Update(TableName, data, new[] { Equal("OperationKey", operationKey) });
                return 1;
            });
        }

        static IReadOnlyDictionary<string, object> SelectForUpdate(MysqlTransactionContext context, string operationKey)
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> rows = context.Select(new MysqlSelectCommand(
                TableName, Fields,
                new[] { Equal("OperationKey", operationKey) },
                Array.Empty<MysqlOrder>(), 1, null, MysqlLockMode.ForUpdate));
            return rows.Count == 0 ? null : rows[0];
        }

        static PromotionWalletRow Map(IReadOnlyDictionary<string, object> row)
        {
            return new PromotionWalletRow
            {
                OperationKey = Convert.ToString(row["OperationKey"], CultureInfo.InvariantCulture),
                UserUID = Convert.ToInt64(row["UserUID"], CultureInfo.InvariantCulture),
                BonusTaskId = Convert.ToString(row["BonusTaskId"], CultureInfo.InvariantCulture),
                Action = (byte)ToInt32(row["Action"]),
                Amount = Convert.ToDecimal(row["Amount"], CultureInfo.InvariantCulture),
                Sequence = ToInt32(row["Sequence"]),
                Status = (byte)ToInt32(row["Status"]),
                AttemptCount = ToInt32(row["AttemptCount"])
            };
        }

        static MysqlCondition Equal(string column, object value)
        {
            return new MysqlCondition(column, MysqlComparisonOperator.Equal, value);
        }

        static int ToInt32(object value)
        {
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        static string TrimError(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return value.Length <= 256 ? value : value.Substring(0, 256);
        }
    }
}
