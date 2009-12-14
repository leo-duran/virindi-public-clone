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
using Decal.Adapter.Wrappers;

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
            bool requiresID();
        }

        internal static class GameInfo
        {
            public static double HaxConvertDouble(string s)
            {
                string ss = s.Replace(',', '.');
                double res;
                if (!double.TryParse(ss, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out res))
                    return 0d;
                return res;
            }

            public static bool IsIDProperty(DoubleValueKey vk)
            {
                switch (vk)
                {
                    case DoubleValueKey.ApproachDistance: return false;
                    case DoubleValueKey.SalvageWorkmanship: return false;
                    default: return true;
                }
            }

            public static bool IsIDProperty(LongValueKey vk)
            {
                switch (vk)
                {
                    case LongValueKey.CreateFlags1: return false;
                    case LongValueKey.Type: return false;
                    case LongValueKey.Icon: return false;
                    case LongValueKey.Category: return false;
                    case LongValueKey.Behavior: return false;
                    case LongValueKey.CreateFlags2: return false;
                    case LongValueKey.IconUnderlay: return false;
                    case LongValueKey.ItemSlots: return false;
                    case LongValueKey.PackSlots: return false;
                    case LongValueKey.MissileType: return false;
                    case LongValueKey.Material: return false;
                    case LongValueKey.Value: return false;
                    case LongValueKey.Unknown10: return false;
                    case LongValueKey.UsageMask: return false;
                    case LongValueKey.IconOutline: return false;
                    case LongValueKey.EquipType: return false;
                    case LongValueKey.UsesRemaining: return false;
                    case LongValueKey.UsesTotal: return false;
                    case LongValueKey.StackCount: return false;
                    case LongValueKey.StackMax: return false;
                    case LongValueKey.Container: return false;
                    case LongValueKey.Slot: return false;
                    case LongValueKey.EquipableSlots: return false;
                    case LongValueKey.EquippedSlots: return false;
                    case LongValueKey.Coverage: return false;
                    case LongValueKey.Unknown100000: return false;
                    case LongValueKey.Unknown800000: return false;
                    case LongValueKey.Unknown8000000: return false;
                    //case LongValueKey.Burden: return false;
                    case LongValueKey.Monarch: return false;
                    case LongValueKey.HookMask: return false;
                    case LongValueKey.IconOverlay: return false;
                    default: return true;
                }
            }

            public static SortedDictionary<string, int> getSetInfo()
            {
                int i = 32;
                SortedDictionary<string, int> r = new SortedDictionary<string, int>();
                r.Add("Protective Clothing", i--);
                r.Add("Gladiatorial Clothing", i--);
                r.Add("Dedication", i--);
                r.Add("Lightning Proof", i--);
                r.Add("Cold Proof", i--);
                r.Add("Acid Proof", i--);
                r.Add("Flame Proof", i--);
                r.Add("Interlocking", i--);
                r.Add("Reinforced", i--);
                r.Add("Hardenend", i--);
                r.Add("Swift", i--);
                r.Add("Wise", i--);
                r.Add("Dexterous", i--);
                r.Add("Hearty", i--);
                r.Add("Crafter's", i--);
                r.Add("Tinker's", i--);
                r.Add("Defender's", i--);
                r.Add("Archer's", i--);
                r.Add("Adept's", i--);
                r.Add("Soldier's", i--);
                r.Add("Leggings of Perfect Light", i--);
                r.Add("Coat of the Perfect Light", i--);
                r.Add("Arm, Mind, Heart", i--);
                r.Add("Empyrean Rings", i--);
                r.Add("Shou-jen", i--);
                r.Add("Relic Alduressa", i--);
                r.Add("Ancient Relic", i--);
                r.Add("Noble Relic", i--);

                return r;
            }

            public static SortedDictionary<string, int> getSkillInfo()
            {
                SortedDictionary<string, int> r = new SortedDictionary<string, int>();
                r.Add("Axe", 1);
                r.Add("Bow", 2);
                r.Add("Crossbow", 3);
                r.Add("Dagger", 4);
                r.Add("Mace", 5);
                r.Add("MeleeD", 6);
                r.Add("MissileD", 7);
                r.Add("Spear", 9);
                r.Add("Staff", 10);
                r.Add("Sword", 11);
                r.Add("Thrown Weapons", 12);
                r.Add("Unarmed Combat", 13);
                r.Add("MagicD", 15);
                r.Add("ManaCon", 16);
                r.Add("Creature", 31);
                r.Add("Item", 32);
                r.Add("Life", 33);
                r.Add("War", 34);
                r.Add("Two-Handed Combat", 41);

                return r;
            }

            public static SortedDictionary<string, int> getMaterialInfo()
            {
                SortedDictionary<string, int> r = new SortedDictionary<string, int>();
                r.Add("Agate", 10);
                r.Add("Alabaster", 66);
                r.Add("Amber", 11);
                r.Add("Amethyst", 12);
                r.Add("Aquamarine", 13);
                r.Add("Armoredillo Hide", 53);
                r.Add("Azurite", 14);
                r.Add("Black Garnet", 15);
                r.Add("Black Opal", 16);
                r.Add("Bloodstone", 17);
                r.Add("Brass", 57);
                r.Add("Bronze", 58);
                r.Add("Carnelian", 18);
                r.Add("Ceramic", 1);
                r.Add("Citrine", 19);
                r.Add("Copper", 59);
                r.Add("Diamond", 20);
                r.Add("Ebony", 73);
                r.Add("Emerald", 21);
                r.Add("Fire Opal", 22);
                r.Add("Gold", 60);
                r.Add("Granite", 67);
                r.Add("Green Garnet", 23);
                r.Add("Green Jade", 24);
                r.Add("Gromnie Hide", 54);
                r.Add("Hematite", 25);
                r.Add("Imperial Topaz", 26);
                r.Add("Iron", 61);
                r.Add("Ivory", 51);
                r.Add("Jet", 27);
                r.Add("Lapis Lazuli", 28);
                r.Add("Lavender Jade", 29);
                r.Add("Leather", 52);
                r.Add("Linen", 4);
                r.Add("Mahogany", 74);
                r.Add("Malachite", 30);
                r.Add("Marble", 68);
                r.Add("Moonstone", 31);
                r.Add("Oak", 75);
                r.Add("Obsidian", 69);
                r.Add("Onyx", 32);
                r.Add("Opal", 33);
                r.Add("Peridot", 34);
                r.Add("Pine", 76);
                r.Add("Porcelain", 2);
                r.Add("Pyreal", 62);
                r.Add("Red Garnet", 35);
                r.Add("Red Jade", 36);
                r.Add("Reed Shark Hide", 55);
                r.Add("Rose Quartz", 37);
                r.Add("Ruby", 38);
                r.Add("Sandstone", 70);
                r.Add("Sapphire", 39);
                r.Add("Satin", 5);
                r.Add("Serpentine", 71);
                r.Add("Silk", 6);
                r.Add("Silver", 63);
                r.Add("Smokey Quartz", 40);
                r.Add("Steel", 64);
                r.Add("Sunstone", 41);
                r.Add("Teak", 77);
                r.Add("Tiger Eye", 42);
                r.Add("Tourmaline", 43);
                r.Add("Turquoise", 44);
                r.Add("Velvet", 7);
                r.Add("White Jade", 45);
                r.Add("White Quartz", 46);
                r.Add("White Sapphire", 47);
                r.Add("Wool", 8);
                r.Add("Yellow Garnet", 48);
                r.Add("Yellow Topaz", 49);
                r.Add("Zircon", 50);

                return r;
            }

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

            public bool requiresID() { return true; }

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

            public string DisplayString() { return string.Format("{0} matches: {1}", vk, rx); }

            public bool requiresID() { return false; }

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
            public LongValueKey vk = Decal.Adapter.Wrappers.LongValueKey.ActivationReqSkillId;

            public LongValKeyLE() { }
            public LongValKeyLE(int k, Decal.Adapter.Wrappers.LongValueKey v) { keyval = k; vk = v; }

            #region iLootRule Members

            public int GetRuleType() { return 2; }

            public bool requiresID() { return GameInfo.IsIDProperty(vk); }

            public string DisplayString()
            {
                if (vk == LongValueKey.Material)
                {
                    SortedDictionary<string, int> matIds = GameInfo.getMaterialInfo();
                    if (matIds.ContainsValue((int)keyval))
                    {
                        foreach (KeyValuePair<string, int> kv in matIds)
                        {
                            if (kv.Value == (int)keyval)
                            {
                                return string.Format("{0} <= {1} ({2})", vk, keyval, kv.Key);
                            }
                        }
                    }
                }
                return string.Format("{0} <= {1}", vk, keyval);
            }

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

            public string DisplayString()
            {
                if (vk == LongValueKey.Material)
                {
                    SortedDictionary<string, int> matIds = GameInfo.getMaterialInfo();
                    if (matIds.ContainsValue((int)keyval))
                    {
                        foreach (KeyValuePair<string, int> kv in matIds)
                        {
                            if (kv.Value == (int)keyval)
                            {
                                return string.Format("{0} >= {1} ({2})", vk, keyval, kv.Key);
                            }
                        }
                    }                    
                }
                return string.Format("{0} >= {1}", vk, keyval);
            }

            public bool requiresID() { return GameInfo.IsIDProperty(vk); }

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

            public bool requiresID() { return GameInfo.IsIDProperty(vk); }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
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

            public bool requiresID() { return GameInfo.IsIDProperty(vk); }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
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

            public bool requiresID() { return true; }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
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

            public bool requiresID() { return false; }

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

            public bool requiresID() { return true; }

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
