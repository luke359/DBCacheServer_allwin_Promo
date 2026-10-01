# Promotion.ResponseDecoder

`Promotion.ResponseDecoder` 是供 Game Server 使用的 .NET 10 函式庫，負責解開 DBCache 回傳之 `CommonInfoData.Data["Payload"]` JSON。它不參考 `DBCacheServer`、`WebProtocol`、玩家連線或 SignalR。

## 協定邊界

```text
DBCache CommonInfoData
  → Promotion.ResponseDecoder
  → Game Server Adapter
  → Game Server 的 CLIENT Response / SignalR
```

DBCache→Game Server 與 Game Server→CLIENT 是不同協定。Adapter 負責玩家查找、CLIENT Response 映射與傳送；是否修改 `WebProtocol` 留待 Game Server→CLIENT 工作開始時決定。

## 使用方式

```csharp
var decoded = new PromoResponseDecoder().Decode(new PromoCommonResponse(
    common.Command,
    common.UserUID,
    common.Data,
    common.Message));

if (!decoded.IsSupportedCommand)
    return;

// 依 decoded.Command 與已轉型的 decoded.Payload 映射為本 Server 的 CLIENT Response。
// decoded.Success 為 false 時，仍可能保有 Payload，例如 WalletInstructionFailed。
```

`PromoGetActivitiesResponse` 與 `PromoGetPlayerOffersResponse` 共用 `PromoGetPlayerOffersPayload`。目前 `PromoClaimUnlockResponse` 只支援沒有 Payload 的正式失敗回覆；收到 Payload 時會回傳 `InvalidResponsePayload`。

## 驗證規則

- 支援八個已發布的 `Promo*Response` Command；未知 Command 回傳 `IsSupportedCommand=false`。
- 所有正式必要欄位與必要巢狀欄位必須存在且型別正確；否則回傳 `InvalidResponsePayload`。
- 未知欄位會忽略，以支援未來新增的可選欄位。
- `ErrorCode` 與有效 Payload 同時存在時仍會解包 Payload，但 `Success=false`。

## 建置與測試

```powershell
dotnet restore .\Promotion.ResponseDecoder.sln --locked-mode
dotnet build .\Promotion.ResponseDecoder.sln --configuration Release --no-restore
dotnet test .\Promotion.ResponseDecoder.sln --configuration Release --no-build
```

`docs/source-contracts/` 保存本版本實作所依據的來源契約快照。單元測試 Fixture 依 `PromotionClientCommands.cs` 的正式輸出格式建立；`PromoMockResponse.cs` 不作為正式成功契約。
