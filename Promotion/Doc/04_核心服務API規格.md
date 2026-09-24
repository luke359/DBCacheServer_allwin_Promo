# PCS-04 核心服務 API(Application Programming Interface，應用程式介面)規格

## 1. 文件資訊

| 項目 | 內容 |
|---|---|
| 文件編號 | PCS-04 |
| 文件名稱 | 核心服務 API 規格 |
| 文件狀態 | 第一版完成 |
| 適用技術 | C# .NET 6、同步 DLL 呼叫 |
| 上游依據 | [優惠活動核心服務企劃](./優惠活動核心服務企劃.md)、[PCS-01](./01_優惠活動資料模型與欄位定義.md)、[PCS-02](./02_資料庫Schema與交易及資料列鎖設計.md)、[PCS-03](./03_核心專用資料存取介面與MysqlAcess擴充規格.md) |
| 文件目的 | 定義核心服務提供給 DBCache 的公開方法、DTO、驗證及回傳契約 |

---

## 2. 文件目錄

1. 文件資訊
2. 文件目錄
3. 文件集合
4. 共用名詞與引用規則
5. API 設計原則
6. 初始化
7. 建立可領資格
8. 查詢可領優惠與遊戲列表
9. 領取優惠
10. 查詢任務狀態
11. 累計有效流水與達標待結案
12. Bonus 用盡結案
13. 玩家主動放棄
14. 每日維護
15. 查詢活動歷史
16. DTO 與列舉
17. 輸入驗證
18. 呼叫順序與範例
19. 版本相容性
20. 交叉引用

---

## 3. 文件集合

| 編號 | 文件 | 狀態 |
|---|---|---|
| PCS-01 | [優惠活動資料模型與欄位定義](./01_優惠活動資料模型與欄位定義.md) | 第一版完成，共用名詞基準 |
| PCS-02 | [資料庫 Schema、唯一索引、交易及資料列鎖設計](./02_資料庫Schema與交易及資料列鎖設計.md) | 第一版完成 |
| PCS-03 | [核心專用資料存取介面與 MysqlAcess 擴充規格](./03_核心專用資料存取介面與MysqlAcess擴充規格.md) | 第一版完成 |
| PCS-04 | 本文件 | 第一版完成 |
| PCS-05 | [錯誤碼、重試及交易一致性規格](./05_錯誤碼重試及交易一致性規格.md) | 第一版完成 |
| PCS-06 | [單元測試與整合測試案例](./06_單元測試與整合測試案例.md) | 第一版完成 |
| PCS-07 | [核心服務實作與整合工作指引](./07_核心服務實作與整合工作指引.md) | 第一版完成（第四批） |

---

## 4. 共用名詞與引用規則

- 共用名詞、領域列舉、欄位語意與 Activity Snapshot(活動設定快照)以 [PCS-01](./01_優惠活動資料模型與欄位定義.md) 為唯一基準。
- 本文件只定義對外 DTO(Data Transfer Object，資料傳輸物件)及服務行為，不改變 PCS-01 的領域語意。
- 交易邊界、Row Lock(資料列鎖)、固定鎖定順序及資料保存方式以 PCS-02 為準。
- API 實作只可依賴 PCS-03 的 `IPromotionDataStore` 與 `IPromotionDataTransaction`，不得直接組合 SQL。
- 所有會異動目前 Bonus Task(紅利任務)的 API 都必須傳入 `ExpectedBonusTaskId`；舊任務要求不得作用於新任務。
- 錯誤碼、可否重試及交易結果不明的 Host 行動以 PCS-05 為準。

---

## 5. API 設計原則

### 5.1 公開 Facade

第一版提供單一同步 Facade(外觀介面)：

```csharp
public interface IPromotionCoreService
{
    PromotionResult<InitializeData> Initialize(InitializeRequest request);

    PromotionResult<CreateEligibilityData> CreateEligibility(
        CreateEligibilityRequest request);

    PromotionResult<AvailablePromotionListData> GetAvailablePromotions(
        GetAvailablePromotionsRequest request);

    PromotionResult<GameServerListData> GetGameServerList(
        GetGameServerListRequest request);

    PromotionResult<ClaimPromotionData> ClaimPromotion(
        ClaimPromotionRequest request);

    PromotionResult<BonusTaskStatusData> GetBonusTaskStatus(
        GetBonusTaskStatusRequest request);

    PromotionResult<AccumulateWagerData> AccumulateWager(
        AccumulateWagerRequest request);

    PromotionResult<CloseBonusTaskData> CloseWagerCompletedBonusTask(
        CloseWagerCompletedBonusTaskRequest request);

    PromotionResult<CloseBonusTaskData> CloseDepletedBonusTask(
        CloseDepletedBonusTaskRequest request);

    PromotionResult<CloseBonusTaskData> AbandonBonusTask(
        AbandonBonusTaskRequest request);

    PromotionResult<DailyMaintenanceData> RunDailyMaintenance(
        DailyMaintenanceRequest request);

    PromotionResult<BonusHistoryPageData> GetBonusHistory(
        GetBonusHistoryRequest request);
}
```

Facade 由 Composition Root(組合根)注入 `IPromotionDataStore` 與 `ILocalClock`。`ILocalClock` 回傳系統本地時間，讓 `CreatedAt`、`UpdatedAt`、`ClaimedAt` 與 `ClosedAt` 可測試；不得以資料庫時間取代。

```csharp
public interface ILocalClock
{
    DateTime GetNow();
}
```

### 5.2 同步模型與執行緒安全

- 第一版所有方法均為同步呼叫，不提供 `Task` 或背景執行版本。
- Facade 可由多執行緒同時呼叫；初始化狀態的讀寫必須具備執行緒安全。
- 業務一致性不得依賴 Facade 的程序內鎖；玩家異動仍使用 PCS-02 的 MySQL Row Lock。
- `BalanceConvertFormula` 是同步委派，會在結案交易中執行；不得進行網路、資料庫、錢包操作或其他不可預期的阻塞作業。
- 公式必須為 Deterministic(決定性)、無副作用；Deadlock(死結)重試時可能以相同輸入呼叫多次。

### 5.3 共通 Result

```csharp
public sealed record PromotionResult<T>(
    PromotionResultKind Kind,
    PromotionErrorCode ErrorCode,
    bool IsRetryable,
    string CorrelationId,
    T? Data,
    PromotionErrorDetailsDto? ErrorDetails);

public sealed record PromotionErrorDetailsDto(
    string? ExistingEventId,
    long? ExistingUserUID,
    TriggerType? ExistingTriggerType,
    DateTime? ExistingEventTime,
    DateOnly? ExistingBusinessDay,
    string? ExistingBonusTaskId);

public enum PromotionResultKind : byte
{
    Succeeded = 1,
    Rejected = 2,
    RetryableFailure = 3,
    PermanentFailure = 4,
    OutcomeUnknown = 5,
    PartialSucceeded = 6
}
```

- `Succeeded` 時 `ErrorCode = None` 且 `Data` 必填。
- `PartialSucceeded` 只供每日維護使用；`Data` 必填並包含逐項失敗。
- `Rejected` 是已成功完成驗證、但目前業務狀態不允許操作；不得視為系統故障。
- `RetryableFailure` 代表本次未提交業務成功結果，Host 可依 PCS-05 重試。
- `OutcomeUnknown` 代表核心服務無法確認 Commit 結果；Host 不得把它當成失敗並直接重送。
- `PermanentFailure` 代表輸入、設定、資料損毀或非預期錯誤，需要修正或人工處理。
- `IsRetryable` 由 `ErrorCode` 固定推導，呼叫端不得自行覆寫。
- `CorrelationId` 不具冪等性，只用於跨元件追蹤。
- `ErrorDetails` 只提供結構化查證資訊；不得包含例外訊息、Stack Trace 或敏感資料。沒有安全細節時為 null。

### 5.4 時間與 Business Day

- `BusinessDayCutover` 在初始化時固定，允許 `00:00:00` 至小於 `24:00:00`，精度到秒。
- Business Day 計算方式為：先以本地時間減去切換時間，再取日期。
- 事件資格使用 `EventTime`；資格查詢使用 `QueryTime`；遊戲資料保留 `GameTime`；每日維護使用 `ExecutionTime`。
- 寫入時間由 `ILocalClock.GetNow()` 取得。Host 提供的事件時間、遊戲時間及維護時間不得冒充資料庫寫入時間。
- 同一次 API 呼叫只取得一次操作時間；同一交易與回傳 DTO 使用同一值。

### 5.5 數值型別與 double 邊界

- 公開 Request 的金額一律使用 `decimal`，不得提供同名 `double` overload(多載)。
- 相容舊系統的 Adapter 若收到 `double`，必須先拒絕 `NaN`、正負無限值，再以可檢查溢位的方式轉為 `decimal`；轉換失敗不得呼叫核心服務。
- 整數活動設定依 PCS-01 使用 `int`；參與金額運算時先精確轉換為 `decimal`。
- 核心計算、實際任務金額與 DTO 計算結果依 PCS-01 使用 `decimal`，結果截斷至小數第 4 位。
- 唯一允許的 `double` 是外部 `BalanceConvertFormula` 的回傳型別；驗證、轉換、截斷及上限處理由核心服務完成。

### 5.6 Wallet Instruction

核心服務不直接異動錢包，只在 Commit 成功或已查證既有成功結果後回傳 Wallet Instruction(錢包指令)：

```csharp
public sealed record WalletInstructionDto(
    string OperationKey,
    WalletAction Action,
    long UserUID,
    string BonusTaskId,
    decimal Amount);

public enum WalletAction : byte
{
    CreditBonusWallet = 1,
    ClearBonusWallet = 2,
    CreditMainWallet = 3
}
```

`OperationKey` 固定為 `{BonusTaskId}:{WalletAction}`。DBCache 必須以此鍵做冪等執行；`ClearBonusWallet` 的 `Amount` 固定為 0。第一版未使用 Transactional Outbox(交易寄件匣)，因此 Host 收到成功結果後必須先可靠保存 Wallet Instruction，再執行錢包異動。回應在 Host 保存前遺失的風險及人工對帳規則見 PCS-05。

---

## 6. 初始化

### 6.1 契約

```csharp
public delegate double BalanceConvertFormula(BalanceConvertContext context);

public sealed record InitializeRequest(
    TimeSpan BusinessDayCutover,
    BalanceConvertFormula BalanceConvertFormula,
    string? CorrelationId = null);

public sealed record InitializeData(
    TimeSpan BusinessDayCutover,
    int SnapshotVersion,
    bool WasAlreadyInitialized);
```

- `BalanceConvertFormula` 不得為 null。
- 第一次成功初始化後，切換時間與委派參考在 Facade 生命週期內不可變。
- 以相同切換時間及同一委派實例重複初始化，回傳成功且 `WasAlreadyInitialized = true`。
- 未初始化前呼叫其他 API，回傳 `ServiceNotInitialized`。
- 已初始化後傳入不同切換時間或不同委派實例，回傳 `InitializationConflict`；既有設定不得被替換。
- 初始化不開啟資料庫交易，也不測試呼叫公式。

### 6.2 Balance Convert Context

```csharp
public sealed record BalanceConvertContext(
    long UserUID,
    string BonusTaskId,
    long EligibilityEntryId,
    long ActivityUID,
    DateOnly BusinessDay,
    decimal BonusAmount,
    decimal RequiredWagerAmount,
    decimal CurrentWagerAmount,
    decimal RemainingWagerAmount,
    decimal SettledBonusWalletBalance,
    decimal GameBetAmount,
    decimal GameWinAmount,
    DateTime GameTime,
    ActivitySnapshotDto ActivitySnapshot);
```

此 Context 只在 `CloseWagerCompletedBonusTask` 結案且 Convert Type 為 `Balance` 時建立。內容是 `AccumulateWager` 已提交的最終任務狀態，以及 Host 傳入的達標局資料；公式不得保存可變參考或修改核心狀態。

---

## 7. 建立可領資格

### 7.1 Request 與 Result

```csharp
public sealed record CreateEligibilityRequest(
    string EventId,
    long UserUID,
    TriggerType TriggerType,
    DateTime EventTime,
    decimal? EligibleDepositAmount,
    IReadOnlyCollection<long> AllowedActivityUIDs,
    string? CorrelationId = null);

public sealed record CreateEligibilityData(
    string EventId,
    long UserUID,
    DateOnly BusinessDay,
    bool IsReplay,
    IReadOnlyList<EligibilityEntryDto> Entries);
```

### 7.2 行為

1. 依 `EventTime` 計算 Business Day。
2. 依 PCS-02 鎖定玩家並查詢 EventId。
3. EventId 不存在且 Trigger Type 為每日首登入／首儲時，比對 Bonus Status 中對應的最近觸發 Business Day。
4. 事件 Business Day 小於或等於已保存日期時仍保存本次 Promotion Trigger Event，回傳成功、`IsReplay = false` 及空 Entries，不讀取活動或建立資格；之後重送此 EventId 則回 `IsReplay = true`。此規則避免延遲的舊日事件讓日期倒退。
5. 尚未觸發或事件 Business Day 晚於保存日期時，於同一交易更新最近觸發 Business Day，再從 `ActivityStatus = Active` 的活動中依活動生效條件、最低儲值、已領取次數及 `AllowedActivityUIDs` 建立資格。每日首儲日期在第一筆成功儲值事件即更新，即使未達任何活動門檻或零活動符合亦不延後。
6. `AllowedActivityUIDs` 空集合代表 Host 不允許任何活動，成功建立零資格事件；不得解讀為全部活動。
7. EventId 已存在且首次輸入內容完全相同時，不重新篩選活動，回傳既有清單及 `IsReplay = true`。
8. EventId 已存在但 `UserUID`、`TriggerType`、`EventTime` 或 `EligibleDepositAmount` 任一不同時，回傳 `EventIdConflict`，並保留首次內容。
9. 首次建立零筆資格仍回傳成功、`IsReplay = false`，且保存 Promotion Trigger Event。
10. 清單固定依 `ActivityUID`、`EligibilityEntryId` 遞增。

`AllowedActivityUIDs` 只決定首次事件可篩選的活動；EventId 重送時不比較此集合，也不因集合改變而重算結果。
每日首登入／首儲判斷不依賴 `RunDailyMaintenance`；Business Day 晚於保存日期時自然可再次觸發。
活動封存不影響已建立的 Eligibility Entry；仍有效的既有資格可依原領取流程取得目前活動設定並建立快照。

---

## 8. 查詢可領優惠

```csharp
public sealed record GetAvailablePromotionsRequest(
    long UserUID,
    DateTime QueryTime,
    string? CorrelationId = null);

public sealed record AvailablePromotionListData(
    long UserUID,
    DateOnly BusinessDay,
    bool HasActiveBonusTask,
    string? ActiveBonusTaskId,
    IReadOnlyList<AvailablePromotionDto> Items);

public sealed record AvailablePromotionDto(
    long EligibilityEntryId,
    string EventId,
    long ActivityUID,
    DateOnly BusinessDay,
    TriggerType TriggerType,
    decimal? EligibleDepositAmount,
    string? ActivityInfo,
    decimal EstimatedBonusAmount,
    decimal EstimatedRequiredWagerAmount,
    int? MaxBetAmount,
    bool IsNonStackable,
    string? ExclusiveGroup);
```

- 只回傳 `Status = Available` 且 `BusinessDay` 等於 `QueryTime` 所屬 Business Day 的資格。
- 顯示資訊與預估金額使用查詢當下的 Promotion Activity；預估值不是領取承諾。
- 活動被停用或更新後，既有資格仍可顯示及領取；領取時再次讀取目前設定並建立快照。
- 有進行中任務時仍回傳尚有效資格，並透過 `HasActiveBonusTask` 告知目前不可領取。
- 清單依 `EligibilityEntryId` 遞增；沒有資料時回傳成功及空集合。

### 8.1 查詢活動可玩遊戲列表

```csharp
public sealed record GetGameServerListRequest(
    long ActivityUID,
    string? CorrelationId = null);

public sealed record GameServerListData(
    long ActivityUID,
    string? GameServerList);
```

- Host 依 `ActivityUID` 查詢目前 `PromotionActivity.GameServerList`；活動不存在時回傳 `ActivityNotFound`，`GameServerList = null` 則為有效查詢結果。
- 核心服務原樣回傳整個字串或 `null`，不拆解、不驗證遊戲代碼，也不以內容判斷遊戲是否允許。
- Host 負責解讀字串及遊戲資格判斷；資料來源為目前活動設定，不使用任務快照。

---

## 9. 領取優惠

```csharp
public sealed record ClaimPromotionRequest(
    long UserUID,
    long EligibilityEntryId,
    string? CorrelationId = null);

public sealed record ClaimPromotionData(
    long UserUID,
    long EligibilityEntryId,
    string BonusTaskId,
    long ActivityUID,
    DateOnly BusinessDay,
    decimal BonusAmount,
    decimal RequiredWagerAmount,
    decimal CurrentWagerAmount,
    decimal RemainingWagerAmount,
    int? MaxBetAmount,
    DateTime ClaimedAt,
    int SnapshotVersion,
    bool IsReplay,
    IReadOnlyList<WalletInstructionDto> WalletInstructions);
```

### 9.1 成功處理

- 依 PCS-02 領取交易重新驗證玩家、Business Day、資格狀態、目前任務、次數上限及互斥條件。
- 使用目前 Promotion Activity 產生版本 2 快照，依 `WagerCalculationType` 計算 Bonus 與 Required Wager Amount；沒有儲金時以 0 計。
- 成功 Commit 後回傳一筆 `CreditBonusWallet`；Amount 等於 `BonusAmount`。
- DBCache 必須可靠保存並完成該指令後，才能允許玩家開始使用任務。

### 9.2 重複呼叫

- 資格已由同一玩家領取，且可由目前 Bonus Status 或 90 日內 Bonus History 還原相同結果時，回傳成功及 `IsReplay = true`；不得建立新任務。
- 重播時回傳同一 BonusTaskId、金額與相同 `OperationKey` 的派發指令，DBCache 仍須做錢包冪等。
- 資格已領取但其任務資料已不在 Bonus Status，且歷史也已清理時，回傳 `ClaimReplayDataUnavailable`，並在 `ErrorDetails.ExistingBonusTaskId` 提供既有識別碼；不得使用目前活動設定重建金額。
- 資格已被排除或逾期時回傳對應業務拒絕；不得改回 Available。

---

## 10. 查詢任務狀態

```csharp
public sealed record GetBonusTaskStatusRequest(
    long UserUID,
    string? CorrelationId = null);

public sealed record BonusTaskStatusData(
    long UserUID,
    bool HasActiveTask,
    ActiveBonusTaskDto? Task);

public sealed record ActiveBonusTaskDto(
    string BonusTaskId,
    long EligibilityEntryId,
    long ActivityUID,
    DateOnly BusinessDay,
    decimal BonusAmount,
    decimal RequiredWagerAmount,
    decimal CurrentWagerAmount,
    decimal RemainingWagerAmount,
    int? MaxBetAmount,
    DateTime ClaimedAt,
    ActivitySnapshotDto ActivitySnapshot);
```

- Bonus Status 不存在或 `TaskState = None` 時回傳成功、`HasActiveTask = false`、`Task = null`。
- `RemainingWagerAmount` 即時計算為 `Max(RequiredWagerAmount - CurrentWagerAmount, 0)`。
- 已提交資料若長期停留在 `Closing` 或 TaskState 與任務欄位組合不合法，依 PCS-05 回傳 `InvalidBonusTaskState`，不得當作無任務；未知列舉、非法 Snapshot 或無法映射的資料列仍回傳 `DataCorruption`。

---

## 11. 累計有效流水與達標待結案

```csharp
public sealed record AccumulateWagerRequest(
    long UserUID,
    string ExpectedBonusTaskId,
    decimal GameBetAmount,
    decimal GameWinAmount,
    DateTime GameTime,
    decimal SettledBonusWalletBalance,
    string? CorrelationId = null);

public sealed record AccumulateWagerData(
    long UserUID,
    string BonusTaskId,
    decimal EffectiveWagerAmount,
    decimal CurrentWagerAmount,
    decimal RemainingWagerAmount,
    BonusTaskOutcome Outcome,
    CloseReason? CloseReason,
    decimal ConvertedAmount,
    DateTime? ClosedAt,
    IReadOnlyList<WalletInstructionDto> WalletInstructions);

public enum BonusTaskOutcome : byte
{
    InProgress = 1,
    Closed = 2,
    AlreadyClosed = 3,
    ReadyToClose = 4
}
```

`Closed = 2` 保留既有列舉值以維持契約相容性；新版 `AccumulateWager` 不再產生此結果，首次達標改回 `ReadyToClose`。

### 11.1 處理規則

1. 鎖定玩家後確認 `ExpectedBonusTaskId` 與目前任務相同。
2. 依快照計算 `EffectiveWagerAmount`，加入 Current Wager Amount。
3. 不論是否達標，均只在本交易保存最新流水，不建立 Bonus History、不重置 Bonus Status，也不回傳 Wallet Instruction。
4. 未達標回傳 `InProgress`；達標回傳 `ReadyToClose`，`RemainingWagerAmount = 0`。
5. Host 收到 `ReadyToClose` 後停止送入後續遊戲局，並以相同達標局資料呼叫 `CloseWagerCompletedBonusTask`。
6. 未達標但 `SettledBonusWalletBalance = 0` 時仍回傳 `InProgress`；Host 應在本次累計成功後呼叫 `CloseDepletedBonusTask`。

`GameWinAmount` 第一版不參與有效流水公式。`SettledBonusWalletBalance` 也不觸發本方法結案；兩者由 Host 原樣帶入後續結案方法，供 Balance Convert Context 與流程判斷使用。

### 11.2 重送責任

DBCache 保證每局最終資料只送一次，因此 Request 不含遊戲局冪等鍵。若核心明確回傳失敗且交易已 Rollback，可依 PCS-05 重試；若回傳 `OutcomeUnknown`，DBCache 不得重送本局，必須先查詢任務／歷史並人工對帳，避免流水重複累加。

### 11.3 流水達標結案

```csharp
public sealed record CloseWagerCompletedBonusTaskRequest(
    long UserUID,
    string ExpectedBonusTaskId,
    decimal SettledBonusWalletBalance,
    decimal GameBetAmount,
    decimal GameWinAmount,
    DateTime GameTime,
    string? CorrelationId = null);
```

- Host 只在 `AccumulateWager` 成功回傳 `ReadyToClose` 後呼叫；本方法不再增加流水。
- 核心鎖定任務並重新確認 `CurrentWagerAmount >= RequiredWagerAmount`；未達標回傳 `Rejected / WagerRequirementNotMet`。
- 以 `WagerCompleted` 結案。Convert Type 為 `Fixed` 時使用固定解鎖額；為 `Balance` 時以 Request 的餘額及達標局資料建立 Balance Convert Context。
- 成功時回傳 `ClearBonusWallet`；Converted Amount 大於 0 時再回傳 `CreditMainWallet`。
- 同 BonusTaskId 已結案時回傳既有歷史及 `AlreadyClosed = true`，可安全重送相同結案要求。

---

## 12. Bonus 用盡結案

```csharp
public sealed record CloseDepletedBonusTaskRequest(
    long UserUID,
    string ExpectedBonusTaskId,
    decimal SettledBonusWalletBalance,
    DateTime NotificationTime,
    string? CorrelationId = null);
```

- 只接受 `SettledBonusWalletBalance = 0`；大於 0 回傳 `BonusWalletNotDepleted`，小於 0 為輸入錯誤。
- 此方法不增加流水，以 `BonusDepleted`、`ConvertedAmount = 0` 結案。
- 若目前任務已達 Required Wager Amount：Convert Type 為 `Fixed` 時必須改以 `WagerCompleted` 結案；Convert Type 為 `Balance` 時仍以 `BonusDepleted` 結案，且不呼叫公式。
- 成功結果不回傳 `ClearBonusWallet`，因 Host 已確認餘額為 0。
- 同 BonusTaskId 已結案時回傳既有歷史及 `AlreadyClosed = true`；結案原因不因重送要求而改變。

---

## 13. 玩家主動放棄

```csharp
public sealed record AbandonBonusTaskRequest(
    long UserUID,
    string ExpectedBonusTaskId,
    DateTime RequestedAt,
    string? CorrelationId = null);

public sealed record CloseBonusTaskData(
    long UserUID,
    string BonusTaskId,
    CloseReason CloseReason,
    decimal FinalCurrentWagerAmount,
    decimal ConvertedAmount,
    DateTime ClosedAt,
    bool AlreadyClosed,
    IReadOnlyList<WalletInstructionDto> WalletInstructions);
```

- 任務進行中時以 `PlayerAbandoned` 結案，Converted Amount 固定為 0。
- Commit 後回傳一筆 `ClearBonusWallet`。
- 若 BonusTaskId 已有歷史，回傳既有結果及 `AlreadyClosed = true`；不得將既有結案原因改成主動放棄。
- 若目前已是另一個 BonusTaskId，回傳 `BonusTaskMismatch`，不得異動新任務。

---

## 14. 每日維護

```csharp
public sealed record DailyMaintenanceRequest(
    DateTime ExecutionTime,
    int BatchSize,
    string? CorrelationId = null);

public sealed record DailyMaintenanceData(
    DateOnly CurrentBusinessDay,
    DateTime HistoryCutoffTime,
    int ExpiredEligibilityCount,
    int ClosedExpiredTaskCount,
    int DeletedHistoryCount,
    IReadOnlyList<WalletInstructionDto> WalletInstructions,
    IReadOnlyList<MaintenanceFailureDto> Failures);

public sealed record MaintenanceFailureDto(
    MaintenanceStage Stage,
    long? UserUID,
    string? BonusTaskId,
    PromotionErrorCode ErrorCode,
    bool IsRetryable);

public enum MaintenanceStage : byte
{
    ExpireEligibility = 1,
    CloseExpiredTask = 2,
    DeleteHistory = 3
}
```

### 14.1 固定界線與批次

- 呼叫開始時由 `ExecutionTime` 計算一次 `CurrentBusinessDay`，並計算 `HistoryCutoffTime = ExecutionTime - 90 日`；整次執行不得重新計算。
- `BatchSize` 適用於候選查詢與歷史刪除，範圍 1～1000。
- 依 PCS-02 順序處理資格逾期、任務逾期、歷史清理。
- 各玩家逾期結案使用獨立交易；一位玩家失敗不回復其他玩家的成功結果。
- `BusinessDayExpired` 成功結案後，每個任務回傳一筆 `ClearBonusWallet`。
- Wallet Instructions 依 UserUID、BonusTaskId 排序並以 OperationKey 去重。

### 14.2 結果狀態

- 全部階段成功時回傳 `Succeeded`。
- 至少一項成功且至少一項失敗時回傳 `PartialSucceeded`、`MaintenancePartiallySucceeded` 及完整統計。
- 尚未完成任何異動即發生整體錯誤時，依錯誤類型回傳失敗。
- 重複執行只處理仍符合條件的資料，不重複建立歷史或轉換終態資格。
- 第一版未保存維護執行及 Wallet Instruction 的 Outbox；若成功回應在 Host 保存前遺失，重跑維護不保證重現上一批清空清單，必須依 PCS-05 人工對帳。

---

## 15. 查詢活動歷史

```csharp
public sealed record GetBonusHistoryRequest(
    long UserUID,
    DateTime FromInclusive,
    DateTime ToExclusive,
    int Offset,
    int Limit,
    string? CorrelationId = null);

public sealed record BonusHistoryPageData(
    long UserUID,
    DateTime FromInclusive,
    DateTime ToExclusive,
    int Offset,
    int Limit,
    IReadOnlyList<BonusHistoryDto> Items);
```

- `FromInclusive < ToExclusive`，查詢跨度不得超過 90 日，且 `ToExclusive` 不得晚於呼叫當下時間。
- `Offset` 不得小於 0；`Limit` 範圍為 1～200。
- 以 `ClosedAt` 降冪、`BonusHistoryId` 降冪排序後套用 Offset／Limit。
- 無資料時回傳成功及空集合。
- 此 API 不承諾回傳已被每日維護合法清除的紀錄。

---

## 16. DTO 與列舉

### 16.1 Eligibility Entry

```csharp
public sealed record EligibilityEntryDto(
    long EligibilityEntryId,
    string EventId,
    long UserUID,
    long ActivityUID,
    DateOnly BusinessDay,
    TriggerType TriggerType,
    decimal? EligibleDepositAmount,
    EligibilityStatus Status,
    string? ClaimedBonusTaskId,
    string? ExcludedByBonusTaskId,
    DateTime StatusChangedAt,
    DateTime CreatedAt);
```

### 16.2 Activity Snapshot

```csharp
public sealed record ActivitySnapshotDto(
    int SnapshotVersion,
    long ActivityUID,
    string? ActivityInfo,
    BonusType BonusType,
    int FixedBonusAmount,
    int MaxBonusAmount,
    int DepositPercentage,
    int? MinimumDepositAmount,
    int WagerMultiplier,
    WagerCalculationType WagerCalculationType,
    int? MaxBetAmount,
    decimal WagerContributionRate,
    ConvertType ConvertType,
    int FixedConvertedAmount,
    int? MaxBalanceConvertedAmount,
    int DailyClaimLimit,
    DateOnly? StartDate,
    DateOnly? EndDate,
    string WeekdayMask,
    TriggerType TriggerType,
    bool IsNonStackable,
    string? ExclusiveGroup,
    DateTime CapturedAt);
```

`WagerCalculationType` 使用 PCS-01 定義的 `byte enum`。新版快照固定使用 `SnapshotVersion = 2`；版本 1 仍依原欄位與原計算語意讀取，映射到 DTO 時其計算方式為 `BonusOnly`，兩個上限為原有整數值。`GameServerList` 不屬於任務快照。

### 16.3 Bonus History

```csharp
public sealed record BonusHistoryDto(
    long BonusHistoryId,
    string BonusTaskId,
    long UserUID,
    long EligibilityEntryId,
    long ActivityUID,
    DateOnly BusinessDay,
    ActivitySnapshotDto ActivitySnapshot,
    decimal BonusAmount,
    decimal RequiredWagerAmount,
    decimal CurrentWagerAmount,
    decimal ConvertedAmount,
    CloseReason CloseReason,
    DateTime ClaimedAt,
    DateTime ClosedAt);
```

### 16.4 DTO 共通限制

- 所有集合回傳非 null 唯讀集合；無資料使用空集合。
- DTO 不直接暴露資料庫 Connection、Transaction、資料列 Dictionary 或可變領域物件。
- API 專用 enum 的既有數值不得重新編號；未知數值不得靜默映射。
- `ActivitySnapshotJson` 不直接暴露給 Host，核心服務反序列化並驗證後回傳 `ActivitySnapshotDto`。
- Error Message(錯誤訊息)不作為程式判斷契約；Host 只依 `Kind`、`ErrorCode`、`IsRetryable` 及 typed data 判斷。

---

## 17. 輸入驗證

所有驗證在開啟業務交易前執行；需要目前資料狀態才能判斷者，必須在取得玩家鎖後再次驗證。

| 類別 | 規則 |
|---|---|
| Request | 不得為 null |
| UserUID、ActivityUID、EligibilityEntryId | 必須大於 0 |
| EventId | 1～128 字元，不得有前後空白，不轉換大小寫 |
| BonusTaskId | 必須為 36 字元 `D` 格式 Guid；比較時保留字串大小寫規則 |
| CorrelationId | null 或 1～128 字元；null 時核心產生新 Guid 字串 |
| enum | 必須為已定義數值 |
| AllowedActivityUIDs | 集合不得為 null；元素大於 0、去重後使用 |
| EligibleDepositAmount | `Deposit`、`FirstDepositOfBusinessDay` 必填且大於 0；其他 Trigger Type 必須為 null |
| 金額 | 不得為負數，不得超過 `DECIMAL(20,4)`，小數最多 4 位 |
| DateTime | `Kind` 必須為 `Unspecified` 或 `Local`；`Utc` 必須由 Host 先轉成本地時間 |
| 分頁／批次 | 遵守各 API 上限，不自動修正 |

此外：

- `GameBetAmount`、`GameWinAmount` 與 `SettledBonusWalletBalance` 均不得小於 0。
- Request 驗證失敗不開啟交易、不呼叫外部公式，也不產生 Wallet Instruction。
- H5gSet 負責活動設定輸入驗證，但核心服務讀取設定時仍須依 PCS-01 驗證資料完整性；錯誤資料回傳 `DataCorruption`。
- 核心服務不驗證身份、代理商、遊戲允許清單、押注上限、遊戲重送、取消或退款；這些由 Host 負責。

---

## 18. 呼叫順序與範例

### 18.1 初始化與建立資格

```csharp
var initialize = service.Initialize(new InitializeRequest(
    BusinessDayCutover: TimeSpan.FromHours(8),
    BalanceConvertFormula: context => legacyFormula(context),
    CorrelationId: correlationId));

var eligibility = service.CreateEligibility(new CreateEligibilityRequest(
    EventId: eventId,
    UserUID: userUid,
    TriggerType: TriggerType.Deposit,
    EventTime: eventTime,
    EligibleDepositAmount: depositAmount,
    AllowedActivityUIDs: allowedActivityUids,
    CorrelationId: correlationId));
```

### 18.2 領取與錢包派發

```csharp
var claim = service.ClaimPromotion(new ClaimPromotionRequest(
    userUid,
    eligibilityEntryId,
    correlationId));

if (claim.Kind == PromotionResultKind.Succeeded)
{
    SaveWalletInstructionsDurably(claim.Data!.WalletInstructions);
    ExecuteWalletInstructionsIdempotently(claim.Data.WalletInstructions);
}
```

不得在 `ClaimPromotion` 成功前先派發 Bonus，也不得在 Wallet Instruction 可靠保存前允許玩家開始任務。

### 18.3 遊戲累計與四種結案

```csharp
var wager = service.AccumulateWager(new AccumulateWagerRequest(
    userUid,
    expectedBonusTaskId,
    gameBetAmount,
    gameWinAmount,
    gameTime,
    settledBonusWalletBalance,
    correlationId));

if (wager.Data?.Outcome == BonusTaskOutcome.ReadyToClose)
{
    var completed = service.CloseWagerCompletedBonusTask(
        new CloseWagerCompletedBonusTaskRequest(
            userUid, expectedBonusTaskId, settledBonusWalletBalance,
            gameBetAmount, gameWinAmount, gameTime, correlationId));
}

var depleted = service.CloseDepletedBonusTask(
    new CloseDepletedBonusTaskRequest(
        userUid, expectedBonusTaskId, 0m, notificationTime, correlationId));

var abandoned = service.AbandonBonusTask(
    new AbandonBonusTaskRequest(
        userUid, expectedBonusTaskId, requestedAt, correlationId));

var maintenance = service.RunDailyMaintenance(
    new DailyMaintenanceRequest(executionTime, 500, correlationId));
```

- `AccumulateWager` 只累計流水；`WagerCompleted` 必須由 Host 另行呼叫 `CloseWagerCompletedBonusTask`。
- `BonusDepleted` 必須由 Host 呼叫 `CloseDepletedBonusTask`；若該局須計流水，先確認 `AccumulateWager` 成功。
- `BusinessDayExpired` 只由每日維護建立。
- 所有成功或部分成功結果的 Wallet Instructions，都須先可靠保存再冪等執行。

---

## 19. 版本相容性

- DLL 使用 Semantic Versioning(語意化版本)：修補版不得改變公開行為；新增向後相容欄位或方法提升次版本；破壞性修改提升主要版本。
- 公開 Request 的既有建構參數不得在同一主要版本重新排序、改型別或改語意。
- Result DTO 可在新次版本尾端新增可選欄位；Host 反序列化或映射時不得假設欄位數固定。
- enum 只能新增未使用數值，不得重編或重用既有值。
- `SnapshotVersion = 1` 的欄位與語意永久固定；新增快照格式使用新版本，讀取端必須保留對仍在資料保存期內版本的支援。
- 未支援的 SnapshotVersion 回傳 `UnsupportedSnapshotVersion`，不得以目前活動設定替代。
- `PromotionErrorCode` 一經發布不得改號；停用錯誤碼時保留數值。

---

## 20. 交叉引用

- 領域模型、列舉、欄位、計算與重置規則：見 [PCS-01](./01_優惠活動資料模型與欄位定義.md)。
- API 背後的交易、Row Lock 與維護批次：見 [PCS-02](./02_資料庫Schema與交易及資料列鎖設計.md)。
- 核心專用資料存取方法與例外分類：見 [PCS-03](./03_核心專用資料存取介面與MysqlAcess擴充規格.md)。
- Result、錯誤碼、冪等、重試與錢包一致性：見 [PCS-05](./05_錯誤碼重試及交易一致性規格.md)。
- API 驗收案例：見 [PCS-06](./06_單元測試與整合測試案例.md)。
