using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserSessionReward.Core.Enums;
using UserSessionReward.Core.Models;
using UserSessionReward.Core.Services;
using UserSessionReward.Infrastructure.MySql;
using UserSessionReward.Tests.Fakes;

namespace UserSessionReward.Tests.Services;

/// <summary>
/// 驗證玩家上升額度 Session 商業流程與寫入資料契約。
/// </summary>
[TestClass]
public sealed class UserSessionRewardServiceTests
{
    /// <summary>
    /// 初始化玩家上升額度服務測試類別。
    /// </summary>
    public UserSessionRewardServiceTests()
    {
    }

    /// <summary>
    /// 固定測試使用的本機時間。
    /// </summary>
    private static readonly DateTime FixedNow = new DateTime(2026, 8, 21, 9, 10, 11, 123, DateTimeKind.Local).AddTicks(4560);

    /// <summary>
    /// 測試用的宿主設定字串；Core 應原樣轉入產生器。
    /// </summary>
    private const string SampleRewardWebSetting = "{\"sample\":\"web-setting\"}";

    /// <summary>
    /// 驗證第一次開分建立正確的 Session 與寫入欄位。
    /// </summary>
    [TestMethod]
    public void CreateSession_第一次開分_建立初始快取並使用本機時間與不變文化格式()
    {
        FakeTargetBalanceGenerator generator = new(900.5, (int)RewardEndStatus.WebForceClose);
        FakeRecordReader reader = new();
        FakeRecordWriter writer = new();
        FakeLogger logger = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1, LastInsertedId = 88 });
        UserSessionRewardService service = CreateService(generator, reader, writer, logger);
        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");

        try
        {
            RewardCacheDataModel result = service.CreateSession(42, 12.5, 100.25, SampleRewardWebSetting);

            Assert.AreEqual(88L, result.RewardRecordId);
            Assert.AreEqual(RewardEndStatus.InProgress, result.RewardEndStatus);
            Assert.AreEqual(900.5, result.TargetBalance);
            Assert.AreEqual(0, result.TotalGameCount);
            Assert.AreEqual(0d, result.TotalBet);
            Assert.IsNull(result.MaxBet);
            Assert.IsNull(result.MinBet);
            Assert.AreEqual(112.75, result.MaxBalance);
            Assert.AreEqual(112.75, result.MinBalance);
            Assert.AreEqual(1, generator.GenerateCallCount);
            Assert.AreEqual(112.75, generator.LastBalanceAfterKeyIn);
            Assert.AreEqual(12.5, generator.LastKeyInAmount);
            Assert.AreEqual(SampleRewardWebSetting, generator.LastRewardWebSetting);
            Assert.AreEqual(1, writer.Invocations.Count);

            MysqlWriteInvocation write = writer.Invocations[0];
            Assert.AreEqual(MysqlWriteOperation.Insert, write.Operation);
            Assert.IsNull(write.Where);
            Assert.AreEqual("12.5", write.Data["KeyInAmount"]);
            Assert.AreEqual("112.75", write.Data["BalanceAfterKeyIn"]);
            Assert.AreEqual("2026-08-21 09:10:11.123456", write.Data["StartTime"]);
            Assert.IsFalse(write.Data.ContainsKey("EndBalance"));
            Assert.IsFalse(write.Data.ContainsKey("KeyOutAmount"));
            Assert.IsFalse(write.Data.ContainsKey("BalanceWhenKeyOut"));
            Assert.IsFalse(write.Data.ContainsKey("ExtraInfo"));
            Assert.IsFalse(write.Data.ContainsKey("MaxBet"));
            Assert.IsFalse(write.Data.ContainsKey("MinBet"));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    /// <summary>
    /// 驗證再次開分先結束舊 Session，才新增新 Session。
    /// </summary>
    [TestMethod]
    public void CreateSession_有進行中舊紀錄_先以再開分結束再新增()
    {
        const int userUid = 42;
        FakeRecordReader reader = new();
        UserSessionRewardRecord oldRecord = CreateRecord(
            userUid,
            11,
            SessionRecordStatus.InProgress,
            RewardEndStatus.InProgress,
            "25",
            "300");
        reader.InProgressRecords.Add(oldRecord);
        reader.RecordById = oldRecord;
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1, LastInsertedId = 12 });
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(1000), reader, writer, new FakeLogger());

        RewardCacheDataModel result = service.CreateSession(userUid, 50, 300, SampleRewardWebSetting);

        Assert.AreEqual(12L, result.RewardRecordId);
        Assert.AreEqual(2, writer.Invocations.Count);
        Assert.AreEqual(MysqlWriteOperation.Update, writer.Invocations[0].Operation);
        Assert.AreEqual("1", writer.Invocations[0].Data["RecordStatus"]);
        Assert.AreEqual("300", writer.Invocations[0].Data["EndBalance"]);
        Assert.AreEqual("2", writer.Invocations[0].Data["RewardEndStatus"]);
        Assert.IsFalse(writer.Invocations[0].Data.ContainsKey("KeyInAmount"));
        Assert.IsFalse(writer.Invocations[0].Data.ContainsKey("KeyOutAmount"));
        Assert.IsFalse(writer.Invocations[0].Data.ContainsKey("BalanceWhenKeyOut"));
        AssertActiveRecordWhere(writer.Invocations[0].Where, 42, 11);
        Assert.AreEqual(MysqlWriteOperation.Insert, writer.Invocations[1].Operation);
    }

    /// <summary>
    /// 驗證第一次洗分追加成對欄位但不結束 Session。
    /// </summary>
    [TestMethod]
    public void RecordKeyOut_第一次洗分_追加成對欄位且不修改狀態或統計()
    {
        const int userUid = 42;
        FakeRecordReader reader = new() { RecordById = CreateRecord(userUid, 15, SessionRecordStatus.InProgress, RewardEndStatus.InProgress) };
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, new FakeLogger());

        SessionOperationResult result = service.RecordKeyOut(userUid, 15, 12.34567, 555.56789);

        Assert.IsTrue(result.IsSuccessful);
        Assert.AreEqual(1, writer.Invocations.Count);
        MysqlWriteInvocation write = writer.Invocations[0];
        Assert.AreEqual("12.3456", write.Data["KeyOutAmount"]);
        Assert.AreEqual("555.5678", write.Data["BalanceWhenKeyOut"]);
        Assert.AreEqual(2, write.Data.Count);
        AssertActiveRecordWhere(write.Where, userUid, 15);
    }

    /// <summary>
    /// 驗證再次洗分會在既有成對欄位後追加新值。
    /// </summary>
    [TestMethod]
    public void RecordKeyOut_已有洗分資料_以逗號追加並移除多餘尾端零()
    {
        const int userUid = 42;
        FakeRecordReader reader = new()
        {
            RecordById = CreateRecord(
                userUid,
                16,
                SessionRecordStatus.InProgress,
                RewardEndStatus.ServerNaturalClose,
                "12.3456",
                "555.5678")
        };
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, new FakeLogger());

        SessionOperationResult result = service.RecordKeyOut(userUid, 16, 10.50009, 700);

        Assert.IsTrue(result.IsSuccessful);
        Assert.AreEqual("12.3456,10.5", writer.Invocations[0].Data["KeyOutAmount"]);
        Assert.AreEqual("555.5678,700", writer.Invocations[0].Data["BalanceWhenKeyOut"]);
        Assert.AreEqual(2, writer.Invocations[0].Data.Count);
    }

    /// <summary>
    /// 驗證洗分值與洗分當下餘額的無效數值都會被拒絕。
    /// </summary>
    /// <param name="keyOutAmount">測試使用的洗分值。</param>
    /// <param name="balanceWhenKeyOut">測試使用的洗分當下餘額。</param>
    [DataTestMethod]
    [DataRow(0d, 100d)]
    [DataRow(0.00009d, 100d)]
    [DataRow(-1d, 100d)]
    [DataRow(double.NaN, 100d)]
    [DataRow(1d, -1d)]
    [DataRow(1d, double.PositiveInfinity)]
    public void RecordKeyOut_數值無效_回傳失敗且不寫入(double keyOutAmount, double balanceWhenKeyOut)
    {
        FakeRecordReader reader = new() { RecordById = CreateRecord(42, 17, SessionRecordStatus.InProgress, RewardEndStatus.InProgress) };
        FakeRecordWriter writer = new();
        FakeLogger logger = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, logger);

        SessionOperationResult result = service.RecordKeyOut(42, 17, keyOutAmount, balanceWhenKeyOut);

        Assert.IsFalse(result.IsSuccessful);
        Assert.IsFalse(result.IsNoOperation);
        Assert.AreEqual(0, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Errors.Count);
    }

    /// <summary>
    /// 驗證找不到進行中紀錄或使用已結束紀錄時都回傳失敗。
    /// </summary>
    [TestMethod]
    public void RecordKeyOut_紀錄不存在或已結束_回傳失敗而非無動作()
    {
        FakeRecordReader reader = new();
        FakeRecordWriter writer = new();
        FakeLogger logger = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, logger);

        SessionOperationResult missingResult = service.RecordKeyOut(42, 18, 1, 100);
        reader.RecordById = CreateRecord(42, 18, SessionRecordStatus.EndedByReopen, RewardEndStatus.PlayerEarlyClose);
        SessionOperationResult endedResult = service.RecordKeyOut(42, 18, 1, 100);

        Assert.IsFalse(missingResult.IsSuccessful);
        Assert.IsFalse(missingResult.IsNoOperation);
        Assert.IsFalse(endedResult.IsSuccessful);
        Assert.IsFalse(endedResult.IsNoOperation);
        Assert.AreEqual(0, writer.Invocations.Count);
        Assert.AreEqual(2, logger.Errors.Count);
    }

    /// <summary>
    /// 驗證既有洗分欄位不成對時拒絕追加，避免產生錯位歷程。
    /// </summary>
    [TestMethod]
    public void RecordKeyOut_既有洗分欄位不成對_回傳失敗()
    {
        FakeRecordReader reader = new()
        {
            RecordById = CreateRecord(42, 19, SessionRecordStatus.InProgress, RewardEndStatus.InProgress, "1", null)
        };
        FakeRecordWriter writer = new();
        FakeLogger logger = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, logger);

        SessionOperationResult result = service.RecordKeyOut(42, 19, 2, 100);

        Assert.IsFalse(result.IsSuccessful);
        Assert.AreEqual(0, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Errors.Count);
    }

    /// <summary>
    /// 驗證洗分更新影響筆數不是一時回傳失敗。
    /// </summary>
    [TestMethod]
    public void RecordKeyOut_影響筆數不是一_回傳失敗()
    {
        FakeRecordReader reader = new() { RecordById = CreateRecord(42, 19, SessionRecordStatus.InProgress, RewardEndStatus.InProgress) };
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 0 });
        FakeLogger logger = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, logger);

        SessionOperationResult result = service.RecordKeyOut(42, 19, 2, 100);

        Assert.IsFalse(result.IsSuccessful);
        Assert.IsFalse(result.IsNoOperation);
        Assert.AreEqual(1, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Errors.Count);
    }

    /// <summary>
    /// 驗證洗分成功後同一筆進行中 Session 仍可執行一般統計更新。
    /// </summary>
    [TestMethod]
    public void RecordKeyOut_成功後_同一Session仍可更新統計與ExtraInfo()
    {
        FakeRecordReader reader = new() { RecordById = CreateRecord(42, 20, SessionRecordStatus.InProgress, RewardEndStatus.InProgress) };
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, new FakeLogger());

        SessionOperationResult keyOutResult = service.RecordKeyOut(42, 20, 5, 100);
        RewardCacheDataModel cache = CreateCache(20, RewardEndStatus.InProgress);
        cache.ExtraInfo = "after-key-out";
        SessionOperationResult updateResult = service.UpdateSession(42, cache, 0);

        Assert.IsTrue(keyOutResult.IsSuccessful);
        Assert.IsTrue(updateResult.IsSuccessful);
        Assert.AreEqual(2, writer.Invocations.Count);
        Assert.AreEqual("5", writer.Invocations[0].Data["KeyOutAmount"]);
        Assert.AreEqual("after-key-out", writer.Invocations[1].Data["ExtraInfo"]);
        Assert.IsFalse(writer.Invocations[1].Data.ContainsKey("KeyOutAmount"));
    }

    /// <summary>
    /// 驗證自然關閉只首次寫入功能關閉狀態，且後續仍可更新統計。
    /// </summary>
    [TestMethod]
    public void UpdateSession_功能自然關閉後_保留首次狀態並持續更新統計()
    {
        const int userUid = 42;
        FakeRecordReader reader = new() { RecordById = CreateRecord(userUid, 20, SessionRecordStatus.InProgress, RewardEndStatus.InProgress) };
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, new FakeLogger());
        RewardCacheDataModel cache = CreateCache(20, RewardEndStatus.ServerNaturalClose);

        SessionOperationResult result = service.UpdateSession(userUid, cache, 800);

        Assert.IsTrue(result.IsSuccessful);
        MysqlWriteInvocation firstWrite = writer.Invocations.Single();
        Assert.AreEqual("1", firstWrite.Data["RewardEndStatus"]);
        Assert.IsFalse(firstWrite.Data.ContainsKey("EndBalance"));
        Assert.IsFalse(firstWrite.Data.ContainsKey("EndTime"));

        reader.RecordById = CreateRecord(userUid, 20, SessionRecordStatus.InProgress, RewardEndStatus.ServerNaturalClose);
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        cache.TotalGameCount = 2;
        cache.TotalBet = 30;
        SessionOperationResult secondResult = service.UpdateSession(userUid, cache, 999);

        Assert.IsTrue(secondResult.IsSuccessful);
        MysqlWriteInvocation secondWrite = writer.Invocations[1];
        Assert.IsFalse(secondWrite.Data.ContainsKey("RewardEndStatus"));
        Assert.AreEqual("2", secondWrite.Data["TotalGameCount"]);
        Assert.AreEqual("30", secondWrite.Data["TotalBet"]);
    }

    /// <summary>
    /// 驗證 Web 強制關閉只寫入狀態 3。
    /// </summary>
    [TestMethod]
    public void UpdateSession_Web強制關閉_只寫入狀態3()
    {
        FakeRecordReader reader = new() { RecordById = CreateRecord(42, 21, SessionRecordStatus.InProgress, RewardEndStatus.InProgress) };
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, new FakeLogger());

        SessionOperationResult result = service.UpdateSession(42, CreateCache(21, RewardEndStatus.WebForceClose), 678);

        Assert.IsTrue(result.IsSuccessful);
        Assert.AreEqual("3", writer.Invocations[0].Data["RewardEndStatus"]);
        Assert.IsFalse(writer.Invocations[0].Data.ContainsKey("EndBalance"));
    }

    /// <summary>
    /// 驗證無效或不相符的流水號會被拒絕並記錄錯誤。
    /// </summary>
    [TestMethod]
    public void UpdateSession_找不到紀錄_拒絕寫入並記錄錯誤()
    {
        FakeLogger logger = new();
        FakeRecordWriter writer = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), new FakeRecordReader(), writer, logger);

        SessionOperationResult result = service.UpdateSession(42, CreateCache(99, RewardEndStatus.InProgress), 1);

        Assert.IsFalse(result.IsSuccessful);
        Assert.IsFalse(result.IsNoOperation);
        Assert.AreEqual(0, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Errors.Count);
        Assert.AreEqual("UpdateSession", logger.Errors[0].OperationName);
        Assert.AreEqual(42, logger.Errors[0].UserUid);
        Assert.AreEqual(99L, logger.Errors[0].RewardRecordId);
        Assert.AreEqual(FixedNow, logger.Errors[0].OccurredAt);
    }

    /// <summary>
    /// 驗證已結束 Session 不接受統計更新。
    /// </summary>
    [TestMethod]
    public void UpdateSession_已結束紀錄_不寫入資料庫()
    {
        FakeRecordReader reader = new() { RecordById = CreateRecord(42, 30, SessionRecordStatus.EndedByReopen, RewardEndStatus.PlayerEarlyClose) };
        FakeRecordWriter writer = new();
        FakeLogger logger = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, logger);

        SessionOperationResult result = service.UpdateSession(42, CreateCache(30, RewardEndStatus.InProgress), 10);

        Assert.IsTrue(result.IsNoOperation);
        Assert.AreEqual(0, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Warnings.Count);
    }

    /// <summary>
    /// 驗證重複結束終態 Session 不會覆寫第一次結束資料。
    /// </summary>
    [TestMethod]
    public void EndSession_已結束紀錄_回傳冪等無動作()
    {
        FakeRecordReader reader = new() { RecordById = CreateRecord(42, 35, SessionRecordStatus.EndedByReopen, RewardEndStatus.PlayerEarlyClose) };
        FakeRecordWriter writer = new();
        FakeLogger logger = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, logger);

        SessionOperationResult result = service.EndSession(42, 35, 999);

        Assert.IsTrue(result.IsNoOperation);
        Assert.AreEqual(0, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Warnings.Count);
    }

    /// <summary>
    /// 驗證一般統計更新不得自行產生玩家提前結束狀態。
    /// </summary>
    [TestMethod]
    public void UpdateSession_快取為玩家提前結束_拒絕寫入()
    {
        FakeLogger logger = new();
        FakeRecordWriter writer = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), new FakeRecordReader(), writer, logger);

        SessionOperationResult result = service.UpdateSession(42, CreateCache(31, RewardEndStatus.PlayerEarlyClose), 10);

        Assert.IsFalse(result.IsSuccessful);
        Assert.AreEqual(0, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Errors.Count);
    }

    /// <summary>
    /// 驗證更新尚無遊戲押分資料時不寫入 MaxBet 與 MinBet，以保留資料庫 NULL。
    /// </summary>
    [TestMethod]
    public void UpdateSession_尚無單局押分資料_省略可空押分欄位()
    {
        FakeRecordReader reader = new() { RecordById = CreateRecord(42, 32, SessionRecordStatus.InProgress, RewardEndStatus.InProgress) };
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, new FakeLogger());
        RewardCacheDataModel cache = CreateCache(32, RewardEndStatus.InProgress);
        cache.TotalGameCount = 0;
        cache.TotalBet = 0;
        cache.MaxBet = 99;
        cache.MinBet = 1;

        SessionOperationResult result = service.UpdateSession(42, cache, 100);

        Assert.IsTrue(result.IsSuccessful);
        Assert.IsFalse(writer.Invocations[0].Data.ContainsKey("MaxBet"));
        Assert.IsFalse(writer.Invocations[0].Data.ContainsKey("MinBet"));
        AssertActiveRecordWhere(writer.Invocations[0].Where, 42, 32);
    }

    /// <summary>
    /// 驗證非空白 ExtraInfo 會隨統計資料原樣覆寫。
    /// </summary>
    [TestMethod]
    public void UpdateSession_ExtraInfo有內容_隨統計資料原樣寫入()
    {
        FakeRecordReader reader = new() { RecordById = CreateRecord(42, 33, SessionRecordStatus.InProgress, RewardEndStatus.InProgress) };
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, new FakeLogger());
        RewardCacheDataModel cache = CreateCache(33, RewardEndStatus.InProgress);
        cache.ExtraInfo = "  {\"source\":\"host\"}  ";

        SessionOperationResult result = service.UpdateSession(42, cache, 999);

        Assert.IsTrue(result.IsSuccessful);
        Assert.AreEqual("  {\"source\":\"host\"}  ", writer.Invocations[0].Data["ExtraInfo"]);
    }

    /// <summary>
    /// 驗證 null、空字串及全空白 ExtraInfo 都不覆寫資料庫內容。
    /// </summary>
    /// <param name="extraInfo">測試使用的額外資訊。</param>
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void UpdateSession_ExtraInfo無效_省略欄位但仍更新統計(string? extraInfo)
    {
        FakeRecordReader reader = new() { RecordById = CreateRecord(42, 34, SessionRecordStatus.InProgress, RewardEndStatus.InProgress) };
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1 });
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(0), reader, writer, new FakeLogger());
        RewardCacheDataModel cache = CreateCache(34, RewardEndStatus.InProgress);
        cache.ExtraInfo = extraInfo;

        SessionOperationResult result = service.UpdateSession(42, cache, 12345);

        Assert.IsTrue(result.IsSuccessful);
        Assert.IsFalse(writer.Invocations[0].Data.ContainsKey("ExtraInfo"));
        Assert.AreEqual("1", writer.Invocations[0].Data["TotalGameCount"]);
    }

    /// <summary>
    /// 驗證同一玩家有多筆進行中 Session 時會停止新增且不嘗試修復。
    /// </summary>
    [TestMethod]
    public void CreateSession_多筆進行中紀錄_記錄錯誤並擲回例外()
    {
        FakeRecordReader reader = new();
        reader.InProgressRecords.Add(CreateRecord(42, 41, SessionRecordStatus.InProgress, RewardEndStatus.InProgress));
        reader.InProgressRecords.Add(CreateRecord(42, 42, SessionRecordStatus.InProgress, RewardEndStatus.InProgress));
        FakeRecordWriter writer = new();
        FakeLogger logger = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(100), reader, writer, logger);

        Assert.ThrowsException<InvalidOperationException>(() => service.CreateSession(42, 1, 1, SampleRewardWebSetting));
        Assert.AreEqual(0, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Errors.Count);
    }

    /// <summary>
    /// 驗證新增未取得有效流水號時不會回傳成功快取資料。
    /// </summary>
    [TestMethod]
    public void CreateSession_新增未回傳有效流水號_擲回例外()
    {
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1, LastInsertedId = 0 });
        FakeLogger logger = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(100), new FakeRecordReader(), writer, logger);

        Assert.ThrowsException<InvalidOperationException>(() => service.CreateSession(42, 1, 1, SampleRewardWebSetting));
        Assert.AreEqual(1, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Errors.Count);
    }

    /// <summary>
    /// 驗證外部產生器回傳非有限數值時不會建立 Session。
    /// </summary>
    [TestMethod]
    public void CreateSession_目標餘額無效_拒絕新增()
    {
        FakeRecordWriter writer = new();
        FakeLogger logger = new();
        UserSessionRewardService service = CreateService(new FakeTargetBalanceGenerator(double.NaN), new FakeRecordReader(), writer, logger);

        Assert.ThrowsException<InvalidOperationException>(() => service.CreateSession(42, 1, 1, SampleRewardWebSetting));
        Assert.AreEqual(0, writer.Invocations.Count);
        Assert.AreEqual(1, logger.Errors.Count);
    }

    /// <summary>
    /// 驗證空字串設定仍會原樣轉入產生器，Core 不拒絕也不改寫。
    /// </summary>
    [TestMethod]
    public void CreateSession_RewardWebSetting為空字串_原樣轉入產生器()
    {
        FakeTargetBalanceGenerator generator = new(1);
        FakeRecordWriter writer = new();
        writer.EnqueueResult(new MysqlWriteResult { AffectedRows = 1, LastInsertedId = 1 });
        UserSessionRewardService service = CreateService(generator, new FakeRecordReader(), writer, new FakeLogger());

        service.CreateSession(42, 1, 1, string.Empty);

        Assert.AreEqual(string.Empty, generator.LastRewardWebSetting);
        Assert.AreEqual(1, writer.Invocations.Count);
    }

    /// <summary>
    /// 驗證 Infrastructure 空寫入器拒絕沒有條件的更新操作。
    /// </summary>
    [TestMethod]
    public void MysqlWriter_Update沒有Where_拒絕執行()
    {
        UserSessionRewardMysqlWriter writer = new();
        Dictionary<string, string> data = new() { ["TotalBet"] = "1" };

        Assert.ThrowsException<ArgumentException>(() => writer.WriteMysql("UserSessionRewardRecord", MysqlWriteOperation.Update, data));
        Assert.ThrowsException<ArgumentException>(() => writer.WriteMysql("UserSessionRewardRecord", MysqlWriteOperation.Update, data, new Dictionary<string, string>()));
    }

    /// <summary>
    /// 驗證同一玩家的工作不會重疊，而不同玩家的工作可以平行。
    /// </summary>
    [TestMethod]
    public async Task InProcessUserOperationSerializer_同玩家依序不同玩家可平行()
    {
        InProcessUserOperationSerializer serializer = new();
        int sameUserCurrent = 0;
        int sameUserMaximum = 0;
        Task[] sameUserTasks = Enumerable.Range(0, 6).Select(_ => Task.Run(() =>
        {
            serializer.Execute(7, () =>
            {
                int current = Interlocked.Increment(ref sameUserCurrent);
                InterlockedExtensions.UpdateMaximum(ref sameUserMaximum, current);
                Thread.Sleep(20);
                Interlocked.Decrement(ref sameUserCurrent);
            });
        })).ToArray();

        await Task.WhenAll(sameUserTasks);

        Assert.AreEqual(1, sameUserMaximum);

        using ManualResetEventSlim bothStarted = new(false);
        int differentUserCurrent = 0;
        Task first = Task.Run(() => serializer.Execute(8, () => WaitForParallelWork(ref differentUserCurrent, bothStarted)));
        Task second = Task.Run(() => serializer.Execute(9, () => WaitForParallelWork(ref differentUserCurrent, bothStarted)));

        Assert.IsTrue(bothStarted.Wait(TimeSpan.FromSeconds(2)));
        await Task.WhenAll(first, second);
    }

    /// <summary>
    /// 建立包含固定相依服務的待測 Session 服務。
    /// </summary>
    /// <param name="generator">欲注入的目標餘額產生器。</param>
    /// <param name="reader">欲注入的資料讀取器。</param>
    /// <param name="writer">欲注入的資料寫入器。</param>
    /// <param name="logger">欲注入的紀錄器。</param>
    /// <returns>使用單一執行程序玩家序列化器與固定本機時鐘的服務。</returns>
    private static UserSessionRewardService CreateService(
        FakeTargetBalanceGenerator generator,
        FakeRecordReader reader,
        FakeRecordWriter writer,
        FakeLogger logger)
    {
        return new UserSessionRewardService(generator, reader, writer, new InProcessUserOperationSerializer(), new FixedClock(FixedNow), logger);
    }

    /// <summary>
    /// 建立最小完整的資料庫讀取紀錄。
    /// </summary>
    /// <param name="userUid">紀錄所屬玩家 UID。</param>
    /// <param name="rewardRecordId">紀錄流水號。</param>
    /// <param name="recordStatus">Session 生命週期狀態。</param>
    /// <param name="rewardEndStatus">功能終止狀態。</param>
    /// <param name="keyOutAmount">既有逗號分隔洗分值。</param>
    /// <param name="balanceWhenKeyOut">既有逗號分隔洗分當下餘額。</param>
    /// <returns>可供讀取器回傳的 Session 紀錄。</returns>
    private static UserSessionRewardRecord CreateRecord(
        int userUid,
        long rewardRecordId,
        SessionRecordStatus recordStatus,
        RewardEndStatus rewardEndStatus,
        string? keyOutAmount = null,
        string? balanceWhenKeyOut = null)
    {
        return new UserSessionRewardRecord
        {
            UserUid = userUid,
            RewardRecordId = rewardRecordId,
            RecordStatus = recordStatus,
            RewardEndStatus = rewardEndStatus,
            KeyOutAmount = keyOutAmount,
            BalanceWhenKeyOut = balanceWhenKeyOut,
            StartTime = FixedNow
        };
    }

    /// <summary>
    /// 建立包含統計資料的外部快取快照。
    /// </summary>
    /// <param name="rewardRecordId">資料庫紀錄流水號。</param>
    /// <param name="rewardEndStatus">外部快取目前的功能終止狀態。</param>
    /// <returns>可傳入更新流程的快取資料。</returns>
    private static RewardCacheDataModel CreateCache(long rewardRecordId, RewardEndStatus rewardEndStatus)
    {
        return new RewardCacheDataModel
        {
            RewardRecordId = rewardRecordId,
            RewardEndStatus = rewardEndStatus,
            TargetBalance = 1000,
            TotalGameCount = 1,
            TotalBet = 10,
            MaxBet = 10,
            MinBet = 10,
            MaxBalance = 200,
            MinBalance = 100
        };
    }

    /// <summary>
    /// 驗證更新條件只含指定進行中 Session 的三個相等條件。
    /// </summary>
    /// <param name="where">測試寫入器記錄的相等條件字典。</param>
    /// <param name="userUid">預期的玩家 UID。</param>
    /// <param name="rewardRecordId">預期的紀錄流水號。</param>
    private static void AssertActiveRecordWhere(Dictionary<string, string>? where, int userUid, long rewardRecordId)
    {
        Assert.IsNotNull(where);
        Assert.AreEqual(3, where.Count);
        Assert.AreEqual(userUid.ToString(CultureInfo.InvariantCulture), where["UserUID"]);
        Assert.AreEqual(rewardRecordId.ToString(CultureInfo.InvariantCulture), where["RewardRecordId"]);
        Assert.AreEqual("0", where["RecordStatus"]);
    }

    /// <summary>
    /// 在兩個不同玩家工作同時進入後解除等待，以驗證可平行性。
    /// </summary>
    /// <param name="current">目前已進入工作區的工作數。</param>
    /// <param name="bothStarted">兩個工作同時開始時要設定的事件。</param>
    private static void WaitForParallelWork(ref int current, ManualResetEventSlim bothStarted)
    {
        int entered = Interlocked.Increment(ref current);
        if (entered == 2)
        {
            bothStarted.Set();
        }

        bothStarted.Wait(TimeSpan.FromSeconds(2));
        Interlocked.Decrement(ref current);
    }
}

/// <summary>
/// 提供以原子操作更新最大整數值的測試輔助方法。
/// </summary>
internal static class InterlockedExtensions
{
    /// <summary>
    /// 若候選值較目前值大，則以比較交換更新目前最大值。
    /// </summary>
    /// <param name="target">要更新的目前最大值。</param>
    /// <param name="candidate">欲比較的候選值。</param>
internal static void UpdateMaximum(ref int target, int candidate)
    {
        int observed;
        do
        {
            observed = Volatile.Read(ref target);
            if (candidate <= observed)
            {
                return;
            }
        }
        while (Interlocked.CompareExchange(ref target, candidate, observed) != observed);
    }
}
