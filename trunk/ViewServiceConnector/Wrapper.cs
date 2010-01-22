///////////////////////////////////////////////////////////////////////////////
//File: Wrapper.cs
//
//Description: Contains the interface definitions for the MetaViewWrappers classes.
//
//References required:
//  System.Drawing
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

#if METAVIEW_PUBLIC_NS
namespace MetaViewWrappers
#else
namespace MyClasses.MetaViewWrappers
#endif
{
#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    delegate void dClickedList(object sender, int row, int col);

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IView: IDisposable
    {
        void Initialize(Decal.Adapter.Wrappers.PluginHost p, string pXML);
        void InitializeRawXML(Decal.Adapter.Wrappers.PluginHost p, string pXML);

        void SetIcon(int icon, int iconlibrary);
        void SetIcon(int portalicon);

        string Title { get; set; }
        bool Visible { get; set; }
#if !VVS_WRAPPERS_PUBLIC
        ViewSystemSelector.eViewSystem ViewType { get; }
#endif

        System.Drawing.Point Location { get; set;}

        IControl this[string id] { get; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IControl : IDisposable
    {
        string Name { get; }
        bool Visible { get; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IButton : IControl
    {
        string Text { get; set; }
        event EventHandler Hit;
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface ICheckBox : IControl
    {
        string Text { get; set; }
        bool Checked { get; set; }
        event EventHandler Change;
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface ITextBox : IControl
    {
        string Text { get; set; }
        event EventHandler Change;
        event EventHandler End;
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface ICombo : IControl
    {
        IComboIndexer Text { get; }
        int Count { get; }
        int Selected { get; set; }
        event EventHandler Change;
        void Add(string text);
        void Insert(int index, string text);
        void RemoveAt(int index);
        void Clear();
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IComboIndexer
    {
        string this[int index] { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface ISlider : IControl
    {
        int Position { get; set; }
        event EventHandler Change;
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IList : IControl
    {
        event dClickedList Click;
        void Clear();
        IListRow this[int row] { get; }
        IListRow AddRow();
        IListRow InsertRow(int pos);
        int RowCount { get; }
        void RemoveRow(int index);
        int ColCount { get; }
        int ScrollPosition { get; set;}
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IListRow
    {
        IListCell this[int col] { get; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IListCell
    {
        System.Drawing.Color Color { get; set; }
        int Width { get; set; }
        object this[int subval] { get; set; }
        void ResetColor();
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IStaticText : IControl
    {
        string Text { get; set; }
        event EventHandler Click;
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface INotebook : IControl
    {
        event EventHandler Change;
        int ActiveTab { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IProgressBar : IControl
    {
        int Position { get; set; }
        string PreText { get; set; }
    }
}
