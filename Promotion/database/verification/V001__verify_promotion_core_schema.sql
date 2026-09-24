-- Read-only metadata fingerprints for V001. The runner compares all four rows with
-- V001__expected_fingerprints.tsv; a mismatch is a deployment stop, never auto-repair.
SET SESSION group_concat_max_len = 1048576;

SELECT 'tables', COUNT(*), SHA2(GROUP_CONCAT(
    CONCAT_WS('|', TABLE_NAME, TABLE_TYPE, ENGINE, TABLE_COLLATION)
    ORDER BY TABLE_NAME SEPARATOR ';'), 256)
FROM information_schema.TABLES
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME IN ('PromotionActivity', 'PromotionTriggerEvent', 'EligibilityEntry', 'BonusStatus', 'BonusHistory')
UNION ALL
SELECT 'columns', COUNT(*), SHA2(GROUP_CONCAT(
    CONCAT_WS('|', TABLE_NAME, ORDINAL_POSITION, COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE,
        IFNULL(COLUMN_DEFAULT, '<NULL>'), EXTRA,
        IFNULL(CHARACTER_SET_NAME, '<NULL>'), IFNULL(COLLATION_NAME, '<NULL>'))
    ORDER BY TABLE_NAME, ORDINAL_POSITION SEPARATOR ';'), 256)
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME IN ('PromotionActivity', 'PromotionTriggerEvent', 'EligibilityEntry', 'BonusStatus', 'BonusHistory')
UNION ALL
SELECT 'indexes', COUNT(*), SHA2(GROUP_CONCAT(
    CONCAT_WS('|', TABLE_NAME, INDEX_NAME, SEQ_IN_INDEX, COLUMN_NAME, NON_UNIQUE,
        INDEX_TYPE, IFNULL(SUB_PART, '<NULL>'), COLLATION)
    ORDER BY TABLE_NAME, INDEX_NAME, SEQ_IN_INDEX SEPARATOR ';'), 256)
FROM information_schema.STATISTICS
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME IN ('PromotionActivity', 'PromotionTriggerEvent', 'EligibilityEntry', 'BonusStatus', 'BonusHistory')
UNION ALL
SELECT 'foreign_keys', COUNT(*), SHA2(GROUP_CONCAT(
    CONCAT_WS('|', k.TABLE_NAME, k.CONSTRAINT_NAME, k.ORDINAL_POSITION,
        k.COLUMN_NAME, k.REFERENCED_TABLE_NAME, k.REFERENCED_COLUMN_NAME,
        r.UPDATE_RULE, r.DELETE_RULE)
    ORDER BY k.TABLE_NAME, k.CONSTRAINT_NAME, k.ORDINAL_POSITION SEPARATOR ';'), 256)
FROM information_schema.KEY_COLUMN_USAGE AS k
JOIN information_schema.REFERENTIAL_CONSTRAINTS AS r
  ON r.CONSTRAINT_SCHEMA = k.CONSTRAINT_SCHEMA
 AND r.CONSTRAINT_NAME = k.CONSTRAINT_NAME
 AND r.TABLE_NAME = k.TABLE_NAME
WHERE k.CONSTRAINT_SCHEMA = DATABASE()
  AND k.REFERENCED_TABLE_NAME IS NOT NULL
  AND k.TABLE_NAME IN ('PromotionActivity', 'PromotionTriggerEvent', 'EligibilityEntry', 'BonusStatus', 'BonusHistory');
