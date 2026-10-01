# 優惠錢包 Request Response Command 對照

## 通訊路徑與封包規則

CLIENT 透過 POST /api/wukong/CommonCommand 送出 CommonInfoData，Wukong 經 SendCommonCommand 轉給 DBCache。 DBCache 依 CommonInfoData.Command 分流，仍以 CommonInfoData 回覆 Game Server。Wukong 在 CommonInfoDataCompletedHandler 依回覆 Command 轉成同名的專用 Response 物件，再經 SignalR 傳給指定玩家。 SignalR 沿用現有 Packet 規則：Type 為 Response 類別名稱，例如 PromoClaimResponse；Content 為該物件的 JSON 字串。優惠回覆 不使用 WebCommonInfoData 傳給 CLIENT。HTTP 202 只代表 Wukong 已接收並轉送請求，業務結果由 SignalR 非同步送達。

## Request 與 Response Command

|用途|Request Command|Response Command|請求 Data|
|---|---|---|---|
|查詢玩家優惠頁（不含當日完成紀錄）|PromoGetActivitiesRequest|PromoGetActivitiesResponse|無必填值|
|查詢玩家優惠頁（含當日完成紀錄）|PromoGetPlayerOffersRequest|PromoGetPlayerOffersResponse|無必填值|
|查詢指定活動可玩遊戲|PromoGetGamesRequest|PromoGetGamesResponse|ActivityUID|
|領取選定資格|PromoClaimRequest|PromoClaimResponse|EligibilityEntryId|
|查詢進行中任務與流水|PromoGetTaskRequest|PromoGetTaskResponse|無必填值|
|領取已達標的手動解鎖金|PromoClaimUnlockRequest|PromoClaimUnlockResponse|BonusTaskId|
|主動放棄任務|PromoAbandonTaskRequest|PromoAbandonTaskResponse|BonusTaskId|
|分頁查詢優惠結案歷史|PromoGetHistoryRequest|PromoGetHistoryResponse|FromInclusive、ToExclusive、Offset、 Limit|

Command 區分大小寫。兩個優惠頁查詢均以 DBCache 從玩家目前所屬代理商取得的活動授權範圍為準，不接受 CLIENT 提供的活動 ID；同一活動在主清單只會有一筆。`PromoGetActivitiesRequest` 不讀取完成歷史，`PromoGetPlayerOffersRequest` 會附帶本活動日已完成紀錄。領取時使用 `EligibilityEntryId`。PromoClaimRequest 用於領取優惠資格並建立 Bonus Task；PromoClaimUnlockRequest 用於手動解鎖類型達標後領取解鎖金。固定金額類型維持達標後由後端自動結案與入帳。

## DBCache 到 Game Server 的 CommonInfoData

|欄位或 Data 鍵|型別|用途|
|---|---|---|
|MsgType|string|固定 CommonInfoData。|
|Command|string|上表的 Request 或 Response Command。|
|UserUID|int|指定玩家；Wukong 先核對登入身分，DBCache 不採信 Data 另填的玩家或代理商 ID。|
|GameServer|GameServerCode|指定 Game Server；Wukong 轉送時填入自身代碼。|
|Data["RequestId"]|string|可選的非同步請求識別；DBCache 原樣帶回，不作為領取、領取解鎖金或放棄的業 務冪等鍵。|
|Data["Payload"]|JSON string|核心業務產生的回覆欄位，結構對應同名的 WebProtocol Response 物件；Game Server 反序列化後送出。錢包指令失敗時仍可能與 ErrorCode 同時存在。|
|Data["ErrorCode"]|string|處理未完全成功時的錯誤碼；核心業務成功但錢包指令失敗時會與 Payload 同時帶回。|
|Data["IsMock"]|string|假回覆時為 true；正式回覆不帶或為 false。|
|Message|string|人可讀說明；不作為業務狀態判斷。|
|Type、MachineUID|int|優惠通訊暫不使用。|

Request 的 Data 值均為字串；整數與金額使用無千分位的十進位格式，日期用 yyyy-MM-dd，時間用 ISO 8601。Game Server 轉換失 敗時，仍回覆對應的專用 Response 類別，並填 InvalidResponsePayload 或 MissingResponsePayload。完整的 ErrorCode／Payload 判斷順序見 `Promotion.ResponseDecoder 解包規格.md`；尤其 ErrorCode 與 Payload 同時存在時，仍須解析 Payload。

## 所有 Response 共用欄位

以下欄位由 PromoResponseBase 提供；八個 Response 類別均繼承它。Wukong 從 CommonInfoData 填入共用欄位，不依賴 Payload 內 的同名值。

|變數|型別|用途|
|---|---|---|
|UserUID|int|接收回覆的玩家 UID。|
|RequestId|string|對應 CLIENT 發出的請求；未提供時為空字串。|
|Success|bool|沒有 ErrorCode 且 Payload 有效時為 true。|
|IsMock|bool|true 表示此回覆只供通訊測試，未執行優惠或錢包業務。|
|ErrorCode|string|失敗原因；成功時為空字串。|
|Message|string|人可讀訊息。|

## PromoGetActivitiesResponse 與 PromoGetPlayerOffersResponse

用途：回傳玩家優惠頁的單一主清單。`PromoGetActivitiesResponse` 的 `TodayCompletedItems` 固定為空；`PromoGetPlayerOffersResponse` 會回傳本活動日完成紀錄。

|變數|型別|用途|
|---|---|---|
|BusinessDay|string|查詢所屬活動日，格式 yyyy-MM-dd。|
|HasActiveBonusTask|bool|玩家是否有進行中的 Bonus Task。|
|ActiveBonusTaskId|string|目前任務 ID；沒有任務時為空字串。|
|ActiveTask|PromoActiveBonusTaskPayload|null 或唯一一筆進行中任務；不包含活動快照。|
|Activities|List<PromoPlayerPromotionActivityPayload>|所有啟用且授權可見的活動，依 ActivityUID 遞增。|
|Activities[].Activity|PromoActivityDisplayPayload|僅包含畫面顯示所需的活動規則欄位。|
|Activities[].EligibilityEntryId|long?|可領資格 ID；為 null 表示不可領。|
|Activities[].CanClaim|bool|資格是否已成立。|
|Activities[].HasActiveTask|bool|此活動是否為玩家目前正在進行的任務。|
|Activities[].ClaimButtonEnabled|bool|前端唯一可用於啟用領取按鈕的旗標。|
|Activities[].ActiveTaskProgress|BonusTaskProgressDto?|進行中任務的目前／需求／剩餘流水；其他活動為 null。|
|TodayCompletedItems|List<PromoTodayCompletedItemPayload>|僅玩家優惠頁查詢回傳；不影響主清單資格或按鈕。|

### 玩家優惠頁巢狀欄位

`Activities[].Activity` 的 `PromoActivityDisplayPayload` 僅包含下列可顯示規則；CLIENT 不可據此自行計算領取資格、紅利、流水或結算：

|欄位|型別|用途|
|---|---|---|
|ActivityInfo|string|活動顯示文字。|
|BonusType|int|紅利類型列舉值。|
|FixedBonusAmount|int|固定紅利金額設定。|
|MaxBonusAmount|int|百分比紅利上限設定。|
|DepositPercentage|int|儲值百分比設定。|
|MinimumDepositAmount|int?|最低儲值門檻；免費活動可為 null。|
|WagerMultiplier|int|洗碼倍數。|
|MaxBetAmount|int?|單筆押注上限；null 表示無限制。|
|DailyClaimLimit|int|每日可領上限。|

`ActiveTask` 包含 `BonusTaskId`、`EligibilityEntryId`、`ActivityUID`、`BusinessDay`、`BonusAmount`、目前／需求／剩餘流水、`MaxBetAmount` 及 `ClaimedAt`。CLIENT 可用其 `ActivityUID` 對照 `Activities` 取得活動顯示資訊。

`TodayCompletedItems` 每筆包含 `BonusHistoryId`、`BonusTaskId`、`ActivityUID`、`ActivityInfo`、`BusinessDay`、Bonus／流水／轉換金額、`CloseReason`、`ClaimedAt` 及 `ClosedAt`。

公開 Payload 不包含 `ActivitySnapshotDto`、`SnapshotVersion`、`CapturedAt`、`WagerCalculationType`、`WagerContributionRate`、`ConvertType`、互斥群組或結算上限等後端快照與結算欄位；這些欄位僅供 Promotion Core 使用。

## PromoGetGamesResponse

用途：顯示指定活動可玩遊戲。DBCache 回傳活動設定字串，遊戲清單的解析與資格判斷由後續業務實作處理。

|變數|型別|用途|
|---|---|---|
|ActivityUID|long|被查詢的活動 ID。|
|GameServerList|string|活動設定中的完整可玩遊戲字串；未設定時為空字串。|

## PromoClaimResponse

用途：回覆領取資格與建立 Bonus Task 的結果。正式業務接入後，應以錢包派發完成狀態決定 CLIENT 何時可開始使用任務；假回 覆不會派發紅利。

|變數|型別|用途|
|---|---|---|
|EligibilityEntryId|long|被領取的資格 ID。|
|BonusTaskId|string|新建或重播的任務 ID。|
|ActivityUID|long|對應活動 ID。|
|BusinessDay|string|領取所屬活動日。|
|BonusAmount|decimal|實際 Bonus 金額。|
|RequiredWagerAmount|decimal|目標流水量。|
|CurrentWagerAmount|decimal|目前已累計流水。|
|RemainingWagerAmount|decimal|尚須完成流水。|
|MaxBetAmount|int?|任務押注上限；未設定時為 null。|
|ClaimedAt|string|領取時間，ISO 8601。|
|IsReplay|bool|true 表示相同資格的重複請求回傳既有結果。|

## PromoGetTaskResponse

用途：顯示目前任務與流水進度。手動解鎖類型達標後仍保留任務，可繼續累計流水；固定金額類型達標後自動結案。沒有任務時 HasActiveTask=false 且 Task=null。

|變數|型別|用途|
|---|---|---|
|HasActiveTask|bool|是否有進行中 Bonus Task。|
|Task|PromoTaskInfo|任務明細；沒有任務時為 null。|
|Task.BonusTaskId|string|任務 ID。|
|Task.EligibilityEntryId|long|原資格 ID。|
|Task.ActivityUID|long|對應活動 ID。|
|Task.BonusAmount|decimal|實際領取 Bonus。|
|Task.RequiredWagerAmount|decimal|目標流水。|
|Task.CurrentWagerAmount|decimal|已累計流水。|
|Task.RemainingWagerAmount|decimal|剩餘流水，不低於零。|
|Task.UnlockMode|string|Auto 或 Manual；由 DBCache 依任務快照決定，僅 Manual 可由玩家領取解鎖金。|
|Task.CanClaimUnlock|bool|DBCache 判斷目前是否可發起手動解鎖金領取；只供 CLIENT 顯示操作入口。|
|Task.EstimatedUnlockAmount|decimal?|查詢當下預估可領金額；無法估算或不適用時為 null，領取時須重新計算。|
|Task.MaxBetAmount|int?|押注上限；未設定時為 null。|
|Task.ClaimedAt|string|領取時間。|

## PromoClaimUnlockResponse

用途：回覆玩家對指定達標任務發起的解鎖金領取。CLIENT 的金額、流水及錢包餘額不可作為計算依據；DBCache 須核對玩家、 任務及最新已結算資料。假回覆不結案、不清空優惠錢包，也不轉入主錢包。

|變數|型別|用途|
|---|---|---|
|BonusTaskId|string|本次要求領取的任務 ID。|
|FinalCurrentWagerAmount|decimal|結案時計入的最終有效流水。|
|ConvertedAmount|decimal|最終計算的解鎖金；不能採信 CLIENT 傳入的預估值。|
|ClosedAt|string|成功結案時間，ISO 8601；失敗時為空字串。|
|IsReplay|bool|相同任務的重複領取是否回傳既有結果。|
|WalletStatus|string|Pending：錢包指令待執行；Credited：已完成轉入；Failed：錢包執行失敗待處理； NotExecuted：僅假回覆。只有 Credited 可顯示已入帳。|

Success=true 表示領取業務已被接受並產生有效結果，不單獨表示錢包已入帳；以 WalletStatus 判斷入帳狀態。若錢包指令晚於此回 覆完成，正式業務須提供後續狀態更新或查詢能力。重送冪等依 BonusTaskId 與錢包 OperationKey，不依 RequestId。

## PromoAbandonTaskResponse

用途：回覆玩家主動放棄進行中任務的結果。假回覆不會結案或清空 Bonus Wallet。

|變數|型別|用途|
|---|---|---|
|BonusTaskId|string|被要求放棄的任務 ID。|
|CloseReason|string|結案原因；成功放棄為 PlayerAbandoned。|
|FinalCurrentWagerAmount|decimal|結案時已累計流水。|
|ConvertedAmount|decimal|解鎖金；主動放棄時為 0。|
|ClosedAt|string|結案時間，ISO 8601。|
|AlreadyClosed|bool|任務是否早已結案。|

## PromoGetHistoryResponse

用途：分頁顯示優惠任務結案歷史。

|變數|型別|用途|
|---|---|---|
|FromInclusive|string|查詢起點，含此時間。|
|ToExclusive|string|查詢終點，不含此時間。|
|Offset|int|跳過筆數。|
|Limit|int|每頁上限。|
|Items|List<PromoHistory Item>|結案紀錄清單。|
|Items[].BonusHistoryId|long|歷史紀錄 ID。|
|Items[].BonusTaskId|string|任務 ID。|
|Items[].ActivityUID|long|對應活動 ID。|
|Items[].ActivityInfo|string|活動顯示資訊。|
|Items[].BusinessDay|string|任務所屬活動日。|
|Items[].BonusAmount|decimal|原 Bonus 金額。|
|Items[].RequiredWagerAmount|decimal|目標流水。|
|Items[].CurrentWagerAmount|decimal|結案時累計流水。|
|Items[].ConvertedAmount|decimal|結案後解鎖金。|
|Items[].CloseReason|string|結案原因。|
|Items[].ClaimedAt|string|領取時間。|
|Items[].ClosedAt|string|結案時間。|

## DBCache 假回覆開關

環境變數 PROMO_FAKE_RESPONSES 控制回覆來源，啟動 DBCache 前設定；重新啟動後生效。預設關閉假回覆；設定為 true 時 才啟用。未設定或設為 false 時走正式業務接點；由於目前業務尚未接入，會明確回覆 ErrorCode=NotImplemented。啟用時八種請求 都會回傳固定範例資料、IsMock=true 與 Success=true；假資料只供 CLIENT 驗證封包和畫面，不會建立資格、領取任務、修改錢包、 結案或寫入歷史。回覆會帶回 RequestId。 註冊、每日首登入、儲值、遊戲結算、Bonus 用盡與活動日切換仍是後端事件，不新增 CLIENT 的優惠 CommonCommand。第一塊 代理商活動查詢與第二塊玩家可領／已領資料的正式服務實作，由後續開發人員補上。
