///////////////////////////////////////////////////////////////////////////////
//File: Plugin.cs
//
//This file is part of the VirindiViews port of SkunkVision, originally written
//  by Gregory Kusnick. The C# frontend code in this port was written by Virindi.
//
//The original SkunkVision is available at: http://sourceforge.net/projects/skunkworks
//  and was released under the MIT license.
//
//This plugin port uses the MetaViewWrapper classes for the Virindi View Service.
//  These wrappers are available at:
//  http://www.virindi.net/repos/virindi_public/trunk/ViewServiceConnector/
//
//Description: This file contains the plugin's startup and shutdown, as well
//  as methods to load and save the plugin's settings.
//
//This file is Copyright (c) 2010 VirindiPlugins
//Original SkunkVision Copyright (c) 2006 Gregory Kusnick
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
using System.Runtime.InteropServices;
using System.Drawing;

namespace SkunkVision_CSharp
{
    public partial class Plugin: Decal.Adapter.PluginBase
    {
        #region Error Handler
        static Plugin Instance;
        internal static void OnError(Exception exx)
        {
            if (Instance != null)
                Instance.InstanceError(exx);
        }
        private void InstanceError(Exception exx)
        {
            Host.Actions.AddChatText("[SV] Error: " + exx.ToString(), 0, 1);
        }
        #endregion Error Handler

        RenderHookWrapper Render;

        protected override void Startup()
        {
            try
            {
                Instance = this;

                Render = new RenderHookWrapper(Host);

                ReadSettings();

                ViewInit();
            }
            catch (Exception exx)
            {
                System.Windows.Forms.MessageBox.Show(exx.ToString());
                OnError(exx);
            }
        }

        protected override void Shutdown()
        {
            try
            {
                ViewDestroy();

                Render.Dispose();

                Instance = null;
            }
            catch (Exception exx)
            {
                System.Windows.Forms.MessageBox.Show(exx.ToString());
                OnError(exx);
            }
        }

        static string PluginConfigPath
        {
            get
            {
                string pp = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                pp = System.IO.Path.Combine(pp, @"Decal Plugins\SkunkVision-VVS");

                //Make sure the path exists
                System.IO.Directory.CreateDirectory(pp);

                return pp;
            }
        }

        //Hax, but whatever
        #region Settings Load/Save

        void WriteColorToFile(System.IO.StreamWriter st, Color c)
        {
            st.WriteLine(c.A.ToString());
            st.WriteLine(c.R.ToString());
            st.WriteLine(c.G.ToString());
            st.WriteLine(c.B.ToString());
        }

        Color ReadColorFromFile(System.IO.StreamReader st)
        {
            byte a = byte.Parse(st.ReadLine());
            byte r = byte.Parse(st.ReadLine());
            byte g = byte.Parse(st.ReadLine());
            byte b = byte.Parse(st.ReadLine());
            return Color.FromArgb(a, r, g, b);
        }

        void ReadSettings()
        {
            try
            {
                string settingsfile = System.IO.Path.Combine(PluginConfigPath, "Settings.txt");
                if (!System.IO.File.Exists(settingsfile)) return;

                using (System.IO.StreamReader st = new System.IO.StreamReader(settingsfile))
                {
                    Render.LightEnabled = bool.Parse(st.ReadLine());
                    Render.SlopeEnabled = bool.Parse(st.ReadLine());
                    Render.WaterEnabled = bool.Parse(st.ReadLine());

                    Render.LightColor = ReadColorFromFile(st);
                    Render.SlopeColor = ReadColorFromFile(st);
                    Render.WaterColor = ReadColorFromFile(st);
                }
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        void SaveSettings()
        {
            try
            {
                string settingsfile = System.IO.Path.Combine(PluginConfigPath, "Settings.txt");

                using (System.IO.StreamWriter st = new System.IO.StreamWriter(settingsfile, false))
                {
                    st.WriteLine(Render.LightEnabled.ToString());
                    st.WriteLine(Render.SlopeEnabled.ToString());
                    st.WriteLine(Render.WaterEnabled.ToString());

                    WriteColorToFile(st, Render.LightColor);
                    WriteColorToFile(st, Render.SlopeColor);
                    WriteColorToFile(st, Render.WaterColor);
                }
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        #endregion Settings Load/Save
    }
}
