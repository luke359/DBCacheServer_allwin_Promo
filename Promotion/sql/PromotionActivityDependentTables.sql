-- Dependent tables for the Promotion Activity core service.
-- Select the target database and create PromotionActivity before running this script.
-- Tables are ordered by foreign-key dependency.
-- CREATE TABLE IF NOT EXISTS only creates a missing table; it does not
-- validate or migrate an existing table with the same name.

CREATE TABLE IF NOT EXISTS `PromotionTriggerEvent` (
    `EventId` VARCHAR(128) NOT NULL COMMENT '觸發事件唯一識別碼',
    `UserUID` BIGINT NOT NULL COMMENT '使用者唯一識別碼',
    `TriggerType` TINYINT UNSIGNED NOT NULL COMMENT '活動觸發類型',
    `EventTime` DATETIME(6) NOT NULL COMMENT '事件發生時間',
    `BusinessDay` DATE NOT NULL COMMENT '事件所屬營業日',
    `EligibleDepositAmount` DECIMAL(20,4) NULL COMMENT '符合資格的存款金額',
    `CreatedAt` DATETIME(6) NOT NULL COMMENT '建立時間',
    PRIMARY KEY (`EventId`),
    KEY `IX_PromotionTriggerEvent_UserUID_BusinessDay`
        (`UserUID`, `BusinessDay`)
) ENGINE=InnoDB
  DEFAULT CHARACTER SET=utf8mb4
  COLLATE=utf8mb4_bin
  COMMENT='優惠活動觸發事件';

CREATE TABLE IF NOT EXISTS `EligibilityEntry` (
    `EligibilityEntryId` BIGINT NOT NULL AUTO_INCREMENT COMMENT '活動資格紀錄唯一識別碼',
    `EventId` VARCHAR(128) NOT NULL COMMENT '觸發事件唯一識別碼',
    `UserUID` BIGINT NOT NULL COMMENT '使用者唯一識別碼',
    `ActivityUID` BIGINT NOT NULL COMMENT '優惠活動唯一識別碼',
    `BusinessDay` DATE NOT NULL COMMENT '資格所屬營業日',
    `TriggerType` TINYINT UNSIGNED NOT NULL COMMENT '活動觸發類型',
    `EligibleDepositAmount` DECIMAL(20,4) NULL COMMENT '符合資格的存款金額',
    `Status` TINYINT UNSIGNED NOT NULL COMMENT '資格狀態',
    `ClaimedBonusTaskId` CHAR(36) NULL COMMENT '已領取的獎勵任務識別碼',
    `ExcludedByBonusTaskId` CHAR(36) NULL COMMENT '造成資格排除的獎勵任務識別碼',
    `StatusChangedAt` DATETIME(6) NOT NULL COMMENT '資格狀態最後變更時間',
    `CreatedAt` DATETIME(6) NOT NULL COMMENT '建立時間',
    PRIMARY KEY (`EligibilityEntryId`),
    UNIQUE KEY `UK_EligibilityEntry_EventId_ActivityUID`
        (`EventId`, `ActivityUID`),
    UNIQUE KEY `UK_EligibilityEntry_ClaimedBonusTaskId`
        (`ClaimedBonusTaskId`),
    KEY `IX_EligibilityEntry_UserUID_BusinessDay_Status`
        (`UserUID`, `BusinessDay`, `Status`),
    KEY `IX_EligibilityEntry_ClaimCount`
        (`UserUID`, `ActivityUID`, `BusinessDay`, `Status`),
    KEY `IX_EligibilityEntry_Status_BusinessDay`
        (`Status`, `BusinessDay`),
    CONSTRAINT `FK_EligibilityEntry_EventId`
        FOREIGN KEY (`EventId`) REFERENCES `PromotionTriggerEvent` (`EventId`),
    CONSTRAINT `FK_EligibilityEntry_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`)
) ENGINE=InnoDB
  DEFAULT CHARACTER SET=utf8mb4
  COLLATE=utf8mb4_bin
  COMMENT='優惠活動資格紀錄';

CREATE TABLE IF NOT EXISTS `PromotionBonusStatus` (
    `UserUID` BIGINT NOT NULL COMMENT '使用者唯一識別碼',
    `TaskState` TINYINT UNSIGNED NOT NULL COMMENT '獎勵任務狀態',
    `BonusTaskId` CHAR(36) NULL COMMENT '獎勵任務唯一識別碼',
    `EligibilityEntryId` BIGINT NULL COMMENT '活動資格紀錄唯一識別碼',
    `ActivityUID` BIGINT NULL COMMENT '優惠活動唯一識別碼',
    `BusinessDay` DATE NULL COMMENT '獎勵所屬營業日',
    `LastFirstLoginBusinessDay` DATE NULL COMMENT '最近一次每日首登入所屬活動日',
    `LastFirstDepositBusinessDay` DATE NULL COMMENT '最近一次每日首儲所屬活動日',
    `ActivitySnapshotJson` TEXT NULL COMMENT '領取時的活動設定快照 JSON',
    `BonusAmount` DECIMAL(20,4) NOT NULL COMMENT '獎勵金額',
    `RequiredWagerAmount` DECIMAL(20,4) NOT NULL COMMENT '要求完成的流水金額',
    `CurrentWagerAmount` DECIMAL(20,4) NOT NULL COMMENT '目前累計的有效流水金額',
    `ClaimedAt` DATETIME(6) NULL COMMENT '獎勵領取時間',
    `CreatedAt` DATETIME(6) NOT NULL COMMENT '建立時間',
    `UpdatedAt` DATETIME(6) NOT NULL COMMENT '最後更新時間',
    PRIMARY KEY (`UserUID`),
    UNIQUE KEY `UK_PromotionBonusStatus_BonusTaskId` (`BonusTaskId`),
    KEY `IX_PromotionBonusStatus_TaskState_BusinessDay`
        (`TaskState`, `BusinessDay`),
    CONSTRAINT `FK_PromotionBonusStatus_EligibilityEntryId`
        FOREIGN KEY (`EligibilityEntryId`) REFERENCES `EligibilityEntry` (`EligibilityEntryId`),
    CONSTRAINT `FK_PromotionBonusStatus_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`)
) ENGINE=InnoDB
  DEFAULT CHARACTER SET=utf8mb4
  COLLATE=utf8mb4_bin
  COMMENT='使用者目前的獎勵任務狀態';

CREATE TABLE IF NOT EXISTS `PromotionBonusHistory` (
    `BonusHistoryId` BIGINT NOT NULL AUTO_INCREMENT COMMENT '獎勵歷程唯一識別碼',
    `BonusTaskId` CHAR(36) NOT NULL COMMENT '獎勵任務唯一識別碼',
    `UserUID` BIGINT NOT NULL COMMENT '使用者唯一識別碼',
    `EligibilityEntryId` BIGINT NOT NULL COMMENT '活動資格紀錄唯一識別碼',
    `ActivityUID` BIGINT NOT NULL COMMENT '優惠活動唯一識別碼',
    `BusinessDay` DATE NOT NULL COMMENT '獎勵所屬營業日',
    `ActivitySnapshotJson` TEXT NOT NULL COMMENT '領取時的活動設定快照 JSON',
    `BonusAmount` DECIMAL(20,4) NOT NULL COMMENT '獎勵金額',
    `RequiredWagerAmount` DECIMAL(20,4) NOT NULL COMMENT '要求完成的流水金額',
    `CurrentWagerAmount` DECIMAL(20,4) NOT NULL COMMENT '結束時累計的有效流水金額',
    `ConvertedAmount` DECIMAL(20,4) NOT NULL COMMENT '實際轉換金額',
    `CloseReason` TINYINT UNSIGNED NOT NULL COMMENT '獎勵任務結束原因',
    `ClaimedAt` DATETIME(6) NOT NULL COMMENT '獎勵領取時間',
    `ClosedAt` DATETIME(6) NOT NULL COMMENT '獎勵任務結束時間',
    `CreatedAt` DATETIME(6) NOT NULL COMMENT '建立時間',
    PRIMARY KEY (`BonusHistoryId`),
    UNIQUE KEY `UK_PromotionBonusHistory_BonusTaskId` (`BonusTaskId`),
    KEY `IX_PromotionBonusHistory_UserUID_ClosedAt`
        (`UserUID`, `ClosedAt`),
    KEY `IX_PromotionBonusHistory_ClosedAt` (`ClosedAt`),
    CONSTRAINT `FK_PromotionBonusHistory_EligibilityEntryId`
        FOREIGN KEY (`EligibilityEntryId`) REFERENCES `EligibilityEntry` (`EligibilityEntryId`),
    CONSTRAINT `FK_PromotionBonusHistory_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`)
) ENGINE=InnoDB
  DEFAULT CHARACTER SET=utf8mb4
  COLLATE=utf8mb4_bin
  COMMENT='已結束的獎勵任務歷程';
