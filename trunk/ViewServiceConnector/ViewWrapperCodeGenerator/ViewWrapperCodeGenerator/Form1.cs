using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace ViewWrapperCodeGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamReader myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = new StreamReader(openFileDialog1.OpenFile())) != null)
                    {
                        using (myStream)
                        {
                            viewobjs.Clear();
                            viewtypes.Clear();
                            textBox1.Text = "";

                            string t = myStream.ReadToEnd();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(t);
                            Parse(doc);
                            Print();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        List<string> viewobjs = new List<string>();
        List<string> viewtypes = new List<string>();
        void Parse(XmlDocument doc)
        {
            RecurseParse(doc.DocumentElement.FirstChild);
        }
        string attr(System.Xml.XmlAttribute a)
        {
            if (a == null) return "";
            if (a.Value == null) return "";
            return a.Value;
        }
        void RecurseParse(XmlNode z)
        {
            string typ = "";
            string name = attr(z.Attributes["name"]);

            switch (attr(z.Attributes["progid"]))
            {
                case "DecalControls.FixedLayout":
                    typ = "";
                    foreach (XmlNode child in z.ChildNodes)
                        RecurseParse(child);
                    break;
                case "DecalControls.Notebook":
                    typ = "INotebook";
                    foreach (XmlNode child in z.ChildNodes)
                        if (child.Name == "page")
                            foreach (XmlNode child2 in child.ChildNodes)
                                RecurseParse(child2);
                    break;
                case "DecalControls.PushButton":
                    typ = "IButton";
                    break;
                case "DecalControls.Checkbox":
                    typ = "ICheckBox";
                    break;
                case "DecalControls.Edit":
                    typ = "ITextBox";
                    break;
                case "DecalControls.StaticText":
                    typ = "IStaticText";
                    break;
                case "DecalControls.List":
                    typ = "IList";
                    break;
                case "DecalControls.Slider":
                    typ = "ISlider";
                    break;
                case "DecalControls.Progress":
                    typ = "IProgressBar"; 
                    break;
                case "DecalControls.Choice":
                    typ = "ICombo";
                    break;
            }
            if (name == "") return;
            if (typ == "") return;

            viewobjs.Add(name);
            viewtypes.Add(typ);
        }
        string NS = "MyClasses.MetaViewWrappers.";
        void PrintNS(StringBuilder b, string c)
        {
            b.Append("static " + NS);
            b.Append(c);
        }
        void Print()
        {
            StringBuilder b = new StringBuilder();

            b.Append("#region Auto-generated view code\n");
            PrintNS(b, "IView View;\n");
            for (int i = 0; i < viewobjs.Count; ++i)
                PrintNS(b, viewtypes[i] + " " + viewobjs[i] + ";\n");
            b.Append("\n");
            b.Append("static void ViewInit()\n");
            b.Append("{\n");
            b.Append("//Create view here\n");
            b.Append("//View = MyClasses.MetaViewWrappers.ViewSystemSelector.CreateViewResource(...);\n");
            for (int i = 0; i < viewobjs.Count; ++i)
                b.Append(viewobjs[i] + " = (" + NS + viewtypes[i] + ")View[\"" + viewobjs[i] + "\"];\n");
            b.Append("}\n");
            b.Append("\n");
            b.Append("static void ViewDestroy()\n");
            b.Append("{\n");
            for (int i = 0; i < viewobjs.Count; ++i)
                b.Append(viewobjs[i] + " = null;\n");
            b.Append("View.Dispose();\n");
            b.Append("}\n");
            b.Append("#endregion Auto-generated view code\n");
            //b.Append("\n");

            textBox1.Text = b.ToString();
        }
    }
}