using System;
using System.Collections.Generic;

namespace DBCacheServer
{
    /// <summary>以玩家與 userGameData.SerialNumber 占用一局。列存在表示不可再送進核心。</summary>
    internal sealed class PromotionWagerRoundStore
    {
        internal const string TableName = "PromotionWagerRound";

        readonly MysqlAcess mysql;

        internal PromotionWagerRoundStore(MysqlAcess mysql)
        {
            this.mysql = mysql ?? throw new ArgumentNullException(nameof(mysql));
        }

        /// <summary>第一次占用回傳 true。已有相同玩家與流水號回傳 false。</summary>
        internal bool TryClaim(int userUid, long serialNumber)
        {
            if (userUid <= 0 || serialNumber <= 0)
                return false;

            try
            {
                mysql.ExecuteParameterizedTransaction(context =>
                {
                    context.Insert(TableName, new Dictionary<string, object>
                    {
                        ["UserUID"] = userUid,
                        ["SerialNumber"] = serialNumber,
                        ["CreatedAt"] = DateTime.Now
                    });
                    return 1;
                });
                return true;
            }
            catch (PromotionDataException ex) when (ex.Kind == PromotionDataErrorKind.DuplicateKey)
            {
                return false;
            }
        }

        /// <summary>核心確認這次沒有提交時才刪除，讓同一局可以再送一次。</summary>
        internal void Release(int userUid, long serialNumber)
        {
            if (userUid <= 0 || serialNumber <= 0)
                return;

            mysql.DeleteParameterizedV2(new MysqlDeleteCommand(
                TableName,
                new[]
                {
                    new MysqlCondition("UserUID", MysqlComparisonOperator.Equal, userUid),
                    new MysqlCondition("SerialNumber", MysqlComparisonOperator.Equal, serialNumber)
                },
                Array.Empty<MysqlOrder>(),
                1));
        }
    }
}
