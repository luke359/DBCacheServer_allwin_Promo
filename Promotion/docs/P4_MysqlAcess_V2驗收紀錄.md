# P4 `MysqlAcess` V2 驗收紀錄

更新日期：2026-09-16。狀態：V2 能力與本階段可執行測試已完成；PCS-06 原 DB-009～DB-030 清單含 P5 Adapter 驗收，且 DB-020 有已核准的舊 API 不改動限制，故**不得宣稱整份清單全部通過**。

## 實作位置與約束

- DBCache：`..\DBCacheServer\MysqlAcess.cs` 只加 `partial`；V2 放在 `MysqlAcess.V2.cs`、`MysqlAcess.V2.Models.cs`、`MysqlAcess.V2.CommandBuilder.cs`。
- V2 每次呼叫使用獨立 Connection；單次交易內所有 Command 使用同一 Connection／Transaction。這取代原規格的共用 Connection／Mutex 模型，已由使用者核准。
- 測試僅使用本機 `promotion_test_` 暫存資料庫；獨立測試表在案例結束後清除。

## PCS-06 DB 案例對照

| 案例 | 狀態 | 證據／待辦 |
|---|---|---|
| DB-009 | 通過 | `NULL` 綁定 `DBNull.Value`，真實 MySQL 往返保留 `null`。 |
| DB-010～012 | P5 待辦 | 五種領域模型全欄位映射、Adapter 的自增 ID 及明確欄位映射尚未實作。V2 原始 Insert ID 已在測試驗證。 |
| DB-013 | 通過 | Builder 與真實 MySQL 均測試攻擊字串作為值，不改變查詢或刪除範圍。 |
| DB-014 | 部分通過 | V2 拒絕非法識別字；核心表／欄白名單屬 P5 Adapter，尚待實作。 |
| DB-015～017 | 通過 | 型別、`IN`、`NULL` 條件、`Limit`／`Offset`、無條件異動與非交易 `FOR UPDATE` 均有測試；`Limit`／`Offset` 已改用 SQL 參數。 |
| DB-018～019 | 通過 | 真實 MySQL 測試交易內可見、交易外不可見、提交、回滾、Context 結束後失效與原例外保留。 |
| DB-020 | 部分通過／設計限制 | 巢狀 V2、交易內非交易 V2、Context 跨執行緒使用已拒絕；在不修改舊方法本體的約束下，舊 API 不能被 V2 動態攔截。舊 API 操作不會加入 V2 交易；Host 與 Adapter 必須禁止混用。 |
| DB-021～027 | P5 待辦 | 玩家列鎖、資格狀態條件、計數、維護分頁與清理、Mapper 與 Adapter 例外處理尚未實作。 |
| DB-028 | 部分通過 | Duplicate Key、真實鎖等待逾時與 Commit 階段受控斷線均以真實 MySQL 驗證；1062／1213／1205／連線錯誤碼分類有決定性測試。Data Corruption 由 P5 Mapper 驗收。 |
| DB-029 | 依核准設計替代 | V2 不使用舊 Mutex；兩筆同實例交易可並行，各自持有獨立 Connection。已在真實 MySQL 驗證。 |
| DB-030 | P5／P6 待辦 | 固定業務鎖定順序需在 Adapter 與核心流程實作後驗證。 |

## 後續進入 P5 前的限制

1. Host／Adapter 不得在 `ExecuteParameterizedTransaction` 的 action 中呼叫任何舊 `MysqlAcess` API；這項規則目前無法由 V2 執行時完全強制。
2. 不得把 P4 V2 的方案測試通過解讀為 DB-009～DB-030 全部通過或正式環境可部署。
3. `net6.0` 搭配目前 `MySql.Data` 9.4.0 的相依套件仍有支援警告；DEC-001 的正式 Provider／版本決策尚未簽核。
