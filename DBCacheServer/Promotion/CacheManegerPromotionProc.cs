using Promotion.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCacheServer
{
    public partial class CacheManeger
    {

        /// <summary>優惠活動 累計流水</summary>
        void PromotionAccumulateWager(UserData user, double totBet, double totWin)
        {
            if (!PromotionCoreHost.IsReady || PromotionCoreHost.Service == null || user == null)
            {
                return;
            }

            //優惠活動 累計流水 #260922
            //UNDONE: accumulateWagerResult 供後續處理錢包指令
            var accResult = PromotionCoreHost.TryAccumulateWager(user.UserUID, totBet, totWin, user.Balance2);
            if (accResult != null)
            {
                //TODO: 處理錢包指令
                switch (accResult.Kind)
                {
                    case PromotionResultKind.Succeeded:
                        {
                            var data = accResult.Data!;
                            if (data.Outcome == BonusTaskOutcome.InProgress)
                            {
                                // 顯示或保存 CurrentWagerAmount、RemainingWagerAmount


                            }
                            else
                            {
                                // Closed 或 AlreadyClosed：
                                // 依 CloseReason、ConvertedAmount、ClosedAt 更新任務狀態



                            }
                        }
                        break;

                    case PromotionResultKind.RetryableFailure:
                        break;

                    case PromotionResultKind.OutcomeUnknown:
                        // 絕對不可直接重送本局。
                        // 凍結相關後續流程、保留原始局資料並建立對帳案件。
                        break;

                    case PromotionResultKind.Rejected:
                    case PromotionResultKind.PermanentFailure:
                        // 記錄 ErrorCode、CorrelationId，依錯誤碼處理或告警
                        break;
                    default:
                        break;
                }
            }
        }


    }
}
