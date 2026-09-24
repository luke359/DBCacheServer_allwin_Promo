# DEC-001 MySQL 與 Provider

| 欄位 | 內容 |
|---|---|
| Decision ID | DEC-001 |
| Status | Proposed |
| 問題 | 目標 MySQL Major／Minor、`sql_mode`、正式 Provider(驅動程式)與版本尚未提供；CI(持續整合) 必須與正式環境一致。 |
| 選項 | A. 沿用正式 DBCache 現用版本與 Provider：整合成本較低，但需先證明符合 PCS-02／03。B. 統一升級到指定版本：功能可控，但需另做相容性與部署評估。 |
| 決策 | 待 DBA 與 Data Owner 確認；不得由此骨架推定。 |
| 理由 | PCS-07 第 22.2 節將此列為 W04～W06 阻擋項。 |
| 影響 | Migration、MysqlAcess V2、Adapter、Integration／Concurrency Test 與發布候選版。 |
| 決策日期 | 待確認 |
| 負責人／核准人 | DBA、Data Owner；姓名待指定 |
| 後續工作 | 確認版本、`sql_mode`、Provider／版本與 CI 一致性；期限待指定。 |

## 本機驗證紀錄（尚非正式環境決策）

- 2026-09-16：DBCacheServer 目標為 `net6.0`，`MysqlAcess` 使用 `MySql.Data` 9.4.0。
- 使用使用者提供的本機連線檔唯讀查詢：MySQL Server 為 9.7.0，`sql_mode` 為 `ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION`，`lower_case_table_names=1`。
- V001 已在獨立的 `promotion_test_20260916161130_ef34d7cf` 套用並通過結構驗證；此證據不代表正式環境版本已確認。
- `MySql.Data` 9.4.0 在本方案 `net6.0` 整合測試專案還原出的部分相依套件會發出目標框架支援警告；即使測試通過，零警告發布關卡仍未滿足。不得透過隱藏警告宣稱完成，需由 Tech Lead／DBA 決定相容版本組合並同步上游規格與 Host。
