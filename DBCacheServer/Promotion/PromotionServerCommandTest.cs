using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol;

namespace DBCacheServer
{
    partial class Program
    {
        /// <summary>
        /// 後台測試正式優惠回覆。格式 promotion:cmd=Command,user=UserUID,其他Data鍵=值。
        /// 呼叫 FillPromoClientResponse；領取與放棄會執行錢包指令。
        /// </summary>
        static string RunPromotionClientResponseTest(string argument)
        {
            string text;
            try
            {
                text = BuildPromotionClientResponseTest(argument);
            }
            catch (Exception ex)
            {
                text = "promotion test failed: " + ex.GetType().Name + " " + ex.Message;
            }

            //MyConsole.WriteLine(text);
            return text;
        }

        static string BuildPromotionClientResponseTest(string argument)
        {
            if (!TryParsePromotionTestArgument(argument, out string command, out int userUid,
                out Dictionary<string, string> data, out string error))
                return error;

            CommonInfoData request = new CommonInfoData
            {
                Command = command,
                UserUID = userUid,
                GameServer = GameServerCode.None
            };
            foreach (KeyValuePair<string, string> pair in data)
                request.Data[pair.Key] = pair.Value;

            CommonInfoData response = new CommonInfoData
            {
                Command = GetPromoResponseCommand(command) ?? "",
                UserUID = userUid,
                GameServer = GameServerCode.None
            };
            FillPromoClientResponse(response, request);
            if (request.Data.TryGetValue("RequestId", out string requestId))
                response.Data["RequestId"] = requestId;

            return FormatPromotionTestResult(request, response);
        }

        static bool TryParsePromotionTestArgument(string argument, out string command, out int userUid,
            out Dictionary<string, string> data, out string error)
        {
            command = "";
            userUid = 0;
            data = new Dictionary<string, string>();
            error = "";
            bool hasUser = false;

            if (string.IsNullOrWhiteSpace(argument))
            {
                error = "promotion 參數為空。格式 promotion:cmd=PromoGetActivitiesRequest,user=1";
                return false;
            }

            string[] parts = argument.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i].Trim();
                if (part.Length == 0)
                    continue;

                int index = part.IndexOf('=');
                if (index <= 0)
                {
                    error = "promotion 參數缺少 '=': " + part;
                    return false;
                }

                string key = part.Substring(0, index).Trim();
                string value = part.Substring(index + 1).Trim();
                if (key == "cmd")
                {
                    command = value;
                }
                else if (key == "user")
                {
                    if (!int.TryParse(value, out userUid))
                    {
                        error = "Invalid UserUID: " + value;
                        return false;
                    }
                    hasUser = true;
                }
                else
                {
                    data[key] = value;
                }
            }

            if (string.IsNullOrWhiteSpace(command))
            {
                error = "promotion 缺少 cmd";
                return false;
            }
            if (!hasUser)
            {
                error = "promotion 缺少 user";
                return false;
            }
            return true;
        }

        static string FormatPromotionTestResult(CommonInfoData request, CommonInfoData response)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("promotion request Command=").Append(request.Command);
            builder.Append(" UserUID=").Append(request.UserUID);
            AppendPromotionTestData(builder, request.Data, false);
            builder.AppendLine("<br>");
            builder.Append("promotion response Command=").Append(response.Command ?? "");
            builder.Append(" UserUID=").Append(response.UserUID);
            builder.Append(" Message=").Append(response.Message ?? "");
            AppendPromotionTestData(builder, response.Data, true);
            return builder.ToString().TrimEnd();
        }

        static void AppendPromotionTestData(StringBuilder builder, Dictionary<string, string> data, bool prettyPayload)
        {
            if (data == null || data.Count == 0)
                return;

            foreach (KeyValuePair<string, string> pair in data)
            {
                builder.AppendLine("<br>");
                builder.Append(pair.Key).Append("=");
                if (prettyPayload && pair.Key == "Payload")
                {
                    builder.AppendLine("<br>");
                    builder.Append(PrettyPromotionPayload(pair.Value));
                }
                else
                {
                    builder.Append(pair.Value);
                }
            }
        }

        static string PrettyPromotionPayload(string payload)
        {
            if (string.IsNullOrEmpty(payload))
                return "";
            try
            {
                string pretty = WebUtility.HtmlEncode(JToken.Parse(payload).ToString(Formatting.Indented));
                string[] lines = pretty.Replace("\r\n", "\n").Split('\n');
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > 0)
                        builder.Append("<br>");

                    string line = lines[i];
                    int indent = 0;
                    while (indent < line.Length && line[indent] == ' ')
                        indent++;

                    for (int space = 0; space < indent; space++)
                        builder.Append("&nbsp;");
                    builder.Append(line, indent, line.Length - indent);
                }
                return builder.ToString();
            }
            catch (JsonException)
            {
                return payload;
            }
        }
    }
}
