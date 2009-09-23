///////////////////////////////////////////////////////////////////////////////
//File: Form1.cs
//
//Description: The DecalPluginOrderer program. This is a simple program to change
//  the order of Decal Plugins in a manner that will not confuse DenAgent. This
//  means that, upon save, each plugin will have one and only one Order key and
//  that key will be in the Plugins Order keystring.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DecalPluginOrderer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<cPlugin> plugins = new List<cPlugin>();
        List<char> usedkeys = new List<char>();

        char GetNewOrderKey()
        {
            char n = 'A';

            while (usedkeys.Contains(n))
                n++;

            usedkeys.Add(n);
            return n;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey pkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Decal\\Plugins");

            //Read in plugin data in GUID order
            foreach (string k in pkey.GetSubKeyNames())
            {
                cPlugin curplg = new cPlugin();
                curplg.GUID = k;

                Microsoft.Win32.RegistryKey mykey = pkey.OpenSubKey(k);

                curplg.orderkey = ((string)mykey.GetValue("Order", "."))[0];
                curplg.Name = (string)mykey.GetValue("", ".");

                plugins.Add(curplg);

                if (!usedkeys.Contains(curplg.orderkey))
                    usedkeys.Add(curplg.orderkey);
            }

            //Switch to keyed order
            List<cPlugin> keyedorderplugins = new List<cPlugin>();
            string sorder = (string)pkey.GetValue("Order", "");
            while (sorder.Length > 0)
            {
                //Read plugins with this key
                bool gotone = false;
                for (int i = plugins.Count - 1; i >= 0; --i)
                {
                    if (plugins[i].orderkey == sorder[0])
                    {
                        keyedorderplugins.Add(plugins[i]);
                        plugins.RemoveAt(i);

                        if (!gotone)
                            gotone = true;
                        else
                            keyedorderplugins[keyedorderplugins.Count - 1].orderkey = GetNewOrderKey();
                    }
                }

                sorder = sorder.Substring(1);
            }

            //Read plugins with no matching key in keystring
            for (int i = plugins.Count - 1; i >= 0; --i)
            {
                keyedorderplugins.Add(plugins[i]);
                plugins.RemoveAt(i);

                keyedorderplugins[keyedorderplugins.Count - 1].orderkey = GetNewOrderKey();
            }

            plugins = keyedorderplugins;
            foreach (cPlugin p in plugins)
                listBox1.Items.Add(p.Name);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex <= 0) return;
            int i = listBox1.SelectedIndex;

            cPlugin pl = plugins[i];
            plugins.RemoveAt(i);
            plugins.Insert(i - 1, pl);

            listBox1.Items.RemoveAt(i);
            listBox1.Items.Insert(i - 1, pl.Name);

            listBox1.SelectedIndex = i - 1;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= plugins.Count) return;
            int i = listBox1.SelectedIndex;

            cPlugin pl = plugins[i];
            plugins.RemoveAt(i);
            plugins.Insert(i + 1, pl);

            listBox1.Items.RemoveAt(i);
            listBox1.Items.Insert(i + 1, pl.Name);

            listBox1.SelectedIndex = i + 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey pkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Decal\\Plugins", true);

            //Write order keys
            foreach (cPlugin plg in plugins)
                pkey.OpenSubKey(plg.GUID, true).SetValue("Order", plg.orderkey.ToString(), Microsoft.Win32.RegistryValueKind.String);
            
            //Write order string
            StringBuilder sb = new StringBuilder();
            foreach (cPlugin plg in plugins)
                sb.Append(plg.orderkey);
            pkey.SetValue("Order", sb.ToString(), Microsoft.Win32.RegistryValueKind.String);
        }
    }

    class cPlugin
    {
        public string Name;
        public string GUID;
        public char orderkey;
    }
}