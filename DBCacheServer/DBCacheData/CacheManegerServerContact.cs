using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;
using VerProtocol;


namespace DBCacheServer
{
    public partial class CacheManeger
    {
        /// <summary>DB資料表_機台定義</summary>
        public const string DB_MachineTableDefine = "MachineTableDefine";


        #region 取得遊戲共用設定
        /// <summary>從資料庫獲取KIOS遊戲單機一般共通設定</summary>
        public static List<Dictionary<string, string>> GetDBKiosCommonGameSetting(string sRange, GameServerCode gameServerCode, GameTypeCode gType)
        {
            MysqlAcess myAcess = MysqlAcess.GetInstance();
            List<Dictionary<string, string>> datalist;

            if (gType == GameTypeCode.Slot)
                datalist = myAcess.select("KiosCommonGameSetting", "*", sRange);
            else if (gType == GameTypeCode.Arcade)
                datalist = myAcess.select("ArcadesCommonGameSetting", "*", sRange);
            else if (gType == GameTypeCode.Fish)
                datalist = myAcess.select("FishCommonGameSetting", "*", sRange);
            else
                datalist = new List<Dictionary<string, string>>(); //No Data

            return datalist;
        }
        /// <summary>從資料庫獲取遊戲單機一般共通設定</summary>
        public static List<Dictionary<string, string>> GetDBCommonGameSetting(string sRange, GameServerCode gameServerCode, GameTypeCode gType)
        {
            MysqlAcess myAcess = MysqlAcess.GetInstance();
            List<Dictionary<string, string>> datalist;

            if (gType == GameTypeCode.Slot)
                datalist = myAcess.select("CommonGameSetting", "*", sRange);
            else if (gType == GameTypeCode.Arcade)
                datalist = myAcess.select("ArcadesCommonGameSetting", "*", sRange);
            else if (gType == GameTypeCode.Fish)
                datalist = myAcess.select("FishCommonGameSetting", "*", sRange);
            else
                datalist = new List<Dictionary<string, string>>(); //No Data

            return datalist;
        }
        /// <summary>從資料庫獲取遊戲全區共用設定</summary>
        public static List<Dictionary<string, string>> GetDBCommonGlobalGameSetting(GameServerCode gameServerCode, GameTypeCode gType)
        {
            MysqlAcess myAcess = MysqlAcess.GetInstance();
            List<Dictionary<string, string>> datalist;
            string selServer = string.Format("GameServerCode = {0}", (int)gameServerCode);

            if (gType == GameTypeCode.Slot)
                datalist = myAcess.select("CommonGlobalGameSetting", "*", selServer);
            else if (gType == GameTypeCode.Arcade)
                datalist = myAcess.select("ArcadesCommonGlobalGameSetting", "*", selServer);
            else if (gType == GameTypeCode.Fish)
                datalist = myAcess.select("FishCommonGlobalGameSetting", "*", selServer);
            else
                datalist = new List<Dictionary<string, string>>(); //No Data

            return datalist;
        }
        #endregion

        #region GameTypeCode 轉換
        static public Dictionary<GameTypeCode, string> GameTypeCodeString = new Dictionary<GameTypeCode, string>{
            {GameTypeCode.Slot ,"老虎机"},
            {GameTypeCode.Fish ,"打鱼机"},
            {GameTypeCode.Arcade ,"押分机"},
            {GameTypeCode.Poker ,"扑克机"},
            //{GameTypeCode.Other ,""},
        };
        /// <summary></summary>
        static public GameTypeCode GameTypeNameToGameTypeCode(string gameTypeName)
        {
            foreach (KeyValuePair<GameTypeCode, string> info in GameTypeCodeString)
            {
                if (gameTypeName == info.Value)
                {
                    return info.Key;
                }
            }
            return GameTypeCode.Other;
        }
        /// <summary></summary>
        static public string GameTypeCodeToGameTypeName(GameTypeCode gameTypeName)
        {
            if (GameTypeCodeString.ContainsKey(gameTypeName))
            {
                return GameTypeCodeString[gameTypeName];
            }
            return "";
        }
        #endregion

        /// <summary>Show Error Message</summary>
        public static void GetDBGameSettingDataError(GameServerCode gServer, string msg = "")
        {
            Console.WriteLine($"    GetDBGameSettingData Error!!  Server[{gServer}]:{msg}");
        }


        #region GameServer物件模組化  //sroptim
        /// <summary>GameServer模組實例列表</summary>
        public Dictionary<GameServerCode, CommonGame> GameServerCodeTable;

        #region GameServer模組實例化
        //ClownsBonusBash pClownsBonusBash = new();
        //Ninja pNinja = new();
        //SuperAce pSuperAce = new();
        //GoldenDragon pGoldenDragon = new();
        //ClownsBonusBash pClownsBonusBash = new();
        //WebMuseumHeist2 pWebMuseumHeist2 = new();
        //HighWayKingPlus pHighWayKingPlus = new();
        //Star97 pStar97 = new();
        DeadDrifter pDeadDrifter = new();
        RoseHeroZ pRoseHeroZ = new();
        FortuneNeko3 pFortuneNeko3 = new();
        Medusa pMedusa = new();
        ScroogesWinterTreasure pScroogesWinterTreasure = new();
        Wukong pWukong = new();
        MahjongWinsSuperScatter pMahjongWinsSuperScatter = new();
        TreasureofAZTEC pTreasureofAZTEC = new();
        MythofNezha pMythofNezha = new();
        DoubleFortune pDoubleFortune = new();
        NieXiaoqian pNieXiaoqian = new();
        PanJinlian pPanJinlian = new();
        DeepSeaWitch pDeepSeaWitch = new();
        MuseumHeist2 pMuseumHeist2 = new();
        UrbansCannon pUrbansCannon = new();
        Olympus1000Plus pOlympus1000Plus = new();
        HangTuah pHangTuah = new();
        SetAwakened pSetAwakened = new();
        Nusantara pNusantara = new();

        //OceanKing8 pOceanKing8 = new();

        //FIFA2022 pFIFA2022 = new();
        #endregion

        /// <summary>製作GameServer模組 物件實例列表</summary>
        void InitGameInstanceTable()
        {
            //UNDONE: 製作GameServer模組 物件實例列表 -=-=-=-=-=-
            GameServerCodeTable = new Dictionary<GameServerCode, CommonGame>();

            //老虎機
            //GameServerCodeTable.Add(GameServerCode.ClownsBonusBash, pClownsBonusBash);
            //GameServerCodeTable.Add(GameServerCode.Ninja, pNinja);
            //GameServerCodeTable.Add(GameServerCode.SuperAce, pSuperAce);
            //GameServerCodeTable.Add(GameServerCode.GoldenDragon, pGoldenDragon);
            //GameServerCodeTable.Add(GameServerCode.ClownsBonusBash, pClownsBonusBash);
            //GameServerCodeTable.Add(GameServerCode.WebMuseumHeist2, pWebMuseumHeist2);
            //GameServerCodeTable.Add(GameServerCode.HighWayKingPlus, pHighWayKingPlus);
            //GameServerCodeTable.Add(GameServerCode.Star97, pStar97);
            GameServerCodeTable.Add(GameServerCode.DeadDrifter, pDeadDrifter);
            GameServerCodeTable.Add(GameServerCode.RoseHeroZ, pRoseHeroZ);
            GameServerCodeTable.Add(GameServerCode.FortuneNeko3, pFortuneNeko3);
            GameServerCodeTable.Add(GameServerCode.Medusa, pMedusa);
            GameServerCodeTable.Add(GameServerCode.ScroogesWinterTreasure, pScroogesWinterTreasure);
            GameServerCodeTable.Add(GameServerCode.Wukong, pWukong);
            GameServerCodeTable.Add(GameServerCode.MahjongWinsSuperScatter, pMahjongWinsSuperScatter);
            GameServerCodeTable.Add(GameServerCode.TreasureofAZTEC, pTreasureofAZTEC);
            GameServerCodeTable.Add(GameServerCode.MythofNezha, pMythofNezha);
            GameServerCodeTable.Add(GameServerCode.DoubleFortune, pDoubleFortune);
            GameServerCodeTable.Add(GameServerCode.NieXiaoqian, pNieXiaoqian);
            GameServerCodeTable.Add(GameServerCode.PanJinlian, pPanJinlian);
            GameServerCodeTable.Add(GameServerCode.DeepSeaWitch, pDeepSeaWitch);
            GameServerCodeTable.Add(GameServerCode.MuseumHeist2, pMuseumHeist2);
            GameServerCodeTable.Add(GameServerCode.UrbansCannon, pUrbansCannon);
            GameServerCodeTable.Add(GameServerCode.Olympus1000Plus, pOlympus1000Plus);
            GameServerCodeTable.Add(GameServerCode.HangTuah, pHangTuah);
            GameServerCodeTable.Add(GameServerCode.SetAwakened, pSetAwakened);
            GameServerCodeTable.Add(GameServerCode.Nusantara, pNusantara);

            //魚機
            //GameServerCodeTable.Add(GameServerCode.OceanKing8, pOceanKing8);


            //押分機
            //GameServerCodeTable.Add(GameServerCode.FIFA2022, pFIFA2022);


            MyConsole.WriteLine($"GameServer模組 實例列表: 數量:{GameServerCodeTable.Count}");

            InitAllGameServer();
        }


        /// <summary>GameServer啟用列表</summary>
        List<GameServerCode> serverEnableList;
        /// <summary>讀取外部檔 Server啟用列表</summary>
        public void ReadGameServerEnableTable()
        {
            serverEnableList = new();
            try
            {
                using StreamReader sr = new StreamReader("ServerEnableTable.txt");
                String text;
                while ((text = sr.ReadLine()) != null)
                {
                    GameServerCode server = GetGameServerCode(StripComment(text));
                    if (server != GameServerCode.None)
                    {
                        if (serverEnableList.Contains(server))
                        {
                            MyConsole.WriteLine($"    ReadGameServerEnableTable Warning !  Server[{server}] is already in the enable list.");
                            continue;
                        }
                        else
                        {
                            serverEnableList.Add(server);
                            //MyConsole.WriteLine($"  啟用遊戲:[{server}]");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MyConsole.WriteLine("ReadGameServerEnableTable Error !  Msg:" + e.Message);
            }
        }
        /// <summary>檢查遊戲伺服器是否啟用</summary>
        public bool IsGameServerEnable(GameServerCode serverCode)
        {
            return serverEnableList.Contains(serverCode);
        }

        /// <summary>GameServer模組 Init</summary>
        void InitAllGameServer()
        {
            try
            {
                //依照外部檔設定, 啟用GameServer
                foreach (var inst in GameServerCodeTable)
                {
                    if (IsGameServerEnable(inst.Key))
                    {
                        inst.Value.Init(CountrySetting); //sroptim
                        //MyConsole.WriteLine($"啟用遊戲伺服器: {inst.Key}");
                    }
                    else
                    {
                        inst.Value.Disable();
                        MyConsole.WriteLine($"  不啟用遊戲伺服器: {inst.Key}");
                    }
                }
            }
            catch (Exception e)
            {
                MyConsole.WriteLine("InitAllGameServer Error !  Msg:" + e.Message);
            }
        }

        /// <summary>取得GameServer物件實例</summary>
        public CommonGame GetGSInstance(GameServerCode ServerCode)
        {
            if (GameServerCodeTable.ContainsKey(ServerCode))
            {
                return GameServerCodeTable[ServerCode];
            }
            return null;
        }
        #endregion


        #region 遊戲名稱語言資訊  當GameServer全部模組化完成後, 此處即可刪除
        /// <summary>遊戲名稱語言資訊</summary>
        class GameNameLangData
        {
            /// <summary>繁中</summary>
            public string CH { get; private set; }
            /// <summary>簡中</summary>
            public string CN { get; private set; }
            /// <summary>英文</summary>
            public string EN { get; private set; }
            /// <summary>泰文</summary>
            public string TH { get; private set; }
            /// <summary>越南文</summary>
            public string VN { get; private set; }

            public GameNameLangData(string ch, string cn, string en, string th, string vn)
            {
                CH = ch;
                CN = cn;
                EN = en;
                TH = th;
                VN = vn;
            }
            /// <summary>取得遊戲名稱 全部 語言資訊 (繁中|簡中|英文|越南文|泰文)</summary>
            public string GetAllName()
            {
                return $"{CH}|{CN}|{EN}|{VN}|{TH}";
            }
        }
        static Dictionary<GameServerCode, GameNameLangData> GameNameLangInfo = new()
        {
            {GameServerCode.HightWayGame, new GameNameLangData("公路之王","公路之王","Highway Kings Gold","ไฮเวย์คิงส์โกลด์","Vua Đường Cao Tốc Vàng") },
            {GameServerCode.GoldenRooster, new GameNameLangData("金雞報喜","金鸡报喜","Golden Rooster","ไก่ทอง","Gà Vàng") },
            {GameServerCode.LargeBlue, new GameNameLangData("大藍","大蓝","Large Blue","สีฟ้าขนาดใหญ่","Xanh Lớn") },
            {GameServerCode.DolphinReef, new GameNameLangData("海豚","海豚","Dolphin","ปลาโลมา","Cá Heo") },
            {GameServerCode.PantherMoon, new GameNameLangData("豹月","豹月","Panther Moon","เสือดำพระจันทร์","Mặt trăng Báo") },
            {GameServerCode.GolfTour, new GameNameLangData("高爾夫巡迴賽","高尔夫巡回赛","Golf Tour","กอล์ฟทัวร์","Chuyến đi chơi golf") },
            {GameServerCode.FunkyMomkey, new GameNameLangData("古怪猴子","古怪猴子","Funky Monkey","ลิงฟังกี้","Chú khỉ vui nhộn") },
            {GameServerCode.FaFaFa, new GameNameLangData("財神發發發5","财神发发发5","God of Wealth 5","เทพเจ้าแห่งความมั่งคั่ง 5","Thần Tài 5") },
            {GameServerCode.ForTuneTree, new GameNameLangData("黃金樹","黄金树","Fortune Tree","ต้นโชคลาภ","Cây tài lộc") },
            {GameServerCode.Acrobatics3, new GameNameLangData("小丑3","小丑3","Acrobatics 3","การแสดงผาดโผน 3","Nhào lộn 3") },
            {GameServerCode.Butterfly, new GameNameLangData("蝴蝶","蝴蝶","Butterfly","ชัยชนะของผีเสื้อ","Chiến thắng bướm") },
            {GameServerCode.BonusBears, new GameNameLangData("獎金熊","奖金熊","Bonus Bears","โบนัสหมี"," Gấu Thưởng") },
            {GameServerCode.PandaFortune, new GameNameLangData("幸運熊貓","熊猫乐园","Panda Fortune","แพนด้าฟอร์จูน","Gấu trúc may mắn") },
            
            {GameServerCode.SeaCaptain, new GameNameLangData("船長","船长","Sea Captain","กัปตันทะเล","Thuyền Trưởng Biển")},
            {GameServerCode.TripleDragons, new GameNameLangData("龍來發","龙来发","Triple Dragons","มังกรสามตัว","Rồng Ba Đầu")},
            {GameServerCode.GoldenChips, new GameNameLangData("黃金籌碼","黄金筹码","Golden Chips","โกลเด้นชิปส์","Chip Vàng")},
            {GameServerCode.GoldBar, new GameNameLangData("黃金777","黄金777","Gold Bar","ทองคำแท่ง","Thanh Vàng")},
            {GameServerCode.BearHoliday, new GameNameLangData("小熊假期","小熊假期","Bear Holiday","หมีฮอลิเดย์","Gấu Nghỉ Mát")},
            {GameServerCode.HooksVoyage, new GameNameLangData("虎克船長","虎克船长","Hook's Voyage","การเดินทางของฮุก","Hành Trình Của Hook")},
            {GameServerCode.WildBuffalo, new GameNameLangData("牛牛牛","牛牛牛","Wild Buffalo","ควายป่า","Trâu Hoang Dã")},
            {GameServerCode.BeerTent, new GameNameLangData("啤酒嘉年華","啤酒嘉年华","Beer Tent","เต็นท์เบียร์","Lều Bia")},
            {GameServerCode.Pandarcher, new GameNameLangData("熊貓大俠","熊猫大侠","Pandarcher","แพนดาร์เชอร์","Panda Cung Kiếm")},
            {GameServerCode.Super8, new GameNameLangData("超8","超8","Super Eight","ซุปเปอร์เอท","Siêu Tám")},
            {GameServerCode.GodOfWealth2, new GameNameLangData("財神發發發2","财神发发发2","God of Wealth 2","เทพเจ้าแห่งความมั่งคั่ง2","Thần Tài 2")},
            {GameServerCode.Star97, new GameNameLangData("明星97","明星97","Star 97","สตาร์ 97","Sao 97")},
            {GameServerCode.JungleParty, new GameNameLangData("叢林樂園","丛林乐园","Jungle Party","จังเกิ้ลปาร์ตี้","Tiệc Rừng")},
            {GameServerCode.A12Bonus, new GameNameLangData("A12 Bonus","A12 Bonus","A12 Bonus","A12โบนัส","A12 Thưởng")},
            {GameServerCode.KungFu, new GameNameLangData("功夫","功夫","KungFu","กังฟู","KungFu")},
            {GameServerCode.BoyKingsTreasure, new GameNameLangData("法老王","法老王","Boy King's Treasure","สมบัติของบอยคิง","Kho Báu Vị Vương")},
            {GameServerCode.NezhaReborn, new GameNameLangData("哪吒重生","哪吒重生","NeZha Reborn","เนจา รีบอร์น","Na Tra Tái Sinh")},
            {GameServerCode.PiratesTreasure, new GameNameLangData("海盜的寶藏","海盗的宝藏","Pirate's Treasure","สมบัติของโจรสลัด","Kho Báu Cướp Biển")},
            {GameServerCode.SuperMiner, new GameNameLangData("超級礦工","超级矿工","Super Miner","ซุปเปอร์ไมเนอร์","Thợ Mỏ Siêu Cấp")},
            {GameServerCode.HighWayKingPlus, new GameNameLangData("公路之王Plus","公路之王Plus","Highway King Plus","Highway King Plus","Highway King Plus")},
            {GameServerCode.PussInBoots, new GameNameLangData("鞋貓劍客","鞋猫剑客","Puss in Boots","พุซอินบู๊ทส์","Mèo Đi Hia")},
            {GameServerCode.FireSpin, new GameNameLangData("風火輪","风火轮","Fire Spin","ไฟสปิน","Vòng Lửa")},
            {GameServerCode.Vault, new GameNameLangData("金庫","金库","Vault","ห้องนิรภัย","Két Sắt")},
            {GameServerCode.FuXingGaoZhao3, new GameNameLangData("福星高照3","福星高照3","Fu Xing Gao Zhao 3","ฟูซิงเกาจ้าว 3","Fu Xing Gao Zhao 3")},
            {GameServerCode.ModernValhalla, new GameNameLangData("摩登Valhalla","摩登Valhalla","Modern Valhalla","วัลฮัลลาสมัยใหม่","Valhalla Hiện Đại")},
            {GameServerCode.BigWinBurst, new GameNameLangData("大爆贏","大爆赢","Big Win Burst","บิ๊กวินระเบิด","Chiến Thắng Lớn")},
            {GameServerCode.WildWest, new GameNameLangData("西部狂野","西部狂野","Wild West","ป่าตะวันตก","Miền Tây Hoang Dã")},
            {GameServerCode.HooksVoyagePlus, new GameNameLangData("虎克船長Plus","虎克船长Plus","Hook's Voyage Plus","โวยาจพลัส","Hành Trình Của Hook Plus")},
            {GameServerCode.WildBeach, new GameNameLangData("熱帶海灘","热带海滩","Wild Beach","หาดป่า","Bãi Biển Hoang Dã")},
            {GameServerCode.Aladdin, new GameNameLangData("阿拉丁","阿拉丁","Aladdin","อะลาดิน","Aladdin")},
            {GameServerCode.StarCafeBar, new GameNameLangData("星咖啡","星咖啡","Star Cafe Bar","สตาร์ คาเฟ่ บาร์","Quán Cà Phê Star")},
            {GameServerCode.TopGun, new GameNameLangData("捍衛戰士","捍卫战士","Top Gun","ท็อปกัน","Top Gun")},
            {GameServerCode.WildWestMegaWay, new GameNameLangData("西部狂野2","西部狂野2","Wild West Mega Ways","วิธีเมก้า ป่าตะวันตก","Miền Tây Hoang Dã 2")},
            {GameServerCode.WildBeachParty, new GameNameLangData("沙灘派對","沙滩派对","Wild Beach Party","ปาร์ตี้ชายหาดป่า","Tiệc Bãi Biển Hoang Dã")},
            {GameServerCode.PoisonApple, new GameNameLangData("毒蘋果","毒苹果","Poison Apple","แอปเปิ้ลพิษ","Quả Táo Độc")},
            {GameServerCode.MuseumHeist, new GameNameLangData("博物館竊案","博物馆窃案","Museum Heist","การปล้นพิพิธภัณฑ์","Cuộc Cướp Bảo Tàng")},
            {GameServerCode.PinupBeauty, new GameNameLangData("海報女郎","海报女郎","Pinup Beauty","พินอัพ บิวตี้","Vẻ Đẹp Pinup")},
            {GameServerCode.RobinHood, new GameNameLangData("羅賓漢","罗宾汉","Robin Hood","โรบินฮู้ด","Robin Hood")},
            {GameServerCode.HuaMulan, new GameNameLangData("花木蘭","花木兰","Hua Mulan","มู่หลาน","Hua Mulan")},
            {GameServerCode.VietnamCuisine, new GameNameLangData("越南美食","越南美食","Vietnam Cuisine","อาหารเวียดนาม","Ẩm Thực Việt Nam")},
            {GameServerCode.PanJinlian, new GameNameLangData("潘金蓮","潘金莲","Pan Jinlian","ปาน จินเหลียน ","Pan Jinlian")},
            {GameServerCode.PastaPomodoro, new GameNameLangData("番茄義大利麵","番茄意大利面","PastaPomodoro","พาสต้าโพโมโดโร","Mì Ý Cà Chua")},
            {GameServerCode.Alice, new GameNameLangData("愛麗絲歷險記","爱丽丝历险记","Adventure Of Alice","การผจญภัยของอลิซ","Cuộc Phiêu Lưu Của Alice")},
            {GameServerCode.WildWest3, new GameNameLangData("西部狂野3","西部狂野3","Wild West 3","ป่าตะวันตก 3","Miền Tây Hoang Dã 3")},
            {GameServerCode.NezhaReborn2, new GameNameLangData("哪吒重生2","哪吒重生2","NeZha Reborn 2","เนจา รีบอร์น 2","Na Tra Tái Sinh 2")},
            {GameServerCode.PrincessRose, new GameNameLangData("玫瑰公主","玫瑰公主","Rose And The Prince","โรสและเจ้าชาย","Hoa Hồng Và Hoàng Tử")},
            {GameServerCode.DeepSeaWitch, new GameNameLangData("深海魔女","深海魔女","Deep Sea Witch","แม่มดทะเลลึก","Phù Thủy Biển Sâu")},
            {GameServerCode.NieXiaoqian, new GameNameLangData("聶小倩","聂小倩","NieXiaoqian","เนี่ยเซียวเฉียน","Nhiếp Tiểu Thiên")},
            {GameServerCode.GoldenBell, new GameNameLangData("金鐘","金钟","Golden Bell","ระฆังทอง","Chuông Vàng")},
            {GameServerCode.Juicy7, new GameNameLangData("果汁7","果汁7","Juicy 7","ฉ่ำ 7","Nước Ép 7")},
            {GameServerCode.SuperCoin1000, new GameNameLangData("超級金幣","超级金币","Super Coin 1000","เหรียญทองซูเปอร์","Tiền vàng siêu cấp")},
            {GameServerCode.WildWest3H, new GameNameLangData("西部狂野3H","西部狂野3H","Wild West 3H","ป่าตะวันตก 3H","Miền Tây Hoang Dã 3H")},
            {GameServerCode.MaJiangWins, new GameNameLangData("麻將發發發","麻将发发发","MahJong Wins","ไพ่นกกระจอกชนะ","Giành Thắng MahJong")},
            {GameServerCode.MuseumHeist2, new GameNameLangData("博物館竊案2","博物馆窃案2","Museum Heist 2","การปล้นพิพิธภัณฑ์ 2","Cuộc Cướp Bảo Tàng 2")},
            {GameServerCode.BigSmall, new GameNameLangData("比大小","比大小","BigSmall","บิ๊กสมอลล์","Lớn Nhỏ")},
            {GameServerCode.Ninja, new GameNameLangData("忍者","忍者","Ninja","นินจา","Ninja")},
            {GameServerCode.SuperAce, new GameNameLangData("超級王牌","超级王牌","SuperAce","ซุปเปอร์เอซ","Siêu Át")},
            {GameServerCode.GoldenDragon, new GameNameLangData("金龍發發發","金龙发发发","Golden Dragon","โกลเด้นดราก้อน","Rồng Vàng")},
            {GameServerCode.ClownsBonusBash, new GameNameLangData("小丑狂歡","小丑狂欢","Clown's Bonus Bash","โบนัสปาร์ตี้ตัวตลก","Tiệc Thưởng Chú Hề")},
            {GameServerCode.DeadDrifter, new GameNameLangData("惡靈戰警","恶灵战警","DeadDrifter","ตำรวจผีสิง","Cảnh Sát Linh Hồn")},
            {GameServerCode.RoseHeroZ, new GameNameLangData("玫瑰英雄","玫瑰英雄","Rose Hero Z","ฮีโร่กุหลาบ Z","Anh Hùng Hoa Hồng Z")},
            {GameServerCode.FortuneNeko3, new GameNameLangData("招財貓3","招财猫3","FortuneNeko 3","ฟอร์จูนเนโกะ","Mèo May Mắn 3")},
            {GameServerCode.Medusa, new GameNameLangData("梅杜莎","梅杜莎","Medusa","เมดูซ่า","Medusa")},
            
            {GameServerCode.XiyouGame, new GameNameLangData("齊天大聖","齐天大圣","Monkey King","ราชาลิง","Vua Khỉ")},
            {GameServerCode.SuperKingDerby, new GameNameLangData("超級賽馬之王","超级赛马之王","Super King Derby","ซุปเปอร์คิงดาร์บี้","Đua Ngựa Siêu Vương")},
            {GameServerCode.Ferrari, new GameNameLangData("法拉利","法拉利","Ferrari","เฟอร์รารี","Ferrari")},
            {GameServerCode.DrollMonkey, new GameNameLangData("猴子爬樹","猴子爬树","","","")},
            {GameServerCode.BMW3D, new GameNameLangData("3D寶馬","3D宝马","3D BMW","บีเอ็มดับเบิลยู 3มิติ","3D BMW")},
            {GameServerCode.AstonMartin, new GameNameLangData("阿斯頓馬丁","阿斯顿马丁","Aston Martin","แอสตัน มาร์ติน","Aston Martin")},
            {GameServerCode.SuperAltyn, new GameNameLangData("超級阿爾金","超级阿尔金","Super Altyn","ซุปเปอร์อัลติน","Siêu Altyn")},
            {GameServerCode.MonkeyKing2, new GameNameLangData("大聖歸來","大圣归来","Monkey King 2","ราชาลิง 2","Vua Khỉ 2")},
            {GameServerCode.FIFA2022, new GameNameLangData("足球世界杯","足球世界杯","FIFA 2022","ฟีฟ่า 2022","")},
            
            {GameServerCode.OceanKing, new GameNameLangData("海王","海王","Ocean King","โอเชี่ยนคิง","Hải Vương")},
            {GameServerCode.OceanKing2, new GameNameLangData("海王2","海王2","Ocean King 2","โอเชี่ยนคิง 2","Hải Vương 2")},
            {GameServerCode.OceanKing3, new GameNameLangData("海王3","海王3","Ocean King 3","โอเชี่ยนคิง 3","Hải Vương 3")},
            {GameServerCode.OceanKing4, new GameNameLangData("海王4","海王4","Ocean King 4","โอเชี่ยนคิง 4","Hải Vương 4")},
            {GameServerCode.OceanKing5, new GameNameLangData("海王5","海王5","Ocean King 5","โอเชี่ยนคิง 5","Hải Vương 5")},
            {GameServerCode.OceanKing6, new GameNameLangData("海王6","海王6","Ocean King 6","โอเชี่ยนคิง 6","Hải Vương 6")},
            {GameServerCode.OceanKing7, new GameNameLangData("海王7","海王7","Ocean King 7","โอเชี่ยนคิง 7","Hải Vương 7")},
            {GameServerCode.OceanKing8, new GameNameLangData("海王8","海王8","Ocean King 8","โอเชี่ยนคิง 8","Hải Vương 8")},
        };

        /// <summary>取得遊戲名稱 全部語言 (繁中|簡中|英文|越南文|泰文)</summary>
        public string GetGameNameAllLang(GameServerCode gServerCode)
        {
            //取得遊戲名稱 全部語言 //繁中|簡中|英文|越南文|泰文 #250924 為排行榜新增
            var gs = GetGSInstance(gServerCode);
            if (gs != null)
            {
                return gs.GetAllName();
            }
            
            if(GameNameLangInfo.ContainsKey(gServerCode))
            {
                return GameNameLangInfo[gServerCode].GetAllName();
            }

            return "||||";
        }

        public void GetGameServerName(GameServerCode gServerCode, out string gameNameCh, out string gameNameEng)
        {
            var gs = GetGSInstance(gServerCode);
            if (gs != null)
            {
                gameNameCh = gs.GameNameChs;
                gameNameEng = gs.GameNameEng;
                return;
            }

            if (GameNameLangInfo.ContainsKey(gServerCode))
            {
                gameNameCh = GameNameLangInfo[gServerCode].CN;
                gameNameEng = GameNameLangInfo[gServerCode].EN;
            }
            else
            {
                gameNameCh = "";
                gameNameEng = "";
            }
        }
        #endregion
    }
}
