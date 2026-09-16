using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public abstract class JPControl
    {
        public double InitPoint;
        public string JPType;
        public int frequencyindex;

        public Dictionary<int, int> Countfrequency;

        abstract public void GetWeights(int Hour);

        abstract public void GetFrequencyIndex();

        abstract public void ReseatInitPoint(int value);
    }

    public class SuperJP : JPControl
    {
        public SuperJP(double init)
        {
            //MysqlAcess myAcess = MysqlAcess.GetInstance();
            //var datalist = myAcess.select("JPSettingtable", "SuperInit", string.Format("JPUID = '1'"));
            InitPoint = init;
            JPType = "SuperShow";
            Countfrequency = new Dictionary<int, int> { { 50, 0 }, { 40, 0 }, { 30, 0 }, { 20, 0 }, { 10, 0 } };
        }

        /// <summary>
        /// 取的權重
        /// </summary>
        /// <param name="Hour"></param>
        public override void GetWeights(int Hour)
        {
            ClearCountfrequency();

            switch (Hour)
            {
                #region 00:00
                case 0:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(75, 86);

                        Countfrequency[40] = seed;
                        Countfrequency[30] = 100 - seed;
                    }
                    break;
                #endregion
                #region 01:00
                case 1:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 3);
                        int seed_2 = random.Next(75, 86);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 02:00
                case 2:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(75, 86);

                        Countfrequency[40] = seed;
                        Countfrequency[30] = 100 - seed;
                    }
                    break;
                #endregion
                #region 03:00
                case 3:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(30, 41);

                        Countfrequency[40] = seed_1;
                        Countfrequency[30] = seed_2;
                        Countfrequency[20] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 04:00
                case 4:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(30, 41);
                        int seed_3 = random.Next(5, 11);

                        Countfrequency[40] = seed_1;
                        Countfrequency[30] = seed_2;
                        Countfrequency[20] = seed_3;
                        Countfrequency[10] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 05:00
                case 5:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 3);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 06:00
                case 6:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(70, 81);

                        Countfrequency[40] = seed;
                        Countfrequency[30] = 100 - seed;
                    }
                    break;
                #endregion
                #region 07:00
                case 7:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 08:00
                case 8:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 09:00
                case 9:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(35, 46);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 10:00
                case 10:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(45, 56);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 11:00
                case 11:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(20, 31);

                        Countfrequency[50] = seed;
                        Countfrequency[40] = 100 - seed;
                    }
                    break;
                #endregion
                #region 12:00
                case 12:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(35, 46);

                        Countfrequency[50] = seed;
                        Countfrequency[40] = 100 - seed;

                    }
                    break;
                #endregion
                #region 13:00
                case 13:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(45, 51);

                        Countfrequency[50] = seed;
                        Countfrequency[40] = 100 - seed;
                    }
                    break;
                #endregion
                #region 14:00
                case 14:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(15, 26);

                        Countfrequency[50] = seed;
                        Countfrequency[40] = 100 - seed;
                    }
                    break;
                #endregion
                #region 15:00
                case 15:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(5, 16);

                        Countfrequency[50] = seed;
                        Countfrequency[40] = 100 - seed;
                    }
                    break;
                #endregion
                #region 16:00
                case 16:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 4);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 17:00
                case 17:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(2, 9);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 18:00
                case 18:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(82, 93);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 19:00
                case 19:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 20:00
                case 20:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 3);
                        int seed_2 = random.Next(75, 86);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 21:00
                case 21:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[50] = seed_1;
                        Countfrequency[40] = seed_2;
                        Countfrequency[30] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 22:00
                case 22:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(5, 11);

                        Countfrequency[50] = seed;
                        Countfrequency[40] = 100 - seed;
                    }
                    break;
                #endregion
                #region 23:00
                case 23:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(20, 31);

                        Countfrequency[50] = seed;
                        Countfrequency[40] = 100 - seed;
                    }
                    break;
                    #endregion
            }
        }

        /// <summary>
        /// 算出增值頻率
        /// </summary>
        public override void GetFrequencyIndex()
        {
            List<int> tempWeights = new List<int>();

            foreach (var Key in Countfrequency.Keys)
            {
                if (Countfrequency[Key] != 0 && tempWeights.Count < 100)
                {
                    for (int i = 0; i < Countfrequency[Key]; i++)
                    {
                        tempWeights.Add(Key);
                    }
                }
            }

            Random random = new Random(DateTime.Now.Millisecond);
            int index = random.Next(0, 100);

            frequencyindex = tempWeights[index];
        }

        /// <summary>清空所有value</summary>
        private void ClearCountfrequency()
        {
            for (int i = 0; i < Countfrequency.Count; i++)
            {
                Countfrequency[i] = 0;
            }
        }

        /// <summary>重置JP</summary>
        public override void ReseatInitPoint(int value)
        {
            InitPoint = value;
        }
    }

    public class MegaJP : JPControl
    {
        public MegaJP(double init)
        {
            //MysqlAcess myAcess = MysqlAcess.GetInstance();
            //var datalist = myAcess.select("JPSettingtable", "MegaInit", string.Format("JPUID = '1'"));
            InitPoint = init;
            JPType = "MegaShow";
            Countfrequency = new Dictionary<int, int> { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 } };
        }

        /// <summary>
        /// 取的權重
        /// </summary>
        /// <param name="Hour"></param>
        public override void GetWeights(int Hour)
        {
            ClearCountfrequency();

            switch (Hour)
            {
                #region 00:00
                case 0:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(75, 86);

                        Countfrequency[1] = seed;
                        Countfrequency[2] = 100 - seed;
                    }
                    break;
                #endregion
                #region 01:00
                case 1:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 3);
                        int seed_2 = random.Next(75, 86);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 02:00
                case 2:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(75, 86);

                        Countfrequency[1] = seed;
                        Countfrequency[2] = 100 - seed;
                    }
                    break;
                #endregion
                #region 03:00
                case 3:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(30, 41);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 04:00
                case 4:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(30, 41);
                        int seed_3 = random.Next(5, 11);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 05:00
                case 5:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 3);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 06:00
                case 6:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(70, 81);

                        Countfrequency[1] = seed;
                        Countfrequency[2] = 100 - seed;
                    }
                    break;
                #endregion
                #region 07:00
                case 7:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 08:00
                case 8:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 09:00
                case 9:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(35, 46);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 10:00
                case 10:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(45, 56);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 11:00
                case 11:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(20, 31);

                        Countfrequency[0] = seed;
                        Countfrequency[1] = 100 - seed;
                    }
                    break;
                #endregion
                #region 12:00
                case 12:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(35, 46);

                        Countfrequency[0] = seed;
                        Countfrequency[1] = 100 - seed;

                    }
                    break;
                #endregion
                #region 13:00
                case 13:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(45, 51);

                        Countfrequency[0] = seed;
                        Countfrequency[1] = 100 - seed;
                    }
                    break;
                #endregion
                #region 14:00
                case 14:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(15, 26);

                        Countfrequency[0] = seed;
                        Countfrequency[1] = 100 - seed;
                    }
                    break;
                #endregion
                #region 15:00
                case 15:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(5, 16);

                        Countfrequency[0] = seed;
                        Countfrequency[1] = 100 - seed;
                    }
                    break;
                #endregion
                #region 16:00
                case 16:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 4);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 17:00
                case 17:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(2, 9);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 18:00
                case 18:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(82, 93);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 19:00
                case 19:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 20:00
                case 20:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 3);
                        int seed_2 = random.Next(75, 86);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 21:00
                case 21:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 22:00
                case 22:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(5, 11);

                        Countfrequency[0] = seed;
                        Countfrequency[1] = 100 - seed;
                    }
                    break;
                #endregion
                #region 23:00
                case 23:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(20, 31);

                        Countfrequency[0] = seed;
                        Countfrequency[1] = 100 - seed;
                    }
                    break;
                    #endregion
            }
        }
        /// <summary>
        /// 算出增值頻率
        /// </summary>
        public override void GetFrequencyIndex()
        {
            List<int> tempWeights = new List<int>();

            foreach (var Key in Countfrequency.Keys)
            {
                if (Countfrequency[Key] != 0 && tempWeights.Count < 100)
                {
                    for (int i = 0; i < Countfrequency[Key]; i++)
                    {
                        tempWeights.Add(Key);
                    }
                }
            }

            Random random = new Random(DateTime.Now.Millisecond);
            int index = random.Next(0, 100);

            frequencyindex = tempWeights[index];
        }

        /// <summary>清空所有value</summary>
        private void ClearCountfrequency()
        {
            for (int i = 0; i < Countfrequency.Count; i++)
            {
                Countfrequency[i] = 0;
            }
        }

        /// <summary>重置JP</summary>
        public override void ReseatInitPoint(int value)
        {
            InitPoint = value;
        }
    }

    public class MajorJP : JPControl
    {
        public MajorJP(double init)
        {
            //MysqlAcess myAcess = MysqlAcess.GetInstance();
            //var datalist = myAcess.select("JPSettingtable", "MajorInit", string.Format("JPUID = '1'"));
            InitPoint = init;
            JPType = "MajorShow";
            Countfrequency = new Dictionary<int, int> { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 } };
        }

        /// <summary>
        /// 取的權重
        /// </summary>
        /// <param name="Hour"></param>
        public override void GetWeights(int Hour)
        {
            ClearCountfrequency();

            switch (Hour)
            {
                #region 00:00
                case 0:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(30, 41);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 01:00
                case 1:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(50, 61);
                        int seed_2 = random.Next(20, 31);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 02:00
                case 2:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(45, 56);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 03:00
                case 3:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(45, 56);
                        int seed_2 = random.Next(20, 31);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 04:00
                case 4:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(15, 26);
                        int seed_2 = random.Next(15, 26);
                        int seed_3 = random.Next(45, 56);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 05:00
                case 5:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(55, 66);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 06:00
                case 6:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 07:00
                case 7:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(10, 21);
                        int seed_3 = random.Next(10, 21);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = seed_3;
                        Countfrequency[3] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 08:00
                case 8:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(10, 21);
                        int seed_3 = random.Next(10, 21);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = seed_3;
                        Countfrequency[3] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 09:00
                case 9:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(25, 36);
                        int seed_3 = random.Next(10, 21);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = seed_3;
                        Countfrequency[3] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 10:00
                case 10:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(30, 41);
                        int seed_3 = random.Next(10, 21);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = seed_3;
                        Countfrequency[3] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 11:00
                case 11:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(82, 93);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 12:00
                case 12:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(10, 21);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = 100 - seed_1;
                    }
                    break;
                #endregion
                #region 13:00
                case 13:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(15, 26);

                        Countfrequency[0] = seed;
                        Countfrequency[1] = 100 - seed;
                    }
                    break;
                #endregion
                #region 14:00
                case 14:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(80, 91);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 15:00
                case 15:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 4);
                        int seed_2 = random.Next(70, 81);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 16:00
                case 16:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(55, 66);
                        int seed_2 = random.Next(25, 36);
                        int seed_3 = random.Next(1, 4);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 17:00
                case 17:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(55, 66);
                        int seed_2 = random.Next(25, 36);
                        int seed_3 = random.Next(0, 3);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 18:00
                case 18:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(55, 66);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 19:00
                case 19:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(50, 61);
                        int seed_2 = random.Next(30, 41);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 20:00
                case 20:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(55, 66);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 21:00
                case 21:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(55, 66);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 22:00
                case 22:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(70, 81);

                        Countfrequency[1] = seed;
                        Countfrequency[2] = 100 - seed;
                    }
                    break;
                #endregion
                #region 23:00
                case 23:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(85, 96);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = 100 - (seed_1 + seed_2);
                    }
                    break;
                    #endregion
            }
        }

        /// <summary>
        /// 算出增值頻率
        /// </summary>
        public override void GetFrequencyIndex()
        {
            List<int> tempWeights = new List<int>();

            foreach (var Key in Countfrequency.Keys)
            {
                if (Countfrequency[Key] != 0 && tempWeights.Count < 100)
                {
                    for (int i = 0; i < Countfrequency[Key]; i++)
                    {
                        tempWeights.Add(Key);
                    }
                }
            }

            Random random = new Random(DateTime.Now.Millisecond);
            int index = random.Next(0, 100);

            frequencyindex = tempWeights[index];
        }

        /// <summary>清空所有value</summary>
        private void ClearCountfrequency()
        {
            for (int i = 0; i < Countfrequency.Count; i++)
            {
                Countfrequency[i] = 0;
            }
        }

        /// <summary>重置JP</summary>
        public override void ReseatInitPoint(int value)
        {
            InitPoint = value;
        }
    }

    public class MinorJP : JPControl
    {
        public MinorJP(double init)
        {
            //MysqlAcess myAcess = MysqlAcess.GetInstance();
            //var datalist = myAcess.select("JPSettingtable", "MinorInit", string.Format("JPUID = '1'"));
            InitPoint = init;
            JPType = "MinorShow";
            Countfrequency = new Dictionary<int, int> { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 } };
        }

        /// <summary>
        /// 取的權重
        /// </summary>
        /// <param name="Hour"></param>
        public override void GetWeights(int Hour)
        {
            ClearCountfrequency();

            switch (Hour)
            {
                #region 00:00
                case 0:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 4);
                        int seed_2 = random.Next(90, 100);
                        int seed_3 = random.Next(0, 2);
                        int seed_4 = random.Next(0, 2);

                        Countfrequency[2] = seed_1;
                        Countfrequency[3] = seed_2;
                        Countfrequency[4] = seed_3;
                        Countfrequency[5] = seed_4;
                        Countfrequency[6] = 100 - (seed_1 + seed_2 + seed_3 + seed_4);
                    }
                    break;
                #endregion
                #region 01:00
                case 1:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 6);
                        int seed_2 = random.Next(85, 96);
                        int seed_3 = random.Next(2, 6);
                        int seed_4 = random.Next(1, 4);

                        Countfrequency[2] = seed_1;
                        Countfrequency[3] = seed_2;
                        Countfrequency[4] = seed_3;
                        Countfrequency[5] = seed_4;
                        Countfrequency[6] = 100 - (seed_1 + seed_2 + seed_3 + seed_4);
                    }
                    break;
                #endregion
                #region 02:00
                case 2:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 4);
                        int seed_2 = random.Next(75, 86);
                        int seed_3 = random.Next(3, 6);
                        int seed_4 = random.Next(3, 6);

                        Countfrequency[2] = seed_1;
                        Countfrequency[3] = seed_2;
                        Countfrequency[4] = seed_3;
                        Countfrequency[5] = seed_4;
                        Countfrequency[6] = 100 - (seed_1 + seed_2 + seed_3 + seed_4);
                    }
                    break;
                #endregion
                #region 03:00
                case 3:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(1, 4);
                        int seed_2 = random.Next(50, 61);
                        int seed_3 = random.Next(1, 4);
                        int seed_4 = random.Next(5, 16);
                        int seed_5 = random.Next(2, 5);
                        int seed_6 = random.Next(10, 21);

                        Countfrequency[2] = seed_1;
                        Countfrequency[3] = seed_2;
                        Countfrequency[4] = seed_3;
                        Countfrequency[5] = seed_4;
                        Countfrequency[6] = seed_5;
                        Countfrequency[7] = seed_6;
                        Countfrequency[8] = 100 - (seed_1 + seed_2 + seed_3 + seed_4 + seed_5 + seed_6);
                    }
                    break;
                #endregion
                #region 04:00
                case 4:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(45, 56);
                        int seed_2 = random.Next(0, 3);
                        int seed_3 = random.Next(5, 16);
                        int seed_4 = random.Next(4, 11);
                        int seed_5 = random.Next(15, 26);

                        Countfrequency[3] = seed_1;
                        Countfrequency[4] = seed_2;
                        Countfrequency[5] = seed_3;
                        Countfrequency[6] = seed_4;
                        Countfrequency[7] = seed_5;
                        Countfrequency[8] = 100 - (seed_1 + seed_2 + seed_3 + seed_4 + seed_5);
                    }
                    break;
                #endregion
                #region 05:00
                case 5:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed = random.Next(1, 4);

                        Countfrequency[2] = seed;
                        Countfrequency[3] = 100 - seed;
                    }
                    break;
                #endregion
                #region 06:00
                case 6:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(77, 84);
                        int seed_2 = random.Next(8, 19);
                        int seed_3 = random.Next(5, 16);
                        int seed_4 = random.Next(2, 5);
                        int seed_5 = random.Next(0, 3);
                        int seed_6 = random.Next(2, 5);

                        Countfrequency[2] = seed_1;
                        Countfrequency[3] = seed_2;
                        Countfrequency[4] = seed_3;
                        Countfrequency[5] = seed_4;
                        Countfrequency[6] = seed_5;
                        Countfrequency[7] = seed_6;
                        Countfrequency[8] = 100 - (seed_1 + seed_2 + seed_3 + seed_4 + seed_5 + seed_6);
                    }
                    break;
                #endregion
                #region 07:00
                case 7:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(45, 56);
                        int seed_2 = random.Next(1, 4);
                        int seed_3 = random.Next(0, 3);
                        int seed_4 = random.Next(35, 46);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = seed_3;
                        Countfrequency[3] = seed_4;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3 + seed_4);
                    }
                    break;
                #endregion
                #region 08:00
                case 8:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(45, 56);
                        int seed_2 = random.Next(0, 3);
                        int seed_3 = random.Next(1, 4);
                        int seed_4 = random.Next(39, 42);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = seed_3;
                        Countfrequency[3] = seed_4;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3 + seed_4);
                    }
                    break;
                #endregion
                #region 09:00
                case 9:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(0, 3);
                        int seed_3 = random.Next(0, 3);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = seed_3;
                        Countfrequency[3] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 10:00
                case 10:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(40, 51);
                        int seed_2 = random.Next(5, 16);
                        int seed_3 = random.Next(5, 16);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = seed_3;
                        Countfrequency[3] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 11:00
                case 11:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(25, 36);
                        int seed_2 = random.Next(20, 31);
                        int seed_3 = random.Next(35, 46);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 12:00
                case 12:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(45, 56);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 13:00
                case 13:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(55, 66);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 14:00
                case 14:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(30, 41);
                        int seed_2 = random.Next(25, 36);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 15:00
                case 15:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(15, 26);
                        int seed_2 = random.Next(20, 31);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 16:00
                case 16:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(5, 16);
                        int seed_2 = random.Next(10, 21);
                        int seed_3 = random.Next(10, 21);
                        int seed_4 = random.Next(45, 56);

                        Countfrequency[0] = seed_1;
                        Countfrequency[1] = seed_2;
                        Countfrequency[2] = seed_3;
                        Countfrequency[3] = seed_4;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3 + seed_4);
                    }
                    break;
                #endregion
                #region 17:00
                case 17:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(10, 21);
                        int seed_2 = random.Next(15, 26);
                        int seed_3 = random.Next(45, 56);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 18:00
                case 18:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(0, 11);
                        int seed_2 = random.Next(5, 16);
                        int seed_3 = random.Next(70, 81);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 19:00
                case 19:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(0, 11);
                        int seed_2 = random.Next(5, 16);
                        int seed_3 = random.Next(70, 81);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 20:00
                case 20:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(2, 7);
                        int seed_2 = random.Next(80, 91);

                        Countfrequency[2] = seed_1;
                        Countfrequency[3] = seed_2;
                        Countfrequency[4] = 100 - (seed_1 + seed_2);
                    }
                    break;
                #endregion
                #region 21:00
                case 21:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(2, 7);
                        int seed_2 = random.Next(4, 11);
                        int seed_3 = random.Next(75, 86);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 22:00
                case 22:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(20, 31);
                        int seed_2 = random.Next(20, 31);
                        int seed_3 = random.Next(35, 46);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = seed_3;
                        Countfrequency[4] = 100 - (seed_1 + seed_2 + seed_3);
                    }
                    break;
                #endregion
                #region 23:00
                case 23:
                    {
                        Random random = new Random(DateTime.Now.Millisecond);
                        int seed_1 = random.Next(30, 41);
                        int seed_2 = random.Next(18, 29);

                        Countfrequency[1] = seed_1;
                        Countfrequency[2] = seed_2;
                        Countfrequency[3] = 100 - (seed_1 + seed_2);
                    }
                    break;
                    #endregion
            }
        }

        /// <summary>
        /// 算出增值頻率
        /// </summary>
        public override void GetFrequencyIndex()
        {
            List<int> tempWeights = new List<int>();

            foreach (var Key in Countfrequency.Keys)
            {
                if (Countfrequency[Key] != 0 && tempWeights.Count < 100)
                {
                    for (int i = 0; i < Countfrequency[Key]; i++)
                    {
                        tempWeights.Add(Key);
                    }
                }
            }

            Random random = new Random(DateTime.Now.Millisecond);
            int index = random.Next(0, 100);

            frequencyindex = tempWeights[index];
        }

        /// <summary>清空所有value</summary>
        private void ClearCountfrequency()
        {
            for (int i = 0; i < Countfrequency.Count; i++)
            {
                Countfrequency[i] = 0;
            }
        }

        /// <summary>重置JP</summary>
        public override void ReseatInitPoint(int value)
        {
            InitPoint = value;
        }
    }
}
