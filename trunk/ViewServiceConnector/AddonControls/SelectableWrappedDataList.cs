///////////////////////////////////////////////////////////////////////////////
//File: SelectableWrappedDataList.cs
//
//Description: SelectableWrappedDataList, a MetaView list with selectable items
//  and containing an associated data field
//
//References required:
//  System.Drawing
//  Wrapper.cs (MetaViewWrapper interface definitions)
//  SelectableWrappedList.cs
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
using System.Drawing;

#if METAVIEW_PUBLIC_NS
using MetaViewWrappers;
#else
using MyClasses.MetaViewWrappers;
#endif

namespace MyClasses
{
    class SelectableWrappedDataList<T>: SelectableWrappedList, IEnumerable<T>
    {
        List<T> data = new List<T>();
        public SelectableWrappedDataList(IList mylist, int[] SelectChangeColumns)
            :base(mylist,SelectChangeColumns)
        {

        }
        public override IListRow AddRow()
        {
            throw new Exception("Cannot use unparameterized methods.");
        }
        public override IListRow InsertRow(int pos)
        {
            throw new Exception("Cannot use unparameterized methods.");
        }
        public override void Clear()
        {
            base.Clear();
            data.Clear();
        }
        public override void RemoveRow(int index)
        {
            base.RemoveRow(index);
            data.RemoveAt(index);
        }
        public IListRow AddRow(T pdata)
        {
            data.Add(pdata);
            return base.AddRow();
        }
        public IListRow InsertRow(int pos, T pdata)
        {
            data.Insert(pos, pdata);
            return base.InsertRow(pos);
        }
        public T GetData(int pos)
        {
            return data[pos];
        }
        public void SetData(int pos, T d)
        {
            data[pos] = d;
        }
        public int IndexOf(T d)
        {
            return data.IndexOf(d);
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #endregion
    }
}
