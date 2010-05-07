///////////////////////////////////////////////////////////////////////////////
//File: LootRules.cs
//
//Description: The old-style Virindi Tank loot rule system.
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
using System.Text.RegularExpressions;

#if VTC_PLUGIN
using uTank2.LootPlugins;
#endif

namespace VTClassic
{
    //A single rule
    internal abstract class iLootRule : iSettingsCollection, ICloneable
    {
        //iSettingsCollection methods
        public abstract void Read(System.IO.StreamReader inf, int fileversion);
        public abstract void Write(CountedStreamWriter inf);

        //LootRule abstract methods
        public abstract int GetRuleType();
        public abstract string DisplayString();
        public abstract bool MayRequireID();

#if VTC_PLUGIN
        //Members only when compiling the plugin, vtank is not referenced for editor
        public abstract bool Match(GameItemInfo id);
        public abstract void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch);
#endif

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        cUniqueID iUniqueID;
        public void SetID(cUniqueID pUniqueID)
        {
            iUniqueID = pUniqueID;
        }
        public cUniqueID GetID()
        {
            return iUniqueID;
        }
    }

    internal enum eLootRuleType
    {
        UnsupportedRequirement = -1,

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

    internal class cUnsupportedRequirement : iLootRule
    {
        public char[] data;

        public override int GetRuleType()
        {
            return (int)eLootRuleType.UnsupportedRequirement;
        }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            ismatch = false;
            hasdecision = true;
        }
#endif

        public override void Read(System.IO.StreamReader inf, int fileversion)
        {

        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.Write(data, 0, data.Length);
        }

        public override string DisplayString()
        {
            return "UNSUPPORTED REQUIREMENT";
        }

        public override bool MayRequireID()
        {
            return false;
        }
    }
    internal class SpellNameMatch : iLootRule
    {
        public System.Text.RegularExpressions.Regex rx = new Regex("");

        public SpellNameMatch() { }
        public SpellNameMatch(System.Text.RegularExpressions.Regex k) { rx = k; }

        public override int GetRuleType() { return (int)eLootRuleType.SpellNameMatch; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<uTank2.MySpell> Spells = id.Spells;
            foreach (uTank2.MySpell sp in Spells)
            {
                if (rx.Match(sp.Name).Success)
                    return true;
            }
            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            //Is the object magical?
            bool ismagical = ((id.GetValueInt(IntValueKey.IconOutline, 0) & 0x01) > 0);
            if (ismagical)
            {
                hasdecision = false;
                ismatch = false;        //Doesn't matter, just have to assign
            }
            else
            {
                hasdecision = true;
                ismatch = false;
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            rx = new System.Text.RegularExpressions.Regex(inf.ReadLine());
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(rx.ToString());
        }

        public override string DisplayString()
        {
            return "SpellNameMatch: " + rx.ToString();
        }

        public override bool MayRequireID()
        {
            return true;
        }
    }
    internal class StringValueMatch : iLootRule
    {
        public System.Text.RegularExpressions.Regex rx = new Regex("");
        public StringValueKey vk = StringValueKey.FullDescription;

        public StringValueMatch() { }
        public StringValueMatch(System.Text.RegularExpressions.Regex k, StringValueKey v) { rx = k; vk = v; }

        public override int GetRuleType() { return (int)eLootRuleType.StringValueMatch; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return rx.Match(id.GetValueString(vk, "")).Success;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            rx = new System.Text.RegularExpressions.Regex(inf.ReadLine());
            vk = (StringValueKey)Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(rx.ToString());
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("{0} matches: {1}", vk, rx);
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }
    }
    internal class LongValKeyLE : iLootRule
    {
        public int keyval = 0;
        public IntValueKey vk = IntValueKey.ActivationReqSkillId;

        public LongValKeyLE() { }
        public LongValKeyLE(int k, IntValueKey v) { keyval = k; vk = v; }

        public override int GetRuleType() { return (int)eLootRuleType.LongValKeyLE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) <= keyval);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            if (vk == IntValueKey.Material)
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
            else if (vk == IntValueKey.WieldReqAttribute)
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

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }
    }
    internal class LongValKeyGE : iLootRule
    {
        public int keyval = 0;
        public IntValueKey vk = IntValueKey.ActivationReqSkillId;

        public LongValKeyGE() { }
        public LongValKeyGE(int k, IntValueKey v) { keyval = k; vk = v; }

        public override int GetRuleType() { return (int)eLootRuleType.LongValKeyGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) >= keyval);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            if (vk == IntValueKey.Material)
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
            else if (vk == IntValueKey.WieldReqAttribute)
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

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }
    }
    internal class DoubleValKeyLE : iLootRule
    {
        public double keyval = 0d;
        public DoubleValueKey vk = DoubleValueKey.AcidProt;

        public DoubleValKeyLE() { }
        public DoubleValKeyLE(double k, DoubleValueKey v) { keyval = k; vk = v; }

        public override int GetRuleType() { return (int)eLootRuleType.DoubleValKeyLE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (((float)id.GetValueDouble(vk, 0)) <= ((float)keyval));
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
            vk = (DoubleValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("{0} <= {1}", vk, keyval);
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }
    }
    internal class DoubleValKeyGE : iLootRule
    {
        public double keyval = 0d;
        public DoubleValueKey vk = DoubleValueKey.AcidProt;

        public DoubleValKeyGE() { }
        public DoubleValKeyGE(double k, DoubleValueKey v) { keyval = k; vk = v; }

        public override int GetRuleType() { return (int)eLootRuleType.DoubleValKeyGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (((float)id.GetValueDouble(vk, 0)) >= ((float)keyval));
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
            vk = (DoubleValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("{0} >= {1}", vk, keyval);
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }
    }
    internal class DamagePercentGE : iLootRule
    {
        public double keyval = 0d;

        public DamagePercentGE() { }
        public DamagePercentGE(double k) { keyval = k; }

        public override int GetRuleType() { return (int)eLootRuleType.DamagePercentGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = false;
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("DamagePercentGE >= {0}", keyval);
        }

        public override bool MayRequireID()
        {
            return false;
        }
    }
    internal class ObjectClassE : iLootRule
    {
        public ObjectClass vk = ObjectClass.Armor;

        public ObjectClassE() { }
        public ObjectClassE(ObjectClass v) { vk = v; }

        public override int GetRuleType() { return (int)eLootRuleType.ObjectClass; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.ObjectClass == vk);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            vk = (ObjectClass)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("ObjectClass = {0}", vk); 
        }

        public override bool MayRequireID()
        {
            return false;
        }
    }
    internal class SpellCountGE : iLootRule
    {
        public int keyval = 0;

        public SpellCountGE() { }
        public SpellCountGE(int k) { keyval = k; }

        public override int GetRuleType() { return (int)eLootRuleType.SpellCountGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return id.Spells.Count >= keyval;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            //Is the object magical?
            bool ismagical = ((id.GetValueInt(IntValueKey.IconOutline, 0) & 0x01) > 0);
            if (ismagical)
            {
                hasdecision = false;
                ismatch = false;        //Doesn't matter, just have to assign
            }
            else
            {
                hasdecision = true;
                ismatch = false;
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("SpellCount >= {0}", keyval);
        }

        public override bool MayRequireID()
        {
            return true;
        }
    }
    internal class SpellMatch : iLootRule
    {
        public Regex rxp = new Regex("");
        public Regex rxn = new Regex("");
        public int cnt = 1;

        public SpellMatch() {  }
        public SpellMatch(Regex p, Regex n, int c) { rxp = p; rxn = n; cnt = c; }

        public override int GetRuleType() { return (int)eLootRuleType.SpellMatch; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            int c = 0;
            System.Collections.ObjectModel.ReadOnlyCollection<uTank2.MySpell> Spells = id.Spells;
            bool rxnEmpty = string.Empty.Equals(rxn.ToString().Trim());

            foreach (uTank2.MySpell sp in Spells)
            {
                if (rxp.Match(sp.Name).Success && (rxnEmpty || !rxn.Match(sp.Name).Success))
                {
                    c++; if (c >= cnt) return true;
                }
            }
            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            //Is the object magical?
            bool ismagical = ((id.GetValueInt(IntValueKey.IconOutline, 0) & 0x01) > 0);
            if (ismagical)
            {
                hasdecision = false;
                ismatch = false;        //Doesn't matter, just have to assign
            }
            else
            {
                hasdecision = true;
                ismatch = false;
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            rxp = new Regex(inf.ReadLine());
            rxn = new Regex(inf.ReadLine());
            cnt = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(rxp.ToString());
            inf.WriteLine(rxn.ToString());
            inf.WriteLine(cnt.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            if (string.Empty.Equals(rxn.ToString().Trim()))
            {
                return string.Format("SpellMatch: {0} [{1} times]",
                rxp, cnt);
            }
            return string.Format("SpellMatch: {0}, but not {1} [{2} times]",
                rxp, rxn, cnt);
        }

        public override bool MayRequireID()
        {
            return true;
        }
    }
    internal class MinDamageGE : iLootRule
    {
        public double keyval = 0d;

        public MinDamageGE() { keyval = 0; }
        public MinDamageGE(double v) { keyval = v; }

        public override int GetRuleType() { return (int)eLootRuleType.MinDamageGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            int maxdamage = id.GetValueInt(IntValueKey.MaxDamage, 0);
            double variance = id.GetValueDouble(DoubleValueKey.Variance, 0.0);
            return maxdamage - (variance * maxdamage) >= keyval;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (id.ObjectClass == ObjectClass.MeleeWeapon)
            {
                hasdecision = false;
                ismatch = false;        //Doesn't matter, just have to assign
            }
            else
            {
                hasdecision = true;
                ismatch = false;
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("MinDamage >= {0}", keyval);
        }

        public override bool MayRequireID()
        {
            return true;
        }
    }
    internal class LongValKeyFlagExists : iLootRule
    {
        public int keyval = 0;
        public IntValueKey vk = IntValueKey.ActivationReqSkillId;

        public LongValKeyFlagExists() { }
        public LongValKeyFlagExists(int k, IntValueKey v) { keyval = k; vk = v; }

        public override int GetRuleType() { return (int)eLootRuleType.LongValKeyFlagExists; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) & keyval) > 0;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("{0} has flags {1}", vk, keyval);
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }
    }


    //A set of rules with an action attached
    internal class cLootItemRule : iSettingsCollection
    {
        public List<iLootRule> IntRules = new List<iLootRule>();
        public int pri;
        public eLootAction act;
        public string name;

        public cLootItemRule()
        {
            name = "";
            act = eLootAction.Keep;
            pri = 0;
        }

        public int Priority() { return pri; }
        public eLootAction Action() { return act; }

        public bool AnyReqRequiresID()
        {
            foreach (iLootRule i in IntRules)
            {
                if (i.MayRequireID())
                {
                    return true;
                }
            }
            return false;
        }

#if VTC_PLUGIN
        public bool Match(GameItemInfo id)
        {
            foreach (iLootRule R in IntRules)
                if (!R.Match(id)) return false;
            return true;
        }

        public void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            bool needid = false;
            foreach (iLootRule R in IntRules)
            {
                bool hd;
                bool im;
                R.EarlyMatch(id, out hd, out im);

                if (hd && (!im))
                {
                    hasdecision = true;
                    ismatch = false;
                    return;
                }

                if (!hd)
                    needid = true;
            }

            if (needid)
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = true;
            }
        }
#endif

        #region iSettingsCollection Members

        public void Read(System.IO.StreamReader inf, int profileversion)
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
                iLootRule newrule;
                switch ((eLootRuleType)ruletype)
                {
                    case eLootRuleType.SpellNameMatch: newrule = new SpellNameMatch(); break;
                    case eLootRuleType.StringValueMatch: newrule = new StringValueMatch(); break;
                    case eLootRuleType.LongValKeyLE: newrule = new LongValKeyLE(); break;
                    case eLootRuleType.LongValKeyGE: newrule = new LongValKeyGE(); break;
                    case eLootRuleType.DoubleValKeyLE: newrule = new DoubleValKeyLE(); break;
                    case eLootRuleType.DoubleValKeyGE: newrule = new DoubleValKeyGE(); break;
                    case eLootRuleType.DamagePercentGE: newrule = new DamagePercentGE(); break;
                    case eLootRuleType.ObjectClass: newrule = new ObjectClassE(); break;
                    case eLootRuleType.SpellCountGE: newrule = new SpellCountGE(); break;
                    case eLootRuleType.SpellMatch: newrule = new SpellMatch(); break;
                    case eLootRuleType.MinDamageGE: newrule = new MinDamageGE(); break;
                    case eLootRuleType.LongValKeyFlagExists: newrule = new LongValKeyFlagExists(); break;
                    default: newrule = null; break;
                }

                if (UTLVersionInfo.VersionHasFeature(eUTLFileFeature.RequirementLengthCode, profileversion))
                {
                    int lengthcode = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                    if (newrule != null)
                    {
                        newrule.Read(inf, profileversion);
                        IntRules.Add(newrule);
                    }
                    else
                    {
                        //Skip
                        char[] b = new char[lengthcode];
                        inf.Read(b, 0, lengthcode);
                        cUnsupportedRequirement Unsup = new cUnsupportedRequirement();
                        Unsup.data = b;
                        IntRules.Add(Unsup);
                    }
                }
                else
                {
                    //Uncoded length, hope it is not null because if it is we are just going to crash
                    newrule.Read(inf, profileversion);
                    IntRules.Add(newrule);
                }
            }
        }

        public void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(name);
            StringBuilder s = new StringBuilder();
            s.Append(Convert.ToString(pri, System.Globalization.CultureInfo.InvariantCulture));
            s.Append(";");
            s.Append(Convert.ToString((int)act, System.Globalization.CultureInfo.InvariantCulture));
            foreach (iLootRule lr in IntRules)
            {
                s.Append(";");
                s.Append(Convert.ToString(lr.GetRuleType(), System.Globalization.CultureInfo.InvariantCulture));
            }
            inf.WriteLine(s.ToString());
            foreach (iLootRule lr in IntRules)
            {
                //Write to a temp buffer so we can generate a length prefix
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CountedStreamWriter sw = new CountedStreamWriter(ms);

                lr.Write(sw);
                sw.Flush();

                inf.WriteLine(sw.Count);
                ms.Position = 0;
                System.IO.StreamReader sr = new System.IO.StreamReader(ms);
                inf.Write(sr.ReadToEnd());
            }
        }

        #endregion
    }

    internal class cLootRules
    {
        public List<cLootItemRule> Rules = new List<cLootItemRule>();
        public UTLFileExtraBlockManager ExtraBlockManager = new UTLFileExtraBlockManager();

#if VTC_PLUGIN
        public eLootAction Classify(GameItemInfo id, out string matchedrulename)
        {
            foreach (cLootItemRule R in Rules)
                if (R.Match(id))
                {
                    matchedrulename = R.name;
                    return R.Action();
                }

            matchedrulename = "";
            return eLootAction.NoLoot;
        }

        public bool NeedsID(GameItemInfo id)
        {
            bool iswalking = false;
            eLootAction walkaction = eLootAction.NoLoot;

            foreach (cLootItemRule R in Rules)
            {
                if (iswalking && (R.act != walkaction))
                    return true;

                bool hd;
                bool im;
                R.EarlyMatch(id, out hd, out im);

                //All higher priority rules don't need ID, and don't match
                //thus if this rule matches and doesn't need ID, we know it
                //will also be the final match and we don't need to ID
                if (hd && im)
                {
#if DEBUGMSG
                    if (iswalking)
                        LootCore.WriteToChat("Walking to next match saved an ID on " + id.GetValueString(StringValueKey.Name, "") + "!");
#endif
                    return false;
                }

                if (!hd)
                {
                    //As long as the action is the same, priority is irrelevant,
                    //so continue to match as long as action is the same
                    iswalking = true;
                    walkaction = R.act;
                    //return true;
                }
            }

            if (iswalking)
            {
                return true;
            }
            else
            {
                //Nothing needs an ID, and nothing matches
                return false;
            }
        }
#endif

        public void Clear()
        {
            Rules.Clear();
        }

        #region iSettingsCollection Members

        public bool Read(System.IO.StreamReader inf, int none)
        {
            try
            {
                Rules.Clear();

                //Version 0 files start with a rulecount at the top,
                //later versions start with
                //UTL
                //file version
                //rulecount
                string firstline = inf.ReadLine();
                int utlversion;
                int count;
                if (firstline == "UTL")
                {
                    utlversion = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

                    if (utlversion > UTLVersionInfo.MAX_PROFILE_VERSION)
                        throw new Exception("Profile file is version " + utlversion.ToString() + ", only version " + UTLVersionInfo.MAX_PROFILE_VERSION.ToString() + " is supported by this version of VTClassic.");

                    count = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    utlversion = 0;
                    count = Convert.ToInt32(firstline, System.Globalization.CultureInfo.InvariantCulture);
                }

                //Read rules
                for (int i = 0; i < count; ++i)
                {
                    cLootItemRule R = new cLootItemRule();
                    R.Read(inf, utlversion);
                    Rules.Add(R);
                }

                //Read extra info
                ExtraBlockManager.Read(inf);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Write(CountedStreamWriter inf)
        {
            try
            {
                inf.WriteLine("UTL");
                inf.WriteLine(UTLVersionInfo.MAX_PROFILE_VERSION);

                inf.WriteLine(Convert.ToString(Rules.Count, System.Globalization.CultureInfo.InvariantCulture));
                foreach (cLootItemRule R in Rules)
                    R.Write(inf);

                ExtraBlockManager.Write(inf);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
