DROP TABLE IF EXISTS `UserSessionRewardRecord`;
CREATE TABLE IF NOT EXISTS `UserSessionRewardRecord`
(
    `RewardRecordId`     BIGINT       NOT NULL AUTO_INCREMENT COMMENT '紀錄流水號',
    `UserUID`            INT          NOT NULL COMMENT '玩家 UID',
    `KeyInAmount`        DOUBLE       NOT NULL COMMENT '建立 Session 時的開分值',
    `BalanceAfterKeyIn`  DOUBLE       NOT NULL COMMENT '完成本次開分後的玩家餘額',
    `KeyOutAmount`       VARCHAR(100) NULL DEFAULT NULL COMMENT '此筆紀錄的洗分值，若多次洗分則以'',''分隔接在後面',
    `BalanceWhenKeyOut`  VARCHAR(100) NULL DEFAULT NULL COMMENT '此筆紀錄洗分當下（未扣除洗分值）的餘額，若多次洗分則以'',''分隔接在後面',
    `ExtraInfo`          VARCHAR(500) NULL DEFAULT NULL COMMENT '額外資訊，DB 不須解包此字串',
    `TargetBalance`      DOUBLE       NOT NULL COMMENT '本次功能的目標餘額',
    `RecordStatus`       TINYINT      NOT NULL DEFAULT 0 COMMENT '0=進行中, 1=再開分結束',
    `RewardEndStatus`    TINYINT      NOT NULL DEFAULT 0 COMMENT '0=功能進行中, 1=Server自然關閉, 2=玩家提前結束, 3=Web強制關閉',
    `EndBalance`         DOUBLE       NULL DEFAULT NULL COMMENT '再次開分結束 Session 時、開分交易前的玩家餘額',
    `TotalGameCount`     INT          NOT NULL DEFAULT 0 COMMENT 'Session 遊玩總場數',
    `TotalBet`           DOUBLE       NOT NULL DEFAULT 0 COMMENT 'Session 累積總押分',
    `MaxBet`             DOUBLE       NULL DEFAULT NULL COMMENT 'Session 最大單局押分；尚未遊戲時為 NULL',
    `MinBet`             DOUBLE       NULL DEFAULT NULL COMMENT 'Session 最小單局押分；尚未遊戲時為 NULL',
    `MaxBalance`         DOUBLE       NOT NULL COMMENT 'Session 最高餘額',
    `MinBalance`         DOUBLE       NOT NULL COMMENT 'Session 最低餘額',
    `StartTime`          DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) COMMENT 'Session 開始時間，內容為伺服器本機時間',
    `EndTime`            DATETIME(6)  NULL DEFAULT NULL COMMENT '再次開分結束 Session 的時間，內容為伺服器本機時間',
    PRIMARY KEY (`RewardRecordId`),
    KEY `IX_UserSessionRewardRecord_UserUID_RecordStatus`
        (`UserUID`, `RecordStatus`),
    KEY `IX_UserSessionRewardRecord_UserUID_StartTime`
        (`UserUID`, `StartTime`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = '玩家開洗分及上升額度功能紀錄';
