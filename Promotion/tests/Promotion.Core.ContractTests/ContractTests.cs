using Promotion.Core.Contracts;
using Xunit;

namespace Promotion.Core.ContractTests;

public sealed class ContractTests
{
    [Fact]
    public void API_009_PublicEnumNumbersRemainStable()
    {
        Assert.Equal((byte)0, (byte)BonusTaskState.None);
        Assert.Equal((byte)5, (byte)TriggerType.Free);
        Assert.Equal((byte)4, (byte)CloseReason.BusinessDayExpired);
        Assert.Equal((byte)6, (byte)PromotionResultKind.PartialSucceeded);
        Assert.Equal(8004, (int)PromotionErrorCode.CommitOutcomeUnknown);
        Assert.Equal(9003, (int)PromotionErrorCode.DataIntegrityConflict);
        Assert.Equal((byte)1, (byte)WagerCalculationType.BonusOnly);
        Assert.Equal((byte)2, (byte)WagerCalculationType.DepositAndBonus);
        Assert.Equal((byte)4, (byte)BonusTaskOutcome.ReadyToClose);
        Assert.Equal(4006, (int)PromotionErrorCode.WagerRequirementNotMet);
    }

    [Fact]
    public void API_ContractHasExactlyTwelveSynchronousMethods()
    {
        var methods = typeof(IPromotionCoreService).GetMethods();
        Assert.Equal(12, methods.Length);
        Assert.Contains(methods, method => method.Name == nameof(IPromotionCoreService.GetGameServerList));
        Assert.Contains(methods, method => method.Name == nameof(IPromotionCoreService.CloseWagerCompletedBonusTask));
        Assert.All(methods, method => Assert.False(typeof(Task).IsAssignableFrom(method.ReturnType)));
    }
}
