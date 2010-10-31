///////////////////////////////////////////////////////////////////////////////
//File: MyDialog_ColorQuery.cs
//
//Description: A derived dialog class to query the user for a color value.
//  The dialog presents the user with 4 sliders (A, R, G, B) and a preview box
//  with the current color. In VVS the preview box is a HudPictureBox control,
//  and in Decal Views it is a blank PushButton control.
//
//This file is Copyright (c) 2010 VirindiPlugins
//
//References required:
//  System.Drawing
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
    class MyDialog_ColorQuery : MyDialog<Color>
    {
        string[] iButtons;
        string iTitle;
        string iLabel;
        int iIcon;
        Color iDefaultInput;


        IButton[] ResultButtons;
        //ITextBox Text;
        IStaticText Label;
        ISlider slA;
        ISlider slR;
        ISlider slG;
        ISlider slB;

        Rectangle ColorBoxRect = new Rectangle(223, 42, 60, 60);

        bool VVSColorBox = false;
#if VVS_REFERENCED
        VirindiViewService.Controls.HudPictureBox vColorBox;
#endif
        Decal.Adapter.Wrappers.PushButtonWrapper dColorBox;

        public MyDialog_ColorQuery(Decal.Adapter.Wrappers.PluginHost H, string[] pbuttons, string ptitle, string plabel, int picon, Color pdefaultinput, string dialogkey, IView pOrigin)
            : base(dialogkey, pOrigin)
        {
            iButtons = pbuttons;
            iTitle = ptitle;
            iLabel = plabel;
            iIcon = picon;
            iDefaultInput = pdefaultinput;

            if (!base.ShowView(H)) return;

            ResultButtons = new IButton[iButtons.Length];
            for (int i = 0; i < iButtons.Length; ++i)
            {
                ResultButtons[i] = (IButton)View["btn" + i.ToString()];
                ResultButtons[i].Hit += new EventHandler(MyDialog_ColorQuery_Hit);
            }
            Label = (IStaticText)View["Label1"];
            Label.Text = iLabel;

            //Set up input boxes
            slA = (ISlider)View["slA"];
            slR = (ISlider)View["slR"];
            slG = (ISlider)View["slG"];
            slB = (ISlider)View["slB"];

            slA.Position = iDefaultInput.A;
            slR.Position = iDefaultInput.R;
            slG.Position = iDefaultInput.G;
            slB.Position = iDefaultInput.B;

            slA.Change += new EventHandler<MVIndexChangeEventArgs>(ChangedSlider);
            slR.Change += new EventHandler<MVIndexChangeEventArgs>(ChangedSlider);
            slG.Change += new EventHandler<MVIndexChangeEventArgs>(ChangedSlider);
            slB.Change += new EventHandler<MVIndexChangeEventArgs>(ChangedSlider);

#if VVS_REFERENCED
            if (VVSColorBox)
            {
                //Create the VVS colorbox
                Curtain_CreateVVSColorBox();
            }
#endif
            if (!VVSColorBox)
            {
                dColorBox = (Decal.Adapter.Wrappers.PushButtonWrapper)(((MyClasses.MetaViewWrappers.DecalControls.View)View).Underlying.Controls["ColorBox"]);
                dColorBox.FaceColor = iDefaultInput;
            }
        }

        void Curtain_CreateVVSColorBox()
        {
            vColorBox = new VirindiViewService.Controls.HudPictureBox();
            VirindiViewService.Controls.HudFixedLayout vlayout = (VirindiViewService.Controls.HudFixedLayout)((MyClasses.MetaViewWrappers.VirindiViewServiceHudControls.View)View).Underlying.Controls["mainlayout"];
            vlayout.AddControl(vColorBox, ColorBoxRect);
            vColorBox.Image = new VirindiViewService.ACImage(iDefaultInput);
        }

        void ChangedSlider(object sender, MVIndexChangeEventArgs e)
        {
            Color cur = CurrentInputColor;
            SetColorBox(cur);
        }

        void SetColorBox(Color c)
        {
#if VVS_REFERENCED
            if (VVSColorBox)
            {
                Curtain_SetColorBox(c);
            }
#endif
            if (!VVSColorBox)
            {
                dColorBox.FaceColor = c;
            }
        }

        void Curtain_SetColorBox(Color c)
        {
            vColorBox.Image = new VirindiViewService.ACImage(c);
        }

        Color CurrentInputColor
        {
            get
            {
                int a = slA.Position;
                int r = slR.Position;
                int g = slG.Position;
                int b = slB.Position;
                return Color.FromArgb(a, r, g, b);
            }
        }

        void MyDialog_ColorQuery_Hit(object sender, EventArgs e)
        {
            IButton me = (IButton)sender;
            int ind = int.Parse(me.Name.Substring(3));

            base.ReturnDialog(me.Text, CurrentInputColor);
        }

        protected override string GetDialogXML()
        {
            StringBuilder b = new StringBuilder();

            b.Append("<?xml version=\"1.0\"?>");
            b.Append("<view icon=\"" + iIcon + "\" title=\"" + iTitle + "\" width=\"296\" height=\"180\">");
            b.Append("<control progid=\"DecalControls.FixedLayout\" name=\"mainlayout\" clipped=\"\">");
            b.Append("<control progid=\"DecalControls.StaticText\" name=\"Label1\" left=\"8\" top=\"8\" width=\"260\" height=\"16\" text=\"Label1\"/>");

            b.Append("<control progid=\"DecalControls.StaticText\" name=\"lblA\" left=\"8\" top=\"34\" width=\"60\" height=\"16\" text=\"Alpha:\"/>");
            b.Append("<control progid=\"DecalControls.StaticText\" name=\"lblR\" left=\"8\" top=\"54\" width=\"60\" height=\"16\" text=\"Red:\"/>");
            b.Append("<control progid=\"DecalControls.StaticText\" name=\"lblG\" left=\"8\" top=\"74\" width=\"60\" height=\"16\" text=\"Green:\"/>");
            b.Append("<control progid=\"DecalControls.StaticText\" name=\"lblB\" left=\"8\" top=\"94\" width=\"60\" height=\"16\" text=\"Blue:\"/>");

            b.Append("<control progid=\"DecalControls.Slider\" name=\"slA\" left=\"60\" top=\"34\" width=\"150\" height=\"16\" minimum=\"0\" maximum=\"255\" textcolor=\"0\" vertical=\"0\"/>");
            b.Append("<control progid=\"DecalControls.Slider\" name=\"slR\" left=\"60\" top=\"54\" width=\"150\" height=\"16\" minimum=\"0\" maximum=\"255\" textcolor=\"0\" vertical=\"0\"/>");
            b.Append("<control progid=\"DecalControls.Slider\" name=\"slG\" left=\"60\" top=\"74\" width=\"150\" height=\"16\" minimum=\"0\" maximum=\"255\" textcolor=\"0\" vertical=\"0\"/>");
            b.Append("<control progid=\"DecalControls.Slider\" name=\"slB\" left=\"60\" top=\"94\" width=\"150\" height=\"16\" minimum=\"0\" maximum=\"255\" textcolor=\"0\" vertical=\"0\"/>");

            bool outputbox = false;
#if VVS_REFERENCED
            if (ViewSystemSelector.IsPresent(null, ViewSystemSelector.eViewSystem.VirindiViewService))
            {
                outputbox = true;
                //We are using the VVS colorbox
                VVSColorBox = true;
            }
#endif
            if (!outputbox)
            {
                //We are using the Decal colorbox
                b.Append("<control progid=\"DecalControls.PushButton\" name=\"ColorBox\" left=\"" + ColorBoxRect.Left.ToString() + "\" top=\"" + ColorBoxRect.Top.ToString() + "\" width=\"" + ColorBoxRect.Width.ToString() + "\" height=\"" + ColorBoxRect.Height.ToString() + "\" text=\"\"/>");
            }

            for (int i = 0; i < iButtons.Length; ++i)
            {
                b.Append("<control progid=\"DecalControls.PushButton\" name=\"btn" + i.ToString() + "\" left=\"" + (224 - i * 72).ToString() + "\" top=\"124\" width=\"64\" height=\"24\" text=\"" + iButtons[i] + "\"/>");
            }

            b.Append("</control>");
            b.Append("</view>");

            return b.ToString();
        }
    }
}