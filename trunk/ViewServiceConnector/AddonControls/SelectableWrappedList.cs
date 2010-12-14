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

#if METAVIEW_PUBLIC_NS
using MetaViewWrappers;
#else
using MyClasses.MetaViewWrappers;
#endif

namespace MyClasses
{
    class SelectableWrappedList : IList
    {
        MyClasses.MetaViewWrappers.IList Underlying;
        int[] iSelectChangeColumns;
        public SelectableWrappedList(IList mylist, int[] SelectChangeColumns)
        {
            Underlying = mylist;
            Underlying.Click += new dClickedList(Underlying_Click);
            iSelectChangeColumns = SelectChangeColumns;

#if VVS_REFERENCED
            if (Underlying.GetType() == typeof(MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.List))
                Curtain_VVSInit();
#endif
        }

        void Curtain_VVSInit()
        {
            ((MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.List)Underlying).Underlying.ThemeChanged += new EventHandler(Underlying_ThemeChanged);
        }

        void Underlying_ThemeChanged(object sender, EventArgs e)
        {
            if (SelectedRow != -1)
                isetrowcolors(SelectedRow, Color.Black);
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

        Color VVSSelectColor { get { return ((MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.List)Underlying).Underlying.Theme.GetVal<Color>("Hint_ListTextSelectionColor"); } } 

        void isetrowcolors(int r, Color c)
        {
            Color rc = c;

#if VVS_REFERENCED
            if (Underlying.GetType() == typeof(MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.List))
            {
                rc = VVSSelectColor;
            }
#endif

            foreach (int col in iSelectChangeColumns)
            {
                Underlying[r][col].Color = rc;
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
            if (Selected != null)
                Selected(this, new MVListSelectEventArgs(0, row, col));
        }

        #region IList Members

        public event dClickedList Click;

        public virtual void Clear()
        {
            bool changed = (iselectedrow != -1);
            iselectedrow = -1;

            Underlying.Clear();

            if (changed)
                if (SelectionChanged != null)
                    SelectionChanged(iselectedrow);
        }

        public IListRow this[int row]
        {
            get { return Underlying[row]; }
        }

        public virtual IListRow AddRow()
        {
            return Underlying.AddRow();
        }

        public virtual IListRow InsertRow(int pos)
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

        public int ScrollPosition
        {
            get
            {
                return Underlying.ScrollPosition;
            }
            set
            {
                Underlying.ScrollPosition = value;
            }
        }

        public event EventHandler<MVListSelectEventArgs> Selected;

        public IListRow Add()
        {
            return Underlying.Add();
        }

        public IListRow Insert(int pos)
        {
            return Underlying.Insert(pos);
        }

        public void Delete(int index)
        {
            Underlying.Delete(index);
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

        public string TooltipText
        {
            get
            {
                return Underlying.TooltipText;
            }
            set
            {
                Underlying.TooltipText = value;
            }
        }

        bool IControl.Visible
        {
            get
            {
                return Underlying.Visible;
            }
            set
            {
                Underlying.Visible = value;
            }
        }

        public int Id
        {
            get { return Underlying.Id; }
        }

        public Rectangle LayoutPosition
        {
            get
            {
                return Underlying.LayoutPosition;
            }
            set
            {
                Underlying.LayoutPosition = value;
            }
        }

        public MetaViewWrappers.IList UnderlyingList
        {
            get
            {
                return Underlying;
            }
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
