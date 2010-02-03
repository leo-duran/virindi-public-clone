///////////////////////////////////////////////////////////////////////////////
//File: RenderHookWrapper.cs
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
//Description: RenderHookWrapper is a wrapper for the SVRenderHook COM control.
//  This wrapper manages COM control lifetime, as well as making sure it is
//  only enabled if one of the rendering components is enabled.
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
    class RenderHookWrapper: IDisposable
    {
        RenderHookLib.SVRenderHook iRenderHook;

        #region ctor / dtor

        public RenderHookWrapper(Decal.Adapter.Wrappers.PluginHost Host)
        {
            iRenderHook = new RenderHookLib.SVRenderHookClass();

            //Init renderhook
            object netsvc = Host.Decal.GetObject("services\\DecalNet.NetService", "{AA405035-E001-4CC3-B43A-156206843D64}");
            iRenderHook.Init(netsvc);
            Marshal.ReleaseComObject(netsvc);
            netsvc = null;

            iRenderHook.fEnabled = false;
            LightColor = iLightColor;
            SlopeColor = iSlopeColor;
            WaterColor = iWaterColor;
        }

        bool disposed = false;
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            GC.SuppressFinalize(this);

            iRenderHook.fEnabled = false;
            iRenderHook.Finalize();
            Marshal.ReleaseComObject(iRenderHook);
            iRenderHook = null;
        }

        ~RenderHookWrapper()
        {
            Dispose();
        }

        #endregion ctor / dtor

        bool iEnabled = false;
        void ChangeEnabled()
        {
            //Only enable the whole thing when one of the
            //individual types is running
            bool newval = iLightEnabled || iSlopeEnabled || iWaterEnabled;
            if (newval != iEnabled)
            {
                iRenderHook.fEnabled = newval;
                iEnabled = newval;
            }
        }

        bool iLightEnabled = false;
        public bool LightEnabled
        {
            get
            {
                return iLightEnabled;
            }
            set
            {
                iLightEnabled = value;
                ChangeEnabled();
                iRenderHook.fLight = value;
            }
        }

        bool iSlopeEnabled = false;
        public bool SlopeEnabled
        {
            get
            {
                return iSlopeEnabled;
            }
            set
            {
                iSlopeEnabled = value;
                ChangeEnabled();
                iRenderHook.fSlope = value;
            }
        }

        bool iWaterEnabled = false;
        public bool WaterEnabled
        {
            get
            {
                return iWaterEnabled;
            }
            set
            {
                iWaterEnabled = value;
                ChangeEnabled();
                iRenderHook.fWater = value;
            }
        }

        Color iLightColor = Color.White;
        public Color LightColor
        {
            get
            {
                return iLightColor;
            }
            set
            {
                iLightColor = value;
                iRenderHook.colorLight = value.ToArgb();
            }
        }

        Color iSlopeColor = Color.Red;
        public Color SlopeColor
        {
            get
            {
                return iSlopeColor;
            }
            set
            {
                iSlopeColor = value;
                iRenderHook.colorSlope = value.ToArgb();
            }
        }

        Color iWaterColor = Color.Red;
        public Color WaterColor
        {
            get
            {
                return iWaterColor;
            }
            set
            {
                iWaterColor = value;
                iRenderHook.colorWater = value.ToArgb();
            }
        }
    }
}
