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
using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace AutoWireupExamplePlugin.Logic
{
    class ObjectTrackerObject
    {
        public int Icon;
        public string Name;
        public int Guid;
        public ObjectTrackerObject(int picon, string pname, int pguid) { Icon = picon; Name = pname; Guid = pguid; }
    }

    class ObjectTracker : IDisposable
    {
        public delegate void delNewObjectDetected(ObjectTrackerObject newobj);
        public event delNewObjectDetected NewObjectDetected;

        public delegate void delObjectDeleted(ObjectTrackerObject oldobj, int oldlistindex);
        public event delObjectDeleted ObjectDeleted;

        List<ObjectTrackerObject> objects = new List<ObjectTrackerObject>();

        public ObjectTracker()
        {
            CoreManager.Current.WorldFilter.CreateObject += new EventHandler<CreateObjectEventArgs>(WorldFilter_CreateObject);
            CoreManager.Current.WorldFilter.ReleaseObject += new EventHandler<ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);
        }

        public void Dispose()
        {
            CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<CreateObjectEventArgs>(WorldFilter_CreateObject);
            CoreManager.Current.WorldFilter.ReleaseObject -= new EventHandler<ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);
        }

        void WorldFilter_CreateObject(object sender, CreateObjectEventArgs e)
        {
            //In 2960 we need to do this to make sure the vendor object isn't initialized,
            //which can cause crashes at vendors.
            if (CoreManager.Current.WorldFilter[e.New.Id] == null) return;

            //Create the object and add it to our list
            ObjectTrackerObject myobject = new ObjectTrackerObject(e.New.Icon, e.New.Name, e.New.Id);
            objects.Add(myobject);

            //Notify anyone listening
            if (NewObjectDetected != null)
                NewObjectDetected(myobject);
        }

        void WorldFilter_ReleaseObject(object sender, ReleaseObjectEventArgs e)
        {
            //Find the object in our list.
            for (int i = objects.Count - 1; i >= 0; --i)
            {
                if (e.Released.Id == objects[i].Guid)
                {
                    ObjectTrackerObject oldobj = objects[i];
                    objects.RemoveAt(i);

                    if (ObjectDeleted != null)
                        ObjectDeleted(oldobj, i);

                    break;
                }
            }
        }

        public ObjectTrackerObject this[int ind]
        {
            get
            {
                return objects[ind];
            }
        }

        public int Count
        {
            get
            {
                return objects.Count;
            }
        }
    }
}
