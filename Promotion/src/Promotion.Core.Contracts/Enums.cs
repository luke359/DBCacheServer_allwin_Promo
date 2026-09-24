namespace Promotion.Core.Contracts;

public enum BonusType : byte { FixedAmount = 1, DepositPercentage = 2 }
public enum TriggerType : byte { Registration = 1, FirstLoginOfBusinessDay = 2, FirstDepositOfBusinessDay = 3, Deposit = 4, Free = 5 }
public enum ConvertType : byte { Fixed = 1, Balance = 2 }
public enum WagerCalculationType : byte { BonusOnly = 1, DepositAndBonus = 2 }
public enum ActivityStatus : byte { Active = 1, Archived = 2 }
public enum EligibilityStatus : byte { Available = 1, Claimed = 2, Excluded = 3, Expired = 4 }
public enum BonusTaskState : byte { None = 0, InProgress = 1, Closing = 2 }
public enum CloseReason : byte { WagerCompleted = 1, BonusDepleted = 2, PlayerAbandoned = 3, BusinessDayExpired = 4 }
public enum PromotionResultKind : byte { Succeeded = 1, Rejected = 2, RetryableFailure = 3, PermanentFailure = 4, OutcomeUnknown = 5, PartialSucceeded = 6 }
public enum WalletAction : byte { CreditBonusWallet = 1, ClearBonusWallet = 2, CreditMainWallet = 3 }
public enum BonusTaskOutcome : byte { InProgress = 1, Closed = 2, AlreadyClosed = 3, ReadyToClose = 4 }
public enum MaintenanceStage : byte { ExpireEligibility = 1, CloseExpiredTask = 2, DeleteHistory = 3 }

public enum PromotionErrorCode : int
{
    None = 0,
    ServiceNotInitialized = 1001, InitializationConflict = 1002, RequestNull = 1003,
    InvalidUserUID = 1004, InvalidIdentifier = 1005, InvalidEnumValue = 1006,
    InvalidDateTime = 1007, InvalidAmount = 1008, InvalidPaginationOrBatchSize = 1009,
    InvalidActivityScope = 1010, InvalidEligibleDepositAmount = 1011, UnsupportedSnapshotVersion = 1012,
    EventIdConflict = 2001,
    EligibilityNotFound = 3001, EligibilityPlayerMismatch = 3002, EligibilityExpired = 3003,
    EligibilityExcluded = 3004, EligibilityAlreadyClaimed = 3005, ActiveBonusTaskExists = 3006,
    DailyClaimLimitReached = 3007, ActivityNotFound = 3008, ClaimReplayDataUnavailable = 3009,
    BonusTaskNotFound = 4001, BonusTaskMismatch = 4002, InvalidBonusTaskState = 4003,
    BonusWalletNotDepleted = 4004, BalanceConvertFormulaFailed = 4005,
    WagerRequirementNotMet = 4006,
    MaintenancePartiallySucceeded = 5001, InvalidHistoryRange = 5002,
    DatabaseDeadlock = 8001, DatabaseLockWaitTimeout = 8002, DatabaseConnectionFailure = 8003,
    CommitOutcomeUnknown = 8004,
    DataCorruption = 9001, UnexpectedError = 9002, DataIntegrityConflict = 9003
}
