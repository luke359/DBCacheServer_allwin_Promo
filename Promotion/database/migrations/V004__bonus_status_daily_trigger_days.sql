-- V004: persist each player's most recent daily-first trigger day.
-- Daily maintenance does not clear these values; a different BusinessDay makes the next trigger eligible.
ALTER TABLE `PromotionBonusStatus`
    ADD COLUMN `LastFirstLoginBusinessDay` DATE NULL DEFAULT NULL
        COMMENT '最近一次每日首登入所屬活動日'
        AFTER `BusinessDay`,
    ADD COLUMN `LastFirstDepositBusinessDay` DATE NULL DEFAULT NULL
        COMMENT '最近一次每日首儲所屬活動日'
        AFTER `LastFirstLoginBusinessDay`;
