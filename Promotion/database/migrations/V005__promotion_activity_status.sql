-- V005: add the activity lifecycle status used to archive activities.
-- Existing rows remain active. WeekdayMask='0000000' continues to mean disabled.
ALTER TABLE `PromotionActivity`
    ADD COLUMN `ActivityStatus` TINYINT UNSIGNED NOT NULL DEFAULT 1
        COMMENT '活動狀態：1=啟用，2=封存'
        AFTER `WeekdayMask`;
