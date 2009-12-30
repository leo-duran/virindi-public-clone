using uTank2;
using uTank2.LootRules;
using Decal.Adapter.Wrappers;
using System.Collections.Generic;
using System;
namespace uTank2_Settings_Editor
{
    public partial class Form1
    {
        void ruleMoveDown(int index, bool setCurrent)
        {
            if (index + 1 >= lstRules.Items.Count) return;
            string swap;
            cLootItemRule swapl;

            swap = (string)lstRules.Items[index + 1];
            swapl = LootRules.Rules[index + 1];
            lstRules.Items[index + 1] = lstRules.Items[index];
            LootRules.Rules[index + 1] = LootRules.Rules[index];
            lstRules.Items[index] = swap;
            LootRules.Rules[index] = swapl;

            if (setCurrent)
            {
                SetCurrentReq(null, 0);
                SetCurrentRule(LootRules.Rules[index + 1], index + 1);
                lstRules.SelectedIndex = index + 1;
            }
            FileChanged = true;
        }

        void ruleMoveUp(int index, bool setCurrent)
        {
            if (index < 1) return;
            ruleMoveDown(index - 1, setCurrent);
            lstRules.SelectedIndex = index - 1;
        }

        void addMaterialRules(int[] matIds, int work, bool update)
        {
            bool create;
            string name;
            foreach (int m in matIds)
            {
                create = true;
                name = string.Format("S: {0}", GameInfo.getMaterialName(m));

                foreach (cLootItemRule r in this.LootRules.Rules)
                {
                    if (r.act == eLootAction.Salvage && r.name.Equals(name))
                    {
                        create = false;

                        if (update)
                        {
                            updateMaterialRule(r, m, work);
                            FileChanged = true;
                        }
                        break;
                    }
                }
                if (create)
                {
                    addMaterialRule(m, work, name);
                }
            }
            SetCurrentRule(null, -1);
        }

        private void addMaterialRule(int mat, int work, string name)
        {
            cLootItemRule r = new cLootItemRule();
            r.name = name;
            r.act = eLootAction.Salvage;

            updateMaterialRule(r, mat, work);

            LootRules.Rules.Add(r);
            lstRules.Items.Add(r.name);
            FileChanged = true;
        }

        private void updateMaterialRule(cLootItemRule r, int mat, int w)
        {
            LongValKeyLE r1 = new LongValKeyLE(mat, LongValueKey.Material);
            LongValKeyGE r2 = new LongValKeyGE(mat, LongValueKey.Material);
            DoubleValKeyGE r3 = new DoubleValKeyGE(Convert.ToDouble(w), DoubleValueKey.SalvageWorkmanship);

            r.IntRules = new List<iLootRule>(new iLootRule[] { r1, r2, r3 });
        }

        private void alterWorkmanshipReqs(eLootAction e, int by)
        {
            if (e == eLootAction.Keep) by = -1 * by;

            foreach (cLootItemRule r in this.LootRules.Rules)
            {
                if (r.act == e)
                {
                    foreach (iLootRule req in r.IntRules)
                    {
                        if (req.GetRuleType() == 5)
                        {
                            ((DoubleValKeyGE)req).keyval = Math.Min(10,((DoubleValKeyGE)req).keyval + by);
                        }
                        else if (req.GetRuleType() == 4)
                        {
                            ((DoubleValKeyLE)req).keyval = Math.Max(1, ((DoubleValKeyLE)req).keyval - by);
                        }

                    }
                }
            }
        }

    }
}