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
using uTank2.LootPlugins;

namespace VTClassic
{
    internal interface iSettingsCollection
    {
        void Read(System.IO.StreamReader inf);
        void Write(System.IO.StreamWriter inf);
    }

    //A single rule
    internal interface iLootRule : iSettingsCollection
    {
        int GetRuleType();
        bool Match(GameItemInfo id);
        //void Read(System.IO.StreamReader inf);
        //void Write(System.IO.StreamWriter inf);
    }

    internal class c0SpellNameMatch : iLootRule
    {
        System.Text.RegularExpressions.Regex rx;

        public c0SpellNameMatch() { }
        public c0SpellNameMatch(System.Text.RegularExpressions.Regex k) { rx = k; }

        #region iLootRule Members

        public int GetRuleType() { return 0; }

        public bool Match(GameItemInfo id)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<uTank2.MySpell> Spells = id.Spells;
            foreach (uTank2.MySpell sp in Spells)
            {
                if (rx.Match(sp.Name).Success)
                    return true;
            }
            return false;
        }

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
    internal class c1StringValueMatch : iLootRule
    {
        public System.Text.RegularExpressions.Regex rx;
        public StringValueKey vk;

        public c1StringValueMatch() { }
        public c1StringValueMatch(System.Text.RegularExpressions.Regex k, StringValueKey v) { rx = k; vk = v; }

        #region iLootRule Members

        public int GetRuleType() { return 1; }

        public bool Match(GameItemInfo id)
        {
            return rx.Match(id.GetValueString(vk, "")).Success;
        }

        public void Read(System.IO.StreamReader inf)
        {
            rx = new System.Text.RegularExpressions.Regex(inf.ReadLine());
            vk = (StringValueKey)Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public void Write(System.IO.StreamWriter inf)
        {
            inf.WriteLine(rx.ToString());
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        #endregion
    }
    internal class c2LongValKeyLE : iLootRule
    {
        public int keyval;
        public IntValueKey vk;

        public c2LongValKeyLE() { }
        public c2LongValKeyLE(int k, IntValueKey v) { keyval = k; vk = v; }

        #region iLootRule Members

        public int GetRuleType() { return 2; }

        public bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) <= keyval);
        }

        public void Read(System.IO.StreamReader inf)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public void Write(System.IO.StreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        #endregion
    }
    internal class c3LongValKeyGE : iLootRule
    {
        public int keyval;
        public IntValueKey vk;

        public c3LongValKeyGE() { }
        public c3LongValKeyGE(int k, IntValueKey v) { keyval = k; vk = v; }

        #region iLootRule Members

        public int GetRuleType() { return 3; }

        public bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) >= keyval);
        }

        public void Read(System.IO.StreamReader inf)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public void Write(System.IO.StreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        #endregion
    }
    internal class c4DoubleValKeyLE : iLootRule
    {
        public double keyval;
        public DoubleValueKey vk;

        public c4DoubleValKeyLE() { }
        public c4DoubleValKeyLE(double k, DoubleValueKey v) { keyval = k; vk = v; }

        #region iLootRule Members

        public int GetRuleType() { return 4; }

        public bool Match(GameItemInfo id)
        {
            return (((float)id.GetValueDouble(vk, 0)) <= ((float)keyval));
        }

        public void Read(System.IO.StreamReader inf)
        {
            keyval = Convert.ToDouble(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (DoubleValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public void Write(System.IO.StreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        #endregion
    }
    internal class c5DoubleValKeyGE : iLootRule
    {
        public double keyval;
        public DoubleValueKey vk;

        public c5DoubleValKeyGE() { }
        public c5DoubleValKeyGE(double k, DoubleValueKey v) { keyval = k; vk = v; }

        #region iLootRule Members

        public int GetRuleType() { return 5; }

        public bool Match(GameItemInfo id)
        {
            return (((float)id.GetValueDouble(vk, 0)) >= ((float)keyval));
        }

        public void Read(System.IO.StreamReader inf)
        {
            keyval = Convert.ToDouble(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (DoubleValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public void Write(System.IO.StreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        #endregion
    }
    internal class c6DamagePercentGE : iLootRule
    {
        public double keyval;

        public c6DamagePercentGE() { }
        public c6DamagePercentGE(double k) { keyval = k; }

        #region iLootRule Members

        public int GetRuleType() { return 6; }

        public bool Match(GameItemInfo id)
        {
            return false;
        }

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
    internal class c7ObjectClassE : iLootRule
    {
        public ObjectClass vk = ObjectClass.Armor;

        public c7ObjectClassE() { }
        public c7ObjectClassE(ObjectClass v) { vk = v; }

        #region iLootRule Members

        public int GetRuleType() { return 7; }

        public bool Match(GameItemInfo id)
        {
            return (id.ObjectClass == vk);
        }

        public void Read(System.IO.StreamReader inf)
        {
            vk = (ObjectClass)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public void Write(System.IO.StreamWriter inf)
        {
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        #endregion
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

        public bool Match(GameItemInfo id)
        {
            foreach (iLootRule R in IntRules)
                if (!R.Match(id)) return false;
            return true;
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
                iLootRule newrule;
                switch (ruletype)
                {
                    case 0: newrule = new c0SpellNameMatch(); break;
                    case 1: newrule = new c1StringValueMatch(); break;
                    case 2: newrule = new c2LongValKeyLE(); break;
                    case 3: newrule = new c3LongValKeyGE(); break;
                    case 4: newrule = new c4DoubleValKeyLE(); break;
                    case 5: newrule = new c5DoubleValKeyGE(); break;
                    case 6: newrule = new c6DamagePercentGE(); break;
                    case 7: newrule = new c7ObjectClassE(); break;
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
            foreach (iLootRule lr in IntRules)
            {
                s.Append(";");
                s.Append(Convert.ToString(lr.GetRuleType(), System.Globalization.CultureInfo.InvariantCulture));
            }
            inf.WriteLine(s.ToString());
            foreach (iLootRule lr in IntRules)
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
        Read = 4,
        User1 = 5,
        User2 = 6,
        User3 = 7,
        User4 = 8,
        User5 = 9
    }

    internal class cLootRules
    {
        public List<cLootItemRule> Rules = new List<cLootItemRule>();
        public eLootAction Classify(GameItemInfo id)
        {
            foreach (cLootItemRule R in Rules)
                if (R.Match(id))
                    return R.Action();

            return eLootAction.NoLoot;
        }

        public void Clear()
        {
            Rules.Clear();
        }

        #region iSettingsCollection Members

        public bool Read(System.IO.StreamReader inf)
        {
            try
            {
                Rules.Clear();
                int count = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                for (int i = 0; i < count; ++i)
                {
                    cLootItemRule R = new cLootItemRule();
                    R.Read(inf);
                    Rules.Add(R);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Write(System.IO.StreamWriter inf)
        {
            try
            {
                inf.WriteLine(Convert.ToString(Rules.Count, System.Globalization.CultureInfo.InvariantCulture));
                foreach (cLootItemRule R in Rules)
                    R.Write(inf);

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
