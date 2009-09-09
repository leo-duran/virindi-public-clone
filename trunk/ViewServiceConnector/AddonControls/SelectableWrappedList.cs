///////////////////////////////////////////////////////////////////////////////
//File: SelectableWrappedList.cs
//
//Description: SelectableWrappedList, a MetaView list with selectable items
//
//References required:
//  System.Drawing
//  Wrapper.cs (MetaViewWrapper interface definitions)
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

namespace MyClasses
{
    class SelectableWrappedList : MyClasses.MetaViewWrappers.IList
    {
        MyClasses.MetaViewWrappers.IList Underlying;
        int[] iSelectChangeColumns;
        public SelectableWrappedList(MyClasses.MetaViewWrappers.IList mylist, int[] SelectChangeColumns)
        {
            Underlying = mylist;
            Underlying.Click += new MyClasses.MetaViewWrappers.dClickedList(Underlying_Click);
            iSelectChangeColumns = SelectChangeColumns;
        }

        int iselectedrow = -1;
        public delegate void delint(int row);
        public event delint SelectionChanged;

        Color iSelectedColor = Color.BlueViolet;
        public Color SelectedColor
        {
            get
            {
                return iSelectedColor;
            }
            set
            {
                iSelectedColor = value;
                if (SelectedRow != -1)
                    isetrowcolors(SelectedRow, value);
            }
        }
        public int SelectedRow
        {
            get
            {
                return iselectedrow;
            }
            set
            {
                if (value >= Underlying.RowCount) return;
                if (value < -1) return;

                iselectrow(value);
            }
        }

        void isetrowcolors(int r, Color c)
        {
            foreach (int col in iSelectChangeColumns)
            {
                Underlying[r][col].Color = c;
            }
        }

        void iresetrowcolors(int r)
        {
            foreach (int c in iSelectChangeColumns)
            {
                Underlying[r][c].ResetColor();
            }
        }

        void iselectrow(int r)
        {
            if (SelectedRow == r) return;

            //deselect old row
            if (SelectedRow != -1)
            {
                iresetrowcolors(SelectedRow);
            }

            //select new row
            if (r != -1)
            {
                isetrowcolors(r, SelectedColor);
            }
            iselectedrow = r;

            if (SelectionChanged != null)
                SelectionChanged(r);
        }

        void Underlying_Click(object sender, int row, int col)
        {
            if (Array.Exists<int>(iSelectChangeColumns, delegate(int obj) { return obj == col; }))
            {
                if (SelectedRow == row)
                    iselectrow(-1);
                else
                    iselectrow(row);
            }
            if (Click != null)
                Click(this, row, col);
        }

        #region IList Members

        public event MyClasses.MetaViewWrappers.dClickedList Click;

        public virtual void Clear()
        {
            Underlying.Clear();
        }

        public MyClasses.MetaViewWrappers.IListRow this[int row]
        {
            get { return Underlying[row]; }
        }

        public virtual MyClasses.MetaViewWrappers.IListRow AddRow()
        {
            return Underlying.AddRow();
        }

        public virtual MyClasses.MetaViewWrappers.IListRow InsertRow(int pos)
        {
            if (SelectedRow >= pos)
                iselectedrow++;
            return Underlying.InsertRow(pos);
        }

        public int RowCount
        {
            get { return Underlying.RowCount; }
        }

        public virtual void RemoveRow(int index)
        {
            if (SelectedRow == index)
                iselectrow(-1);
            else if (SelectedRow > index)
                iselectedrow--;
            Underlying.RemoveRow(index);
        }

        public int ColCount
        {
            get
            {
                return Underlying.ColCount;
            }
        }

        #endregion

        #region IControl Members

        public string Name
        {
            get { return Underlying.Name; }
        }

        public bool Visible
        {
            get { return Underlying.Visible; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Underlying.Dispose();
        }

        #endregion
    }
}
