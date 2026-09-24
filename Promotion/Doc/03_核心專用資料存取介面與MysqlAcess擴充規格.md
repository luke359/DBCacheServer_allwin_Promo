# PCS-03 核心專用資料存取介面與 MysqlAcess 擴充規格

## 1. 文件資訊

| 項目 | 內容 |
|---|---|
| 文件編號 | PCS-03 |
| 文件名稱 | 核心專用資料存取介面與 `MysqlAcess` 擴充規格 |
| 文件狀態 | 第一版完成 |
| 上游依據 | [優惠活動核心服務企劃](./優惠活動核心服務企劃.md)、[PCS-01](./01_優惠活動資料模型與欄位定義.md)、[PCS-02](./02_資料庫Schema與交易及資料列鎖設計.md)、[MysqlAcess 使用說明](./MysqlAcess使用說明.md) |
| 文件目的 | 隔離核心業務與資料庫細節，並定義第一版必須補足的安全參數化與交易能力 |

---

## 2. 文件目錄

1. 文件資訊
2. 文件目錄
3. 文件集合
4. 設計目標與分層
5. 核心專用資料存取介面
6. 介面操作契約
7. 領域模型與資料表對照
8. `MysqlAcess` 現況差距
9. `MysqlAcess` 擴充介面
10. 參數與查詢模型
11. 交易 Context 行為
12. 錯誤與例外契約
13. 格式、NULL 與映射規則
14. 安全限制
15. 實作與驗收清單
16. 交叉引用

---

## 3. 文件集合

| 編號 | 文件 | 狀態 |
|---|---|---|
| PCS-01 | [優惠活動資料模型與欄位定義](./01_優惠活動資料模型與欄位定義.md) | 第一版完成，共用名詞基準 |
| PCS-02 | [資料庫 Schema、唯一索引、交易及資料列鎖設計](./02_資料庫Schema與交易及資料列鎖設計.md) | 第一版完成 |
| PCS-03 | 本文件 | 第一版完成 |
| PCS-04 | [核心服務 API 規格](./04_核心服務API規格.md) | 第一版完成 |
| PCS-05 | [錯誤碼、重試及交易一致性規格](./05_錯誤碼重試及交易一致性規格.md) | 第一版完成 |
| PCS-06 | [單元測試與整合測試案例](./06_單元測試與整合測試案例.md) | 第一版完成 |
| PCS-07 | [核心服務實作與整合工作指引](./07_核心服務實作與整合工作指引.md) | 第一版完成（第四批） |

名詞、列舉及欄位語意以 PCS-01 為準；實體表、索引、交易順序與鎖定規則以 PCS-02 為準。

---

## 4. 設計目標與分層

```text
核心業務邏輯
    ↓ 只依賴 IPromotionDataStore / IPromotionDataTransaction
核心專用 MySQL Adapter
    ↓ 只傳入程式常數定義的表名、欄名與查詢結構
MysqlAcess 新增的型別化參數與交易 API
    ↓
MySQL InnoDB
```

必須符合下列限制：

- 核心業務邏輯不得直接操作資料表名稱、欄位名稱、SQL 或 `Dictionary<string, string>`。
- 核心專用 Adapter(轉接器)負責領域模型與資料列互轉、欄位白名單及資料完整性驗證。
- `MysqlAcess` 繼續負責連線、Command、Parameter 與 Transaction。依已核准的 V2 調整，每次 V2 呼叫使用獨立 Connection，不共用舊 API 的 Connection／Mutex；單次 Transaction 內的所有 Command 仍使用同一 Connection／Transaction。
- 跨程序的玩家鎖只由 MySQL `SELECT ... FOR UPDATE` 提供；程序內 Mutex 不能代替。
- 第一版沿用同步呼叫模型，不要求另行導入 ORM(Object-Relational Mapping，物件關聯映射)或新連線元件。

---

## 5. 核心專用資料存取介面

以下為 C# .NET 6 邏輯契約。實作可依專案命名空間調整型別所在檔案，但不得改變交易與資料語意。

### 5.1 交易入口

```csharp
public interface IPromotionDataStore
{
    T ExecuteInTransaction<T>(
        Func<IPromotionDataTransaction, T> action);

    IReadOnlyList<PromotionActivity> GetActivitiesByTriggerType(
        TriggerType triggerType,
        IReadOnlyCollection<long> allowedActivityUids);

    PromotionActivity? GetActivity(long activityUid);

    IReadOnlyList<EligibilityEntry> GetAvailableEligibilityEntries(
        long userUid,
        DateOnly businessDay);

    BonusStatus? GetBonusStatus(long userUid);

    IReadOnlyList<BonusHistory> GetBonusHistory(
        long userUid,
        DateTime fromInclusive,
        DateTime toExclusive,
        int offset,
        int limit);

    IReadOnlyList<long> GetUsersWithExpiredEligibility(
        DateOnly currentBusinessDay,
        long afterUserUid,
        int batchSize);

    IReadOnlyList<long> GetUsersWithExpiredTasks(
        DateOnly currentBusinessDay,
        long afterUserUid,
        int batchSize);

    int DeleteExpiredBonusHistory(
        DateTime cutoffExclusive,
        int batchSize);
}
```

`GetActivitiesByTriggerType` 映射並驗證符合 Trigger Type 與 UID 範圍的候選活動；核心的 `ActivityEligibilityEvaluator` 只採用 `ActivityStatus = Active` 的活動，避免非法狀態被 SQL 條件靜默忽略。`GetActivity` 依 UID 讀取目前設定時不排除 `Archived`，確保封存前已建立且仍有效的 Eligibility Entry 可以完成領取。

非交易查詢只供畫面查詢與維護候選清單使用。任何根據查詢結果進行異動的流程，都必須進入 `ExecuteInTransaction` 後重新讀取並驗證。
`GetActivity` 亦供 Host 查詢目前活動的 `GameServerList`；Adapter 原樣映射最長 500 字元的字串或 `null`，不解析內容。兩個可為空上限須映射為 `int?`，`WagerCalculationType` 須驗證為 1 或 2。

### 5.2 交易內操作

```csharp
public interface IPromotionDataTransaction
{
    BonusStatus GetOrCreateBonusStatusForUpdate(
        long userUid,
        DateTime operationTime);

    PromotionActivity? GetActivity(long activityUid);

    IReadOnlyList<PromotionActivity> GetActivitiesByTriggerType(
        TriggerType triggerType,
        IReadOnlyCollection<long> allowedActivityUids);

    PromotionTriggerEvent? GetTriggerEvent(string eventId);

    IReadOnlyList<EligibilityEntry> GetEligibilityEntriesByEventId(
        string eventId);

    void InsertTriggerEvent(PromotionTriggerEvent triggerEvent);

    IReadOnlyList<EligibilityEntry> InsertEligibilityEntries(
        IReadOnlyList<NewEligibilityEntry> entries);

    EligibilityEntry? GetEligibilityEntryForUpdate(
        long eligibilityEntryId);

    int CountClaimedEligibility(
        long userUid,
        long activityUid,
        DateOnly businessDay);

    void MarkEligibilityClaimed(
        long eligibilityEntryId,
        string bonusTaskId,
        DateTime changedAt);

    int ExcludeEligibilityEntries(
        long userUid,
        DateOnly businessDay,
        long claimedEligibilityEntryId,
        string bonusTaskId,
        bool excludeNonStackable,
        string? exclusiveGroup,
        DateTime changedAt);

    int ExpireAvailableEligibilityEntries(
        long userUid,
        DateOnly currentBusinessDay,
        DateTime changedAt);

    void SaveBonusStatus(BonusStatus bonusStatus);

    BonusHistory? GetBonusHistoryByTaskId(string bonusTaskId);

    BonusHistory InsertBonusHistory(NewBonusHistory history);
}
```

`IPromotionDataTransaction` 的實例只在傳入 `ExecuteInTransaction` 的委派執行期間有效，不得保存至欄位、跨執行緒使用或在交易結束後呼叫。

`NewEligibilityEntry` 包含 PCS-01 Eligibility Entry 除 `EligibilityEntryId` 外的所有新增欄位；`NewBonusHistory` 包含 PCS-01 Bonus History 除 `BonusHistoryId` 外的所有新增欄位。新增方法必須使用 `LastInsertedId` 建立並回傳含資料庫識別碼的完整模型，不得以 `0` 表示尚未取得識別碼。

---

## 6. 介面操作契約

### 6.1 ExecuteInTransaction

- 使用 `READ COMMITTED` 開始交易。
- `action` 正常回傳後明確 Commit，再將結果交給呼叫端。
- `action` 擲出任何例外時 Rollback，保留原始例外或包裝為可辨識的資料存取例外。
- Commit 失敗或結果不明時不得在原交易 Context 內重新呼叫 `action`；交由上層依 PCS-05 查證狀態。只有查證可證明未提交時，上層才可用新連線與新交易重新呼叫完整業務操作。
- 不得在交易內呼叫本介面的非交易查詢方法，避免使用另一條連線讀到不同狀態。

### 6.2 GetOrCreateBonusStatusForUpdate

- 以 UserUID 取得 PCS-02 規定的玩家級 Row Lock。
- 不存在時新增空白 Bonus Status；若發生主鍵競爭，重新查詢並鎖定既有列。
- 回傳前驗證 TaskState 與 nullable 任務欄位符合 PCS-01。

### 6.3 Promotion Trigger Event

- `GetTriggerEvent` 回傳 EventId 首次處理輸入。
- `InsertTriggerEvent` 遇 EventId 唯一鍵衝突時，擲出可辨識的 DuplicateKey(重複鍵)例外；上層只能查回既有事件，不可重新篩選活動。
- `GetEligibilityEntriesByEventId` 必須以 ActivityUID、EligibilityEntryId 的固定順序回傳，讓首次與重送結果排序一致。
- `InsertEligibilityEntries` 與來源 Promotion Trigger Event 必須使用同一交易。

### 6.4 Eligibility Entry

- `GetEligibilityEntryForUpdate` 必須產生 `SELECT ... FOR UPDATE`。
- `CountClaimedEligibility` 只計算 `Status = Claimed`，不得把失敗、Excluded 或 Expired 計入。
- `MarkEligibilityClaimed` 的更新條件必須同時包含 `EligibilityEntryId` 與 `Status = Available`；影響列數不是 1 時視為並發衝突。
- `ExcludeEligibilityEntries` 只允許將 Available 更新為 Excluded，依 EligibilityEntryId 固定順序處理。
- `ExpireAvailableEligibilityEntries` 只更新 `BusinessDay < currentBusinessDay` 且仍為 Available 的資料。

### 6.5 Bonus Status 與 Bonus History

- `SaveBonusStatus` 必須以 UserUID 與交易內已鎖定列更新，包含 `LastFirstLoginBusinessDay`、`LastFirstDepositBusinessDay`，禁止沒有 WHERE 的更新。
- 更新後需檢查影響列數。若資料內容相同導致 MySQL 回傳 0，Adapter 必須重新讀取並確認狀態，不可直接當成不存在。
- `InsertBonusHistory` 遇 BonusTaskId 重複時，只能在讀取既有歷史且內容符合後視為冪等成功。
- Bonus History 不提供一般更新方法。

### 6.6 維護查詢

- 候選玩家清單使用 Keyset Pagination(鍵集分頁)：`UserUID > afterUserUid ORDER BY UserUID LIMIT batchSize`，不得用不穩定的無排序分頁。
- 候選查詢結果不是鎖定承諾；逐位玩家進入交易後必須重新驗證。
- `DeleteExpiredBonusHistory` 只允許 `ClosedAt < cutoffExclusive`，依 BonusHistoryId 排序分批刪除。

---

## 7. 領域模型與資料表對照

| 領域模型 | 資料表 | Adapter 責任 |
|---|---|---|
| PromotionActivity | PromotionActivity | ActivityStatus 等列舉、日期、nullable 金額與 WeekdayMask 驗證 |
| PromotionTriggerEvent | PromotionTriggerEvent | EventId 長度、事件時間及 BusinessDay 映射 |
| EligibilityEntry | EligibilityEntry | 狀態列舉、nullable 任務識別碼與合法狀態組合驗證 |
| PromotionBonusStatus | BonusStatus | TaskState、任務欄位完整性、每日首登／首儲日期及 Activity Snapshot JSON 驗證 |
| PromotionBonusHistory | BonusHistory | CloseReason、不可變快照與金額欄位驗證 |

讀到未知列舉值、無法解析的日期／數值、非法 JSON、缺少必要欄位或不合法的欄位組合時，Adapter 必須擲出資料損毀例外，不得以預設值繼續業務流程。

---

## 8. `MysqlAcess` 現況差距

依 [MysqlAcess 使用說明](./MysqlAcess使用說明.md)，現有參數化 API 尚不足以完成 PCS-02：

| 需求 | 現況 | 第一版處理 |
|---|---|---|
| 多步驟 Transaction | 無公開交易 API | 新增交易 Context |
| `SELECT ... FOR UPDATE` | 不支援 | 新增 LockMode |
| 寫入 SQL `NULL` | `Dictionary<string,string>` 不允許 null | 新增型別化 nullable 參數 |
| `<`、`>`、`IN` 等條件 | 只有 AND + 相等 | 新增白名單條件模型 |
| `ORDER BY`、`LIMIT` | 不支援 | 新增型別化排序與限制 |
| 參數化 DELETE | 無 | 新增交易內與非交易分批 Delete |
| Count 查詢 | 無專用支援 | 新增 Scalar(純量)查詢 |
| Duplicate Key 判別 | 一般例外 | 新增穩定錯誤分類 |
| 整筆交易重試控制 | 單條 SQL 自動重連重試 | 新交易 API 禁止隱式重送 |

不得使用舊版 SQL 字串拼接方法補足上表缺口。

---

## 9. `MysqlAcess` 擴充介面

新增 API 必須與現有方法並存，不變更既有呼叫的簽名與行為。名稱可在實作時依類別結構微調，但能力與安全限制不得刪減。

```csharp
public sealed partial class MysqlAcess
{
    public T ExecuteParameterizedTransaction<T>(
        Func<MysqlTransactionContext, T> action,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    public IReadOnlyList<IReadOnlyDictionary<string, object?>>
        SelectParameterizedV2(MysqlSelectCommand command);

    public long DeleteParameterizedV2(MysqlDeleteCommand command);
}

public sealed class MysqlTransactionContext
{
    public MysqlParameterizedWriteResult Insert(
        string tableName,
        IReadOnlyDictionary<string, object?> data);

    public long Update(
        string tableName,
        IReadOnlyDictionary<string, object?> data,
        IReadOnlyList<MysqlCondition> where);

    public IReadOnlyList<IReadOnlyDictionary<string, object?>> Select(
        MysqlSelectCommand command);

    public object? ExecuteScalar(MysqlScalarCommand command);

    public long Delete(MysqlDeleteCommand command);
}
```

非交易 `SelectParameterizedV2` 供純查詢使用；任何業務異動都必須使用同一 `MysqlTransactionContext` 完成所有 Select、Insert、Update 與 Delete。

---

## 10. 參數與查詢模型

### 10.1 條件

```csharp
public enum MysqlComparisonOperator : byte
{
    Equal = 1,
    NotEqual = 2,
    LessThan = 3,
    LessThanOrEqual = 4,
    GreaterThan = 5,
    GreaterThanOrEqual = 6,
    In = 7,
    IsNull = 8,
    IsNotNull = 9
}

public sealed record MysqlCondition(
    string ColumnName,
    MysqlComparisonOperator Operator,
    object? Value);
```

- 多個條件第一版一律使用 `AND` 組合。
- `In` 的 Value 必須是非空的唯讀集合，每個元素各自綁定參數。
- 只有 `IsNull`、`IsNotNull` 允許 Value 為 null 且不建立值參數。
- 不支援任意 SQL 片段、子查詢、函式或呼叫端提供的運算子字串。

### 10.2 排序與鎖模式

```csharp
public enum MysqlSortDirection : byte
{
    Ascending = 1,
    Descending = 2
}

public sealed record MysqlOrder(
    string ColumnName,
    MysqlSortDirection Direction);

public enum MysqlLockMode : byte
{
    None = 0,
    ForUpdate = 1
}
```

`ForUpdate` 只能在有效的 `MysqlTransactionContext` 中使用；非交易查詢若指定鎖模式必須立即擲出 `ArgumentException`。

### 10.3 Select、Scalar 與 Delete

```csharp
public sealed record MysqlSelectCommand(
    string TableName,
    IReadOnlyList<string> Fields,
    IReadOnlyList<MysqlCondition> Where,
    IReadOnlyList<MysqlOrder> OrderBy,
    int? Limit,
    int? Offset,
    MysqlLockMode LockMode = MysqlLockMode.None);

public sealed record MysqlScalarCommand(
    string TableName,
    MysqlScalarOperation Operation,
    string? Field,
    IReadOnlyList<MysqlCondition> Where);

public enum MysqlScalarOperation : byte
{
    Count = 1
}

public sealed record MysqlDeleteCommand(
    string TableName,
    IReadOnlyList<MysqlCondition> Where,
    IReadOnlyList<MysqlOrder> OrderBy,
    int? Limit);
```

- `Fields` 為空可代表 `*`，但核心 Adapter 應列出明確欄位，避免 Schema 增欄改變映射。
- Update 與 Delete 的 Where 不可為空。
- Limit 必須大於 0；Offset 不得為負數。
- 表名、欄名沿用 `MysqlAcess` 既有英數字底線驗證，並由核心 Adapter 再做白名單限制。

---

## 11. 交易 Context 行為

`ExecuteParameterizedTransaction` 必須依序執行：

1. 建立並開啟本次 V2 呼叫專用 Connection；開啟失敗時不執行 action。
2. 交易開始前確認 Connection 可用；交易中不重連。
3. 以指定 Isolation Level 開始 MySQL Transaction。
4. 建立綁定該連線與 Transaction 的 `MysqlTransactionContext`。
5. 執行 action；Context 產生的每個 Command 都必須設定同一 Transaction。
6. action 成功時 Commit；Commit 完成後使 Context 失效。
7. action 或 Commit 失敗時嘗試 Rollback；保留原始失敗資訊。
8. Dispose Transaction 與本次專用 Connection。

禁止行為：

- 交易開始後因單條 Command 失敗而自行重連、重送該 Command 或建立新 Transaction。
- 巢狀呼叫 `ExecuteParameterizedTransaction`。
- 將 Context 帶出 action。
- 在 action 內呼叫舊 `MysqlAcess` API 或非交易 V2 API。
- 在未提交交易中提前對 DBCache 產生錢包異動指令。

V2 不使用舊 API 的共用 Connection 或 Mutex，因此舊 `ReConnect` 重新指派 Mutex 不會影響 V2 交易連線。跨程序一致性仍由 PCS-02 的 Row Lock 與唯一索引負責。由於舊 API 本體保持不變，V2 action 內混用舊 API 仍須由 Host 接線與程式審查禁止；舊 API 操作不會加入 V2 Transaction。

---

## 12. 錯誤與例外契約

資料存取層至少提供以下穩定分類，讓上層不必解析例外訊息文字：

```csharp
public enum PromotionDataErrorKind : byte
{
    Validation = 1,
    DuplicateKey = 2,
    Deadlock = 3,
    LockWaitTimeout = 4,
    Connection = 5,
    CommitOutcomeUnknown = 6,
    DataCorruption = 7,
    ConcurrencyConflict = 8,
    InvalidBonusTaskState = 9,
    Unexpected = 255
}

public sealed class PromotionDataException : Exception
{
    public PromotionDataErrorKind Kind { get; }
    public string? ConstraintName { get; }
}
```

- DuplicateKey 必須盡可能帶回 ConstraintName，讓 Adapter 區分 EventId、BonusTaskId 與其他唯一鍵。
- Deadlock 與 LockWaitTimeout 只表示整筆交易可由上層重新評估，不代表目前步驟可直接重送。
- CommitOutcomeUnknown 表示無法確定資料庫是否已提交，上層須先依業務識別碼查詢。
- DataCorruption 表示資料列不符合 PCS-01，禁止以預設值繼續。
- `InvalidBonusTaskState` 表示已提交的 BonusStatus 為 `Closing` 或任務欄位組合非法；非法 Snapshot 仍屬 `DataCorruption`。
- 對外錯誤碼與重試次數由 PCS-05 定義；本層不得直接回傳 UI 或 Host 錯誤碼。

---

## 13. 格式、NULL 與映射規則

### 13.1 型別化參數

V2 API 接受的值限定為：

- `string`
- `byte`、`short`、`int`、`long`
- `bool`
- `decimal`、`double`
- `DateTime`、`DateOnly`
- `Guid`
- `null`
- `In` 條件使用的上述型別集合

不在白名單中的型別立即擲出 `ArgumentException`。`null` 綁定為 `DBNull.Value`；不得以空字串代替 SQL `NULL`。

### 13.2 寫入

- 數字以實際數字型別綁定，不先轉為受地區影響的字串。
- `DateOnly` 以當日零時的資料庫 DATE 參數綁定，不套用時區轉換。
- `DateTime` 依系統本地時間寫入 `DATETIME(6)`，保留微秒精度。
- `Guid` 以小寫 `D` 格式字串寫入 CHAR(36)。
- Activity Snapshot 先依 PCS-01 序列化成 UTF-8 JSON 文字，再以 string 參數寫入。

### 13.3 讀取

- V2 API 保留資料庫 `NULL` 為 null，不沿用舊 API 將 NULL 轉為空字串的行為。
- Adapter 使用明確欄名讀取，並檢查必要欄位是否存在。
- decimal 不得經過 double 中轉。
- 未知列舉數值、溢位、無效日期與非法快照都轉為 DataCorruption。

舊 `InsertParameterized`、`UpdateParameterized`、`SelectParameterized` 的字串格式與 `CultureInfo.InvariantCulture` 規則保持不變；新核心 Adapter 不使用舊 API。

---

## 14. 安全限制

- 所有值都必須使用資料庫參數，不得插入 SQL 字串。
- 識別字只能通過既有英數字底線驗證，且核心 Adapter 必須使用固定白名單。
- `OrderBy`、`Operator`、`LockMode` 由 enum 產生固定 SQL token，不接受呼叫端字串。
- Update／Delete 無條件時必須拒絕執行。
- Limit 應設定實作上限，第一版核心批次建議最大 1000。
- 例外記錄不得輸出完整 Activity Snapshot、EventId 原始輸入或任何連線密碼；記錄識別碼時依系統日誌政策遮罩。
- SQL 日誌不得將參數值重新串回可執行 SQL。

---

## 15. 實作與驗收清單

實作階段需逐項完成：

- [ ] 新增型別化 nullable Parameter Binder(參數繫結器)。
- [ ] 新增條件、排序、Limit、Scalar 與 Delete Command Builder(命令建構器)。
- [ ] 新增 `ExecuteParameterizedTransaction` 與交易 Context。
- [ ] 新增 `SELECT ... FOR UPDATE`。
- [ ] 新增穩定的 DuplicateKey、Deadlock、LockWaitTimeout 與 CommitOutcomeUnknown 分類。
- [ ] 建立核心專用 Adapter 與 PCS-01 全欄位映射。
- [ ] 確認同一交易內所有 Command 共用連線與 Transaction。
- [ ] 確認交易失敗不執行單條 SQL 自動重送。
- [ ] 依 PCS-02 建立 Row Lock、唯一索引與分批清理整合測試。
- [ ] 修改 `MysqlAcess.cs` 時同步更新 [MysqlAcess 使用說明](./MysqlAcess使用說明.md)。

此清單是實作驗收條件，不代表本次文件工作已修改程式碼。

---

## 16. 交叉引用

- 共用名詞、欄位、列舉與重置規則：見 [PCS-01](./01_優惠活動資料模型與欄位定義.md)。
- Schema、唯一索引、交易流程與固定鎖定順序：見 [PCS-02](./02_資料庫Schema與交易及資料列鎖設計.md)。
- 公開 API 與 DTO：見 [PCS-04](./04_核心服務API規格.md)。
- 資料例外轉換、交易重試與 Commit 結果不明處理：見 [PCS-05](./05_錯誤碼重試及交易一致性規格.md)。
- Adapter、交易、Row Lock 與錯誤分類測試：見 [PCS-06](./06_單元測試與整合測試案例.md)。
