///////////////////////////////////////////////////////////////////////////////
//File: MyTimer.cs
//
//Description: A set of timer classes which are restricted to firing at most
//  once per frame.
//
//References required:
//  Decal.Interop.Core
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
using System.Windows.Forms;
using Decal.Adapter.Wrappers;
using Decal.Adapter;

namespace MyClasses
{
#if _MYCLASSES_PUBLIC
    public
#else
    internal
#endif
    class MyTimer: iMyTimer
    {
        //************Static members************
        static PluginHost pHost;
        static bool Shutdown = true;
        static ulong iFrameNum = 0;
        static ulong iLoginNum = 0;
        static List<MyTimer> timers = new List<MyTimer>();
        public delegate void delMyError(Exception exx);
        static delMyError iErrorFunc;

        public static void Init(PluginHost ppHost, delMyError pErrorFunc)
        {
            if (!Shutdown) return;
            Shutdown = false;
            pHost = ppHost;
            iErrorFunc = pErrorFunc;

            iFrameNum = 0;
            iLoginNum++;
            pHost.Underlying.Hooks.RenderPreUI += new Decal.Interop.Core.IACHooksEvents_RenderPreUIEventHandler(hooks_RenderPreUI);
            CoreManager.Current.PluginTermComplete += new EventHandler<EventArgs>(Current_PluginTermComplete);
        }

        static void Current_PluginTermComplete(object sender, EventArgs e)
        {
            Shutdown = true;

            List<MyTimer> displist = new List<MyTimer>();
            foreach (MyTimer t in timers)
                displist.Add(t);
            foreach (MyTimer t in displist)
                t.Dispose();
            timers.Clear();

            //pHost.Underlying.Hooks.RenderPreUI -= new Decal.Interop.Core.IACHooksEvents_RenderPreUIEventHandler(hooks_RenderPreUI);
            CoreManager.Current.PluginTermComplete -= new EventHandler<EventArgs>(Current_PluginTermComplete);

            pHost = null;
            iErrorFunc = null;
        }

        static void hooks_RenderPreUI()
        {
            iFrameNum++;
        }

        public static ulong CurrentFrame
        {
            get
            {
                return iFrameNum;
            }
        }

        public static ulong CurrentLogin
        {
            get
            {
                return iLoginNum;
            }
        }

        //************Nonstatic members************
        ulong LastFrame = 0;
        ulong LoginID = 0;
        bool disposed = false;

        Timer tim = new Timer();
        public event System.EventHandler Tick;

        public void Start()
        {
            //System.Windows.Forms.MessageBox.Show("Timer started.");
            tim.Start();
        }

        public void Stop()
        {
            //System.Windows.Forms.MessageBox.Show("Timer stopped.");
            tim.Stop();
        }

        public MyTimer()
        {
            if (Shutdown) throw new Exception("MyTimer class must be statically initialized before any timers can be created.");
            tim.Tick += new EventHandler(tim_Tick);
            LoginID = CurrentLogin;
            timers.Add(this);
        }

        void tim_Tick(object sender, EventArgs e)
        {
            if (Shutdown || (LoginID != CurrentLogin))
            {
                if (tim != null)
                {
                    tim.Stop();
                    tim.Dispose();
                    tim = null;
                }
                return;
            }

            if (LastFrame != iFrameNum)
            {
                LastFrame = iFrameNum;
                if (Tick != null)
                {
                    try
                    {
                        Tick(this, null);
                    }
                    catch (Exception exx)
                    {
                        if (iErrorFunc != null)
                            iErrorFunc(exx);
                    }
                    finally
                    {
                        GC.WaitForPendingFinalizers();
                    }
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return tim.Enabled;
            }
            set
            {
                tim.Enabled = value;
            }
        }

        public int Interval
        {
            get
            {
                return tim.Interval;
            }
            set
            {
                tim.Interval = value;
            }
        }
        
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            if (tim != null)
            {
                tim.Stop();
                tim.Dispose();
                tim = null;
            }

            if (timers.Contains(this))
                timers.Remove(this);
        }
    }

#if _MYCLASSES_PUBLIC
    public
#else
    internal
#endif
    static class SingleFireTimer
    {
        public delegate void delEmpty();
        static List<MyTimer> t = new List<MyTimer>();
        static List<delEmpty> callbacks = new List<delEmpty>();
        static List<ulong> callframes = new List<ulong>();
        public static void SetTimer(delEmpty callback, int time)
        {
            MyTimer ttt = new MyTimer();
            t.Add(ttt);
            callbacks.Add(callback);
            callframes.Add(MyTimer.CurrentFrame);

            ttt.Interval = time;
            ttt.Tick += new EventHandler(ttt_Tick);
            ttt.Start();
        }

        static MyTimer search;
        static void ttt_Tick(object sender, EventArgs e)
        {
            search = (MyTimer)sender;
            int ind = t.FindIndex(Pred);
            if (callframes[ind] == MyTimer.CurrentFrame) return;

            search.Stop();
            callbacks[ind].Invoke();

            t.RemoveAt(ind);
            callbacks.RemoveAt(ind);
            callframes.RemoveAt(ind);
        }
        static bool Pred(MyTimer q)
        {
            return (q == search);
        }
    }
    static class CallNextFrameIfAlready
    {
        public delegate void delEmpty();
        static List<delEmpty> callbacks = new List<delEmpty>();
        static List<ulong> cblastframe = new List<ulong>();
        static List<bool> needcb = new List<bool>();
        static List<string> keys = new List<string>();
        static bool going = false;
        public static void Call(delEmpty func, string key)
        {
            if (!keys.Contains(key))
            {
                callbacks.Add(func);
                cblastframe.Add(ulong.MinValue);
                needcb.Add(false);
                keys.Add(key);
            }
            int i = keys.IndexOf(key);
            if (cblastframe[i] < MyTimer.CurrentFrame)
            {
                cblastframe[i] = MyTimer.CurrentFrame;
                needcb[i] = false;
                func();
            }
            else
            {
                //We need a callback next frame
                if (!going)
                    SingleFireTimer.SetTimer(new SingleFireTimer.delEmpty(docallback), 1);
                needcb[i] = true;
            }
        }
        static void docallback()
        {
            //Hi, we just got notified of a new frame
            for (int i = needcb.Count - 1; i >= 0 ; --i)
            {
                if (needcb[i])
                {
                    Call(callbacks[i], keys[i]);
                    /*
                    if (!needcb[i])
                    {
                        callbacks.RemoveAt(i);
                        cblastframe.RemoveAt(i);
                        needcb.RemoveAt(i);
                        keys.RemoveAt(i);
                    }
                    */
                }
            }
        }
    }

}