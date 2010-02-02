///////////////////////////////////////////////////////////////////////////////
//File: MyDialog_StringQuery.cs
//
//Description: A derived dialog class to query the user for a single string value.
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

#if METAVIEW_PUBLIC_NS
using MetaViewWrappers;
#else
using MyClasses.MetaViewWrappers;
#endif

namespace MyClasses
{
    class MyDialog_StringQuery : MyDialog<string>
    {
        string[] iButtons;
        string iTitle;
        string iLabel;
        int iIcon;
        delInputVerify iVerify;
        string iDefaultInput;

        public delegate string[] delInputVerify(string input);

        IButton[] ResultButtons;
        ITextBox Text;
        IStaticText Label;

        public MyDialog_StringQuery(Decal.Adapter.Wrappers.PluginHost H, string[] pbuttons, string ptitle, string plabel, int picon, delInputVerify pVerify, string pdefaultinput, string dialogkey, IView pOrigin)
            : base(dialogkey, pOrigin)
        {
            iButtons = pbuttons;
            iTitle = ptitle;
            iLabel = plabel;
            iIcon = picon;
            iVerify = pVerify;
            iDefaultInput = pdefaultinput;

            if (!base.ShowView(H)) return;

            ResultButtons = new IButton[iButtons.Length];
            for (int i = 0; i < iButtons.Length; ++i)
            {
                ResultButtons[i] = (IButton)View["btn" + i.ToString()];
                ResultButtons[i].Hit += new EventHandler(MyDialog_StringQuery_Hit);
            }
            Text = (ITextBox)View["Textbox1"];
            Text.Text = iDefaultInput;
            Label = (IStaticText)View["Label1"];
            Label.Text = iLabel;

#if VVS_REFERENCED
            if (View.ViewType == ViewSystemSelector.eViewSystem.VirindiViewService)
                SetStrVVSProps();
#endif
        }

#if VVS_REFERENCED
        void SetStrVVSProps()
        {
            VirindiViewService.HudView v = ((MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.View)View).Underlying;
            VirindiViewService.Controls.HudTextBox tb = (VirindiViewService.Controls.HudTextBox)v.Controls["Textbox1"];
            VirindiViewService.HudView.FocusControl = tb;
        }
#endif

        void MyDialog_StringQuery_Hit(object sender, EventArgs e)
        {
            IButton me = (IButton)sender;
            int ind = int.Parse(me.Name.Substring(3));

            if (CanPressButton(ind))
            {
                base.ReturnDialog(me.Text, Text.Text);
            }
        }

        /*
        int GetButtonIndex(string b)
        {
            for (int i = 0; i < iButtons.Length; ++i)
                if (b == iButtons[i])
                    return i;
            return -1;
        }
        */

        bool CanPressButton(int b)
        {
            if (iVerify == null) return true;

            string[] possible = iVerify(Text.Text);
            foreach (string p in possible)
            {
                if (p == iButtons[b])
                    return true;
            }
            return false;
        }

        protected override string GetDialogXML()
        {
            StringBuilder b = new StringBuilder();

            b.Append("<?xml version=\"1.0\"?>");
            b.Append("<view icon=\"" + iIcon + "\" title=\"" + iTitle + "\" width=\"296\" height=\"116\">");
            b.Append("<control progid=\"DecalControls.FixedLayout\" clipped=\"\">");
            b.Append("<control progid=\"DecalControls.StaticText\" name=\"Label1\" left=\"18\" top=\"8\" width=\"260\" height=\"16\" text=\"Label1\"/>");
            b.Append("<control progid=\"DecalControls.Edit\" name=\"Textbox1\" left=\"18\" top=\"24\" width=\"260\" height=\"16\" imageportalsrc=\"4726\" text=\"\"/>");

            for (int i = 0; i < iButtons.Length; ++i)
            {
                b.Append("<control progid=\"DecalControls.PushButton\" name=\"btn" + i.ToString() + "\" left=\"" + (224 - i * 72).ToString() + "\" top=\"52\" width=\"64\" height=\"24\" text=\"" + iButtons[i] + "\"/>");
            }

            b.Append("</control>");
            b.Append("</view>");

            return b.ToString();
        }
    }
}