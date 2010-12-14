///////////////////////////////////////////////////////////////////////////////
//File: MainView.cs
//
//Description: View handler code for the Virindi Reporter plugin.
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
    public partial class PluginCore
    {
        MyClasses.MetaViewWrappers.IView View;
        MyClasses.MetaViewWrappers.IButton cmdReset;
        MyClasses.MetaViewWrappers.IButton cmdReport;
        MyClasses.MetaViewWrappers.IButton cmdReportA;
        MyClasses.MetaViewWrappers.IButton cmdReportF;
        void InitView()
        {
            View = MyClasses.MetaViewWrappers.ViewSystemSelector.CreateViewResource(Host, "VirindiReporter.ViewXML.mainView.xml");
            cmdReset = (MyClasses.MetaViewWrappers.IButton)View["cmdReset"];
            cmdReport = (MyClasses.MetaViewWrappers.IButton)View["cmdReport"];
            cmdReportA = (MyClasses.MetaViewWrappers.IButton)View["cmdReportA"];
            cmdReportF = (MyClasses.MetaViewWrappers.IButton)View["cmdReportF"];
            cmdReset.Hit += new EventHandler(cmdReset_Hit);
            cmdReport.Hit += new EventHandler(cmdReport_Hit);
            cmdReportA.Hit += new EventHandler(cmdReportA_Hit);
            cmdReportF.Hit += new EventHandler(cmdReportF_Hit);
        }

        void DestroyView()
        {
            cmdReset.Hit -= new EventHandler(cmdReset_Hit);
            cmdReport.Hit -= new EventHandler(cmdReport_Hit);
            cmdReportA.Hit -= new EventHandler(cmdReportA_Hit);
            cmdReportF.Hit -= new EventHandler(cmdReportF_Hit);
            cmdReset = null;
            cmdReport = null;
            cmdReportA = null;
            cmdReportF = null;
            View.Dispose();
        }

        void cmdReset_Hit(object sender, EventArgs e)
        {
            try
            {
                ClassGroup.XPCounting.Reset();
            }
            catch (Exception ex)
            {
                errorLogging.LogError(errorLogFile, ex);
            }
        }

        public static long xpforlevel(int level)
        {
            return (long)Math.Ceiling((Math.Pow((double)level + 5, 5) - Math.Pow(6, 5)) / 9);
        }
        /*
        [ControlReference("cmdETA")]
        private ButtonWrapper cmdETA;

        [ControlEvent("cmdETA", "Click")]
        private void cmdETA_onClick(object sender, ControlEventArgs args)
        {
            cXPCounting.sReportData srd = ClassGroup.XPCounting.GetXPRate();
            StringBuilder sb = new StringBuilder();
            sb.Append("[VR] You need ");
            long thisxp, nextxp;
            thisxp = Core.CharacterFilter.TotalXP;
            nextxp = xpforlevel(Core.CharacterFilter.Level + 1);
            sb.AppendFormat("{0:N0}", nextxp - thisxp);
            sb.Append(" XP to reach level ");
            sb.Append(Core.CharacterFilter.Level + 1);
            sb.Append(", ETA ");

            if (srd.XPPerHour > 0)
            {
                TimeSpan ts = TimeSpan.FromHours((nextxp - thisxp) / srd.XPPerHour);
                if (ts.Days > 0)
                {
                    sb.AppendFormat("{0} days, {1:00}:{2:00}:{3:00}",
                        ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
                }
                else
                {
                    sb.AppendFormat("{0:00}:{1:00}:{2:00}",
                        ts.Hours, ts.Minutes, ts.Seconds);
                }
            }
            else
            {
                sb.Append("INFINITY");
            }
            Host.Actions.AddChatText(sb.ToString(), 4, 1);
        }
        */

        string GetReport()
        {
            cXPCounting.sReportData srd = ClassGroup.XPCounting.GetXPRate();
            StringBuilder sb = new StringBuilder();
            sb.Append("You've made ");
            sb.Append(string.Format("{0:N0}", srd.XP));
            sb.Append(" XP in ");

            if (srd.Time.TotalDays >= 1)
            {
                sb.AppendFormat("{0} days, {1:00}:{2:00}:{3:00}",
                    srd.Time.Days, srd.Time.Hours, srd.Time.Minutes, srd.Time.Seconds);
            }
            else
            {
                sb.AppendFormat("{0:00}:{1:00}:{2:00}",
                    srd.Time.Hours, srd.Time.Minutes, srd.Time.Seconds);
            }

            sb.Append(" for ");
            sb.AppendFormat("{0:N0}", srd.XPPerHour);
            sb.Append(" XP/hour");

            if (srd.b5mingood)
            {
                sb.Append(" (5min ");
                sb.AppendFormat("{0:N0}", srd.XP5min);
                sb.Append(" XP/hour)");
            }

            sb.Append(". ");
            /////////////////

            long thisxp, nextxp;
            thisxp = Core.CharacterFilter.TotalXP;
            nextxp = xpforlevel(Core.CharacterFilter.Level + 1);
            sb.AppendFormat("{0:N0}", nextxp - thisxp);
            sb.Append(" XP to reach level ");
            sb.Append(Core.CharacterFilter.Level + 1);
            sb.Append(", ETA ");

            if (srd.XPPerHour > 0)
            {
                TimeSpan ts = TimeSpan.FromHours((nextxp - thisxp) / srd.XPPerHour);
                if (ts.TotalDays >= 1)
                {
                    sb.AppendFormat("{0} days, {1:00}:{2:00}:{3:00}",
                        ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
                }
                else
                {
                    sb.AppendFormat("{0:00}:{1:00}:{2:00}",
                        ts.Hours, ts.Minutes, ts.Seconds);
                }
            }
            else
            {
                sb.Append("INFINITY");
            }
            sb.Append(".");

            /////////////////

            return sb.ToString();
        }

        void cmdReportA_Hit(object sender, EventArgs e)
        {
            Host.Actions.InvokeChatParser("/a " + GetReport());
        }

        void cmdReportF_Hit(object sender, EventArgs e)
        {
            Host.Actions.InvokeChatParser("/f " + GetReport());
        }

        void cmdReport_Hit(object sender, EventArgs e)
        {
            try
            {
                Host.Actions.AddChatText("[VR] " + GetReport(), 4, 1);
            }
            catch (Exception ex)
            {
                errorLogging.LogError(errorLogFile, ex);
            }
        }
    }
}