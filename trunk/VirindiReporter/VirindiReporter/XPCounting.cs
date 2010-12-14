///////////////////////////////////////////////////////////////////////////////
//File: XPCounting.cs
//
//Description: XP routine helper class for the Virindi Reporter plugin.
//
//This file is Copyright (c) 2010 VirindiPlugins
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
        }
        DateTime starttime;
        long startxp;
        bool started = false;

        public cXPCounting (cClassGroup csg)
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000 * 60;
            C = csg;
            C.Core.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);

            for (int i = 0; i < 6; ++i)
                lastxp5[i] = -1;
        }

        long[] lastxp5 = new long[6];
        int ptr5 = 0;

        void timer_Tick(object sender, EventArgs e)
        {
            ptr5++;
            if (ptr5 >= 6) ptr5 = 0;

            lastxp5[ptr5] = C.Core.CharacterFilter.TotalXP;
        }
        ~cXPCounting()
        {
            //Dispose();
            //C.Core.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
        }
        bool d = false;
        public void Dispose()
        {
            if (d) return;
            d = true;

            timer.Tick -= new EventHandler(timer_Tick);
            timer.Stop();
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

            timer.Stop();
            timer.Start();

            for (int i = 0; i < 6; ++i)
                lastxp5[i] = -1;

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

                //int cptr5 = ptr5 - 1;
                //if (cptr5 < 0) cptr5 = 4;
                int nptr5 = ptr5 + 1;
                if (nptr5 >= 6) nptr5 = 0;

                srd.XP5min = 12 * (lastxp5[ptr5] - lastxp5[nptr5]);



                srd.b5mingood = (lastxp5[nptr5] != -1);

                //C.Host.Actions.AddChatText(lastxp5[0].ToString() + ", " + lastxp5[1].ToString() + ", " + lastxp5[2].ToString() + ", " + lastxp5[3].ToString() + ", " + lastxp5[4].ToString() + ", ", 0);
                //C.Host.Actions.AddChatText(ptr5.ToString(), 0);
            }

            return (srd);
        }

    }
}
