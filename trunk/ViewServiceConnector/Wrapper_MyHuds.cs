///////////////////////////////////////////////////////////////////////////////
//File: Wrapper_MyHuds.cs
//
//Description: Contains MetaViewWrapper classes implementing Virindi View Service
//  views. These classes are only compiled if the VVS_REFERENCED symbol is defined.
//
//References required:
//  System.Drawing
//  VirindiViewService (if VVS_REFERENCED is defined)
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

#if VVS_REFERENCED

using System;
using System.Collections.Generic;
using System.Text;
using VirindiViewService;

#if METAVIEW_PUBLIC_NS
namespace MetaViewWrappers.VirindiViewServiceHudControls
#else
namespace MyClasses.MetaViewWrappers.VirindiViewServiceHudControls
#endif
{
#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class View : IView
    {
        HudView myView;
        public HudView Underlying { get { return myView; } }

        #region IView Members

        public void Initialize(Decal.Adapter.Wrappers.PluginHost p, string pXML)
        {
            VirindiViewService.XMLParsers.Decal3XMLParser ps = new VirindiViewService.XMLParsers.Decal3XMLParser();
            ViewProperties iprop;
            ControlGroup igroup;
            ps.ParseFromResource(pXML, out iprop, out igroup);
            myView = new VirindiViewService.HudView(iprop, igroup);
        }

        public void InitializeRawXML(Decal.Adapter.Wrappers.PluginHost p, string pXML)
        {
            VirindiViewService.XMLParsers.Decal3XMLParser ps = new VirindiViewService.XMLParsers.Decal3XMLParser();
            ViewProperties iprop;
            ControlGroup igroup;
            ps.Parse(pXML, out iprop, out igroup);
            myView = new VirindiViewService.HudView(iprop, igroup);
        }

        public void SetIcon(int icon, int iconlibrary)
        {
            myView.Icon = ACImage.FromIconLibrary(icon, iconlibrary);
        }

        public void SetIcon(int portalicon)
        {
            myView.Icon = portalicon;
        }

        public string Title
        {
            get
            {
                return myView.Title;
            }
            set
            {
                myView.Title = value;
            }
        }

        public bool Visible
        {
            get
            {
                return myView.Visible;
            }
            set
            {
                myView.Visible = value;
            }
        }

        public bool Activated
        {
            get
            {
                return Visible;
            }
            set
            {
                Visible = value;
            }
        }

        public void Activate()
        {
            Visible = true;
        }

        public void Deactivate()
        {
            Visible = false;
        }

        public System.Drawing.Point Location
        {
            get
            {
                return myView.Location;
            }
            set
            {
                myView.Location = value;
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(myView.Width, myView.Height);
            }
        }

        public System.Drawing.Rectangle Position
        {
            get
            {
                return new System.Drawing.Rectangle(Location, Size);
            }
            set
            {
                Location = value.Location;
                myView.ClientArea = value.Size;
            }
        }

#if VVS_WRAPPERS_PUBLIC
        internal
#else
        public
#endif
        ViewSystemSelector.eViewSystem ViewType { get { return ViewSystemSelector.eViewSystem.VirindiViewService; } }

        public IControl this[string id]
        {
            get
            {
                Control ret = null;
                VirindiViewService.Controls.HudControl iret = myView[id];
                if (iret.GetType() == typeof(VirindiViewService.Controls.HudButton))
                    ret = new Button();
                if (iret.GetType() == typeof(VirindiViewService.Controls.HudCheckBox))
                    ret = new CheckBox();
                if (iret.GetType() == typeof(VirindiViewService.Controls.HudTextBox))
                    ret = new TextBox();
                if (iret.GetType() == typeof(VirindiViewService.Controls.HudCombo))
                    ret = new Combo();
                if (iret.GetType() == typeof(VirindiViewService.Controls.HudHSlider))
                    ret = new Slider();
                if (iret.GetType() == typeof(VirindiViewService.Controls.HudList))
                    ret = new List();
                if (iret.GetType() == typeof(VirindiViewService.Controls.HudStaticText))
                    ret = new StaticText();
                if (iret.GetType() == typeof(VirindiViewService.Controls.HudTabView))
                    ret = new Notebook();
                if (iret.GetType() == typeof(VirindiViewService.Controls.HudProgressBar))
                    ret = new ProgressBar();

                if (ret == null) return null;

                ret.myControl = iret;
                ret.myName = id;
                ret.Initialize();
                allocatedcontrols.Add(ret);
                return ret;
            }
        }

        #endregion

        #region IDisposable Members

        bool disposed = false;
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            GC.SuppressFinalize(this);

            foreach (Control c in allocatedcontrols)
                c.Dispose();

            myView.Dispose();
        }

        #endregion

        List<Control> allocatedcontrols = new List<Control>();
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class Control : IControl
    {
        internal VirindiViewService.Controls.HudControl myControl;
        internal string myName;
        public VirindiViewService.Controls.HudControl Underlying { get { return myControl; } }

        public virtual void Initialize()
        {

        }

        #region IControl Members

        public string Name
        {
            get { return myName; }
        }

        public bool Visible
        {
            get { return myControl.Visible; }
        }

        VirindiViewService.TooltipSystem.cTooltipInfo itooltipinfo = null;
        public string TooltipText
        {
            get
            {
                if (itooltipinfo != null)
                    return itooltipinfo.Text;
                else
                    return "";
            }
            set
            {
                if (itooltipinfo != null)
                {
                    VirindiViewService.TooltipSystem.RemoveTooltip(itooltipinfo);
                    itooltipinfo = null;
                }
                if (!String.IsNullOrEmpty(value))
                {
                    itooltipinfo = VirindiViewService.TooltipSystem.AssociateTooltip(myControl, value);
                }
            }
        }

        #endregion

        #region IDisposable Members

        bool disposed = false;
        public virtual void Dispose()
        {
            if (disposed) return;
            disposed = true;

            myControl.Dispose();
        }

        #endregion
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class Button : Control, IButton
    {
        public override void Initialize()
        {
            base.Initialize();
            ((VirindiViewService.Controls.HudButton)myControl).MouseEvent += new EventHandler<VirindiViewService.Controls.ControlMouseEventArgs>(Button_MouseEvent);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((VirindiViewService.Controls.HudButton)myControl).MouseEvent -= new EventHandler<VirindiViewService.Controls.ControlMouseEventArgs>(Button_MouseEvent);
        }

        void Button_MouseEvent(object sender, VirindiViewService.Controls.ControlMouseEventArgs e)
        {
            switch (e.EventType)
            {
                case VirindiViewService.Controls.ControlMouseEventArgs.MouseEventType.MouseHit:
                    if (Click != null)
                        Click(this, new MVControlEventArgs(0));
                    return;
                case VirindiViewService.Controls.ControlMouseEventArgs.MouseEventType.MouseDown:
                    if (Hit != null)
                        Hit(this, null);
                    return;
            }
        }

        #region IButton Members

        public string Text
        {
            get
            {
                return ((VirindiViewService.Controls.HudButton)myControl).Text;
            }
            set
            {
                ((VirindiViewService.Controls.HudButton)myControl).Text = value;
            }
        }

        public System.Drawing.Color TextColor
        {
            get
            {
                return System.Drawing.Color.Black;
            }
            set
            {

            }
        }

        public event EventHandler Hit;
        public event EventHandler<MVControlEventArgs> Click;

        #endregion
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class CheckBox : Control, ICheckBox
    {
        public override void Initialize()
        {
            base.Initialize();
            ((VirindiViewService.Controls.HudCheckBox)myControl).Change += new EventHandler(CheckBox_Change);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((VirindiViewService.Controls.HudCheckBox)myControl).Change -= new EventHandler(CheckBox_Change);
        }

        void CheckBox_Change(object sender, EventArgs e)
        {
            if (Change != null)
                Change(this, new MVCheckBoxChangeEventArgs(0, Checked));
            if (Change_Old != null)
                Change_Old(this, null);
        }

        #region ICheckBox Members

        public string Text
        {
            get
            {
                return ((VirindiViewService.Controls.HudCheckBox)myControl).Text;
            }
            set
            {
                ((VirindiViewService.Controls.HudCheckBox)myControl).Text = value;
            }
        }

        public bool Checked
        {
            get
            {
                return ((VirindiViewService.Controls.HudCheckBox)myControl).Checked;
            }
            set
            {
                ((VirindiViewService.Controls.HudCheckBox)myControl).Checked = value;
            }
        }

        public event EventHandler<MVCheckBoxChangeEventArgs> Change;
        public event EventHandler Change_Old;

        #endregion
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class TextBox : Control, ITextBox
    {
        public override void Initialize()
        {
            base.Initialize();
            ((VirindiViewService.Controls.HudTextBox)myControl).Change += new EventHandler(TextBox_Change);
            myControl.LostFocus += new EventHandler(myControl_LostFocus);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((VirindiViewService.Controls.HudTextBox)myControl).Change -= new EventHandler(TextBox_Change);
            myControl.LostFocus -= new EventHandler(myControl_LostFocus);
        }

        void TextBox_Change(object sender, EventArgs e)
        {
            if (Change != null)
                Change(this, new MVTextBoxChangeEventArgs(0, Text));
            if (Change_Old != null)
                Change_Old(this, null);
        }

        void myControl_LostFocus(object sender, EventArgs e)
        {
            if (!myControl.HasFocus) return;

            if (End != null)
                End(this, new MVTextBoxEndEventArgs(0, true));
        }

        #region ITextBox Members

        public string Text
        {
            get
            {
                return ((VirindiViewService.Controls.HudTextBox)myControl).Text;
            }
            set
            {
                ((VirindiViewService.Controls.HudTextBox)myControl).Text = value;
            }
        }

        public int Caret
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        public event EventHandler<MVTextBoxChangeEventArgs> Change;
        public event EventHandler Change_Old;
        public event EventHandler<MVTextBoxEndEventArgs> End;

        #endregion
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class Combo : Control, ICombo
    {
        List<object> iData = new List<object>();

        public class ComboIndexer : IComboIndexer
        {
            Combo underlying;
            internal ComboIndexer(Combo c)
            {
                underlying = c;
            }

            #region IComboIndexer Members

            public string this[int index]
            {
                get
                {
                    return ((VirindiViewService.Controls.HudStaticText)(((VirindiViewService.Controls.HudCombo)underlying.myControl)[index])).Text;
                }
                set
                {
                    ((VirindiViewService.Controls.HudStaticText)(((VirindiViewService.Controls.HudCombo)underlying.myControl)[index])).Text = value;
                }
            }

            #endregion
        }

        public class ComboDataIndexer : IComboDataIndexer
        {
            Combo underlying;
            internal ComboDataIndexer(Combo c)
            {
                underlying = c;
            }

            #region IComboIndexer Members

            public object this[int index]
            {
                get
                {
                    return underlying.iData[index];
                }
                set
                {
                    underlying.iData[index] = value;
                }
            }

            #endregion
        }

        public override void Initialize()
        {
            base.Initialize();
            ((VirindiViewService.Controls.HudCombo)myControl).Change += new EventHandler(Combo_Change);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((VirindiViewService.Controls.HudCombo)myControl).Change -= new EventHandler(Combo_Change);
        }

        void Combo_Change(object sender, EventArgs e)
        {
            if (Change != null)
                Change(this, new MVIndexChangeEventArgs(0, Selected));
            if (Change_Old != null)
                Change_Old(this, null);
        }

        #region ICombo Members

        public IComboIndexer Text
        {
            get { return new ComboIndexer(this); }
        }

        public IComboDataIndexer Data
        {
            get { return new ComboDataIndexer(this); }
        }

        public int Count
        {
            get { return ((VirindiViewService.Controls.HudCombo)myControl).Count; }
        }

        public int Selected
        {
            get
            {
                return ((VirindiViewService.Controls.HudCombo)myControl).Current;
            }
            set
            {
                ((VirindiViewService.Controls.HudCombo)myControl).Current = value;
            }
        }

        public event EventHandler<MVIndexChangeEventArgs> Change;
        public event EventHandler Change_Old;

        public void Add(string text)
        {
            ((VirindiViewService.Controls.HudCombo)myControl).AddItem(text, null);
            iData.Add(null);
        }

        public void Add(string text, object obj)
        {
            ((VirindiViewService.Controls.HudCombo)myControl).AddItem(text, null);
            iData.Add(obj);
        }

        public void Insert(int index, string text)
        {
            ((VirindiViewService.Controls.HudCombo)myControl).InsertItem(index, text, null);
            iData.Insert(index, null);
        }

        public void RemoveAt(int index)
        {
            ((VirindiViewService.Controls.HudCombo)myControl).DeleteItem(index);
            iData.RemoveAt(index);
        }

        public void Remove(int index)
        {
            RemoveAt(index);
        }

        public void Clear()
        {
            ((VirindiViewService.Controls.HudCombo)myControl).Clear();
            iData.Clear();
        }

        #endregion
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class Slider : Control, ISlider
    {
        public override void Initialize()
        {
            base.Initialize();
            ((VirindiViewService.Controls.HudHSlider)myControl).Changed += new VirindiViewService.Controls.LinearPositionControl.delScrollChanged(Slider_Changed);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((VirindiViewService.Controls.HudHSlider)myControl).Changed -= new VirindiViewService.Controls.LinearPositionControl.delScrollChanged(Slider_Changed);
        }

        void Slider_Changed(int min, int max, int pos)
        {
            if (Change != null)
                Change(this, new MVIndexChangeEventArgs(0, pos));
            if (Change_Old != null)
                Change_Old(this, null);
        }

        #region ISlider Members

        public int Position
        {
            get
            {
                return ((VirindiViewService.Controls.HudHSlider)myControl).Position;
            }
            set
            {
                ((VirindiViewService.Controls.HudHSlider)myControl).Position = value;
            }
        }

        public event EventHandler<MVIndexChangeEventArgs> Change;
        public event EventHandler Change_Old;

        #endregion
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class List : Control, IList
    {
        public override void Initialize()
        {
            base.Initialize();
            ((VirindiViewService.Controls.HudList)myControl).Click += new VirindiViewService.Controls.HudList.delClickedControl(List_Click);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((VirindiViewService.Controls.HudList)myControl).Click -= new VirindiViewService.Controls.HudList.delClickedControl(List_Click);
        }

        void List_Click(object sender, int row, int col)
        {
            if (Click != null)
                Click(this, row, col);
            if (Selected != null)
                Selected(this, new MVListSelectEventArgs(0, row, col));
        }

        public class ListRow : IListRow
        {
            List myList;
            int myRow;
            internal ListRow(int row, List l)
            {
                myList = l;
                myRow = row;
            }

            #region IListRow Members

            public IListCell this[int col]
            {
                get { return new ListCell(myRow, col, myList); }
            }

            #endregion
        }
        public class ListCell : IListCell
        {
            List myList;
            int myRow;
            int myCol;
            internal ListCell(int row, int col, List l)
            {
                myRow = row;
                myCol = col;
                myList = l;
            }

            #region IListCell Members

            public void ResetColor()
            {
                ((VirindiViewService.Controls.HudStaticText)(((VirindiViewService.Controls.HudList)myList.myControl)[myRow][myCol])).ResetTextColor();
            }

            public System.Drawing.Color Color
            {
                get
                {
                    return ((VirindiViewService.Controls.HudStaticText)(((VirindiViewService.Controls.HudList)myList.myControl)[myRow][myCol])).TextColor;
                }
                set
                {
                    ((VirindiViewService.Controls.HudStaticText)(((VirindiViewService.Controls.HudList)myList.myControl)[myRow][myCol])).TextColor = value;
                }
            }

            public int Width
            {
                get
                {
                    return ((VirindiViewService.Controls.HudStaticText)(((VirindiViewService.Controls.HudList)myList.myControl)[myRow][myCol])).ClipRegion.Width;
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public object this[int subval]
            {
                get
                {
                    VirindiViewService.Controls.HudControl c = ((VirindiViewService.Controls.HudList)myList.myControl)[myRow][myCol];
                    if (subval == 0)
                    {
                        if (c.GetType() == typeof(VirindiViewService.Controls.HudStaticText))
                            return ((VirindiViewService.Controls.HudStaticText)c).Text;
                        if (c.GetType() == typeof(VirindiViewService.Controls.HudCheckBox))
                            return ((VirindiViewService.Controls.HudCheckBox)c).Checked;
                    }
                    else if (subval == 1)
                    {
                        if (c.GetType() == typeof(VirindiViewService.Controls.HudPictureBox))
                            return ((VirindiViewService.Controls.HudPictureBox)c).Image.PortalImageID;
                    }
                    return null;
                }
                set
                {
                    VirindiViewService.Controls.HudControl c = ((VirindiViewService.Controls.HudList)myList.myControl)[myRow][myCol];
                    if (subval == 0)
                    {
                        if (c.GetType() == typeof(VirindiViewService.Controls.HudStaticText))
                            ((VirindiViewService.Controls.HudStaticText)c).Text = (string)value;
                        if (c.GetType() == typeof(VirindiViewService.Controls.HudCheckBox))
                            ((VirindiViewService.Controls.HudCheckBox)c).Checked = (bool)value;
                    }
                    else if (subval == 1)
                    {
                        if (c.GetType() == typeof(VirindiViewService.Controls.HudPictureBox))
                            ((VirindiViewService.Controls.HudPictureBox)c).Image = (int)value;
                    }
                }
            }

            #endregion
        }

        #region IList Members

        public event dClickedList Click;
        public event EventHandler<MVListSelectEventArgs> Selected;

        public void Clear()
        {
            ((VirindiViewService.Controls.HudList)myControl).ClearRows();
        }

        public IListRow this[int row]
        {
            get { return new ListRow(row, this); }
        }

        public IListRow AddRow()
        {
            ((VirindiViewService.Controls.HudList)myControl).AddRow();
            return new ListRow(((VirindiViewService.Controls.HudList)myControl).RowCount - 1, this);
        }

        public IListRow Add()
        {
            return AddRow();
        }

        public IListRow InsertRow(int pos)
        {
            ((VirindiViewService.Controls.HudList)myControl).InsertRow(pos);
            return new ListRow(pos, this);
        }

        public IListRow Insert(int pos)
        {
            return InsertRow(pos);
        }

        public int RowCount
        {
            get { return ((VirindiViewService.Controls.HudList)myControl).RowCount; }
        }

        public void RemoveRow(int index)
        {
            ((VirindiViewService.Controls.HudList)myControl).RemoveRow(index);
        }

        public void Delete(int index)
        {
            RemoveRow(index);
        }

        public int ColCount
        {
            get
            {
                return 0;
                //return ((VirindiViewService.Controls.HudList)myControl).ColumnCount;
            }
        }

        public int ScrollPosition
        {
            get
            {
                return ((VirindiViewService.Controls.HudList)myControl).ScrollPosition;
            }
            set
            {
                ((VirindiViewService.Controls.HudList)myControl).ScrollPosition = value;
            }
        }

        #endregion
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class StaticText : Control, IStaticText
    {
        public override void Initialize()
        {
            base.Initialize();
            //((VirindiViewService.Controls.HudStaticText)myControl)
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #region IStaticText Members

        public string Text
        {
            get
            {
                return ((VirindiViewService.Controls.HudStaticText)myControl).Text;
            }
            set
            {
                ((VirindiViewService.Controls.HudStaticText)myControl).Text = value;
            }
        }

#pragma warning disable 0067
        public event EventHandler<MVControlEventArgs> Click;
#pragma warning restore 0067

        #endregion
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class Notebook : Control, INotebook
    {
        public override void Initialize()
        {
            base.Initialize();
            ((VirindiViewService.Controls.HudTabView)myControl).OpenTabChange += new EventHandler(Notebook_OpenTabChange);
        }

        public override void Dispose()
        {
            ((VirindiViewService.Controls.HudTabView)myControl).OpenTabChange -= new EventHandler(Notebook_OpenTabChange);
            base.Dispose();
        }

        void Notebook_OpenTabChange(object sender, EventArgs e)
        {
            if (Change != null)
                Change(this, new MVIndexChangeEventArgs(0, ActiveTab));
        }

        #region INotebook Members

        public event EventHandler<MVIndexChangeEventArgs> Change;

        public int ActiveTab
        {
            get
            {
                return ((VirindiViewService.Controls.HudTabView)myControl).CurrentTab;
            }
            set
            {
                ((VirindiViewService.Controls.HudTabView)myControl).CurrentTab = value;
                ((VirindiViewService.Controls.HudTabView)myControl).Invalidate();
            }
        }

        #endregion
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class ProgressBar : Control, IProgressBar
    {

        #region IProgressBar Members

        public int Position
        {
            get
            {
                return ((VirindiViewService.Controls.HudProgressBar)myControl).Position;
            }
            set
            {
                ((VirindiViewService.Controls.HudProgressBar)myControl).Position = value;
            }
        }

        public int Value
        {
            get
            {
                return Position;
            }
            set
            {
                Position = value;
            }
        }

        public string PreText
        {
            get
            {
                return ((VirindiViewService.Controls.HudProgressBar)myControl).PreText;
            }
            set
            {
                ((VirindiViewService.Controls.HudProgressBar)myControl).PreText = value;
            }
        }

        #endregion
    }
}

#endif