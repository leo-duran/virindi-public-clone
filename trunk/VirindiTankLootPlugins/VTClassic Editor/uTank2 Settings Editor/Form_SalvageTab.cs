using System;
using System.Collections.Generic;
using System.Text;

namespace VTClassic
{
    partial class Form1
    {
        VTClassic.UTLBlockHandlers.UTLBlock_SalvageCombine SalvageBlock { get { return LootRules.ExtraBlockManager.GetFirstBlock("SalvageCombine") as VTClassic.UTLBlockHandlers.UTLBlock_SalvageCombine; } }
        bool Tab_Salvage_Working = false;
		const int DEFAULT_VALUECOMBINE_VALUE = 390000;

        List<int> Tab_Salvage_MaterialComboList;
        void Tab_Salvage_GenerateMaterialComboList()
        {
            if (Tab_Salvage_MaterialComboList != null) return;

            Tab_Salvage_MaterialComboList = new List<int>();
            tSC_cmbMaterial.Items.Clear();
            foreach (KeyValuePair<string, int> kp in GameInfo.getMaterialInfo())
            {
                Tab_Salvage_MaterialComboList.Add(kp.Value);
                tSC_cmbMaterial.Items.Add(kp.Key);
            }
        }

        List<int> Tab_Salvage_lstKeys = new List<int>();
        
        void Tab_Salvage_Refresh()
        {
            Tab_Salvage_GenerateMaterialComboList();

            tSC_listCombine.Items.Clear();
            Tab_Salvage_lstKeys.Clear();
            foreach (KeyValuePair<int, string> kp in SalvageBlock.MaterialCombineStrings)
            {
                Tab_Salvage_lstKeys.Add(kp.Key);
                tSC_listCombine.Items.Add("");
				UpdateIndexTitle(tSC_listCombine.Items.Count - 1);
            }

            tSC_txtDefaultCombine.Text = SalvageBlock.DefaultCombineString;

            Tab_Salvage_Select();
        }

        void Tab_Salvage_Select()
        {
            if ((tSC_listCombine.SelectedIndex >= 0) && (tSC_listCombine.SelectedIndex < Tab_Salvage_lstKeys.Count))
            {
				Tab_Salvage_Working = true;

                int mat = Tab_Salvage_lstKeys[tSC_listCombine.SelectedIndex];
                tSC_cmbMaterial.SelectedIndex = Tab_Salvage_MaterialComboList.IndexOf(mat);

                string cmb = SalvageBlock.MaterialCombineStrings[mat];
                tSC_txtCombineRange.Text = cmb;

                tSC_groupCRS.Visible = true;

				bool valuemode = SalvageBlock.MaterialValueModeValues.ContainsKey(mat);
				tSC_chkValueMode.Checked = valuemode;
				tSC_txtValueMode.Visible = valuemode;
				if (valuemode) tSC_txtValueMode.Text = SalvageBlock.MaterialValueModeValues[mat].ToString();

				Tab_Salvage_Working = false;
            }
            else
            {
                tSC_groupCRS.Visible = false;
            }
        }

		void UpdateIndexTitle(int i)
		{
			int mat = Tab_Salvage_lstKeys[i];
			Tab_Salvage_Working = true;
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}: {1}", GameInfo.getMaterialName(mat), SalvageBlock.MaterialCombineStrings[mat]);
			if (SalvageBlock.MaterialValueModeValues.ContainsKey(mat))
			{
				sb.AppendFormat(", Val >= {0}", SalvageBlock.MaterialValueModeValues[mat]);
			}
			tSC_listCombine.Items[i] = sb.ToString();
			Tab_Salvage_Working = false;
		}

        private void tSC_listCombine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tab_Salvage_Working) return;

            Tab_Salvage_Select();
        }

        private void tSC_cmbMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (Tab_Salvage_Working) return;

            if ((tSC_listCombine.SelectedIndex >= 0) && (tSC_listCombine.SelectedIndex < Tab_Salvage_lstKeys.Count))
            {
                //See if we can do this...there can only be one entry for each mat
                int oldmat = Tab_Salvage_lstKeys[tSC_listCombine.SelectedIndex];
                int newmat = Tab_Salvage_MaterialComboList[tSC_cmbMaterial.SelectedIndex];

                if (Tab_Salvage_lstKeys.Contains(newmat))
                {
                    //Rejected!
                    tSC_cmbMaterial.SelectedIndex = Tab_Salvage_MaterialComboList.IndexOf(oldmat);
                    return;
                }

                //Okay, do the actual switch
                Tab_Salvage_lstKeys[tSC_listCombine.SelectedIndex] = newmat;
                SalvageBlock.MaterialCombineStrings[newmat] = SalvageBlock.MaterialCombineStrings[oldmat];
                SalvageBlock.MaterialCombineStrings.Remove(oldmat);

                //Update list
				UpdateIndexTitle(tSC_listCombine.SelectedIndex);

                FileChanged = true;
            }
        }

        private void tSC_txtCombineRange_TextChanged(object sender, EventArgs e)
        {
			if (Tab_Salvage_Working) return;

            if ((tSC_listCombine.SelectedIndex >= 0) && (tSC_listCombine.SelectedIndex < Tab_Salvage_lstKeys.Count))
            {
                int mat = Tab_Salvage_lstKeys[tSC_listCombine.SelectedIndex];

                if (SalvageBlock.MaterialCombineStrings[mat] == tSC_txtCombineRange.Text) return;
                SalvageBlock.MaterialCombineStrings[mat] = tSC_txtCombineRange.Text;

                //Update list
				UpdateIndexTitle(tSC_listCombine.SelectedIndex);

                FileChanged = true;
            }
        }

		private void tSC_chkValueMode_CheckedChanged(object sender, EventArgs e)
		{
			if (Tab_Salvage_Working) return;

			if ((tSC_listCombine.SelectedIndex >= 0) && (tSC_listCombine.SelectedIndex < Tab_Salvage_lstKeys.Count))
			{
				tSC_txtValueMode.Visible = tSC_chkValueMode.Checked;

				if (tSC_chkValueMode.Checked)
				{
					tSC_txtValueMode.Text = DEFAULT_VALUECOMBINE_VALUE.ToString();
				}

				int v = 0;
				if (!int.TryParse(tSC_txtValueMode.Text, out v))
					v = DEFAULT_VALUECOMBINE_VALUE;

				int mat = Tab_Salvage_lstKeys[tSC_listCombine.SelectedIndex];
				if (tSC_chkValueMode.Checked)
				{
					SalvageBlock.MaterialValueModeValues[mat] = v;
				}
				else
				{
					if (SalvageBlock.MaterialValueModeValues.ContainsKey(mat))
						SalvageBlock.MaterialValueModeValues.Remove(mat);
				}

				UpdateIndexTitle(tSC_listCombine.SelectedIndex);

				FileChanged = true;
			}
		}

		private void tSC_txtValueMode_TextChanged(object sender, EventArgs e)
		{
			if (Tab_Salvage_Working) return;

			if ((tSC_listCombine.SelectedIndex >= 0) && (tSC_listCombine.SelectedIndex < Tab_Salvage_lstKeys.Count))
			{
				int v = 0;
				if (!int.TryParse(tSC_txtValueMode.Text, out v))
					v = DEFAULT_VALUECOMBINE_VALUE;

				int mat = Tab_Salvage_lstKeys[tSC_listCombine.SelectedIndex];
				if (tSC_chkValueMode.Checked)
				{
					SalvageBlock.MaterialValueModeValues[mat] = v;
				}

				UpdateIndexTitle(tSC_listCombine.SelectedIndex);

				FileChanged = true;
			}
		}

        private void tSC_txtDefaultCombine_TextChanged(object sender, EventArgs e)
        {
            if (SalvageBlock.DefaultCombineString == tSC_txtDefaultCombine.Text) return;

            SalvageBlock.DefaultCombineString = tSC_txtDefaultCombine.Text;

            FileChanged = true;
        }

        private void tSC_btnNew_Click(object sender, EventArgs e)
        {
            //Find a material to use for the new rule
            int newmat = -1;

            foreach (int m in Tab_Salvage_MaterialComboList)
            {
                if (!Tab_Salvage_lstKeys.Contains(m))
                {
                    newmat = m;
                    break;
                }
            }

            if (newmat == -1)
            {
                //All materials are in use
                return;
            }

            SalvageBlock.MaterialCombineStrings[newmat] = SalvageBlock.DefaultCombineString;
            Tab_Salvage_lstKeys.Add(newmat);
            tSC_listCombine.Items.Add(String.Format("{0}: {1}", GameInfo.getMaterialName(newmat), SalvageBlock.MaterialCombineStrings[newmat]));
            tSC_listCombine.SelectedIndex = tSC_listCombine.Items.Count - 1;

            FileChanged = true;
        }

        private void tSC_btnDelete_Click(object sender, EventArgs e)
        {
            if ((tSC_listCombine.SelectedIndex >= 0) && (tSC_listCombine.SelectedIndex < Tab_Salvage_lstKeys.Count))
            {
                int mat = Tab_Salvage_lstKeys[tSC_listCombine.SelectedIndex];
                int index = tSC_listCombine.SelectedIndex;

                Tab_Salvage_lstKeys.RemoveAt(index);
                tSC_listCombine.Items.RemoveAt(index);
                SalvageBlock.MaterialCombineStrings.Remove(mat);

                //Select the previous item
                if (tSC_listCombine.Items.Count > 0)
                {
                    int newindex = index - 1;
                    if (newindex < 0) newindex = 0;
                    tSC_listCombine.SelectedIndex = newindex;
                }

                FileChanged = true;
            }
        }

        private void exportRangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog od = new System.Windows.Forms.SaveFileDialog();
            od.DefaultExt = ".ucr";
            od.Filter = "VTClassic Combine Ranges|*.ucr";
            od.InitialDirectory = GetVTankProfileDirectory();
            od.ShowDialog();

            if (od.FileName != "")
            {
                using (CountedStreamWriter pf = new CountedStreamWriter(od.FileName))
                {
                    SalvageBlock.Write(pf);
                }
            }
        }

        private void importRangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog od = new System.Windows.Forms.OpenFileDialog();
            od.DefaultExt = ".ucr";
            od.Filter = "VTClassic Combine Ranges|*.ucr";
            od.InitialDirectory = GetVTankProfileDirectory();
            od.ShowDialog();
            if (od.FileName != "")
            {
                using (System.IO.StreamReader pf = new System.IO.StreamReader(od.FileName))
                {
                    SalvageBlock.Read(pf, 0);
                }

                Tab_Salvage_Refresh();

                FileChanged = true;
            }
        }
    }
}
