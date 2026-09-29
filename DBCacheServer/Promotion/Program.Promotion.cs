namespace DBCacheServer
{
    partial class Program
    {
        /// <summary>由目前玩家快取及所屬代理商設定取得可見優惠；CLIENT 不可指定此範圍。</summary>
        internal static string GetPromotionActivityUidListForUser(int userUid)
        {
            UserData user = DBCache.Getuser(userUid);
            if (user == null)
                return "";
            EntityData entity = DBCache.GetEntityData(user.EntityId);
            return entity == null ? "" : entity.GetActivityUIDList();
        }
    }
}
