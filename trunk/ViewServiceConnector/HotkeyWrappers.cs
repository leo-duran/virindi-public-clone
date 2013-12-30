///////////////////////////////////////////////////////////////////////////////
//File: HotkeyWrappers.cs
//
//Description: Abstraction for DHS/VHS hotkey operation.
//
//References required:
//  VirindiHotkeySystem
//  VHS_Connector.cs
//
//This file is Copyright (c) 2013 VirindiPlugins
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

namespace MyClasses
{
    delegate bool delHotkeyAction();

    static class HotkeyWrapperManager
    {
        static bool Started = false;
        static string PluginName = null;
        static bool UsingDHS = false;
        static bool UsingVHS = false;
        static Dictionary<string, delHotkeyAction> NameHotkeyActions = new Dictionary<string, delHotkeyAction>();
        static Dictionary<object, delHotkeyAction> SenderHotkeyActions = new Dictionary<object, delHotkeyAction>();

        #region Startup

        public static void Startup(string iPluginName)
        {
            if (Started) return;
            Started = true;

            PluginName = iPluginName;
            UsingDHS = (Decal.Adapter.CoreManager.Current.HotkeySystem != null);
            UsingVHS = VHS_Connector.IsVHSPresent();
            NameHotkeyActions.Clear();
            SenderHotkeyActions.Clear();

            if (UsingDHS) Startup_DHS();
            if (UsingVHS) Startup_VHS();
        }

        static void Startup_DHS()
        {
            Decal.Adapter.CoreManager.Current.HotkeySystem.Hotkey += new EventHandler<Decal.Adapter.Wrappers.HotkeyEventArgs>(HotkeySystem_Hotkey);
        }

        static void Startup_VHS()
        {
            
        }

        #endregion Startup

        #region Shutdown

        public static void Shutdown()
        {
            if (!Started) return;
            Started = false;

            if (UsingDHS) Shutdown_DHS();
            if (UsingVHS) Shutdown_VHS();
        }

        static void Shutdown_DHS()
        {
            Decal.Adapter.CoreManager.Current.HotkeySystem.Hotkey -= new EventHandler<Decal.Adapter.Wrappers.HotkeyEventArgs>(HotkeySystem_Hotkey);
        }

        static void Shutdown_VHS()
        {

        }

        #endregion Shutdown

        //DHS global hotkey fired event
        static void HotkeySystem_Hotkey(object sender, Decal.Adapter.Wrappers.HotkeyEventArgs e)
        {
            if (!Started) return;

            string prefix = PluginName + ": ";
            string title = e.Title;
            if (title.Length <= prefix.Length) return;
            if (!title.StartsWith(prefix)) return;
            title = title.Substring(prefix.Length);
            if (NameHotkeyActions.ContainsKey(title) && (NameHotkeyActions[title] != null))
                e.Eat = NameHotkeyActions[title]();
        }

        //VHS individual key fired event
        static void ii_Fired2(object sender, VirindiHotkeySystem.VHotkeyInfo.cEatableFiredEventArgs e)
        {
            if (!Started) return;

            if (!SenderHotkeyActions.ContainsKey(sender)) return;
            if (SenderHotkeyActions[sender] == null) return;

            e.Eat = SenderHotkeyActions[sender]();
        }

        public static void AddHotkey(delHotkeyAction paction, string ptitle, string pdescription, int pvirtualKey, bool paltState, bool pcontrolState, bool pshiftState)
        {
            //Virtual key list:
            //http://web.archive.org/web/20100610104546/http://api.farmanager.com/en/winapi/virtualkeycodes.html
            //http://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx

            if (UsingDHS) AddHotkey_DHS(paction, ptitle, pdescription, pvirtualKey, paltState, pcontrolState, pshiftState);
            if (UsingVHS) AddHotkey_VHS(paction, ptitle, pdescription, pvirtualKey, paltState, pcontrolState, pshiftState);
        }

        static void AddHotkey_DHS(delHotkeyAction paction, string ptitle, string pdescription, int pvirtualKey, bool paltState, bool pcontrolState, bool pshiftState)
        {
            string decaltitle = PluginName + ": " + ptitle;
            NameHotkeyActions[ptitle] = paction;
            if (!Decal.Adapter.CoreManager.Current.HotkeySystem.Exists(decaltitle))
            {
                Decal.Adapter.CoreManager.Current.HotkeySystem.AddHotkey(PluginName, decaltitle, pdescription, pvirtualKey, paltState, pcontrolState, pshiftState);
            }
        }

        static void AddHotkey_VHS(delHotkeyAction paction, string ptitle, string pdescription, int pvirtualKey, bool paltState, bool pcontrolState, bool pshiftState)
        {
            VirindiHotkeySystem.VHotkeyInfo ii = new VirindiHotkeySystem.VHotkeyInfo(ptitle, pdescription, pvirtualKey, paltState, pcontrolState, pshiftState);
            VirindiHotkeySystem.VHotkeySystem.InstanceReal.AddHotkey(ii);
            ii.Fired2 += new EventHandler<VirindiHotkeySystem.VHotkeyInfo.cEatableFiredEventArgs>(ii_Fired2);
            SenderHotkeyActions[ii] = paction;
        }

    }
}
