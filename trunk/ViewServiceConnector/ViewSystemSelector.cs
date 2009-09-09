///////////////////////////////////////////////////////////////////////////////
//File: ViewSystemSelector.cs
//
//Description: Contains the MyClasses.MetaViewWrappers.ViewSystemSelector class,
//  which is used to determine whether the Virindi View Service is enabled.
//  As with all the VVS wrappers, the VVS_REFERENCED compilation symbol must be
//  defined for the VVS code to be compiled. Otherwise, only Decal views are used.
//
//References required:
//  VirindiViewService (if VVS_REFERENCED is defined)
//  Decal.Adapter
//  Decal.Interop.Core
//
//This file is Copyright (c) 2009 VirindiPlugins
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

namespace MyClasses.MetaViewWrappers
{
    internal static class ViewSystemSelector
    {
        public enum eViewSystem
        {
            DecalInject,
            VirindiViewService,
        }
        public static bool IsPresent(Decal.Adapter.Wrappers.PluginHost pHost, eViewSystem VSystem)
        {
            switch (VSystem)
            {
                case eViewSystem.DecalInject:
                    return true;
                case eViewSystem.VirindiViewService:
                    return VirindiViewsPresent(pHost);
                default:
                    return false;
            }
        }
        static bool VirindiViewsPresent(Decal.Adapter.Wrappers.PluginHost pHost)
        {
#if VVS_REFERENCED
            Guid g = new Guid("DBAC9286-B38D-4570-961F-D4D9349AE3D4");

            Decal.Interop.Core.DecalEnum esv = null;
            try
            {
                esv = pHost.Underlying.Decal.get_Configuration("Services", ref g);
                return esv.Enabled;
            }
            catch { }
            finally
            {
                if (esv != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(esv);
            }
            return false;
#else
            return false;
#endif
        }
        public static bool VirindiViewsPresent(Decal.Adapter.Wrappers.PluginHost pHost, Version minver)
        {
#if VVS_REFERENCED
            Guid g = new Guid("DBAC9286-B38D-4570-961F-D4D9349AE3D4");

            Decal.Interop.Core.DecalEnum esv = null;
            try
            {
                esv = pHost.Underlying.Decal.get_Configuration("Services", ref g);
                if (!esv.Enabled) return false;
                Version curver = new Version(esv.Version);
                return (curver >= minver);
            }
            catch { }
            finally
            {
                if (esv != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(esv);
            }
            return false;
#else
            return false;
#endif
        }
        public static IView CreateViewResource(Decal.Adapter.Wrappers.PluginHost pHost, string pXMLResource)
        {
#if VVS_REFERENCED
            if (IsPresent(pHost, eViewSystem.VirindiViewService))
                return CreateViewResource(pHost, pXMLResource, eViewSystem.VirindiViewService);
            else
#endif
                return CreateViewResource(pHost, pXMLResource, eViewSystem.DecalInject);
        }
        public static IView CreateViewResource(Decal.Adapter.Wrappers.PluginHost pHost, string pXMLResource, eViewSystem VSystem)
        {
            if (!IsPresent(pHost, VSystem)) return null;
            switch (VSystem)
            {
                case eViewSystem.DecalInject:
                    return CreateDecalViewResource(pHost, pXMLResource);
                case eViewSystem.VirindiViewService:
#if VVS_REFERENCED
                    return CreateMyHudViewResource(pHost, pXMLResource);
#else
                    break;
#endif
            }
            return null;
        }
        static IView CreateDecalViewResource(Decal.Adapter.Wrappers.PluginHost pHost, string pXMLResource)
        {
            IView ret = new MyClasses.MetaViewWrappers.DecalControls.View();
            ret.Initialize(pHost, pXMLResource);
            return ret;
        }

#if VVS_REFERENCED
        static IView CreateMyHudViewResource(Decal.Adapter.Wrappers.PluginHost pHost, string pXMLResource)
        {
            IView ret = new MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.View();
            ret.Initialize(pHost, pXMLResource);
            return ret;
        }
#endif

        public static bool AnySystemHasChatOpen(Decal.Adapter.Wrappers.PluginHost pHost)
        {
            if (IsPresent(pHost, eViewSystem.VirindiViewService))
                if (HasChatOpen_VirindiViews()) return true;
            if (pHost.Actions.ChatState) return true;
            return false;
        }
        
        static bool HasChatOpen_VirindiViews()
        {
#if VVS_REFERENCED
            if (VirindiViewService.HudView.FocusControl != null)
            {
                if (VirindiViewService.HudView.FocusControl.GetType() == typeof(VirindiViewService.Controls.HudTextBox))
                    return true;
            }
            return false;
#else
            return false;
#endif
        }
        
    }
}
