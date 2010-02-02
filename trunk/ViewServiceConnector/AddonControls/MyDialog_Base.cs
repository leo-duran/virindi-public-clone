///////////////////////////////////////////////////////////////////////////////
//File: MyDialog_Base.cs
//
//Description: Base class for a MetaViewWrapper-based dialogbox. Dialogs are
//  managed by the static MyDialogContainer class, which manages dialog
//  class lifetime.
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
using System.Runtime.InteropServices;

#if METAVIEW_PUBLIC_NS
using MetaViewWrappers;
#else
using MyClasses.MetaViewWrappers;
#endif

namespace MyClasses
{
    //Used to keep dialogs around until the user exits them
    static class MyDialogContainer
    {
        //Autoshutdown
        static bool Init = false;
        public static void Initialize()
        {
            if (Init) return;
            CoreManager.Current.PluginTermComplete += new EventHandler<EventArgs>(Current_PluginTermComplete);
        }

        static void Current_PluginTermComplete(object sender, EventArgs e)
        {
            CoreManager.Current.PluginTermComplete -= new EventHandler<EventArgs>(Current_PluginTermComplete);
            ClearAll();
            Init = false;
        }


        static List<object> ActiveDialogsAnon = new List<object>();
        static Dictionary<string, object> ActiveDialogs = new Dictionary<string, object>();

        static Dictionary<object, GCHandle> Handles = new Dictionary<object, GCHandle>();
        static Dictionary<object, delDispCall> DispCalls = new Dictionary<object, delDispCall>();
        public delegate void delDispCall();

        public static void ClearAll()
        {
            List<delDispCall> d = new List<delDispCall>();
            foreach (KeyValuePair<object, delDispCall> kp in DispCalls)
            {
                d.Add(kp.Value);
            }

            foreach (delDispCall cl in d)
            {
                if (cl != null)
                    cl();
            }
        }

        public static void AddDialog(string k, object obj, delDispCall disp)
        {
            if (k == "")
            {
                ActiveDialogsAnon.Add(obj);
                Handles[obj] = GCHandle.Alloc(obj, GCHandleType.Normal);
                DispCalls[obj] = disp;
            }
            else
            {
                if (ActiveDialogs.ContainsKey(k))
                {
                    ActiveDialogs.Remove(k);
                    Handles.Remove(obj);
                    DispCalls.Remove(obj);
                }
                
                ActiveDialogs[k] = obj;
                Handles[obj] = GCHandle.Alloc(obj, GCHandleType.Normal);
                DispCalls[obj] = disp;
            }
        }

        public static void RemoveDialog(string k, object obj)
        {
            if (ActiveDialogsAnon.Contains(obj))
                ActiveDialogsAnon.Remove(obj);

            if (ActiveDialogs.ContainsKey(k))
                ActiveDialogs.Remove(k);

            if (Handles.ContainsKey(obj))
                Handles.Remove(obj);

            if (DispCalls.ContainsKey(obj))
                DispCalls.Remove(obj);
        }

        public static bool DialogExists(string k)
        {
            if (k == "") return false;
            return ActiveDialogs.ContainsKey(k);
        }
    }

    abstract class MyDialog<T> : IDisposable
    {
        protected IView View;
        string iDialogKey;
        protected IView OriginatingView;

        protected MyDialog(string pDialogKey, IView pOrigin)
        {
            iDialogKey = pDialogKey;
            OriginatingView = pOrigin;
        }

        //Must be called by the inherited constructor
        protected bool ShowView(Decal.Adapter.Wrappers.PluginHost H)
        {
            if (MyDialogContainer.DialogExists(iDialogKey))
            {
                disp = true;
                GC.SuppressFinalize(this);
                return false;
            }

            MyDialogContainer.Initialize();
            MyDialogContainer.AddDialog(iDialogKey, this, Dispose);
            View = ViewSystemSelector.CreateViewXML(H, GetDialogXML());
            View.Visible = true;

            //Center view
            System.Drawing.Rectangle wnd = H.Actions.RegionWindow;
            int x = (wnd.Width - View.Size.Width) / 2;
            int y = (wnd.Height - View.Size.Height) / 2;
            View.Location = new System.Drawing.Point(x, y);

#if VVS_REFERENCED
            if (View.ViewType == ViewSystemSelector.eViewSystem.VirindiViewService)
                SetVVSProps();
#endif

            return true;
        }

#if VVS_REFERENCED
        void SetVVSProps()
        {
            VirindiViewService.HudView v = ((MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.View)View).Underlying;
            v.UserMinimizable = false;
            v.UserGhostable = false;
            v.UserAlphaChangeable = false;
            v.ShowInBar = false;
            v.ForcedZOrder = 10;
        }
#endif

        ~MyDialog()
        {
            Dispose();
        }

        bool disp = false;
        public virtual void Dispose()
        {
            if (disp) return;
            disp = true;

            GC.SuppressFinalize(this);
            MyDialogContainer.RemoveDialog(iDialogKey, this);
            OnDisposed();
            if (View != null)
            {
                View.Dispose();
                View = null;
            }
        }

        protected abstract string GetDialogXML();

        protected virtual void OnDisposed()
        {

        }

        protected void ReturnDialog(string ButtonRet, T ValueRet)
        {
            if (DialogReturned != null)
                DialogReturned(ButtonRet, ValueRet);

            Dispose();

            if (OriginatingView != null)
            {
                OriginatingView.Visible = true;
            }
        }

        public delegate void delDialogReturned(string PressedButton, T ResultValue);
        public event delDialogReturned DialogReturned;
    }
}