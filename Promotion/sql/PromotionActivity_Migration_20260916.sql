-- 對已存在的 PromotionActivity 套用本次欄位變更。
-- 執行前請先選擇目標資料庫並完成備份；此腳本只執行一次。
-- 現有上限數值原樣保留；新增資料的預設值分別為 5 與 150。

ALTER TABLE `PromotionActivity`
    ADD COLUMN `WagerCalculationType` TINYINT UNSIGNED NOT NULL DEFAULT 1
        COMMENT '目標流水計算方式：1=優惠×洗碼量，2=(儲金+優惠)×洗碼量'
        AFTER `WagerMultiplier`,
    MODIFY COLUMN `MaxBetAmount` INT NULL DEFAULT 5
        COMMENT '單筆投注金額上限；NULL=無限制',
    ADD COLUMN `GameServerList` VARCHAR(500) NULL DEFAULT NULL
        COMMENT '優惠可玩遊戲列表；核心僅原樣提供 Host 查詢'
        AFTER `MaxBetAmount`,
    MODIFY COLUMN `MaxBalanceConvertedAmount` INT NULL DEFAULT 150
        COMMENT '餘額轉換金額上限；NULL=無限制';
