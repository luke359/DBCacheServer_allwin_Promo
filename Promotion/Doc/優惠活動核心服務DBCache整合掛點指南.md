# 優惠活動核心服務 DBCache 整合掛點指南

## 1. 範圍與現況

本文件說明 DBCache Host 在各業務時機呼叫核心服務的掛點。公開契約以 `src/Promotion.Core.Contracts/IPromotionCoreService.cs`、`RequestsAndResults.cs`、`CommonDtos.cs` 為準，共 12 個同步 API(Application Programming Interface，應用程式介面)。Host 的組合根與核心初始化位於 `DBCacheServer/Promotion/PromotionCoreHost.cs`；實際事件訂閱與業務接線由 DBCache 維護團隊實作及維護。本方案不再保留獨立的 `Promotion.Host.DBCache` 專案。

所有方法都回傳 `PromotionResult<T>`：`Kind`（`Succeeded`、`Rejected`、`RetryableFailure`、`PermanentFailure`、`OutcomeUnknown`；每日維護另可能有 `PartialSucceeded`）、`ErrorCode`、`IsRetryable`、`CorrelationId`、`Data`、`ErrorDetails`。成功時讀取 `Data`；失敗時依結果種類與錯誤碼處理。`CorrelationId` 只供追蹤，不能當作事件、遊戲局或錢包操作的冪等鍵。以下各節的「輸出」指成功時的 `Data` 型別與欄位。

共通輸入規則：`CorrelationId` 可省略，由核心產生；`UserUID` 必須大於 0；時間欄位使用 Host 本地時間（`DateTimeKind.Local` 或 `Unspecified`），不可傳 `Utc`；金額為非負 `decimal`，最多四位小數。`ExpectedBonusTaskId` 須使用核心回傳的任務識別碼。Business Day(營業日)依初始化的切換時間計算。所有方法都要在同一服務實例成功執行 `Initialize` 後呼叫。

## 2. 流程掛點總覽

| Host 業務時機 | 核心方法 | 呼叫目的 |
|---|---|---|
| Host 啟動、核心服務實例建立後且承接優惠流量前 | `Initialize` | 設定營業日切換時間與達標時的餘額轉換公式 |
| 註冊、每次登入候選、每筆成功儲值，或 Host 判定免費活動事件成立時 | `CreateEligibility` | 為事件建立玩家可領資格；核心保證每日首登入／首儲同日只產生一次資格 |
| Host 要顯示玩家當前全部可領優惠時 | `GetAvailablePromotions` | 取得當前營業日可領清單與是否有進行中任務 |
| Host 要查特定活動可玩遊戲伺服器設定時 | `GetGameServerList` | 取得該活動目前的 `GameServerList` 字串 |
| 玩家選定一筆資格並確認領取時 | `ClaimPromotion` | 建立任務並取得紅利錢包入帳指令 |
| Host 要顯示或核對玩家目前紅利任務時 | `GetBonusTaskStatus` | 取得進行中任務、剩餘流水與活動快照 |
| 一局遊戲完成最終結算，且該局須計入進行中優惠任務時 | `AccumulateWager` | 只累計有效流水；達標時回 `ReadyToClose`，不結案 |
| `AccumulateWager` 成功回傳 `ReadyToClose` 時 | `CloseWagerCompletedBonusTask` | 由 Host 明確要求以 `WagerCompleted` 結案 |
| Host 收到紅利錢包已用盡且無待累計遊戲局的確認通知時 | `CloseDepletedBonusTask` | 不增加流水，將用盡的任務結案 |
| 玩家確認放棄目前優惠任務時 | `AbandonBonusTask` | 主動放棄並取得清空紅利錢包指令 |
| 營業日切換後的排程／維護作業時 | `RunDailyMaintenance` | 逾期資格、逾期任務結案及 90 日歷史清理 |
| Host 要顯示玩家已結案優惠歷史時 | `GetBonusHistory` | 依結案時間區間分頁查詢 |

## 3. 全部公開方法

### 3.1 `Initialize(InitializeRequest) → PromotionResult<InitializeData>`

**掛點：** Host 啟動時，在服務實例建構完成、開始接受優惠請求及啟動維護作業之前。若服務實例重建，需對新實例重新呼叫。

**輸入：** `BusinessDayCutover: TimeSpan` 為每日營業日切換時間（`00:00:00` 至小於 `24:00:00`，秒精度）；`BalanceConvertFormula: BalanceConvertFormula` 為流水達標且活動 `ConvertType=Balance` 時使用的同步公式委派，輸入 `BalanceConvertContext`、回傳 `double`；`CorrelationId?: string` 為追蹤碼。公式須具決定性、無副作用，可能在交易重試時以相同輸入執行多次。

**輸出：** `BusinessDayCutover` 為已採用的切換時間；`SnapshotVersion` 為核心新建快照版本（目前為 2）；`WasAlreadyInitialized` 指相同切換時間與**同一委派實例**的重複初始化。設定不同會回 `InitializationConflict`；尚未成功初始化時其他 API 會回 `ServiceNotInitialized`。此方法不存取資料庫。

### 3.2 `CreateEligibility(CreateEligibilityRequest) → PromotionResult<CreateEligibilityData>`

**掛點：** Host 已確認觸發事件成立後呼叫：玩家註冊成功、登入成功、儲值成功，或免費活動由 Host 決定的觸發時機。登入候選可使用 `FirstLoginOfBusinessDay`；每筆成功儲值可使用 `FirstDepositOfBusinessDay` 交由核心判斷當日首次。若同一筆儲值還要評估一般 `Deposit` 活動，須另呼叫一次並使用另一個全域唯一 EventId。應在事件持久化後帶入可重送的事件識別碼；不要在單純開啟優惠頁時產生事件。

**輸入：** `EventId: string` 為 Host 產生並可靠保存的事件唯一碼（1～128 字元，前後不可有空白，重試沿用原值）；`UserUID: long` 為玩家；`TriggerType: TriggerType` 取 `Registration`、`FirstLoginOfBusinessDay`、`FirstDepositOfBusinessDay`、`Deposit`、`Free`；`EventTime: DateTime` 為事件成立時間，用來算營業日；`EligibleDepositAmount?: decimal` 只在兩種儲值觸發時必填且大於 0，其他觸發時必須為 `null`；`AllowedActivityUIDs: IReadOnlyCollection<long>` 為 Host 允許的活動 ID 範圍，空集合表示**不允許任何活動**；`CorrelationId?` 可省略。

**輸出：** `EventId`、`UserUID`、`BusinessDay`；`IsReplay` 表示同一事件重送；`Entries: IReadOnlyList<EligibilityEntryDto>` 為建立或查出的資格清單，零筆也是成功。每筆資格包含 `EligibilityEntryId`、`EventId`、`UserUID`、`ActivityUID`、`BusinessDay`、`TriggerType`、`EligibleDepositAmount`、`Status`、`ClaimedBonusTaskId`、`ExcludedByBonusTaskId`、`StatusChangedAt`、`CreatedAt`。Business Day 小於或等於已保存每日觸發日期的新 EventId 會保存 Event 並回 `Succeeded`、`IsReplay=false`、空 Entries；重送該 EventId 則 `IsReplay=true`。第一筆成功儲值即占用當日首儲，即使金額未達任何活動門檻或允許活動清單為空；延遲的舊日事件不會使日期倒退。既有 EventId 的玩家、觸發種類、事件時間或儲值金額不一致會回 `EventIdConflict`；重送不重新篩選活動。

### 3.3 `GetAvailablePromotions(GetAvailablePromotionsRequest) → PromotionResult<AvailablePromotionListData>`

**掛點：** Host 要回傳玩家目前全部可領優惠資訊時，例如優惠列表、領取選單或領取前刷新。這是查詢，不建立資格；需要先由對應事件呼叫 `CreateEligibility`。

**輸入：** `UserUID: long`；`QueryTime: DateTime` 為查詢當下本地時間、決定要看的營業日；`CorrelationId?`。

**輸出：** `UserUID`、`BusinessDay`、`HasActiveBonusTask`、`ActiveBonusTaskId?`，以及 `Items: IReadOnlyList<AvailablePromotionDto>`。每項有 `EligibilityEntryId`、`EventId`、`ActivityUID`、`BusinessDay`、`TriggerType`、`EligibleDepositAmount?`、`ActivityInfo?`、`EstimatedBonusAmount`、`EstimatedRequiredWagerAmount`、`MaxBetAmount?`、`IsNonStackable`、`ExclusiveGroup?`。只列當前營業日、狀態為 `Available` 的資格；有進行中任務仍會列出資格，但此時不可直接領取。預估金額取查詢當下活動設定，領取時仍會重新計算。空清單為成功。

### 3.4 `GetGameServerList(GetGameServerListRequest) → PromotionResult<GameServerListData>`

**掛點：** Host 需取得某個活動目前的可玩遊戲伺服器設定時，例如呈現活動詳情或由 Host 判斷遊戲資格時。

**輸入：** `ActivityUID: long` 為活動 ID，須大於 0；`CorrelationId?`。

**輸出：** `ActivityUID`、`GameServerList?: string`。核心原樣回傳目前活動設定字串或 `null`；`null` 是有效資料，活動不存在才是 `ActivityNotFound`。字串格式解讀與遊戲資格判斷由 Host 負責，核心不拆解字串，也不使用任務快照回答此查詢。

### 3.5 `ClaimPromotion(ClaimPromotionRequest) → PromotionResult<ClaimPromotionData>`

**掛點：** 玩家從可領清單選定一筆資格並確認領取時。Host 傳 `EligibilityEntryId`，核心會在交易中再次檢查資格、營業日、活動與進行中任務。

**輸入：** `UserUID: long`；`EligibilityEntryId: long` 為 `GetAvailablePromotions.Items` 或 `CreateEligibility.Entries` 回傳的資格 ID，須大於 0；`CorrelationId?`。

**輸出：** `UserUID`、`EligibilityEntryId`、`BonusTaskId`、`ActivityUID`、`BusinessDay`、`BonusAmount`、`RequiredWagerAmount`、`CurrentWagerAmount`、`RemainingWagerAmount`、`MaxBetAmount?`、`ClaimedAt`、`SnapshotVersion`、`IsReplay`、`WalletInstructions`。首次成功一般回傳 `CreditBonusWallet`；同一資格重送可能回 `IsReplay=true` 及相同 `OperationKey` 指令。Host 須先可靠保存指令，再依鍵冪等入紅利錢包；入帳完成前不要讓玩家使用任務。`BonusTaskId` 由核心產生，後續任務異動都傳此值。

### 3.6 `GetBonusTaskStatus(GetBonusTaskStatusRequest) → PromotionResult<BonusTaskStatusData>`

**掛點：** Host 要顯示任務進度、取得後續異動的 `BonusTaskId`，或處理不明結果後需核對目前狀態時。

**輸入：** `UserUID: long`；`CorrelationId?`。

**輸出：** `UserUID`、`HasActiveTask`、`Task?: ActiveBonusTaskDto`。無進行中任務時 `HasActiveTask=false`、`Task=null`；有任務時 `Task` 包含 `BonusTaskId`、`EligibilityEntryId`、`ActivityUID`、`BusinessDay`、`BonusAmount`、`RequiredWagerAmount`、`CurrentWagerAmount`、`RemainingWagerAmount`、`MaxBetAmount?`、`ClaimedAt`、`ActivitySnapshot`。`RemainingWagerAmount` 為目前即時計算值。非法任務狀態不可當成無任務；應依 `InvalidBonusTaskState` 或 `DataCorruption` 處理。

### 3.7 `AccumulateWager(AccumulateWagerRequest) → PromotionResult<AccumulateWagerData>`

**掛點：** 一局遊戲的下注、派彩及紅利錢包餘額都已**最終結算**，且 Host 判定該局應累計至目前任務時，每局呼叫一次。Host 要先以既有遊戲局紀錄完成單局去重；核心 Request 沒有遊戲局冪等鍵。

**輸入：** `UserUID: long`；`ExpectedBonusTaskId: string` 為該局對應的核心任務 ID；`GameBetAmount: decimal` 為本局下注額；`GameWinAmount: decimal` 為本局派彩額（第一版不參與流水公式，但會傳入餘額轉換公式）；`GameTime: DateTime` 為遊戲時間；`SettledBonusWalletBalance: decimal` 為該局結算後紅利錢包餘額；`CorrelationId?`。

**輸出：** `UserUID`、`BonusTaskId`、`EffectiveWagerAmount`、`CurrentWagerAmount`、`RemainingWagerAmount`、`Outcome`（`InProgress`、`ReadyToClose`、`AlreadyClosed`）、`CloseReason?`、`ConvertedAmount`、`ClosedAt?`、`WalletInstructions`。本方法只保存流水，不結案、不建立 History 且不回 Wallet Instruction。未達標回 `InProgress`；達標回 `ReadyToClose`，Host 必須停止送入後續局並呼叫下一方法。未達標且結算餘額為 0 時，先確認本次累計成功，再呼叫 `CloseDepletedBonusTask`。`AlreadyClosed` 回傳歷史狀態，不會再次累計。若結果為 `OutcomeUnknown`，不得盲目重送本局；保留原局資料並核對任務及歷史。

### 3.8 `CloseWagerCompletedBonusTask(CloseWagerCompletedBonusTaskRequest) → PromotionResult<CloseBonusTaskData>`

**掛點：** `AccumulateWager` 成功回傳 `ReadyToClose` 後立即呼叫。兩次呼叫間 Host 必須阻止該任務再接受新的遊戲局。

**輸入：** `UserUID: long`；`ExpectedBonusTaskId: string`；`SettledBonusWalletBalance: decimal`、`GameBetAmount: decimal`、`GameWinAmount: decimal`、`GameTime: DateTime` 必須沿用造成達標的同一局最終資料；`CorrelationId?`。

**輸出：** `CloseBonusTaskData`：`UserUID`、`BonusTaskId`、`CloseReason`、`FinalCurrentWagerAmount`、`ConvertedAmount`、`ClosedAt`、`AlreadyClosed`、`WalletInstructions`。只有核心重新確認流水達標才以 `WagerCompleted` 結案；未達標回 `WagerRequirementNotMet`。成功時先回 `ClearBonusWallet`，轉換額大於 0 時再回 `CreditMainWallet`。相同任務重送可由 History 重播。

### 3.9 `CloseDepletedBonusTask(CloseDepletedBonusTaskRequest) → PromotionResult<CloseBonusTaskData>`

**掛點：** Host 收到紅利錢包餘額已用盡的確認通知時。若同一局本身要計流水，先呼叫 `AccumulateWager`；只有其成功回 `InProgress` 時才走本方法，回 `ReadyToClose` 則改走 `CloseWagerCompletedBonusTask`。

**輸入：** `UserUID: long`；`ExpectedBonusTaskId: string`；`SettledBonusWalletBalance: decimal` 必須為 0；`NotificationTime: DateTime` 為通知時間；`CorrelationId?`。

**輸出：** `CloseBonusTaskData`：`UserUID`、`BonusTaskId`、`CloseReason`、`FinalCurrentWagerAmount`、`ConvertedAmount`、`ClosedAt`、`AlreadyClosed`、`WalletInstructions`。本方法不增加流水；一般以 `BonusDepleted` 結案。若原任務已達標且為固定轉換活動，會以 `WagerCompleted` 結案。相同任務已有歷史時回既有結果及 `AlreadyClosed=true`；非零餘額回 `BonusWalletNotDepleted`。Host 已確認紅利餘額為 0，因此本方法輸出不含 `ClearBonusWallet`。

### 3.10 `AbandonBonusTask(AbandonBonusTaskRequest) → PromotionResult<CloseBonusTaskData>`

**掛點：** 玩家明確確認放棄進行中的優惠任務時，在 Host 接受放棄操作後呼叫。

**輸入：** `UserUID: long`；`ExpectedBonusTaskId: string` 為玩家當前要放棄的任務；`RequestedAt: DateTime` 為請求時間；`CorrelationId?`。

**輸出：** 與 3.9 相同的 `CloseBonusTaskData` 欄位。首次成功以 `PlayerAbandoned` 結案，`ConvertedAmount=0`，回傳 `ClearBonusWallet` 指令；Host 須先可靠保存再清空錢包。同任務已結案時 `AlreadyClosed=true`，回傳原結案原因與對應指令，不重新改寫歷史；若目前是另一任務且指定的舊任務查無歷史，會回 `BonusTaskMismatch`。

### 3.11 `RunDailyMaintenance(DailyMaintenanceRequest) → PromotionResult<DailyMaintenanceData>`

**掛點：** Host 的每日維護排程，在營業日切換後執行。由 Host 控制排程、分片與重跑；同一輪重試沿用原 `ExecutionTime`。此排程不清除每日首登入／首儲日期；新活動日判斷不依賴排程是否已執行。

**輸入：** `ExecutionTime: DateTime` 為此次維護的固定本地時間，用來算當前營業日與歷史清理界線；`BatchSize: int` 為候選查詢與刪除批量，範圍 1～1000；`CorrelationId?`。

**輸出：** `CurrentBusinessDay`、`HistoryCutoffTime`（執行時間減 90 日）、`ExpiredEligibilityCount`、`ClosedExpiredTaskCount`、`DeletedHistoryCount`、`WalletInstructions`、`Failures`。每筆 `MaintenanceFailureDto` 含 `Stage`（`ExpireEligibility`、`CloseExpiredTask`、`DeleteHistory`）、`UserUID?`、`BonusTaskId?`、`ErrorCode`、`IsRetryable`。逾期任務結案的錢包指令須可靠保存並執行；部分成功可能回 `PartialSucceeded`。重跑只處理尚符合條件的資料，**不保證重現前一次成功回應中的錢包指令**，故收到成功結果後要立即可靠保存，遺失時須對帳。

### 3.12 `GetBonusHistory(GetBonusHistoryRequest) → PromotionResult<BonusHistoryPageData>`

**掛點：** Host 要顯示玩家已結案的優惠活動歷史頁面，或依任務歷史進行狀態核對時。

**輸入：** `UserUID: long`；`FromInclusive: DateTime`、`ToExclusive: DateTime` 為 `ClosedAt` 的左閉右開查詢區間，必須前小後大、跨度不超過 90 日，結束時間不可晚於呼叫當下；`Offset: int` 不小於 0；`Limit: int` 為 1～200；`CorrelationId?`。

**輸出：** 回顯 `UserUID`、`FromInclusive`、`ToExclusive`、`Offset`、`Limit`，以及 `Items: IReadOnlyList<BonusHistoryDto>`。每筆歷史含 `BonusHistoryId`、`BonusTaskId`、`UserUID`、`EligibilityEntryId`、`ActivityUID`、`BusinessDay`、`ActivitySnapshot`、`BonusAmount`、`RequiredWagerAmount`、`CurrentWagerAmount`、`ConvertedAmount`、`CloseReason`、`ClaimedAt`、`ClosedAt`。依 `ClosedAt`、`BonusHistoryId` 遞減排序；沒有資料回空集合。每日維護可清除超過保存期的歷史，不能把查無資料單獨視為從未發生。

## 4. Host 接線時的共通處理

1. **事件與任務識別：** `EventId` 由 Host 產生並可靠保存，重試不得換值；`BonusTaskId` 只用核心回傳值。`CorrelationId` 不能替代兩者。
2. **錢包指令：** `WalletInstructionDto` 含 `OperationKey`、`Action`、`UserUID`、`BonusTaskId`、`Amount`。Host 收到成功或部分成功結果後，先可靠保存全部指令，再依 `OperationKey` 冪等執行。`ClearBonusWallet` 必須先於同任務的 `CreditMainWallet`。指令執行失敗只重試指令，不重呼已成功的核心業務操作。
3. **錯誤與結果不明：** `Rejected` 是業務拒絕；`RetryableFailure` 依錯誤規格以原 Request 重試；`OutcomeUnknown` 先停止相關後續流程並查證，特別是遊戲局不可直接重送。`ErrorDetails` 可能包含既有事件或任務識別，供衝突查證。
4. **流程路由：** 已進入新核心的玩家，其資格、任務、流水、結案與錢包處理須維持同一路徑；不能因旗標切換使進行中任務落回舊流程。
5. **領取重播：** `ClaimPromotion` 即使在任務已結案後重播，也可能依歷史回傳原 `CreditBonusWallet` 指令。Host 只按既有 `OperationKey` 核對／補辦原指令，不可把已結案任務重新開放使用。
6. **遊戲與結案先後：** 遊戲局須取得最終下注、派彩和結算後紅利餘額再呼叫 `AccumulateWager`。回 `ReadyToClose` 時以同局資料呼叫 `CloseWagerCompletedBonusTask`；未達標但餘額為 0 時才呼叫 `CloseDepletedBonusTask`。兩次呼叫間不得再送入新局。遊戲局結果不明時，僅查詢狀態與歷史不足以證明流水確切加了幾次，需保留原局紀錄進行對帳。
7. **維護與錢包：** 維護的每位玩家可能獨立成功或失敗；`PartialSucceeded` 要保存成功項目的指令與失敗清單。同一輪重跑固定 `ExecutionTime`，但不能靠重跑補取已遺失的指令清單。
8. **上線前置：** Host 須有可靠的 Wallet Instruction 儲存、按 `OperationKey` 唯一化與依序執行機制，並完成事件 ID 保存、遊戲局去重、結果不明對帳、核心初始化及版本路由，才可接入會異動任務的流程。

### 共用巢狀資料欄位

- `ActivitySnapshotDto`（3.6 的進行中任務及 3.12 的歷史內含）：`SnapshotVersion`、`ActivityUID`、`ActivityInfo?`、`BonusType`、`FixedBonusAmount`、`MaxBonusAmount`、`DepositPercentage`、`MinimumDepositAmount?`、`WagerMultiplier`、`WagerCalculationType`、`MaxBetAmount?`、`WagerContributionRate`、`ConvertType`、`FixedConvertedAmount`、`MaxBalanceConvertedAmount?`、`DailyClaimLimit`、`StartDate?`、`EndDate?`、`WeekdayMask`、`TriggerType`、`IsNonStackable`、`ExclusiveGroup?`、`CapturedAt`。這是領取時保留的活動設定，不應以目前活動設定覆蓋。
- `BalanceConvertContext`（3.1 公式輸入）：`UserUID`、`BonusTaskId`、`EligibilityEntryId`、`ActivityUID`、`BusinessDay`、`BonusAmount`、`RequiredWagerAmount`、`CurrentWagerAmount`、`RemainingWagerAmount`、`SettledBonusWalletBalance`、`GameBetAmount`、`GameWinAmount`、`GameTime`、`ActivitySnapshot`。公式只在符合餘額轉換的達標結案時由核心呼叫；Host 不直接呼叫公式作為結案替代。

## 5. 依據

- [核心服務 API 規格](./04_核心服務API規格.md)：方法、DTO、業務語意。
- [錯誤碼、重試及交易一致性規格](./05_錯誤碼重試及交易一致性規格.md)：結果分類、重試、錢包與不明結果。
- [核心服務實作與整合工作指引](./07_核心服務實作與整合工作指引.md)：Host 呼叫、排程、部署前條件。
- `src/Promotion.Core.Contracts/IPromotionCoreService.cs`、`RequestsAndResults.cs`、`CommonDtos.cs`、`Enums.cs` 及 `src/Promotion.Core/PromotionCoreService*.cs`：目前程式契約與實作。
