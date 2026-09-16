namespace Protocol
{
    public enum ErrorCode
    {
        Ok = 0,
        InvalidOperation = 1,
        InvalidParameter = 2,
        CustomError = 3,
        InvalidMinStartRateLimit = 4,
        IllegalUser = 5,
        /// <summary>遊戲伺服器尚未工作</summary>
        GameServerNotWork = 6,
    }
}

