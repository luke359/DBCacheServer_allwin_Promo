using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
	/// <summary>代理商營業模式資訊</summary>
	public class BusinessModeTableData
	{
		//營業模式 開分外送 #20240807
		/// <summary>解除營業模式Credit最低限制</summary>
        const double BusinessModeBalanceLimit = 1;

		/// <summary>模式1 Enable</summary>
		public bool Mode1 = false;
		/// <summary>新玩家 自動派發</summary>
		public int Mode1AutoSend = 20;
		/// <summary>洗分限制</summary>	
		public int Mode1KeyOutLimit = 1000;
		/// <summary>洗分限制模式 0(IN玩家總押分) 1(OUT玩家Credit)</summary>
		public int Mode1KeyOutMode = 1;
		/// <summary>實際洗分</summary>
		public int Mode1RealKeyOut = 30;
		/// <summary>新玩家註冊</summary>
		public BModeISO Mode1_1 = new BModeISO();

		/// <summary>模式2 新玩家充值 Enable</summary>
		public bool Mode2 = false;
		/// <summary>洗分限制模式 0(IN玩家總押分) 1(OUT玩家Credit)</summary>
		public int Mode2KeyOutMode = 1;
		/// <summary>執行次數</summary>
		public int Mode2Times = 1;
		public BModeISO Mode2_1 = new BModeISO();
		public BModeISO Mode2_2 = new BModeISO();
		public BModeISO Mode2_3 = new BModeISO();
		public BModeISO Mode2_4 = new BModeISO();
		public BModeISO Mode2_5 = new BModeISO();
		public BModeISO Mode2_6 = new BModeISO();

		/// <summary>特殊節日開送 每日首充 Enable</summary>
		public bool Mode3 = false;
		/// <summary>特殊節日 (首充) 開始日期</summary>
		public DateTime Mode3Date = Convert.ToDateTime("2021-01-01").Date;
		/// <summary>特殊節日 (首充) 連續天數 (1~100)</summary>
		public int Mode3Days = 1;
		/// <summary>洗分限制模式 (首充) 0(IN玩家總押分) 1(OUT玩家Credit)</summary>
		public int Mode3KeyOutMode = 1;
		public BModeISO Mode3_1 = new BModeISO();
		public BModeISO Mode3_2 = new BModeISO();
		public BModeISO Mode3_3 = new BModeISO();
		public BModeISO Mode3_4 = new BModeISO();
		public BModeISO Mode3_5 = new BModeISO();
		public BModeISO Mode3_6 = new BModeISO();

		/// <summary>特殊節日開送 Enable</summary>
		public bool Mode4 = false;
		/// <summary>特殊節日 開始日期</summary>
		public DateTime Mode4Date = Convert.ToDateTime("2021-01-01").Date;
		/// <summary>特殊節日 連續天數 (1~100)</summary>
		public int Mode4Days = 1;
		/// <summary>洗分限制模式 0(IN玩家總押分) 1(OUT玩家Credit)</summary>
		public int Mode4KeyOutMode = 1;
		/// <summary>執行次數</summary>
		public int Mode4Times = 1;
		public BModeISO Mode4_1 = new BModeISO();
		public BModeISO Mode4_2 = new BModeISO();
		public BModeISO Mode4_3 = new BModeISO();
		public BModeISO Mode4_4 = new BModeISO();
		public BModeISO Mode4_5 = new BModeISO();
		public BModeISO Mode4_6 = new BModeISO();

		/// <summary>假日開送 首充 Enable</summary>
		public bool Mode5 = false;
		/// <summary>假日 (首充) 星期幾</summary>
		public string Mode5DofW = "0,0"; //星期6~日
		/// <summary>洗分限制模式 (首充) 0(IN玩家總押分) 1(OUT玩家Credit)</summary>
		public int Mode5KeyOutMode = 1;
		public BModeISO Mode5_1 = new BModeISO();
		public BModeISO Mode5_2 = new BModeISO();
		public BModeISO Mode5_3 = new BModeISO();
		public BModeISO Mode5_4 = new BModeISO();
		public BModeISO Mode5_5 = new BModeISO();
		public BModeISO Mode5_6 = new BModeISO();

		/// <summary>假日開送 Enable</summary>
		public bool Mode6 = false;
		/// <summary>假日 星期幾</summary>
		public string Mode6DofW = "0,0"; //星期6~日
		/// <summary>洗分限制模式 0(IN玩家總押分) 1(OUT玩家Credit)</summary>
		public int Mode6KeyOutMode = 1;
		/// <summary>執行次數</summary>
		public int Mode6Times = 1;
		public BModeISO Mode6_1 = new BModeISO();
		public BModeISO Mode6_2 = new BModeISO();
		public BModeISO Mode6_3 = new BModeISO();
		public BModeISO Mode6_4 = new BModeISO();
		public BModeISO Mode6_5 = new BModeISO();
		public BModeISO Mode6_6 = new BModeISO();

		/// <summary>平日開送 首充 Enable</summary>
		public bool Mode7 = false;
		/// <summary>平日 (首充) 星期幾</summary>
		public string Mode7DofW = "0,0,0,0,0"; //星期1~5
		/// <summary>洗分限制模式 (首充) 0(IN玩家總押分) 1(OUT玩家Credit)</summary>
		public int Mode7KeyOutMode = 1;
		public BModeISO Mode7_1 = new BModeISO();
		public BModeISO Mode7_2 = new BModeISO();
		public BModeISO Mode7_3 = new BModeISO();
		public BModeISO Mode7_4 = new BModeISO();
		public BModeISO Mode7_5 = new BModeISO();
		public BModeISO Mode7_6 = new BModeISO();

		/// <summary>平日開送 Enable</summary>
		public bool Mode8 = false;
		/// <summary>平日 星期幾</summary>
		public string Mode8DofW = "0,0,0,0,0"; //星期1~5
		/// <summary>洗分限制模式 0(IN玩家總押分) 1(OUT玩家Credit)</summary>
		public int Mode8KeyOutMode = 1;
		/// <summary>執行次數</summary>
		public int Mode8Times = 1;
		public BModeISO Mode8_1 = new BModeISO();
		public BModeISO Mode8_2 = new BModeISO();
		public BModeISO Mode8_3 = new BModeISO();
		public BModeISO Mode8_4 = new BModeISO();
		public BModeISO Mode8_5 = new BModeISO();
		public BModeISO Mode8_6 = new BModeISO();

		//模式9 使用Country設定
		/// <summary>一般開洗 Enable</summary>
		//public bool Mode9 = false;
		///// <summary></summary>
		//public double Mode9MoneyCoinRatio = 1.0;
		///// <summary>管理員Credit上限</summary>
		//public double Mode9ManagerBalanceLimit = 100000000000.0;
		///// <summary>管理員單筆Credit上限</summary>
		//public double Mode9ManagerDepositUnit = 100000000.0;
		///// <summary>玩家Credit上限</summary>
		//public double Mode9PlayerBalanceLimit = 100000000000.0;
		///// <summary>單筆Credit上限</summary>
		//public double Mode9PlayerDepositUnit = 100000000.0;

		/// <summary>一週有效日 (日一二三四五六)</summary>
		public bool[] AvailDofW = new bool[7]; //依照DayOfWeek排列
		/// <summary>每日首充 一週有效日 (日一二三四五六)</summary>
		public bool[] AvailDofWFr = new bool[7];


		/// <summary>營業模式開送洗</summary>
		public class BModeISO
		{
			/// <summary>開分值</summary>
			public int KeyInValue = 0;
			/// <summary>開分送值</summary>
			public int KeyInSend = 0;
			/// <summary>洗分限制</summary>
			public int KeyOutLimit = 0;

			/// <summary></summary>
			public void StrSet(string kiv, string kis, string kol)
			{
				KeyInValue = Convert.ToInt32(kiv);
				KeyInSend = Convert.ToInt32(kis);
				KeyOutLimit = Convert.ToInt32(kol);
			}
			/// <summary>開分值 和 開分送值 是有效設定</summary>
			public bool IsAvail()
			{
				if (KeyInValue > 0 && KeyInSend > 0)
				{
					return true;
				}
				return false;
			}
			/// <summary>玩家開分值 是有效設定</summary>
			/// <param name="keyIn">玩家開分值</param>
			public bool IsAvail(double keyIn)
			{
				if (KeyInValue > 0 && KeyInSend > 0)
				{
					if (keyIn == KeyInValue)
					{
						return true;
					}
				}
				return false;
			}
		}

        //營業模式 開分外送 功能
        /// <summary>營業模式</summary>
        public enum EBusinessMode
        {
            /// <summary>無模式</summary>
            None = 0,

			//特殊模式 正常情況下不會進入
            /// <summary>(模式1) 新註冊</summary>
            MD1_NewRegister,
            /// <summary>(模式2) 新玩家</summary>
            MD2_NewIn,

			//正常模式
            /// <summary>(模式3) 特殊節日 首充</summary>
            MD3_FestiveFr,
            /// <summary>(模式4) 特殊節日</summary>
            MD4_Festive,
            /// <summary>(模式5) 假日首充</summary>
            MD5_HolidayFr,
            /// <summary>(模式6) 假日</summary>
            MD6_Holiday,
            /// <summary>(模式7) 平日首充</summary>
            MD7_WeekdaysFr,
            /// <summary>(模式8) 平日</summary>
            MD8_Weekdays,
            /// <summary>(模式9) 一般</summary>
            //MD9_Normal,  模式9 直接使用Country設定, 所以不須設置模式
        }

        //public enum EBusinessMode
        //{
        //    /// <summary>(模式1) 新註冊 送</summary>
        //    NewRegister = 0,
        //    /// <summary>(模式2-1) 新玩家 開送</summary>
        //    NewIn_1,
        //    /// <summary>(模式2-2) 新玩家 開送</summary>
        //    NewIn_2,
        //    /// <summary>(模式2-3) 新玩家 開送</summary>
        //    NewIn_3,
        //    /// <summary>(模式3-1) 特殊節日 開送</summary>
        //    Festive_1,
        //    /// <summary>(模式3-2) 特殊節日 開送</summary>
        //    Festive_2,
        //    /// <summary>(模式3-3) 特殊節日 開送</summary>
        //    Festive_3,
        //    /// <summary>(模式4-1) 假日 開送</summary>
        //    Holiday_1,
        //    /// <summary>(模式4-2) 假日 開送</summary>
        //    Holiday_2,
        //    /// <summary>(模式4-3) 假日 開送</summary>
        //    Holiday_3,
        //    /// <summary>(模式5-1) 平日 開送</summary>
        //    Weekdays_1,
        //    /// <summary>(模式5-2) 平日 開送</summary>
        //    Weekdays_2,
        //    /// <summary>(模式5-3) 平日 開送</summary>
        //    Weekdays_3,
        //    /// <summary>(模式6) 一般</summary>
        //    Normal
        //}
        /// <summary>營業模式狀態</summary>
        //public enum EBusinessAct
        //{
        //    /// <summary>停止 (可洗分)</summary>
        //    None = 0,
        //    /// <summary>執行中 (不可洗分)</summary>
        //    Run,
        //}

		/// <summary>取得模式3 特殊節日 每日首充 結束日期</summary>
		public DateTime Mode3DateEnd()
		{
			return Mode3Date.AddDays(Mode4Days - 1).Date;
		}
		/// <summary>取得模式4 特殊節日 結束日期</summary>
		public DateTime Mode4DateEnd()
		{
			return Mode4Date.AddDays(Mode4Days - 1).Date;
		}

		/// <summary>取得模式 執行次數</summary>
		public int GetModeTimes(EBusinessMode mode)
		{
            if (mode == EBusinessMode.MD2_NewIn) return Mode2Times;
            if (mode == EBusinessMode.MD4_Festive) return Mode4Times;
            if (mode == EBusinessMode.MD6_Holiday) return Mode6Times;
            if (mode == EBusinessMode.MD8_Weekdays) return Mode8Times;

            if (mode == EBusinessMode.None) return 0;
            //if (mode == EBusinessMode.MD9_Normal) return 0;

            return 1;  //其餘模式只執行1次
		}

        /// <summary>取出模式開送洗</summary>
        /// <param name="mode">營業模式</param>
        /// <param name="subMode">模式子項目 (1~3/6)</param>
        public BModeISO GetBModeISO(EBusinessMode mode, int subMode)
        {
            if (mode == EBusinessMode.MD1_NewRegister)
            {
                return Mode1_1;
            }
            else if (mode == EBusinessMode.MD2_NewIn)
            {
                if(subMode == 1) return Mode2_1;
                if(subMode == 2) return Mode2_2;
                if(subMode == 3) return Mode2_3;
                if(subMode == 4) return Mode2_4;
                if(subMode == 5) return Mode2_5;
                if(subMode == 6) return Mode2_6;
            }
            else if (mode == EBusinessMode.MD3_FestiveFr)
            {
                if(subMode == 1) return Mode3_1;
                if(subMode == 2) return Mode3_2;
                if(subMode == 3) return Mode3_3;
                if(subMode == 4) return Mode3_4;
                if(subMode == 5) return Mode3_5;
                if(subMode == 6) return Mode3_6;
            }
            else if (mode == EBusinessMode.MD4_Festive)
            {
                if(subMode == 1) return Mode4_1;
                if(subMode == 2) return Mode4_2;
                if(subMode == 3) return Mode4_3;
                if(subMode == 4) return Mode4_4;
                if(subMode == 5) return Mode4_5;
                if(subMode == 6) return Mode4_6;
            }
            else if (mode == EBusinessMode.MD5_HolidayFr)
            {
                if(subMode == 1) return Mode5_1;
                if(subMode == 2) return Mode5_2;
                if(subMode == 3) return Mode5_3;
                if(subMode == 4) return Mode5_4;
                if(subMode == 5) return Mode5_5;
                if(subMode == 6) return Mode5_6;
            }
            else if (mode == EBusinessMode.MD6_Holiday)
            {
                if(subMode == 1) return Mode6_1;
                if(subMode == 2) return Mode6_2;
                if(subMode == 3) return Mode6_3;
                if(subMode == 4) return Mode6_4;
                if(subMode == 5) return Mode6_5;
                if(subMode == 6) return Mode6_6;
            }
            else if (mode == EBusinessMode.MD7_WeekdaysFr)
            {
                if(subMode == 1) return Mode7_1;
                if(subMode == 2) return Mode7_2;
                if(subMode == 3) return Mode7_3;
                if(subMode == 4) return Mode7_4;
                if(subMode == 5) return Mode7_5;
                if(subMode == 6) return Mode7_6;
            }
            else if (mode == EBusinessMode.MD8_Weekdays)
            {
                if(subMode == 1) return Mode8_1;
                if(subMode == 2) return Mode8_2;
                if(subMode == 3) return Mode8_3;
                if(subMode == 4) return Mode8_4;
                if(subMode == 5) return Mode8_5;
                if(subMode == 6) return Mode8_6;
            }

            return null;
        }

        /// <summary>取出洗分限制模式 0(IN玩家總押分) 1(OUT玩家Credit) 2(無洗分限制)</summary>
        public int GetKeyOutMode(EBusinessMode mode)
        {
            if (mode == EBusinessMode.MD1_NewRegister) return Mode1KeyOutMode;
            if (mode == EBusinessMode.MD2_NewIn) return Mode2KeyOutMode;
            if (mode == EBusinessMode.MD3_FestiveFr) return Mode3KeyOutMode;
            if (mode == EBusinessMode.MD4_Festive) return Mode4KeyOutMode;
            if (mode == EBusinessMode.MD5_HolidayFr) return Mode5KeyOutMode;
            if (mode == EBusinessMode.MD6_Holiday) return Mode6KeyOutMode;
            if (mode == EBusinessMode.MD7_WeekdaysFr) return Mode7KeyOutMode;
            if (mode == EBusinessMode.MD8_Weekdays) return Mode8KeyOutMode;
            return 2; //無模式
        }

		/// <summary>洗分限制解除判斷</summary>
		/// <param name="mode">營業模式</param>
		/// <param name="subMode">模式子項目 (1~3/6)</param>
		/// <param name="totBet">玩家總押分</param>
		/// <param name="credit">玩家Credit</param>
		public bool KeyOutUnlock(EBusinessMode mode, int subMode, double totBet, double credit)
		{
			if (credit > BusinessModeBalanceLimit)
			{
				BModeISO bMode = GetBModeISO(mode, subMode);
				if (bMode != null)
				{
					if (bMode.IsAvail()) //|| mode == EBusinessMode.NewRegister)
					{
						int koMode = GetKeyOutMode(mode);
						//依模式設定決定使用哪個條件來判斷
						if (koMode == 0)
						{
							//Use 總押分
							if (totBet < bMode.KeyOutLimit)
							{
								return false;
							}
						}
						else if (koMode == 1)
						{
							//Use Credit
							if (credit < bMode.KeyOutLimit)
							{
								return false;
							}
						}
					}
				}
			}
			else //玩家Credit <= 1幣
			{

			}

			return true; //模式狀態可解除
		}

		/// <summary>開分 模式子項目 判斷 (1~6, 0=無符合)</summary>
		public int KeyInSubMode(EBusinessMode mode, double keyIn)
		{
			//BusinessModeTableData.BModeISO bMode;

			if (mode == EBusinessMode.MD2_NewIn && Mode2)
			{
				if (Mode2_1.IsAvail(keyIn)) { return 1; }
				if (Mode2_2.IsAvail(keyIn)) { return 2; }
				if (Mode2_3.IsAvail(keyIn)) { return 3; }
				if (Mode2_4.IsAvail(keyIn)) { return 4; }
				if (Mode2_5.IsAvail(keyIn)) { return 5; }
				if (Mode2_6.IsAvail(keyIn)) { return 6; }
			}
			else if (mode == EBusinessMode.MD3_FestiveFr && Mode3)
			{
				if (Mode3_1.IsAvail(keyIn)) { return 1; }
				if (Mode3_2.IsAvail(keyIn)) { return 2; }
				if (Mode3_3.IsAvail(keyIn)) { return 3; }
				if (Mode3_4.IsAvail(keyIn)) { return 4; }
				if (Mode3_5.IsAvail(keyIn)) { return 5; }
				if (Mode3_6.IsAvail(keyIn)) { return 6; }
			}
			else if (mode == EBusinessMode.MD4_Festive && Mode4)
			{
				if (Mode4_1.IsAvail(keyIn)) { return 1; }
				if (Mode4_2.IsAvail(keyIn)) { return 2; }
				if (Mode4_3.IsAvail(keyIn)) { return 3; }
				if (Mode4_4.IsAvail(keyIn)) { return 4; }
				if (Mode4_5.IsAvail(keyIn)) { return 5; }
				if (Mode4_6.IsAvail(keyIn)) { return 6; }
			}
			else if (mode == EBusinessMode.MD5_HolidayFr && Mode5)
			{
				if (Mode5_1.IsAvail(keyIn)) { return 1; }
				if (Mode5_2.IsAvail(keyIn)) { return 2; }
				if (Mode5_3.IsAvail(keyIn)) { return 3; }
				if (Mode5_4.IsAvail(keyIn)) { return 4; }
				if (Mode5_5.IsAvail(keyIn)) { return 5; }
				if (Mode5_6.IsAvail(keyIn)) { return 6; }
			}
			else if (mode == EBusinessMode.MD6_Holiday && Mode6)
			{
				if (Mode4_4.IsAvail(keyIn)) { return 1; }
				if (Mode4_4.IsAvail(keyIn)) { return 2; }
				if (Mode4_4.IsAvail(keyIn)) { return 3; }
				if (Mode4_4.IsAvail(keyIn)) { return 4; }
				if (Mode4_4.IsAvail(keyIn)) { return 5; }
				if (Mode4_4.IsAvail(keyIn)) { return 6; }
			}
			else if (mode == EBusinessMode.MD7_WeekdaysFr && Mode7)
			{
				if (Mode7_1.IsAvail(keyIn)) { return 1; }
				if (Mode7_2.IsAvail(keyIn)) { return 2; }
				if (Mode7_3.IsAvail(keyIn)) { return 3; }
				if (Mode7_4.IsAvail(keyIn)) { return 4; }
				if (Mode7_5.IsAvail(keyIn)) { return 5; }
				if (Mode7_6.IsAvail(keyIn)) { return 6; }
			}
			else if (mode == EBusinessMode.MD8_Weekdays && Mode8)
			{
				if (Mode8_1.IsAvail(keyIn)) { return 1; }
				if (Mode8_2.IsAvail(keyIn)) { return 2; }
				if (Mode8_3.IsAvail(keyIn)) { return 3; }
				if (Mode8_4.IsAvail(keyIn)) { return 4; }
				if (Mode8_5.IsAvail(keyIn)) { return 5; }
				if (Mode8_6.IsAvail(keyIn)) { return 6; }
			}

			return 0;
		}


		/// <summary>從DB資料更新內存</summary>
		public bool GetDBData(Dictionary<string, string> datalist)
		{
			try
			{
				Mode1 = Convert.ToBoolean(datalist["Mode1"]);
				Mode1AutoSend = Convert.ToInt32(datalist["Mode1AutoSend"]);
				Mode1KeyOutLimit = Convert.ToInt32(datalist["Mode1KeyOutLimit"]);
				Mode1KeyOutMode = Convert.ToInt32(datalist["Mode1KeyOutMode"]);
				Mode1RealKeyOut = Convert.ToInt32(datalist["Mode1RealKeyOut"]);
				Mode1_1.StrSet("100", datalist["Mode1AutoSend"], datalist["Mode1KeyOutLimit"]); //模式1沒有開分值, 將其預設為100

				Mode2 = Convert.ToBoolean(datalist["Mode2"]);
				Mode2KeyOutMode = Convert.ToInt32(datalist["Mode2KeyOutMode"]);
				Mode2Times = Convert.ToInt32(datalist["Mode2Times"]);
				Mode2_1.StrSet(datalist["Mode2_1_KeyInValue"], datalist["Mode2_1_KeyInSend"], datalist["Mode2_1_KeyOutLimit"]);
				Mode2_2.StrSet(datalist["Mode2_2_KeyInValue"], datalist["Mode2_2_KeyInSend"], datalist["Mode2_2_KeyOutLimit"]);
				Mode2_3.StrSet(datalist["Mode2_3_KeyInValue"], datalist["Mode2_3_KeyInSend"], datalist["Mode2_3_KeyOutLimit"]);
				Mode2_4.StrSet(datalist["Mode2_4_KeyInValue"], datalist["Mode2_4_KeyInSend"], datalist["Mode2_4_KeyOutLimit"]);
				Mode2_5.StrSet(datalist["Mode2_5_KeyInValue"], datalist["Mode2_5_KeyInSend"], datalist["Mode2_5_KeyOutLimit"]);
				Mode2_6.StrSet(datalist["Mode2_6_KeyInValue"], datalist["Mode2_6_KeyInSend"], datalist["Mode2_6_KeyOutLimit"]);

				Mode3 = Convert.ToBoolean(datalist["Mode3"]);
				Mode3Date = Convert.ToDateTime(datalist["Mode3Date"]);
				Mode3Days = Convert.ToInt32(datalist["Mode3Days"]);
				Mode3KeyOutMode = Convert.ToInt32(datalist["Mode3KeyOutMode"]);
				Mode3_1.StrSet(datalist["Mode3_1_KeyInValue"], datalist["Mode3_1_KeyInSend"], datalist["Mode3_1_KeyOutLimit"]);
				Mode3_2.StrSet(datalist["Mode3_2_KeyInValue"], datalist["Mode3_2_KeyInSend"], datalist["Mode3_2_KeyOutLimit"]);
				Mode3_3.StrSet(datalist["Mode3_3_KeyInValue"], datalist["Mode3_3_KeyInSend"], datalist["Mode3_3_KeyOutLimit"]);
				Mode3_4.StrSet(datalist["Mode3_4_KeyInValue"], datalist["Mode3_4_KeyInSend"], datalist["Mode3_4_KeyOutLimit"]);
				Mode3_5.StrSet(datalist["Mode3_5_KeyInValue"], datalist["Mode3_5_KeyInSend"], datalist["Mode3_5_KeyOutLimit"]);
				Mode3_6.StrSet(datalist["Mode3_6_KeyInValue"], datalist["Mode3_6_KeyInSend"], datalist["Mode3_6_KeyOutLimit"]);

				Mode4 = Convert.ToBoolean(datalist["Mode4"]);
				Mode4Date = Convert.ToDateTime(datalist["Mode4Date"]);
				Mode4Days = Convert.ToInt32(datalist["Mode4Days"]);
				Mode4KeyOutMode = Convert.ToInt32(datalist["Mode4KeyOutMode"]);
				Mode4Times = Convert.ToInt32(datalist["Mode4Times"]);
				Mode4_1.StrSet(datalist["Mode4_1_KeyInValue"], datalist["Mode4_1_KeyInSend"], datalist["Mode4_1_KeyOutLimit"]);
				Mode4_2.StrSet(datalist["Mode4_2_KeyInValue"], datalist["Mode4_2_KeyInSend"], datalist["Mode4_2_KeyOutLimit"]);
				Mode4_3.StrSet(datalist["Mode4_3_KeyInValue"], datalist["Mode4_3_KeyInSend"], datalist["Mode4_3_KeyOutLimit"]);
				Mode4_4.StrSet(datalist["Mode4_4_KeyInValue"], datalist["Mode4_4_KeyInSend"], datalist["Mode4_4_KeyOutLimit"]);
				Mode4_5.StrSet(datalist["Mode4_5_KeyInValue"], datalist["Mode4_5_KeyInSend"], datalist["Mode4_5_KeyOutLimit"]);
				Mode4_6.StrSet(datalist["Mode4_6_KeyInValue"], datalist["Mode4_6_KeyInSend"], datalist["Mode4_6_KeyOutLimit"]);

				Mode5 = Convert.ToBoolean(datalist["Mode5"]);
				Mode5DofW = datalist["Mode5DofW"];
				Mode5KeyOutMode = Convert.ToInt32(datalist["Mode5KeyOutMode"]);
				Mode5_1.StrSet(datalist["Mode5_1_KeyInValue"], datalist["Mode5_1_KeyInSend"], datalist["Mode5_1_KeyOutLimit"]);
				Mode5_2.StrSet(datalist["Mode5_2_KeyInValue"], datalist["Mode5_2_KeyInSend"], datalist["Mode5_2_KeyOutLimit"]);
				Mode5_3.StrSet(datalist["Mode5_3_KeyInValue"], datalist["Mode5_3_KeyInSend"], datalist["Mode5_3_KeyOutLimit"]);
				Mode5_4.StrSet(datalist["Mode5_4_KeyInValue"], datalist["Mode5_4_KeyInSend"], datalist["Mode5_4_KeyOutLimit"]);
				Mode5_5.StrSet(datalist["Mode5_5_KeyInValue"], datalist["Mode5_5_KeyInSend"], datalist["Mode5_5_KeyOutLimit"]);
				Mode5_6.StrSet(datalist["Mode5_6_KeyInValue"], datalist["Mode5_6_KeyInSend"], datalist["Mode5_6_KeyOutLimit"]);

				Mode6 = Convert.ToBoolean(datalist["Mode6"]);
				Mode6DofW = datalist["Mode6DofW"];
				Mode6KeyOutMode = Convert.ToInt32(datalist["Mode6KeyOutMode"]);
				Mode6Times = Convert.ToInt32(datalist["Mode6Times"]);
				Mode6_1.StrSet(datalist["Mode6_1_KeyInValue"], datalist["Mode6_1_KeyInSend"], datalist["Mode6_1_KeyOutLimit"]);
				Mode6_2.StrSet(datalist["Mode6_2_KeyInValue"], datalist["Mode6_2_KeyInSend"], datalist["Mode6_2_KeyOutLimit"]);
				Mode6_3.StrSet(datalist["Mode6_3_KeyInValue"], datalist["Mode6_3_KeyInSend"], datalist["Mode6_3_KeyOutLimit"]);
				Mode6_4.StrSet(datalist["Mode6_4_KeyInValue"], datalist["Mode6_4_KeyInSend"], datalist["Mode6_4_KeyOutLimit"]);
				Mode6_5.StrSet(datalist["Mode6_5_KeyInValue"], datalist["Mode6_5_KeyInSend"], datalist["Mode6_5_KeyOutLimit"]);
				Mode6_6.StrSet(datalist["Mode6_6_KeyInValue"], datalist["Mode6_6_KeyInSend"], datalist["Mode6_6_KeyOutLimit"]);

				Mode7 = Convert.ToBoolean(datalist["Mode7"]);
				Mode7DofW = datalist["Mode7DofW"];
				Mode7KeyOutMode = Convert.ToInt32(datalist["Mode7KeyOutMode"]);
				Mode7_1.StrSet(datalist["Mode7_1_KeyInValue"], datalist["Mode7_1_KeyInSend"], datalist["Mode7_1_KeyOutLimit"]);
				Mode7_2.StrSet(datalist["Mode7_2_KeyInValue"], datalist["Mode7_2_KeyInSend"], datalist["Mode7_2_KeyOutLimit"]);
				Mode7_3.StrSet(datalist["Mode7_3_KeyInValue"], datalist["Mode7_3_KeyInSend"], datalist["Mode7_3_KeyOutLimit"]);
				Mode7_4.StrSet(datalist["Mode7_4_KeyInValue"], datalist["Mode7_4_KeyInSend"], datalist["Mode7_4_KeyOutLimit"]);
				Mode7_5.StrSet(datalist["Mode7_5_KeyInValue"], datalist["Mode7_5_KeyInSend"], datalist["Mode7_5_KeyOutLimit"]);
				Mode7_6.StrSet(datalist["Mode7_6_KeyInValue"], datalist["Mode7_6_KeyInSend"], datalist["Mode7_6_KeyOutLimit"]);

				Mode8 = Convert.ToBoolean(datalist["Mode8"]);
				Mode8DofW = datalist["Mode8DofW"];
				Mode8KeyOutMode = Convert.ToInt32(datalist["Mode8KeyOutMode"]);
				Mode8Times = Convert.ToInt32(datalist["Mode8Times"]);
				Mode8_1.StrSet(datalist["Mode8_1_KeyInValue"], datalist["Mode8_1_KeyInSend"], datalist["Mode8_1_KeyOutLimit"]);
				Mode8_2.StrSet(datalist["Mode8_2_KeyInValue"], datalist["Mode8_2_KeyInSend"], datalist["Mode8_2_KeyOutLimit"]);
				Mode8_3.StrSet(datalist["Mode8_3_KeyInValue"], datalist["Mode8_3_KeyInSend"], datalist["Mode8_3_KeyOutLimit"]);
				Mode8_4.StrSet(datalist["Mode8_4_KeyInValue"], datalist["Mode8_4_KeyInSend"], datalist["Mode8_4_KeyOutLimit"]);
				Mode8_5.StrSet(datalist["Mode8_5_KeyInValue"], datalist["Mode8_5_KeyInSend"], datalist["Mode8_5_KeyOutLimit"]);
				Mode8_6.StrSet(datalist["Mode8_6_KeyInValue"], datalist["Mode8_6_KeyInSend"], datalist["Mode8_6_KeyOutLimit"]);

				//模式9 使用Country設定
				//Mode9 = Convert.ToBoolean(datalist["Mode9"]);
				//Mode9MoneyCoinRatio = Convert.ToDouble(datalist["Mode9MoneyCoinRatio"]);
				//Mode9ManagerBalanceLimit = Convert.ToDouble(datalist["Mode9ManagerBalanceLimit"]);
				//Mode9ManagerDepositUnit = Convert.ToDouble(datalist["Mode9ManagerDepositUnit"]);
				//Mode9PlayerBalanceLimit = Convert.ToDouble(datalist["Mode9PlayerBalanceLimit"]);
				//Mode9PlayerDepositUnit = Convert.ToDouble(datalist["Mode9PlayerDepositUnit"]);


				//一週有效日設定
				SetDofW(AvailDofWFr, Mode5, Mode5DofW, Mode7, Mode7DofW);
				SetDofW(AvailDofW, Mode6, Mode6DofW, Mode8, Mode8DofW);

				//StringBuilder tstr = new StringBuilder();
				//for (int i = 0; i < 7; i++)
				//{
				//	tstr.Append(i + ":" + (AvailDofW[i] ? "1" : "0") + ", ");
				//}
				//MyConsole.WriteLine("		一週有效日[" + tstr.ToString() + "]");
				return true;
			}
            catch (Exception ex)
            {
                MyConsole.WriteLine("GetDBData Error: " + ex.Message);
                return false;
            }
        }

		/// <summary>一週有效日設定</summary>
		void SetDofW(bool[] availDofW, bool m4s, string m4, bool m5s, string m5)
		{
			for (int i = 0; i < 7; i++) availDofW[i] = false;

			if (m4s)
			{
				string[] tmp = m4.Split(',');
				if (tmp.Length >= 2)
				{
					if (tmp[0] == "1") { availDofW[6] = true; } //六
					if (tmp[1] == "1") { availDofW[0] = true; } //日
				}
			}

			if (m5s)
			{
				string[] tmp2 = m5.Split(',');
				if (tmp2.Length >= 5)
				{
					if (tmp2[0] == "1") { availDofW[1] = true; } //一
					if (tmp2[1] == "1") { availDofW[2] = true; } //二
					if (tmp2[2] == "1") { availDofW[3] = true; } //三
					if (tmp2[3] == "1") { availDofW[4] = true; } //四
					if (tmp2[4] == "1") { availDofW[5] = true; } //五
				}
			}
		}
	}
}
