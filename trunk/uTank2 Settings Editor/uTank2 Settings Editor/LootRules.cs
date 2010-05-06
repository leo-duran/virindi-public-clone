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
using System.Text.RegularExpressions;

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
        internal interface iLootRule : ICloneable
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
            private static SortedDictionary<string, int> matIds;

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
                if (matIds == null)
                {
                    matIds = new SortedDictionary<string, int>();
                    matIds.Add("Agate", 10);
                    matIds.Add("Alabaster", 66);
                    matIds.Add("Amber", 11);
                    matIds.Add("Amethyst", 12);
                    matIds.Add("Aquamarine", 13);
                    matIds.Add("Armoredillo Hide", 53);
                    matIds.Add("Azurite", 14);
                    matIds.Add("Black Garnet", 15);
                    matIds.Add("Black Opal", 16);
                    matIds.Add("Bloodstone", 17);
                    matIds.Add("Brass", 57);
                    matIds.Add("Bronze", 58);
                    matIds.Add("Carnelian", 18);
                    matIds.Add("Ceramic", 1);
                    matIds.Add("Citrine", 19);
                    matIds.Add("Copper", 59);
                    matIds.Add("Diamond", 20);
                    matIds.Add("Ebony", 73);
                    matIds.Add("Emerald", 21);
                    matIds.Add("Fire Opal", 22);
                    matIds.Add("Gold", 60);
                    matIds.Add("Granite", 67);
                    matIds.Add("Green Garnet", 23);
                    matIds.Add("Green Jade", 24);
                    matIds.Add("Gromnie Hide", 54);
                    matIds.Add("Hematite", 25);
                    matIds.Add("Imperial Topaz", 26);
                    matIds.Add("Iron", 61);
                    matIds.Add("Ivory", 51);
                    matIds.Add("Jet", 27);
                    matIds.Add("Lapis Lazuli", 28);
                    matIds.Add("Lavender Jade", 29);
                    matIds.Add("Leather", 52);
                    matIds.Add("Linen", 4);
                    matIds.Add("Mahogany", 74);
                    matIds.Add("Malachite", 30);
                    matIds.Add("Marble", 68);
                    matIds.Add("Moonstone", 31);
                    matIds.Add("Oak", 75);
                    matIds.Add("Obsidian", 69);
                    matIds.Add("Onyx", 32);
                    matIds.Add("Opal", 33);
                    matIds.Add("Peridot", 34);
                    matIds.Add("Pine", 76);
                    matIds.Add("Porcelain", 2);
                    matIds.Add("Pyreal", 62);
                    matIds.Add("Red Garnet", 35);
                    matIds.Add("Red Jade", 36);
                    matIds.Add("Reed Shark Hide", 55);
                    matIds.Add("Rose Quartz", 37);
                    matIds.Add("Ruby", 38);
                    matIds.Add("Sandstone", 70);
                    matIds.Add("Sapphire", 39);
                    matIds.Add("Satin", 5);
                    matIds.Add("Serpentine", 71);
                    matIds.Add("Silk", 6);
                    matIds.Add("Silver", 63);
                    matIds.Add("Smokey Quartz", 40);
                    matIds.Add("Steel", 64);
                    matIds.Add("Sunstone", 41);
                    matIds.Add("Teak", 77);
                    matIds.Add("Tiger Eye", 42);
                    matIds.Add("Tourmaline", 43);
                    matIds.Add("Turquoise", 44);
                    matIds.Add("Velvet", 7);
                    matIds.Add("White Jade", 45);
                    matIds.Add("White Quartz", 46);
                    matIds.Add("White Sapphire", 47);
                    matIds.Add("Wool", 8);
                    matIds.Add("Yellow Garnet", 48);
                    matIds.Add("Yellow Topaz", 49);
                    matIds.Add("Zircon", 50);
                }
                return matIds;
            }

            public static string getMaterialName(int materialId)
            {
                if (matIds == null) getMaterialInfo();
                if (matIds.ContainsValue(materialId))
                {
                    foreach (KeyValuePair<string, int> kv in matIds)
                    {
                        if (kv.Value == materialId)
                        {
                            return kv.Key;
                        }
                    }
                }

                return string.Empty;
            }



            public static SortedDictionary<string, int[]> getMaterialGroups()
            {
                SortedDictionary<string, int[]> r = new SortedDictionary<string, int[]>();
                // Armor Tinkering: Alabaster, Armoredillo Hide, Bronze, Ceramic, Marble, Peridot, Reedshark Hide, Steel, Wool, Yellow Topaz, Zircon
                r.Add("Armor Tinkering", new int[] { 66, 53, 58, 1, 68, 34, 55, 64, 8, 49, 50 });
                // Item Tinkering: Copper, Ebony, Gold, Linen, Pine, Porcelain, Moonstone, Silk, Silver, Teak
                r.Add("Item Tinkering", new int[] { 59, 73, 60, 4, 76, 2, 31, 6, 63, 77 });
                // Magic Item Tinkering: Agate, Azurite, Black Opal, Bloodstone, Carnelian, Citrine, Fire Opal, Hematite, Lavender Jade, Malachite, Opal, Red Jade, Rose Quartz, Sunstone
                r.Add("Magic Item Tinkering", new int[] { 10, 14, 16, 17, 18, 19, 22, 25, 29, 30, 33, 36, 37, 41 });
                // Weapon Tinkering: Aquamarine, Black Garnet, Brass, Emerald, Granite, Iron, Imperial Topaz, Jet, Mahogany, Oak, Red Garnet, Velvet, White Sapphire
                r.Add("Weapon Tinkering", new int[] { 13, 15, 57, 21, 67, 61, 26, 27, 74, 75, 35, 7, 47 });
                // Basic Tinkering: Ivory, Leather
                r.Add("Basic Tinkering", new int[] { 51, 52 });
                // Gearcrafting: Amber, Diamond, Gromnie Hide, Pyreal, Ruby, Sapphire
                r.Add("Gearcrafting", new int[] { 11, 20, 54, 62, 38, 39 });
                // Armor Imbues: Peridot, Yellow Topaz, Zircon
                r.Add("Armor Imbues", new int[] { 34, 49, 50 });
                // Protection Tinks: Alabaster, Armoredillo Hide, Bronze, Ceramic, Marble, Reedshark Hide, Steel, Wool
                r.Add("Protection Tinks", new int[] { 66, 53, 58, 1, 68, 55, 64, 8 });
                // Weapon Imbues: Aquamarine, Black Garnet, Emerald, Imperial Topaz, Jet, Red Garnet, White Sapphire, Sunstone, Black Opal
                r.Add("Weapon Imbues", new int[] { 13, 15, 21, 26, 27, 35, 47, 41, 16 });
                // Brass/Iron/Granite/Hog
                r.Add("Brass/Granite/Iron/Mahog", new int[] { 57, 67, 61, 74 } );
                // RG/BG/Jet
                r.Add("RG/BG/Jet", new int[] { 35, 15, 27 });
                return r;
            }

        }

        internal enum eLootRuleType
        {
            SpellNameMatch = 0,
            StringValueMatch = 1,
            LongValKeyLE = 2,
            LongValKeyGE = 3,
            DoubleValKeyLE = 4,
            DoubleValKeyGE = 5,
            DamagePercentGE = 6,
            ObjectClass = 7,
            SpellCountGE = 8,
            SpellMatch = 9,
            MinDamageGE = 10,
            LongValKeyFlagExists = 11,
        }

        internal class SpellNameMatch : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) {tn=ttnn;} public cUniqueID GetID() {return tn;}
            public System.Text.RegularExpressions.Regex rx;

            public SpellNameMatch() { rx = new System.Text.RegularExpressions.Regex(""); }
            public SpellNameMatch(System.Text.RegularExpressions.Regex k) { rx = k; }

            #region iLootRule Members

            public int GetRuleType() { return (int)eLootRuleType.SpellNameMatch; }

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


            #region ICloneable Member

            public object Clone()
            {
                SpellNameMatch c = (SpellNameMatch)this.MemberwiseClone();
                c.rx = new System.Text.RegularExpressions.Regex(rx.ToString());
                return c;
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

            public int GetRuleType() { return (int)eLootRuleType.StringValueMatch; }

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

            #region ICloneable Member

            public object Clone()
            {
                StringValueMatch c = (StringValueMatch)this.MemberwiseClone();
                c.rx = new System.Text.RegularExpressions.Regex(rx.ToString());
                return c;
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

            public int GetRuleType() { return (int)eLootRuleType.LongValKeyLE; }

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
                else if (vk == LongValueKey.WieldReqAttribute)
                {
                    SortedDictionary<string, int> skillIds = GameInfo.getSkillInfo();
                    if (skillIds.ContainsValue((int)keyval))
                    {
                        foreach (KeyValuePair<string, int> kv in skillIds)
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

            #region ICloneable Member

            public object Clone()
            {
                return this.MemberwiseClone();
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

            public int GetRuleType() { return (int)eLootRuleType.LongValKeyGE; }

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
                else if (vk == LongValueKey.WieldReqAttribute)
                {
                    SortedDictionary<string, int> skillIds = GameInfo.getSkillInfo();
                    if (skillIds.ContainsValue((int)keyval))
                    {
                        foreach (KeyValuePair<string, int> kv in skillIds)
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

            #region ICloneable Member

            public object Clone()
            {
                return this.MemberwiseClone();
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

            public int GetRuleType() { return (int)eLootRuleType.DoubleValKeyLE; }

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

            #region ICloneable Member

            public object Clone()
            {
                return this.MemberwiseClone();
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

            public int GetRuleType() { return (int)eLootRuleType.DoubleValKeyGE; }

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

            #region ICloneable Member

            public object Clone()
            {
                return this.MemberwiseClone();
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

            public int GetRuleType() { return (int)eLootRuleType.DamagePercentGE; }

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

            #region ICloneable Member

            public object Clone()
            {
                return this.MemberwiseClone();
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

            public int GetRuleType() { return (int)eLootRuleType.ObjectClass; }

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

            #region ICloneable Member

            public object Clone()
            {
                return this.MemberwiseClone();
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

            public int GetRuleType() { return (int)eLootRuleType.SpellCountGE; }

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

            #region ICloneable Member

            public object Clone()
            {
                return this.MemberwiseClone();
            }

            #endregion
        }
        internal class SpellMatch : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) { tn = ttnn; } public cUniqueID GetID() { return tn; }
            public Regex rxp;
            public Regex rxn;
            public int cnt;

            public SpellMatch() { rxp = new Regex(""); rxn = new Regex(""); cnt = 1; }
            public SpellMatch(Regex p, Regex n, int c) { rxp = p; rxn = n; cnt = c; }

            #region iLootRule Members

            public int GetRuleType() { return (int)eLootRuleType.SpellMatch; }

            public string DisplayString()
            {
                if (string.Empty.Equals(rxn.ToString().Trim()))
                {
                    return string.Format("SpellMatch: {0} [{1} times]",
                    rxp, cnt);
                }
                return string.Format("SpellMatch: {0}, but not {1} [{2} times]",
                    rxp, rxn, cnt);
            }

            public bool requiresID() { return true; }

            public void Read(System.IO.StreamReader inf)
            {
                rxp = new Regex(inf.ReadLine());
                rxn = new Regex(inf.ReadLine());
                cnt = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(rxp.ToString());
                inf.WriteLine(rxn.ToString());
                inf.WriteLine(cnt.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            #endregion

            #region ICloneable Member

            public object Clone()
            {
                SpellMatch c = (SpellMatch)this.MemberwiseClone();
                c.rxp = new System.Text.RegularExpressions.Regex(rxp.ToString());
                c.rxn = new System.Text.RegularExpressions.Regex(rxn.ToString());
                return c;
            }

            #endregion
        }
        internal class MinDamageGE : iLootRule
        {
            public cUniqueID tn; public void SetID(cUniqueID ttnn) { tn = ttnn; } public cUniqueID GetID() { return tn; }
            public double keyval;

            public MinDamageGE() { keyval = 0; }
            public MinDamageGE(double v) { keyval = v; }

            #region iLootRule Members

            public int GetRuleType() { return (int)eLootRuleType.MinDamageGE; }

            public string DisplayString()
            {
                return string.Format("MinDamage >= {0}", keyval);
            }

            public bool requiresID() { return true; }

            public void Read(System.IO.StreamReader inf)
            {
                keyval = Math.Round(Convert.ToDouble(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture), 2);
            }

            public void Write(System.IO.StreamWriter inf)
            {
                inf.WriteLine(keyval.ToString());
            }

            #endregion

            #region ICloneable Member

            public object Clone()
            {
                return this.MemberwiseClone();
            }

            #endregion
        }
        //LongValKeyFlagExists - NOT IMPLEMENTED
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

        public bool requiresID()
        {
            foreach (LootRules.iLootRule i in IntRules)
            {
                if (i.requiresID())
                {
                    return true;
                }
            }
            return false;
        }

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
                switch ((uTank2.LootRules.eLootRuleType)ruletype)
                {
                    case uTank2.LootRules.eLootRuleType.SpellNameMatch: newrule = new LootRules.SpellNameMatch(); break;
                    case uTank2.LootRules.eLootRuleType.StringValueMatch: newrule = new LootRules.StringValueMatch(); break;
                    case uTank2.LootRules.eLootRuleType.LongValKeyLE: newrule = new LootRules.LongValKeyLE(); break;
                    case uTank2.LootRules.eLootRuleType.LongValKeyGE: newrule = new LootRules.LongValKeyGE(); break;
                    case uTank2.LootRules.eLootRuleType.DoubleValKeyLE: newrule = new LootRules.DoubleValKeyLE(); break;
                    case uTank2.LootRules.eLootRuleType.DoubleValKeyGE: newrule = new LootRules.DoubleValKeyGE(); break;
                    case uTank2.LootRules.eLootRuleType.DamagePercentGE: newrule = new LootRules.DamagePercentGE(); break;
                    case uTank2.LootRules.eLootRuleType.ObjectClass: newrule = new LootRules.ObjectClass(); break;
                    case uTank2.LootRules.eLootRuleType.SpellCountGE: newrule = new LootRules.SpellCountGE(); break;
                    case uTank2.LootRules.eLootRuleType.SpellMatch: newrule = new LootRules.SpellMatch(); break;
                    case uTank2.LootRules.eLootRuleType.MinDamageGE: newrule = new LootRules.MinDamageGE(); break;
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

    public enum eLootAction
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
