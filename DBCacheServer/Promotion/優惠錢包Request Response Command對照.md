# 優惠錢包 Request Response Command 對照

## 通訊路徑與封包規則

CLIENT 透過 POST /api/wukong/CommonCommand 送出 CommonInfoData，Wukong 經 SendCommonCommand 轉給 DBCache。 DBCache 依 CommonInfoData.Command 分流，仍以 CommonInfoData 回覆 Game Server。Wukong 在 CommonInfoDataCompletedHandler 依回覆 Command 轉成同名的專用 Response 物件，再經 SignalR 傳給指定玩家。 SignalR 沿用現有 Packet 規則：Type 為 Response 類別名稱，例如 PromoClaimResponse；Content 為該物件的 JSON 字串。優惠回覆 不使用 WebCommonInfoData 傳給 CLIENT。HTTP 202 只代表 Wukong 已接收並轉送請求，業務結果由 SignalR 非同步送達。

## Request 與 Response Command

|用途|Request Command|Response Command|請求 Data|
|---|---|---|---|
|查詢玩家所屬代理商在本 活動日的全部活動|PromoGetActivitiesRequest|PromoGetActivitiesResponse|無必填值|
|查詢玩家本活動日可領與 已領優惠|PromoGetPlayerOffersRequest|PromoGetPlayerOffersResponse|無必填值|
|查詢指定活動可玩遊戲|PromoGetGamesRequest|PromoGetGamesResponse|ActivityUID|
|領取選定資格|PromoClaimRequest|PromoClaimResponse|EligibilityEntryId|
|查詢進行中任務與流水|PromoGetTaskRequest|PromoGetTaskResponse|無必填值|
|領取已達標的手動解鎖金|PromoClaimUnlockRequest|PromoClaimUnlockResponse|BonusTaskId|
|主動放棄任務|PromoAbandonTaskRequest|PromoAbandonTaskResponse|BonusTaskId|
|分頁查詢優惠結案歷史|PromoGetHistoryRequest|PromoGetHistoryResponse|FromInclusive、ToExclusive、Offset、 Limit|

Command 區分大小寫。第一個查詢不按玩家是否已觸發、可領或已領過濾；第二個查詢分開回傳 AvailableItems 與 ClaimedItems。 同一活動可有多筆資格，領取時使用 EligibilityEntryId。 PromoClaimRequest 用於領取優惠資格並建立 Bonus Task；PromoClaimUnlockRequest 用於手動解鎖類型達標後領取解鎖金。固定金 額類型維持達標後由後端自動結案與入帳。手動解鎖類型達標後須保留任務並繼續累計已結算流水，直到玩家領取或發生其他結案 條件；此業務規則尚待核心服務實作。

## DBCache 到 Game Server 的 CommonInfoData

|欄位或 Data 鍵|型別|用途|
|---|---|---|
|MsgType|string|固定 CommonInfoData。|
|Command|string|上表的 Request 或 Response Command。|
|UserUID|int|指定玩家；Wukong 先核對登入身分，DBCache 不採信 Data 另填的玩家或代理商 ID。|
|GameServer|GameServerCode|指定 Game Server；Wukong 轉送時填入自身代碼。|
|Data["RequestId"]|string|可選的非同步請求識別；DBCache 原樣帶回，不作為領取、領取解鎖金或放棄的業 務冪等鍵。|
|Data["Payload"]|JSON string|成功回覆的業務欄位，結構對應同名的 WebProtocol Response 物件；Game Server 反序列化後送出。|
|Data["ErrorCode"]|string|失敗時的錯誤碼；成功時不帶。|
|Data["IsMock"]|string|假回覆時為 true；正式回覆不帶或為 false。|
|Message|string|人可讀說明；不作為業務狀態判斷。|
|Type、MachineUID|int|優惠通訊暫不使用。|

Request 的 Data 值均為字串；整數與金額使用無千分位的十進位格式，日期用 yyyy-MM-dd，時間用 ISO 8601。Game Server 轉換失 敗時，仍回覆對應的專用 Response 類別，並填 InvalidResponsePayload 或 MissingResponsePayload。

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

## PromoGetActivitiesResponse

用途：優惠頁第一塊，顯示玩家所屬代理商在本活動日的所有有效活動。業務篩選由後續 DBCache 開發完成。

|變數|型別|用途|
|---|---|---|
|BusinessDay|string|活動日，格式 yyyy-MM-dd。|
|Items|List<PromoActivit yItem>|活動項目；空陣列代表當日沒有活動。|
|Items[].ActivityUID|long|活動唯一識別碼。|
|Items[].ActivityInfo|string|顯示名稱或說明。|
|Items[].TriggerType|string|觸發類型，例如 Free 或 Deposit。|
|Items[].BonusType|string|紅利派發類型。|
|Items[].FixedBonusAmount|int?|固定 Bonus 設定；不適用時為 null。|
|Items[].DepositPercentage|int?|儲值百分比設定；不適用時為 null。|
|Items[].MaxBonusAmount|int?|Bonus 上限；不適用時為 null。|
|Items[].MinimumDepositAmount|int?|最低儲值金額；不適用時為 null。|
|Items[].WagerMultiplier|int|目標流水的洗碼倍數。|
|Items[].DailyClaimLimit|int|每活動日可成功領取的次數上限。|

## PromoGetPlayerOffersResponse

用途：優惠頁第二塊，分別顯示玩家本活動日可領資格與已領優惠。已領資料包含進行中與已結案任務；後續由 DBCache 彙整。

|變數|型別|用途|
|---|---|---|
|BusinessDay|string|查詢所屬活動日。|
|AvailableItems|List<PromoAvaila bleOfferItem>|尚可領取的資格清單。|
|AvailableItems[].EligibilityEntryId|long|領取時提交的資格 ID。|
|AvailableItems[].ActivityUID|long|對應活動 ID。|
|AvailableItems[].EventId|string|建立資格的事件 ID。|
|AvailableItems[].ActivityInfo|string|活動顯示資訊。|
|AvailableItems[].TriggerType|string|建立資格的觸發類型。|
|AvailableItems[].EligibleDepositAmount|decimal?|該資格的合格儲值金額；無儲值時為 null。|
|AvailableItems[].EstimatedBonusAmount|decimal|查詢當下的預估 Bonus，非領取承諾。|
|AvailableItems[].EstimatedRequiredWag erAmount|decimal|查詢當下的預估目標流水。|
|AvailableItems[].MaxBetAmount|int?|活動押注上限；未設定時為 null。|
|ClaimedItems|List<PromoClaime dOfferItem>|本活動日已領取的資格／任務清單。|
|ClaimedItems[].EligibilityEntryId|long|原資格 ID。|
|ClaimedItems[].BonusTaskId|string|領取後建立的任務 ID。|
|ClaimedItems[].ActivityUID|long|對應活動 ID。|
|ClaimedItems[].ActivityInfo|string|活動顯示資訊。|
|ClaimedItems[].BonusAmount|decimal|實際領取 Bonus。|
|ClaimedItems[].ClaimedAt|string|領取時間，ISO 8601。|
|ClaimedItems[].TaskState|string|Active 或 Closed 等任務狀態。|
|ClaimedItems[].CloseReason|string|已結案時的結案原因；進行中為空字串。|
|HasActiveBonusTask|bool|玩家目前是否有進行中任務。|
|ActiveBonusTaskId|string|目前任務 ID；沒有任務時為空字串。|

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
