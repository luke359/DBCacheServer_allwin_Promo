using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace VerProtocol
{
    public class VerMessage
    {
        //默认密钥向量 
        static byte[] _IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        static string _Key = "dongbinhuiasxiny";//密钥,128位  
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="type">版本類型</param>
        /// <param name="RemoteEndPoint">同類型遊戲數量</param>
        /// <param name="Count">玩家遠端位址</param>
        /// <returns></returns>
        public static VerGameData Pack(VerOperationCode oper, VerVersionCode type, int Count = 0, string RemoteEndPoint = "")
        {
            List<VerGameData.ClassType> tempClassType = new List<VerGameData.ClassType>();
            tempClassType.Add(VerGameData.ClassType.versionData);
            tempClassType.Add(VerGameData.ClassType.userData);
            VerGameData SendData = new VerGameData(tempClassType);
            SendData.operationCode = oper;
            SendData.versionData.type = type;
            SendData.versionData.SameTypeGameCount = (Count != 0 ? Count : 0);
            SendData.userData.RemoteEndPoint = (RemoteEndPoint != "" ? RemoteEndPoint : "");

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="GameVersion">同類型遊戲儲存字典</param>
        /// <param name="RemoteEndPoint">玩家遠端位址</param>
        /// <returns></returns>
        public static VerGameData Pack(VerOperationCode oper, VerErrorCode error, VerVersionCode type, Dictionary<int, string> GameVersion, string RemoteEndPoint = "")
        {
            List<VerGameData.ClassType> tempClassType = new List<VerGameData.ClassType>();
            tempClassType.Add(VerGameData.ClassType.versionData);
            tempClassType.Add(VerGameData.ClassType.userData);
            VerGameData Sendata = new VerGameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.versionData.type = type;
            Sendata.versionData.SameTypeGameVersion = GameVersion;
            Sendata.userData.RemoteEndPoint = (RemoteEndPoint != "" ? RemoteEndPoint : "");

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="GameVersion">同類型遊戲儲存字典</param>
        /// <param name="RemoteEndPoint">玩家遠端位址</param>
        /// <returns></returns>
        public static VerGameData Pack(VerOperationCode oper, VerErrorCode error)
        {
            List<VerGameData.ClassType> tempClassType = new List<VerGameData.ClassType>();
            tempClassType.Add(VerGameData.ClassType.versionData);
            tempClassType.Add(VerGameData.ClassType.userData);
            VerGameData Sendata = new VerGameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            return Sendata;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="UnSerializeObj"></param>
        /// <returns></returns>
        public static byte[] SerializrToStream(object UnSerializeObj)
        {
            try
            {
                //分组加密算法
                SymmetricAlgorithm des = Rijndael.Create();
                des.Padding = PaddingMode.Zeros;
                //设置密钥及密钥向量
                des.Key = Encoding.UTF8.GetBytes(_Key);
                des.IV = _IV;

                MemoryStream stream = new MemoryStream();
                MemoryStream SerializrObject = new MemoryStream();

                CryptoStream cs = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write);

                byte[] datainfo = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(UnSerializeObj));

                cs.Write(datainfo, 0, datainfo.Length);
                cs.FlushFinalBlock();

                cs.Close();
                stream.Close();
                SerializrObject.Close();

                byte[] decryptBytes = new byte[stream.ToArray().Length];
                MemoryStream ms = new MemoryStream(stream.ToArray());

                CryptoStream cs2 = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                cs2.Read(decryptBytes, 0, decryptBytes.Length);

                cs2.Close();
                ms.Close();

                //MemoryStream stream = new MemoryStream(SerializeArray);
                MemoryStream stream2 = new MemoryStream(decryptBytes);
                //IFormatter formatter = new BinaryFormatter();
                stream2.Seek(0, SeekOrigin.Begin);

                byte[] datainfo3 = stream2.ToArray();

                //int OLength = 0;

                //for (int i = (datainfo3.Length - 1); i >= 0; i--)
                //{
                //    if (datainfo3[i] == '}')
                //    {
                //        OLength = i + 1;

                //        break;
                //    }
                //}

                //byte[] datainfo4 = new byte[OLength];

                //Array.Copy(datainfo3, datainfo4, OLength);

                string datainfo4 = Encoding.UTF8.GetString(datainfo3);

                var values = JsonConvert.DeserializeObject<Dictionary<string, Object>>(datainfo4);

                if (!values.ContainsKey("MsgType"))
                {
                    int abc = 0;

                    abc += 1;
                }

                return stream.ToArray();//得到加密后的字节数组
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Message:Serialize Error! Message : {0}", ex.Message), Console.ForegroundColor = ConsoleColor.Red);

                byte[] temp = new byte[10];

                return temp;
            }
        }

        public static byte[] SerializrToStreamUTF8(object UnSerializeObj)
        {
            try
            {
                //分组加密算法
                SymmetricAlgorithm des = Rijndael.Create();
                des.Padding = PaddingMode.Zeros;
                //设置密钥及密钥向量
                des.Key = Encoding.UTF8.GetBytes(_Key);
                des.IV = _IV;

                MemoryStream stream = new MemoryStream();
                MemoryStream SerializrObject = new MemoryStream();

                CryptoStream cs = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write);

                byte[] datainfo = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(UnSerializeObj));

                cs.Write(datainfo, 0, datainfo.Length);
                cs.FlushFinalBlock();

                cs.Close();
                stream.Close();
                SerializrObject.Close();

                byte[] decryptBytes = new byte[stream.ToArray().Length];
                MemoryStream ms = new MemoryStream(stream.ToArray());

                CryptoStream cs2 = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                cs2.Read(decryptBytes, 0, decryptBytes.Length);

                cs2.Close();
                ms.Close();

                //MemoryStream stream = new MemoryStream(SerializeArray);
                MemoryStream stream2 = new MemoryStream(decryptBytes);
                //IFormatter formatter = new BinaryFormatter();
                stream2.Seek(0, SeekOrigin.Begin);

                byte[] datainfo3 = stream2.ToArray();

                //int OLength = 0;

                //for (int i = (datainfo3.Length - 1); i >= 0; i--)
                //{
                //    if (datainfo3[i] == '}')
                //    {
                //        OLength = i + 1;

                //        break;
                //    }
                //}

                //byte[] datainfo4 = new byte[OLength];

                //Array.Copy(datainfo3, datainfo4, OLength);

                string datainfo4 = Encoding.UTF8.GetString(datainfo3);

                var values = JsonConvert.DeserializeObject<Dictionary<string, Object>>(datainfo4);

                if (!values.ContainsKey("MsgType"))
                {
                    int abc = 0;

                    abc += 1;
                }

                return stream.ToArray();//得到加密后的字节数组
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Message:Serialize Error! Message : {0}", ex.Message), Console.ForegroundColor = ConsoleColor.Red);

                byte[] temp = new byte[10];

                return temp;
            }
        }


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="SerializeArray"></param>
        /// <returns></returns>
        public static object DeserializeFromStream(byte[] SerializeArray)
        {
            try
            {
                SymmetricAlgorithm des = Rijndael.Create();
                des.Padding = PaddingMode.Zeros;
                des.Key = Encoding.UTF8.GetBytes(_Key);
                des.IV = _IV;

                byte[] decryptBytes = new byte[SerializeArray.Length];
                MemoryStream ms = new MemoryStream(SerializeArray);

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                cs.Read(decryptBytes, 0, decryptBytes.Length);

                cs.Close();
                ms.Close();

                MemoryStream stream = new MemoryStream(decryptBytes);
                stream.Seek(0, SeekOrigin.Begin);

                string datainfo3 = Encoding.UTF8.GetString(stream.ToArray());
                var values = JsonConvert.DeserializeObject<Dictionary<string, Object>>(datainfo3);
                object UnSerializeObj;
                string msgtype = values["MsgType"].ToString();

                switch (msgtype)
                {
                    case "VerGameData":
                        UnSerializeObj = JsonConvert.DeserializeObject<VerGameData>(datainfo3);
                        break;
                    default:
                        UnSerializeObj = null;
                        break;
                }

                return UnSerializeObj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Message:Deserialize Error! Message : {0}", ex.Message), Console.ForegroundColor = ConsoleColor.Red);
            }
            return null;
        }

        public static object DeserializeFromStreamUTF8(byte[] SerializeArray)
        {
            try
            {
                SymmetricAlgorithm des = Rijndael.Create();
                des.Padding = PaddingMode.Zeros;
                des.Key = Encoding.UTF8.GetBytes(_Key);
                des.IV = _IV;

                byte[] decryptBytes = new byte[SerializeArray.Length];
                MemoryStream ms = new MemoryStream(SerializeArray);

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                cs.Read(decryptBytes, 0, decryptBytes.Length);

                cs.Close();
                ms.Close();

                MemoryStream stream = new MemoryStream(decryptBytes);
                stream.Seek(0, SeekOrigin.Begin);

                string datainfo3 = Encoding.UTF8.GetString(stream.ToArray());
                var values = JsonConvert.DeserializeObject<Dictionary<string, Object>>(datainfo3);
                object UnSerializeObj;
                string msgtype = values["MsgType"].ToString();

                switch (msgtype)
                {
                    case "VerGameData":
                        UnSerializeObj = JsonConvert.DeserializeObject<VerGameData>(datainfo3);
                        break;
                    default:
                        UnSerializeObj = null;
                        break;
                }

                return UnSerializeObj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Message:Deserialize Error! Message : {0}", ex.Message), Console.ForegroundColor = ConsoleColor.Red);
            }
            return null;
        }
    }

    [Serializable, DataContract]
    public class VerGameData
    {
        public enum ClassType
        {
            userData,
            versionData,
        }

        [DataMember]
        public string MsgType = "VerGameData";

        /// <summary>傳送訊信的類型</summary>
        [DataMember]
        public VerOperationCode operationCode;
        /// <summary>回傳訊息是否成功的類型</summary>
        [DataMember]
        public VerErrorCode errorCode;
        /// <summary>回傳偵錯訊息</summary>
        [DataMember]
        public string DebugMessage;
        /// <summary>使用者資料</summary>
        [DataMember]
        public VerUserData userData;
        /// <summary>版本資訊</summary>
        [DataMember]
        public VerVersionInfo versionData;

        public VerGameData() 
        {
            MsgType = "VerGameData";
        }

        public VerGameData(List<ClassType> type)
        {
            MsgType = "VerGameData";

            for (int i = 0; i < type.Count; i++)
            {
                switch (type[i])
                {
                    case ClassType.userData:
                        userData = new VerUserData();
                        break;
                    case ClassType.versionData:
                        versionData = new VerVersionInfo();
                        break;
                }
            }
        }
    }

    [Serializable, DataContract]
    public class VerUserData
    {
        [DataMember]
        public int memberUniquelID;
        [DataMember]
        public double SessionID;
        [DataMember]
        public string RemoteEndPoint;
        [DataMember]
        public string IP;
        [DataMember]
        public string memberID;
        [DataMember]
        public string memberPW;
        [DataMember]
        public string NewPW;
        [DataMember]
        public string Nickname;
        [DataMember]
        public int Sex;
        [DataMember]
        public string Email;
        [DataMember]
        public double Balnce;
        [DataMember]
        public int ratio;
        [DataMember]
        public double WinCredit;
        [DataMember]
        public short Usersituation;
        [DataMember]
        public int Energy;
        [DataMember]
        public int Star;
        [DataMember]
        public double GameRedEnvelope;
        [DataMember]
        public Dictionary<string, VerSystemTextColor> SysTemText;
    }


    [Serializable, DataContract]
    public class VerVersionInfo
    {
        [DataMember]
        public VerVersionCode type;
        [DataMember]
        public Dictionary<int, string> SameTypeGameVersion;
        [DataMember]
        public int SameTypeGameCount;
    }
}

