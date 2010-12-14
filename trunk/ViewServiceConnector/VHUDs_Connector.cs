///////////////////////////////////////////////////////////////////////////////
//File: VHUDs_Connector.cs
//
//Description: A static class to aid in interfacing with the VHUDs plugin, eg.
//  to add status information to the status HUD. To use, call either SchedulePulse()
//  at plugin startup or Pulse() at Core.PluginInitComplete.
//
//References required:
//  VirindiHUDs
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
using Decal.Adapter;
using Decal.Adapter.Wrappers;
using System.Reflection;

namespace MyClasses
{
    static class VHUDs_Connector
    {
        static bool cached = false;
        static bool ispresent = false;
        public static void Pulse()
        {
            ispresent = false;
            cached = false;
        }

        static bool psched = false;
        public static void SchedulePulse()
        {
            if (psched) return;
            psched = true;

            Decal.Adapter.CoreManager.Current.PluginInitComplete += new EventHandler<EventArgs>(Current_PluginInitComplete);
        }

        static void Current_PluginInitComplete(object sender, EventArgs e)
        {
            if (psched) Decal.Adapter.CoreManager.Current.PluginInitComplete -= new EventHandler<EventArgs>(Current_PluginInitComplete);
            Pulse();
        }

        public static bool IsVHUDsPresent()
        {
            if (cached) return ispresent;

            try
            {
                //See if VHUDs assembly is loaded
                System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
                //bool l = false;
                foreach (System.Reflection.Assembly a in asms)
                {
                    AssemblyName nmm = a.GetName();
                    if ((nmm.Name == "VirindiHUDs") && (nmm.Version >= new System.Version("1.0.0.5")))
                    {
                        cached = true;
                        ispresent = true;
                        return true;
                    }
                }
            }
            catch
            {

            }

            cached = true;
            ispresent = false;
            return false;
        }

        public static void Status_UpdateEntry(string pPluginName, string pEntryName, string pValue)
        {
            if (!IsVHUDsPresent()) return;
            
            Curtain_Status_UpdateEntry(pPluginName, pEntryName, pValue);
        }

        public static void Status_UpdateEntry(string pPluginName, string pEntryName, string pValue, System.Drawing.Color pColor)
        {
            if (!IsVHUDsPresent()) return;

            Curtain_Status_UpdateEntry(pPluginName, pEntryName, pValue, pColor);
        }

        static void Curtain_Status_UpdateEntry(string pPluginName, string pEntryName, string pValue)
        {
            VirindiHUDs.UIs.StatusModel.UpdateEntry(pPluginName, pEntryName, pValue);
        }

        static void Curtain_Status_UpdateEntry(string pPluginName, string pEntryName, string pValue, System.Drawing.Color pColor)
        {
            VirindiHUDs.UIs.StatusModel.UpdateEntry(pPluginName, pEntryName, pValue, pColor);
        }
    }
}