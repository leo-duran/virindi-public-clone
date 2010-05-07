///////////////////////////////////////////////////////////////////////////////
//File: Form1.cs
//
//Description: The Virindi Tank Loot Editor. This is a standalone windows program
//  to edit loot settings files. It's pretty messy, and should probably be
//  rewritten someday.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VTClassic
{
    public partial class Form1 : Form
    {
        public const string AppName = "Virindi Tank Loot Editor";
        public Form1()
        {
            InitializeComponent();
            InitKeys();

            FileChanged = false;
            FileName = "";
            this.Text = AppName + " - New File";
            SetCurrentReq(null, 0);
            SetCurrentRule(null, 0);

            InitInfoArea();
        }

        string FileName = "";
        bool FileChanged = false;
        cLootRules LootRules = new cLootRules();
        cLootItemRule CurrentRule;
        int CurrentRuleNum;
        iLootRule CurrentReq;
        int CurrentReqNum;
        bool Working = false;

        SortedList<int, StringValueKey> SVKOptions = new SortedList<int,StringValueKey>();
        SortedList<int, IntValueKey> LVKOptions = new SortedList<int,IntValueKey>();
        SortedList<int, DoubleValueKey> DVKOptions = new SortedList<int, DoubleValueKey>();
        SortedList<int, ObjectClass> OCOptions = new SortedList<int, ObjectClass>();

        void InitInfoArea()
        {
            this.cmbSet.Items.Clear();
            this.cmbSet.Items.Add("<Select a set>");
            this.cmbSet.SelectedIndex = 0;
            foreach (KeyValuePair<string, int> kv in GameInfo.getSetInfo())
                this.cmbSet.Items.Add(kv.Key);
            this.cmbSet.SelectedIndexChanged += new EventHandler(cmbSet_SelectedIndexChanged);

            this.cmbSkill.Items.Clear();
            this.cmbSkill.Items.Add("<Select a skill>");
            this.cmbSkill.SelectedIndex = 0;
            foreach (KeyValuePair<string, int> kv in GameInfo.getSkillInfo())
                this.cmbSkill.Items.Add(kv.Key);
            this.cmbSkill.SelectedIndexChanged += new EventHandler(cmbSkill_SelectedIndexChanged);

            this.cmbMaterial.Items.Clear();
            this.cmbMaterial.Items.Add("<Select a material>");
            this.cmbMaterial.SelectedIndex = 0;
            foreach (KeyValuePair<string, int> kv in GameInfo.getMaterialInfo())
                this.cmbMaterial.Items.Add(kv.Key);
            this.cmbMaterial.SelectedIndexChanged += new EventHandler(cmbMaterial_SelectedIndexChanged);
        }

        void InitKeys()
        {
            int i = 0;
            SVKOptions.Add(i++, StringValueKey.FullDescription);
            SVKOptions.Add(i++, StringValueKey.Name);

            i = 0;
            LVKOptions.Add(i++, IntValueKey.ActivationReqSkillId);
            LVKOptions.Add(i++, IntValueKey.AffectsVitalAmt);
            LVKOptions.Add(i++, IntValueKey.ArmorLevel);
            LVKOptions.Add(i++, IntValueKey.ArmorSetID);
            LVKOptions.Add(i++, IntValueKey.Burden);
            LVKOptions.Add(i++, IntValueKey.Category);
            LVKOptions.Add(i++, IntValueKey.Coverage);
            LVKOptions.Add(i++, IntValueKey.CurrentMana);
            LVKOptions.Add(i++, IntValueKey.DamageType);
            LVKOptions.Add(i++, IntValueKey.ElementalDmgBonus);
            LVKOptions.Add(i++, IntValueKey.EquipableSlots);
            LVKOptions.Add(i++, IntValueKey.EquipSkill);
            LVKOptions.Add(i++, IntValueKey.EquipType);
            LVKOptions.Add(i++, IntValueKey.Heritage);
            LVKOptions.Add(i++, IntValueKey.IconOutline);
            LVKOptions.Add(i++, IntValueKey.IconOverlay);
            LVKOptions.Add(i++, IntValueKey.IconUnderlay);
            LVKOptions.Add(i++, IntValueKey.LockpickSkillBonus);
            LVKOptions.Add(i++, IntValueKey.LoreRequirement);
            LVKOptions.Add(i++, IntValueKey.ManaCost);
            LVKOptions.Add(i++, IntValueKey.Material);
            LVKOptions.Add(i++, IntValueKey.MaxDamage);
            LVKOptions.Add(i++, IntValueKey.MaximumMana);
            LVKOptions.Add(i++, IntValueKey.MissileType);
            LVKOptions.Add(i++, IntValueKey.RankRequirement);
            LVKOptions.Add(i++, IntValueKey.RareId);
            LVKOptions.Add(i++, IntValueKey.SkillLevelReq);
            LVKOptions.Add(i++, IntValueKey.SpellCount);
            LVKOptions.Add(i++, IntValueKey.TotalValue);
            LVKOptions.Add(i++, IntValueKey.Type);
            LVKOptions.Add(i++, IntValueKey.Value);
            LVKOptions.Add(i++, IntValueKey.WandElemDmgType);
            LVKOptions.Add(i++, IntValueKey.WeapSpeed);
            LVKOptions.Add(i++, IntValueKey.WieldReqAttribute);
            LVKOptions.Add(i++, IntValueKey.WieldReqType);
            LVKOptions.Add(i++, IntValueKey.WieldReqValue);
            LVKOptions.Add(i++, IntValueKey.Workmanship);

            i = 0;
            DVKOptions.Add(i++, DoubleValueKey.AcidProt);
            DVKOptions.Add(i++, DoubleValueKey.AttackBonus);
            DVKOptions.Add(i++, DoubleValueKey.BludgeonProt);
            DVKOptions.Add(i++, DoubleValueKey.ColdProt);
            DVKOptions.Add(i++, DoubleValueKey.DamageBonus);
            DVKOptions.Add(i++, DoubleValueKey.ElementalDamageVersusMonsters);
            DVKOptions.Add(i++, DoubleValueKey.FireProt);
            DVKOptions.Add(i++, DoubleValueKey.HealingKitRestoreBonus);
            DVKOptions.Add(i++, DoubleValueKey.LightningProt);
            DVKOptions.Add(i++, DoubleValueKey.MagicDBonus);
            DVKOptions.Add(i++, DoubleValueKey.ManaCBonus);
            DVKOptions.Add(i++, DoubleValueKey.ManaRateOfChange);
            DVKOptions.Add(i++, DoubleValueKey.ManaStoneChanceDestruct);
            DVKOptions.Add(i++, DoubleValueKey.ManaTransferEfficiency);
            DVKOptions.Add(i++, DoubleValueKey.MeleeDefenseBonus);
            DVKOptions.Add(i++, DoubleValueKey.MissileDBonus);
            DVKOptions.Add(i++, DoubleValueKey.PierceProt);
            DVKOptions.Add(i++, DoubleValueKey.Range);
            DVKOptions.Add(i++, DoubleValueKey.SalvageWorkmanship);
            DVKOptions.Add(i++, DoubleValueKey.SlashProt);
            DVKOptions.Add(i++, DoubleValueKey.Variance);

            i = 0;
            OCOptions.Add(i++, ObjectClass.Armor);
            OCOptions.Add(i++, ObjectClass.BaseAlchemy);
            OCOptions.Add(i++, ObjectClass.BaseCooking);
            OCOptions.Add(i++, ObjectClass.BaseFletching);
            OCOptions.Add(i++, ObjectClass.Book);
            OCOptions.Add(i++, ObjectClass.Bundle);
            OCOptions.Add(i++, ObjectClass.Clothing);
            OCOptions.Add(i++, ObjectClass.Container);
            OCOptions.Add(i++, ObjectClass.CraftedAlchemy);
            OCOptions.Add(i++, ObjectClass.CraftedCooking);
            OCOptions.Add(i++, ObjectClass.CraftedFletching);
            OCOptions.Add(i++, ObjectClass.Foci);
            OCOptions.Add(i++, ObjectClass.Food);
            OCOptions.Add(i++, ObjectClass.Gem);
            OCOptions.Add(i++, ObjectClass.HealingKit);
            OCOptions.Add(i++, ObjectClass.Jewelry);
            OCOptions.Add(i++, ObjectClass.Journal);
            OCOptions.Add(i++, ObjectClass.Key);
            OCOptions.Add(i++, ObjectClass.Lockpick);
            OCOptions.Add(i++, ObjectClass.ManaStone);
            OCOptions.Add(i++, ObjectClass.MeleeWeapon);
            OCOptions.Add(i++, ObjectClass.Misc);
            OCOptions.Add(i++, ObjectClass.MissileWeapon);
            OCOptions.Add(i++, ObjectClass.Money);
            OCOptions.Add(i++, ObjectClass.Plant);
            OCOptions.Add(i++, ObjectClass.Salvage);
            OCOptions.Add(i++, ObjectClass.Scroll);
            OCOptions.Add(i++, ObjectClass.SpellComponent);
            OCOptions.Add(i++, ObjectClass.TradeNote);
            OCOptions.Add(i++, ObjectClass.Ust);
            OCOptions.Add(i++, ObjectClass.WandStaffOrb);
        }

        StringValueKey SVKFromIndex(int i)
        {
            return SVKOptions[i];
        }
        int IndexFromSVK(StringValueKey k)
        {
            return SVKOptions.Keys[SVKOptions.IndexOfValue(k)];
        }
        IntValueKey LVKFromIndex(int i)
        {
            return LVKOptions[i];
        }
        int IndexFromLVK(IntValueKey k)
        {
            return LVKOptions.Keys[LVKOptions.IndexOfValue(k)];
        }
        DoubleValueKey DVKFromIndex(int i)
        {
            return DVKOptions[i];
        }
        int IndexFromDVK(DoubleValueKey k)
        {
            return DVKOptions.Keys[DVKOptions.IndexOfValue(k)];
        }
        ObjectClass OCFromIndex(int i)
        {
            return OCOptions[i];
        }
        int IndexFromOC(ObjectClass k)
        {
            return OCOptions.Keys[OCOptions.IndexOfValue(k)];
        }

        void SetCurrentRule(cLootItemRule cr, int crn)
        {
            Working = true;
            CurrentRule = cr;
            CurrentRuleNum = crn;
            if (cr == null)
            {
                groupRule.Visible = false;
                if (lstRules.SelectedIndex >= 0) lstRules.SelectedIndex = -1;
            }
            else
            {
                lstRules.SelectedIndex = crn;
                groupRule.Visible = true;
                txtRuleName.Text = cr.name;
                cmbAction.Items.Clear();
                cmbAction.Items.Add("Keep");
                cmbAction.Items.Add("Salvage");
                cmbAction.Items.Add("Sell");
                cmbAction.Items.Add("Read");
                switch (cr.act)
                {
                    case eLootAction.Keep:
                        cmbAction.SelectedIndex = 0;
                        break;
                    case eLootAction.Salvage:
                        cmbAction.SelectedIndex = 1;
                        break;
                    case eLootAction.Sell:
                        cmbAction.SelectedIndex = 2;
                        break;
                    case eLootAction.Read:
                        cmbAction.SelectedIndex = 3;
                        break;
                }


                SetCurrentReq(null, 0);
                lstRequirements.Items.Clear();
                foreach (iLootRule newlr in cr.IntRules)
                {
                    lstRequirements.Items.Add(newlr.DisplayString());
                }
            }
            Working = false;
        }

        void SetCurrentReq(iLootRule cr, int crn)
        {
            Working = true;

            CurrentReq = cr;
            CurrentReqNum = crn;
            if (cr == null)
            {
                groupReqs.Visible = false;
            }
            else
            {
                lstRequirements.SelectedIndex = crn;
                groupReqs.Visible = true;
                cmbReqType.Items.Clear();
                cmbReqType.Items.Add("Spell Name Match");
                cmbReqType.Items.Add("String Value Match");
                cmbReqType.Items.Add("Long Value Key <=");
                cmbReqType.Items.Add("Long Value Key >=");
                cmbReqType.Items.Add("Double Value Key <=");
                cmbReqType.Items.Add("Double Value Key >=");
                cmbReqType.Items.Add("Damage Percentage >=");
                cmbReqType.Items.Add("ObjectClass");
                cmbReqType.Items.Add("Spell Count >=");
                cmbReqType.Items.Add("Spell Match and Count");
                cmbReqType.Items.Add("MinDamage >=");
                cmbReqType.SelectedIndex = cr.GetRuleType();

                cmbActsOn.Items.Clear();
                cmbKey.Items.Clear();
                txtValue.Text = "";
                switch (cr.GetRuleType())
                {
                    case 0:
                        lblActsOn.Visible = false;
                        cmbActsOn.Visible = false;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        lblValue.Text = "Value";
                        txtValue.Visible = true;
                        txtValue.Text = ((SpellNameMatch)cr).rx.ToString();
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                    case 1:
                        lblActsOn.Visible = true;
                        cmbActsOn.Visible = true;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        txtValue.Visible = true;
                        lblValue.Text = "Value";
                        cmbActsOn.Items.Clear();
                        for (int i = 0; i < SVKOptions.Count; ++i) cmbActsOn.Items.Add(SVKOptions[i]);
                        cmbActsOn.SelectedIndex = IndexFromSVK(((StringValueMatch)cr).vk);
                        txtValue.Text = ((StringValueMatch)cr).rx.ToString();
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                    case 2:
                        lblActsOn.Visible = true;
                        cmbActsOn.Visible = true;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        lblValue.Text = "Value";
                        txtValue.Visible = true;
                        cmbActsOn.Items.Clear();
                        for (int i = 0; i < LVKOptions.Count; ++i) cmbActsOn.Items.Add(LVKOptions[i]);
                        cmbActsOn.SelectedIndex = IndexFromLVK(((LongValKeyLE)cr).vk);
                        txtValue.Text = ((LongValKeyLE)cr).keyval.ToString();
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                    case 3:
                        lblActsOn.Visible = true;
                        cmbActsOn.Visible = true;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        lblValue.Text = "Value";
                        txtValue.Visible = true;
                        cmbActsOn.Items.Clear();
                        for (int i = 0; i < LVKOptions.Count; ++i) cmbActsOn.Items.Add(LVKOptions[i]);
                        cmbActsOn.SelectedIndex = IndexFromLVK(((LongValKeyGE)cr).vk);
                        txtValue.Text = ((LongValKeyGE)cr).keyval.ToString();
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                    case 4:
                        lblActsOn.Visible = true;
                        cmbActsOn.Visible = true;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        lblValue.Text = "Value";
                        txtValue.Visible = true;
                        cmbActsOn.Items.Clear();
                        for (int i = 0; i < DVKOptions.Count; ++i) cmbActsOn.Items.Add(DVKOptions[i]);
                        cmbActsOn.SelectedIndex = IndexFromDVK(((DoubleValKeyLE)cr).vk);
                        txtValue.Text = ((DoubleValKeyLE)cr).keyval.ToString();
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                    case 5:
                        lblActsOn.Visible = true;
                        cmbActsOn.Visible = true;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        lblValue.Text = "Value";
                        txtValue.Visible = true;
                        cmbActsOn.Items.Clear();
                        for (int i = 0; i < DVKOptions.Count; ++i) cmbActsOn.Items.Add(DVKOptions[i]);
                        cmbActsOn.SelectedIndex = IndexFromDVK(((DoubleValKeyGE)cr).vk);
                        txtValue.Text = ((DoubleValKeyGE)cr).keyval.ToString();
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                    case 6:
                        lblActsOn.Visible = false;
                        cmbActsOn.Visible = false;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        lblValue.Text = "Value";
                        txtValue.Visible = true;
                        txtValue.Text = ((DamagePercentGE)cr).keyval.ToString();
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                    case 7:
                        lblActsOn.Visible = false;
                        cmbActsOn.Visible = false;
                        lblKey.Visible = true;
                        cmbKey.Visible = true;
                        for (int i = 0; i < OCOptions.Count; ++i) cmbKey.Items.Add(OCOptions[i]);
                        cmbKey.SelectedIndex = IndexFromOC(((ObjectClassE)cr).vk);
                        lblValue.Visible = false;
                        txtValue.Visible = false;
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                    case 8:
                        lblActsOn.Visible = false;
                        cmbActsOn.Visible = false;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        lblValue.Text = "Value";
                        txtValue.Visible = true;
                        txtValue.Text = ((SpellCountGE)cr).keyval.ToString();
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                    case 9:
                        lblActsOn.Visible = false;
                        cmbActsOn.Visible = false;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        lblValue.Text = "Does match";
                        txtValue.Visible = true;
                        txtValue.Text = ((SpellMatch)cr).rxp.ToString();
                        lblValue2.Visible = true;
                        lblValue2.Text = "Does NOT match";
                        txtValue2.Visible = true;
                        txtValue2.Text = ((SpellMatch)cr).rxn.ToString();
                        lblValue3.Visible = true;
                        lblValue3.Text = "Miminum spells that match";
                        txtValue3.Visible = true;
                        txtValue3.Text = ((SpellMatch)cr).cnt.ToString();
                        break;
                    case 10:
                        lblActsOn.Visible = false;
                        cmbActsOn.Visible = false;
                        lblKey.Visible = false;
                        cmbKey.Visible = false;
                        lblValue.Visible = true;
                        lblValue.Text = "Value";
                        txtValue.Visible = true;
                        txtValue.Text = ((MinDamageGE)cr).keyval.ToString();
                        lblValue2.Visible = false;
                        txtValue2.Visible = false;
                        lblValue3.Visible = false;
                        txtValue3.Visible = false;
                        break;
                }
            }

            Working = false;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSave()) return;

            FileChanged = false;
            FileName = "";
            this.Text = AppName + " - New File";
            LootRules.Rules.Clear();
            lstRules.Items.Clear();

            SetCurrentReq(null, 0);
            SetCurrentRule(null, 0);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSave()) return;

            System.Windows.Forms.OpenFileDialog od = new OpenFileDialog();
            od.DefaultExt = ".utl";
            od.Filter = "uTank settings files|*.utl";
            od.InitialDirectory = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Decal\\Plugins\\{642F1F48-16BE-48BF-B1D4-286652C4533E}").GetValue("ProfilePath").ToString();
            od.ShowDialog();
            if (od.FileName != "")
            {
                FileName = od.FileName;
                FileChanged = false;
                this.Text = AppName + " - " + FileName;

                System.IO.StreamReader pf = new System.IO.StreamReader(FileName);
                LootRules.Read(pf, 0);
                pf.Close();

                Working = true;
                lstRules.Items.Clear();
                foreach (cLootItemRule ir in LootRules.Rules)
                {
                    lstRules.Items.Add(ir.name);
                }
                Working = false;

                SetCurrentReq(null, 0);
                SetCurrentRule(null, 0);
            }
        }

        bool CheckSave()
        {
            if (FileChanged)
            {
                DialogResult b = System.Windows.Forms.MessageBox.Show("File not saved. Save now?", "Save", MessageBoxButtons.YesNoCancel);
                switch (b)
                {
                    case DialogResult.Yes:
                        saveAsToolStripMenuItem_Click(null, null);
                        return saveasres;
                        //break;
                    case DialogResult.No:
                        return true;
                        //break;
                    case DialogResult.Cancel:
                        return false;
                        //break;
                }
            }
            return true;
        }

        void DoSave()
        {
            FileChanged = false;
            CountedStreamWriter pf = new CountedStreamWriter(FileName);
            LootRules.Write(pf);
            pf.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileName == "")
                saveAsToolStripMenuItem_Click(sender, e);
            else
                DoSave();
        }

        bool saveasres;

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog od = new SaveFileDialog();
            od.DefaultExt = ".utl";
            od.Filter = "uTank settings files|*.utl";
            od.InitialDirectory = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Decal\\Plugins\\{642F1F48-16BE-48BF-B1D4-286652C4533E}").GetValue("Path").ToString();
            od.ShowDialog();
            saveasres = false;
            if (od.FileName != "")
            {
                FileName = od.FileName;
                this.Text = AppName + " - " + FileName;
                DoSave();
                saveasres = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckSave())
                this.Dispose();
        }

        private void lstRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Working) return;

            if (lstRules.SelectedIndex >= 0)
                SetCurrentRule(LootRules.Rules[lstRules.SelectedIndex], lstRules.SelectedIndex);
            else
                SetCurrentRule(null, 0);
        }

        private void cmdCloneRule_Click(object sender, EventArgs e)
        {
            if (CurrentRule != null)
            {
                cLootItemRule lr = new cLootItemRule();
                lr.name = CurrentRule.name;
                lr.act = CurrentRule.act;
                foreach (iLootRule r in CurrentRule.IntRules)
                {
                    lr.IntRules.Add((iLootRule)r.Clone());
                }
                LootRules.Rules.Add(lr);
                lstRules.Items.Add(lr.name);
                SetCurrentRule(lr, LootRules.Rules.Count - 1);
            }
        }

        private void cmdNewRule_Click(object sender, EventArgs e)
        {
            FileChanged = true;
            cLootItemRule lr = new cLootItemRule();
            lr.name = "New Rule";
            lr.act = eLootAction.Keep;
            LootRules.Rules.Add(lr);
            lstRules.Items.Add(lr.name);
            SetCurrentRule(lr, LootRules.Rules.Count - 1);
        }

        private void cmdDeleteRule_Click(object sender, EventArgs e)
        {
            if (CurrentRule == null) return;
            FileChanged = true;

            Working = true;

            LootRules.Rules.RemoveAt(CurrentRuleNum);
            lstRules.Items.RemoveAt(CurrentRuleNum);
            SetCurrentRule(null, 0);

            Working = false;
        }

        private void lstRequirements_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Working) return;

            if (lstRequirements.SelectedIndex >= 0)
                SetCurrentReq(CurrentRule.IntRules[lstRequirements.SelectedIndex], lstRequirements.SelectedIndex);
            else
                SetCurrentReq(null, 0);
        }

        private void cmdNewReq_Click(object sender, EventArgs e)
        {
            FileChanged = true;
            SpellNameMatch lr = new SpellNameMatch();

            CurrentRule.IntRules.Add(lr);
            lstRequirements.Items.Add(lr.DisplayString());
            SetCurrentReq(lr, CurrentRule.IntRules.Count-1);
        }

        private void cmdCloneReq_Click(object sender, EventArgs e)
        {
            if (CurrentReq != null)
            {
                FileChanged = true;
                iLootRule lr = (iLootRule)CurrentReq.Clone();

                CurrentRule.IntRules.Add(lr);
                lstRequirements.Items.Add(lr.DisplayString());
                SetCurrentReq(lr, CurrentRule.IntRules.Count - 1);
            }
        }

        private void cmdDeleteReq_Click(object sender, EventArgs e)
        {
            if (CurrentReq == null) return;
            FileChanged = true;

            Working = true;

            CurrentRule.IntRules.RemoveAt(CurrentReqNum);
            lstRequirements.Items.RemoveAt(CurrentReqNum);
            SetCurrentReq(null, 0);

            Working = false;
        }

        private void txtRuleName_TextChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            CurrentRule.name = txtRuleName.Text;
            lstRules.Items[CurrentRuleNum] = txtRuleName.Text;

            Working = false;
        }

        private void cmdAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            switch (cmbAction.SelectedIndex)
            {
                case 0:
                    CurrentRule.act = eLootAction.Keep;
                    break;
                case 1:
                    CurrentRule.act = eLootAction.Salvage;
                    break;
                case 2:
                    CurrentRule.act = eLootAction.Sell;
                    break;
                case 3:
                    CurrentRule.act = eLootAction.Read;
                    break;
            }

            Working = false;
        }

        private void cmbReqType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            iLootRule newlr;
            switch (cmbReqType.SelectedIndex)
            {
                case 0:
                    newlr = new SpellNameMatch();
                    break;
                case 1:
                    newlr = new StringValueMatch();
                    break;
                case 2:
                    newlr = new LongValKeyLE();
                    break;
                case 3:
                    newlr = new LongValKeyGE();
                    break;
                case 4:
                    newlr = new DoubleValKeyLE();
                    break;
                case 5:
                    newlr = new DoubleValKeyGE();
                    break;
                case 6:
                    newlr = new DamagePercentGE();
                    break;
                case 7:
                    newlr = new ObjectClassE();
                    break;
                case 8:
                    newlr = new SpellCountGE();
                    break;
                case 9:
                    newlr = new SpellMatch();
                    break;
                case 10:
                    newlr = new MinDamageGE();
                    break;
                default:
                    newlr = CurrentReq;
                    break;
            }
            if (newlr.GetType() != CurrentReq.GetType())
            {
                //Change type
                //lstRequirements.Items[CurrentReqNum] = newlr.GetType().ToString().Split(new char[] { '.' })[2];
                CurrentRule.IntRules[CurrentReqNum] = newlr;
                SetCurrentReq(newlr, CurrentReqNum);
                lstRequirements.Items[CurrentReqNum] = newlr.DisplayString();
            }

            Working = false;
        }

        private void cmbActsOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            switch (CurrentReq.GetRuleType())
            {
                case 1:
                    ((StringValueMatch)CurrentReq).vk = SVKFromIndex(cmbActsOn.SelectedIndex);
                    break;
                case 2:
                    ((LongValKeyLE)CurrentReq).vk = LVKFromIndex(cmbActsOn.SelectedIndex);
                    break;
                case 3:
                    ((LongValKeyGE)CurrentReq).vk = LVKFromIndex(cmbActsOn.SelectedIndex);
                    break;
                case 4:
                    ((DoubleValKeyLE)CurrentReq).vk = DVKFromIndex(cmbActsOn.SelectedIndex);
                    break;
                case 5:
                    ((DoubleValKeyGE)CurrentReq).vk = DVKFromIndex(cmbActsOn.SelectedIndex);
                    break;
            }

            lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();

            Working = false;
        }

        private void cmbKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;

            Working = true;
            if (CurrentReq.GetRuleType() == 7)
            {
                ((ObjectClassE)CurrentReq).vk = OCFromIndex(cmbKey.SelectedIndex);
                lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            }
            Working = false;
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;
            try
            {
                switch (CurrentReq.GetRuleType())
                {
                    case 0:
                        ((SpellNameMatch)CurrentReq).rx = new System.Text.RegularExpressions.Regex(txtValue.Text);
                        break;
                    case 1:
                        ((StringValueMatch)CurrentReq).rx = new System.Text.RegularExpressions.Regex(txtValue.Text);
                        break;
                    case 2:
                        ((LongValKeyLE)CurrentReq).keyval = System.Convert.ToInt32(txtValue.Text);
                        break;
                    case 3:
                        ((LongValKeyGE)CurrentReq).keyval = System.Convert.ToInt32(txtValue.Text);
                        break;
                    case 4:
                        ((DoubleValKeyLE)CurrentReq).keyval = System.Convert.ToDouble(txtValue.Text);
                        break;
                    case 5:
                        ((DoubleValKeyGE)CurrentReq).keyval = System.Convert.ToDouble(txtValue.Text);
                        break;
                    case 6:
                        ((DamagePercentGE)CurrentReq).keyval = System.Convert.ToDouble(txtValue.Text);
                        break;
                    case 8:
                        ((SpellCountGE)CurrentReq).keyval = System.Convert.ToInt32(txtValue.Text);
                        break;
                    case 9:
                        ((SpellMatch)CurrentReq).rxp = new System.Text.RegularExpressions.Regex(txtValue.Text);
                        break;
                    case 10:
                        ((MinDamageGE)CurrentReq).keyval = System.Convert.ToDouble(txtValue.Text);
                        break;
                }
                lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            } catch (Exception) { }
            Working = false;
        }

        void txtValue2_TextChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;
            try
            {
                switch (CurrentReq.GetRuleType())
                {
                    case 9:
                        ((SpellMatch)CurrentReq).rxn = new System.Text.RegularExpressions.Regex(txtValue2.Text);
                        break;
                }
                lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            } catch (Exception) { }
            Working = false;
        }

        void txtValue3_TextChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;
            try
            {
                switch (CurrentReq.GetRuleType())
                {
                    case 9:
                        ((SpellMatch)CurrentReq).cnt = Convert.ToInt32(txtValue3.Text);
                        break;
                }
                lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            }
            catch (Exception) { }
            Working = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ruleMoveUp(lstRules.SelectedIndex, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ruleMoveDown(lstRules.SelectedIndex, true);
        }

        private void addSalvageRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSalvageRulesForm asr = new AddSalvageRulesForm();
            DialogResult r = asr.ShowDialog(this);
            if (r == DialogResult.OK || r == DialogResult.Yes)
            {
                try
                {
                    int[] matIds = GameInfo.getMaterialGroups()[asr.grp];
                    this.addMaterialRules(matIds, asr.wrk, r == DialogResult.Yes);
                }
                catch (Exception ex) { }
            }
            asr.Dispose();
        }

        private void increaseSalvageWorkmanshipsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateWorkReqsForm f = new UpdateWorkReqsForm();
            DialogResult r = f.ShowDialog(this);
            if (r == DialogResult.OK)
            {
                try
                {
                    this.alterWorkmanshipReqs(f.act, f.wrk);
                    SetCurrentReq(null, 0);
                    SetCurrentRule(null, -1);
                }
                catch (Exception ex) { }
            }
            f.Dispose();
        }

        private void autoSortRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstRules.Visible = false;
            try
            {
                SetCurrentReq(null, 0);
                SetCurrentRule(null, -1);

                int i, j;
                bool swap;
                for (i = lstRules.Items.Count - 1; i > 0; i--)
                {
                    for (j = 0; j < i; j++)
                    {
                        swap = (LootRules.Rules[j].act > LootRules.Rules[j + 1].act)
                            || (LootRules.Rules[j].act == LootRules.Rules[j + 1].act
                                && LootRules.Rules[j].AnyReqRequiresID() && !LootRules.Rules[j + 1].AnyReqRequiresID())
                            || (LootRules.Rules[j].act == LootRules.Rules[j + 1].act
                                && LootRules.Rules[j].AnyReqRequiresID() == LootRules.Rules[j + 1].AnyReqRequiresID()
                                && LootRules.Rules[j].name.CompareTo(LootRules.Rules[j + 1].name) > 0);
                        if (swap) ruleMoveDown(j, false);
                    }
                }
            }
            catch (Exception ex) { }
            lstRules.Visible = true;
        }

    }
}