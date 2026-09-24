# Promotion Core 實作狀態

此目錄以 PCS-01～PCS-07 為規格來源，使用 C# 10、.NET 6 與 UTF-8。

目前已建立 P0 的獨立 Solution(方案)骨架、中央套件版本與鎖定檔，以及 P1／P2 的公開契約、領域模型、純計算、快照與輸入驗證。依更新的企劃案，兩個上限已改為 `null` 表示無限制，新增 `GameServerList` 查詢契約與 `WagerCalculationType`，新快照為版本 2 並保留版本 1 讀取能力。P3 已包含 V001 建表、V002 活動欄位升級、V003 獎勵表更名，以及 V004 在 PromotionBonusStatus 保存每日首登入／首儲最近 Business Day；每日維護不清除這兩個日期。P4 已在 DBCache 專案新增 `MysqlAcess` partial V2、型別化參數與交易 Context。P5 核心專用 MySQL Adapter、DBCache V2 橋接、五表 Mapper 與本機可獨立執行的驗收已完成；完整 DB-009～DB-030 清單仍包含 P4 限制與 P6 流程相依案例。P6 Facade 本機階段驗收已完成；P7 的 DBCache Host 整合位於 `DBCacheServer/Promotion/PromotionCoreHost.cs`，不再保留獨立的 `Promotion.Host.DBCache` 專案。公開 `IPromotionCoreService` 是契約。

本地測試用 MySQL 連線在這裡 D:\Mobile_Sever_t01\SqlConnection.txt  

驗證命令：

```powershell
dotnet restore .\PromotionCore.sln --locked-mode
dotnet build .\PromotionCore.sln --configuration Release --no-restore
dotnet test .\tests\Promotion.Core.UnitTests\Promotion.Core.UnitTests.csproj --configuration Release --no-build
dotnet test .\tests\Promotion.Core.ContractTests\Promotion.Core.ContractTests.csproj --configuration Release --no-build
dotnet test .\tests\Promotion.ArchitectureTests\Promotion.ArchitectureTests.csproj --configuration Release --no-build
```

完成 P4～P7 與正式發布前須先完成 [DEC-001](docs/decisions/DEC-001_MySQL與Provider.md) 與 [DEC-002](docs/decisions/DEC-002_程式位置與專案映射.md)，並依 PCS-07 處理其餘對應決策。現有 `sql/` 腳本不是本次具版本紀錄的 Migration(資料庫移轉)。

本機 V001 執行方式（僅接受 loopback `promotion_test_` 資料庫）：

```powershell
.\database\migrations\Invoke-PromotionMigration.ps1 -ConnectionFile 'D:\Mobile_Sever_t01\SqlConnection.txt' -DatabaseName 'promotion_test_20260916161130_ef34d7cf'
.\database\migrations\Invoke-PromotionMigrationV002.ps1 -ConnectionFile 'D:\Mobile_Sever_t01\SqlConnection.txt' -DatabaseName 'promotion_test_20260916161130_ef34d7cf'
.\database\migrations\Invoke-PromotionMigrationV003.ps1 -ConnectionFile 'D:\Mobile_Sever_t01\SqlConnection.txt' -DatabaseName 'promotion_test_20260916161130_ef34d7cf'
.\database\migrations\Invoke-PromotionMigrationV004.ps1 -ConnectionFile 'D:\Mobile_Sever_t01\SqlConnection.txt' -DatabaseName 'promotion_test_20260916161130_ef34d7cf'
```

新建空測試庫依序執行 V001、V002、V003、V004；既有 V003 資料庫直接執行 V004。勿再執行僅驗證 69 欄舊結構的 V001 runner。V002 保留既有兩個上限值；四個 runner 都只接受 loopback `promotion_test_` 資料庫。

P3／P4 整合測試從環境變數 `PROMOTION_TEST_MYSQL_CONNECTION_STRING` 取得連線字串；必須指向 loopback 與 `promotion_test_` 資料庫。P4 測試會在該測試庫短暫建立並清除獨立測試表，不將密碼寫入 Repository。只在確認所有核心表都沒有資料且具備審核授權時，才可使用 `Invoke-PromotionRollback.ps1` 的 `-AllowEmptySchemaRollback`；先以 `-WhatIf` 檢查，正式資料庫禁止使用此腳本。

P4 依使用者核准調整為 V2 每次呼叫使用獨立 Connection；不再依賴舊 `ReConnect` 會重新指派的共用 Mutex。舊 `MysqlAcess` 方法本體保持不變，因此尚不能全面阻止 V2 交易 action 混用舊 API；舊 API 操作也不會加入 V2 Transaction。進入正式 Host 接線前，須以架構／程式審查禁止此類混用，不能把初步 V2 測試通過視為可上線。

工作指引的正式環境 MySQL／Provider 決策仍未簽核；`MySql.Data` 9.4.0 與 `net6.0` 組合在整合測試建置出現相依套件支援警告，零警告關卡尚未通過。已提供只限空資料測試庫的受控回復腳本，但未執行回復。

P4 V2 本階段的測試與未滿足條件詳見 [P4 驗收紀錄](docs/P4_MysqlAcess_V2驗收紀錄.md)。DB-009～DB-030 包含尚未實作的 P5 Adapter 案例，不得將 V2 測試通過視為完整 DB 清單通過。

P5 已實作的範圍及仍待補齊的驗收案例詳見 [P5 驗收紀錄](docs/P5_MySQL_Adapter驗收紀錄.md)。P6 Core Facade 的本機實作與階段驗收詳見 [P6 驗收紀錄](docs/P6_Core_Facade驗收紀錄.md)；PCS-06 全案例與 50 回併發壓力驗證屬 P8。
