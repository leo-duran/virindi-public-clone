///////////////////////////////////////////////////////////////////////////////
//File: Form1.cs
//
//Description: The Virindi Tank Loot Editor. This is a standalone windows program
//  to edit loot settings files. It's pretty messy, and should probably be
//  rewritten someday.
//
//This file is Copyright (c) 2008 VirindiPlugins
//
//The original copy of this code can be obtained from http://www.virindi.net/repos/virindi_public
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

            FileChanged = false;
            FileName = "";
            this.Text = AppName + " - New File";
            SetCurrentReq(null, 0);
            SetCurrentRule(null, 0);

            RefreshTabData();

            InitInfoArea();
        }

        void RefreshTabData()
        {
            Tab_Salvage_Refresh();
        }

        string FileName = "";
        bool FileChanged = false;
        cLootRules LootRules = new cLootRules();
        cLootItemRule CurrentRule;
        int CurrentRuleNum;
        iLootRule CurrentReq;
        int CurrentReqNum;
        bool Working = false;

        bool CtrlPressed;

        List<eLootRuleType> RequirementComboEntries = new List<eLootRuleType>();

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
                lstRules.SelectedIndex = -1;
                lstRules.SelectedIndex = crn;
                groupRule.Visible = true;
                txtRuleName.Text = cr.name;
                cmbAction.Items.Clear();
                cmbAction.Items.AddRange(eLootActionTool.FriendlyNames().ToArray());
                txtKeepCount.Visible = false;
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
                    case eLootAction.KeepUpTo:
                        cmbAction.SelectedIndex = 4;
                        txtKeepCount.Visible = true;
                        txtKeepCount.Text = cr.LootActionData.ToString();
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

        int GetRequirementComboIndexForRuleType(eLootRuleType ruletype)
        {
            for (int i = 0; i < RequirementComboEntries.Count; ++i)
                if (RequirementComboEntries[i] == ruletype)
                    return i;
            return -1;
        }

        void AddRequirementComboEntry(string text, eLootRuleType ruletype)
        {
            cmbReqType.Items.Add(text);
            RequirementComboEntries.Add(ruletype);
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
                RequirementComboEntries.Clear();

                Array arr = Enum.GetValues(typeof(eLootRuleType));
                foreach (int it in arr)
                {
                    eLootRuleType lrt = ((eLootRuleType)it);
                    iLootRule lr = LootRuleCreator.CreateLootRule(lrt);
                    if (lr == null) continue;

                    AddRequirementComboEntry(lr.FriendlyName(), lrt);
                }


                cmbReqType.SelectedIndex = GetRequirementComboIndexForRuleType((eLootRuleType)cr.GetRuleType());

                //*******************
                //Fill control values
                //*******************

                lblActsOn.Visible = cr.UI_ActsOnCombo_Uses();
                cmbActsOn.Visible = lblActsOn.Visible;
                if (lblActsOn.Visible)
                {
                    lblActsOn.Text = cr.UI_ActsOnCombo_Label();
                    cmbActsOn.Items.Clear();
                    foreach (string s in cr.UI_ActsOnCombo_Options())
                        cmbActsOn.Items.Add(s);
                    cmbActsOn.SelectedIndex = cr.UI_ActsOnCombo_Get();
                }

                lblKey.Visible = cr.UI_KeyCombo_Uses();
                cmbKey.Visible = lblKey.Visible;
                if (lblKey.Visible)
                {
                    lblKey.Text = cr.UI_KeyCombo_Label();
                    cmbKey.Items.Clear();
                    foreach (string s in cr.UI_KeyCombo_Options())
                        cmbKey.Items.Add(s);
                    cmbActsOn.SelectedIndex = cr.UI_KeyCombo_Get();
                }

                lblValue.Visible = cr.UI_TextValue_Uses();
                txtValue.Visible = lblValue.Visible;
                if (lblValue.Visible)
                {
                    lblValue.Text = cr.UI_TextValue_Label();
                    txtValue.Text = cr.UI_TextValue_Get();
                }

                lblValue2.Visible = cr.UI_TextValue2_Uses();
                txtValue2.Visible = lblValue2.Visible;
                if (lblValue2.Visible)
                {
                    lblValue2.Text = cr.UI_TextValue2_Label();
                    txtValue2.Text = cr.UI_TextValue2_Get();
                }

                lblValue3.Visible = cr.UI_TextValue3_Uses();
                txtValue3.Visible = lblValue3.Visible;
                if (lblValue3.Visible)
                {
                    lblValue3.Text = cr.UI_TextValue3_Label();
                    txtValue3.Text = cr.UI_TextValue3_Get();
                }

                //*******************
                //*******************
            }

            Working = false;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSave()) return;

            FileChanged = false;
            FileName = "";
            this.Text = AppName + " - New File";
            LootRules = new cLootRules();
            lstRules.Items.Clear();

            SetCurrentReq(null, 0);
            SetCurrentRule(null, 0);

            RefreshTabData();
        }

        string GetVTankProfileDirectory()
        {
            string s = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Decal\\Plugins\\{642F1F48-16BE-48BF-B1D4-286652C4533E}").GetValue("ProfilePath").ToString();
            return System.IO.Path.GetFullPath(s);           
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSave()) return;

            System.Windows.Forms.OpenFileDialog od = new OpenFileDialog();
            od.DefaultExt = ".utl";
            od.Filter = "uTank settings files|*.utl";
            od.InitialDirectory = GetVTankProfileDirectory();
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

                RefreshTabData();
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
            od.InitialDirectory = GetVTankProfileDirectory();
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

            txtKeepCount.Visible = false;
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
                case 4:
                    CurrentRule.act = eLootAction.KeepUpTo;
                    txtKeepCount.Visible = true;
                    break;
            }

            Working = false;
        }

        private void txtKeepCount_TextChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            int.TryParse(txtKeepCount.Text, out CurrentRule.LootActionData);

            Working = false;
        }

        private void cmbReqType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            iLootRule newlr;
            eLootRuleType ruletype = RequirementComboEntries[cmbReqType.SelectedIndex];
            newlr = LootRuleCreator.CreateLootRule(ruletype);
            if (newlr == null)
                newlr = CurrentReq;

            if (newlr.GetType() != CurrentReq.GetType())
            {
                //Change type
                CurrentRule.IntRules[CurrentReqNum] = newlr;
                SetCurrentReq(newlr, CurrentReqNum);
                lstRequirements.Items[CurrentReqNum] = newlr.DisplayString();

                lstRules.Invalidate();
                lstRequirements.Invalidate();
            }

            Working = false;
        }

        private void cmbActsOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            CurrentReq.UI_ActsOnCombo_Set(cmbActsOn.SelectedIndex);

            lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            lstRules.Invalidate();
            lstRequirements.Invalidate();

            Working = false;
        }

        private void cmbKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;

            Working = true;


            CurrentReq.UI_KeyCombo_Set(cmbKey.SelectedIndex);

            lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            lstRules.Invalidate();
            lstRequirements.Invalidate();


            Working = false;
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            CurrentReq.UI_TextValue_Set(txtValue.Text);

            lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            lstRules.Invalidate();
            lstRequirements.Invalidate();

            Working = false;
        }

        void txtValue2_TextChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            CurrentReq.UI_TextValue2_Set(txtValue2.Text);

            lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            lstRules.Invalidate();
            lstRequirements.Invalidate();


            Working = false;
        }

        void txtValue3_TextChanged(object sender, EventArgs e)
        {
            if (Working) return;
            FileChanged = true;
            Working = true;

            CurrentReq.UI_TextValue3_Set(txtValue3.Text);

            lstRequirements.Items[CurrentReqNum] = CurrentReq.DisplayString();
            lstRules.Invalidate();
            lstRequirements.Invalidate();

            Working = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (CtrlPressed)
            {
                ruleMoveUp(lstRules.SelectedIndex, true);
                ruleMoveUp(lstRules.SelectedIndex, true);
            }
            ruleMoveUp(lstRules.SelectedIndex, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CtrlPressed)
            {
                ruleMoveDown(lstRules.SelectedIndex, true);
                ruleMoveDown(lstRules.SelectedIndex, true);
            }
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
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Exception: " + ex.ToString()); }
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
                    SetCurrentRule(CurrentRule, CurrentRuleNum);
                }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Exception: " + ex.ToString()); }   
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
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Exception: " + ex.ToString()); }
            lstRules.Visible = true;
        }

        private void addPackslotRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectMinPackSlotsForm f = new SelectMinPackSlotsForm();
            DialogResult r = f.ShowDialog(this);
            if (r == DialogResult.OK || r == DialogResult.Yes)
            {
                lstRules.Visible = false;
                try
                {
                    SetCurrentReq(null, 0);
                    foreach (cLootItemRule rule in LootRules.Rules)
                    {
                        bool needsReq = true;
                        if (rule.Action() == f.act)
                        {
                            foreach (iLootRule req in rule.IntRules)
                            {
                                if (req.GetRuleType() == eLootRuleType.CharacterMainPackEmptySlotsGE)
                                {
                                    needsReq = false;
                                    if (r == DialogResult.Yes)
                                    {
                                        ((CharacterMainPackEmptySlotsGE)req).keyval = f.slots;
                                        FileChanged = true;
                                    }
                                    break;
                                }
                            }
                            if (needsReq)
                            {
                                rule.IntRules.Add(new CharacterMainPackEmptySlotsGE(f.slots));
                                FileChanged = true;
                            }
                        }
                    }
                }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Exception: " + ex.ToString()); }

                SetCurrentRule(CurrentRule, CurrentRuleNum);
                lstRules.Visible = true;
            }
            f.Dispose();
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            CtrlPressed = e.Control;
        }

        private void tabControl1_KeyUp(object sender, KeyEventArgs e)
        {
            CtrlPressed = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (CurrentRule != null)
            {
                bool disabled = false;
                List<iLootRule> remove = new List<iLootRule>();
                foreach (iLootRule req in CurrentRule.IntRules)
                {
                    if (req.GetRuleType() == eLootRuleType.DisabledRule)
                    {
                        disabled = disabled || ((DisabledRule)req).b;
                        remove.Add(req);
                    }
                }
                foreach (iLootRule req in remove)
                {
                    CurrentRule.IntRules.Remove(req);
                }

                if (!disabled)
                {
                    CurrentRule.IntRules.Add(new DisabledRule(true));
                }
                SetCurrentRule(CurrentRule, CurrentRuleNum);
                FileChanged = true;
            }

        }      

    }
}