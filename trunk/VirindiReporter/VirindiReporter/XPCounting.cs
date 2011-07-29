///////////////////////////////////////////////////////////////////////////////
//File: XPCounting.cs
//
//Description: XP routine helper class for the Virindi Reporter plugin.
//
//This file is Copyright (c) 2010-2011 VirindiPlugins
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace VirindiReporter
{
    internal class cXPCounting: IDisposable
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        cClassGroup C;
        public struct sReportData
        {
            public long XP;
            public double XPPerHour;
            public TimeSpan Time;
            public double XP5min;
            public bool b5mingood;

			public long XP_Luminance;
			public double XPPerHour_Luminance;
			public double XP5Min_Luminance;
        }
        DateTime starttime;
        long startxp;
		long startxp_luminance;
		long current_luminance = 0;
        bool started = false;

        public cXPCounting (cClassGroup csg)
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000 * 60;
            C = csg;
            C.Core.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
			C.Core.MessageProcessed += new EventHandler<Decal.Adapter.MessageProcessedEventArgs>(Core_MessageProcessed);

            for (int i = 0; i < 6; ++i)
                lastxp5[i] = -1;
        }

		void Core_MessageProcessed(object sender, Decal.Adapter.MessageProcessedEventArgs e)
		{
			//Need this to track luminance
			switch (e.Message.Type)
			{
				case 0x02CF: //Set character qword
					{
						int key = e.Message.Value<int>("key");
						if (key == 0x06)
						{
							current_luminance = e.Message.Value<long>("value");
						}
					}
					break;
				case 0xF7B0: //Ordered message
					switch (e.Message.Value<int>("event"))
					{
						case 0x0013: //Login character
							{
								Decal.Adapter.MessageStruct properties = e.Message.Struct("properties");
								if ((properties.Value<int>("flags") & 0x00000080) > 0)
								{
									short qwordcount = properties.Value<short>("qwordCount");
									for (short i = 0; i < qwordcount; ++i)
									{
										int key = properties.Struct("qwords").Struct(i).Value<int>("key");
										if (key == 0x06)
										{
											current_luminance = properties.Struct("qwords").Struct(i).Value<long>("value");
											break;
										}
									}
								}
							}
							break;
					}
					break;
			}
		}

        long[] lastxp5 = new long[6];
		long[] lastxp5_luminance = new long[6];
        int ptr5 = 0;

        void timer_Tick(object sender, EventArgs e)
        {
            ptr5++;
            if (ptr5 >= 6) ptr5 = 0;

            lastxp5[ptr5] = C.Core.CharacterFilter.TotalXP;
			lastxp5_luminance[ptr5] = current_luminance;
        }
        ~cXPCounting()
        {
			Dispose();
        }
        bool d = false;
        public void Dispose()
        {
            if (d) return;
            d = true;

            timer.Tick -= new EventHandler(timer_Tick);
            timer.Stop();

			C.Core.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
			C.Core.MessageProcessed -= new EventHandler<Decal.Adapter.MessageProcessedEventArgs>(Core_MessageProcessed);
        }

        void CharacterFilter_LoginComplete(object sender, EventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            started = true;
            starttime = DateTime.Now;
            startxp = C.Core.CharacterFilter.TotalXP;
			startxp_luminance = current_luminance;

            timer.Stop();
            timer.Start();

			for (int i = 0; i < 6; ++i)
			{
				lastxp5[i] = -1;
				lastxp5_luminance[i] = -1;
			}

            timer_Tick(null, null);
        }
        public sReportData GetXPRate()
        {
            sReportData srd = new sReportData();

            if (started)
            {
                srd.Time = DateTime.Now - starttime;
                srd.XP = C.Core.CharacterFilter.TotalXP - startxp;
                srd.XPPerHour = srd.XP / srd.Time.TotalHours;

				srd.XP_Luminance = current_luminance - startxp_luminance;
				srd.XPPerHour_Luminance = srd.XP_Luminance / srd.Time.TotalHours;

                int nptr5 = ptr5 + 1;
                if (nptr5 >= 6) nptr5 = 0;

                srd.XP5min = 12 * (lastxp5[ptr5] - lastxp5[nptr5]);
				srd.XP5Min_Luminance = 12 * (lastxp5_luminance[ptr5] - lastxp5_luminance[nptr5]);



                srd.b5mingood = (lastxp5[nptr5] != -1);
            }

            return (srd);
        }

    }
}
