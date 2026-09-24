namespace Promotion.Core.Contracts;

public interface IPromotionCoreService
{
    PromotionResult<InitializeData> Initialize(InitializeRequest request);
    PromotionResult<CreateEligibilityData> CreateEligibility(CreateEligibilityRequest request);
    PromotionResult<AvailablePromotionListData> GetAvailablePromotions(GetAvailablePromotionsRequest request);
    PromotionResult<GameServerListData> GetGameServerList(GetGameServerListRequest request);
    PromotionResult<ClaimPromotionData> ClaimPromotion(ClaimPromotionRequest request);
    PromotionResult<BonusTaskStatusData> GetBonusTaskStatus(GetBonusTaskStatusRequest request);
    PromotionResult<AccumulateWagerData> AccumulateWager(AccumulateWagerRequest request);
    PromotionResult<CloseBonusTaskData> CloseWagerCompletedBonusTask(CloseWagerCompletedBonusTaskRequest request);
    PromotionResult<CloseBonusTaskData> CloseDepletedBonusTask(CloseDepletedBonusTaskRequest request);
    PromotionResult<CloseBonusTaskData> AbandonBonusTask(AbandonBonusTaskRequest request);
    PromotionResult<DailyMaintenanceData> RunDailyMaintenance(DailyMaintenanceRequest request);
    PromotionResult<BonusHistoryPageData> GetBonusHistory(GetBonusHistoryRequest request);
}

public interface ILocalClock { DateTime GetNow(); }
