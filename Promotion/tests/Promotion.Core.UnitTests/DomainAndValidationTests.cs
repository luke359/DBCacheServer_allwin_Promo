using System.Text.Json;
using Promotion.Core;
using Promotion.Core.Contracts;
using Promotion.Core.Domain;
using Promotion.Core.Validation;
using Xunit;

namespace Promotion.Core.UnitTests;

public sealed class DomainAndValidationTests
{
    [Theory]
    [InlineData(EligibilityStatus.Claimed)]
    [InlineData(EligibilityStatus.Excluded)]
    [InlineData(EligibilityStatus.Expired)]
    public void DOM_001_002_EligibilityTransitionsOnlyFromAvailable(EligibilityStatus target)
    {
        var changed = DomainInvariantValidator.TransitionEligibility(TestActivity.Entry(), target,
            TestActivity.Now.AddSeconds(1), target == EligibilityStatus.Expired ? null : TestActivity.TaskId);
        Assert.Equal(target, changed.Status);
        Assert.Equal(TestActivity.Now.AddSeconds(1), changed.StatusChangedAt);
        Assert.Throws<DomainInvariantException>(() =>
            DomainInvariantValidator.TransitionEligibility(changed, EligibilityStatus.Expired, TestActivity.Now.AddSeconds(2)));
    }

    [Fact]
    public void DOM_003_004_ClaimedAndExcludedIdentifiers()
    {
        var claimed = DomainInvariantValidator.TransitionEligibility(TestActivity.Entry(), EligibilityStatus.Claimed,
            TestActivity.Now, TestActivity.TaskId);
        Assert.Equal(TestActivity.TaskId, claimed.ClaimedBonusTaskId);
        Assert.Null(claimed.ExcludedByBonusTaskId);
        var excluded = DomainInvariantValidator.TransitionEligibility(TestActivity.Entry(), EligibilityStatus.Excluded,
            TestActivity.Now, TestActivity.TaskId);
        Assert.Null(excluded.ClaimedBonusTaskId);
        Assert.Equal(TestActivity.TaskId, excluded.ExcludedByBonusTaskId);
    }

    [Fact]
    public void DOM_005_006_007_BonusStatusValidationAndReset()
    {
        var active = TestActivity.Active();
        DomainInvariantValidator.ValidateBonusStatus(active);
        Assert.Throws<DomainInvariantException>(() => DomainInvariantValidator.ValidateBonusStatus(active with { TaskState = BonusTaskState.Closing }));
        var reset = DomainInvariantValidator.ResetBonusStatus(active, TestActivity.Now.AddMinutes(1));
        DomainInvariantValidator.ValidateBonusStatus(reset);
        Assert.Equal(BonusTaskState.None, reset.TaskState);
        Assert.Null(reset.BonusTaskId);
        Assert.Null(reset.ActivitySnapshotJson);
        Assert.Equal(0m, reset.BonusAmount);
        Assert.Equal(active.CreatedAt, reset.CreatedAt);
        Assert.Equal(active.LastFirstLoginBusinessDay, reset.LastFirstLoginBusinessDay);
        Assert.Equal(active.LastFirstDepositBusinessDay, reset.LastFirstDepositBusinessDay);
    }

    [Fact]
    public void DOM_008_009_SnapshotRoundTripAndUnsupportedVersion()
    {
        var snapshot = TestActivity.Snapshot();
        var json = ActivitySnapshotSerializer.Serialize(snapshot);
        using var document = JsonDocument.Parse(json);
        Assert.Equal(JsonValueKind.Number, document.RootElement.GetProperty("FixedBonusAmount").ValueKind);
        Assert.Equal(JsonValueKind.Number, document.RootElement.GetProperty("WagerContributionRate").ValueKind);
        Assert.Equal("2026-09-15 10:00:00.123456", document.RootElement.GetProperty("CapturedAt").GetString());
        Assert.Equal(snapshot, ActivitySnapshotSerializer.Deserialize(json));
        Assert.Equal(2, snapshot.SnapshotVersion);
        Assert.Equal(WagerCalculationType.BonusOnly, snapshot.WagerCalculationType);
        Assert.Throws<UnsupportedSnapshotVersionException>(() => ActivitySnapshotSerializer.Deserialize(
            json.Replace("\"SnapshotVersion\":2", "\"SnapshotVersion\":3", StringComparison.Ordinal)));
    }

    [Fact]
    public void DOM_010_014_SnapshotBytesRemainStableAndInvalidJsonFails()
    {
        var original = ActivitySnapshotSerializer.Serialize(TestActivity.Snapshot());
        var changedActivity = TestActivity.Create() with { FixedBonusAmount = 99 };
        var status = TestActivity.Active();
        DomainInvariantValidator.ValidateBonusStatus(status);
        Assert.Equal(original, status.ActivitySnapshotJson);
        Assert.NotEqual(original, ActivitySnapshotSerializer.Serialize(ActivitySnapshotFactory.Capture(changedActivity, TestActivity.Now)));
        Assert.Throws<DomainInvariantException>(() => ActivitySnapshotSerializer.Deserialize("{}"));
        Assert.Throws<DomainInvariantException>(() => DomainInvariantValidator.ValidateBonusStatus(
            status with { ActivitySnapshotJson = "not json" }));
    }

    [Fact]
    public void DOM_TriggerEventDepositShapeIsValidated()
    {
        var valid = new PromotionTriggerEvent("evt", 10001, TriggerType.Deposit, TestActivity.Now,
            new DateOnly(2026, 9, 15), 10m, TestActivity.Now);
        DomainInvariantValidator.ValidateTriggerEvent(valid);
        Assert.Throws<DomainInvariantException>(() => DomainInvariantValidator.ValidateTriggerEvent(
            valid with { EligibleDepositAmount = null }));
        Assert.Throws<DomainInvariantException>(() => DomainInvariantValidator.ValidateTriggerEvent(
            valid with { TriggerType = TriggerType.Free }));
    }

    [Fact]
    public void BD_014_InvalidActivityIsRejected()
    {
        Assert.Throws<DomainInvariantException>(() => DomainInvariantValidator.ValidateActivity(
            TestActivity.Create() with { WeekdayMask = "11111x1" }));
        Assert.Throws<DomainInvariantException>(() => DomainInvariantValidator.ValidateActivity(
            TestActivity.Create() with { EndDate = new DateOnly(2026, 8, 31) }));
        Assert.Throws<DomainInvariantException>(() => DomainInvariantValidator.ValidateActivity(
            TestActivity.Create() with { ActivityStatus = (ActivityStatus)3 }));
    }

    [Fact]
    public void NewActivityFieldsAndLegacySnapshotAreValidated()
    {
        var activity = TestActivity.Create() with
        {
            WagerCalculationType = WagerCalculationType.DepositAndBonus,
            MaxBetAmount = null,
            MaxBalanceConvertedAmount = null,
            GameServerList = "game-1,game-2"
        };
        DomainInvariantValidator.ValidateActivity(activity);
        var captured = ActivitySnapshotFactory.Capture(activity, TestActivity.Now);
        Assert.Equal(2, captured.SnapshotVersion);
        Assert.Null(captured.MaxBetAmount);
        Assert.Null(captured.MaxBalanceConvertedAmount);
        Assert.DoesNotContain("GameServerList", ActivitySnapshotSerializer.Serialize(captured));
        Assert.Equal(captured, ActivitySnapshotSerializer.Deserialize(ActivitySnapshotSerializer.Serialize(captured)));
        Assert.Throws<DomainInvariantException>(() => DomainInvariantValidator.ValidateActivity(
            activity with { GameServerList = new string('x', 501) }));
        Assert.Throws<DomainInvariantException>(() => DomainInvariantValidator.ValidateActivity(
            activity with { WagerCalculationType = (WagerCalculationType)3 }));

        var v1 = TestActivity.Snapshot() with { SnapshotVersion = 1 };
        var v1Json = ActivitySnapshotSerializer.Serialize(v1);
        Assert.DoesNotContain("WagerCalculationType", v1Json);
        Assert.Equal(v1, ActivitySnapshotSerializer.Deserialize(v1Json));
        Assert.Throws<DomainInvariantException>(() => ActivitySnapshotSerializer.Serialize(
            v1 with { MaxBetAmount = null }));
    }

    [Fact]
    public void GameServerListRequestIsValidated()
    {
        Assert.Equal(PromotionErrorCode.RequestNull,
            PromotionRequestValidator.Validate((GetGameServerListRequest?)null));
        Assert.Equal(PromotionErrorCode.InvalidIdentifier,
            PromotionRequestValidator.Validate(new GetGameServerListRequest(0)));
        Assert.Equal(PromotionErrorCode.None,
            PromotionRequestValidator.Validate(new GetGameServerListRequest(20001)));
    }

    [Fact]
    public void VAL_001_002_003_004_NullUserIdentifiersAndEnums()
    {
        Assert.Equal(PromotionErrorCode.RequestNull, PromotionRequestValidator.Validate((ClaimPromotionRequest?)null));
        Assert.Equal(PromotionErrorCode.InvalidUserUID, PromotionRequestValidator.Validate(new ClaimPromotionRequest(0, 1)));
        Assert.Equal(PromotionErrorCode.InvalidIdentifier, PromotionRequestValidator.Validate(new ClaimPromotionRequest(1, 0)));
        Assert.Equal(PromotionErrorCode.InvalidEnumValue, PromotionRequestValidator.Validate(new CreateEligibilityRequest(
            "evt", 1, (TriggerType)99, TestActivity.Now, null, Array.Empty<long>())));
    }

    [Fact]
    public void VAL_005_006_007_008_009_010_TimeAmountBatchScopeDepositAndHistory()
    {
        Assert.Equal(PromotionErrorCode.InvalidDateTime, PromotionRequestValidator.Validate(
            new GetAvailablePromotionsRequest(1, DateTime.SpecifyKind(TestActivity.Now, DateTimeKind.Utc))));
        Assert.Equal(PromotionErrorCode.InvalidAmount, PromotionRequestValidator.Validate(
            new AccumulateWagerRequest(1, TestActivity.TaskId, 1.00001m, 0, TestActivity.Now, 1)));
        Assert.Equal(PromotionErrorCode.InvalidAmount, PromotionRequestValidator.Validate(
            new CloseWagerCompletedBonusTaskRequest(1, TestActivity.TaskId, 1, -1, 0, TestActivity.Now)));
        Assert.Equal(PromotionErrorCode.InvalidPaginationOrBatchSize, PromotionRequestValidator.Validate(
            new DailyMaintenanceRequest(TestActivity.Now, 1001)));
        Assert.Equal(PromotionErrorCode.InvalidActivityScope, PromotionRequestValidator.Validate(
            new CreateEligibilityRequest("evt", 1, TriggerType.Deposit, TestActivity.Now, 10, new long[] { 0 })));
        Assert.Equal(PromotionErrorCode.InvalidEligibleDepositAmount, PromotionRequestValidator.Validate(
            new CreateEligibilityRequest("evt", 1, TriggerType.Deposit, TestActivity.Now, null, Array.Empty<long>())));
        Assert.Equal(PromotionErrorCode.InvalidHistoryRange, PromotionRequestValidator.Validate(
            new GetBonusHistoryRequest(1, TestActivity.Now.AddDays(-91), TestActivity.Now, 0, 10), TestActivity.Now));
    }

    [Fact]
    public void API_009_ResultFactoryUsesSpecifiedKindsAndRetryability()
    {
        var rejected = PromotionResultFactory.Failure<object>("corr", PromotionErrorCode.BonusTaskMismatch);
        Assert.Equal(PromotionResultKind.Rejected, rejected.Kind);
        Assert.False(rejected.IsRetryable);
        Assert.Null(rejected.Data);
        var permanent = PromotionResultFactory.Failure<object>("corr", PromotionErrorCode.ActivityNotFound);
        Assert.Equal(PromotionResultKind.PermanentFailure, permanent.Kind);
        var retryable = PromotionResultFactory.Failure<object>("corr", PromotionErrorCode.BalanceConvertFormulaFailed);
        Assert.Equal(PromotionResultKind.RetryableFailure, retryable.Kind);
        Assert.True(retryable.IsRetryable);
        var unknown = PromotionResultFactory.Failure<object>("corr", PromotionErrorCode.CommitOutcomeUnknown);
        Assert.Equal(PromotionResultKind.OutcomeUnknown, unknown.Kind);
        Assert.False(unknown.IsRetryable);
        var success = PromotionResultFactory.Success("corr", new object());
        Assert.Equal(PromotionErrorCode.None, success.ErrorCode);
        Assert.NotNull(success.Data);
    }
}
