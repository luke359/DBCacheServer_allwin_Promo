using UserSessionReward.Core.Abstractions;
using UserSessionReward.Core.Enums;
using UserSessionReward.Core.Models;

namespace UserSessionReward.Tests.Fakes;

/// <summary>
/// 提供可控制回傳值並記錄呼叫次數的目標餘額產生器測試替身。
/// </summary>
internal sealed class FakeTargetBalanceGenerator : ITargetBalanceGenerator
{
    /// <summary>
    /// 保存產生器應回傳的目標餘額。
    /// </summary>
    private readonly double _targetBalance;

    /// <summary>
    /// 保存產生器應回傳的功能終止狀態。
    /// </summary>
    private readonly int _rewardEndStatus;

    /// <summary>
    /// 初始化可控制回傳值的產生器。
    /// </summary>
    /// <param name="targetBalance">呼叫 <see cref="Generate"/> 時應回傳的目標餘額。</param>
    /// <param name="rewardEndStatus">呼叫 <see cref="Generate"/> 時應回傳的功能終止狀態。</param>
    internal FakeTargetBalanceGenerator(double targetBalance, int rewardEndStatus = 0)
    {
        _targetBalance = targetBalance;
        _rewardEndStatus = rewardEndStatus;
    }

    /// <summary>
    /// 取得產生器被呼叫的次數。
    /// </summary>
    internal int GenerateCallCount { get; private set; }

    /// <summary>
    /// 取得最近一次傳入產生器的開分後餘額。
    /// </summary>
    internal double? LastBalanceAfterKeyIn { get; private set; }

    /// <summary>
    /// 取得最近一次傳入產生器的開分值。
    /// </summary>
    internal double? LastKeyInAmount { get; private set; }

    /// <summary>
    /// 取得最近一次原樣轉入產生器的設定字串。
    /// </summary>
    internal string? LastRewardWebSetting { get; private set; }

    /// <summary>
    /// 記錄開分後餘額、開分值與設定字串後回傳預先設定的功能終止狀態與目標餘額。
    /// </summary>
    /// <param name="balanceAfterKeyIn">服務傳入的開分交易套用後玩家餘額。</param>
    /// <param name="keyInAmount">服務傳入的本次開分值。</param>
    /// <param name="rewardWebSetting">服務原樣轉入的宿主設定字串。</param>
    /// <returns>建構時設定的功能終止狀態與目標餘額。</returns>
    public (int rewardEndStatus, double targetBalance) Generate(
        double balanceAfterKeyIn,
        double keyInAmount,
        string rewardWebSetting)
    {
        GenerateCallCount++;
        LastBalanceAfterKeyIn = balanceAfterKeyIn;
        LastKeyInAmount = keyInAmount;
        LastRewardWebSetting = rewardWebSetting;
        return (_rewardEndStatus, _targetBalance);
    }
}

/// <summary>
/// 提供固定本機時間的測試時鐘。
/// </summary>
internal sealed class FixedClock : IClock
{
    /// <summary>
    /// 初始化固定本機時間來源。
    /// </summary>
    /// <param name="now">測試期間固定回傳的本機時間。</param>
    internal FixedClock(DateTime now)
    {
        Now = now;
    }

    /// <summary>
    /// 取得固定的本機現在時間。
    /// </summary>
    internal DateTime Now { get; }

    /// <summary>
    /// 取得固定的本機現在時間。
    /// </summary>
    /// <value>測試建立時提供的本機時間。</value>
    /// <inheritdoc />
    DateTime IClock.Now => Now;
}

/// <summary>
/// 提供可配置 Session 讀取結果的測試資料讀取器。
/// </summary>
internal sealed class FakeRecordReader : IUserSessionRewardRecordReader
{
    /// <summary>
    /// 初始化沒有預設讀取結果的測試資料讀取器。
    /// </summary>
    internal FakeRecordReader()
    {
    }

    /// <summary>
    /// 保存指定玩家查詢進行中 Session 時應回傳的紀錄。
    /// </summary>
    internal List<UserSessionRewardRecord> InProgressRecords { get; } = new();

    /// <summary>
    /// 取得或設定以玩家與流水號讀取單筆紀錄時的回傳值。
    /// </summary>
    internal UserSessionRewardRecord? RecordById { get; set; }

    /// <summary>
    /// 回傳預先設定的進行中 Session 清單。
    /// </summary>
    /// <param name="userUid">欲查詢的玩家 UID。</param>
    /// <returns>測試設定的進行中 Session 清單副本。</returns>
    public IReadOnlyList<UserSessionRewardRecord> GetInProgressByUserUid(int userUid)
    {
        return InProgressRecords.ToArray();
    }

    /// <summary>
    /// 依玩家與流水號回傳預先設定的單筆紀錄。
    /// </summary>
    /// <param name="userUid">預期的玩家 UID。</param>
    /// <param name="rewardRecordId">預期的流水號。</param>
    /// <returns>兩個識別值均符合時的測試紀錄，否則為 <see langword="null"/>。</returns>
    public UserSessionRewardRecord? GetByUserUidAndRewardRecordId(int userUid, long rewardRecordId)
    {
        return RecordById is { } record && record.UserUid == userUid && record.RewardRecordId == rewardRecordId
            ? record
            : null;
    }
}

/// <summary>
/// 記錄 MySQL 寫入引數並依序回傳可控制結果的測試寫入器。
/// </summary>
internal sealed class FakeRecordWriter : IUserSessionRewardRecordWriter
{
    /// <summary>
    /// 初始化沒有預設寫入結果的測試寫入器。
    /// </summary>
    internal FakeRecordWriter()
    {
    }

    /// <summary>
    /// 保護可變測試集合的同步鎖定物件。
    /// </summary>
    private readonly object _syncRoot = new();

    /// <summary>
    /// 保存各次寫入應回傳的結果佇列。
    /// </summary>
    private readonly Queue<MysqlWriteResult> _results = new();

    /// <summary>
    /// 取得依呼叫順序記錄的 MySQL 寫入內容。
    /// </summary>
    internal List<MysqlWriteInvocation> Invocations { get; } = new();

    /// <summary>
    /// 將下一次寫入的預期結果加入佇列。
    /// </summary>
    /// <param name="result">下一次 <see cref="WriteMysql"/> 應回傳的結果。</param>
    internal void EnqueueResult(MysqlWriteResult result)
    {
        lock (_syncRoot)
        {
            _results.Enqueue(result);
        }
    }

    /// <summary>
    /// 記錄寫入引數並回傳預先安排的結果。
    /// </summary>
    /// <param name="sqlTableName">呼叫端要求寫入的資料表名稱。</param>
    /// <param name="operation">呼叫端要求的操作種類。</param>
    /// <param name="data">呼叫端傳入的欄位資料。</param>
    /// <param name="where">呼叫端傳入的更新條件。</param>
    /// <returns>佇列中的下一個結果。</returns>
    /// <exception cref="InvalidOperationException">當測試未設定足夠的寫入結果時擲回。</exception>
    public MysqlWriteResult WriteMysql(
        string sqlTableName,
        MysqlWriteOperation operation,
        Dictionary<string, string> data,
        Dictionary<string, string>? where = null)
    {
        lock (_syncRoot)
        {
            Invocations.Add(new MysqlWriteInvocation
            {
                SqlTableName = sqlTableName,
                Operation = operation,
                Data = new Dictionary<string, string>(data),
                Where = where is null ? null : new Dictionary<string, string>(where)
            });

            if (_results.Count == 0)
            {
                throw new InvalidOperationException("測試尚未安排 MySQL 寫入結果。");
            }

            return _results.Dequeue();
        }
    }
}

/// <summary>
/// 表示測試所記錄的一次 MySQL 寫入呼叫。
/// </summary>
internal sealed class MysqlWriteInvocation
{
    /// <summary>
    /// 初始化空白的 MySQL 寫入呼叫紀錄。
    /// </summary>
    internal MysqlWriteInvocation()
    {
    }

    /// <summary>
    /// 取得或設定資料表名稱。
    /// </summary>
    internal string SqlTableName { get; init; } = string.Empty;

    /// <summary>
    /// 取得或設定寫入操作種類。
    /// </summary>
    internal MysqlWriteOperation Operation { get; init; }

    /// <summary>
    /// 取得或設定寫入欄位資料副本。
    /// </summary>
    internal Dictionary<string, string> Data { get; init; } = new();

    /// <summary>
    /// 取得或設定更新相等條件字典副本。
    /// </summary>
    internal Dictionary<string, string>? Where { get; init; }
}

/// <summary>
/// 收集服務錯誤與警告紀錄的測試記錄器。
/// </summary>
internal sealed class FakeLogger : IUserSessionRewardLogger
{
    /// <summary>
    /// 初始化空白的測試記錄器。
    /// </summary>
    internal FakeLogger()
    {
    }

    /// <summary>
    /// 取得服務寫入的錯誤紀錄。
    /// </summary>
    internal List<SessionRewardLogEntry> Errors { get; } = new();

    /// <summary>
    /// 取得服務寫入的警告紀錄。
    /// </summary>
    internal List<SessionRewardLogEntry> Warnings { get; } = new();

    /// <summary>
    /// 保存一筆錯誤紀錄。
    /// </summary>
    /// <param name="entry">服務建立的結構化錯誤紀錄。</param>
    public void LogError(SessionRewardLogEntry entry)
    {
        Errors.Add(entry);
    }

    /// <summary>
    /// 保存一筆警告紀錄。
    /// </summary>
    /// <param name="entry">服務建立的結構化警告紀錄。</param>
    public void LogWarning(SessionRewardLogEntry entry)
    {
        Warnings.Add(entry);
    }
}
