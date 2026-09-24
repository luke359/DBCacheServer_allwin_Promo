-- PCS-02 V001. Apply only through Invoke-PromotionMigration.ps1 to an empty, dedicated database.
-- MySQL DDL implicitly commits; a partial failure requires inspection and a reviewed forward repair.

CREATE TABLE `PromotionActivity` (
    `ActivityUID` BIGINT NOT NULL,
    `ActivityInfo` VARCHAR(100) NULL,
    `BonusType` TINYINT UNSIGNED NOT NULL,
    `FixedBonusAmount` INT NOT NULL,
    `MaxBonusAmount` INT NOT NULL,
    `DepositPercentage` INT NOT NULL,
    `MinimumDepositAmount` INT NULL,
    `WagerMultiplier` INT NOT NULL,
    `MaxBetAmount` INT NOT NULL,
    `WagerContributionRate` DECIMAL(20,4) NOT NULL,
    `ConvertType` TINYINT UNSIGNED NOT NULL,
    `FixedConvertedAmount` INT NOT NULL,
    `MaxBalanceConvertedAmount` INT NOT NULL,
    `DailyClaimLimit` INT NOT NULL,
    `StartDate` DATE NULL,
    `EndDate` DATE NULL,
    `WeekdayMask` CHAR(7) NOT NULL,
    `TriggerType` TINYINT UNSIGNED NOT NULL,
    `IsNonStackable` TINYINT UNSIGNED NOT NULL,
    `ExclusiveGroup` VARCHAR(64) NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    `UpdatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`ActivityUID`),
    KEY `IX_PromotionActivity_TriggerType` (`TriggerType`),
    KEY `IX_PromotionActivity_ExclusiveGroup` (`ExclusiveGroup`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;

CREATE TABLE `PromotionTriggerEvent` (
    `EventId` VARCHAR(128) NOT NULL,
    `UserUID` BIGINT NOT NULL,
    `TriggerType` TINYINT UNSIGNED NOT NULL,
    `EventTime` DATETIME(6) NOT NULL,
    `BusinessDay` DATE NOT NULL,
    `EligibleDepositAmount` DECIMAL(20,4) NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`EventId`),
    KEY `IX_PromotionTriggerEvent_UserUID_BusinessDay`
        (`UserUID`, `BusinessDay`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;

CREATE TABLE `EligibilityEntry` (
    `EligibilityEntryId` BIGINT NOT NULL AUTO_INCREMENT,
    `EventId` VARCHAR(128) NOT NULL,
    `UserUID` BIGINT NOT NULL,
    `ActivityUID` BIGINT NOT NULL,
    `BusinessDay` DATE NOT NULL,
    `TriggerType` TINYINT UNSIGNED NOT NULL,
    `EligibleDepositAmount` DECIMAL(20,4) NULL,
    `Status` TINYINT UNSIGNED NOT NULL,
    `ClaimedBonusTaskId` CHAR(36) NULL,
    `ExcludedByBonusTaskId` CHAR(36) NULL,
    `StatusChangedAt` DATETIME(6) NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`EligibilityEntryId`),
    UNIQUE KEY `UK_EligibilityEntry_EventId_ActivityUID` (`EventId`, `ActivityUID`),
    UNIQUE KEY `UK_EligibilityEntry_ClaimedBonusTaskId` (`ClaimedBonusTaskId`),
    KEY `IX_EligibilityEntry_UserUID_BusinessDay_Status` (`UserUID`, `BusinessDay`, `Status`),
    KEY `IX_EligibilityEntry_ClaimCount` (`UserUID`, `ActivityUID`, `BusinessDay`, `Status`),
    KEY `IX_EligibilityEntry_Status_BusinessDay` (`Status`, `BusinessDay`),
    CONSTRAINT `FK_EligibilityEntry_EventId`
        FOREIGN KEY (`EventId`) REFERENCES `PromotionTriggerEvent` (`EventId`),
    CONSTRAINT `FK_EligibilityEntry_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;

CREATE TABLE `BonusStatus` (
    `UserUID` BIGINT NOT NULL,
    `TaskState` TINYINT UNSIGNED NOT NULL,
    `BonusTaskId` CHAR(36) NULL,
    `EligibilityEntryId` BIGINT NULL,
    `ActivityUID` BIGINT NULL,
    `BusinessDay` DATE NULL,
    `ActivitySnapshotJson` TEXT NULL,
    `BonusAmount` DECIMAL(20,4) NOT NULL,
    `RequiredWagerAmount` DECIMAL(20,4) NOT NULL,
    `CurrentWagerAmount` DECIMAL(20,4) NOT NULL,
    `ClaimedAt` DATETIME(6) NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    `UpdatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`UserUID`),
    UNIQUE KEY `UK_BonusStatus_BonusTaskId` (`BonusTaskId`),
    KEY `IX_BonusStatus_TaskState_BusinessDay` (`TaskState`, `BusinessDay`),
    CONSTRAINT `FK_BonusStatus_EligibilityEntryId`
        FOREIGN KEY (`EligibilityEntryId`) REFERENCES `EligibilityEntry` (`EligibilityEntryId`),
    CONSTRAINT `FK_BonusStatus_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;

CREATE TABLE `BonusHistory` (
    `BonusHistoryId` BIGINT NOT NULL AUTO_INCREMENT,
    `BonusTaskId` CHAR(36) NOT NULL,
    `UserUID` BIGINT NOT NULL,
    `EligibilityEntryId` BIGINT NOT NULL,
    `ActivityUID` BIGINT NOT NULL,
    `BusinessDay` DATE NOT NULL,
    `ActivitySnapshotJson` TEXT NOT NULL,
    `BonusAmount` DECIMAL(20,4) NOT NULL,
    `RequiredWagerAmount` DECIMAL(20,4) NOT NULL,
    `CurrentWagerAmount` DECIMAL(20,4) NOT NULL,
    `ConvertedAmount` DECIMAL(20,4) NOT NULL,
    `CloseReason` TINYINT UNSIGNED NOT NULL,
    `ClaimedAt` DATETIME(6) NOT NULL,
    `ClosedAt` DATETIME(6) NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`BonusHistoryId`),
    UNIQUE KEY `UK_BonusHistory_BonusTaskId` (`BonusTaskId`),
    KEY `IX_BonusHistory_UserUID_ClosedAt` (`UserUID`, `ClosedAt`),
    KEY `IX_BonusHistory_ClosedAt` (`ClosedAt`),
    CONSTRAINT `FK_BonusHistory_EligibilityEntryId`
        FOREIGN KEY (`EligibilityEntryId`) REFERENCES `EligibilityEntry` (`EligibilityEntryId`),
    CONSTRAINT `FK_BonusHistory_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;
