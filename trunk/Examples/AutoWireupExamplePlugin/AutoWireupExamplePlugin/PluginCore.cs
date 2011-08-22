///////////////////////////////////////////////////////////////////////////////
//This file is Copyright (c) 2011 VirindiPlugins
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

namespace AutoWireupExamplePlugin
{
    public class PluginCore : Decal.Adapter.PluginBase
    {
        internal static Decal.Adapter.Wrappers.PluginHost host = null;

        //View component instances
        internal static Views.MainView mainview = null;

        //Logic component instances
        internal static Logic.ObjectTracker objecttracker = null;

        protected override void Startup()
        {
            host = Host;

            //Start plugin logic components
            objecttracker = new Logic.ObjectTracker();

            //Start view components
            mainview = new Views.MainView();
        }

        protected override void Shutdown()
        {
            //Stop view components
            mainview.Dispose();

            //Stop plugin logic components
            objecttracker.Dispose();

            host = null;
        }

        internal static void WriteToChat(string s)
        {
            host.Actions.AddChatText("[AWEP] " + s, 0);
        }
    }
}
