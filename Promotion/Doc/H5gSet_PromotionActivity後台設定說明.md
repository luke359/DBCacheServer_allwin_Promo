# H5gSet PromotionActivity 後台設定說明

## 1. 文件目的

本文件提供 H5gSet(活動設定後台)團隊建立、編輯、停用及封存 Promotion Activity(優惠活動設定)時使用，說明 `PromotionActivity` 資料表每個欄位的用途、預設值、輸入範圍及跨欄位驗證規則。

本文件只涵蓋 H5gSet 負責的活動設定，不涵蓋玩家資格、Bonus Task(紅利任務)、錢包或活動歷史資料。

規格依據：

- [優惠活動核心服務企劃](./優惠活動核心服務企劃.md)第 7 節。
- [PCS-01 優惠活動資料模型與欄位定義](./01_優惠活動資料模型與欄位定義.md)第 5～8 節。
- [PCS-02 資料庫 Schema、唯一索引、交易及資料列鎖設計](./02_資料庫Schema與交易及資料列鎖設計.md)第 4～6 節。

---

## 2. H5gSet 責任與重要行為

- H5gSet 負責建立、更新、停用及封存活動，以及在寫入前完成所有欄位與跨欄位驗證。
- 核心服務仍會在讀取資料時驗證完整性；若 H5gSet 寫入非法設定，核心服務會視為 Data Corruption(資料毀損)，不會自行套用預設值修復。
- `PromotionActivity` 保存「目前設定」，設定可以更新。
- 玩家成功領取時，核心服務會將當下設定保存為 Activity Snapshot(活動快照)。已開始的 Bonus Task 使用該快照，不受後續設定更新影響。
- 活動不得實體刪除。需要停用時，將 `WeekdayMask` 設為 `0000000`。
- 停用只會阻止產生新的 Eligibility Entry(可領活動項目)；停用前已建立的資格仍可被查詢及領取，領取時會使用當時最新的活動設定建立快照。
- 封存時將 `ActivityStatus` 設為 `2`（`Archived`）。封存活動不再產生新資格；封存前已建立且仍有效的資格仍可被查詢及領取。
- 所有 SQL 值必須使用參數，不得直接拼接使用者輸入。

---

## 3. 資料庫共通規則

| 項目 | 規則 |
|---|---|
| 資料表名稱 | `PromotionActivity` |
| Storage Engine(儲存引擎) | `InnoDB` |
| Character Set(字元集) | `utf8mb4` |
| Collation(定序) | `utf8mb4_bin` |
| 整數活動設定欄位 | 資料庫使用 `INT`；只接受整數 |
| 押注流水比例 | 資料庫使用 `DECIMAL(20,4)`；不得經由浮點數轉換後再寫入 |
| 日期 | `DATE`，格式為 `yyyy-MM-dd` |
| 時間 | `DATETIME(6)`，使用系統本地時間，格式為 `yyyy-MM-dd HH:mm:ss.ffffff` |
| 大小寫 | `ExclusiveGroup` 使用 `utf8mb4_bin` 比對，因此區分大小寫 |
| 資料刪除 | 禁止刪除；使用 `WeekdayMask = '0000000'` 停用，或使用 `ActivityStatus = 2` 封存 |

企劃標示為整數的活動設定欄位在資料庫以 `INT` 保存，H5gSet 也只能接受整數。只有允許小數的 `WagerContributionRate` 使用 `DECIMAL(20,4)`。

---

## 4. 建立活動時的預設值

| 後台設定 | 資料庫欄位 | 預設值 |
|---|---|---:|
| 活動 UID | `ActivityUID` | 建立時產生，不提供固定預設值 |
| 活動資訊 | `ActivityInfo` | `NULL` |
| 優惠類型 | `BonusType` | `1`：FixedAmount |
| 固定金額 | `FixedBonusAmount` | `10` |
| 優惠最高金額 | `MaxBonusAmount` | `10` |
| 儲值百分比 | `DepositPercentage` | `30` |
| 最低儲值金額 | `MinimumDepositAmount` | `10`；Free 活動使用 `NULL` |
| 洗碼量 | `WagerMultiplier` | `30` |
| 目標流水計算方式 | `WagerCalculationType` | `1`：BonusOnly |
| 押注上限 | `MaxBetAmount` | `5`；可改為 `NULL` 表示無限制 |
| 可玩遊戲列表 | `GameServerList` | `NULL` |
| 押注流水比例 | `WagerContributionRate` | `100` |
| 解鎖金類型 | `ConvertType` | `1`：Fixed |
| 固定解鎖金 | `FixedConvertedAmount` | `5` |
| 依餘額解鎖金上限 | `MaxBalanceConvertedAmount` | `150`；可改為 `NULL` 表示無限制 |
| 領取次數 | `DailyClaimLimit` | `1` |
| 活動開始日期 | `StartDate` | `NULL` |
| 活動結束日期 | `EndDate` | `NULL` |
| 星期遮罩 | `WeekdayMask` | `0000000`，即預設停用 |
| 活動狀態 | `ActivityStatus` | `1`：Active |
| 觸發類型 | `TriggerType` | `3`：FirstDepositOfBusinessDay |
| 不可疊加 | `IsNonStackable` | `0`：false |
| 優惠互斥群組 | `ExclusiveGroup` | `NULL` |

`CreatedAt` 與 `UpdatedAt` 沒有資料庫預設值，新增資料時必須由 H5gSet 明確寫入。

---

## 5. 列舉值

### 5.1 BonusType(優惠類型)

| 資料庫值 | 名稱 | 說明 |
|---:|---|---|
| `1` | `FixedAmount` | 領取時派發 `FixedBonusAmount` |
| `2` | `DepositPercentage` | 以合格儲值金額乘上 `DepositPercentage` 計算，並受 `MaxBonusAmount` 限制 |

### 5.2 ConvertType(解鎖金類型)

| 資料庫值 | 名稱 | 說明 |
|---:|---|---|
| `1` | `Fixed` | 達標後發放 `FixedConvertedAmount` |
| `2` | `Balance` | 由外部公式計算；`MaxBalanceConvertedAmount` 有值時不得超過上限 |

### 5.2.1 WagerCalculationType(目標流水計算方式)

| 資料庫值 | 名稱 | 說明 |
|---:|---|---|
| `1` | `BonusOnly` | `BonusAmount × WagerMultiplier` |
| `2` | `DepositAndBonus` | `(EligibleDepositAmount + BonusAmount) × WagerMultiplier`；沒有儲金時以 0 計 |

### 5.3 TriggerType(觸發類型)

| 資料庫值 | 名稱 | 後台顯示名稱 |
|---:|---|---|
| `1` | `Registration` | 註冊 |
| `2` | `FirstLoginOfBusinessDay` | 每日首登入 |
| `3` | `FirstDepositOfBusinessDay` | 每日首儲 |
| `4` | `Deposit` | 儲值 |
| `5` | `Free` | 免費活動 |

### 5.4 ActivityStatus(活動狀態)

| 資料庫值 | 名稱 | 說明 |
|---:|---|---|
| `1` | `Active` | 可依日期、WeekdayMask 及其他條件產生新資格 |
| `2` | `Archived` | 已封存，不再產生新資格；既有有效資格仍可領取 |

不得保存表格以外的列舉值。

---

## 6. 欄位說明與輸入範圍

### 6.1 識別與顯示欄位

| 欄位 | 資料庫型別 | 必填 | 功能與輸入規則 |
|---|---|---:|---|
| `ActivityUID` | `BIGINT` | 是 | 活動唯一識別碼。對應 C# `long`，資料庫技術範圍為 -9,223,372,036,854,775,808～9,223,372,036,854,775,807。業務上由時間值加衝突序號產生；建立後不可修改，且不是 `AUTO_INCREMENT`。後台新增畫面可先顯示為未產生，但寫入資料庫前必須取得唯一值。 |
| `ActivityInfo` | `VARCHAR(100)` | 否 | 顯示給玩家的活動名稱或說明，最多 100 字元。未設定時保存 `NULL`。 |

### 6.2 Bonus 派發欄位

| 欄位 | 資料庫型別 | 必填 | H5gSet 輸入限制 | 功能 |
|---|---|---:|---|---|
| `BonusType` | `TINYINT UNSIGNED` | 是 | `1` 或 `2` | 決定使用固定金額或儲值百分比計算 Bonus。 |
| `FixedBonusAmount` | `INT` | 是 | 整數 `1～100` | `BonusType = 1` 時實際派發的 Bonus。即使選擇百分比類型，因資料庫欄位不可為空，仍須保存合法值。 |
| `MaxBonusAmount` | `INT` | 是 | 整數 `1～100` | `BonusType = 2` 時的 Bonus 上限。即使選擇固定金額類型，仍須保存合法值。 |
| `DepositPercentage` | `INT` | 是 | 整數 `1～1000`，數值代表百分比 | `BonusType = 2` 時的計算比例。例如 `30` 代表 30%，不是 0.3。即使選擇固定金額類型，仍須保存合法值。 |
| `MinimumDepositAmount` | `INT` | 視條件 | 儲值型活動使用整數 `10～1000`；Free 活動必須為 `NULL` | 每日首儲或儲值事件必須達到此金額才符合活動。Registration 與 FirstLoginOfBusinessDay 不使用此值，建議保存 `NULL`。 |

百分比 Bonus 的計算方式：

```text
原始 Bonus = 合格儲值金額 × DepositPercentage ÷ 100
BonusAmount = Min(原始 Bonus 截斷至小數第 4 位, MaxBonusAmount)
```

### 6.3 流水與押注欄位

| 欄位 | 資料庫型別 | 必填 | H5gSet 輸入限制 | 功能 |
|---|---|---:|---|---|
| `WagerMultiplier` | `INT` | 是 | 整數 `1～100` | 洗碼量；依 `WagerCalculationType` 計算目標流水量，結果直接截斷至小數第 4 位。 |
| `WagerCalculationType` | `TINYINT UNSIGNED` | 是 | `1` 或 `2` | 決定目標流水量的計算基數。 |
| `MaxBetAmount` | `INT` | 否 | `NULL` 或整數 `1～100` | `NULL` 表示無限制；有值時由 GameServer(遊戲伺服器)執行單局押注上限，核心僅保存及提供設定。 |
| `GameServerList` | `VARCHAR(500)` | 否 | `NULL` 或最長 500 字元的字串 | 優惠可玩遊戲列表；核心僅將完整字串原樣提供 Host 查詢，不解字串或判斷遊戲資格。格式與判斷由 Host 負責。 |
| `WagerContributionRate` | `DECIMAL(20,4)` | 是 | `1.0～100.0`，可含小數，入庫最多 4 位小數 | 每局押注可計入流水的百分比。例如 `50` 代表押注額的 50%。 |

有效流水的計算方式：

```text
本局有效流水 = 本局押注 × WagerContributionRate ÷ 100
```

結果直接截斷至小數第 4 位，不四捨五入。

### 6.4 解鎖金欄位

| 欄位 | 資料庫型別 | 必填 | H5gSet 輸入限制 | 功能 |
|---|---|---:|---|---|
| `ConvertType` | `TINYINT UNSIGNED` | 是 | `1` 或 `2` | 決定完成流水後使用固定解鎖金或依餘額公式計算。 |
| `FixedConvertedAmount` | `INT` | 是 | 整數 `1～1000` | `ConvertType = 1` 時達標後發放的金額。即使選擇 Balance，仍須保存合法值。 |
| `MaxBalanceConvertedAmount` | `INT` | 否 | `NULL` 或整數 `1～1000` | `NULL` 表示無限制；有值且 `ConvertType = 2` 時作為外部公式結果上限。 |

### 6.5 有效期間與領取欄位

| 欄位 | 資料庫型別 | 必填 | H5gSet 輸入限制 | 功能 |
|---|---|---:|---|---|
| `DailyClaimLimit` | `INT` | 是 | 整數 `1～99` | 每位玩家在每個 Business Day(活動日)成功領取本活動的次數上限。失敗的領取不計次數。 |
| `StartDate` | `DATE` | 否 | 合法日期或 `NULL` | 活動從此 Business Day 起生效，包含該日；`NULL` 表示不限制開始日期。 |
| `EndDate` | `DATE` | 否 | 合法日期或 `NULL` | 活動從此 Business Day 起停止，**不包含該日**；`NULL` 表示無限期。 |
| `WeekdayMask` | `CHAR(7)` | 是 | 長度必須剛好為 7，且只能包含 `0`、`1` | 由左至右依序代表星期一至星期日；`1` 表示有效，`0` 表示無效。`0000000` 表示停用。 |
| `ActivityStatus` | `TINYINT UNSIGNED` | 是 | `1` 或 `2` | `1` 表示 Active；`2` 表示 Archived。封存與 WeekdayMask 停用是不同概念。 |
| `TriggerType` | `TINYINT UNSIGNED` | 是 | `1～5` 的已定義列舉值 | 指定哪一種事件可以產生本活動的可領資格。 |

當 `StartDate` 與 `EndDate` 都有值時，必須符合 `StartDate < EndDate`。

`WeekdayMask` 範例：

| 值 | 意義 |
|---|---|
| `1111111` | 每天有效 |
| `1111100` | 星期一至星期五有效 |
| `0000011` | 星期六、星期日有效 |
| `1000000` | 只在星期一有效 |
| `0000000` | 停用，不產生新資格 |

### 6.6 疊加、互斥及時間欄位

| 欄位 | 資料庫型別 | 必填 | H5gSet 輸入限制 | 功能 |
|---|---|---:|---|---|
| `IsNonStackable` | `TINYINT UNSIGNED` | 是 | `0` 或 `1` | `1` 代表不可疊加、`0` 代表可疊加。此欄位控制該活動是否會與其他同樣設為不可疊加的可領資格互相排除，不代表玩家可以同時進行多個 Bonus Task。 |
| `ExclusiveGroup` | `VARCHAR(64)` | 否 | `NULL` 或最長 64 字元的非空群組名稱 | 同一非 `NULL` 群組內只能成功領取一個活動。群組名稱區分大小寫；例如 `VIP` 與 `vip` 是不同群組。無群組時應保存 `NULL`，不建議使用空字串。 |
| `CreatedAt` | `DATETIME(6)` | 是 | 合法系統本地時間 | 建立時間；建立後不可修改。 |
| `UpdatedAt` | `DATETIME(6)` | 是 | 合法系統本地時間 | 最後更新時間；每次實際修改活動時更新。 |

`IsNonStackable` 與 `ExclusiveGroup` 是兩套同時生效的規則。優惠互斥群組不受 `IsNonStackable` 影響；同群組的其他未領資格仍會被排除。

---

## 7. 跨欄位驗證規則

| 編號 | 條件 | H5gSet 必須執行的驗證 |
|---|---|---|
| V01 | `BonusType = 2` | `TriggerType` 只能為 `3`（每日首儲）或 `4`（儲值）。禁止搭配註冊、每日首登入或免費活動。 |
| V02 | `TriggerType = 3` 或 `4` | `MinimumDepositAmount` 必須有值，且為整數 `10～1000`。 |
| V03 | `TriggerType = 5` | `MinimumDepositAmount` 必須為 `NULL`。 |
| V04 | `StartDate`、`EndDate` 都有值 | 必須符合 `StartDate < EndDate`。 |
| V05 | 新增活動 | 所有資料庫必填欄位、`CreatedAt`、`UpdatedAt` 及唯一 `ActivityUID` 都必須有值。 |
| V06 | 更新活動 | 不得變更 `ActivityUID` 或 `CreatedAt`；必須更新 `UpdatedAt`。 |
| V07 | 停用活動 | 只能將 `WeekdayMask` 設為 `0000000`，不得刪除資料列。 |
| V08 | 封存活動 | 將 `ActivityStatus` 設為 `2`，不得刪除資料列；封存不取消既有資格。 |
| V09 | 所有列舉及布林值 | 只能保存本文件列出的數值，不得接受未知數值。 |
| V10 | 未被目前類型使用的非空欄位 | 資料庫 `NOT NULL` 欄位仍須保存合法值；`MaxBetAmount`、`MaxBalanceConvertedAmount` 可保存 `NULL`，表示無限制。 |

後台畫面可以依 `BonusType` 或 `ConvertType` 將暫時不使用的欄位設為唯讀或收合，但送出時仍須帶入合法值。

---

## 8. 活動生效條件

一筆活動必須同時符合下列條件，核心服務才會在觸發事件發生時建立新的可領資格：

1. `ActivityStatus = 1`（`Active`）。
2. `WeekdayMask` 對應目前 Business Day 的位置為 `1`。
3. `WeekdayMask` 不等於 `0000000`。
4. Business Day 不早於 `StartDate`；`StartDate = NULL` 時不限制。
5. Business Day 早於 `EndDate`；`EndDate = NULL` 時不限制。
6. 收到的事件類型等於 `TriggerType`。
7. 儲值型事件的合格儲值金額不小於 `MinimumDepositAmount`。
8. 玩家在該 Business Day 已成功領取本活動的次數小於 `DailyClaimLimit`。
9. 活動在 Host(主程式)提供的代理商可參加活動範圍內。

第 9 項不是 `PromotionActivity` 欄位，由 Host 另外控管。

---

## 9. 建議的後台操作限制

- 新增畫面依第 4 節填入預設值；由於預設 `WeekdayMask = 0000000`，新活動建立後預設為停用。
- `ActivityUID` 建立後顯示為唯讀。
- 編輯送出前顯示活動生效日期採「開始日包含、結束日不包含」，避免操作人員誤解。
- 停用操作應顯示提示：「只停止建立新資格；既有可領資格不會自動取消。」
- 封存操作應顯示提示：「封存後不再建立新資格；既有可領資格不會自動取消。」封存活動預設不顯示於日常活動列表，另提供封存篩選條件。
- 修改金額、流水或解鎖設定時應顯示提示：「已開始的任務不受影響；尚未領取的既有資格在領取時會使用最新設定。」
- `ExclusiveGroup` 建議由可管理的群組清單選取，避免因大小寫或拼字差異形成不同群組。
- 寫入成功後重新讀取資料並顯示實際保存值，便於確認日期、四位小數與時間精度。
