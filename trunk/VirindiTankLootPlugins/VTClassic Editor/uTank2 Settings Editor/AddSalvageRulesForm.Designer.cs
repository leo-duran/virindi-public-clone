namespace VTClassic
{
    partial class AddSalvageRulesForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdAddSRGroup = new System.Windows.Forms.ComboBox();
            this.lblAddSRGroup = new System.Windows.Forms.Label();
            this.trbAddSRWork = new System.Windows.Forms.TrackBar();
            this.lblAddSRWork = new System.Windows.Forms.Label();
            this.lblAddSRWorkV = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblAddSRInfo = new System.Windows.Forms.Label();
            this.btnAddSRAdd = new System.Windows.Forms.Button();
            this.btnAddSRCancel = new System.Windows.Forms.Button();
            this.btnAddSRMerge = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trbAddSRWork)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdAddSRGroup
            // 
            this.cmdAddSRGroup.FormattingEnabled = true;
            this.cmdAddSRGroup.Location = new System.Drawing.Point(85, 6);
            this.cmdAddSRGroup.Name = "cmdAddSRGroup";
            this.cmdAddSRGroup.Size = new System.Drawing.Size(227, 21);
            this.cmdAddSRGroup.TabIndex = 0;
            this.cmdAddSRGroup.SelectedIndexChanged += new System.EventHandler(this.cmdAddSRGroup_SelectedIndexChanged);
            // 
            // lblAddSRGroup
            // 
            this.lblAddSRGroup.AutoSize = true;
            this.lblAddSRGroup.Location = new System.Drawing.Point(3, 9);
            this.lblAddSRGroup.Name = "lblAddSRGroup";
            this.lblAddSRGroup.Size = new System.Drawing.Size(76, 13);
            this.lblAddSRGroup.TabIndex = 1;
            this.lblAddSRGroup.Text = "Material Group";
            // 
            // trbAddSRWork
            // 
            this.trbAddSRWork.Location = new System.Drawing.Point(81, 33);
            this.trbAddSRWork.Minimum = 1;
            this.trbAddSRWork.Name = "trbAddSRWork";
            this.trbAddSRWork.Size = new System.Drawing.Size(206, 45);
            this.trbAddSRWork.TabIndex = 2;
            this.trbAddSRWork.Value = 10;
            this.trbAddSRWork.Scroll += new System.EventHandler(this.trbAddSRWork_Scroll);
            // 
            // lblAddSRWork
            // 
            this.lblAddSRWork.AutoSize = true;
            this.lblAddSRWork.Location = new System.Drawing.Point(3, 45);
            this.lblAddSRWork.Name = "lblAddSRWork";
            this.lblAddSRWork.Size = new System.Drawing.Size(72, 13);
            this.lblAddSRWork.TabIndex = 3;
            this.lblAddSRWork.Text = "Workmanship";
            // 
            // lblAddSRWorkV
            // 
            this.lblAddSRWorkV.AutoSize = true;
            this.lblAddSRWorkV.Location = new System.Drawing.Point(293, 45);
            this.lblAddSRWorkV.Name = "lblAddSRWorkV";
            this.lblAddSRWorkV.Size = new System.Drawing.Size(19, 13);
            this.lblAddSRWorkV.TabIndex = 4;
            this.lblAddSRWorkV.Text = "10";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblAddSRInfo);
            this.groupBox1.Location = new System.Drawing.Point(12, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 72);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Materials in this group";
            // 
            // lblAddSRInfo
            // 
            this.lblAddSRInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddSRInfo.Location = new System.Drawing.Point(3, 16);
            this.lblAddSRInfo.Name = "lblAddSRInfo";
            this.lblAddSRInfo.Size = new System.Drawing.Size(294, 53);
            this.lblAddSRInfo.TabIndex = 0;
            this.lblAddSRInfo.Text = "label1";
            // 
            // btnAddSRAdd
            // 
            this.btnAddSRAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAddSRAdd.Location = new System.Drawing.Point(12, 154);
            this.btnAddSRAdd.Name = "btnAddSRAdd";
            this.btnAddSRAdd.Size = new System.Drawing.Size(90, 22);
            this.btnAddSRAdd.TabIndex = 6;
            this.btnAddSRAdd.Text = "Add rules";
            this.btnAddSRAdd.UseVisualStyleBackColor = true;
            // 
            // btnAddSRCancel
            // 
            this.btnAddSRCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAddSRCancel.Location = new System.Drawing.Point(222, 154);
            this.btnAddSRCancel.Name = "btnAddSRCancel";
            this.btnAddSRCancel.Size = new System.Drawing.Size(90, 22);
            this.btnAddSRCancel.TabIndex = 7;
            this.btnAddSRCancel.Text = "Cancel";
            this.btnAddSRCancel.UseVisualStyleBackColor = true;
            // 
            // btnAddSRMerge
            // 
            this.btnAddSRMerge.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnAddSRMerge.Location = new System.Drawing.Point(117, 154);
            this.btnAddSRMerge.Name = "btnAddSRMerge";
            this.btnAddSRMerge.Size = new System.Drawing.Size(90, 22);
            this.btnAddSRMerge.TabIndex = 8;
            this.btnAddSRMerge.Text = "Add/Update rules";
            this.btnAddSRMerge.UseVisualStyleBackColor = true;
            // 
            // AddSalvageRulesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 184);
            this.Controls.Add(this.btnAddSRMerge);
            this.Controls.Add(this.btnAddSRCancel);
            this.Controls.Add(this.btnAddSRAdd);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblAddSRWorkV);
            this.Controls.Add(this.lblAddSRWork);
            this.Controls.Add(this.trbAddSRWork);
            this.Controls.Add(this.lblAddSRGroup);
            this.Controls.Add(this.cmdAddSRGroup);
            this.Name = "AddSalvageRulesForm";
            this.Text = "AddSalvageRulesForm";
            ((System.ComponentModel.ISupportInitialize)(this.trbAddSRWork)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmdAddSRGroup;
        private System.Windows.Forms.Label lblAddSRGroup;
        private System.Windows.Forms.TrackBar trbAddSRWork;
        private System.Windows.Forms.Label lblAddSRWork;
        private System.Windows.Forms.Label lblAddSRWorkV;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblAddSRInfo;
        private System.Windows.Forms.Button btnAddSRAdd;
        private System.Windows.Forms.Button btnAddSRCancel;
        private System.Windows.Forms.Button btnAddSRMerge;
    }
}