///////////////////////////////////////////////////////////////////////////////
//File: Wrapper_Decal.cs
//
//Description: Contains MetaViewWrapper classes implementing Decal views.
//
//References required:
//  System.Drawing
//  Decal.Adapter
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

namespace MyClasses.MetaViewWrappers.DecalControls
{
    internal class View : IView
    {
        Decal.Adapter.Wrappers.ViewWrapper myView;
        public Decal.Adapter.Wrappers.ViewWrapper Underlying { get { return myView; } }

        #region IView Members

        public void Initialize(Decal.Adapter.Wrappers.PluginHost p, string pXML)
        {
            myView = p.LoadViewResource(pXML);
        }

        public void SetIcon(int icon, int iconlibrary)
        {
            myView.SetIcon(icon, iconlibrary);
        }

        public void SetIcon(int portalicon)
        {
            throw new Exception("The method or operation is not implemented.");
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
                return myView.Activated;
            }
            set
            {
                myView.Activated = value;
            }
        }

        public ViewSystemSelector.eViewSystem ViewType { get { return ViewSystemSelector.eViewSystem.DecalInject; } }

        public IControl this[string id]
        {
            get
            {
                Control ret = null;
                Decal.Adapter.Wrappers.IControlWrapper iret = myView.Controls[id];
                if (iret.GetType() == typeof(Decal.Adapter.Wrappers.PushButtonWrapper))
                    ret = new Button();
                if (iret.GetType() == typeof(Decal.Adapter.Wrappers.CheckBoxWrapper))
                    ret = new CheckBox();
                if (iret.GetType() == typeof(Decal.Adapter.Wrappers.TextBoxWrapper))
                    ret = new TextBox();
                if (iret.GetType() == typeof(Decal.Adapter.Wrappers.ChoiceWrapper))
                    ret = new Combo();
                if (iret.GetType() == typeof(Decal.Adapter.Wrappers.SliderWrapper))
                    ret = new Slider();
                if (iret.GetType() == typeof(Decal.Adapter.Wrappers.ListWrapper))
                    ret = new List();
                if (iret.GetType() == typeof(Decal.Adapter.Wrappers.StaticWrapper))
                    ret = new StaticText();
                if (iret.GetType() == typeof(Decal.Adapter.Wrappers.NotebookWrapper))
                    ret = new Notebook();
                if (iret.GetType() == typeof(Decal.Adapter.Wrappers.ProgressWrapper))
                    ret = new ProgressBar();

                if (ret == null) return null;

                ret.myControl = iret;
                ret.myName = id;
                ret.Initialize();
                allocatedcontrols.Add(ret);
                return ret;
            }
        }

        List<Control> allocatedcontrols = new List<Control>();

        #endregion

        #region IDisposable Members

        bool disposed = false;
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            foreach (Control c in allocatedcontrols)
                c.Dispose();

            myView.Dispose();
        }

        #endregion
    }
    internal class Control : IControl
    {
        internal Decal.Adapter.Wrappers.IControlWrapper myControl;
        public Decal.Adapter.Wrappers.IControlWrapper Underlying { get { return myControl; } }
        internal string myName;

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
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IDisposable Members

        bool disposed = false;
        public virtual void Dispose()
        {
            if (disposed) return;
            disposed = true;

            //myControl.Dispose();
        }

        #endregion
    }
    internal class Button : Control, IButton
    {
        public override void Initialize()
        {
            base.Initialize();
            ((Decal.Adapter.Wrappers.PushButtonWrapper)myControl).Hit += new EventHandler<Decal.Adapter.ControlEventArgs>(Button_Hit);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((Decal.Adapter.Wrappers.PushButtonWrapper)myControl).Hit -= new EventHandler<Decal.Adapter.ControlEventArgs>(Button_Hit);
        }

        void Button_Hit(object sender, Decal.Adapter.ControlEventArgs e)
        {
            if (Hit != null)
                Hit(this, null);
        }

        #region IButton Members

        public string Text
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public event EventHandler Hit;

        #endregion
    }
    internal class CheckBox : Control, ICheckBox
    {
        public override void Initialize()
        {
            base.Initialize();
            ((Decal.Adapter.Wrappers.CheckBoxWrapper)myControl).Change += new EventHandler<Decal.Adapter.CheckBoxChangeEventArgs>(CheckBox_Change);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((Decal.Adapter.Wrappers.CheckBoxWrapper)myControl).Change -= new EventHandler<Decal.Adapter.CheckBoxChangeEventArgs>(CheckBox_Change);
        }

        void CheckBox_Change(object sender, Decal.Adapter.CheckBoxChangeEventArgs e)
        {
            if (Change != null)
                Change(this, null);
        }

        #region ICheckBox Members

        public string Text
        {
            get
            {
                return ((Decal.Adapter.Wrappers.CheckBoxWrapper)myControl).Text;
            }
            set
            {
                ((Decal.Adapter.Wrappers.CheckBoxWrapper)myControl).Text = value;
            }
        }

        public bool Checked
        {
            get
            {
                return ((Decal.Adapter.Wrappers.CheckBoxWrapper)myControl).Checked;
            }
            set
            {
                ((Decal.Adapter.Wrappers.CheckBoxWrapper)myControl).Checked = value;
            }
        }

        public event EventHandler Change;

        #endregion
    }
    internal class TextBox : Control, ITextBox
    {
        public override void Initialize()
        {
            base.Initialize();
            ((Decal.Adapter.Wrappers.TextBoxWrapper)myControl).Change += new EventHandler<Decal.Adapter.TextBoxChangeEventArgs>(TextBox_Change);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((Decal.Adapter.Wrappers.TextBoxWrapper)myControl).Change -= new EventHandler<Decal.Adapter.TextBoxChangeEventArgs>(TextBox_Change);
        }

        void TextBox_Change(object sender, Decal.Adapter.TextBoxChangeEventArgs e)
        {
            if (Change != null)
                Change(this, null);
        }

        #region ITextBox Members

        public string Text
        {
            get
            {
                return ((Decal.Adapter.Wrappers.TextBoxWrapper)myControl).Text;
            }
            set
            {
                ((Decal.Adapter.Wrappers.TextBoxWrapper)myControl).Text = value;
            }
        }

        public event EventHandler Change;

        #endregion
    }
    internal class Combo : Control, ICombo
    {
        public override void Initialize()
        {
            base.Initialize();
            ((Decal.Adapter.Wrappers.ChoiceWrapper)myControl).Change += new EventHandler<Decal.Adapter.IndexChangeEventArgs>(Combo_Change);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((Decal.Adapter.Wrappers.ChoiceWrapper)myControl).Change -= new EventHandler<Decal.Adapter.IndexChangeEventArgs>(Combo_Change);
        }

        void Combo_Change(object sender, Decal.Adapter.IndexChangeEventArgs e)
        {
            if (Change != null)
                Change(this, null);
        }

        #region ICombo Members

        public IComboIndexer Text
        {
            get
            {
                return new ComboIndexer(this);
            }
        }

        public int Count
        {
            get
            {
                return ((Decal.Adapter.Wrappers.ChoiceWrapper)myControl).Count;
            }
        }

        public int Selected
        {
            get
            {
                return ((Decal.Adapter.Wrappers.ChoiceWrapper)myControl).Selected;
            }
            set
            {
                ((Decal.Adapter.Wrappers.ChoiceWrapper)myControl).Selected = value;
            }
        }

        public event EventHandler Change;

        public void Add(string text)
        {
            ((Decal.Adapter.Wrappers.ChoiceWrapper)myControl).Add(text, null);
        }

        public void Insert(int index, string text)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            ((Decal.Adapter.Wrappers.ChoiceWrapper)myControl).Remove(index);
        }

        public void Clear()
        {
            ((Decal.Adapter.Wrappers.ChoiceWrapper)myControl).Clear();
        }

        #endregion
        internal class ComboIndexer: IComboIndexer
        {
            Combo myCombo;
            internal ComboIndexer(Combo c)
            {
                myCombo = c;
            }

            #region IComboIndexer Members

            public string this[int index]
            {
                get
                {
                    return ((Decal.Adapter.Wrappers.ChoiceWrapper)myCombo.myControl).Text[index];
                }
                set
                {
                    ((Decal.Adapter.Wrappers.ChoiceWrapper)myCombo.myControl).Text[index] = value;
                }
            }

            #endregion
        }
    }
    internal class Slider : Control, ISlider
    {
        public override void Initialize()
        {
            base.Initialize();
            ((Decal.Adapter.Wrappers.SliderWrapper)myControl).Change += new EventHandler<Decal.Adapter.IndexChangeEventArgs>(Slider_Change);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((Decal.Adapter.Wrappers.SliderWrapper)myControl).Change -= new EventHandler<Decal.Adapter.IndexChangeEventArgs>(Slider_Change);
        }

        void Slider_Change(object sender, Decal.Adapter.IndexChangeEventArgs e)
        {
            if (Change != null)
                Change(this, null);
        }

        #region ISlider Members

        public int Position
        {
            get
            {
                return ((Decal.Adapter.Wrappers.SliderWrapper)myControl).SliderPostition;
            }
            set
            {
                ((Decal.Adapter.Wrappers.SliderWrapper)myControl).SliderPostition = value;
            }
        }

        public event EventHandler Change;

        #endregion
    }
    internal class List : Control, IList
    {
        public override void Initialize()
        {
            base.Initialize();
            ((Decal.Adapter.Wrappers.ListWrapper)myControl).Selected += new EventHandler<Decal.Adapter.ListSelectEventArgs>(List_Selected);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((Decal.Adapter.Wrappers.ListWrapper)myControl).Selected -= new EventHandler<Decal.Adapter.ListSelectEventArgs>(List_Selected);
        }

        void List_Selected(object sender, Decal.Adapter.ListSelectEventArgs e)
        {
            if (Click != null)
                Click(this, e.Row, e.Column);
        }

        #region IList Members

        public event dClickedList Click;

        public void Clear()
        {
            ((Decal.Adapter.Wrappers.ListWrapper)myControl).Clear();
        }

        public IListRow this[int row]
        {
            get
            {
                return new ListRow(this, row);
            }
        }

        public IListRow AddRow()
        {
            ((Decal.Adapter.Wrappers.ListWrapper)myControl).Add();
            return new ListRow(this, ((Decal.Adapter.Wrappers.ListWrapper)myControl).RowCount - 1);
        }

        public IListRow InsertRow(int pos)
        {
            ((Decal.Adapter.Wrappers.ListWrapper)myControl).Insert(pos);
            return new ListRow(this, pos);
        }

        public int RowCount
        {
            get { return ((Decal.Adapter.Wrappers.ListWrapper)myControl).RowCount; }
        }

        public void RemoveRow(int index)
        {
            ((Decal.Adapter.Wrappers.ListWrapper)myControl).Delete(index);
        }

        public int ColCount
        {
            get
            {
                return ((Decal.Adapter.Wrappers.ListWrapper)myControl).ColCount;
            }
        }

        #endregion

        public class ListRow : IListRow
        {
            internal List myList;
            internal int myRow;
            internal ListRow(List l, int r)
            {
                myList = l;
                myRow = r;
            }


            #region IListRow Members

            public IListCell this[int col]
            {
                get { return new ListCell(myList, myRow, col); }
            }

            #endregion
        }

        public class ListCell : IListCell
        {
            internal List myList;
            internal int myRow;
            internal int myCol;
            public ListCell(List l, int r, int c)
            {
                myList = l;
                myRow = r;
                myCol = c;
            }

            #region IListCell Members

            public void ResetColor()
            {
                Color = System.Drawing.Color.White;
            }

            public System.Drawing.Color Color
            {
                get
                {
                    return ((Decal.Adapter.Wrappers.ListWrapper)myList.myControl)[myRow][myCol].Color;
                }
                set
                {
                    ((Decal.Adapter.Wrappers.ListWrapper)myList.myControl)[myRow][myCol].Color = value;
                }
            }

            public int Width
            {
                get
                {
                    return ((Decal.Adapter.Wrappers.ListWrapper)myList.myControl)[myRow][myCol].Width;
                }
                set
                {
                    ((Decal.Adapter.Wrappers.ListWrapper)myList.myControl)[myRow][myCol].Width = value;
                }
            }

            public object this[int subval]
            {
                get
                {
                    return ((Decal.Adapter.Wrappers.ListWrapper)myList.myControl)[myRow][myCol][subval];
                }
                set
                {
                    ((Decal.Adapter.Wrappers.ListWrapper)myList.myControl)[myRow][myCol][subval] = value;
                }
            }

            #endregion
        }

    }
    internal class StaticText : Control, IStaticText
    {

        #region IStaticText Members

        public string Text
        {
            get
            {
                return ((Decal.Adapter.Wrappers.StaticWrapper)myControl).Text;
            }
            set
            {
                ((Decal.Adapter.Wrappers.StaticWrapper)myControl).Text = value;
            }
        }

#pragma warning disable 0067
        public event EventHandler Click;
#pragma warning restore 0067

        #endregion
    }
    internal class Notebook : Control, INotebook
    {
        public override void Initialize()
        {
            base.Initialize();
            ((Decal.Adapter.Wrappers.NotebookWrapper)myControl).Change += new EventHandler<Decal.Adapter.IndexChangeEventArgs>(Notebook_Change);
        }

        public override void Dispose()
        {
            base.Dispose();
            ((Decal.Adapter.Wrappers.NotebookWrapper)myControl).Change -= new EventHandler<Decal.Adapter.IndexChangeEventArgs>(Notebook_Change);
        }

        void Notebook_Change(object sender, Decal.Adapter.IndexChangeEventArgs e)
        {
            if (Change != null)
                Change(this, null);
        }

        #region INotebook Members

        public event EventHandler Change;

        public int ActiveTab
        {
            get
            {
                return ((Decal.Adapter.Wrappers.NotebookWrapper)myControl).ActiveTab;
            }
            set
            {
                ((Decal.Adapter.Wrappers.NotebookWrapper)myControl).ActiveTab = value;
            }
        }

        #endregion
    }
    internal class ProgressBar : Control, IProgressBar
    {

        #region IProgressBar Members

        public int Position
        {
            get
            {
                return ((Decal.Adapter.Wrappers.ProgressWrapper)myControl).Value;
            }
            set
            {
                ((Decal.Adapter.Wrappers.ProgressWrapper)myControl).Value = value;
            }
        }

        public string PreText
        {
            get
            {
                return ((Decal.Adapter.Wrappers.ProgressWrapper)myControl).PreText;
            }
            set
            {
                ((Decal.Adapter.Wrappers.ProgressWrapper)myControl).PreText = value;
            }
        }

        #endregion
    }
}

