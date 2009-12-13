///////////////////////////////////////////////////////////////////////////////
//File: LootRules.cs
//
//Description: This is the core of Virindi Tank looting, without the ingame parts.
//  Each of the rule and requirement classes reads and writes to the hackish
//  file format. After looking through this, you will probably understand why I
//  haven't done a lot with looting.
//
//This file is Copyright (c) 2008 VirindiPlugins
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
using System.Windows.Forms;

namespace uTank2
{
    internal interface iSettingsCollection
    {
        void Read(System.IO.StreamReader inf);
        void Write(System.IO.StreamWriter inf);
    }
    internal class cUniqueID: IComparable<cUniqueID>
    {
        static long last = 0;
        public static cUniqueID New(int t, TreeNode n)
        {
            return new cUniqueID(++last, t, n);
        }

        public TreeNode node;
        public int type;
        long v;
        cUniqueID(long z, int t, TreeNode n)
        {
            node = n;
            type = t;
            v = z;
        }

        #region IComparable<cUniqueID> Members

        public int CompareTo(cUniqueID other)
        {
            return v.CompareTo(other);
        }

        #endregion
    }
    namespace LootRules
    {
        //A single rule
        internal interface iLootRule
        {
            int GetRuleType();
            void Read(System.IO.StreamReader inf);
            void Write(System.IO.StreamWriter inf);
            void SetID(cUniqueID ttnn);
            cUniqueID GetID();
            string DisplayString();
        }

        internal class SpellNameMatch : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) {tn=ttnn;} public cUniqueID GetID() {return tn;}
            public System.Text.RegularExpressions.Regex rx;

            public SpellNameMatch() { rx = new System.Text.RegularExpressions.Regex(""); }
            public SpellNameMatch(System.Text.RegularExpressions.Regex k) { rx = k; }

            #region iLootRule Members

            public int GetRuleType() { return 0; }

            public string DisplayString() { return "SpellNameMatch: " + rx.ToString(); }

            public void Read(System.IO.StreamReader inf)
            {
                rx = new System.Text.RegularExpressions.Regex(inf.ReadLine());
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(rx.ToString());
            }

            #endregion
        }
        internal class StringValueMatch : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) {tn=ttnn;} public cUniqueID GetID() {return tn;}
            public System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("");
            public Decal.Adapter.Wrappers.StringValueKey vk = Decal.Adapter.Wrappers.StringValueKey.FullDescription;

            public StringValueMatch() { }
            public StringValueMatch(System.Text.RegularExpressions.Regex k, Decal.Adapter.Wrappers.StringValueKey v) { rx = k; vk = v; }

            #region iLootRule Members

            public int GetRuleType() { return 1; }

            public string DisplayString() { return "StringValueMatch: " + rx.ToString(); }

            public void Read(System.IO.StreamReader inf)
            {
                rx = new System.Text.RegularExpressions.Regex(inf.ReadLine());
                vk = (Decal.Adapter.Wrappers.StringValueKey)Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(rx.ToString());
                inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
            }

            #endregion
        }
        internal class LongValKeyLE : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) {tn=ttnn;} public cUniqueID GetID() {return tn;}
            public int keyval;
            public Decal.Adapter.Wrappers.LongValueKey vk = Decal.Adapter.Wrappers.LongValueKey.ActivationReqSkillId;

            public LongValKeyLE() { }
            public LongValKeyLE(int k, Decal.Adapter.Wrappers.LongValueKey v) { keyval = k; vk = v; }

            #region iLootRule Members

            public int GetRuleType() { return 2; }

            public string DisplayString() { return string.Format("{0} <= {1}", vk, keyval); }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                vk = (Decal.Adapter.Wrappers.LongValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
                inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
            }

            #endregion
        }
        internal class LongValKeyGE : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) {tn=ttnn;} public cUniqueID GetID() {return tn;}
            public int keyval;
            public Decal.Adapter.Wrappers.LongValueKey vk = Decal.Adapter.Wrappers.LongValueKey.ActivationReqSkillId;

            public LongValKeyGE() { }
            public LongValKeyGE(int k, Decal.Adapter.Wrappers.LongValueKey v) { keyval = k; vk = v; }

            #region iLootRule Members

            public int GetRuleType() { return 3; }

            public string DisplayString() { return string.Format("{0} >= {1}", vk, keyval); }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                vk = (Decal.Adapter.Wrappers.LongValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
                inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
            }

            #endregion
        }
        internal class DoubleValKeyLE : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) {tn=ttnn;} public cUniqueID GetID() {return tn;}
            public double keyval;
            public Decal.Adapter.Wrappers.DoubleValueKey vk = Decal.Adapter.Wrappers.DoubleValueKey.AcidProt;

            public DoubleValKeyLE() { }
            public DoubleValKeyLE(double k, Decal.Adapter.Wrappers.DoubleValueKey v) { keyval = k; vk = v; }

            #region iLootRule Members

            public int GetRuleType() { return 4; }

            public string DisplayString() { return string.Format("{0} <= {1}", vk, keyval); }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = Convert.ToDouble(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                vk = (Decal.Adapter.Wrappers.DoubleValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
                inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
            }

            #endregion
        }
        internal class DoubleValKeyGE : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) {tn=ttnn;} public cUniqueID GetID() {return tn;}
            public double keyval;
            public Decal.Adapter.Wrappers.DoubleValueKey vk = Decal.Adapter.Wrappers.DoubleValueKey.AcidProt;

            public DoubleValKeyGE() { }
            public DoubleValKeyGE(double k, Decal.Adapter.Wrappers.DoubleValueKey v) { keyval = k; vk = v; }

            #region iLootRule Members

            public int GetRuleType() { return 5; }

            public string DisplayString() { return string.Format("{0} >= {1}", vk, keyval); }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = Convert.ToDouble(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                vk = (Decal.Adapter.Wrappers.DoubleValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
                inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
            }

            #endregion
        }
        internal class DamagePercentGE : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) {tn=ttnn;} public cUniqueID GetID() {return tn;}
            public double keyval;

            public DamagePercentGE() { }
            public DamagePercentGE(double k) { keyval = k; }

            #region iLootRule Members

            public int GetRuleType() { return 6; }

            public string DisplayString() { return string.Format("DamagePercentGE >= {0}", keyval); }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = Convert.ToDouble(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            }

            #endregion
        }
        internal class ObjectClass : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) { tn = ttnn; } public cUniqueID GetID() { return tn; }
            public Decal.Adapter.Wrappers.ObjectClass vk = Decal.Adapter.Wrappers.ObjectClass.Armor;

            public ObjectClass() { }
            public ObjectClass(Decal.Adapter.Wrappers.ObjectClass v) { vk = v; }

            #region iLootRule Members

            public int GetRuleType() { return 7; }

            public string DisplayString() { return string.Format("ObjectClass = {0}", vk); }

            public void Read(System.IO.StreamReader inf)
            {
                vk = (Decal.Adapter.Wrappers.ObjectClass)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
            }

            #endregion
        }
        internal class SpellCountGE : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) { tn = ttnn; } public cUniqueID GetID() { return tn; }
            public int keyval;

            public SpellCountGE() { }
            public SpellCountGE(int k) { keyval = k; }

            #region iLootRule Members

            public int GetRuleType() { return 8; }

            public string DisplayString() { return string.Format("SpellCount >= {0}", keyval); }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            }

            #endregion
        }
    }

    //A set of rules with an action attached
    internal class cLootItemRule : iSettingsCollection
    {
        public List<LootRules.iLootRule> IntRules = new List<uTank2.LootRules.iLootRule>();
        int pri;
        public eLootAction act;
        public string name;

        public cUniqueID tn; public void SetID(cUniqueID ttnn) {tn=ttnn;} public cUniqueID GetID() {return tn;}

        public int Priority() { return pri; }

        #region iSettingsCollection Members

        public void Read(System.IO.StreamReader inf)
        {
            name = inf.ReadLine();
            string[] clines = inf.ReadLine().Split(new char[] { ';' });
            //Priority, Action

            pri = Convert.ToInt32(clines[0], System.Globalization.CultureInfo.InvariantCulture);
            act = (eLootAction)Convert.ToInt32(clines[1], System.Globalization.CultureInfo.InvariantCulture);

            //Rules
            IntRules.Clear();
            for (int i = 2; i < clines.Length; ++i)
            {
                int ruletype = Convert.ToInt32(clines[i], System.Globalization.CultureInfo.InvariantCulture);
                LootRules.iLootRule newrule;
                switch (ruletype)
                {
                    case 0: newrule = new LootRules.SpellNameMatch(); break;
                    case 1: newrule = new LootRules.StringValueMatch(); break;
                    case 2: newrule = new LootRules.LongValKeyLE(); break;
                    case 3: newrule = new LootRules.LongValKeyGE(); break;
                    case 4: newrule = new LootRules.DoubleValKeyLE(); break;
                    case 5: newrule = new LootRules.DoubleValKeyGE(); break;
                    case 6: newrule = new LootRules.DamagePercentGE(); break;
                    case 7: newrule = new LootRules.ObjectClass(); break;
                    case 8: newrule = new LootRules.SpellCountGE(); break;
                    default: newrule = null; break;
                }
                newrule.Read(inf);
                IntRules.Add(newrule);
            }
        }

        public void Write(System.IO.StreamWriter inf)
        {
            inf.WriteLine(name);
            StringBuilder s = new StringBuilder();
            s.Append(Convert.ToString(pri, System.Globalization.CultureInfo.InvariantCulture));
            s.Append(";");
            s.Append(Convert.ToString((int)act, System.Globalization.CultureInfo.InvariantCulture));
            foreach (LootRules.iLootRule lr in IntRules)
            {
                s.Append(";");
                s.Append(Convert.ToString(lr.GetRuleType(), System.Globalization.CultureInfo.InvariantCulture));
            }
            inf.WriteLine(s.ToString());
            foreach (LootRules.iLootRule lr in IntRules)
                lr.Write(inf);
        }

        #endregion
    }

    internal enum eLootAction
    {
        NoLoot = 0,
        Keep = 1,
        Salvage = 2,
        Sell = 3,
        Read = 4
    }

    internal class cLootRules : iSettingsCollection
    {
        public cUniqueID tn; public void SetID(cUniqueID ttnn) { tn = ttnn; } public cUniqueID GetID() { return tn; }
        public List<cLootItemRule> Rules = new List<cLootItemRule>();

        #region iSettingsCollection Members

        public void Read(System.IO.StreamReader inf)
        {
            Rules.Clear();
            int count = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            for (int i=0;i<count;++i)
            {
                cLootItemRule R = new cLootItemRule();
                R.Read(inf);
                Rules.Add(R);
            }
        }

        public void Write(System.IO.StreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(Rules.Count, System.Globalization.CultureInfo.InvariantCulture));
            foreach (cLootItemRule R in Rules)
                R.Write(inf);
        }

        #endregion
    }
}
