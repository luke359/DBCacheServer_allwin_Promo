# Promotion.ResponseDecoder 函式庫製作與使用指導

## 1. 目的與邊界

本函式庫用於讓多個 Game Server 共用「正式優惠回覆」的 Command 驗證、`Data` 安全讀取、JSON 解包、ErrorCode 與 `Success` 判斷規則。

函式庫**只負責解包**，不得依賴任何特定 Game Server 的下列項目：

- 玩家連線、登入 Session 或玩家查找邏輯。
- SignalR 傳送邏輯。
- 個別 Game Server 的 `CommonInfoData` 類別。
- 個別 Game Server 的 CLIENT Response 類別。

各 Game Server 僅保留一層薄的轉接程式：將自己的 `CommonInfoData` 轉為函式庫輸入模型、以 `UserUID` 查找玩家，並把函式庫輸出映射為既有 CLIENT Response 後送出。

此邊界可讓函式庫先以手動 DLL 引用，日後不改 API 即可發佈為內部 NuGet 套件。

## 2. 建立前必須帶入新專案的檔案

新專案建立時，請將下列檔案複製至新專案的 `docs/source-contracts/`。它們是設計與驗收依據，不應以手動抄寫後的記憶取代。

|優先度|來源檔案|用途|新專案用途|
|---|---|---|---|
|必要|`DBCacheServer/Promotion/Promotion.ResponseDecoder 解包規格.md`|解包優先順序、錯誤處理、`Success` 規則|主要實作規格|
|必要|`DBCacheServer/Promotion/優惠錢包Request Response Command對照.md`|八組 Command、所有外部回覆欄位、CLIENT 封包契約|DTO 與測試案例依據|
|必要|`DBCacheServer/Promotion/PromotionClientCommands.cs`|DBCache 實際寫出的 Payload 型別、欄位與 JSON 產生位置|Payload DTO 的權威實作對照；僅作參考，不直接編譯引用|
|必要|`Promotion/src/Promotion.Core.Contracts/CommonDtos.cs`|巢狀型別，例如 `ActivitySnapshotDto`、`ActiveBonusTaskDto`、`BonusHistoryDto`|完整巢狀 DTO 欄位依據|
|必要|各 Game Server 專案內的 `CommonInfoData` 定義檔|確認外層欄位與 `Data` 字典實際型別|撰寫各 Server 的轉接層|
|必要|各 Game Server 專案內的 `PromoResponseBase` 與八個 `Promo*Response` 定義檔|確認 CLIENT Response 是否持有 `Payload` 屬性或採扁平欄位|撰寫各 Server 的映射層|
|必要|各 Game Server 專案內的 `CommonInfoDataCompletedHandler`（或等效接收處理器）及 SignalR 傳送檔|確認玩家查找、封包 `Type`／`Content` 與例外記錄規則|撰寫各 Server 的轉接層|
|建議|`DBCacheServer/PromoMockResponse.cs`|八種測試 Payload 範例|建立 JSON 解包測試資料|
|建議|`Promotion/Doc/05_錯誤碼重試及交易一致性規格.md`|錢包失敗與結果不明的業務限制|驗證錯誤後 CLIENT 行為|

### 2.1 不應直接複製為相依組件的檔案

不要將整個 `DBCacheServer` 專案或其內部類別直接作為新函式庫相依項目。函式庫只應重新定義對外 JSON 契約需要的本機 DTO；否則每個 Game Server 都會被迫相依 DBCache 的執行環境與版本。

## 3. 目前已確認的協定現況

1. 支援八個 `Promo*Response` Command；未知 Command 不可轉送 CLIENT。
2. `Data["Payload"]` 是 JSON 字串，不是物件。
3. `Data["ErrorCode"]` 與合法 Payload 可同時存在，例如 `WalletInstructionFailed`；此時仍必須解包 Payload，但 `Success=false`。
4. `Data`、其鍵值與 Payload 都可能缺少。正常協定錯誤不可中斷接收迴圈。
5. 金額欄位使用 `decimal`；識別碼使用 `long` 或 `string`；時間欄位保留 ISO 8601 字串。
6. 目前正式 `PromoClaimUnlockResponse` 只預期 `ErrorCode=NotImplemented` 且沒有 Payload。`PromoMockResponse.cs` 的解鎖成功 Payload 僅供 Mock 測試，不能視為正式已上線契約。

## 4. 建議專案結構

預設以 C#、.NET 10 建立：

```text
Promotion.ResponseDecoder.sln
├─ src/
│  └─ Promotion.ResponseDecoder/
│     ├─ Contracts/
│     │  ├─ PromoCommonResponse.cs
│     │  ├─ PromoDecodeResult.cs
│     │  ├─ PromoCommands.cs
│     │  └─ Payloads/
│     │     ├─ PromoGetPlayerOffersPayload.cs
│     │     ├─ PromoGetGamesPayload.cs
│     │     ├─ PromoClaimPayload.cs
│     │     ├─ PromoGetTaskPayload.cs
│     │     ├─ PromoAbandonPayload.cs
│     │     ├─ PromoGetHistoryPayload.cs
│     │     └─ NestedPayloadDtos.cs
│     ├─ Decoding/
│     │  └─ PromoResponseDecoder.cs
│     └─ Serialization/
│        └─ PromoJsonSerializer.cs
├─ tests/
│  └─ Promotion.ResponseDecoder.Tests/
│     ├─ Fixtures/
│     └─ PromoResponseDecoderTests.cs
└─ docs/
   └─ source-contracts/
```

建議命名空間為 `Company.Promotion.ResponseDecoder`；`Company` 改成公司既有根命名空間。不要使用 `DBCacheServer` 或任一 Game Server 名稱作為函式庫根命名空間。

## 5. 目標框架與 JSON 選擇

### 預設選擇

- Target Framework（目標框架）：`net10.0`。
- JSON Serializer（JSON 序列化器）：Newtonsoft.Json（JSON.NET）13.0.4。

理由：目前 DBCache 正以 Newtonsoft.Json 產生正式 Payload；使用相同序列化器可降低 public field、空值及既有行為的差異。

建立命令範例：

```powershell
dotnet new sln --name Promotion.ResponseDecoder
dotnet new classlib --framework net10.0 --name Promotion.ResponseDecoder --output src/Promotion.ResponseDecoder
dotnet new xunit --framework net10.0 --name Promotion.ResponseDecoder.Tests --output tests/Promotion.ResponseDecoder.Tests
dotnet add src/Promotion.ResponseDecoder package Newtonsoft.Json --version 13.0.4
dotnet add tests/Promotion.ResponseDecoder.Tests reference src/Promotion.ResponseDecoder
dotnet sln add src/Promotion.ResponseDecoder/Promotion.ResponseDecoder.csproj
dotnet sln add tests/Promotion.ResponseDecoder.Tests/Promotion.ResponseDecoder.Tests.csproj
```

若有 Game Server 無法執行 .NET 10，才依其實際版本改採較低目標框架（例如 `net6.0`）；若必須同時支援不同世代服務，再評估 `netstandard2.0`。這是相容性決策，須先盤點所有目標服務後再變更。不可在未確認相容需求前同時維護多個 Target Framework。

## 6. 公開 API 設計

函式庫輸入須使用中立模型，不直接接收某個 Server 的 `CommonInfoData`：

```csharp
public sealed class PromoCommonResponse
{
    public string Command { get; init; } = string.Empty;
    public long UserUID { get; init; }
    public IReadOnlyDictionary<string, string>? Data { get; init; }
    public string? Message { get; init; }
}

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
    public object? Payload { get; init; }
}

public interface IPromoResponseDecoder
{
    PromoDecodeResult Decode(PromoCommonResponse response);
}
```

實作時可將 `Payload` 的公開型別收斂為 `PromoPayloadBase`，以避免消費端使用無型別的 `object`。每一種 Payload 繼承該基底型別，消費端再依 `Command` 進行模式比對。

`Decode` 對於下列可預期輸入不可擲出例外：`Data` 缺少、鍵值缺少、空白 Payload、無效 JSON、JSON 型別不符、未知 Command。只有無法恢復的程式錯誤才由呼叫端的全域例外處理器記錄。

## 7. 解包規則

`PromoResponseDecoder.Decode` 必須完全遵循以下規則：

|條件|結果|
|---|---|
|未知 Command|`IsSupportedCommand=false`；不建立可傳送給 CLIENT 的結果。|
|有 ErrorCode、無 Payload|保留 ErrorCode，`Success=false`。|
|有 ErrorCode、有合法 Payload|保留 ErrorCode 與解出的 Payload，`Success=false`。|
|無 ErrorCode、有合法 Payload|`Success=true`。|
|無 ErrorCode、無 Payload|`ErrorCode="MissingResponsePayload"`，`Success=false`。|
|Payload 無效或型別不符|`ErrorCode="InvalidResponsePayload"`，`Success=false`，Payload 為 `null`。|

`RequestId`、`ErrorCode`、`Payload` 和 `IsMock` 都必須使用 `TryGetValue` 安全讀取。`IsMock` 僅在值為不分大小寫的 `true` 時為 `true`。

`PromoClaimUnlockResponse` 在目前版本沒有可解析的正式成功 Payload；若收到其正式回覆且未帶 Payload，依一般 ErrorCode 失敗回覆處理。正式契約實作成功 Payload 前，不建立推測性的 DTO。

## 8. 各 Game Server 的使用方式

每台 Game Server 建立自己的 `PromotionResponseAdapter`，流程如下：

```text
收到 CommonInfoData
  → 判斷是否為 Promo*Response
  → 轉為 PromoCommonResponse
  → decoder.Decode
  → 未支援：不轉送 CLIENT，寫入系統記錄
  → 依 UserUID 找到登入玩家
  → 依 Command 將結果映射為該 Server 的 Promo*Response
  → 依既有 SignalR 規則送出 Type=Response 類別名、Content=Response JSON
```

轉接層範例：

```csharp
PromoDecodeResult decoded = _promoDecoder.Decode(new PromoCommonResponse
{
    Command = common.Command,
    UserUID = common.UserUID,
    Data = common.Data,
    Message = common.Message
});

if (!decoded.IsSupportedCommand)
{
    LogUnknownPromoResponse(common.Command, common.UserUID);
    return;
}

if (!TryGetOnlinePlayer(decoded.UserUID, out var player))
{
    LogPromoPlayerNotOnline(decoded.UserUID, decoded.RequestId);
    return;
}

object clientResponse = MapToLocalPromoResponse(decoded);
SendToPlayer(player, clientResponse);
```

`MapToLocalPromoResponse` 必須將 `RequestId`、`ErrorCode`、`Message`、`Success`、`IsMock` 原樣映射，並依 `Command` 將已轉型的 Payload 寫入該 Server 的 Response。此層不可重新判斷成功與否，也不可因 `WalletInstructionFailed` 丟棄 Payload。

## 9. 測試與驗收

每個 Command 至少應有一個合法 JSON 測試。另須覆蓋下列案例：

1. `PromoGetTaskResponse`：合法 Payload、無 ErrorCode，預期 `Success=true`。
2. `PromoClaimResponse`：`EligibilityExpired`、無 Payload，預期保留錯誤且不擲出例外。
3. `PromoClaimResponse`：`WalletInstructionFailed`、合法 Payload，預期 Payload 存在且 `Success=false`。
4. `PromoGetGamesResponse`：無 ErrorCode、無 Payload，預期 `MissingResponsePayload`。
5. `PromoGetHistoryResponse`：Payload 為 `{bad}`，預期 `InvalidResponsePayload`。
6. `PromoClaimUnlockResponse`：`NotImplemented`、無 Payload，預期一般失敗結果。
7. `Data=null`、空字典、鍵值為 null、未知 Command。

測試資料優先來自 `PromoMockResponse.cs`，另需自行建立 ErrorCode 與錯誤 JSON Fixture。不可只測試 Mock 成功路徑。

## 10. 先以手動 DLL 發佈

初期可使用下列方式：

```powershell
dotnet test
dotnet build src/Promotion.ResponseDecoder/Promotion.ResponseDecoder.csproj -c Release
```

將 `src/Promotion.ResponseDecoder/bin/Release/net10.0/` 內的 DLL 與其相依 DLL 複製至各 Game Server 的受控 `lib/Promotion.ResponseDecoder/<版本>/` 目錄，再以專案參考加入。每一次複製都必須紀錄版本號、組件 SHA-256 與部署的 Game Server 清單。

手動引用期間，禁止以相同檔名覆蓋而不改版本資料夾；否則無法追查哪一台 Server 使用哪個協定版本。

## 11. 日後轉為內部 NuGet

函式庫 API 與 DTO 無須改寫，只需在 `.csproj` 增加封裝資訊：

```xml
<PropertyGroup>
  <PackageId>Company.Promotion.ResponseDecoder</PackageId>
  <Version>1.0.0</Version>
  <Authors>Company</Authors>
  <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
</PropertyGroup>
```

產生套件：

```powershell
dotnet pack src/Promotion.ResponseDecoder/Promotion.ResponseDecoder.csproj -c Release
```

之後將 Game Server 的 DLL `<Reference>` 改為：

```xml
<PackageReference Include="Company.Promotion.ResponseDecoder" Version="1.0.0" />
```

套件版本規則：

- 修補 JSON 例外處理或不改公開 API 的修正：Patch，例如 `1.0.1`。
- 新增可選欄位或新的向後相容 Command：Minor，例如 `1.1.0`。
- 修改既有欄位型別、移除公開 API 或改變錯誤判斷：Major，例如 `2.0.0`。

## 12. 開工前確認清單

- [ ] 所有目標 Game Server 都可執行 `net10.0`；若否，確認需要支援的最低目標框架。
- [ ] 已取得每台 Game Server 的 `CommonInfoData`、Promo Response 與接收／送包程式。
- [ ] 已複製第 2 節所列的必要契約文件。
- [ ] 已確認 CLIENT Response 是持有 `Payload`，或需要把 Payload 欄位扁平複製。
- [ ] 已確認每台 Server 對未知 Command、玩家離線與記錄敏感資料的既有規則。
- [ ] 已建立第 9 節的全部單元測試，且測試通過。

未完成以上確認前，不應將函式庫接入會對 CLIENT 發送正式優惠結果的流程。
