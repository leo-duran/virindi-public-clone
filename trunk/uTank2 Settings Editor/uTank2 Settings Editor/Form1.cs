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
using uTank2;
using uTank2.LootRules;

namespace uTank2_Settings_Editor
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
        uTank2.LootRules.iLootRule CurrentReq;
        int CurrentReqNum;
        bool Working = false;

        SortedList<int, Decal.Adapter.Wrappers.StringValueKey> SVKOptions = new SortedList<int,Decal.Adapter.Wrappers.StringValueKey>();
        SortedList<int, Decal.Adapter.Wrappers.LongValueKey> LVKOptions = new SortedList<int,Decal.Adapter.Wrappers.LongValueKey>();
        SortedList<int, Decal.Adapter.Wrappers.DoubleValueKey> DVKOptions = new SortedList<int, Decal.Adapter.Wrappers.DoubleValueKey>();
        SortedList<int, Decal.Adapter.Wrappers.ObjectClass> OCOptions = new SortedList<int, Decal.Adapter.Wrappers.ObjectClass>();

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
            SVKOptions.Add(i++, Decal.Adapter.Wrappers.StringValueKey.FullDescription);
            SVKOptions.Add(i++, Decal.Adapter.Wrappers.StringValueKey.Name);

            i = 0;
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.ActivationReqSkillId);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.ArmorLevel);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.ArmorSetID);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.Burden);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.Category);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.Coverage);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.CurrentMana);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.DamageType);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.ElementalDmgBonus);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.EquipableSlots);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.EquipSkill);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.EquipType);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.HealKitSkillBonus);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.Heritage);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.IconOutline);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.IconOverlay);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.IconUnderlay);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.LockpickSkillBonus);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.LoreRequirement);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.ManaCost);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.Material);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.MaxDamage);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.MaximumMana);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.MissileType);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.RankRequirement);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.RareId);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.SkillLevelReq);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.SpellCount);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.TotalValue);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.Type);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.Value);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.WandElemDmgType);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.WeapSpeed);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.WieldReqAttribute);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.WieldReqType);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.WieldReqValue);
            LVKOptions.Add(i++, Decal.Adapter.Wrappers.LongValueKey.Workmanship);

            i = 0;
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.AcidProt);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.AttackBonus);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.BludgeonProt);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.ColdProt);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.DamageBonus);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.ElementalDamageVersusMonsters);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.FireProt);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.HealingKitRestoreBonus);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.LightningProt);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.MagicDBonus);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.ManaCBonus);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.ManaRateOfChange);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.ManaStoneChanceDestruct);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.ManaTransferEfficiency);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.MeleeDefenseBonus);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.MissileDBonus);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.PierceProt);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.Range);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.SalvageWorkmanship);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.SlashProt);
            DVKOptions.Add(i++, Decal.Adapter.Wrappers.DoubleValueKey.Variance);

            i = 0;
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Armor);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.BaseAlchemy);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.BaseCooking);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.BaseFletching);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Book);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Bundle);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Clothing);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Container);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.CraftedAlchemy);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.CraftedCooking);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.CraftedFletching);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Foci);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Food);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Gem);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.HealingKit);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Jewelry);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Journal);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Key);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Lockpick);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.ManaStone);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.MeleeWeapon);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Misc);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.MissileWeapon);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Money);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Plant);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Salvage);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Scroll);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.SpellComponent);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.TradeNote);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.Ust);
            OCOptions.Add(i++, Decal.Adapter.Wrappers.ObjectClass.WandStaffOrb);
        }

        Decal.Adapter.Wrappers.StringValueKey SVKFromIndex(int i)
        {
            return SVKOptions[i];
        }
        int IndexFromSVK(Decal.Adapter.Wrappers.StringValueKey k)
        {
            return SVKOptions.Keys[SVKOptions.IndexOfValue(k)];
        }
        Decal.Adapter.Wrappers.LongValueKey LVKFromIndex(int i)
        {
            return LVKOptions[i];
        }
        int IndexFromLVK(Decal.Adapter.Wrappers.LongValueKey k)
        {
            return LVKOptions.Keys[LVKOptions.IndexOfValue(k)];
        }
        Decal.Adapter.Wrappers.DoubleValueKey DVKFromIndex(int i)
        {
            return DVKOptions[i];
        }
        int IndexFromDVK(Decal.Adapter.Wrappers.DoubleValueKey k)
        {
            return DVKOptions.Keys[DVKOptions.IndexOfValue(k)];
        }
        Decal.Adapter.Wrappers.ObjectClass OCFromIndex(int i)
        {
            return OCOptions[i];
        }
        int IndexFromOC(Decal.Adapter.Wrappers.ObjectClass k)
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
                foreach (uTank2.LootRules.iLootRule newlr in cr.IntRules)
                {
                    lstRequirements.Items.Add(newlr.DisplayString());
                }
            }
            Working = false;
        }

        void SetCurrentReq(uTank2.LootRules.iLootRule cr, int crn)
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
                        txtValue.Text = ((uTank2.LootRules.SpellNameMatch)cr).rx.ToString();
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
                        cmbActsOn.SelectedIndex = IndexFromSVK(((uTank2.LootRules.StringValueMatch)cr).vk);
                        txtValue.Text = ((uTank2.LootRules.StringValueMatch)cr).rx.ToString();
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
                        cmbActsOn.SelectedIndex = IndexFromLVK(((uTank2.LootRules.LongValKeyLE)cr).vk);
                        txtValue.Text = ((uTank2.LootRules.LongValKeyLE)cr).keyval.ToString();
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
                        cmbActsOn.SelectedIndex = IndexFromLVK(((uTank2.LootRules.LongValKeyGE)cr).vk);
                        txtValue.Text = ((uTank2.LootRules.LongValKeyGE)cr).keyval.ToString();
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
                        cmbActsOn.SelectedIndex = IndexFromDVK(((uTank2.LootRules.DoubleValKeyLE)cr).vk);
                        txtValue.Text = ((uTank2.LootRules.DoubleValKeyLE)cr).keyval.ToString();
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
                        cmbActsOn.SelectedIndex = IndexFromDVK(((uTank2.LootRules.DoubleValKeyGE)cr).vk);
                        txtValue.Text = ((uTank2.LootRules.DoubleValKeyGE)cr).keyval.ToString();
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
                        txtValue.Text = ((uTank2.LootRules.DamagePercentGE)cr).keyval.ToString();
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
                        cmbKey.SelectedIndex = IndexFromOC(((uTank2.LootRules.ObjectClass)cr).vk);
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
                        txtValue.Text = ((uTank2.LootRules.SpellCountGE)cr).keyval.ToString();
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
                        txtValue.Text = ((uTank2.LootRules.SpellMatch)cr).rxp.ToString();
                        lblValue2.Visible = true;
                        lblValue2.Text = "Does NOT match";
                        txtValue2.Visible = true;
                        txtValue2.Text = ((uTank2.LootRules.SpellMatch)cr).rxn.ToString();
                        lblValue3.Visible = true;
                        lblValue3.Text = "Miminum spells that match";
                        txtValue3.Visible = true;
                        txtValue3.Text = ((uTank2.LootRules.SpellMatch)cr).cnt.ToString();
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
                LootRules.Read(pf);
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
            System.IO.StreamWriter pf = new System.IO.StreamWriter(FileName);
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
                    lr.IntRules.Add(r);
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
            uTank2.LootRules.SpellNameMatch lr = new uTank2.LootRules.SpellNameMatch();

            CurrentRule.IntRules.Add(lr);
            lstRequirements.Items.Add(lr.DisplayString());
            SetCurrentReq(lr, CurrentRule.IntRules.Count-1);
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

            uTank2.LootRules.iLootRule newlr;
            switch (cmbReqType.SelectedIndex)
            {
                case 0:
                    newlr = new uTank2.LootRules.SpellNameMatch();
                    break;
                case 1:
                    newlr = new uTank2.LootRules.StringValueMatch();
                    break;
                case 2:
                    newlr = new uTank2.LootRules.LongValKeyLE();
                    break;
                case 3:
                    newlr = new uTank2.LootRules.LongValKeyGE();
                    break;
                case 4:
                    newlr = new uTank2.LootRules.DoubleValKeyLE();
                    break;
                case 5:
                    newlr = new uTank2.LootRules.DoubleValKeyGE();
                    break;
                case 6:
                    newlr = new uTank2.LootRules.DamagePercentGE();
                    break;
                case 7:
                    newlr = new uTank2.LootRules.ObjectClass();
                    break;
                case 8:
                    newlr = new uTank2.LootRules.SpellCountGE();
                    break;
                case 9:
                    newlr = new uTank2.LootRules.SpellMatch();
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
                    ((uTank2.LootRules.StringValueMatch)CurrentReq).vk = SVKFromIndex(cmbActsOn.SelectedIndex);
                    break;
                case 2:
                    ((uTank2.LootRules.LongValKeyLE)CurrentReq).vk = LVKFromIndex(cmbActsOn.SelectedIndex);
                    break;
                case 3:
                    ((uTank2.LootRules.LongValKeyGE)CurrentReq).vk = LVKFromIndex(cmbActsOn.SelectedIndex);
                    break;
                case 4:
                    ((uTank2.LootRules.DoubleValKeyLE)CurrentReq).vk = DVKFromIndex(cmbActsOn.SelectedIndex);
                    break;
                case 5:
                    ((uTank2.LootRules.DoubleValKeyGE)CurrentReq).vk = DVKFromIndex(cmbActsOn.SelectedIndex);
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
                ((uTank2.LootRules.ObjectClass)CurrentReq).vk = OCFromIndex(cmbKey.SelectedIndex);
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
                        ((uTank2.LootRules.SpellNameMatch)CurrentReq).rx = new System.Text.RegularExpressions.Regex(txtValue.Text);
                        break;
                    case 1:
                        ((uTank2.LootRules.StringValueMatch)CurrentReq).rx = new System.Text.RegularExpressions.Regex(txtValue.Text);
                        break;
                    case 2:
                        ((uTank2.LootRules.LongValKeyLE)CurrentReq).keyval = System.Convert.ToInt32(txtValue.Text);
                        break;
                    case 3:
                        ((uTank2.LootRules.LongValKeyGE)CurrentReq).keyval = System.Convert.ToInt32(txtValue.Text);
                        break;
                    case 4:
                        ((uTank2.LootRules.DoubleValKeyLE)CurrentReq).keyval = System.Convert.ToDouble(txtValue.Text);
                        break;
                    case 5:
                        ((uTank2.LootRules.DoubleValKeyGE)CurrentReq).keyval = System.Convert.ToDouble(txtValue.Text);
                        break;
                    case 6:
                        ((uTank2.LootRules.DamagePercentGE)CurrentReq).keyval = System.Convert.ToDouble(txtValue.Text);
                        break;
                    case 8:
                        ((uTank2.LootRules.SpellCountGE)CurrentReq).keyval = System.Convert.ToInt32(txtValue.Text);
                        break;
                    case 9:
                        ((uTank2.LootRules.SpellMatch)CurrentReq).rxp = new System.Text.RegularExpressions.Regex(txtValue.Text);
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
                        ((uTank2.LootRules.SpellMatch)CurrentReq).rxn = new System.Text.RegularExpressions.Regex(txtValue2.Text);
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
                        ((uTank2.LootRules.SpellMatch)CurrentReq).cnt = Convert.ToInt32(txtValue3.Text);
                        break;
                }
                lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            }
            catch (Exception) { }
            Working = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (lstRules.SelectedIndex <= 0) return;
            string swap;
            cLootItemRule swapl;

            swap = (string)lstRules.Items[lstRules.SelectedIndex - 1];
            swapl = LootRules.Rules[lstRules.SelectedIndex - 1];
            lstRules.Items[lstRules.SelectedIndex - 1] = lstRules.Items[lstRules.SelectedIndex];
            LootRules.Rules[lstRules.SelectedIndex - 1] = LootRules.Rules[lstRules.SelectedIndex];
            lstRules.Items[lstRules.SelectedIndex] = swap;
            LootRules.Rules[lstRules.SelectedIndex] = swapl;

            SetCurrentReq(null, 0);
            SetCurrentRule(LootRules.Rules[lstRules.SelectedIndex - 1], lstRules.SelectedIndex - 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lstRules.SelectedIndex + 1 >= lstRules.Items.Count) return;
            string swap;
            cLootItemRule swapl;

            swap = (string)lstRules.Items[lstRules.SelectedIndex + 1];
            swapl = LootRules.Rules[lstRules.SelectedIndex + 1];
            lstRules.Items[lstRules.SelectedIndex + 1] = lstRules.Items[lstRules.SelectedIndex];
            LootRules.Rules[lstRules.SelectedIndex + 1] = LootRules.Rules[lstRules.SelectedIndex];
            lstRules.Items[lstRules.SelectedIndex] = swap;
            LootRules.Rules[lstRules.SelectedIndex] = swapl;

            SetCurrentReq(null, 0);
            SetCurrentRule(LootRules.Rules[lstRules.SelectedIndex + 1], lstRules.SelectedIndex + 1);
        }

    }
}