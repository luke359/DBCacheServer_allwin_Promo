using System;

namespace DBCacheServer
{
    internal static class ApiH5TransChannel
    {
        internal static bool TryParseWebCommand(string webCommand, out int databaseChannel)
        {
            databaseChannel = 0;
            if (string.IsNullOrEmpty(webCommand)) return false;

            string[] parts = webCommand.Split(';');
            if (parts.Length != 2 || !string.Equals(parts[0], "APIH5", StringComparison.Ordinal))
                return false;
            if (!int.TryParse(parts[1], out int messageChannel) ||
                !string.Equals(parts[1], messageChannel.ToString(), StringComparison.Ordinal))
                return false;

            switch (messageChannel)
            {
                case 4: databaseChannel = 15; return true;
                case 9: databaseChannel = 21; return true;
                case 12: databaseChannel = 24; return true;
                case 13: databaseChannel = 25; return true;
                case 14: databaseChannel = 26; return true;
                case 15: databaseChannel = 27; return true;
                case 16: databaseChannel = 28; return true;
                case 17: databaseChannel = 29; return true;
                case 18: databaseChannel = 30; return true;
                case 19: databaseChannel = 31; return true;
                case 21: databaseChannel = 33; return true;
                case 22: databaseChannel = 34; return true;
                case 23: databaseChannel = 35; return true;
                case 24: databaseChannel = 36; return true;
                case 25: databaseChannel = 37; return true;
                case 26: databaseChannel = 38; return true;
                case 27: databaseChannel = 39; return true;
                case 28: databaseChannel = 40; return true;
                case 29: databaseChannel = 41; return true;
                case 30: databaseChannel = 42; return true;
                case 31: databaseChannel = 43; return true;
                case 32: databaseChannel = 44; return true;
                case 33: databaseChannel = 45; return true;
                case 34: databaseChannel = 46; return true;
                case 35: databaseChannel = 47; return true;
                case 36: databaseChannel = 48; return true;
                case 37: databaseChannel = 49; return true;
                default: return false;
            }
        }
    }
}
