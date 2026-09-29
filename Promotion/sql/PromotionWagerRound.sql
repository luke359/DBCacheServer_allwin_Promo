-- DBCache 優惠遊戲局去重。這不是核心服務的五張表。
-- 先選好 DBCache 使用的資料庫再執行。
-- CREATE TABLE IF NOT EXISTS 只會在表不存在時建立，不會修改同名舊表。
-- 同一玩家的 SerialNumber 只允許送進 AccumulateWager 一次。列不自動刪除。

CREATE TABLE IF NOT EXISTS `PromotionWagerRound` (
    `UserUID` BIGINT NOT NULL COMMENT '玩家唯一識別碼',
    `SerialNumber` BIGINT NOT NULL COMMENT '該筆 userGameData 的玩家流水號',
    `CreatedAt` DATETIME(6) NOT NULL COMMENT '第一次占用此局的時間',
    PRIMARY KEY (`UserUID`, `SerialNumber`)
) ENGINE=InnoDB
  DEFAULT CHARACTER SET=utf8mb4
  COLLATE=utf8mb4_bin
  COMMENT='優惠遊戲局去重';
