///////////////////////////////////////////////////////////////////////////////
//File: Form1.Designer.cs
//
//Description: The Virindi Tank Loot Editor.
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

namespace uTank2_Settings_Editor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstRules = new System.Windows.Forms.ListBox();
            this.cmdNewRule = new System.Windows.Forms.Button();
            this.groupRule = new System.Windows.Forms.GroupBox();
            this.groupReqs = new System.Windows.Forms.GroupBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.cmbKey = new System.Windows.Forms.ComboBox();
            this.lblKey = new System.Windows.Forms.Label();
            this.cmbActsOn = new System.Windows.Forms.ComboBox();
            this.lblActsOn = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbReqType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbAction = new System.Windows.Forms.ComboBox();
            this.txtRuleName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstRequirements = new System.Windows.Forms.ListBox();
            this.cmdDeleteReq = new System.Windows.Forms.Button();
            this.cmdNewReq = new System.Windows.Forms.Button();
            this.cmdDeleteRule = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupRule.SuspendLayout();
            this.groupReqs.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(583, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(149, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // lstRules
            // 
            this.lstRules.FormattingEnabled = true;
            this.lstRules.Location = new System.Drawing.Point(12, 59);
            this.lstRules.Name = "lstRules";
            this.lstRules.Size = new System.Drawing.Size(160, 303);
            this.lstRules.TabIndex = 1;
            this.lstRules.SelectedIndexChanged += new System.EventHandler(this.lstRules_SelectedIndexChanged);
            // 
            // cmdNewRule
            // 
            this.cmdNewRule.Location = new System.Drawing.Point(12, 368);
            this.cmdNewRule.Name = "cmdNewRule";
            this.cmdNewRule.Size = new System.Drawing.Size(75, 23);
            this.cmdNewRule.TabIndex = 2;
            this.cmdNewRule.Text = "New";
            this.cmdNewRule.UseVisualStyleBackColor = true;
            this.cmdNewRule.Click += new System.EventHandler(this.cmdNewRule_Click);
            // 
            // groupRule
            // 
            this.groupRule.Controls.Add(this.groupReqs);
            this.groupRule.Controls.Add(this.label2);
            this.groupRule.Controls.Add(this.cmbAction);
            this.groupRule.Controls.Add(this.txtRuleName);
            this.groupRule.Controls.Add(this.label1);
            this.groupRule.Controls.Add(this.lstRequirements);
            this.groupRule.Controls.Add(this.cmdDeleteReq);
            this.groupRule.Controls.Add(this.cmdNewReq);
            this.groupRule.Location = new System.Drawing.Point(178, 33);
            this.groupRule.Name = "groupRule";
            this.groupRule.Size = new System.Drawing.Size(393, 358);
            this.groupRule.TabIndex = 3;
            this.groupRule.TabStop = false;
            this.groupRule.Text = "Rule";
            // 
            // groupReqs
            // 
            this.groupReqs.Controls.Add(this.txtValue);
            this.groupReqs.Controls.Add(this.lblValue);
            this.groupReqs.Controls.Add(this.cmbKey);
            this.groupReqs.Controls.Add(this.lblKey);
            this.groupReqs.Controls.Add(this.cmbActsOn);
            this.groupReqs.Controls.Add(this.lblActsOn);
            this.groupReqs.Controls.Add(this.label3);
            this.groupReqs.Controls.Add(this.cmbReqType);
            this.groupReqs.Location = new System.Drawing.Point(234, 99);
            this.groupReqs.Name = "groupReqs";
            this.groupReqs.Size = new System.Drawing.Size(153, 253);
            this.groupReqs.TabIndex = 4;
            this.groupReqs.TabStop = false;
            this.groupReqs.Text = "Requirements";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(6, 152);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(142, 20);
            this.txtValue.TabIndex = 10;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(6, 136);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(34, 13);
            this.lblValue.TabIndex = 9;
            this.lblValue.Text = "Value";
            // 
            // cmbKey
            // 
            this.cmbKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKey.FormattingEnabled = true;
            this.cmbKey.Location = new System.Drawing.Point(6, 112);
            this.cmbKey.Name = "cmbKey";
            this.cmbKey.Size = new System.Drawing.Size(142, 21);
            this.cmbKey.TabIndex = 8;
            this.cmbKey.SelectedIndexChanged += new System.EventHandler(this.cmbKey_SelectedIndexChanged);
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Location = new System.Drawing.Point(6, 96);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(25, 13);
            this.lblKey.TabIndex = 7;
            this.lblKey.Text = "Key";
            // 
            // cmbActsOn
            // 
            this.cmbActsOn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActsOn.FormattingEnabled = true;
            this.cmbActsOn.Location = new System.Drawing.Point(6, 72);
            this.cmbActsOn.Name = "cmbActsOn";
            this.cmbActsOn.Size = new System.Drawing.Size(142, 21);
            this.cmbActsOn.TabIndex = 6;
            this.cmbActsOn.SelectedIndexChanged += new System.EventHandler(this.cmbActsOn_SelectedIndexChanged);
            // 
            // lblActsOn
            // 
            this.lblActsOn.AutoSize = true;
            this.lblActsOn.Location = new System.Drawing.Point(6, 56);
            this.lblActsOn.Name = "lblActsOn";
            this.lblActsOn.Size = new System.Drawing.Size(45, 13);
            this.lblActsOn.TabIndex = 5;
            this.lblActsOn.Text = "Acts On";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Requirement Type";
            // 
            // cmbReqType
            // 
            this.cmbReqType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReqType.FormattingEnabled = true;
            this.cmbReqType.Location = new System.Drawing.Point(6, 32);
            this.cmbReqType.Name = "cmbReqType";
            this.cmbReqType.Size = new System.Drawing.Size(142, 21);
            this.cmbReqType.TabIndex = 3;
            this.cmbReqType.SelectedIndexChanged += new System.EventHandler(this.cmbReqType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Action";
            // 
            // cmbAction
            // 
            this.cmbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAction.FormattingEnabled = true;
            this.cmbAction.Location = new System.Drawing.Point(159, 39);
            this.cmbAction.Name = "cmbAction";
            this.cmbAction.Size = new System.Drawing.Size(121, 21);
            this.cmbAction.TabIndex = 2;
            this.cmbAction.SelectedIndexChanged += new System.EventHandler(this.cmdAction_SelectedIndexChanged);
            // 
            // txtRuleName
            // 
            this.txtRuleName.Location = new System.Drawing.Point(159, 13);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.Size = new System.Drawing.Size(228, 20);
            this.txtRuleName.TabIndex = 1;
            this.txtRuleName.TextChanged += new System.EventHandler(this.txtRuleName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rule Name";
            // 
            // lstRequirements
            // 
            this.lstRequirements.FormattingEnabled = true;
            this.lstRequirements.Location = new System.Drawing.Point(6, 115);
            this.lstRequirements.Name = "lstRequirements";
            this.lstRequirements.Size = new System.Drawing.Size(222, 199);
            this.lstRequirements.TabIndex = 0;
            this.lstRequirements.SelectedIndexChanged += new System.EventHandler(this.lstRequirements_SelectedIndexChanged);
            // 
            // cmdDeleteReq
            // 
            this.cmdDeleteReq.Location = new System.Drawing.Point(161, 320);
            this.cmdDeleteReq.Name = "cmdDeleteReq";
            this.cmdDeleteReq.Size = new System.Drawing.Size(67, 23);
            this.cmdDeleteReq.TabIndex = 2;
            this.cmdDeleteReq.Text = "Delete";
            this.cmdDeleteReq.UseVisualStyleBackColor = true;
            this.cmdDeleteReq.Click += new System.EventHandler(this.cmdDeleteReq_Click);
            // 
            // cmdNewReq
            // 
            this.cmdNewReq.Location = new System.Drawing.Point(9, 320);
            this.cmdNewReq.Name = "cmdNewReq";
            this.cmdNewReq.Size = new System.Drawing.Size(63, 23);
            this.cmdNewReq.TabIndex = 1;
            this.cmdNewReq.Text = "New";
            this.cmdNewReq.UseVisualStyleBackColor = true;
            this.cmdNewReq.Click += new System.EventHandler(this.cmdNewReq_Click);
            // 
            // cmdDeleteRule
            // 
            this.cmdDeleteRule.Location = new System.Drawing.Point(97, 368);
            this.cmdDeleteRule.Name = "cmdDeleteRule";
            this.cmdDeleteRule.Size = new System.Drawing.Size(75, 23);
            this.cmdDeleteRule.TabIndex = 4;
            this.cmdDeleteRule.Text = "Delete";
            this.cmdDeleteRule.UseVisualStyleBackColor = true;
            this.cmdDeleteRule.Click += new System.EventHandler(this.cmdDeleteRule_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 20);
            this.button1.TabIndex = 5;
            this.button1.Text = "Move Up";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 400);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdDeleteRule);
            this.Controls.Add(this.groupRule);
            this.Controls.Add(this.cmdNewRule);
            this.Controls.Add(this.lstRules);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupRule.ResumeLayout(false);
            this.groupRule.PerformLayout();
            this.groupReqs.ResumeLayout(false);
            this.groupReqs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ListBox lstRules;
        private System.Windows.Forms.Button cmdNewRule;
        private System.Windows.Forms.GroupBox groupRule;
        private System.Windows.Forms.GroupBox groupReqs;
        private System.Windows.Forms.ListBox lstRequirements;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbAction;
        private System.Windows.Forms.TextBox txtRuleName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbKey;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.ComboBox cmbActsOn;
        private System.Windows.Forms.Label lblActsOn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbReqType;
        private System.Windows.Forms.Button cmdDeleteReq;
        private System.Windows.Forms.Button cmdNewReq;
        private System.Windows.Forms.Button cmdDeleteRule;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Button button1;


    }
}

