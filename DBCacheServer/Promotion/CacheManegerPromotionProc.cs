using Promotion.Core.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCacheServer
{
    public partial class CacheManeger
    {
        static readonly ConcurrentDictionary<int, object> PromotionWagerLocks = new ConcurrentDictionary<int, object>();
        static readonly ConcurrentDictionary<int, PromotionWagerFreeze> PromotionSettlementFrozen =
            new ConcurrentDictionary<int, PromotionWagerFreeze>();
        static readonly ConcurrentDictionary<int, PromotionPendingClose> PromotionPendingCloses =
            new ConcurrentDictionary<int, PromotionPendingClose>();

        /// <summary>優惠活動 累計流水。同一玩家的累計與結案連續做完，中間不接受下一局。serialNumber 是該筆 userGameData 的玩家流水號。</summary>
        void PromotionAccumulateWager(UserData user, double totBet, double totWin, long serialNumber)
        {
            if (!PromotionCoreHost.IsReady || PromotionCoreHost.Service == null || user == null)
                return;

            int userUid = user.UserUID;
            lock (PromotionWagerLocks.GetOrAdd(userUid, _ => new object()))
            {
                if (PromotionSettlementFrozen.ContainsKey(userUid))
                {
                    MyConsole.WriteLine(
                        "Promotion 此玩家優惠結算已停止，本局不送入：UserUID=" + userUid
                        + " SerialNumber=" + serialNumber
                        + " Bet=" + totBet
                        + " Win=" + totWin);
                    return;
                }

                if (!TryFinishPendingPromotionClose(userUid))
                {
                    MyConsole.WriteLine(
                        "Promotion 上一局結案尚未完成，本局不送入：UserUID=" + userUid
                        + " SerialNumber=" + serialNumber
                        + " Bet=" + totBet
                        + " Win=" + totWin);
                    return;
                }

                if (serialNumber <= 0)
                {
                    MyConsole.WriteLine(
                        "Promotion 遊戲局沒有 SerialNumber，不送入核心：UserUID=" + userUid
                        + " Bet=" + totBet
                        + " Win=" + totWin);
                    return;
                }

                if (!PromotionCoreHost.HasActiveBonusTask(userUid))
                    return;

                PromotionWagerRoundStore rounds = new PromotionWagerRoundStore(MysqlAcess.GetInstance());
                bool claimed;
                try
                {
                    claimed = rounds.TryClaim(userUid, serialNumber);
                }
                catch (Exception ex)
                {
                    MyConsole.WriteLine(
                        "Promotion 遊戲局去重失敗，本局不送入：UserUID=" + userUid
                        + " SerialNumber=" + serialNumber
                        + " " + ex.Message);
                    return;
                }

                if (!claimed)
                {
                    MyConsole.WriteLine(
                        "Promotion 同一局已送過，不再累計：UserUID=" + userUid
                        + " SerialNumber=" + serialNumber);
                    return;
                }

                DateTime gameTime = DateTime.Now;
                double settledBonus = user.Balance2;
                PromotionResult<AccumulateWagerData> accResult = PromotionCoreHost.TryAccumulateWager(
                    userUid, totBet, totWin, settledBonus, gameTime);
                if (accResult == null)
                {
                    MyConsole.WriteLine(
                        "Promotion 核心沒有回結果，本局序號保留不再送入：UserUID=" + userUid
                        + " SerialNumber=" + serialNumber);
                    return;
                }

                if (accResult.Kind == PromotionResultKind.RetryableFailure)
                {
                    try
                    {
                        rounds.Release(userUid, serialNumber);
                    }
                    catch (Exception ex)
                    {
                        MyConsole.WriteLine(
                            "Promotion 遊戲局序號未能釋放：UserUID=" + userUid
                            + " SerialNumber=" + serialNumber
                            + " " + ex.Message);
                    }
                    return;
                }

                if (accResult.Kind == PromotionResultKind.OutcomeUnknown)
                {
                    FreezePromotionSettlement(userUid, "AccumulateWager", accResult, totBet, totWin, settledBonus, gameTime, null);
                    return;
                }

                if (accResult.Kind != PromotionResultKind.Succeeded || accResult.Data == null)
                    return;

                AccumulateWagerData data = accResult.Data;
                if (data.Outcome == BonusTaskOutcome.ReadyToClose)
                {
                    ClosePromotionTask(userUid, data.BonusTaskId, totBet, totWin, settledBonus, gameTime, false);
                    return;
                }

                if (data.Outcome == BonusTaskOutcome.InProgress &&
                    PromotionCoreHost.IsZeroPromotionAmount(settledBonus))
                {
                    ClosePromotionTask(userUid, data.BonusTaskId, totBet, totWin, settledBonus, gameTime, true);
                }
            }
        }

        /// <summary>補做尚未完成的結案。成功後才允許送入新局。</summary>
        bool TryFinishPendingPromotionClose(int userUid)
        {
            if (!PromotionPendingCloses.TryGetValue(userUid, out PromotionPendingClose pending))
                return true;

            PromotionResult<CloseBonusTaskData> result = pending.Depleted
                ? PromotionCoreHost.TryCloseDepleted(userUid, pending.BonusTaskId, pending.GameTime)
                : PromotionCoreHost.TryCloseWagerCompleted(
                    userUid, pending.BonusTaskId, pending.Bet, pending.Win, pending.SettledBonusBalance, pending.GameTime);

            if (result != null && result.Kind == PromotionResultKind.Succeeded)
            {
                ApplyPromotionCloseWallet(result.Data == null ? null : result.Data.WalletInstructions);
                PromotionPendingCloses.TryRemove(userUid, out _);
                return true;
            }

            if (result != null && result.Kind == PromotionResultKind.OutcomeUnknown)
            {
                FreezePromotionSettlement(
                    userUid, pending.Depleted ? "CloseDepletedBonusTask" : "CloseWagerCompletedBonusTask",
                    result, pending.Bet, pending.Win, pending.SettledBonusBalance, pending.GameTime, pending.BonusTaskId);
                PromotionPendingCloses.TryRemove(userUid, out _);
                return false;
            }

            if (result == null || result.Kind == PromotionResultKind.RetryableFailure)
                return false;

            PromotionPendingCloses.TryRemove(userUid, out _);
            return true;
        }

        void ClosePromotionTask(
            int userUid, string bonusTaskId, double bet, double win, double settledBonus, DateTime gameTime, bool depleted)
        {
            PromotionResult<CloseBonusTaskData> result = depleted
                ? PromotionCoreHost.TryCloseDepleted(userUid, bonusTaskId, gameTime)
                : PromotionCoreHost.TryCloseWagerCompleted(userUid, bonusTaskId, bet, win, settledBonus, gameTime);

            if (result != null && result.Kind == PromotionResultKind.Succeeded)
            {
                ApplyPromotionCloseWallet(result.Data == null ? null : result.Data.WalletInstructions);
                return;
            }

            if (result != null && result.Kind == PromotionResultKind.OutcomeUnknown)
            {
                FreezePromotionSettlement(
                    userUid, depleted ? "CloseDepletedBonusTask" : "CloseWagerCompletedBonusTask",
                    result, bet, win, settledBonus, gameTime, bonusTaskId);
                return;
            }

            if (result == null || result.Kind == PromotionResultKind.RetryableFailure)
            {
                PromotionPendingCloses[userUid] = new PromotionPendingClose
                {
                    BonusTaskId = bonusTaskId,
                    Bet = bet,
                    Win = win,
                    SettledBonusBalance = settledBonus,
                    GameTime = gameTime,
                    Depleted = depleted
                };
                MyConsole.WriteLine(
                    "Promotion 結案尚未完成，此玩家暫停送入新局：UserUID=" + userUid
                    + " BonusTaskId=" + bonusTaskId);
            }
        }

        static void ApplyPromotionCloseWallet(IReadOnlyList<WalletInstructionDto> instructions)
        {
            IReadOnlyList<WalletInstructionDto> ordered = OrderPromotionWalletInstructions(instructions);
            if (ordered == null || ordered.Count == 0)
                return;

            PromotionWalletExecutionResult wallet = PromotionWalletDispatcher.Execute(ordered);
            if (!wallet.AllSucceeded)
            {
                MyConsole.WriteLine(
                    "Promotion 結案錢包尚未完成：" + wallet.ErrorCode + " " + wallet.Message);
            }
        }

        /// <summary>清空紅利錢包先於把解鎖金加入主錢包。</summary>
        static IReadOnlyList<WalletInstructionDto> OrderPromotionWalletInstructions(
            IReadOnlyList<WalletInstructionDto> instructions)
        {
            if (instructions == null || instructions.Count == 0)
                return instructions;

            List<WalletInstructionDto> clear = new List<WalletInstructionDto>();
            List<WalletInstructionDto> other = new List<WalletInstructionDto>();
            List<WalletInstructionDto> creditMain = new List<WalletInstructionDto>();
            for (int i = 0; i < instructions.Count; i++)
            {
                WalletInstructionDto item = instructions[i];
                if (item.Action == WalletAction.ClearBonusWallet)
                    clear.Add(item);
                else if (item.Action == WalletAction.CreditMainWallet)
                    creditMain.Add(item);
                else
                    other.Add(item);
            }

            clear.AddRange(other);
            clear.AddRange(creditMain);
            return clear;
        }

        static void FreezePromotionSettlement<T>(
            int userUid, string stage, PromotionResult<T> result,
            double bet, double win, double settledBonus, DateTime gameTime, string bonusTaskId)
        {
            string taskId = bonusTaskId;
            if (string.IsNullOrEmpty(taskId) && result != null && result.ErrorDetails != null)
                taskId = result.ErrorDetails.ExistingBonusTaskId;

            PromotionWagerFreeze record = new PromotionWagerFreeze
            {
                UserUid = userUid,
                Stage = stage,
                BonusTaskId = taskId ?? "",
                Bet = bet,
                Win = win,
                SettledBonusBalance = settledBonus,
                GameTime = gameTime,
                CorrelationId = result == null ? "" : result.CorrelationId,
                ErrorCode = result == null ? PromotionErrorCode.None : result.ErrorCode,
                RecordedAt = DateTime.Now
            };
            PromotionSettlementFrozen.TryAdd(userUid, record);
            MyConsole.WriteLine(
                "Promotion 結果不明，已停止此玩家後續優惠結算：UserUID=" + record.UserUid
                + " Stage=" + record.Stage
                + " BonusTaskId=" + record.BonusTaskId
                + " Bet=" + record.Bet
                + " Win=" + record.Win
                + " SettledBonusBalance=" + record.SettledBonusBalance
                + " GameTime=" + record.GameTime.ToString("yyyy-MM-dd HH:mm:ss.fff")
                + " ErrorCode=" + record.ErrorCode
                + " CorrelationId=" + record.CorrelationId
                + "。不可重送本局，請依遊戲紀錄與任務狀態人工對帳。");
        }

        sealed class PromotionPendingClose
        {
            public string BonusTaskId;
            public double Bet;
            public double Win;
            public double SettledBonusBalance;
            public DateTime GameTime;
            public bool Depleted;
        }

        sealed class PromotionWagerFreeze
        {
            public int UserUid;
            public string Stage;
            public string BonusTaskId;
            public double Bet;
            public double Win;
            public double SettledBonusBalance;
            public DateTime GameTime;
            public string CorrelationId;
            public PromotionErrorCode ErrorCode;
            public DateTime RecordedAt;
        }

        /// <summary>依已保存的錢包指令異動記憶體餘額，並排入玩家資料寫回。</summary>
        internal bool TryApplyPromotionWallet(WalletInstructionDto instruction, out string error)
        {
            error = null;
            if (instruction == null || instruction.UserUID <= 0 || instruction.UserUID > int.MaxValue)
            {
                error = "玩家編號無效";
                return false;
            }
            if (instruction.Amount < 0 ||
                (instruction.Action == WalletAction.ClearBonusWallet && instruction.Amount != 0))
            {
                error = "錢包金額無效";
                return false;
            }

            int userUid = (int)instruction.UserUID;
            double amount = Math.Round((double)instruction.Amount, Program.AccuracyDigitBal);
            if (instruction.Action == WalletAction.CreditMainWallet)
                return TryCreditPromotionMainWallet(userUid, amount, out error);

            lock (BatchDepositV2PlayerLocks.GetOrAdd(userUid, _ => new object()))
            lock (UserDataList)
            {
                if (!UserDataList.ContainsKey(userUid))
                {
                    error = "玩家不在快取，錢包尚未入帳";
                    return false;
                }

                UserData user = UserDataList[userUid];
                if (instruction.Action == WalletAction.CreditBonusWallet)
                    user.CreditPromotionBonusWallet(amount);
                else if (instruction.Action == WalletAction.ClearBonusWallet)
                    user.ClearPromotionBonusWallet();
                else
                {
                    error = "未知的錢包動作";
                    return false;
                }

                AddUserdataUpdateList(userUid);
                return true;
            }
        }

        bool TryCreditPromotionMainWallet(int userUid, double amount, out string error)
        {
            error = null;
            lock (UserDataList)
            {
                if (!UserDataList.ContainsKey(userUid))
                {
                    error = "玩家不在快取，錢包尚未入帳";
                    return false;
                }
            }

            UpdateUserBalance(amount, userUid, UpdateBalanceSource.Promotion);
            lock (UserDataList)
            {
                if (!UserDataList.ContainsKey(userUid))
                {
                    error = "玩家不在快取，錢包尚未入帳";
                    return false;
                }
            }
            return true;
        }
    }
}
