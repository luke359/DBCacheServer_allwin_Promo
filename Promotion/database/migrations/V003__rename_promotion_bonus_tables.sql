-- V003: rename legacy bonus tables and their schema objects to the Promotion-prefixed names.
-- Apply only when BonusStatus and BonusHistory exist and the Promotion-prefixed tables do not.

RENAME TABLE
    `BonusStatus` TO `PromotionBonusStatus`,
    `BonusHistory` TO `PromotionBonusHistory`;

ALTER TABLE `PromotionBonusStatus`
    DROP FOREIGN KEY `FK_BonusStatus_EligibilityEntryId`,
    DROP FOREIGN KEY `FK_BonusStatus_ActivityUID`,
    RENAME INDEX `UK_BonusStatus_BonusTaskId` TO `UK_PromotionBonusStatus_BonusTaskId`,
    RENAME INDEX `IX_BonusStatus_TaskState_BusinessDay` TO `IX_PromotionBonusStatus_TaskState_BusinessDay`,
    RENAME INDEX `FK_BonusStatus_EligibilityEntryId` TO `FK_PromotionBonusStatus_EligibilityEntryId`,
    RENAME INDEX `FK_BonusStatus_ActivityUID` TO `FK_PromotionBonusStatus_ActivityUID`,
    ADD CONSTRAINT `FK_PromotionBonusStatus_EligibilityEntryId`
        FOREIGN KEY (`EligibilityEntryId`) REFERENCES `EligibilityEntry` (`EligibilityEntryId`),
    ADD CONSTRAINT `FK_PromotionBonusStatus_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`);

ALTER TABLE `PromotionBonusHistory`
    DROP FOREIGN KEY `FK_BonusHistory_EligibilityEntryId`,
    DROP FOREIGN KEY `FK_BonusHistory_ActivityUID`,
    RENAME INDEX `UK_BonusHistory_BonusTaskId` TO `UK_PromotionBonusHistory_BonusTaskId`,
    RENAME INDEX `IX_BonusHistory_UserUID_ClosedAt` TO `IX_PromotionBonusHistory_UserUID_ClosedAt`,
    RENAME INDEX `IX_BonusHistory_ClosedAt` TO `IX_PromotionBonusHistory_ClosedAt`,
    RENAME INDEX `FK_BonusHistory_EligibilityEntryId` TO `FK_PromotionBonusHistory_EligibilityEntryId`,
    RENAME INDEX `FK_BonusHistory_ActivityUID` TO `FK_PromotionBonusHistory_ActivityUID`,
    ADD CONSTRAINT `FK_PromotionBonusHistory_EligibilityEntryId`
        FOREIGN KEY (`EligibilityEntryId`) REFERENCES `EligibilityEntry` (`EligibilityEntryId`),
    ADD CONSTRAINT `FK_PromotionBonusHistory_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`);
