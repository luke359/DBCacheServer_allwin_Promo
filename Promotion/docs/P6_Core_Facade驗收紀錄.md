# P6 Core Facade 驗收紀錄

更新日期：2026-09-17。狀態：P6 本機核心服務實作與階段驗收完成；P7 Host 接線及 P8 全案例驗收另行進行。

## 實作範圍

- `PromotionCoreService` 實作 `IPromotionCoreService` 的 12 個 API。初始化採一次性安全發布；回傳集合封裝為唯讀集合。
- 建立資格、查詢、領取、流水、獨立達標結案、紅利用盡、玩家放棄、每日維護及歷史查詢皆透過 P5 Data Store。舊任務重播以 BonusTaskId 與玩家歸屬查 History，不改動新任務。
- 四種結案共用單一 Transaction 路徑：暫設 `Closing`、計算轉換、插入 History、重置 Status；失敗時整筆 Rollback。已提交而殘留的 `Closing` 回 `InvalidBonusTaskState`（4003），非法 Snapshot 回 `DataCorruption`。
- Deadlock、Lock Wait Timeout、已確認未提交的 Connection Failure 最多三次完整交易；EventId 主鍵競爭查回首次事實，Claim 的 TaskId 唯一鍵碰撞換 Guid 重試。Commit 結果不明只對可去重操作查證；`AccumulateWager` 立即停止重送。
- 每日維護按玩家隔離失敗，維護結案在 Commit 結果不明時以 History 查證並還原 Wallet Instruction。

## 驗證結果

- 本次變更驗證：單元測試 57/57、契約測試 2/2、架構測試 1/1 通過；MySQL 整合專案建置成功，12 項不需連線的案例通過，17 項因環境未提供 `PROMOTION_TEST_MYSQL_CONNECTION_STRING` 未能執行。`Promotion.Core.ConcurrencyTests` 目前沒有可發現的測試。
- MySQL 整合案例已對齊 12 個 API 的主要路徑，包含累計達標回 `ReadyToClose`、Host 獨立達標結案、事件時間微秒重播、跨玩家拒絕、舊任務與新任務隔離、Guid 碰撞、固定轉換、餘額歸零、公式失敗回滾、每日維護及 `Closing` 分類；需在具測試連線字串的環境重新執行完整 29 項案例。
- 注入測試涵蓋 Deadlock、Lock Wait Timeout、Connection Backoff、EventId 競爭、Commit Unknown 查證與不重送、維護部分成功及維護結案回應遺失。

## 後續階段

- P7 將接上 DBCache Request Adapter、可靠保存 Wallet Instruction、Dispatcher 與對帳；在此之前不得啟用優惠業務入口。
- P8 依 PCS-06 逐項執行完整案例矩陣及 50 回併發壓力驗證。DEC-001 正式 MySQL／Provider 版本簽核與既有 `net6.0` 套件支援警告仍須在正式部署前處理。
