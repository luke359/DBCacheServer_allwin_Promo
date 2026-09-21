using System.Globalization;
using UserSessionReward.Core.Enums;

namespace UserSessionReward.Core.Services;

/// <summary>
/// 將寫入資料庫的數值、列舉與本機時間格式化為穩定字串。
/// </summary>
internal static class MysqlValueFormatter
{
    /// <summary>
    /// 將浮點數格式化為不受目前文化設定影響的字串。
    /// </summary>
    /// <param name="value">欲格式化的浮點數。</param>
    /// <returns>使用小數點 <c>.</c> 的字串。</returns>
    internal static string FormatDouble(double value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 將洗分數值直接截斷至最多四位小數，並格式化為不含多餘尾端零的字串。
    /// </summary>
    /// <param name="value">欲格式化的非負有限浮點數。</param>
    /// <returns>使用小數點 <c>.</c> 且最多四位小數的字串。</returns>
    internal static string FormatTruncatedKeyOutDouble(double value)
    {
        const double scale = 10000d;
        double truncatedValue = Math.Truncate(value * scale) / scale;
        return truncatedValue.ToString("0.####", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 將整數格式化為不受目前文化設定影響的十進位字串。
    /// </summary>
    /// <param name="value">欲格式化的整數。</param>
    /// <returns>十進位整數字串。</returns>
    internal static string FormatInt64(long value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 將整數格式化為不受目前文化設定影響的十進位字串。
    /// </summary>
    /// <param name="value">欲格式化的整數。</param>
    /// <returns>十進位整數字串。</returns>
    internal static string FormatInt32(int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 將 Session 狀態格式化為資料庫使用的數值字串。
    /// </summary>
    /// <param name="value">欲格式化的 Session 狀態。</param>
    /// <returns>對應列舉底層數值的十進位字串。</returns>
    internal static string FormatSessionRecordStatus(SessionRecordStatus value)
    {
        return ((byte)value).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 將功能終止狀態格式化為資料庫使用的數值字串。
    /// </summary>
    /// <param name="value">欲格式化的功能終止狀態。</param>
    /// <returns>對應列舉底層數值的十進位字串。</returns>
    internal static string FormatRewardEndStatus(RewardEndStatus value)
    {
        return ((byte)value).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 將本機時間格式化為 MySQL <c>DATETIME(6)</c> 字串，不做時區轉換。
    /// </summary>
    /// <param name="value">欲格式化的本機時間。</param>
    /// <returns>格式為 <c>yyyy-MM-dd HH:mm:ss.ffffff</c> 的本機時間字串。</returns>
    internal static string FormatDateTime(DateTime value)
    {
        return value.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture);
    }
}
