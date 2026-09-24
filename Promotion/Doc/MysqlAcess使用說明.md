# MysqlAcess 使用說明

## 目的

本文件說明 `DBCacheServer/MysqlAcess.cs` 的資料庫存取方式，供 DBCache 之後加功能時直接參考。讀完本文件即可決定用哪一套 API、字典值怎麼填、錯誤怎麼處理，不必再分析原始碼。

- 原始碼：`DBCacheServer/MysqlAcess.cs`
- 參考實作：`DBCacheServer/DBCacheData/UserSessionReward.cs`
- 連線設定：執行目錄下的 `SqlConnection.txt`
- 目標語言／資料庫：C#、MySQL

若本文件與原始碼不一致，以原始碼為準，並同步更新本文件。

---

## 快速決策（新功能請先看這裡）

| 情境 | 使用方法 | 不要使用 |
|---|---|---|
| 新增一列，條件為欄位相等 | `InsertParameterized` | `insert` |
| 更新列，條件全部是 `AND` + 相等 | `UpdateParameterized` | `update` |
| 查詢列，條件全部是 `AND` + 相等 | `SelectParameterized` | `select` |
| 需要 `BETWEEN`、`!=`、`OR`、`IN`、`LIKE`、`ORDER BY`、`LIMIT` | 先擴充參數化 API，或暫時用舊 `select`／`update(..., where)` | 不要把複雜 SQL 片段塞進參數化 dictionary |
| 刪除 | 目前只有舊 `delete`（字串 WHERE） | 尚無 `DeleteParameterized` |
| 維護既有呼叫、行為必須與舊程式相同 | 沿用該處已在用的舊方法 | 不要只改方法名、不改字典值格式 |

**新功能預設走參數化查詢（Parameterized Query）。** 舊方法是字串拼接（String Concatenation），全專案仍大量使用，但有 SQL 注入（SQL Injection）風險，且 `insert`／`update`／`replace` 裡的 `AddWithValue` **沒有真正綁到 SQL**。

兩套 API **不可混用同一份 dictionary**：

- 參數化：值是純資料，**不要**加單引號。例如 `100`、`alice`。
- 舊方法：呼叫端常把 SQL 語法一起放進值裡。字串欄常寫 `"'alice'"`，WHERE 也是整段 SQL。

---

## 取得實例與連線

```csharp
MysqlAcess mysql = MysqlAcess.GetInstance();
```

- 單例（Singleton）。第一次呼叫時讀 `SqlConnection.txt` 並開啟一條共用 `MySqlConnection`。
- 設定檔格式（每行 `鍵:值`）：

```text
DBIP:127.0.0.1
DBID:root
DBPWD:password
DBDataBase:dbname
```

- 連線字串固定加上：`SslMode=Disabled`、`allowpublickeyretrieval=true`、`charset=utf8`、`Allow User Variables=True`。
- 不要自己 `new MysqlAcess()`（建構子是 private）。

---

## 參數化 API（新功能用這套）

### 共同規則

1. 表名、欄名只能是英數字與底線，且不可從數字開頭。例如 `UserUID` 可以，`User-UID`、`123Table`、`` `UserUID` `` 不行。
2. 表名與欄名由 API 加上反引號；呼叫端不要自己加 `` ` ``。
3. 值綁成 `@set_欄名`（寫入）或 `@where_欄名`（條件），**不拼進 SQL**。
4. dictionary 的值不得為 `null`。資料庫 NULL 目前沒有對應寫法；讀回來的 NULL 會變成空字串 `""`。
5. 條件全部是 `AND` + 相等，沒有 `OR`、`IN`、`BETWEEN`、`ORDER BY`、`LIMIT`。
6. 沿用共用連線與互斥鎖（Mutex）。第一次失敗會記錄、重連再試一次；第二次仍失敗則擲出例外。
7. 呼叫端必須用 `try/catch` 處理例外，不要假設一定成功。

識別字驗證失敗、dictionary 為 null／空、Update 沒有 where，都會在進資料庫前擲出 `ArgumentException` 或 `ArgumentNullException`。

### InsertParameterized

```csharp
public MysqlParameterizedWriteResult InsertParameterized(
    string tableName,
    Dictionary<string, string> data)
```

回傳：

| 欄位 | 意義 |
|---|---|
| `AffectedRows` | 影響列數 |
| `LastInsertedId` | 本連線本次新增的自動增量（Auto Increment）。沒有自動增量或失敗時可能為 `0` |

```csharp
var data = new Dictionary<string, string>
{
    ["UserUID"] = userUid.ToString(CultureInfo.InvariantCulture),
    ["RecordStatus"] = "0",
    ["StartTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture)
};

MysqlParameterizedWriteResult result = MysqlAcess.GetInstance()
    .InsertParameterized("UserSessionRewardRecord", data);

long newId = result.LastInsertedId;
```

實際送出的 SQL 形如：

```sql
INSERT INTO `UserSessionRewardRecord` (`UserUID`, `RecordStatus`, `StartTime`)
VALUES (@set_UserUID, @set_RecordStatus, @set_StartTime)
```

流水號請用 `LastInsertedId`，不要再 `SELECT ... ORDER BY id DESC LIMIT 1`。

### UpdateParameterized

```csharp
public long UpdateParameterized(
    string tableName,
    Dictionary<string, string> data,
    Dictionary<string, string> where)
```

- `data`、`where` 都不可為 null 或空。沒有 where 會直接擲出例外（避免誤更新整張表）。
- 回傳影響列數。影響 `0` 表示條件沒有對到列，不一定是錯誤。

```csharp
var data = new Dictionary<string, string>
{
    ["RecordStatus"] = "1",
    ["EndTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture)
};
var where = new Dictionary<string, string>
{
    ["UserUID"] = userUid.ToString(CultureInfo.InvariantCulture),
    ["RewardRecordId"] = rewardRecordId.ToString(CultureInfo.InvariantCulture)
};

long affected = MysqlAcess.GetInstance()
    .UpdateParameterized("UserSessionRewardRecord", data, where);
```

實際 WHERE 形如：

```sql
UPDATE `UserSessionRewardRecord`
SET `RecordStatus` = @set_RecordStatus, `EndTime` = @set_EndTime
WHERE `UserUID` = @where_UserUID AND `RewardRecordId` = @where_RewardRecordId
```

### SelectParameterized

```csharp
public List<Dictionary<string, string>> SelectParameterized(
    string tableName,
    IReadOnlyList<string> fields,
    Dictionary<string, string> where)
```

- `fields` 為 null 或空：查 `*`。
- `where` 為 null 或空：不加 WHERE（會整表掃描，請謹慎）。
- `fields` 不可重複。
- 回傳永遠是清單，不會是 `null`。沒有資料時為空清單。
- 資料庫 NULL → `""`。
- `DateTime` 讀回固定格式 `yyyy-MM-dd HH:mm:ss.ffffff`（不受機器地區設定影響）。
- 其他型別用 `CultureInfo.InvariantCulture` 轉成字串。

```csharp
string[] fields = { "RewardRecordId", "UserUID", "RecordStatus", "StartTime" };
var where = new Dictionary<string, string>
{
    ["UserUID"] = userUid.ToString(CultureInfo.InvariantCulture),
    ["RecordStatus"] = "0"
};

List<Dictionary<string, string>> rows = MysqlAcess.GetInstance()
    .SelectParameterized("UserSessionRewardRecord", fields, where);

if (rows.Count == 0)
{
    return;
}

string idText = rows[0]["RewardRecordId"];
```

讀回後請自行轉型。日期建議同時接受微秒與秒精度：

```csharp
const string FormatMicro = "yyyy-MM-dd HH:mm:ss.ffffff";
const string FormatSec = "yyyy-MM-dd HH:mm:ss";

if (!DateTime.TryParseExact(value, FormatMicro, CultureInfo.InvariantCulture,
        DateTimeStyles.AssumeLocal, out DateTime parsed)
    && !DateTime.TryParseExact(value, FormatSec, CultureInfo.InvariantCulture,
        DateTimeStyles.AssumeLocal, out parsed))
{
    throw new InvalidOperationException("無法解析時間。值=" + value);
}
```

數字請用 `int.Parse(text, CultureInfo.InvariantCulture)` 或 `double.Parse(text, CultureInfo.InvariantCulture)`，不要用會吃地區設定的預設 `Parse`。

---

## 值格式（參數化 dictionary）

呼叫端負責把 C# 值變成穩定字串。建議一律用 `CultureInfo.InvariantCulture`，小數點用 `.`。

| C# 型別 | 建議寫法 | 正確例子 | 錯誤例子 |
|---|---|---|---|
| `int`／`long` | `value.ToString(CultureInfo.InvariantCulture)` | `123` | `"123" `（多空白通常沒問題，但不要加引號） |
| `byte` 列舉 | `((byte)enumValue).ToString(CultureInfo.InvariantCulture)` | `0` | `InProgress` |
| `double` | `value.ToString(CultureInfo.InvariantCulture)` | `10.5` | `10,5`、`"'10.5'"` |
| `DateTime` | `value.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture)` | `2026-09-04 09:49:00.000000` | `'2026-09-04 09:49:00'` |
| `string` | 原字串，不要包引號 | `alice` | `'alice'`、`"'alice'"` |
| `null` | 不允許 | — | `null` |

參考格式化實作：`UserSessionReward.Core` 的 `MysqlValueFormatter`。新功能若沒有 Core 可依賴，請在呼叫端用同一套規則，不要各寫各的。

---

## 參數化能力邊界

目前做得到：

- `INSERT` 指定欄位
- `UPDATE ... SET ... WHERE col1=@p1 AND col2=@p2`
- `SELECT col1, col2 FROM table WHERE col1=@p1 AND col2=@p2`
- `SELECT * FROM table`（where 為空時）

目前做不到（請先擴充 API，不要硬塞舊 where 字串）：

- `DeleteParameterized`
- `OR`、`IN`、`BETWEEN`、`!=`、`IS NULL`、`LIKE`
- `ORDER BY`、`LIMIT`、`JOIN`、分頁
- 把資料庫 NULL 寫進去
- 動態表名／欄名來自外部輸入（識別字雖有白名單，仍應由程式常數或內部對照表提供）

需要 `ORDER BY`／`LIMIT` 時的權宜作法：用參數化查出候選列，再在 C# 排序／取前 N 筆。資料量大時應擴充 API，不要整表拉回。

---

## 舊 API（只給既有程式與無法參數化的特例）

舊方法把表名、欄位、WHERE、值直接拼成 SQL。`insert`／`update`／`replace` 雖然呼叫了 `Parameters.AddWithValue`，SQL 裡沒有 `@參數`，那些參數不會生效。

### 方法一覽

| 方法 | 簽名重點 | 回傳 | 行為摘要 |
|---|---|---|---|
| `insert(table, Dictionary)` | 值直接拼進 `VALUE (...)` | `int`：自動增量或 `1`／失敗 `-1` | 成功後用 `SHOW COLUMNS` + `SELECT ... DESC LIMIT 1` 猜新 ID，高併發可能拿到別人的 ID |
| `insert(table, string)` | 整段 `updatedata` 接在 `INSERT INTO table` 後面 | 同上 | 標成「沒用到的」，不要新用 |
| `replace(table, Dictionary)` | 同 insert，語法是 `REPLACE INTO` | 影響列數或 `0` | 假參數化 |
| `select(table, fields, where, order, limit)` | 全部字串拼接 | `List<Dictionary<string, string>>`，失敗為空清單 | **每次開新連線**，不用 Mutex |
| `selectdesc(...)` | 同 select，固定加 `DESC` | 清單或 `null` | 走共用連線 + Mutex |
| `update(table, data, col, value)` | `SET` 與 `WHERE col=value` 都拼接 | 影響列數或 `-1` | `value` 若是字串，呼叫端要自己加 `'...'` |
| `update(table, data, where)` | `WHERE` 為任意 SQL 片段 | 同上 | 可寫 `BETWEEN`、`!=` 等 |
| `get(...)` | 內部呼叫 `select(..., limit: "1")` | 一筆 dictionary；沒有資料時為空 dictionary（不是 null） | |
| `delete(table, where)` | `WHERE` 為任意 SQL 片段 | 影響列數或 `-1` | 沒有參數化版本 |

舊 `select` 範例（既有程式常見寫法，**新功能不要學**）：

```csharp
var rows = mysql.select(
    "Usertable",
    "UserUID, UserBalance",
    string.Format("UserUID = {0}", userUid));

var rows2 = mysql.select(
    "AspNetUsers",
    "*",
    string.Format("UserName = '{0}'", account));  // account 含單引號即可注入
```

舊 `update` 字串欄必須由呼叫端加引號：

```csharp
mysql.update("AspNetUsers", data, "Id", "'" + userId + "'");
```

若把「沒有引號的純值 dictionary」丟進舊 `insert`／`update`，字串欄會變成 SQL 語法錯誤或被當成識別字。反之，把 `"'alice'"` 丟進參數化，資料庫會存進含引號的字面值。

### 舊方法何時勉強可用

- 必須 `BETWEEN`／`OR`／`IN`，且尚未擴充參數化 API。
- 必須刪除，且 WHERE 來源完全受程式控制（常數或已驗證的內部識別字 + 數值）。
- 修改舊流程且要保持原行為。

即使走舊方法，WHERE 與值也不要直接串外部輸入。能改成參數化就改。

---

## 連線、鎖與錯誤

| | 參數化 Insert／Update／Select | 舊 `select` | 舊 insert／update／delete／`selectdesc` |
|---|---|---|---|
| 連線 | 共用 `dbConnection` | 每次 `new MySqlConnection` | 共用 `dbConnection` |
| 鎖 | 同一把 `m_mutex` | 無 | `m_mutex` |
| 失敗 | 記錄 → 重連再試 → 仍失敗則擲出例外 | 記錄後回空清單 | 記錄 → 重連再試 → 回 `-1` 或 `null` |

注意：

- 參數化讀寫與舊寫入共用同一把鎖，會互相等待。
- 舊 `select` 不占這把鎖，較不容易被寫入堵住，但也因此與寫入沒有同一條連線上的先後保證。
- 參數化失敗會丟例外；舊方法常回 `-1`。不要用舊方法的「檢查回傳值」習慣去接參數化 API。
- 自動重試一次可能讓「看起來像逾時」的呼叫實際執行了兩次。`INSERT` 若沒有唯一鍵，理論上可能寫入兩列。需要嚴格一次語意時，請用唯一鍵或交易（目前 API 沒有公開交易）。

---

## 建議的新功能寫法

以「依 UserUID 查一筆、沒有就新增、有就更新」為例：

```csharp
using System.Globalization;

MysqlAcess mysql = MysqlAcess.GetInstance();
string table = "YourTableName";  // 必須是程式常數，不可來自封包欄位

var where = new Dictionary<string, string>
{
    ["UserUID"] = userUid.ToString(CultureInfo.InvariantCulture)
};

List<Dictionary<string, string>> rows = mysql.SelectParameterized(
    table,
    new[] { "UID", "UserUID", "Amount" },
    where);

var data = new Dictionary<string, string>
{
    ["UserUID"] = userUid.ToString(CultureInfo.InvariantCulture),
    ["Amount"] = amount.ToString(CultureInfo.InvariantCulture)
};

if (rows.Count == 0)
{
    MysqlParameterizedWriteResult inserted = mysql.InsertParameterized(table, data);
    long newId = inserted.LastInsertedId;
}
else
{
    long affected = mysql.UpdateParameterized(table, data, where);
}
```

對照完整轉接器（含表名／欄位白名單、Insert 禁止 where、讀回日期解析）：`DBCacheData/UserSessionReward.cs` 的 `HostUserSessionRewardRecordReader` 與 `HostUserSessionRewardRecordWriter`。

新功能若只服務一張表，建議在自己的類別再包一層白名單，不要讓任意字串當表名／欄名傳到 `MysqlAcess`。

---

## 常見錯誤

| 症狀 | 原因 | 處理 |
|---|---|---|
| `SQL 識別字只能是英數字與底線` | 表名／欄名含 `-`、空白、反引號或中文 | 只用程式常數，名稱符合規則 |
| `寫入資料字典不得為空`／`Update 操作必須提供 where` | 傳了空 dictionary 或 Update 沒條件 | 補齊 data／where |
| `欄位值不得為 null` | dictionary 值是 `null` | 改空字串或不要寫該欄（目前無法寫 DB NULL） |
| 字串欄存進 `'alice'`（含引號） | 把舊方法的 dictionary 直接拿去參數化 | 去掉呼叫端加的單引號 |
| 舊 `insert`／`update` 語法錯誤 | 把參數化的純值 dictionary 拿去舊方法 | 字串欄要由呼叫端加 `'...'`，或改走參數化 |
| 參數化查不到資料，但舊 `select` 查得到 | WHERE 能力不同，或值多了引號 | 對一下實際綁定值；複雜條件需擴充 API |
| `LastInsertedId` 為 `0` | 表沒有自動增量，或新增失敗 | 先確認表結構；失敗應已擲出例外 |
| 取到別人的新 ID | 用了舊 `insert` 的 DESC 回查 | 改 `InsertParameterized` |
| 第二次重試也失敗 | 連線或 SQL 本身有問題 | 看 console 的 `Acess InsertParameterized 表名` 與例外堆疊 |

---

## 維護備註

改 `MysqlAcess.cs` 時請同步本文件，至少檢查：

1. 是否新增／刪除公開方法（例如 `DeleteParameterized`、`ORDER BY`、`LIMIT`）。
2. 參數化 WHERE 是否仍只有 `AND` + 相等。
3. 值格式、NULL 規則、識別字規則是否改變。
4. 連線、Mutex、重試、例外 vs 回傳 `-1` 是否改變。
5. `LastInsertedId` 與讀回日期格式是否改變。

文件日期：2026-09-04。對應原始碼：`MysqlAcess.cs`（參數化區段約第 1144 行起）。

---

## V2 擴充（2026-09-16）

實際 V2 原始碼位於 DBCache 專案的 `MysqlAcess.V2.cs`、`MysqlAcess.V2.Models.cs` 與 `MysqlAcess.V2.CommandBuilder.cs`；原 `MysqlAcess.cs` 只將類別宣告加上 `partial`，既有方法本體未改動。本節與上文的舊版 API 說明分開閱讀；目前 DBCache 原始檔並沒有上文描述的所有舊版 `*Parameterized` 方法。

```csharp
var mysql = MysqlAcess.GetInstance();
var count = mysql.ExecuteParameterizedTransaction(context =>
{
    var rows = context.Select(new MysqlSelectCommand(
        "PromotionActivity", new[] { "UID" },
        new[] { new MysqlCondition("UID", MysqlComparisonOperator.Equal, activityId) },
        Array.Empty<MysqlOrder>(), 1, null, MysqlLockMode.ForUpdate));
    return rows.Count;
});
```

- `ExecuteParameterizedTransaction<T>` 預設 `ReadCommitted`；每次呼叫建立並持有獨立 Connection，交易內所有 Command 使用同一 Connection 與 Transaction，結束後釋放。V2 不共用舊 API 的 Connection 或可被 `ReConnect` 重新指定的 Mutex。action 成功才 Commit，失敗時嘗試 Rollback，交易完成後 Context 失效。交易內不做單一 Command 自動重送。
- `MysqlTransactionContext` 提供 `Insert`、`Update`、`Select`、`ExecuteScalar` 與 `Delete`。非交易 V2 提供 `SelectParameterizedV2` 與 `DeleteParameterizedV2`；非交易 `Select` 不可指定 `ForUpdate`。
- 條件只支援固定 enum 運算子與 `AND`；欄名與表名僅限 ASCII 英文字母、數字、底線，首字須為英文字母或底線。值均以參數綁定；`null` 是 SQL `NULL`；`Limit` 上限為 1000。
- `Update`／`Delete` 必須提供非空 `Where`。重複鍵、死鎖、鎖等待逾時、連線錯誤與 Commit 結果不明透過 `PromotionDataException.Kind` 分類。Commit 結果不明時，呼叫端需先按業務識別碼查證，不可直接重送。
- V2 會拒絕巢狀 V2 Transaction，以及在 V2 Transaction action 內呼叫非交易 V2 API。由於本次約束是不修改舊 API 方法本體，**尚無法全面阻止 action 內呼叫舊 API**；即使兩者現在使用不同 Connection，在舊 API 上執行的操作也不會加入 V2 Transaction。呼叫端必須遵守禁用規則，此項仍是 PCS-03 的驗收缺口。
- 核心 Adapter 須自行限制表名與欄名白名單，V2 的字元檢查不能代替業務白名單。
