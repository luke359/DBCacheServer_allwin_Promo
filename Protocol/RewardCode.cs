using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    public enum RewardType
    {
        Mission,
        LottoTicket,
        SignIn,
        StarVoucher,
        Week,
        Rebate
    }

    public enum MissionRewardStatus
    {
        Error = 0,
        GetRewardSuccess = 1,
        MissionNotYet = 2,
        AlreadyGet = 3
    }

    public enum LottoRewardStatus
    {
        Error = 0,
        GetRewardSuccess = 1,
        NoNumber = 2,
        Missed = 3,
        AlreadyGet = 4
    }
}
