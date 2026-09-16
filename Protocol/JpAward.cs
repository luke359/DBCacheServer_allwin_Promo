namespace Protocol
{
    /// <summary>JP獎項種類</summary>
    public enum JpAward
    {
        None = 0,
        MINOR = 1,
        MAJOR = 2,
        MEGA = 3,
        SUPER = 4,
        BONUS = 5
    }

    /// <summary>JP獎出牌種類</summary>
    public enum JpLotteryType
    {
        /// <summary>假出</summary>
        FAKE = 0,
        /// <summary>真出</summary>
        REAL = 1,
        /// <summary>逼牌</summary>
        Enforce = 2,
    }

    /// <summary>JP獎出牌來源</summary>
    public enum JpLotteryCast
    {
        /// <summary>一般時控逼JP</summary>
        Normal = 0,
        /// <summary>WEB逼JP</summary>
        WEB = 1,
        /// <summary>金庫逼JP</summary>
        Treasury = 2,
        /// <summary>GameServer逼JP</summary>
        Server = 3
    }
}
