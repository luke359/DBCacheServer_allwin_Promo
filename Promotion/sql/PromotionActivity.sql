-- PromotionActivity table for H5gSet.
-- Select the target database before running this script.
-- CREATE TABLE IF NOT EXISTS only creates a missing table; it does not
-- validate or migrate an existing table with the same name.
-- Existing legacy tables: run PromotionActivity_Migration_20260916.sql once,
-- then apply database/migrations/V005__promotion_activity_status.sql.

CREATE TABLE IF NOT EXISTS `PromotionActivity` (
    `ActivityUID` BIGINT NOT NULL COMMENT '優惠活動唯一識別碼',
    `ActivityInfo` VARCHAR(100) NULL DEFAULT NULL COMMENT '優惠活動說明',
    `BonusType` TINYINT UNSIGNED NOT NULL DEFAULT 1 COMMENT '獎勵類型',
    `FixedBonusAmount` INT NOT NULL DEFAULT 10 COMMENT '固定獎勵金額',
    `MaxBonusAmount` INT NOT NULL DEFAULT 10 COMMENT '獎勵金額上限',
    `DepositPercentage` INT NOT NULL DEFAULT 30 COMMENT '存款獎勵百分比',
    `MinimumDepositAmount` INT NULL DEFAULT 10 COMMENT '最低存款金額',
    `WagerMultiplier` INT NOT NULL DEFAULT 30 COMMENT '流水倍數',
    `WagerCalculationType` TINYINT UNSIGNED NOT NULL DEFAULT 1 COMMENT '目標流水計算方式：1=優惠×洗碼量，2=(儲金+優惠)×洗碼量',
    `MaxBetAmount` INT NULL DEFAULT 5 COMMENT '單筆投注金額上限；NULL=無限制',
    `GameServerList` VARCHAR(500) NULL DEFAULT NULL COMMENT '優惠可玩遊戲列表；核心僅原樣提供 Host 查詢',
    `WagerContributionRate` DECIMAL(20,4) NOT NULL DEFAULT 100 COMMENT '有效流水貢獻比例',
    `ConvertType` TINYINT UNSIGNED NOT NULL DEFAULT 1 COMMENT '轉換類型',
    `FixedConvertedAmount` INT NOT NULL DEFAULT 5 COMMENT '固定轉換金額',
    `MaxBalanceConvertedAmount` INT NULL DEFAULT 150 COMMENT '餘額轉換金額上限；NULL=無限制',
    `DailyClaimLimit` INT NOT NULL DEFAULT 1 COMMENT '每日領取次數上限',
    `StartDate` DATE NULL DEFAULT NULL COMMENT '活動開始日期',
    `EndDate` DATE NULL DEFAULT NULL COMMENT '活動結束日期',
    `WeekdayMask` CHAR(7) NOT NULL DEFAULT '0000000' COMMENT '活動適用星期遮罩',
    `ActivityStatus` TINYINT UNSIGNED NOT NULL DEFAULT 1 COMMENT '活動狀態：1=啟用，2=封存',
    `TriggerType` TINYINT UNSIGNED NOT NULL DEFAULT 3 COMMENT '活動觸發類型',
    `IsNonStackable` TINYINT UNSIGNED NOT NULL DEFAULT 0 COMMENT '是否禁止與其他活動疊加',
    `ExclusiveGroup` VARCHAR(64) NULL DEFAULT NULL COMMENT '互斥活動群組',
    `CreatedAt` DATETIME(6) NOT NULL COMMENT '建立時間',
    `UpdatedAt` DATETIME(6) NOT NULL COMMENT '最後更新時間',
    PRIMARY KEY (`ActivityUID`),
    KEY `IX_PromotionActivity_TriggerType` (`TriggerType`),
    KEY `IX_PromotionActivity_ExclusiveGroup` (`ExclusiveGroup`)
) ENGINE=InnoDB
  DEFAULT CHARACTER SET=utf8mb4
  COLLATE=utf8mb4_bin
  COMMENT='優惠活動設定';
