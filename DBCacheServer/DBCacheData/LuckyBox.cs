using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class LuckyBox
    {
        List<string> Box = new List<string>();

        RedEnvelopeRate mRedEnvelopeRate = new RedEnvelopeRate();

        Random random = new Random((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

        public LuckyBox(RedEnvelopeRate redEnvelopeRate)
        {
            mRedEnvelopeRate = redEnvelopeRate;

            int Temp = 0;

            double odds = 10.0;

            Box.Clear();

            Temp = (int)(mRedEnvelopeRate.Bonus1Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus1");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus2Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus2");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus3Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus3");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus4Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus4");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus5Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus5");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus6Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus6");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus7Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus7");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus8Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus8");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus9Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus9");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus9Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus9");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus10Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus10");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus11Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus11");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus12Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus12");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus13Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus13");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus14Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus14");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus15Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus15");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus16Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus16");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus17Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus17");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus18Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus18");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus19Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus19");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus20Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus20");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus21Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus21");
            }

            Temp = (int)(mRedEnvelopeRate.Bonus22Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Bonus22");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket1Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket1");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket2Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket2");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket3Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket3");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket4Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket4");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket5Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket5");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket6Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket6");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket7Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket7");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket8Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket8");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket9Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket9");
            }

            Temp = (int)(mRedEnvelopeRate.Ticket10Rate * odds);

            for (int i = 0; i < Temp; i++)
            {
                Box.Add("Ticket10");
            }

            for (int i = 0; i < 1000; i++)
            {
                int cnt1 = random.Next(0, Box.Count);

                int cnt2 = random.Next(0, Box.Count);

                string temp = Box[cnt1];

                Box[cnt1] = Box[cnt2];

                Box[cnt2] = temp;
            }
        }

        public string GetBoxBall()
        {
            int cnt1 = random.Next(0, Box.Count);

            return Box[cnt1];
        }
    }
}
