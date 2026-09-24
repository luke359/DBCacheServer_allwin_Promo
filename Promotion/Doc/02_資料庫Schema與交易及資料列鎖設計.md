# PCS-02 資料庫 Schema(結構描述)、唯一索引、交易及資料列鎖設計

## 1. 文件資訊

| 項目 | 內容 |
|---|---|
| 文件編號 | PCS-02 |
| 文件名稱 | 資料庫 Schema、唯一索引、交易及資料列鎖設計 |
| 文件狀態 | 第一版完成 |
| 上游依據 | [優惠活動核心服務企劃](./優惠活動核心服務企劃.md)、[PCS-01](./01_優惠活動資料模型與欄位定義.md) |
| 文件目的 | 定義 MySQL 實體表、索引、交易邊界、Row Lock(資料列鎖)與冪等性約束 |

---

## 2. 文件目錄

1. 文件資訊
2. 文件目錄
3. 文件集合
4. 共用規則
5. 資料表總覽
6. Schema
7. 索引與限制用途
8. 共通交易規則
9. 建立資格交易
10. 領取優惠交易
11. 累計流水交易
12. Bonus Task 結案交易
13. 每日維護交易
14. Deadlock 與重試原則
15. 資料保存與清理
16. 交叉引用

---

## 3. 文件集合

| 編號 | 文件 | 狀態 |
|---|---|---|
| PCS-01 | [優惠活動資料模型與欄位定義](./01_優惠活動資料模型與欄位定義.md) | 第一版完成，共用名詞基準 |
| PCS-02 | 本文件 | 第一版完成 |
| PCS-03 | [核心專用資料存取介面與 MysqlAcess 擴充規格](./03_核心專用資料存取介面與MysqlAcess擴充規格.md) | 第一版完成 |
| PCS-04 | [核心服務 API 規格](./04_核心服務API規格.md) | 第一版完成 |
| PCS-05 | [錯誤碼、重試及交易一致性規格](./05_錯誤碼重試及交易一致性規格.md) | 第一版完成 |
| PCS-06 | [單元測試與整合測試案例](./06_單元測試與整合測試案例.md) | 第一版完成 |
| PCS-07 | [核心服務實作與整合工作指引](./07_核心服務實作與整合工作指引.md) | 第一版完成（第四批） |

名詞、列舉值與欄位語意以 PCS-01 為準，本文件不另行定義同名業務概念。

---

## 4. 共用規則

- Storage Engine(儲存引擎)使用 InnoDB，才能提供交易、Row Lock 與外鍵。
- Character Set(字元集)使用 `utf8mb4`，Collation(定序)使用 `utf8mb4_bin`；EventId、BonusTaskId 與 ExclusiveGroup 必須區分大小寫。
- 資料表及欄位使用 Pascal Case。
- C# `long` 對應 `BIGINT`；部署前須確認既有玩家主檔的 `UserUID` 型別，所有關聯欄位必須完全相同。
- 整數活動設定使用 `INT`；小數活動設定、任務金額、流水及計算結果使用 `DECIMAL(20,4)`；一般時間使用 `DATETIME(6)`；Business Day 使用 `DATE`。
- 所有 SQL 值必須參數化。表名、欄名、排序欄位與鎖模式只能由程式內部白名單產生。
- 下列 DDL(Data Definition Language，資料定義語言)是第一版目標 Schema；實際 Migration(資料庫移轉)檔須在實作階段依部署環境的 MySQL 版本驗證後建立。

---

## 5. 資料表總覽

| 資料表 | 用途 | 資料生命週期 |
|---|---|---|
| PromotionActivity | H5gSet 管理的目前活動設定 | 不實體刪除；`WeekdayMask = '0000000'` 表示停用，`ActivityStatus = 2` 表示封存 |
| PromotionTriggerEvent | EventId 首次處理事實，用於重送與零結果冪等 | 第一版不刪除 |
| EligibilityEntry | 玩家在指定 Business Day 的可領資格 | 狀態終結後保留；第一版不刪除 |
| PromotionBonusStatus | 每位玩家唯一的目前優惠總狀態 | 永久保留、不刪除 |
| PromotionBonusHistory | 已結案 Bonus Task | ClosedAt 超過 90 日後刪除 |

Promotion Trigger Event 與 Eligibility Entry 第一版不設定清理期限，因上游企劃尚未授權刪除這兩類冪等性資料。若日後需要清理，必須先定義 EventId 可重送的最長期限，再同步修改 PCS-01、PCS-05 與 PCS-06。

---

## 6. Schema

### 6.1 PromotionActivity

下列為新建資料表的目標結構；既有資料表的參考 SQL 位於 [PromotionActivity_Migration_20260916.sql](../sql/PromotionActivity_Migration_20260916.sql)。Promotion Core 已執行 V001 的資料庫須依序執行 [V002 Migration](../database/migrations/V002__promotion_activity_new_fields.sql)、[V003 Migration](../database/migrations/V003__rename_promotion_bonus_tables.sql)、[V004 Migration](../database/migrations/V004__bonus_status_daily_trigger_days.sql) 與 [V005 Migration](../database/migrations/V005__promotion_activity_status.sql)。V005 新增 `ActivityStatus`，既有資料與新資料預設為 `Active = 1`；`Archived = 2`。V004 在 PromotionBonusStatus 新增兩個可為 `NULL` 的每日觸發日期；既有資料不回填。既有兩個上限數值保留原值；新資料列的預設值分別為 `MaxBetAmount = 5`、`MaxBalanceConvertedAmount = 150`，`GameServerList = NULL`，`WagerCalculationType = 1`。

```sql
CREATE TABLE `PromotionActivity` (
    `ActivityUID` BIGINT NOT NULL,
    `ActivityInfo` VARCHAR(100) NULL,
    `BonusType` TINYINT UNSIGNED NOT NULL,
    `FixedBonusAmount` INT NOT NULL,
    `MaxBonusAmount` INT NOT NULL,
    `DepositPercentage` INT NOT NULL,
    `MinimumDepositAmount` INT NULL,
    `WagerMultiplier` INT NOT NULL,
    `WagerCalculationType` TINYINT UNSIGNED NOT NULL DEFAULT 1,
    `MaxBetAmount` INT NULL DEFAULT 5,
    `GameServerList` VARCHAR(500) NULL DEFAULT NULL,
    `WagerContributionRate` DECIMAL(20,4) NOT NULL,
    `ConvertType` TINYINT UNSIGNED NOT NULL,
    `FixedConvertedAmount` INT NOT NULL,
    `MaxBalanceConvertedAmount` INT NULL DEFAULT 150,
    `DailyClaimLimit` INT NOT NULL,
    `StartDate` DATE NULL,
    `EndDate` DATE NULL,
    `WeekdayMask` CHAR(7) NOT NULL,
    `ActivityStatus` TINYINT UNSIGNED NOT NULL DEFAULT 1,
    `TriggerType` TINYINT UNSIGNED NOT NULL,
    `IsNonStackable` TINYINT UNSIGNED NOT NULL,
    `ExclusiveGroup` VARCHAR(64) NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    `UpdatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`ActivityUID`),
    KEY `IX_PromotionActivity_TriggerType` (`TriggerType`),
    KEY `IX_PromotionActivity_ExclusiveGroup` (`ExclusiveGroup`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;
```

H5gSet 與核心服務須依 PCS-01 驗證列舉值、數值範圍、日期先後及 WeekdayMask 格式；不依賴不同 MySQL 版本對 `CHECK` 的支援差異。建立新資格時只採用 `ActivityStatus = 1` 的活動；Adapter 仍須映射並驗證候選活動，使非法狀態回報 Data Corruption。封存前已建立的資格仍可讀取該活動並領取。

### 6.2 PromotionTriggerEvent

```sql
CREATE TABLE `PromotionTriggerEvent` (
    `EventId` VARCHAR(128) NOT NULL,
    `UserUID` BIGINT NOT NULL,
    `TriggerType` TINYINT UNSIGNED NOT NULL,
    `EventTime` DATETIME(6) NOT NULL,
    `BusinessDay` DATE NOT NULL,
    `EligibleDepositAmount` DECIMAL(20,4) NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`EventId`),
    KEY `IX_PromotionTriggerEvent_UserUID_BusinessDay`
        (`UserUID`, `BusinessDay`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;
```

`EventId` 主鍵確保事件重送不會建立第二筆處理事實；即使事件產生零筆資格，這筆資料仍必須提交。

### 6.3 EligibilityEntry

```sql
CREATE TABLE `EligibilityEntry` (
    `EligibilityEntryId` BIGINT NOT NULL AUTO_INCREMENT,
    `EventId` VARCHAR(128) NOT NULL,
    `UserUID` BIGINT NOT NULL,
    `ActivityUID` BIGINT NOT NULL,
    `BusinessDay` DATE NOT NULL,
    `TriggerType` TINYINT UNSIGNED NOT NULL,
    `EligibleDepositAmount` DECIMAL(20,4) NULL,
    `Status` TINYINT UNSIGNED NOT NULL,
    `ClaimedBonusTaskId` CHAR(36) NULL,
    `ExcludedByBonusTaskId` CHAR(36) NULL,
    `StatusChangedAt` DATETIME(6) NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`EligibilityEntryId`),
    UNIQUE KEY `UK_EligibilityEntry_EventId_ActivityUID`
        (`EventId`, `ActivityUID`),
    UNIQUE KEY `UK_EligibilityEntry_ClaimedBonusTaskId`
        (`ClaimedBonusTaskId`),
    KEY `IX_EligibilityEntry_UserUID_BusinessDay_Status`
        (`UserUID`, `BusinessDay`, `Status`),
    KEY `IX_EligibilityEntry_ClaimCount`
        (`UserUID`, `ActivityUID`, `BusinessDay`, `Status`),
    KEY `IX_EligibilityEntry_Status_BusinessDay`
        (`Status`, `BusinessDay`),
    CONSTRAINT `FK_EligibilityEntry_EventId`
        FOREIGN KEY (`EventId`) REFERENCES `PromotionTriggerEvent` (`EventId`),
    CONSTRAINT `FK_EligibilityEntry_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;
```

`UK_EligibilityEntry_ClaimedBonusTaskId` 允許多列 `NULL`，但同一 BonusTaskId 只能標記一筆資格為已領取。

### 6.4 PromotionBonusStatus

```sql
CREATE TABLE `PromotionBonusStatus` (
    `UserUID` BIGINT NOT NULL,
    `TaskState` TINYINT UNSIGNED NOT NULL,
    `BonusTaskId` CHAR(36) NULL,
    `EligibilityEntryId` BIGINT NULL,
    `ActivityUID` BIGINT NULL,
    `BusinessDay` DATE NULL,
    `LastFirstLoginBusinessDay` DATE NULL DEFAULT NULL,
    `LastFirstDepositBusinessDay` DATE NULL DEFAULT NULL,
    `ActivitySnapshotJson` TEXT NULL,
    `BonusAmount` DECIMAL(20,4) NOT NULL,
    `RequiredWagerAmount` DECIMAL(20,4) NOT NULL,
    `CurrentWagerAmount` DECIMAL(20,4) NOT NULL,
    `ClaimedAt` DATETIME(6) NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    `UpdatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`UserUID`),
    UNIQUE KEY `UK_PromotionBonusStatus_BonusTaskId` (`BonusTaskId`),
    KEY `IX_PromotionBonusStatus_TaskState_BusinessDay`
        (`TaskState`, `BusinessDay`),
    CONSTRAINT `FK_PromotionBonusStatus_EligibilityEntryId`
        FOREIGN KEY (`EligibilityEntryId`) REFERENCES `EligibilityEntry` (`EligibilityEntryId`),
    CONSTRAINT `FK_PromotionBonusStatus_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;
```

建立空白 Bonus Status 時使用：`TaskState = 0`、三個金額欄位為 `0`、所有任務 nullable 欄位為 `NULL`；兩個每日觸發日期初始亦為 `NULL`，後續不隨任務結案或每日維護清除。

### 6.5 PromotionBonusHistory

```sql
CREATE TABLE `PromotionBonusHistory` (
    `BonusHistoryId` BIGINT NOT NULL AUTO_INCREMENT,
    `BonusTaskId` CHAR(36) NOT NULL,
    `UserUID` BIGINT NOT NULL,
    `EligibilityEntryId` BIGINT NOT NULL,
    `ActivityUID` BIGINT NOT NULL,
    `BusinessDay` DATE NOT NULL,
    `ActivitySnapshotJson` TEXT NOT NULL,
    `BonusAmount` DECIMAL(20,4) NOT NULL,
    `RequiredWagerAmount` DECIMAL(20,4) NOT NULL,
    `CurrentWagerAmount` DECIMAL(20,4) NOT NULL,
    `ConvertedAmount` DECIMAL(20,4) NOT NULL,
    `CloseReason` TINYINT UNSIGNED NOT NULL,
    `ClaimedAt` DATETIME(6) NOT NULL,
    `ClosedAt` DATETIME(6) NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL,
    PRIMARY KEY (`BonusHistoryId`),
    UNIQUE KEY `UK_PromotionBonusHistory_BonusTaskId` (`BonusTaskId`),
    KEY `IX_PromotionBonusHistory_UserUID_ClosedAt`
        (`UserUID`, `ClosedAt`),
    KEY `IX_PromotionBonusHistory_ClosedAt` (`ClosedAt`),
    CONSTRAINT `FK_PromotionBonusHistory_EligibilityEntryId`
        FOREIGN KEY (`EligibilityEntryId`) REFERENCES `EligibilityEntry` (`EligibilityEntryId`),
    CONSTRAINT `FK_PromotionBonusHistory_ActivityUID`
        FOREIGN KEY (`ActivityUID`) REFERENCES `PromotionActivity` (`ActivityUID`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin;
```

Bonus History 不建立對 Bonus Status 的外鍵，因為 Bonus Status 在結案後會清空目前 BonusTaskId；兩者以不可重複的 BonusTaskId 建立業務關聯。

---

## 7. 索引與限制用途

| 限制／索引 | 保證或查詢用途 |
|---|---|
| PromotionTriggerEvent PK(EventId) | 相同事件只首次處理一次，包含零資格結果 |
| EligibilityEntry UK(EventId, ActivityUID) | 同一事件對同一活動最多建立一筆資格 |
| EligibilityEntry UK(ClaimedBonusTaskId) | 一個任務只能來自一筆資格 |
| EligibilityEntry IX(UserUID, BusinessDay, Status) | 查詢玩家可領資格、每日逾期處理 |
| EligibilityEntry IX(UserUID, ActivityUID, BusinessDay, Status) | 領取前計算本活動當日已成功領取次數 |
| PromotionBonusStatus PK(UserUID) | 每位玩家唯一狀態及玩家級 Row Lock |
| PromotionBonusStatus UK(BonusTaskId) | 目前任務識別碼不重複 |
| PromotionBonusStatus IX(TaskState, BusinessDay) | 找出跨 Business Day 的進行中任務 |
| PromotionBonusHistory UK(BonusTaskId) | 結案與每日維護重試不重複建立歷史 |
| PromotionBonusHistory IX(UserUID, ClosedAt) | 玩家 90 日歷史查詢 |
| PromotionBonusHistory IX(ClosedAt) | 90 日歷史清理 |

唯一索引是冪等性的最後防線，不能取代交易內的既有結果查詢與業務驗證。

---

## 8. 共通交易規則

### 8.1 Isolation Level

- 第一版使用 MySQL InnoDB 的 `READ COMMITTED`。
- 每個交易必須明確 Commit(提交)或 Rollback(回復)，禁止依連線關閉隱含處理。
- 不得在同一業務交易內混用其他連線或舊版 `MysqlAcess.select`。

### 8.2 玩家鎖

所有會新增或修改某玩家 Promotion Trigger Event、Eligibility Entry、Bonus Status 或 Bonus History 的流程，先確保該玩家有 Bonus Status，再執行：

```sql
SELECT ...
FROM `PromotionBonusStatus`
WHERE `UserUID` = @UserUID
FOR UPDATE;
```

這一列是玩家級鎖定錨點，可跨 DBCache 程序與實例序列化同一玩家的異動。`MysqlAcess` 的程序內 Mutex 只能保護共用連線，不能取代此 Row Lock。

若 Bonus Status 尚不存在，使用「嘗試新增；遇到主鍵重複後重新查詢」建立空白列，再於同一交易內 `SELECT ... FOR UPDATE`。不得使用「先查沒有、再無條件新增」作為唯一併發控制。

### 8.3 固定鎖定順序

為降低 Deadlock(死結)，同一交易依下列順序取得鎖：

1. PromotionBonusStatus，以 UserUID 遞增。
2. PromotionTriggerEvent，以 EventId 遞增。
3. EligibilityEntry，以 EligibilityEntryId 遞增。
4. PromotionActivity，以 ActivityUID 遞增；一般只做一致性讀取，不要求鎖定。
5. PromotionBonusHistory，以 BonusTaskId 唯一鍵新增或查詢。

同一玩家的排除操作涉及多筆 Eligibility Entry 時，必須先依 EligibilityEntryId 遞增取得或更新，禁止使用不固定順序。

### 8.4 交易回傳

- 交易內任何步驟失敗時，不得回傳成功 DTO。
- 只有 Commit 成功後才可通知 DBCache 異動 Bonus Wallet 或 Main Wallet。
- Commit 結果不確定時，不得盲目重做非冪等寫入；應以 EventId、BonusTaskId 或目前資料狀態重新查詢。完整規則由 PCS-05 定義。

---

## 9. 建立資格交易

輸入至少包含 EventId、UserUID、TriggerType、EventTime、EligibleDepositAmount 及 Host 允許的活動範圍。

1. 開始交易並鎖定 `PromotionBonusStatus(UserUID)`；不存在則建立後鎖定。
2. 依 EventId 查詢 PromotionTriggerEvent。
3. 若已存在，讀取其關聯 Eligibility Entry，Commit 唯讀交易並回傳首次結果；不得重新篩選活動。
4. 若不存在，以事件時間計算 BusinessDay，新增 PromotionTriggerEvent；主鍵衝突時回到步驟 2，讀取既有結果。
5. `FirstLoginOfBusinessDay` 或 `FirstDepositOfBusinessDay` 的 Business Day 小於或等於對應的最後觸發日期時，不讀活動、不建立 Eligibility Entry，直接以成功及空清單提交；該新 EventId 仍保留供重送。
6. 每日首次觸發且事件 Business Day 晚於保存日期時，在同一交易更新對應的最後觸發日期；首儲不因儲值金額未達任何活動門檻或零活動符合而延後，延遲的舊日事件也不得使日期倒退。
7. 讀取符合的 Promotion Activity，依 ActivityUID 遞增新增所有符合條件的 Eligibility Entry。
8. Commit；零筆 Eligibility Entry 也是成功結果。

PromotionTriggerEvent 與其 Eligibility Entry 必須在同一交易提交，避免重送讀到「事件已完成但資格尚未建立」的部分狀態。
每日觸發日期亦必須與 Trigger Event、Eligibility Entry 在同一交易提交。較晚 Business Day 以日期遞增自然取得首次資格，不依賴每日維護清除旗號。

---

## 10. 領取優惠交易

1. 開始交易並鎖定 `PromotionBonusStatus(UserUID)`。
2. 以 EligibilityEntryId 讀取並鎖定指定資格。
3. 重新驗證玩家、BusinessDay、`Status = Available`、目前無任務、領取次數上限與互斥條件。
4. 讀取 Promotion Activity，產生 Activity Snapshot，計算 BonusAmount 與 RequiredWagerAmount。
5. 產生 BonusTaskId。
6. 將 Eligibility Entry 更新為 `Claimed` 並寫入 ClaimedBonusTaskId。
7. 將 Bonus Status 更新為 `InProgress` 及完整任務資料。
8. 依 EligibilityEntryId 遞增，將不可疊加或相同 Exclusive Group 的其他 `Available` 資格更新為 `Excluded`，並寫入 ExcludedByBonusTaskId。
9. Commit 後才將 Bonus 派發資訊回傳 DBCache。

步驟 3 的領取次數以同一 `UserUID + ActivityUID + BusinessDay + Status = Claimed` 的 Eligibility Entry 數量計算。失敗嘗試不會改成 Claimed，因此不消耗次數。

任一步驟失敗時，資格狀態、Bonus Status、領取次數效果與排除結果全部回復。

---

## 11. 累計流水交易

1. 開始交易並鎖定 `PromotionBonusStatus(UserUID)`。
2. 驗證目前 `TaskState = InProgress`，且 BonusTaskId 與呼叫端預期任務一致。
3. 以 Activity Snapshot 計算本局有效流水及新的 CurrentWagerAmount。
4. 更新 CurrentWagerAmount 與 UpdatedAt 後 Commit；即使達標或 Bonus Wallet 結算後餘額為零，也不在本交易建立歷史或重置任務。
5. 未達標回 `InProgress`；達標回 `ReadyToClose`。Host 收到 `ReadyToClose` 後另呼叫達標結案；未達標但餘額為零時另呼叫用盡結案。

DBCache 負責確保每局最終遊戲資料只送入一次；核心資料庫不另建遊戲局號去重表。

---

## 12. Bonus Task 結案交易

適用於達標、Bonus 用盡、玩家主動放棄與 Business Day 逾期。達標結案由 Host 在 `AccumulateWager` 成功回傳 `ReadyToClose` 後另行呼叫 `CloseWagerCompletedBonusTask`。

1. 開始獨立交易並鎖定 `PromotionBonusStatus(UserUID)`。
2. 驗證目前 BonusTaskId 與預期任務一致。
3. 若 Bonus History 已有相同 BonusTaskId，視為既有成功結果，不再建立或重置。
4. 將 TaskState 暫時設為 `Closing`，僅存在於未提交交易。
5. 達標結案先重新驗證 `CurrentWagerAmount >= RequiredWagerAmount`，再以 Activity Snapshot 及 Host 傳入的達標局資料計算 ConvertedAmount；外部公式失敗時 Rollback。
6. 以 Bonus Status 的完整任務資料建立 Bonus History。
7. 依 PCS-01 第 14 節重置 Bonus Status。
8. Commit 後回傳結案結果；DBCache 才能清空 Bonus Wallet 或發放 Converted Amount。

`UK_PromotionBonusHistory_BonusTaskId` 保證同一任務不會因重試或同時執行建立兩筆歷史。若新增遇到唯一鍵衝突，必須讀取既有歷史並確認內容，不得將其他 SQL 錯誤一律當成成功。

---

## 13. 每日維護交易

每日維護以本次計算出的 CurrentBusinessDay 與 HistoryCutoffTime 為固定輸入；一次執行期間不得因跨秒或跨日重新計算界線。

### 13.1 Eligibility Entry 逾期

- 依 UserUID、EligibilityEntryId 遞增分批處理 `Status = Available AND BusinessDay < CurrentBusinessDay`。
- 每位玩家先鎖定 Bonus Status，再將仍為 Available 的舊資格更新為 Expired。
- 重複執行時，已為 Claimed、Excluded 或 Expired 的資料不變。

### 13.2 進行中任務逾期

- 依 UserUID 遞增找出 `TaskState = InProgress AND BusinessDay < CurrentBusinessDay`。
- 每位玩家各自執行一個第 12 節結案交易，CloseReason 使用 `BusinessDayExpired`。
- 單筆失敗只 Rollback 該玩家，不得清除其 Bonus Status；其他玩家可繼續處理。
- 回傳清單以 BonusTaskId 去重，供 DBCache 清空 Bonus Wallet。

### 13.3 Bonus History 清理

使用參數化條件分批刪除：

```sql
DELETE FROM `PromotionBonusHistory`
WHERE `ClosedAt` < @HistoryCutoffTime
ORDER BY `BonusHistoryId`
LIMIT @BatchSize;
```

重複執行直到影響列數小於 BatchSize。此清理可獨立交易，不得與所有玩家逾期結案包成一個大型交易。

---

## 14. Deadlock 與重試原則

- MySQL Deadlock 或 Lock Wait Timeout(鎖定等待逾時)必須回復整個交易，不得只重做最後一條 SQL。
- 核心服務只可針對 PCS-05 列為可重試的錯誤重做完整業務交易。
- 每次重試都必須重新讀取 Bonus Status、Eligibility Entry 與 Bonus History，不得沿用第一次的物件狀態。
- EventId、BonusTaskId、狀態條件與唯一索引共同保證重試安全。
- `MysqlAcess` 既有「失敗後重連並重送單條 SQL」行為不可用於新交易 API，避免 Commit 結果不明時重複寫入。

---

## 15. 資料保存與清理

| 資料表 | 第一版規則 |
|---|---|
| PromotionActivity | 不刪除；可停用或封存 |
| PromotionTriggerEvent | 不刪除，保留 EventId 冪等性 |
| EligibilityEntry | 不刪除，保留資格結果與領取稽核 |
| PromotionBonusStatus | 永久保留 |
| PromotionBonusHistory | 以 ClosedAt 為基準，超過 90 日刪除 |

若資料量要求必須清理 Promotion Trigger Event 或 Eligibility Entry，應先另行確認 EventId 重送期限與稽核保存期限；這不是單純索引或維運調整。

---

## 16. 交叉引用

- 名詞、列舉、欄位與 Activity Snapshot：見 [PCS-01](./01_優惠活動資料模型與欄位定義.md)。
- Repository 介面、交易 Context 與 `MysqlAcess` 擴充：見 [PCS-03](./03_核心專用資料存取介面與MysqlAcess擴充規格.md)。
- API 的預期 BonusTaskId 與維護輸入：見 [PCS-04](./04_核心服務API規格.md)。
- Deadlock、Commit 不確定與重試次數：見 [PCS-05](./05_錯誤碼重試及交易一致性規格.md)。
- Schema、唯一鍵、併發領取及維護測試：見 [PCS-06](./06_單元測試與整合測試案例.md)。
