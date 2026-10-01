# Promotion.ResponseDecoder 解包規格

## 目的與適用範圍

本文件定義 `Promotion.ResponseDecoder` 函式庫在 DBCache→Game Server 的 `CommonInfoData` 區段收到優惠回覆後，如何解析 `Command`、`Data["ErrorCode"]` 與 `Data["Payload"]`，並產生中立的解包結果。Game Server→CLIENT 的 `WebProtocol`／SignalR 區段是另一份契約，由各 Server 的轉接層處理。

本規格不處理下列事項：

- GameServer 的 `CommonInfoData` 類別、玩家連線查找或登入狀態。
- CLIENT Response 類別、SignalR 封包與傳送。
- 錢包指令的執行、重試或業務狀態變更。

各 GameServer 應以轉接層將自己的回覆封包轉為函式庫的中立輸入模型，並將解包結果映射到該 Server 的 CLIENT Response。

## 中立輸入與輸出契約

函式庫輸入至少須提供下列欄位：

|欄位|型別|規則|
|---|---|---|
|`Command`|`string`|必須是八種 `Promo*Response` 之一；未知 Command 不可產生可傳送給 CLIENT 的結果。|
|`UserUID`|`long`|原樣保留，供呼叫端決定回覆目標；不可從 Payload 推導。|
|`Data`|`IReadOnlyDictionary<string, string>?`|可為 `null`，鍵值可缺少。|
|`Message`|`string?`|人可讀訊息；原樣保留，不可用於業務分支判斷。|

解包結果至少須提供：`Command`、`UserUID`、`RequestId`、`IsMock`、`Success`、`ErrorCode`、`Message` 及已轉型的 `Payload`。

## 回覆欄位規則

|來源欄位|函式庫處理方式|
|---|---|
|`Command`|決定 Payload DTO 型別。未知 Command 回傳不支援結果，呼叫端不可轉送。|
|`UserUID`|原樣帶入結果。|
|`Data["RequestId"]`|可選；原樣帶入結果，供呼叫端配對非同步請求。不可作為業務冪等鍵。|
|`Data["ErrorCode"]`|可選；非空白代表處理未完全成功。必須原樣帶入，除非 Payload 無效而被 `InvalidResponsePayload` 覆寫。|
|`Data["Payload"]`|可選；存在時視為 JSON 字串，並依 Command 反序列化。其存在不代表整體成功。|
|`Data["IsMock"]`|可選；值為不分大小寫的 `true` 時，結果的 `IsMock=true`。|
|`Message`|原樣帶入結果；不可用於業務分支判斷。|

`Data`、各鍵值及 `Payload` 都可能缺少。函式庫不得因字典索引或 JSON 例外中斷呼叫端的接收迴圈。

## ErrorCode 與 Payload 判斷矩陣

|ErrorCode|Payload|函式庫行為|`Success`|
|---|---|---|---|
|無或空白|有效 JSON|解析 Payload。|`true`|
|有值|不存在或空白|不解析 Payload，保留 ErrorCode 與 Message。|`false`|
|有值|有效 JSON|仍須解析 Payload，保留業務欄位、ErrorCode 與 Message。|`false`|
|無或空白|不存在或空白|設定 `ErrorCode="MissingResponsePayload"`。|`false`|
|任意|無效 JSON 或型別不符|設定 `ErrorCode="InvalidResponsePayload"`，Payload 為 `null`。|`false`|

第三列是正式回覆的重要例外。核心業務已產生 Payload 後，錢包指令可能失敗並追加 `ErrorCode="WalletInstructionFailed"`；函式庫不得因 ErrorCode 存在而略過 Payload。

## ErrorCode 處理原則

ErrorCode 為穩定的機器可讀字串。函式庫不可自行改名、轉數字或以 `Message` 取代它。

|類別|目前可能值|解包要求|
|---|---|---|
|請求格式|`InvalidIdentifier`、`InvalidDateTime`、`InvalidPaginationOrBatchSize`|保留錯誤；通常無 Payload。|
|服務狀態|`ServiceNotInitialized`、`NotImplemented`、`UnexpectedError`|保留錯誤；通常無 Payload。|
|資格／任務業務|`EligibilityNotFound`、`EligibilityPlayerMismatch`、`EligibilityExpired`、`EligibilityExcluded`、`EligibilityAlreadyClaimed`、`ActiveBonusTaskExists`、`DailyClaimLimitReached`、`BonusTaskNotFound`、`BonusTaskMismatch`、`InvalidBonusTaskState`、`WagerRequirementNotMet` 等|保留錯誤；通常無 Payload。|
|暫時性核心失敗|`DatabaseDeadlock`、`DatabaseLockWaitTimeout`、`DatabaseConnectionFailure`、`CommitOutcomeUnknown`|保留錯誤；函式庫不做重試。|
|錢包後處理|`WalletInstructionFailed`|若 Payload 存在必須解析，且 `Success=false`。|

函式庫只負責保留與判斷結果；是否重試、顯示訊息、重新查詢或執行錢包對帳，均由呼叫端依業務規格決定。

## Command 與 Payload 型別對照

|Command|反序列化型別|備註|
|---|---|---|
|`PromoGetActivitiesResponse`|`PromoGetPlayerOffersPayload`|`TodayCompletedItems` 固定為空陣列。|
|`PromoGetPlayerOffersResponse`|`PromoGetPlayerOffersPayload`|包含本活動日已完成紀錄。|
|`PromoGetGamesResponse`|`PromoGetGamesPayload`||
|`PromoClaimResponse`|`PromoClaimPayload`|`WalletInstructionFailed` 時仍可能有此 Payload。|
|`PromoGetTaskResponse`|`PromoGetTaskPayload`||
|`PromoClaimUnlockResponse`|目前沒有正式成功 Payload|目前僅預期 `NotImplemented` 且無 Payload。|
|`PromoAbandonTaskResponse`|`PromoAbandonPayload`|`WalletInstructionFailed` 時仍可能有此 Payload。|
|`PromoGetHistoryResponse`|`PromoGetHistoryPayload`||

Payload JSON 欄位名稱為 PascalCase，必須與 DBCache 實際產生的 public field 同名。DTO 金額欄位使用 `decimal`；識別碼使用 `long` 或 `string`，不可降為 `int` 或 `double`；時間欄位以 ISO 8601 字串保留。

Payload 根節點必須是 JSON 物件。解碼器必須驗證目前正式 Payload 的必要既有欄位及必要巢狀欄位均存在且型別正確；缺少或型別不符均為 `InvalidResponsePayload`。未知欄位應忽略，以保留新增可選欄位的向前相容性。

## Payload 欄位摘要

|Payload 型別|主要欄位|
|---|---|
|`PromoGetPlayerOffersPayload`|`BusinessDay`、`HasActiveBonusTask`、`ActiveBonusTaskId`、`ActiveTask`、`Activities`、`TodayCompletedItems`；活動資料為精簡顯示模型。|
|`PromoGetGamesPayload`|`ActivityUID`、`GameServerList`|
|`PromoClaimPayload`|`EligibilityEntryId`、`BonusTaskId`、`ActivityUID`、`BusinessDay`、`BonusAmount`、`RequiredWagerAmount`、`CurrentWagerAmount`、`RemainingWagerAmount`、`MaxBetAmount`、`ClaimedAt`、`IsReplay`|
|`PromoGetTaskPayload`|`HasActiveTask`、`Task`；`Task` 含 `BonusTaskId`、流水金額、`UnlockMode`、`CanClaimUnlock`、`EstimatedUnlockAmount` 等|
|`PromoAbandonPayload`|`BonusTaskId`、`CloseReason`、`FinalCurrentWagerAmount`、`ConvertedAmount`、`ClosedAt`、`AlreadyClosed`|
|`PromoGetHistoryPayload`|`FromInclusive`、`ToExclusive`、`Offset`、`Limit`、`Items`|

### 玩家優惠頁巢狀契約

- `Activities[].Activity` 是 `PromoActivityDisplayPayload`，只含活動名稱、紅利類型與金額／百分比設定、最低儲值、洗碼倍數、押注上限及每日領取上限。
- `ActiveTask` 是 `PromoActiveBonusTaskPayload`，只含任務識別、活動識別、活動日、紅利／流水金額、押注上限與領取時間；CLIENT 以 `ActivityUID` 對照 `Activities` 顯示活動資訊。
- `TodayCompletedItems` 是 `PromoTodayCompletedItemPayload`，只含歷史與畫面顯示所需資料，使用 `ActivityInfo`，不含活動快照。
- Payload 不得要求或依賴 `ActivitySnapshotDto` 的 `SnapshotVersion`、`CapturedAt`、結算、流水貢獻或互斥規則欄位；這些欄位不是對外契約。

完整公開欄位以 `PromotionClientCommands.cs` 與 ResponseDecoder 的 `Contracts/Payloads/PromoPayloads.cs` 為準；`Promotion.Core.Contracts` 的 DTO 僅為後端來源模型，不是直接序列化的公開格式。

## 函式庫解包流程

1. 驗證 `Command` 是否為已支援的優惠 Response；否則回傳不支援結果。
2. 以安全讀取方式取得 `RequestId`、`ErrorCode`、`Payload` 與 `IsMock`；缺少鍵視為空值。
3. 建立結果的共用欄位：`Command`、`UserUID`、`RequestId`、`IsMock`、`ErrorCode`、`Message`。
4. Payload 存在時，依 Command 反序列化；即使 ErrorCode 有值也要執行。
5. 若 Payload 無效，設定 `ErrorCode="InvalidResponsePayload"`、`Success=false` 與 `Payload=null`；可由呼叫端記錄 Command、UserUID、RequestId 與例外，不記錄敏感玩家資料。
6. 若無 ErrorCode 且 Payload 缺少，設定 `ErrorCode="MissingResponsePayload"` 與 `Success=false`。
7. 其餘情況以 `string.IsNullOrWhiteSpace(ErrorCode) && payloadValid` 設定 `Success`。

## C# .NET 10 API 骨架

以下為函式庫 API 形狀，實際 Payload DTO 應採用明確型別而非動態物件：

```csharp
public sealed record PromoCommonResponse(
    string Command,
    long UserUID,
    IReadOnlyDictionary<string, string>? Data,
    string? Message);

public abstract class PromoPayloadBase;

public sealed class PromoDecodeResult
{
    public bool IsSupportedCommand { get; init; }
    public string Command { get; init; } = string.Empty;
    public long UserUID { get; init; }
    public string RequestId { get; init; } = string.Empty;
    public bool IsMock { get; init; }
    public bool Success { get; init; }
    public string ErrorCode { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public PromoPayloadBase? Payload { get; init; }
}

public interface IPromoResponseDecoder
{
    PromoDecodeResult Decode(PromoCommonResponse response);
}
```

`Decode` 不得因可預期的協定輸入問題擲出例外。JSON.NET 的 `JsonException` 與型別轉換例外必須在函式庫內轉為 `InvalidResponsePayload`。

## 驗收案例

|案例|輸入|預期|
|---|---|---|
|一般查詢成功|`PromoGetTaskResponse`，合法 Payload，無 ErrorCode|解出 Task，`Success=true`。|
|核心拒絕|`PromoClaimResponse`，`ErrorCode=EligibilityExpired`，無 Payload|保留錯誤，`Success=false`，不擲出例外。|
|錢包失敗|`PromoClaimResponse`，`ErrorCode=WalletInstructionFailed`，合法 Payload|解出任務資訊，`Success=false`。|
|缺少 Payload|`PromoGetGamesResponse`，無 ErrorCode、無 Payload|`ErrorCode=MissingResponsePayload`。|
|錯誤 JSON|`PromoGetHistoryResponse`，Payload 為 `{bad}`|`ErrorCode=InvalidResponsePayload`，呼叫端可繼續接收。|
|未實作手動解鎖|`PromoClaimUnlockResponse`，`ErrorCode=NotImplemented`，無 Payload|一般失敗結果，不建立假資料。|
|未知 Command|非 `Promo*Response` 或未列入對照表的值|`IsSupportedCommand=false`，不產生可轉送結果。|

## 依據文件

- `優惠錢包Request Response Command對照.md`：外部欄位、CLIENT 契約與各 Payload 詳細欄位。
- `PromotionClientCommands.cs`：目前正式 Payload 的實際組裝與 JSON 序列化來源。
- `Promotion.Core.Contracts/CommonDtos.cs`、`RequestsAndResults.cs`：巢狀 DTO 定義。
- `05_錯誤碼重試及交易一致性規格.md`：錢包失敗、重試與結果不明的業務限制。

函式庫的建立、手動 DLL 發佈、內部 NuGet 發佈與 GameServer 轉接方式，請參考 `Promotion.ResponseDecoder 函式庫製作與使用指導.md`。
