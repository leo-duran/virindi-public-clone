///////////////////////////////////////////////////////////////////////////////
//File: PluginCore.cs
//
//Description: Core code for the Virindi Reporter plugin.
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

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirindiReporter
{
    internal class cClassGroup : IDisposable
    {
        bool disp = false;
        public void Dispose()
        {
            if (disp) return;
            disp = true;
            GC.SuppressFinalize(this);

            XPCounting.Dispose();

            XPCounting = null;
            Core = null;
            Host = null;
        }
        public cXPCounting XPCounting;
        public PluginHost Host;
        public CoreManager Core;
        public cClassGroup(PluginHost h, CoreManager c)
        {
            Host = h;
            Core = c;

            XPCounting = new cXPCounting(this);
        }
        ~cClassGroup()
        {
            Dispose();
        }
    }
    public partial class PluginCore : PluginBase
    {
        public string errorLogFile { get { return Path + "errorLog.txt"; } }
        void LogError(Exception exx)
        {
            errorLogging.LogError(errorLogFile, exx);
        }

        cClassGroup ClassGroup;

        protected override void Startup()
        {
            try
            {
                ClassGroup = new cClassGroup(Host,Core);
                InitView();

                MyClasses.MyTimer.Init(Host, LogError);
                Core.PluginInitComplete += new EventHandler<EventArgs>(Core_PluginInitComplete);
            }
            catch (Exception ex)
            {
                errorLogging.LogError(errorLogFile, ex);
            }
        }

        protected override void Shutdown()
        {
            try
            {
                if (statustimer != null)
                {
                    statustimer.Dispose();
                    statustimer = null;
                }

                Core.PluginInitComplete -= new EventHandler<EventArgs>(Core_PluginInitComplete);

                DestroyView();
                ClassGroup.Dispose();
                ClassGroup = null;
            }
            catch (Exception ex)
            {
                errorLogging.LogError(errorLogFile, ex);
            }
        }

        MyClasses.MyTimer statustimer = null;

        void Core_PluginInitComplete(object sender, EventArgs e)
        {
            try
            {
                MyClasses.VHUDs_Connector.Pulse();
                if (MyClasses.VHUDs_Connector.IsVHUDsPresent())
                {
                    statustimer = new MyClasses.MyTimer();
                    statustimer.Interval = 1000;
                    statustimer.Tick += new EventHandler(statustimer_Tick);
                    statustimer.Start();
                }
            }
            catch (Exception exx)
            {
                errorLogging.LogError(errorLogFile, exx);
            }
        }

        void statustimer_Tick(object sender, EventArgs e)
        {
            cXPCounting.sReportData srd = ClassGroup.XPCounting.GetXPRate();

            MyClasses.VHUDs_Connector.Status_UpdateEntry("VReporter", "XP Earned", string.Format("{0:N0}", srd.XP));

            MyClasses.VHUDs_Connector.Status_UpdateEntry("VReporter", "XP/Hour", string.Format("{0:N0} xp/h", srd.XPPerHour));

            string xp5min = "---";
            if (srd.b5mingood)
                xp5min = string.Format("{0:N0} xp/h", srd.XP5min);
            MyClasses.VHUDs_Connector.Status_UpdateEntry("VReporter", "XP/Hour (5min)", xp5min);

            string runtime = "";
            if (srd.Time.TotalDays >= 1)
            {
                runtime = string.Format("{0} days, {1:00}:{2:00}:{3:00}",
                    srd.Time.Days, srd.Time.Hours, srd.Time.Minutes, srd.Time.Seconds);
            }
            else
            {
                runtime = string.Format("{0:00}:{1:00}:{2:00}",
                    srd.Time.Hours, srd.Time.Minutes, srd.Time.Seconds);
            }
            MyClasses.VHUDs_Connector.Status_UpdateEntry("VReporter", "Runtime since XP reset", runtime);

            long thisxp, nextxp;
            thisxp = Core.CharacterFilter.TotalXP;
            nextxp = xpforlevel(Core.CharacterFilter.Level + 1);
            string leveleta = "";
            if (srd.XPPerHour > 0)
            {
                TimeSpan ts = TimeSpan.FromHours((nextxp - thisxp) / srd.XPPerHour);
                if (ts.TotalDays >= 1)
                {
                    leveleta = string.Format("{0} days, {1:00}:{2:00}:{3:00}",
                        ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
                }
                else
                {
                    leveleta = string.Format("{0:00}:{1:00}:{2:00}",
                        ts.Hours, ts.Minutes, ts.Seconds);
                }
            }
            else
            {
                leveleta = "INFINITY";
            }
            MyClasses.VHUDs_Connector.Status_UpdateEntry("VReporter", "Level ETA", leveleta);
        }
    }
}