///////////////////////////////////////////////////////////////////////////////
//File: MainView.cs
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
//Description: This file contains the view rendering portions of the plugin.
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

namespace SkunkVision_CSharp
{
    public partial class Plugin
    {
        #region Auto-generated view code
        MyClasses.MetaViewWrappers.IView View;

        MyClasses.MetaViewWrappers.ICheckBox chkSlope;
        MyClasses.MetaViewWrappers.ICheckBox chkWater;
        MyClasses.MetaViewWrappers.ICheckBox chkLight;

        MyClasses.MetaViewWrappers.IButton bSetSlope;
        MyClasses.MetaViewWrappers.IButton bSetWater;
        MyClasses.MetaViewWrappers.IButton bSetLight;

        void ViewInit()
        {
            //Create view here
            View = MyClasses.MetaViewWrappers.ViewSystemSelector.CreateViewResource(Host, "SkunkVision_CSharp.MainView.xml");
            chkSlope = (MyClasses.MetaViewWrappers.ICheckBox)View["chkSlope"];
            chkWater = (MyClasses.MetaViewWrappers.ICheckBox)View["chkWater"];
            chkLight = (MyClasses.MetaViewWrappers.ICheckBox)View["chkLight"];

            chkSlope.Checked = Render.SlopeEnabled;
            chkWater.Checked = Render.WaterEnabled;
            chkLight.Checked = Render.LightEnabled;

            chkSlope.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkSlope_Change);
            chkWater.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkWater_Change);
            chkLight.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(chkLight_Change);

            bSetSlope = (MyClasses.MetaViewWrappers.IButton)View["bSetSlope"];
            bSetWater = (MyClasses.MetaViewWrappers.IButton)View["bSetWater"];
            bSetLight = (MyClasses.MetaViewWrappers.IButton)View["bSetLight"];

            bSetSlope.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(bSetSlope_Click);
            bSetWater.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(bSetWater_Click);
            bSetLight.Click += new EventHandler<MyClasses.MetaViewWrappers.MVControlEventArgs>(bSetLight_Click);
        }

        void ViewDestroy()
        {
            chkSlope = null;
            chkWater = null;
            chkLight = null;

            bSetSlope = null;
            bSetWater = null;
            bSetLight = null;

            View.Dispose();
        }
        #endregion Auto-generated view code


        void chkLight_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                Render.LightEnabled = e.Checked;
                SaveSettings();
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        void chkWater_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                Render.WaterEnabled = e.Checked;
                SaveSettings();
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        void chkSlope_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
        {
            try
            {
                Render.SlopeEnabled = e.Checked;
                SaveSettings();
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        void bSetLight_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {
                MyClasses.MyDialog_ColorQuery dlgLightColor = new MyClasses.MyDialog_ColorQuery(Host, new string[] { "Cancel", "Set" }, "SkunkVision: Set Dungeon Light Color", "Select new color for dungeon light:", 0, Render.LightColor, "SVdlg", View);
                dlgLightColor.DialogReturned += new MyClasses.MyDialog<System.Drawing.Color>.delDialogReturned(dlgLightColor_DialogReturned);
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        void dlgLightColor_DialogReturned(string PressedButton, System.Drawing.Color ResultValue)
        {
            try
            {
                if (PressedButton == "Set")
                {
                    Render.LightColor = ResultValue;
                    SaveSettings();
                }
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        void bSetWater_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {
                MyClasses.MyDialog_ColorQuery dlgWaterColor = new MyClasses.MyDialog_ColorQuery(Host, new string[] { "Cancel", "Set" }, "SkunkVision: Set Impassable Water Color", "Select new color for impassable water:", 0, Render.WaterColor, "SVdlg", View);
                dlgWaterColor.DialogReturned += new MyClasses.MyDialog<System.Drawing.Color>.delDialogReturned(dlgWaterColor_DialogReturned);
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        void dlgWaterColor_DialogReturned(string PressedButton, System.Drawing.Color ResultValue)
        {
            try
            {
                if (PressedButton == "Set")
                {
                    Render.WaterColor = ResultValue;
                    SaveSettings();
                }
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        void bSetSlope_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
        {
            try
            {
                MyClasses.MyDialog_ColorQuery dlgSlopeColor = new MyClasses.MyDialog_ColorQuery(Host, new string[] { "Cancel", "Set" }, "SkunkVision: Set Impassable Slope Color", "Select new color for impassable slopes:", 0, Render.SlopeColor, "SVdlg", View);
                dlgSlopeColor.DialogReturned += new MyClasses.MyDialog<System.Drawing.Color>.delDialogReturned(dlgSlopeColor_DialogReturned);
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }

        void dlgSlopeColor_DialogReturned(string PressedButton, System.Drawing.Color ResultValue)
        {
            try
            {
                if (PressedButton == "Set")
                {
                    Render.SlopeColor = ResultValue;
                    SaveSettings();
                }
            }
            catch (Exception exx)
            {
                OnError(exx);
            }
        }
    }
}
