using Promotion.Core.Contracts;
using Promotion.Core.Data;
using Promotion.Core.Domain;
using Promotion.Core.Validation;

namespace Promotion.Core;

public sealed partial class PromotionCoreService
{
    public PromotionResult<GetPlayerPromotionPageData> GetPlayerPromotionPage(GetPlayerPromotionPageRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<GetPlayerPromotionPageData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        var businessDay = BusinessDay(request.QueryTime);
        var activityUids = request.VisibleActivityUIDs.Distinct().ToArray();

        return Read(correlation, () =>
        {
            var status = data.GetBonusStatus(request.UserUID);
            var activeTask = ActiveTask(status);
            var activities = activityUids.Length == 0
                ? Array.Empty<PromotionActivity>()
                : data.GetActivitiesByIds(activityUids)
                    .Where(activity => activity.ActivityStatus == ActivityStatus.Active)
                    .OrderBy(activity => activity.ActivityUID)
                    .ToArray();
            var eligibilityByActivity = activityUids.Length == 0
                ? new Dictionary<long, EligibilityEntry>()
                : data.GetAvailableEligibilityEntries(request.UserUID, businessDay, activityUids)
                    .Where(entry => activityUids.Contains(entry.ActivityUID))
                    .GroupBy(entry => entry.ActivityUID)
                    .ToDictionary(group => group.Key, group => group.MaxBy(entry => entry.EligibilityEntryId)!);

            var items = activities.Select(activity =>
            {
                eligibilityByActivity.TryGetValue(activity.ActivityUID, out var eligibility);
                var hasActiveTask = activeTask?.ActivityUID == activity.ActivityUID;
                var canClaim = eligibility is not null;
                var progress = hasActiveTask
                    ? new BonusTaskProgressDto(activeTask!.CurrentWagerAmount, activeTask.RequiredWagerAmount,
                        activeTask.RemainingWagerAmount)
                    : null;
                return new PlayerPromotionActivityDto(activity.ActivityUID,
                    ActivitySnapshotFactory.Capture(activity, request.QueryTime), eligibility?.EligibilityEntryId,
                    canClaim, hasActiveTask, canClaim && activeTask is null, progress);
            });
            var history = request.IncludeTodayCompleted
                ? Freeze(data.GetBonusHistoryByBusinessDay(request.UserUID, businessDay).Select(HistoryDto))
                : null;
            return new GetPlayerPromotionPageData(request.UserUID, businessDay, activeTask is not null,
                activeTask, Freeze(items), history);
        });
    }

    private static ActiveBonusTaskDto? ActiveTask(BonusStatus? status)
    {
        if (status is null || status.TaskState == BonusTaskState.None) return null;
        if (status.TaskState != BonusTaskState.InProgress)
            throw new PromotionDataException(PromotionDataErrorKind.InvalidBonusTaskState, "Invalid bonus task state.");
        var snapshot = ActivitySnapshotSerializer.Deserialize(status.ActivitySnapshotJson!);
        return new ActiveBonusTaskDto(status.BonusTaskId!, status.EligibilityEntryId!.Value,
            status.ActivityUID!.Value, status.BusinessDay!.Value, status.BonusAmount,
            status.RequiredWagerAmount, status.CurrentWagerAmount,
            PromotionAmountCalculator.RemainingWager(status.RequiredWagerAmount, status.CurrentWagerAmount),
            snapshot.MaxBetAmount, status.ClaimedAt!.Value, snapshot);
    }
}
