using Protocol;
using System;
using System.Collections.Generic;
using System.Globalization;
using UserSessionReward.Core.Abstractions;
using UserSessionReward.Core.Enums;
using UserSessionReward.Core.Models;
using UserSessionReward.Core.Services;
using ProbabilityLib;

namespace DBCacheServer
{
    public partial class CacheManeger
    {
        #region 玩家上升額度 Session

        private const string UserSessionRewardTableName = "UserSessionRewardRecord";
        private const string MysqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";
        private const string MysqlDateTimeFormatSeconds = "yyyy-MM-dd HH:mm:ss";

        private static readonly string[] UserSessionRewardColumns =
        {
            "RewardRecordId",
            "UserUID",
            "KeyInAmount",
            "BalanceAfterKeyIn",
            "KeyOutAmount",
            "BalanceWhenKeyOut",
            "ExtraInfo",
            "TargetBalance",
            "RecordStatus",
            "RewardEndStatus",
            "EndBalance",
            "TotalGameCount",
            "TotalBet",
            "MaxBet",
            "MinBet",
            "MaxBalance",
            "MinBalance",
            "StartTime",
            "EndTime"
        };

        private static readonly HashSet<string> UserSessionRewardColumnSet =
            new HashSet<string>(UserSessionRewardColumns, StringComparer.Ordinal);

        /// <summary>玩家上升額度 Session 服務。Core 不保存 RewardCacheData，由呼叫端負責保存。</summary>
        private readonly UserSessionRewardService userSessionRewardService = BuildUserSessionRewardService();

        /// <summary>組合 Core 服務與本宿主轉接器。讀寫已接參數化 MySQL；目標餘額產生器仍為空實作。</summary>
        private static UserSessionRewardService BuildUserSessionRewardService()
        {
            return new UserSessionRewardService(
                new HostTargetBalanceGenerator(),
                new HostUserSessionRewardRecordReader(),
                new HostUserSessionRewardRecordWriter(),
                new InProcessUserOperationSerializer(),
                new SystemClock(),
                new HostUserSessionRewardLogger());
        }

        /// <summary>建立新的上升額度 Session；必要時先以再開分結束舊 Session。成功後請呼叫端保存回傳的 RewardCacheData。</summary>
        public RewardCacheDataModel CreateSession(UserData userdata, int userUid, double keyInAmount, double balanceBeforeTransaction, string rewardWebSetting)
        {
            CheckInProgressSession(userdata);  //#260821

            return userSessionRewardService.CreateSession(userUid, keyInAmount, balanceBeforeTransaction, rewardWebSetting);
        }

        /// <summary>以再開分結束指定 Session。終態 Session 會回傳 IsNoOperation，不覆寫第一次結束值。</summary>
        public SessionOperationResult EndSession(int userUid, long rewardRecordId, double balanceBeforeTransaction)
        {
            return userSessionRewardService.EndSession(userUid, rewardRecordId, balanceBeforeTransaction);
        }

        /// <summary>將一次洗分追加至進行中 Session。洗分不結束 Session，也不修改功能狀態。</summary>
        public SessionOperationResult RecordKeyOut(int userUid, long rewardRecordId, double keyOutAmount, double balanceWhenKeyOut)
        {
            return userSessionRewardService.RecordKeyOut(userUid, rewardRecordId, keyOutAmount, balanceWhenKeyOut);
        }

        /// <summary>更新進行中 Session 統計；第一次自然／Web 關閉時寫入 RewardEndStatus。rewardCacheData 不可為 null。currentBalance 僅為 API 相容保留。</summary>
        public SessionOperationResult UpdateSession(int userUid, RewardCacheDataModel rewardCacheData, double currentBalance)
        {
            return userSessionRewardService.UpdateSession(userUid, rewardCacheData, currentBalance);
        }

        /// <summary>讀取指定玩家最新的進行中 Session。沒有紀錄時為 null；若有多筆則以 StartTime 最新者為主。</summary>
        public UserSessionRewardRecord GetInProgressSession(int userUid)
        {
            IReadOnlyList<UserSessionRewardRecord> records = new HostUserSessionRewardRecordReader().GetInProgressByUserUid(userUid);
            if (records.Count == 0)
            {
                return null;
            }

            if (records.Count == 1)
            {
                return records[0];
            }

            UserSessionRewardRecord latestRecord = records[0];
            for (int i = 1; i < records.Count; i++)
            {
                if (records[i].StartTime > latestRecord.StartTime)
                {
                    latestRecord = records[i];
                }
            }

            return latestRecord;
        }

        /// <summary>Check玩家上升額度資訊紀錄</summary>
        public void CheckInProgressSession(UserData user)
        {
            var datalist = myAcess.select("Usertable", "RewardWebSetting, RewardCacheData", String.Format("UserUID = {0}", user.UserUID));

            if(datalist == null || datalist.Count == 0)
            {
                MyConsole.WriteLine($"    查詢玩家[{user.UserUID}]Session: 查無資料");
                return;
            }

            user.SetRewardCacheData(datalist[0]);

            var userSessionRecord = GetInProgressSession(user.UserUID);
            if (userSessionRecord == null)
            {
                if (user.rewardCacheDataModel.RewardRecordId > 0)
                {
                    user.EndRewardCacheData();
                    AddUserdataUpdateList(user.UserUID);
                }
                MyConsole.WriteLine($"    查詢玩家[{user.UserUID}]Session: 查無紀錄");
                return;
            }

            user.CheckInProgressSession(userSessionRecord);
        }

        /// <summary>目標餘額產生器空實作。RewardWebSetting 與演算法定案前不可產生 TargetBalance。</summary>
        private sealed class HostTargetBalanceGenerator : ITargetBalanceGenerator
        {
            public (int rewardEndStatus, double targetBalance) Generate(double balanceAfterKeyIn, double keyInAmount, string rewardWebSetting)
            {
                (int rewardEndStatus, double targetBalance) = ProbCal.KeyInProc(balanceAfterKeyIn, keyInAmount, rewardWebSetting);

                targetBalance = Math.Round(targetBalance, 4);

                //double targetBalance = Math.Round(ProbCal.GetTargetBalance(balanceAfterKeyIn, keyInAmount, rewardWebSetting), 4);
                MyConsole.WriteLine($"    產生目標餘額「{targetBalance}」 EndStat({rewardEndStatus})  RewardWebSetting=[{rewardWebSetting}]");
                return (rewardEndStatus, targetBalance);
            }
        }

        /// <summary>以參數化查詢讀取 UserSessionRewardRecord。表名與欄位受白名單限制。</summary>
        private sealed class HostUserSessionRewardRecordReader : IUserSessionRewardRecordReader
        {
            public IReadOnlyList<UserSessionRewardRecord> GetInProgressByUserUid(int userUid)
            {
                Dictionary<string, string> where = new Dictionary<string, string>
                {
                    ["UserUID"] = userUid.ToString(CultureInfo.InvariantCulture),
                    ["RecordStatus"] = ((byte)SessionRecordStatus.InProgress).ToString(CultureInfo.InvariantCulture)
                };

                List<Dictionary<string, string>> rows = MysqlAcess.GetInstance().SelectParameterized(
                    UserSessionRewardTableName,
                    UserSessionRewardColumns,
                    where);

                List<UserSessionRewardRecord> records = new List<UserSessionRewardRecord>(rows.Count);
                for (int i = 0; i < rows.Count; i++)
                {
                    records.Add(MapRecord(rows[i]));
                }

                return records;
            }

            public UserSessionRewardRecord GetByUserUidAndRewardRecordId(int userUid, long rewardRecordId)
            {
                Dictionary<string, string> where = new Dictionary<string, string>
                {
                    ["UserUID"] = userUid.ToString(CultureInfo.InvariantCulture),
                    ["RewardRecordId"] = rewardRecordId.ToString(CultureInfo.InvariantCulture)
                };

                List<Dictionary<string, string>> rows = MysqlAcess.GetInstance().SelectParameterized(
                    UserSessionRewardTableName,
                    UserSessionRewardColumns,
                    where);

                if (rows.Count == 0)
                {
                    return null;
                }

                if (rows.Count > 1)
                {
                    throw new InvalidOperationException(
                        "UserUID 與 RewardRecordId 查詢回傳超過一筆。UserUID=" + userUid
                        + " RewardRecordId=" + rewardRecordId);
                }

                return MapRecord(rows[0]);
            }

            private static UserSessionRewardRecord MapRecord(Dictionary<string, string> row)
            {
                if (row == null)
                {
                    throw new ArgumentNullException(nameof(row));
                }

                return new UserSessionRewardRecord
                {
                    RewardRecordId = ParseInt64(row, "RewardRecordId"),
                    UserUid = ParseInt32(row, "UserUID"),
                    KeyInAmount = ParseDouble(row, "KeyInAmount"),
                    BalanceAfterKeyIn = ParseDouble(row, "BalanceAfterKeyIn"),
                    KeyOutAmount = ParseOptionalString(row, "KeyOutAmount"),
                    BalanceWhenKeyOut = ParseOptionalString(row, "BalanceWhenKeyOut"),
                    ExtraInfo = ParseOptionalString(row, "ExtraInfo"),
                    TargetBalance = ParseDouble(row, "TargetBalance"),
                    RecordStatus = ParseRecordStatus(row, "RecordStatus"),
                    RewardEndStatus = ParseRewardEndStatus(row, "RewardEndStatus"),
                    EndBalance = ParseNullableDouble(row, "EndBalance"),
                    TotalGameCount = ParseInt32(row, "TotalGameCount"),
                    TotalBet = ParseDouble(row, "TotalBet"),
                    MaxBet = ParseNullableDouble(row, "MaxBet"),
                    MinBet = ParseNullableDouble(row, "MinBet"),
                    MaxBalance = ParseDouble(row, "MaxBalance"),
                    MinBalance = ParseDouble(row, "MinBalance"),
                    StartTime = ParseRequiredUtc(row, "StartTime"),
                    EndTime = ParseOptionalUtc(row, "EndTime")
                };
            }

            private static string GetRequired(Dictionary<string, string> row, string column)
            {
                string value;
                if (!row.TryGetValue(column, out value) || string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("讀取 UserSessionRewardRecord 缺少必要欄位或值為空。欄位=" + column);
                }

                return value;
            }

            private static int ParseInt32(Dictionary<string, string> row, string column)
            {
                return int.Parse(GetRequired(row, column), CultureInfo.InvariantCulture);
            }

            private static long ParseInt64(Dictionary<string, string> row, string column)
            {
                return long.Parse(GetRequired(row, column), CultureInfo.InvariantCulture);
            }

            private static double ParseDouble(Dictionary<string, string> row, string column)
            {
                return double.Parse(GetRequired(row, column), CultureInfo.InvariantCulture);
            }

            private static double? ParseNullableDouble(Dictionary<string, string> row, string column)
            {
                string value;
                if (!row.TryGetValue(column, out value) || string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                return double.Parse(value, CultureInfo.InvariantCulture);
            }

            private static string ParseOptionalString(Dictionary<string, string> row, string column)
            {
                string value;
                if (!row.TryGetValue(column, out value) || string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                return value;
            }

            private static SessionRecordStatus ParseRecordStatus(Dictionary<string, string> row, string column)
            {
                byte raw = byte.Parse(GetRequired(row, column), CultureInfo.InvariantCulture);
                if (!Enum.IsDefined(typeof(SessionRecordStatus), raw))
                {
                    throw new InvalidOperationException("無法識別的 RecordStatus。值=" + raw);
                }

                return (SessionRecordStatus)raw;
            }

            private static RewardEndStatus ParseRewardEndStatus(Dictionary<string, string> row, string column)
            {
                byte raw = byte.Parse(GetRequired(row, column), CultureInfo.InvariantCulture);
                if (!Enum.IsDefined(typeof(RewardEndStatus), raw))
                {
                    throw new InvalidOperationException("無法識別的 RewardEndStatus。值=" + raw);
                }

                return (RewardEndStatus)raw;
            }

            private static DateTime ParseRequiredUtc(Dictionary<string, string> row, string column)
            {
                DateTime? parsed = ParseOptionalUtc(row, column);
                if (!parsed.HasValue)
                {
                    throw new InvalidOperationException("讀取 UserSessionRewardRecord 缺少必要時間或值為空。欄位=" + column);
                }

                return parsed.Value;
            }

            private static DateTime? ParseOptionalUtc(Dictionary<string, string> row, string column)
            {
                string value;
                if (!row.TryGetValue(column, out value) || string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                DateTime parsed;
                if (DateTime.TryParseExact(
                        value,
                        MysqlDateTimeFormat,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeLocal,
                        out parsed)
                    || DateTime.TryParseExact(
                        value,
                        MysqlDateTimeFormatSeconds,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeLocal,
                        out parsed))
                {
                    return parsed;
                }

                throw new InvalidOperationException("無法解析本地時間。欄位=" + column + " 值=" + value);
            }
        }

        /// <summary>以參數化查詢寫入 UserSessionRewardRecord。表名與欄位受白名單限制；值不拼進 SQL。</summary>
        private sealed class HostUserSessionRewardRecordWriter : IUserSessionRewardRecordWriter
        {
            public MysqlWriteResult WriteMysql(string sqlTableName, MysqlWriteOperation operation, Dictionary<string, string> data, Dictionary<string, string> where = null)
            {
                ValidateWriteArguments(sqlTableName, operation, data, where);

                MysqlAcess mysql = MysqlAcess.GetInstance();
                switch (operation)
                {
                    case MysqlWriteOperation.Insert:
                        MysqlParameterizedWriteResult insertResult = mysql.InsertParameterized(sqlTableName, data);
                        return new MysqlWriteResult
                        {
                            AffectedRows = insertResult.AffectedRows,
                            LastInsertedId = insertResult.LastInsertedId > 0 ? insertResult.LastInsertedId : (long?)null
                        };
                    case MysqlWriteOperation.Update:
                        long affectedRows = mysql.UpdateParameterized(sqlTableName, data, where);
                        return new MysqlWriteResult
                        {
                            AffectedRows = affectedRows,
                            LastInsertedId = null
                        };
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), operation, "不支援的 MySQL 寫入操作。");
                }
            }

            private static void ValidateWriteArguments(string sqlTableName, MysqlWriteOperation operation, Dictionary<string, string> data, Dictionary<string, string> where)
            {
                if (sqlTableName == null)
                {
                    throw new ArgumentNullException(nameof(sqlTableName));
                }

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                if (!string.Equals(sqlTableName, UserSessionRewardTableName, StringComparison.Ordinal))
                {
                    throw new ArgumentException("資料表名稱不在此寫入器的白名單內。", nameof(sqlTableName));
                }

                if (data.Count == 0)
                {
                    throw new ArgumentException("寫入資料字典不得為空。", nameof(data));
                }

                ValidateColumns(data, nameof(data));

                switch (operation)
                {
                    case MysqlWriteOperation.Insert when where != null && where.Count > 0:
                        throw new ArgumentException("Insert 操作不得提供 where 條件。", nameof(where));
                    case MysqlWriteOperation.Update when where == null || where.Count == 0:
                        throw new ArgumentException("Update 操作必須提供 where 條件。", nameof(where));
                    case MysqlWriteOperation.Insert:
                        return;
                    case MysqlWriteOperation.Update:
                        ValidateColumns(where, nameof(where));
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), operation, "不支援的 MySQL 寫入操作。");
                }
            }

            private static void ValidateColumns(Dictionary<string, string> values, string paramName)
            {
                foreach (KeyValuePair<string, string> pair in values)
                {
                    if (!UserSessionRewardColumnSet.Contains(pair.Key))
                    {
                        throw new ArgumentException("欄位名稱不在白名單內。欄位=" + pair.Key, paramName);
                    }

                    if (pair.Value == null)
                    {
                        throw new ArgumentException("欄位值不得為 null。欄位=" + pair.Key, paramName);
                    }
                }
            }
        }

        /// <summary>將 Core 結構化錯誤與警告轉寫到 MyConsole。</summary>
        private sealed class HostUserSessionRewardLogger : IUserSessionRewardLogger
        {
            public void LogError(SessionRewardLogEntry entry)
            {
                WriteLog("Error", MessageType.Error, entry);
            }

            public void LogWarning(SessionRewardLogEntry entry)
            {
                WriteLog("Warning", MessageType.Warning, entry);
            }

            private static void WriteLog(string level, MessageType messageType, SessionRewardLogEntry entry)
            {
                if (entry == null)
                {
                    MyConsole.WriteLine("[UserSessionReward][" + level + "] entry 為 null。", messageType);
                    return;
                }

                string recordId = entry.RewardRecordId.HasValue ? entry.RewardRecordId.Value.ToString() : "null";
                string recordStatus = entry.RecordStatus.HasValue ? entry.RecordStatus.Value.ToString() : "null";
                string rewardEndStatus = entry.RewardEndStatus.HasValue ? entry.RewardEndStatus.Value.ToString() : "null";
                string localTime = entry.OccurredAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

                MyConsole.WriteLine(
                    "[UserSessionReward][" + level + "] Operation=" + entry.OperationName
                    + " UserUid=" + entry.UserUid
                    + " RewardRecordId=" + recordId
                    + " RecordStatus=" + recordStatus
                    + " RewardEndStatus=" + rewardEndStatus
                    + " LocalTime=" + localTime
                    + " Message=" + entry.Message,
                    messageType);
            }
        }

        #endregion
    }
}
