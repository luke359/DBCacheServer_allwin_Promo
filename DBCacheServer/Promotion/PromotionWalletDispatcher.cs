using System;
using System.Collections.Generic;
using Promotion.Core.Contracts;

namespace DBCacheServer
{
    internal sealed class PromotionWalletExecutionResult
    {
        public bool Saved = true;
        public bool AllSucceeded = true;
        public string ErrorCode = "";
        public string Message = "";
    }

    /// <summary>先把錢包指令寫入 PromotionWalletOperation，再依序入帳。</summary>
    internal static class PromotionWalletDispatcher
    {
        const string FailedCode = "WalletInstructionFailed";
        static CacheManeger cache;

        internal static void Bind(CacheManeger value)
        {
            cache = value;
        }

        internal static PromotionWalletExecutionResult Execute(IReadOnlyList<WalletInstructionDto> instructions)
        {
            if (instructions == null || instructions.Count == 0)
                return new PromotionWalletExecutionResult();

            PromotionWalletExecutionResult result = new PromotionWalletExecutionResult();
            try
            {
                PromotionWalletStore store = new PromotionWalletStore(MysqlAcess.GetInstance());
                List<WalletInstructionDto> saved = new List<WalletInstructionDto>();
                Dictionary<string, int> sequenceByTask = new Dictionary<string, int>();
                for (int i = 0; i < instructions.Count; i++)
                {
                    WalletInstructionDto instruction = instructions[i];
                    if (!PromotionWalletStore.CanSave(instruction))
                    {
                        result.Saved = false;
                        result.AllSucceeded = false;
                        result.ErrorCode = FailedCode;
                        result.Message = "錢包指令無效";
                        Log(instruction, "無法保存");
                        continue;
                    }

                    int sequence = 0;
                    if (sequenceByTask.TryGetValue(instruction.BonusTaskId, out int next))
                        sequence = next;
                    sequenceByTask[instruction.BonusTaskId] = sequence + 1;
                    try
                    {
                        store.Save(instruction, sequence);
                        saved.Add(instruction);
                    }
                    catch (Exception ex)
                    {
                        result.Saved = false;
                        result.AllSucceeded = false;
                        result.ErrorCode = FailedCode;
                        result.Message = "錢包指令保存失敗";
                        MyConsole.WriteLine(
                            "Promotion 錢包指令保存失敗：OperationKey=" + instruction.OperationKey
                            + " " + ex.Message);
                    }
                }

                if (!result.Saved)
                    return result;

                string applyError = Apply(store, saved);
                if (applyError != null)
                {
                    result.AllSucceeded = false;
                    result.ErrorCode = FailedCode;
                    if (string.IsNullOrEmpty(result.Message))
                        result.Message = applyError;
                }
                return result;
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion 錢包指令處理失敗：" + ex.Message);
                return new PromotionWalletExecutionResult
                {
                    Saved = false,
                    AllSucceeded = false,
                    ErrorCode = FailedCode,
                    Message = "錢包指令保存失敗"
                };
            }
        }

        internal static void RetryIncomplete()
        {
            try
            {
                if (cache == null)
                    return;
                PromotionWalletStore store = new PromotionWalletStore(MysqlAcess.GetInstance());
                Apply(store, store.ListRetryable());
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion 錢包指令重試失敗：" + ex.Message);
            }
        }

        internal static void DeleteExpiredCompleted()
        {
            try
            {
                new PromotionWalletStore(MysqlAcess.GetInstance()).DeleteExpiredCompleted();
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion 錢包操作紀錄清理失敗：" + ex.Message);
            }
        }

        static string Apply(PromotionWalletStore store, IReadOnlyList<WalletInstructionDto> instructions)
        {
            if (cache == null)
                return "優惠錢包尚未就緒";

            string firstError = null;
            for (int i = 0; i < instructions.Count; i++)
                firstError = ApplyOne(store, instructions[i], store.Get(instructions[i].OperationKey), firstError);
            return firstError;
        }

        static string Apply(PromotionWalletStore store, IReadOnlyList<PromotionWalletRow> rows)
        {
            if (cache == null)
                return "優惠錢包尚未就緒";

            string firstError = null;
            for (int i = 0; i < rows.Count; i++)
            {
                PromotionWalletRow row = rows[i];
                if (row.Status == PromotionWalletStore.StatusCompleted)
                    continue;
                if (!Enum.IsDefined(typeof(WalletAction), row.Action))
                {
                    store.MarkFailed(row.OperationKey, "未知的錢包動作");
                    if (firstError == null)
                        firstError = "未知的錢包動作";
                    continue;
                }

                WalletInstructionDto instruction = new WalletInstructionDto(
                    row.OperationKey, (WalletAction)row.Action, row.UserUID, row.BonusTaskId, row.Amount);
                firstError = ApplyOne(store, instruction, row, firstError);
            }
            return firstError;
        }

        static string ApplyOne(PromotionWalletStore store, WalletInstructionDto instruction,
            PromotionWalletRow row, string firstError)
        {
            if (row == null || row.Status == PromotionWalletStore.StatusCompleted)
                return firstError;
            if (store.HasEarlierIncomplete(row.BonusTaskId, row.Sequence))
                return firstError;
            if (!cache.TryApplyPromotionWallet(instruction, out string error))
            {
                store.MarkFailed(instruction.OperationKey, error);
                Log(instruction, error);
                return firstError ?? (string.IsNullOrEmpty(error) ? "錢包入帳失敗" : error);
            }

            store.MarkCompleted(instruction.OperationKey);
            return firstError;
        }

        static void Log(WalletInstructionDto instruction, string message)
        {
            string key = instruction == null ? "" : instruction.OperationKey;
            MyConsole.WriteLine("Promotion 錢包指令未入帳：OperationKey=" + key + " " + message);
        }
    }
}
