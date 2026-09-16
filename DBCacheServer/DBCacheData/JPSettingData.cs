using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class JPSettingData
    {
        /// <summary>唯一碼</summary>
        public int JPUID;
        /// <summary>SUPER啟動值</summary>
        public int SuperInit;
        /// <summary>SUPER終止值</summary>
        public int SuperEnd;
        /// <summary>SUPER累積百分比</summary>
        public double SuperPercen;
        /// <summary>MEGA啟動值</summary>
        public int MegaInit;
        /// <summary>Mega終止值</summary>
        public int MegaEnd;
        /// <summary>Mega累積百分比</summary>
        public double MegaPercen;
        /// <summary>MAJOR啟動值</summary>
        public int MajorInit;
        /// <summary>Major終止值</summary>
        public int MajorEnd;
        /// <summary>Major累積百分比</summary>
        public double MajorPercen;
        /// <summary>MINOR啟動值</summary>
        public int MinorInit;
        /// <summary>Minor終止值</summary>
        public int MinorEnd;
        /// <summary>Minor累積百分比</summary>
        public double MinorPercen;
        /// <summary>50%JP最低總押分</summary>
        public double HalfJPGetMinBet;
        /// <summary>100%JP最低總押分</summary>
        public double AllJPGetMinBet;
        /// <summary>JP押分層級數</summary>
        public int FJP_Level;

        /// <summary>大廳大水庫(SUPER)出牌旗標</summary>
        public bool IsBigWater_SUPER;
        /// <summary>大廳大水庫(SUPER)時限</summary>
        public int BigWater_SUPER_DL;
        public int BigWater_SUPER_DU;
        public int BigWater_SUPER_HL;
        public int BigWater_SUPER_HU;
        public int BigWater_SUPER_ML;
        public int BigWater_SUPER_MU;
        /// <summary>大廳大水庫(MEGA)出牌旗標</summary>
        public bool IsBigWater_MEGA;
        /// <summary>大廳大水庫(MEGA)時限</summary>
        public int BigWater_MEGA_DL;
        public int BigWater_MEGA_DU;
        public int BigWater_MEGA_HL;
        public int BigWater_MEGA_HU;
        public int BigWater_MEGA_ML;
        public int BigWater_MEGA_MU;
        /// <summary>大廳大水庫(MAJOR)出牌旗標</summary>
        public bool IsBigWater_MAJOR;
        /// <summary>大廳大水庫(MAJOR)時限</summary>
        public int BigWater_MAJOR_DL;
        public int BigWater_MAJOR_DU;
        public int BigWater_MAJOR_HL;
        public int BigWater_MAJOR_HU;
        public int BigWater_MAJOR_ML;
        public int BigWater_MAJOR_MU;
        /// <summary>大廳大水庫(MINOR)出牌旗標</summary>
        public bool IsBigWater_MINOR;
        /// <summary>大廳大水庫(MINOR)時限</summary>
        public int BigWater_MINOR_DL;
        public int BigWater_MINOR_DU;
        public int BigWater_MINOR_HL;
        public int BigWater_MINOR_HU;
        public int BigWater_MINOR_ML;
        public int BigWater_MINOR_MU;
        /// <summary>金庫功能設定</summary>
	    public bool TreasuryFg;
	    public int TreasuryRest_L_Limit;
	    public int TreasuryRest_U_Limit;
	    public int TreasuryEnable_L_Limit;
	    public int TreasuryEnable_U_Limit;
	    public double TreasuryWin_L_Limit;
	    public double TreasuryWin_U_Limit;
        //public int TreasuryCountdownTime;
        //public int TreasuryWinTime;
        /// <summary>JP放出預報</summary>
        //public int TreasuryStatus;

        public double LittleJP1;
        public double LittleJP2;
        public double LittleJP3;
        public double LittleJP4;

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            JPUID = Convert.ToInt32(datalist["JPUID"]);
            FJP_Level = Convert.ToInt32(datalist["FJP_Level"]);  //MARKLU: JP彩金押分Level設定值 20201008
            SuperInit = Convert.ToInt32(datalist["SuperInit"]);
            SuperEnd = Convert.ToInt32(datalist["SuperEnd"]);
            SuperPercen = Convert.ToDouble(datalist["SuperPercen"]);
            MegaInit = Convert.ToInt32(datalist["MegaInit"]);
            MegaEnd = Convert.ToInt32(datalist["MegaEnd"]);
            MegaPercen = Convert.ToDouble(datalist["MegaPercen"]);
            MajorInit = Convert.ToInt32(datalist["MajorInit"]);
            MajorEnd = Convert.ToInt32(datalist["MajorEnd"]);
            MajorPercen = Convert.ToDouble(datalist["MajorPercen"]);
            MinorInit = Convert.ToInt32(datalist["MinorInit"]);
            MinorEnd = Convert.ToInt32(datalist["MinorEnd"]);
            MinorPercen = Convert.ToDouble(datalist["MinorPercen"]);
            HalfJPGetMinBet = Convert.ToDouble(datalist["HalfJPGetMinBet"]);
            AllJPGetMinBet = Convert.ToDouble(datalist["AllJPGetMinBet"]);
            IsBigWater_SUPER = Convert.ToBoolean(datalist["IsBigWater_SUPER"]);
            BigWater_SUPER_DL = Convert.ToInt32(datalist["BigWater_SUPER_DL"]);
            BigWater_SUPER_DU = Convert.ToInt32(datalist["BigWater_SUPER_DU"]);
            BigWater_SUPER_HL = Convert.ToInt32(datalist["BigWater_SUPER_HL"]);
            BigWater_SUPER_HU = Convert.ToInt32(datalist["BigWater_SUPER_HU"]);
            BigWater_SUPER_ML = Convert.ToInt32(datalist["BigWater_SUPER_ML"]);
            BigWater_SUPER_MU = Convert.ToInt32(datalist["BigWater_SUPER_MU"]);
            IsBigWater_MEGA = Convert.ToBoolean(datalist["IsBigWater_MEGA"]);
            BigWater_MEGA_DL = Convert.ToInt32(datalist["BigWater_MEGA_DL"]);
            BigWater_MEGA_DU = Convert.ToInt32(datalist["BigWater_MEGA_DU"]);
            BigWater_MEGA_HL = Convert.ToInt32(datalist["BigWater_MEGA_HL"]);
            BigWater_MEGA_HU = Convert.ToInt32(datalist["BigWater_MEGA_HU"]);
            BigWater_MEGA_ML = Convert.ToInt32(datalist["BigWater_MEGA_ML"]);
            BigWater_MEGA_MU = Convert.ToInt32(datalist["BigWater_MEGA_MU"]);
            IsBigWater_MAJOR = Convert.ToBoolean(datalist["IsBigWater_MAJOR"]);
            BigWater_MAJOR_DL = Convert.ToInt32(datalist["BigWater_MAJOR_DL"]);
            BigWater_MAJOR_DU = Convert.ToInt32(datalist["BigWater_MAJOR_DU"]);
            BigWater_MAJOR_HL = Convert.ToInt32(datalist["BigWater_MAJOR_HL"]);
            BigWater_MAJOR_HU = Convert.ToInt32(datalist["BigWater_MAJOR_HU"]);
            BigWater_MAJOR_ML = Convert.ToInt32(datalist["BigWater_MAJOR_ML"]);
            BigWater_MAJOR_MU = Convert.ToInt32(datalist["BigWater_MAJOR_MU"]);
            IsBigWater_MINOR = Convert.ToBoolean(datalist["IsBigWater_MINOR"]);
            BigWater_MINOR_DL = Convert.ToInt32(datalist["BigWater_MINOR_DL"]);
            BigWater_MINOR_DU = Convert.ToInt32(datalist["BigWater_MINOR_DU"]);
            BigWater_MINOR_HL = Convert.ToInt32(datalist["BigWater_MINOR_HL"]);
            BigWater_MINOR_HU = Convert.ToInt32(datalist["BigWater_MINOR_HU"]);
            BigWater_MINOR_ML = Convert.ToInt32(datalist["BigWater_MINOR_ML"]);
            BigWater_MINOR_MU = Convert.ToInt32(datalist["BigWater_MINOR_MU"]);
            //MARKLU: 金庫功能設定 20201008
            TreasuryFg = Convert.ToBoolean(datalist["TreasuryFg"]);
            TreasuryRest_L_Limit = Convert.ToInt32(datalist["TreasuryRest_L_Limit"]);
            TreasuryRest_U_Limit = Convert.ToInt32(datalist["TreasuryRest_U_Limit"]);
            TreasuryEnable_L_Limit = Convert.ToInt32(datalist["TreasuryEnable_L_Limit"]);
            TreasuryEnable_U_Limit = Convert.ToInt32(datalist["TreasuryEnable_U_Limit"]);
            TreasuryWin_L_Limit = Convert.ToDouble(datalist["TreasuryWin_L_Limit"]);
            TreasuryWin_U_Limit = Convert.ToDouble(datalist["TreasuryWin_U_Limit"]);
            //TreasuryCountdownTime = Convert.ToInt32(datalist["TreasuryCountdownTime"]);
            //TreasuryWinTime = Convert.ToInt32(datalist["TreasuryWinTime"]);
            LittleJP1 = Convert.ToDouble(datalist["LittleJP1"]);
            LittleJP2 = Convert.ToDouble(datalist["LittleJP2"]);
            LittleJP3 = Convert.ToDouble(datalist["LittleJP3"]);
            LittleJP4 = Convert.ToDouble(datalist["LittleJP4"]);
        }

        /// <summary>Login時取出小JP設定字串</summary>
        public string GetLittleJPStr()
        {
            return LittleJP1 + ";" + LittleJP2 + ";" + LittleJP3 + ";" + LittleJP4;
        }
    }
}
