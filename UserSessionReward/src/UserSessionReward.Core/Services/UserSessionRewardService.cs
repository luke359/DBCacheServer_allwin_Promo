using UserSessionReward.Core.Abstractions;
using UserSessionReward.Core.Enums;
using UserSessionReward.Core.Models;

namespace UserSessionReward.Core.Services;

/// <summary>
/// 執行玩家上升額度 Session 的新增、結束與統計更新商業流程。
/// </summary>
/// <remarks>
/// 每次公開操作都會以玩家 UID 序列化。此服務只讀寫 Session 紀錄邊界，並只接收或回傳 <see cref="RewardCacheDataModel"/>；外部快取的保存、讀取與清除責任不在 Core。
/// </remarks>
public sealed class UserSessionRewardService
{
    /// <summary>
    /// 指定本功能使用的 MySQL 資料表白名單名稱。
    /// </summary>
    private const string UserSessionRewardRecordTableName = "UserSessionRewardRecord";

    /// <summary>
    /// 產生新 Session 目標餘額的外部規則。
    /// </summary>
    private readonly ITargetBalanceGenerator _targetBalanceGenerator;

    /// <summary>
    /// 讀取 Session 紀錄的外部資料存取實作。
    /// </summary>
    private readonly IUserSessionRewardRecordReader _recordReader;

    /// <summary>
    /// 寫入 Session 紀錄的 MySQL 邊界實作。
    /// </summary>
    private readonly IUserSessionRewardRecordWriter _recordWriter;

    /// <summary>
    /// 確保同一玩家操作依序完成的序列化實作。
    /// </summary>
    private readonly IUserOperationSerializer _operationSerializer;

    /// <summary>
    /// 提供所有寫入與紀錄所需本機時間的來源。
    /// </summary>
    private readonly IClock _clock;

    /// <summary>
    /// 記錄商業流程錯誤與警告的外部實作。
    /// </summary>
    private readonly IUserSessionRewardLogger _logger;

    /// <summary>
    /// 初始化玩家上升額度 Session 服務。
    /// </summary>
    /// <param name="targetBalanceGenerator">外部注入的目標餘額產生器；Core 只原樣轉入本次開分值與宿主提供的 <c>rewardWebSetting</c>，不解析或保存該字串。</param>
    /// <param name="recordReader">依玩家與紀錄流水號讀取 Session 的資料存取實作。</param>
    /// <param name="recordWriter">以 <see cref="Dictionary{TKey,TValue}"/> 寫入 MySQL 欄位的實作。</param>
    /// <param name="operationSerializer">以玩家 UID 為鍵的同玩家序列化實作。</param>
    /// <param name="clock">可替換的伺服器本機時間來源。</param>
    /// <param name="logger">用於保留錯誤與警告脈絡的記錄實作。</param>
    /// <exception cref="ArgumentNullException">當任一相依服務為 <see langword="null"/> 時擲回。</exception>
    public UserSessionRewardService(
        ITargetBalanceGenerator targetBalanceGenerator,
        IUserSessionRewardRecordReader recordReader,
        IUserSessionRewardRecordWriter recordWriter,
        IUserOperationSerializer operationSerializer,
        IClock clock,
        IUserSessionRewardLogger logger)
    {
        _targetBalanceGenerator = targetBalanceGenerator ?? throw new ArgumentNullException(nameof(targetBalanceGenerator));
        _recordReader = recordReader ?? throw new ArgumentNullException(nameof(recordReader));
        _recordWriter = recordWriter ?? throw new ArgumentNullException(nameof(recordWriter));
        _operationSerializer = operationSerializer ?? throw new ArgumentNullException(nameof(operationSerializer));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 建立一筆新的進行中 Session，必要時先以再開分結束舊的進行中 Session。
    /// </summary>
    /// <param name="userUid">本次開分的玩家 UID。</param>
    /// <param name="keyInAmount">本次建立 Session 的開分值；本服務不額外限制正負值。</param>
    /// <param name="balanceBeforeTransaction">本次開分交易套用前的玩家餘額。</param>
    /// <param name="rewardWebSetting">宿主提供的目標餘額規則設定字串；本服務不解析、不驗證、不保存，僅原樣轉入外部產生器。</param>
    /// <returns>新增資料庫紀錄成功後建立的 <see cref="RewardCacheDataModel"/>，由外部宿主保存。</returns>
    /// <exception cref="InvalidOperationException">當舊 Session 無法結束、資料完整性不正確、產生器沒有有效目標餘額，或新增寫入未成功時擲回。</exception>
    /// <remarks>
    /// 寫入的 <c>BalanceAfterKeyIn</c> 為交易前餘額加上 <paramref name="keyInAmount"/>，且 <c>StartTime</c> 一律使用伺服器本機時間。
    /// <paramref name="rewardWebSetting"/> 不會寫入資料庫或 <see cref="RewardCacheDataModel"/>。
    /// </remarks>
    public RewardCacheDataModel CreateSession(int userUid, double keyInAmount, double balanceBeforeTransaction, string rewardWebSetting)
    {
        return _operationSerializer.Execute(userUid, () => CreateSessionCore(userUid, keyInAmount, balanceBeforeTransaction, rewardWebSetting));
    }

    /// <summary>
    /// 以再次開分原因首次結束指定 Session。
    /// </summary>
    /// <param name="userUid">紀錄必須相符的玩家 UID。</param>
    /// <param name="rewardRecordId">欲結束的資料庫紀錄流水號。</param>
    /// <param name="balanceBeforeTransaction">本次再開分交易套用前的玩家餘額，將在首次結束時寫入 <c>EndBalance</c>。</param>
    /// <returns>實際寫入成功、已結束而未動作，或錯誤拒絕的結果。</returns>
    /// <remarks>
    /// 已結束 Session 不會覆寫 <c>EndBalance</c>、<c>EndTime</c>、<c>KeyInAmount</c> 或功能首次終止資料；時間一律使用伺服器本機時間。
    /// </remarks>
    public SessionOperationResult EndSession(
        int userUid,
        long rewardRecordId,
        double balanceBeforeTransaction)
    {
        return _operationSerializer.Execute(
            userUid,
            () => EndSessionCore(userUid, rewardRecordId, balanceBeforeTransaction));
    }

    /// <summary>
    /// 將一次成功洗分的洗分值與交易套用前餘額追加至指定的進行中 Session。
    /// </summary>
    /// <param name="userUid">紀錄必須相符的玩家 UID。</param>
    /// <param name="rewardRecordId">欲追加洗分資料的資料庫紀錄流水號。</param>
    /// <param name="keyOutAmount">本次洗分值，截斷至四位小數後必須是大於零的有限數值。</param>
    /// <param name="balanceWhenKeyOut">本次洗分交易套用前的玩家餘額，必須是非負有限數值。</param>
    /// <returns>實際追加成功或錯誤拒絕的結果。</returns>
    /// <remarks>洗分不結束 Session，也不修改功能狀態、結束欄位、統計資料或額外資訊。</remarks>
    public SessionOperationResult RecordKeyOut(
        int userUid,
        long rewardRecordId,
        double keyOutAmount,
        double balanceWhenKeyOut)
    {
        return _operationSerializer.Execute(
            userUid,
            () => RecordKeyOutCore(userUid, rewardRecordId, keyOutAmount, balanceWhenKeyOut));
    }

    /// <summary>
    /// 更新進行中 Session 的遊戲統計，並在首次自然或 Web 關閉時寫入功能終止資料。
    /// </summary>
    /// <param name="userUid">紀錄必須相符的玩家 UID。</param>
    /// <param name="rewardCacheData">由外部宿主保存後回傳的快取快照。</param>
    /// <param name="currentBalance">為維持既有 API 相容性而保留；Core 不使用此值。</param>
    /// <returns>實際寫入成功、已結束而未動作，或錯誤拒絕的結果。</returns>
    /// <exception cref="ArgumentNullException">當 <paramref name="rewardCacheData"/> 為 <see langword="null"/> 時擲回。</exception>
    /// <remarks>
    /// 功能已自然或強制關閉時，只要 Session 仍進行中仍可更新統計；Core 不保存或回寫外部快取，且所有寫入時間均由本機時鐘提供。
    /// </remarks>
    public SessionOperationResult UpdateSession(int userUid, RewardCacheDataModel rewardCacheData, double currentBalance)
    {
        ArgumentNullException.ThrowIfNull(rewardCacheData);
        _ = currentBalance;
        return _operationSerializer.Execute(userUid, () => UpdateSessionCore(userUid, rewardCacheData));
    }

    /// <summary>
    /// 在既有玩家序列化範圍內執行新增 Session 流程。
    /// </summary>
    /// <param name="userUid">本次開分的玩家 UID。</param>
    /// <param name="keyInAmount">建立 Session 的開分值。</param>
    /// <param name="balanceBeforeTransaction">開分交易套用前的玩家餘額。</param>
    /// <param name="rewardWebSetting">宿主提供的目標餘額規則設定字串；僅原樣轉入外部產生器。</param>
    /// <returns>新增成功後的新快取資料。</returns>
    /// <exception cref="InvalidOperationException">當前置結束或新增資料庫寫入無法成功完成時擲回。</exception>
    private RewardCacheDataModel CreateSessionCore(int userUid, double keyInAmount, double balanceBeforeTransaction, string rewardWebSetting)
    {
        IReadOnlyList<UserSessionRewardRecord> inProgressRecords = _recordReader.GetInProgressByUserUid(userUid);
        if (inProgressRecords.Count > 1)
        {
            const string message = "同一玩家查到超過一筆進行中的 Session，拒絕建立新紀錄。";
            LogError("CreateSession", userUid, null, null, null, message);
            throw new InvalidOperationException(message);
        }

        if (inProgressRecords.Count == 1)
        {
            UserSessionRewardRecord oldRecord = inProgressRecords[0];
            if (!IsRecordOwnedByUser(oldRecord, userUid))
            {
                const string message = "進行中 Session 的玩家 UID 與查詢玩家不相符，拒絕建立新紀錄。";
                LogError("CreateSession", userUid, oldRecord.RewardRecordId, oldRecord.RecordStatus, oldRecord.RewardEndStatus, message);
                throw new InvalidOperationException(message);
            }

            SessionOperationResult endResult = EndSessionCore(userUid, oldRecord.RewardRecordId, balanceBeforeTransaction);
            if (!endResult.IsSuccessful)
            {
                const string message = "舊的進行中 Session 未成功結束，拒絕建立新紀錄。";
                LogError("CreateSession", userUid, oldRecord.RewardRecordId, oldRecord.RecordStatus, oldRecord.RewardEndStatus, message);
                throw new InvalidOperationException(message);
            }
        }

        double balanceAfterKeyIn = balanceBeforeTransaction + keyInAmount;
        (int rewardEndStatus, double targetBalance) = GenerateTargetBalance(userUid, balanceAfterKeyIn, keyInAmount, rewardWebSetting);
        DateTime now = GetNow();
        Dictionary<string, string> data = new()
        {
            ["UserUID"] = MysqlValueFormatter.FormatInt32(userUid),
            ["KeyInAmount"] = MysqlValueFormatter.FormatDouble(keyInAmount),
            ["BalanceAfterKeyIn"] = MysqlValueFormatter.FormatDouble(balanceAfterKeyIn),
            ["TargetBalance"] = MysqlValueFormatter.FormatDouble(targetBalance),
            ["RecordStatus"] = MysqlValueFormatter.FormatSessionRecordStatus(SessionRecordStatus.InProgress),
            ["RewardEndStatus"] = MysqlValueFormatter.FormatRewardEndStatus((RewardEndStatus)rewardEndStatus),
            ["TotalGameCount"] = MysqlValueFormatter.FormatInt32(0),
            ["TotalBet"] = MysqlValueFormatter.FormatDouble(0),
            ["MaxBalance"] = MysqlValueFormatter.FormatDouble(balanceAfterKeyIn),
            ["MinBalance"] = MysqlValueFormatter.FormatDouble(balanceAfterKeyIn),
            ["StartTime"] = MysqlValueFormatter.FormatDateTime(now)
        };

        MysqlWriteResult? writeResult = TryWrite("CreateSession", userUid, null, null, null, MysqlWriteOperation.Insert, data, null);
        if (writeResult is null || writeResult.AffectedRows != 1 || writeResult.LastInsertedId is null || writeResult.LastInsertedId <= 0)
        {
            const string message = "新增 Session 未取得一筆影響資料列與有效的 LastInsertedId。";
            LogError("CreateSession", userUid, null, null, null, message);
            throw new InvalidOperationException(message);
        }

        return new RewardCacheDataModel
        {
            RewardRecordId = writeResult.LastInsertedId.Value,
            RewardEndStatus = RewardEndStatus.InProgress,
            TargetBalance = targetBalance,
            TotalGameCount = 0,
            TotalBet = 0,
            MaxBet = null,
            MinBet = null,
            MaxBalance = balanceAfterKeyIn,
            MinBalance = balanceAfterKeyIn,
            ExtraInfo = null
        };
    }

    /// <summary>
    /// 在既有玩家序列化範圍內，以再次開分原因首次結束指定 Session。
    /// </summary>
    /// <param name="userUid">紀錄必須相符的玩家 UID。</param>
    /// <param name="rewardRecordId">欲結束的紀錄流水號。</param>
    /// <param name="balanceBeforeTransaction">再次開分交易套用前的玩家餘額。</param>
    /// <returns>實際寫入、無動作或拒絕的結果。</returns>
    private SessionOperationResult EndSessionCore(
        int userUid,
        long rewardRecordId,
        double balanceBeforeTransaction)
    {
        if (rewardRecordId <= 0)
        {
            const string message = "RewardRecordId 必須是正整數，拒絕結束 Session。";
            LogError("EndSession", userUid, rewardRecordId, null, null, message);
            return Failed(message);
        }

        UserSessionRewardRecord? record = _recordReader.GetByUserUidAndRewardRecordId(userUid, rewardRecordId);
        if (record is null || !IsRecordOwnedByUser(record, userUid))
        {
            const string message = "找不到指定玩家與 RewardRecordId 相符的 Session，拒絕結束。";
            LogError("EndSession", userUid, rewardRecordId, record?.RecordStatus, record?.RewardEndStatus, message);
            return Failed(message);
        }

        if (record.RecordStatus != SessionRecordStatus.InProgress)
        {
            const string message = "Session 已結束，保留第一次結束結果而不寫入資料庫。";
            LogWarning("EndSession", userUid, rewardRecordId, record.RecordStatus, record.RewardEndStatus, message);
            return NoOperation(message);
        }

        DateTime now = GetNow();
        Dictionary<string, string> data = new()
        {
            ["RecordStatus"] = MysqlValueFormatter.FormatSessionRecordStatus(SessionRecordStatus.EndedByReopen),
            ["EndBalance"] = MysqlValueFormatter.FormatDouble(balanceBeforeTransaction),
            ["EndTime"] = MysqlValueFormatter.FormatDateTime(now)
        };

        if (record.RewardEndStatus == RewardEndStatus.InProgress)
        {
            data["RewardEndStatus"] = MysqlValueFormatter.FormatRewardEndStatus(RewardEndStatus.PlayerEarlyClose);
        }

        Dictionary<string, string> where = BuildActiveRecordWhere(userUid, rewardRecordId);
        MysqlWriteResult? writeResult = TryWrite(
            "EndSession",
            userUid,
            rewardRecordId,
            record.RecordStatus,
            record.RewardEndStatus,
            MysqlWriteOperation.Update,
            data,
            where);
        if (writeResult?.AffectedRows == 1)
        {
            return Succeeded("Session 已成功結束。");
        }

        UserSessionRewardRecord? rereadRecord = _recordReader.GetByUserUidAndRewardRecordId(userUid, rewardRecordId);
        if (rereadRecord is not null && IsRecordOwnedByUser(rereadRecord, userUid) && rereadRecord.RecordStatus != SessionRecordStatus.InProgress)
        {
            const string message = "Session 在更新前已由其他已序列化操作結束，保留第一次結束結果。";
            LogWarning("EndSession", userUid, rewardRecordId, rereadRecord.RecordStatus, rereadRecord.RewardEndStatus, message);
            return NoOperation(message);
        }

        const string failedMessage = "結束 Session 的影響筆數不是一，且重新讀取後未確認為重複結束。";
        LogError("EndSession", userUid, rewardRecordId, record.RecordStatus, record.RewardEndStatus, failedMessage);
        return Failed(failedMessage);
    }

    /// <summary>
    /// 在既有玩家序列化範圍內，追加一次成功洗分的洗分值與交易前餘額。
    /// </summary>
    /// <param name="userUid">紀錄必須相符的玩家 UID。</param>
    /// <param name="rewardRecordId">欲追加洗分資料的紀錄流水號。</param>
    /// <param name="keyOutAmount">本次洗分值。</param>
    /// <param name="balanceWhenKeyOut">本次洗分交易套用前的玩家餘額。</param>
    /// <returns>實際寫入或拒絕的結果。</returns>
    private SessionOperationResult RecordKeyOutCore(
        int userUid,
        long rewardRecordId,
        double keyOutAmount,
        double balanceWhenKeyOut)
    {
        if (rewardRecordId <= 0)
        {
            const string message = "RewardRecordId 必須是正整數，拒絕記錄洗分。";
            LogError("RecordKeyOut", userUid, rewardRecordId, null, null, message);
            return Failed(message);
        }

        if (!double.IsFinite(keyOutAmount) || keyOutAmount <= 0 || Math.Truncate(keyOutAmount * 10000d) <= 0)
        {
            const string message = "KeyOutAmount 必須是截斷至四位小數後仍大於零的有限數值，拒絕記錄洗分。";
            LogError("RecordKeyOut", userUid, rewardRecordId, null, null, message);
            return Failed(message);
        }

        if (!double.IsFinite(balanceWhenKeyOut) || balanceWhenKeyOut < 0)
        {
            const string message = "BalanceWhenKeyOut 必須是非負有限數值，拒絕記錄洗分。";
            LogError("RecordKeyOut", userUid, rewardRecordId, null, null, message);
            return Failed(message);
        }

        UserSessionRewardRecord? record = _recordReader.GetByUserUidAndRewardRecordId(userUid, rewardRecordId);
        if (record is null || !IsRecordOwnedByUser(record, userUid))
        {
            const string message = "找不到指定玩家與 RewardRecordId 相符的 Session，拒絕記錄洗分。";
            LogError("RecordKeyOut", userUid, rewardRecordId, record?.RecordStatus, record?.RewardEndStatus, message);
            return Failed(message);
        }

        if (record.RecordStatus != SessionRecordStatus.InProgress)
        {
            const string message = "Session 已結束，拒絕記錄洗分。";
            LogError("RecordKeyOut", userUid, rewardRecordId, record.RecordStatus, record.RewardEndStatus, message);
            return Failed(message);
        }

        bool hasKeyOutAmount = !string.IsNullOrEmpty(record.KeyOutAmount);
        bool hasBalanceWhenKeyOut = !string.IsNullOrEmpty(record.BalanceWhenKeyOut);
        if (hasKeyOutAmount != hasBalanceWhenKeyOut)
        {
            const string message = "既有洗分值與洗分當下餘額不成對，拒絕追加洗分資料。";
            LogError("RecordKeyOut", userUid, rewardRecordId, record.RecordStatus, record.RewardEndStatus, message);
            return Failed(message);
        }

        string formattedKeyOutAmount = MysqlValueFormatter.FormatTruncatedKeyOutDouble(keyOutAmount);
        string formattedBalanceWhenKeyOut = MysqlValueFormatter.FormatTruncatedKeyOutDouble(balanceWhenKeyOut);
        Dictionary<string, string> data = new()
        {
            ["KeyOutAmount"] = AppendCommaSeparatedValue(record.KeyOutAmount, formattedKeyOutAmount),
            ["BalanceWhenKeyOut"] = AppendCommaSeparatedValue(record.BalanceWhenKeyOut, formattedBalanceWhenKeyOut)
        };

        Dictionary<string, string> where = BuildActiveRecordWhere(userUid, rewardRecordId);
        MysqlWriteResult? writeResult = TryWrite(
            "RecordKeyOut",
            userUid,
            rewardRecordId,
            record.RecordStatus,
            record.RewardEndStatus,
            MysqlWriteOperation.Update,
            data,
            where);
        if (writeResult?.AffectedRows == 1)
        {
            return Succeeded("洗分資料已成功追加。");
        }

        const string failedMessage = "追加洗分資料的影響筆數不是一。";
        LogError("RecordKeyOut", userUid, rewardRecordId, record.RecordStatus, record.RewardEndStatus, failedMessage);
        return Failed(failedMessage);
    }

    /// <summary>
    /// 在既有玩家序列化範圍內更新進行中 Session 的統計與首次功能關閉狀態。
    /// </summary>
    /// <param name="userUid">紀錄必須相符的玩家 UID。</param>
    /// <param name="rewardCacheData">外部宿主提供的最新快取快照。</param>
    /// <returns>實際寫入、無動作或拒絕的結果。</returns>
    private SessionOperationResult UpdateSessionCore(int userUid, RewardCacheDataModel rewardCacheData)
    {
        if (rewardCacheData.RewardRecordId <= 0)
        {
            const string message = "RewardCacheDataModel.RewardRecordId 必須是正整數，拒絕更新 Session。";
            LogError("UpdateSession", userUid, rewardCacheData.RewardRecordId, null, rewardCacheData.RewardEndStatus, message);
            return Failed(message);
        }

        if (!Enum.IsDefined(typeof(RewardEndStatus), rewardCacheData.RewardEndStatus))
        {
            const string message = "RewardCacheDataModel.RewardEndStatus 不是支援的功能狀態，拒絕更新 Session。";
            LogError("UpdateSession", userUid, rewardCacheData.RewardRecordId, null, rewardCacheData.RewardEndStatus, message);
            return Failed(message);
        }

        if (rewardCacheData.RewardEndStatus == RewardEndStatus.PlayerEarlyClose)
        {
            const string message = "一般統計更新不可自行產生 RewardEndStatus=2，拒絕更新。";
            LogError("UpdateSession", userUid, rewardCacheData.RewardRecordId, null, rewardCacheData.RewardEndStatus, message);
            return Failed(message);
        }

        UserSessionRewardRecord? record = _recordReader.GetByUserUidAndRewardRecordId(userUid, rewardCacheData.RewardRecordId);
        if (record is null || !IsRecordOwnedByUser(record, userUid))
        {
            const string message = "找不到指定玩家與 RewardRecordId 相符的 Session，拒絕更新。";
            LogError("UpdateSession", userUid, rewardCacheData.RewardRecordId, record?.RecordStatus, record?.RewardEndStatus, message);
            return Failed(message);
        }

        if (record.RecordStatus != SessionRecordStatus.InProgress)
        {
            const string message = "Session 已結束，不接受遊戲統計更新。";
            LogWarning("UpdateSession", userUid, rewardCacheData.RewardRecordId, record.RecordStatus, record.RewardEndStatus, message);
            return NoOperation(message);
        }

        Dictionary<string, string> data = new()
        {
            ["TotalGameCount"] = MysqlValueFormatter.FormatInt32(rewardCacheData.TotalGameCount),
            ["TotalBet"] = MysqlValueFormatter.FormatDouble(rewardCacheData.TotalBet),
            ["MaxBalance"] = MysqlValueFormatter.FormatDouble(rewardCacheData.MaxBalance),
            ["MinBalance"] = MysqlValueFormatter.FormatDouble(rewardCacheData.MinBalance)
        };

        if (rewardCacheData.TotalGameCount > 0 && rewardCacheData.MaxBet.HasValue)
        {
            data["MaxBet"] = MysqlValueFormatter.FormatDouble(rewardCacheData.MaxBet.Value);
        }

        if (rewardCacheData.TotalGameCount > 0 && rewardCacheData.MinBet.HasValue)
        {
            data["MinBet"] = MysqlValueFormatter.FormatDouble(rewardCacheData.MinBet.Value);
        }

        if (!string.IsNullOrWhiteSpace(rewardCacheData.ExtraInfo))
        {
            data["ExtraInfo"] = rewardCacheData.ExtraInfo;
        }

        AddRewardEndStatusWhenRequired(userUid, rewardCacheData, record, data);

        Dictionary<string, string> where = BuildActiveRecordWhere(userUid, rewardCacheData.RewardRecordId);
        MysqlWriteResult? writeResult = TryWrite(
            "UpdateSession",
            userUid,
            rewardCacheData.RewardRecordId,
            record.RecordStatus,
            record.RewardEndStatus,
            MysqlWriteOperation.Update,
            data,
            where);
        if (writeResult?.AffectedRows == 1)
        {
            return Succeeded("Session 統計已成功更新。");
        }

        const string failedMessage = "更新 Session 的影響筆數不是一。";
        LogError("UpdateSession", userUid, rewardCacheData.RewardRecordId, record.RecordStatus, record.RewardEndStatus, failedMessage);
        return Failed(failedMessage);
    }

    /// <summary>
    /// 取得外部目標餘額產生器的有效結果。
    /// </summary>
    /// <param name="userUid">用於錯誤紀錄的玩家 UID。</param>
    /// <param name="balanceAfterKeyIn">本次建立 Session 的開分交易套用後玩家餘額，會原樣傳入外部產生器。</param>
    /// <param name="keyInAmount">本次建立 Session 的開分值，會原樣傳入外部產生器。</param>
    /// <param name="rewardWebSetting">宿主提供的目標餘額規則設定字串，會原樣傳入外部產生器。</param>
    /// <returns>有限且可寫入 MySQL 的目標餘額。</returns>
    /// <exception cref="InvalidOperationException">當外部產生器擲回例外，或回傳非有限數值時擲回。</exception>
    private (int rewardEndStatus, double targetBalance) GenerateTargetBalance(int userUid, double balanceAfterKeyIn, double keyInAmount, string rewardWebSetting)
    {
        try
        {
            (int rewardEndStatus, double targetBalance) = _targetBalanceGenerator.Generate(balanceAfterKeyIn, keyInAmount, rewardWebSetting);
            if (!double.IsFinite(targetBalance))
            {
                const string message = "外部目標餘額產生器回傳非有限數值，拒絕新增 Session。";
                LogError("CreateSession", userUid, null, null, null, message);
                throw new InvalidOperationException(message);
            }

            return (rewardEndStatus, targetBalance);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception exception)
        {
            const string message = "外部目標餘額產生器執行失敗，拒絕新增 Session。";
            LogError("CreateSession", userUid, null, null, null, $"{message} {exception.Message}");
            throw new InvalidOperationException(message, exception);
        }
    }

    /// <summary>
    /// 依功能終止狀態與資料庫現況，決定是否首次寫入功能關閉狀態。
    /// </summary>
    /// <param name="userUid">用於錯誤或警告紀錄的玩家 UID。</param>
    /// <param name="rewardCacheData">外部宿主提供的最新快取快照。</param>
    /// <param name="record">目前讀取到的資料庫紀錄。</param>
    /// <param name="data">即將傳入 <c>WriteMysql</c> 的更新欄位字典。</param>
    private void AddRewardEndStatusWhenRequired(
        int userUid,
        RewardCacheDataModel rewardCacheData,
        UserSessionRewardRecord record,
        Dictionary<string, string> data)
    {
        if (rewardCacheData.RewardEndStatus is RewardEndStatus.ServerNaturalClose or RewardEndStatus.WebForceClose)
        {
            if (record.RewardEndStatus == RewardEndStatus.InProgress)
            {
                data["RewardEndStatus"] = MysqlValueFormatter.FormatRewardEndStatus(rewardCacheData.RewardEndStatus);
            }
            else if (record.RewardEndStatus != rewardCacheData.RewardEndStatus)
            {
                const string message = "快取與資料庫的功能終止狀態不一致，保留資料庫首次寫入值。";
                LogWarning("UpdateSession", userUid, rewardCacheData.RewardRecordId, record.RecordStatus, record.RewardEndStatus, message);
            }
        }
    }

    /// <summary>
    /// 執行 MySQL 寫入，並在寫入層擲回例外時保留結構化錯誤紀錄。
    /// </summary>
    /// <param name="operationName">呼叫寫入的商業操作名稱。</param>
    /// <param name="userUid">相關玩家 UID。</param>
    /// <param name="rewardRecordId">相關紀錄流水號；新增前可為 <see langword="null"/>。</param>
    /// <param name="recordStatus">寫入前已讀取到的 Session 狀態。</param>
    /// <param name="rewardEndStatus">寫入前已讀取到的功能終止狀態。</param>
    /// <param name="operation">新增或更新操作種類。</param>
    /// <param name="data">僅含本次更新欄位的字典。</param>
    /// <param name="where">更新時的受信任相等條件字典。</param>
    /// <returns>寫入層正常回傳的結果；發生例外時為 <see langword="null"/>。</returns>
    private MysqlWriteResult? TryWrite(
        string operationName,
        int userUid,
        long? rewardRecordId,
        SessionRecordStatus? recordStatus,
        RewardEndStatus? rewardEndStatus,
        MysqlWriteOperation operation,
        Dictionary<string, string> data,
        Dictionary<string, string>? where)
    {
        try
        {
            return _recordWriter.WriteMysql(UserSessionRewardRecordTableName, operation, data, where);
        }
        catch (Exception exception)
        {
            LogError(operationName, userUid, rewardRecordId, recordStatus, rewardEndStatus, $"MySQL 寫入層執行失敗：{exception.Message}");
            return null;
        }
    }

    /// <summary>
    /// 建立只允許更新指定進行中 Session 的相等條件字典。
    /// </summary>
    /// <param name="userUid">受白名單欄位 <c>UserUID</c> 使用的玩家 UID。</param>
    /// <param name="rewardRecordId">受白名單欄位 <c>RewardRecordId</c> 使用的流水號。</param>
    /// <returns>只由程式內部數值與白名單欄位組成的相等條件字典。</returns>
    /// <remarks>回傳值會傳入 MySQL 寫入層，由宿主以參數化查詢處理，不得拼接未驗證玩家、Web 或其他外部字串。</remarks>
    private static Dictionary<string, string> BuildActiveRecordWhere(int userUid, long rewardRecordId)
    {
        return new Dictionary<string, string>
        {
            ["UserUID"] = MysqlValueFormatter.FormatInt32(userUid),
            ["RewardRecordId"] = MysqlValueFormatter.FormatInt64(rewardRecordId),
            ["RecordStatus"] = MysqlValueFormatter.FormatSessionRecordStatus(SessionRecordStatus.InProgress)
        };
    }

    /// <summary>
    /// 將新值追加到既有逗號分隔字串，或在尚無既有值時直接回傳新值。
    /// </summary>
    /// <param name="existingValue">既有值；可為 <see langword="null"/> 或空字串。</param>
    /// <param name="newValue">已完成驗證與格式化的新值。</param>
    /// <returns>可直接寫入資料庫的單一值或逗號分隔字串。</returns>
    private static string AppendCommaSeparatedValue(string? existingValue, string newValue)
    {
        return string.IsNullOrEmpty(existingValue) ? newValue : $"{existingValue},{newValue}";
    }

    /// <summary>
    /// 檢查讀取紀錄是否確實屬於輸入玩家。
    /// </summary>
    /// <param name="record">欲驗證的讀取紀錄。</param>
    /// <param name="userUid">預期的玩家 UID。</param>
    /// <returns>紀錄玩家 UID 相符時為 <see langword="true"/>。</returns>
    private static bool IsRecordOwnedByUser(UserSessionRewardRecord record, int userUid)
    {
        return record.UserUid == userUid;
    }

    /// <summary>
    /// 取得伺服器本機現在時間。
    /// </summary>
    /// <returns>可安全寫入 MySQL 本機時間欄位的時間。</returns>
    private DateTime GetNow()
    {
        return _clock.Now;
    }

    /// <summary>
    /// 建立成功寫入一筆 Session 的結果。
    /// </summary>
    /// <param name="message">提供外部追蹤的結果說明。</param>
    /// <returns>標示成功的操作結果。</returns>
    private static SessionOperationResult Succeeded(string message)
    {
        return new SessionOperationResult { IsSuccessful = true, IsNoOperation = false, Message = message };
    }

    /// <summary>
    /// 建立未寫入資料庫的冪等結果。
    /// </summary>
    /// <param name="message">提供外部追蹤的結果說明。</param>
    /// <returns>標示無動作的操作結果。</returns>
    private static SessionOperationResult NoOperation(string message)
    {
        return new SessionOperationResult { IsSuccessful = false, IsNoOperation = true, Message = message };
    }

    /// <summary>
    /// 建立因資料錯誤或寫入失敗而拒絕的結果。
    /// </summary>
    /// <param name="message">提供外部追蹤的錯誤說明。</param>
    /// <returns>標示失敗的操作結果。</returns>
    private static SessionOperationResult Failed(string message)
    {
        return new SessionOperationResult { IsSuccessful = false, IsNoOperation = false, Message = message };
    }

    /// <summary>
    /// 記錄流程錯誤與完整狀態脈絡。
    /// </summary>
    /// <param name="operationName">發生錯誤的操作名稱。</param>
    /// <param name="userUid">相關玩家 UID。</param>
    /// <param name="rewardRecordId">相關紀錄流水號。</param>
    /// <param name="recordStatus">已讀取到的 Session 狀態。</param>
    /// <param name="rewardEndStatus">已讀取到的功能終止狀態。</param>
    /// <param name="message">錯誤原因。</param>
    private void LogError(
        string operationName,
        int userUid,
        long? rewardRecordId,
        SessionRecordStatus? recordStatus,
        RewardEndStatus? rewardEndStatus,
        string message)
    {
        _logger.LogError(CreateLogEntry(operationName, userUid, rewardRecordId, recordStatus, rewardEndStatus, message));
    }

    /// <summary>
    /// 記錄流程警告與完整狀態脈絡。
    /// </summary>
    /// <param name="operationName">發生警告的操作名稱。</param>
    /// <param name="userUid">相關玩家 UID。</param>
    /// <param name="rewardRecordId">相關紀錄流水號。</param>
    /// <param name="recordStatus">已讀取到的 Session 狀態。</param>
    /// <param name="rewardEndStatus">已讀取到的功能終止狀態。</param>
    /// <param name="message">警告原因。</param>
    private void LogWarning(
        string operationName,
        int userUid,
        long? rewardRecordId,
        SessionRecordStatus? recordStatus,
        RewardEndStatus? rewardEndStatus,
        string message)
    {
        _logger.LogWarning(CreateLogEntry(operationName, userUid, rewardRecordId, recordStatus, rewardEndStatus, message));
    }

    /// <summary>
    /// 建立符合錯誤紀錄欄位要求的結構化紀錄。
    /// </summary>
    /// <param name="operationName">發生問題的操作名稱。</param>
    /// <param name="userUid">相關玩家 UID。</param>
    /// <param name="rewardRecordId">相關紀錄流水號。</param>
    /// <param name="recordStatus">已讀取到的 Session 狀態。</param>
    /// <param name="rewardEndStatus">已讀取到的功能終止狀態。</param>
    /// <param name="message">錯誤或警告原因。</param>
    /// <returns>包含本機發生時間的結構化紀錄。</returns>
    private SessionRewardLogEntry CreateLogEntry(
        string operationName,
        int userUid,
        long? rewardRecordId,
        SessionRecordStatus? recordStatus,
        RewardEndStatus? rewardEndStatus,
        string message)
    {
        return new SessionRewardLogEntry
        {
            OperationName = operationName,
            UserUid = userUid,
            RewardRecordId = rewardRecordId,
            RecordStatus = recordStatus,
            RewardEndStatus = rewardEndStatus,
            Message = message,
            OccurredAt = GetNow()
        };
    }
}
