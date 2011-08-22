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
using MyClasses.MetaViewWrappers;

namespace AutoWireupExamplePlugin.Views
{
    [MVView("AutoWireupExamplePlugin.Views.MainView.xml")]
    class MainView : IDisposable
    {
        public MainView()
        {
            //Display the view
            MVWireupHelper.WireupStart(this, PluginCore.host);

            //Logic events
            PluginCore.objecttracker.NewObjectDetected += new Logic.ObjectTracker.delNewObjectDetected(objecttracker_NewObjectDetected);
            PluginCore.objecttracker.ObjectDeleted += new Logic.ObjectTracker.delObjectDeleted(objecttracker_ObjectDeleted);
        }

        public void Dispose()
        {
            //Logic events
            PluginCore.objecttracker.NewObjectDetected -= new Logic.ObjectTracker.delNewObjectDetected(objecttracker_NewObjectDetected);
            PluginCore.objecttracker.ObjectDeleted -= new Logic.ObjectTracker.delObjectDeleted(objecttracker_ObjectDeleted);

            //Remove the view
            MVWireupHelper.WireupEnd(this);
        }

        #region Control instances

        [MVControlReference("lstObjects")]
        public IList lstObjects;

        #endregion Control instances

        #region Control events

        [MVControlEvent("lstObjects", "Selected")]
        private void lstObjects_Selected(object sender, MyClasses.MetaViewWrappers.MVListSelectEventArgs e)
        {
            //Set this object as the current selection in the game.
            int objectguid = PluginCore.objecttracker[e.Row].Guid;

            PluginCore.host.Actions.SelectItem(objectguid);
        }

        #endregion Control events

        #region Logic events

        void objecttracker_NewObjectDetected(Logic.ObjectTrackerObject newobj)
        {
            IListRow newrow = lstObjects.AddRow();
            newrow[0][1] = newobj.Icon;
            newrow[1][0] = newobj.Name;
        }

        void objecttracker_ObjectDeleted(Logic.ObjectTrackerObject oldobj, int oldlistindex)
        {
            lstObjects.RemoveRow(oldlistindex);

            //Tell the user
            PluginCore.WriteToChat("Object deleted: " + oldobj.Name);
        }

        #endregion Logic events
    }
}
