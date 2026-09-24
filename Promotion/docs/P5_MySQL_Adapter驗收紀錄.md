# P5 MySQL Adapter 驗收紀錄

更新日期：2026-09-17。狀態：P5 本機實作與可獨立執行的驗收已完成；正式環境簽核與跨流程鎖序驗證尚有外部前置條件。

## 已實作

- `PromotionMySqlDataStore`、`PromotionMySqlDataTransaction` 覆蓋 PCS-03 資料存取介面；DBCache 專案的 `PromotionV2Gateway` 轉接既有 `MysqlAcess` V2，維持 `Promotion.Data.MySql` 不反向參考 Host。
- 五張表固定欄位、欄位與 Constraint 白名單及全欄位 Row Mapper；資料庫 `NULL`、decimal、DATE、DATETIME(6)、`CHAR(36)` 與 Snapshot 依領域不變條件映射，非法資料分類為 `DataCorruption`。
- 資格與歷史新增回傳真實 `LastInsertedId`；PromotionBonusStatus 玩家列使用 `FOR UPDATE`；條件領取、Claimed 計數、可用資格過期、Keyset 候選與歷史分批刪除已接入 V2。

## 已驗證

- `Promotion.Data.MySql` Release 建置零警告零錯誤。
- DBCache Release 建置成功；仍有既有警告與 `MySql.Data` 9.4.0 在 `net6.0` 的套件支援警告。
- 本機 `promotion_test_` 資料庫：P5 專項測試 5/5、P4／P5 全套 27/27 通過；加入 P6 流程與併發測試後為 29/29 通過。涵蓋五表欄位往返、真實自增 ID、兩端建立玩家列、條件領取、Claimed 計數、不可疊加與互斥群組、資格過期、維護候選跨頁、歷史截止時間與批次刪除、額外 Schema 欄位、非法 enum／Snapshot 拒絕，以及欄位與 Constraint 白名單。零影響列的狀態重新讀取與分頁／清理查詢模型亦有專項測試。P5 驗收時單元測試 41/41、契約測試 2/2、架構測試 1/1 通過；P6 階段的最新數量見 P6 驗收紀錄。

## 後續依賴

- 使用者已確認新增 `ConcurrencyConflict`，資格條件更新 0 列與 PromotionBonusStatus 更新後重新讀取不一致均使用此分類；PCS-03、PCS-05 與 DB-022 測試規格已同步。
- DB-030 要驗證 PromotionBonusStatus → TriggerEvent → Eligibility → Activity → PromotionBonusHistory 的完整業務鎖序，須在 P6 流程協調元件完成後進行；P5 已固定資格資料按 `EligibilityEntryId` 遞增處理。
- DEC-001 正式 MySQL／Provider 版本尚未簽核；`net6.0`／`MySql.Data` 9.4.0 的支援警告未解。這阻擋正式環境驗收，不阻擋本機 P5 實作。P6 階段成果見 [P6 驗收紀錄](P6_Core_Facade驗收紀錄.md)；P7 Host 業務入口仍不可啟用。
