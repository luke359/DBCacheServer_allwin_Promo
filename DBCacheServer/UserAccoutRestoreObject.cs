using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;

namespace DBCacheServer
{
    public class UserAccoutRestoreObject
    {
        private static readonly UserAccoutRestoreObject _instance = new UserAccoutRestoreObject();
        public static UserAccoutRestoreObject Instance => _instance;

        private static readonly object _sync = new object();
        private readonly string connectionString;

        private UserAccoutRestoreObject()
        {
            try
            {
                string host = "", id = "", pwd = "", db = "";

                foreach (var line in File.ReadAllLines("SqlConnection.txt"))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Trim().Split(':');
                    if (parts.Length < 2) continue;

                    switch (parts[0])
                    {
                        case "DBIP": host = parts[1]; break;
                        case "DBID": id = parts[1]; break;
                        case "DBPWD": pwd = parts[1]; break;
                        case "DBDataBase": db = parts[1]; break;
                    }
                }

                connectionString =
                    $"server={host};uid={id};pwd={pwd};database={db};SslMode=Disabled;" +
                    "allowpublickeyretrieval=true;charset=utf8;Allow User Variables=True;";
            }
            catch (Exception e)
            {
                MyConsole.WriteLine("Failed to read SqlConnection.txt:");
                MyConsole.WriteLine(e.Message);
            }
        }

        // ============================================================
        // ████ 1. 對外的主入口：用 UserIDList 還原所有資料 ████
        // ============================================================
        public int RestoreBackupByUserID(List<string> userIdList)
        {
            if (userIdList == null || userIdList.Count == 0)
                return 0;

            lock (_sync)
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        try
                        {
                            // Step 1：UserID → UserUID
                            List<long> uidList = GetUserUIDs(conn, tx, userIdList);

                            if (uidList.Count == 0)
                            {
                                // 無資料可還原
                                return 0;
                            }

                            // Step 2：依 UserUID 執行真正的還原邏輯
                            RestoreBackupByUIDs(conn, tx, uidList);

                            tx.Commit();

                            return (int)uidList[0];
                        }
                        catch
                        {
                            tx.Rollback();
                            throw;
                            return 0;
                        }
                    }
                }
            }
        }

        // ============================================================
        // ████ 2. 由 UserID 查 UserUID ████
        // ============================================================
        private List<long> GetUserUIDs(MySqlConnection conn, MySqlTransaction tx, List<string> userIdList)
        {
            List<long> result = new List<long>();
            string inParams = CreateInParams("uid", userIdList.Count);

            // 使用 LOWER 達成不區分大小寫比對
            string sql = $"SELECT UserUID FROM Usertable_Backup WHERE LOWER(UserID) IN ({inParams});";

            using (var cmd = new MySqlCommand(sql, conn, tx))
            {
                for (int i = 0; i < userIdList.Count; i++)
                    cmd.Parameters.AddWithValue($"@uid{i}", userIdList[i].ToLower());

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        result.Add(rd.GetInt64(0));
                }
            }

            return result;
        }

        // ============================================================
        // ████ 3. 主還原流程：依 UserUID 執行四大表還原 ████
        // ============================================================
        private void RestoreBackupByUIDs(MySqlConnection conn, MySqlTransaction tx, List<long> uidList)
        {
            if (GetBackupCount(conn, tx, uidList) == 0)
                return;

            string inParams = CreateInParams("p", uidList.Count);

            // -------------------------------
            // 1. Usertable
            // -------------------------------
            ExecuteNonQuery(conn, tx,
            $@"
            INSERT INTO Usertable (
                UserUID, EntityId, AspNetUserId, UserID, UserPwd, Nickname, Sex, UserBalance,
                Email, blockFlag, Usersituation, IsBot, Energy, Star, SessionID, FirstBetTime, 
                CalFg, PhoneNum, KeyInAward, KeyOutLimit, PlayerPlayTotal, IntroducerUID, 
                RewardStatus, UserTotalProfit, AccTotalReward, ReceivingRewardBonus, PlayerTotalBet, 
                BusinessMode, BusinessSubMode, BusinessRunCount, FeverRedBonusTotalWin, Buildtime
            )
            SELECT 
                UserUID, EntityId, AspNetUserId, UserID, UserPwd, Nickname, Sex, UserBalance,
                Email, blockFlag, Usersituation, IsBot, Energy, Star, SessionID, FirstBetTime, 
                CalFg, PhoneNum, KeyInAward, KeyOutLimit, PlayerPlayTotal, IntroducerUID, 
                RewardStatus, UserTotalProfit, AccTotalReward, ReceivingRewardBonus, PlayerTotalBet, 
                BusinessMode, BusinessSubMode, BusinessRunCount, FeverRedBonusTotalWin, Buildtime
            FROM Usertable_Backup
            WHERE UserUID IN ({inParams})
            ON DUPLICATE KEY UPDATE
                EntityId = VALUES(EntityId),
                AspNetUserId = VALUES(AspNetUserId),
                UserID = VALUES(UserID),
                UserPwd = VALUES(UserPwd),
                Nickname = VALUES(Nickname),
                Sex = VALUES(Sex),
                UserBalance = VALUES(UserBalance),
                Email = VALUES(Email),
                blockFlag = VALUES(blockFlag),
                Usersituation = VALUES(Usersituation),
                IsBot = VALUES(IsBot),
                Energy = VALUES(Energy),
                Star = VALUES(Star),
                SessionID = VALUES(SessionID),
                FirstBetTime = VALUES(FirstBetTime),
                CalFg = VALUES(CalFg),
                PhoneNum = VALUES(PhoneNum),
                KeyInAward = VALUES(KeyInAward),
                KeyOutLimit = VALUES(KeyOutLimit),
                PlayerPlayTotal = VALUES(PlayerPlayTotal),
                IntroducerUID = VALUES(IntroducerUID),
                RewardStatus = VALUES(RewardStatus),
                UserTotalProfit = VALUES(UserTotalProfit),
                AccTotalReward = VALUES(AccTotalReward),
                ReceivingRewardBonus = VALUES(ReceivingRewardBonus),
                PlayerTotalBet = VALUES(PlayerTotalBet),
                BusinessMode = VALUES(BusinessMode),
                BusinessSubMode = VALUES(BusinessSubMode),
                BusinessRunCount = VALUES(BusinessRunCount),
                FeverRedBonusTotalWin = VALUES(FeverRedBonusTotalWin),
                Buildtime = VALUES(Buildtime);
            ", uidList);

            // -------------------------------
            // 2. UserBankTable
            // -------------------------------
            ExecuteNonQuery(conn, tx,
            $@"
            INSERT INTO UserBankTable (UserUID, BuddhasPalm)
            SELECT UserUID, BuddhasPalm
            FROM UserBankTable_Backup
            WHERE UserUID IN ({inParams})
            ON DUPLICATE KEY UPDATE
                BuddhasPalm = VALUES(BuddhasPalm);
            ", uidList);

            // -------------------------------
            // 3. PlayerDayMissionTable
            // -------------------------------
            ExecuteNonQuery(conn, tx,
            $@"
            INSERT INTO PlayerDayMissionTable (
                UID, UserUID, DayMissionUID, GroupID, MissionID, Status, TaskDetail, 
                AwardDetail, Buildtime
            )
            SELECT 
                UID, UserUID, DayMissionUID, GroupID, MissionID, Status, TaskDetail, 
                AwardDetail, Buildtime
            FROM PlayerDayMissionTable_Backup
            WHERE UserUID IN ({inParams})
            ON DUPLICATE KEY UPDATE
                Status = VALUES(Status),
                TaskDetail = VALUES(TaskDetail),
                AwardDetail = VALUES(AwardDetail),
                Buildtime = VALUES(Buildtime);
            ", uidList);

            // -------------------------------
            // 4. PlayerWeekMissionTable
            // -------------------------------
            ExecuteNonQuery(conn, tx,
            $@"
            INSERT INTO PlayerWeekMissionTable (
                UID, UserUID, DayLog, MissionCount, Status, TimeStamp
            )
            SELECT 
                UID, UserUID, DayLog, MissionCount, Status, TimeStamp
            FROM PlayerWeekMissionTable_Backup
            WHERE UserUID IN ({inParams})
            ON DUPLICATE KEY UPDATE
                DayLog = VALUES(DayLog),
                MissionCount = VALUES(MissionCount),
                Status = VALUES(Status),
                TimeStamp = VALUES(TimeStamp);
            ", uidList);

            // -------------------------------
            // 最後刪除 Backup 資料
            // -------------------------------
            ExecuteNonQuery(conn, tx, $"DELETE FROM Usertable_Backup WHERE UserUID IN ({inParams});", uidList);
            ExecuteNonQuery(conn, tx, $"DELETE FROM UserBankTable_Backup WHERE UserUID IN ({inParams});", uidList);
            ExecuteNonQuery(conn, tx, $"DELETE FROM PlayerDayMissionTable_Backup WHERE UserUID IN ({inParams});", uidList);
            ExecuteNonQuery(conn, tx, $"DELETE FROM PlayerWeekMissionTable_Backup WHERE UserUID IN ({inParams});", uidList);
        }

        // ============================================================
        // ████ 4. 共用 SQL 執行方法 ████
        // ============================================================
        private void ExecuteNonQuery(MySqlConnection conn, MySqlTransaction tx, string sql, List<long> uidList = null)
        {
            using (var cmd = new MySqlCommand(sql, conn, tx))
            {
                if (uidList != null)
                {
                    for (int i = 0; i < uidList.Count; i++)
                        cmd.Parameters.AddWithValue($"@p{i}", uidList[i]);
                }

                cmd.ExecuteNonQuery();
            }
        }

        private int GetBackupCount(MySqlConnection conn, MySqlTransaction tx, List<long> uidList)
        {
            string inParams = CreateInParams("p", uidList.Count);
            string sql = $"SELECT COUNT(*) FROM Usertable_Backup WHERE UserUID IN ({inParams});";

            using (var cmd = new MySqlCommand(sql, conn, tx))
            {
                for (int i = 0; i < uidList.Count; i++)
                    cmd.Parameters.AddWithValue($"@p{i}", uidList[i]);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // 建立 @prefix0,@prefix1,@prefix2....
        private string CreateInParams(string prefix, int count)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < count; i++)
                names.Add($"@{prefix}{i}");
            return string.Join(",", names);
        }
    }
}
