-- V002: preserve existing limits; NULL now means unlimited. DDL is not transactional.
ALTER TABLE `PromotionActivity`
    ADD COLUMN `WagerCalculationType` TINYINT UNSIGNED NOT NULL DEFAULT 1
        COMMENT '1=bonus x multiplier, 2=(deposit+bonus) x multiplier'
        AFTER `WagerMultiplier`,
    MODIFY COLUMN `MaxBetAmount` INT NULL DEFAULT 5
        COMMENT 'NULL=unlimited',
    ADD COLUMN `GameServerList` VARCHAR(500) NULL DEFAULT NULL
        COMMENT 'Host-readable game list; core does not parse'
        AFTER `MaxBetAmount`,
    MODIFY COLUMN `MaxBalanceConvertedAmount` INT NULL DEFAULT 150
        COMMENT 'NULL=unlimited';
