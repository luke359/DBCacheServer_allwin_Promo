-- DBCache 優惠錢包操作紀錄。這不是核心服務的五張表。
-- 先選好 DBCache 使用的資料庫再執行。
-- CREATE TABLE IF NOT EXISTS 只會在表不存在時建立，不會修改同名舊表。

CREATE TABLE IF NOT EXISTS `PromotionWalletOperation` (
    `OperationKey` VARCHAR(160) NOT NULL COMMENT '冪等鍵，格式為 BonusTaskId:WalletAction',
    `UserUID` BIGINT NOT NULL COMMENT '玩家唯一識別碼',
    `BonusTaskId` VARCHAR(64) NOT NULL COMMENT '核心產生的任務識別碼',
    `Action` TINYINT UNSIGNED NOT NULL COMMENT '1=CreditBonusWallet 2=ClearBonusWallet 3=CreditMainWallet',
    `Amount` DECIMAL(20,4) NOT NULL COMMENT '入帳金額；ClearBonusWallet 固定為 0',
    `Sequence` INT NOT NULL COMMENT '同一批指令的順序，數字小的先執行',
    `Status` TINYINT UNSIGNED NOT NULL COMMENT '0=尚未入帳 1=已入帳 2=入帳失敗',
    `AttemptCount` INT NOT NULL COMMENT '已嘗試入帳次數',
    `LastError` VARCHAR(256) NULL COMMENT '最後一次失敗原因',
    `CreatedAt` DATETIME(6) NOT NULL COMMENT '指令保存時間',
    `CompletedAt` DATETIME(6) NULL COMMENT '入帳完成時間；尚未完成時為 NULL',
    PRIMARY KEY (`OperationKey`),
    KEY `IX_PromotionWalletOperation_BonusTaskId_Sequence` (`BonusTaskId`, `Sequence`),
    KEY `IX_PromotionWalletOperation_Status_CompletedAt` (`Status`, `CompletedAt`)
) ENGINE=InnoDB
  DEFAULT CHARACTER SET=utf8mb4
  COLLATE=utf8mb4_bin
  COMMENT='優惠錢包操作紀錄';
