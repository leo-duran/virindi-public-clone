using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using IOPath = System.IO.Path;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Decal.Filters;

namespace SpellbarSaver
{
	[MyClasses.MetaViewWrappers.MVView("SpellbarSaver.MainView.xml")]
    [MyClasses.MetaViewWrappers.MVWireUpControlEvents]
	public partial class PluginCore : PluginBase
	{
		private const int AegisRedIcon = 0x060018DC;
		private static string[] msRomanNumerals = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
		private static Dictionary<string, int> msRomanToInt = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		static PluginCore()
		{
			for (int i = 0; i < msRomanNumerals.Length; i++)
			{
				msRomanToInt[msRomanNumerals[i]] = i + 1;
			}
		}

		private class SpellSchools
		{
			public const int War = 1, Life = 2, Item = 3, Creature = 4;
		}

		#region Control References
#pragma warning disable 649
		[MyClasses.MetaViewWrappers.MVControlReference("nbkTabs")]
        private MyClasses.MetaViewWrappers.INotebook nbkTabs;

		private static class SpellList
		{
			public const int Icon = 0, Name = 1, SpellId = 2, OrigSpellId = 3;
		}

		private static class SpellListColors
		{
			public static readonly Color Yellow = Util.ColorRgb(0xEEDD44);
			public static readonly Color Green = Util.ColorRgb(0x33DD11);
			public static readonly Color RedGray = Util.ColorRgb(0xCC9999);
		}

		[MyClasses.MetaViewWrappers.MVControlReferenceArray("lst1", "lst2", "lst3", "lst4", "lst5", "lst6", "lst7")]
		private MyClasses.MetaViewWrappers.IList[] lstSpells;

		[MyClasses.MetaViewWrappers.MVControlReference("choLoad")]
		private MyClasses.MetaViewWrappers.ICombo choLoad;

		[MyClasses.MetaViewWrappers.MVControlReference("edtSaveAs")]
		private MyClasses.MetaViewWrappers.ITextBox edtSaveAs;

		[MyClasses.MetaViewWrappers.MVControlReference("chkReplCrit")]
		private MyClasses.MetaViewWrappers.ICheckBox chkReplCrit;

		[MyClasses.MetaViewWrappers.MVControlReference("choLowCrit")]
		private MyClasses.MetaViewWrappers.ICombo choLowCrit;

		[MyClasses.MetaViewWrappers.MVControlReference("choHighCrit")]
		private MyClasses.MetaViewWrappers.ICombo choHighCrit;

		[MyClasses.MetaViewWrappers.MVControlReference("chkReplLife")]
		private MyClasses.MetaViewWrappers.ICheckBox chkReplLife;

		[MyClasses.MetaViewWrappers.MVControlReference("choLowLife")]
		private MyClasses.MetaViewWrappers.ICombo choLowLife;

		[MyClasses.MetaViewWrappers.MVControlReference("choHighLife")]
		private MyClasses.MetaViewWrappers.ICombo choHighLife;

		[MyClasses.MetaViewWrappers.MVControlReference("chkReplItem")]
		private MyClasses.MetaViewWrappers.ICheckBox chkReplItem;

		[MyClasses.MetaViewWrappers.MVControlReference("choLowItem")]
		private MyClasses.MetaViewWrappers.ICombo choLowItem;

		[MyClasses.MetaViewWrappers.MVControlReference("choHighItem")]
		private MyClasses.MetaViewWrappers.ICombo choHighItem;

		[MyClasses.MetaViewWrappers.MVControlReference("chkReplWar")]
		private MyClasses.MetaViewWrappers.ICheckBox chkReplWar;

		[MyClasses.MetaViewWrappers.MVControlReference("choLowWar")]
		private MyClasses.MetaViewWrappers.ICombo choLowWar;

		[MyClasses.MetaViewWrappers.MVControlReference("choHighWar")]
		private MyClasses.MetaViewWrappers.ICombo choHighWar;

		[MyClasses.MetaViewWrappers.MVControlReference("chkOnlyReplUnknown")]
		private MyClasses.MetaViewWrappers.ICheckBox chkOnlyReplUnknown;

		[MyClasses.MetaViewWrappers.MVControlReference("edtToTab")]
		private MyClasses.MetaViewWrappers.ITextBox edtToTab;

		[MyClasses.MetaViewWrappers.MVControlReference("chkShowAsSaved")]
		private MyClasses.MetaViewWrappers.ICheckBox chkShowAsSaved;

		[MyClasses.MetaViewWrappers.MVControlReference("chkShowAsLoaded")]
		private MyClasses.MetaViewWrappers.ICheckBox chkShowAsLoaded;

		[MyClasses.MetaViewWrappers.MVControlReference("chkAutoBackup")]
		private MyClasses.MetaViewWrappers.ICheckBox chkAutoBackup;

		[MyClasses.MetaViewWrappers.MVControlReference("txtTitle")]
        private MyClasses.MetaViewWrappers.IStaticText txtTitle;
#pragma warning restore 649
		#endregion

		private void MainView_InitializeBeforeSettings()
		{
			txtTitle.Text = Util.PluginNameVer + " by Digero of Leafcull";

			choLoad.Clear();
			choLoad.Add("<Current>", null);
			choLoad.Selected = 0;
		}

		private void MainView_InitializeAfterSettings()
		{
			DirectoryInfo spellbarDir = new DirectoryInfo(Util.FullPath("Spellbars"));
			if (!spellbarDir.Exists)
			{
				spellbarDir.Create();
			}
			else
			{
				SortedDictionary<string, string> profiles = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				foreach (FileInfo file in spellbarDir.GetFiles())
				{
					if (file.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
					{
						string profileName = file.Name.Substring(0, file.Name.Length - 4);
						profiles[profileName] = file.FullName;
					}
				}
				foreach (KeyValuePair<string, string> namePath in profiles)
				{
					choLoad.Add(namePath.Key, namePath.Value);
				}
			}

			edtSaveAs.Text = Core.CharacterFilter.Name;
		}

		private void MainView_Dispose()
		{

		}

		private bool IsDisplayingCurrentChar
		{
			get { return choLoad.Selected == 0; }
		}

		private string ToRomanNumeral(int i)
		{
			if (i >= 1 && i <= msRomanNumerals.Length)
				return msRomanNumerals[i - 1];
			return i.ToString();
		}

		private bool TryParseRomanNumeral(string romanNumeral, out int i)
		{
			return msRomanToInt.TryGetValue(romanNumeral.Trim(), out i) || int.TryParse(romanNumeral, out i);
		}

		private void DisplaySpellTabs(List<Spell>[] spellTabs)
		{
			for (int t = 0; t < spellTabs.Length; t++)
			{
				lstSpells[t].Clear();
				if (spellTabs[t].Count == 0)
				{
					InsertEmptyTabRow(lstSpells[t]);
				}
				else
				{
					for (int i = 0; i < spellTabs[t].Count; i++)
					{
						InsertSpellRow(lstSpells[t], i, spellTabs[t][i]);
					}
				}
			}
		}

		private bool IsEmptyTab(MyClasses.MetaViewWrappers.IList tab)
		{
			return tab.RowCount == 0 || 
				(tab.RowCount == 1 && (string)tab[0][SpellList.Name][0] == EmptyTabString);
		}

		private void InsertEmptyTabRow(MyClasses.MetaViewWrappers.IList tab)
		{
            MyClasses.MetaViewWrappers.IListRow row = tab.Add();
			row[SpellList.Icon][1] = AegisRedIcon;
			row[SpellList.Name][0] = EmptyTabString;
			row[SpellList.SpellId][0] = "-1";
			row[SpellList.OrigSpellId][0] = "-1";
		}

		private void InsertSpellRow(MyClasses.MetaViewWrappers.IList tab, int index, Spell spell)
		{
            MyClasses.MetaViewWrappers.IListRow row = (index >= tab.RowCount) ? tab.Add() : tab.Insert(index);
			UpdateSpellRow(row, index, spell);
		}

		private void RefreshSpellTabs()
		{
			for (int t = 0; t < lstSpells.Length; t++)
			{
				if (!IsEmptyTab(lstSpells[t]))
				{
					for (int r = 0; r < lstSpells[t].RowCount; r++)
					{
						UpdateSpellRow(lstSpells[t][r], r);
					}
				}
			}
		}

        private void UpdateSpellRow(MyClasses.MetaViewWrappers.IListRow row, int index)
		{
			int origSpellId = int.Parse((string)row[SpellList.OrigSpellId][0]);
			Spell origSpell = mFS.SpellTable.GetById(origSpellId);

			UpdateSpellRow(row, index, origSpell);
		}

        private void UpdateSpellRow(MyClasses.MetaViewWrappers.IListRow row, int index, Spell spell)
		{
			Color rowColor = Color.White;
			if (!Core.CharacterFilter.IsSpellKnown(spell.Id))
			{
				rowColor = SpellListColors.RedGray;
			}

			bool replace;
			int minLevel, maxLevel;
			switch (spell.School.Id)
			{
				case SpellSchools.Creature:
					replace = chkReplCrit.Checked;
					minLevel = choLowCrit.Selected + 1;
					maxLevel = choHighCrit.Selected + 1;
					break;
				case SpellSchools.Item:
					replace = chkReplItem.Checked;
					minLevel = choLowItem.Selected + 1;
					maxLevel = choHighItem.Selected + 1;
					break;
				case SpellSchools.Life:
					replace = chkReplLife.Checked;
					minLevel = choLowLife.Selected + 1;
					maxLevel = choHighLife.Selected + 1;
					break;
				case SpellSchools.War:
					replace = chkReplWar.Checked;
					minLevel = choLowWar.Selected + 1;
					maxLevel = choHighWar.Selected + 1;
					break;
				default:
					throw new Exception("Unknown spell school: " + spell.School.Name + " (ID: " + spell.School.Id + ")");
			}

			// Search for the highest level spell known in the group
			int curLevel = 0;
			int bestSpellId = spell.Id, bestLevel = minLevel;
			int displayIcon = spell.IconId;
			string displayName = spell.Name;
			if (!IsDisplayingCurrentChar && replace
					&& (!Core.CharacterFilter.IsSpellKnown(spell.Id) || !chkOnlyReplUnknown.Checked))
			{
				List<SpellAndLevel> group;
				if (SpellIdToGroup.TryGetValue(spell.Id, out group))
				{
					foreach (SpellAndLevel sl in group)
					{
						if (spell.Id == sl.SpellId)
						{
							curLevel = sl.Level;
							if (curLevel < minLevel)
							{
								// Don't upgrade if the current spell is lower than the min level
								bestSpellId = spell.Id;
								break;
							}
						}
						if (Core.CharacterFilter.IsSpellKnown(sl.SpellId)
								&& sl.Level >= bestLevel && sl.Level <= maxLevel)
						{
							bestSpellId = sl.SpellId;
							bestLevel = sl.Level;
						}
					}
					if (bestSpellId != spell.Id)
					{
						if (bestLevel < curLevel)
						{
							rowColor = SpellListColors.Yellow;
						}
						else if (bestLevel > curLevel)
						{
							rowColor = SpellListColors.Green;
						}
						else if (bestLevel == curLevel)
						{
							// The rare case where they may know a spell such as 
							// Dark Flame, but not Flame Bolt V
							rowColor = Color.White;
						}

						if (chkShowAsLoaded.Checked)
						{
							Spell tmp = mFS.SpellTable.GetById(bestSpellId);
							displayIcon = tmp.IconId;
							displayName = tmp.Name;
						}
					}
				}
			}

			row[SpellList.Icon][1] = displayIcon;
			row[SpellList.Name][0] = "(" + (index + 1) + ") " + displayName;
			row[SpellList.Name].Color = rowColor;
			row[SpellList.SpellId][0] = bestSpellId.ToString();
			row[SpellList.OrigSpellId][0] = spell.Id.ToString();
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("nbkTabs", "Change")]
		private void nbkTabs_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				edtToTab.Text = ToRomanNumeral(e.Index + 1);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("lst1", "Selected")]
		[MyClasses.MetaViewWrappers.MVControlEvent("lst2", "Selected")]
		[MyClasses.MetaViewWrappers.MVControlEvent("lst3", "Selected")]
		[MyClasses.MetaViewWrappers.MVControlEvent("lst4", "Selected")]
		[MyClasses.MetaViewWrappers.MVControlEvent("lst5", "Selected")]
		[MyClasses.MetaViewWrappers.MVControlEvent("lst6", "Selected")]
		[MyClasses.MetaViewWrappers.MVControlEvent("lst7", "Selected")]
		private void lstSpells_Selected(object sender, MyClasses.MetaViewWrappers.MVListSelectEventArgs e)
		{
			try
			{

			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("choLoad", "Change")]
		private void choLoad_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				if (mSettingsLoaded && e.Index >= 0)
				{
					if (IsDisplayingCurrentChar)
					{
						DisplaySpellTabs(mSpellTabs);
					}
					else
					{
						DisplaySpellTabs(LoadTabsXml(choLoad.Text[choLoad.Selected]));
					}
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("btnDelete", "Click")]
		private void btnDelete_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
		{
			try
			{
				string profileName = choLoad.Text[choLoad.Selected];
				if (IsDisplayingCurrentChar)
				{
					Util.Warning("You can't delete the <Current> profile.");
				}
				else if (!Util.IsControlDown())
				{
					Util.Warning("Please hold down CTRL and click Delete to delete the " + profileName + " profile.");
				}
				else
				{
					File.Delete(Util.FullPath(@"Spellbars\" + profileName + ".xml"));
					Util.Message("Profile " + profileName + " deleted.");
					int curSel = choLoad.Selected;
					choLoad.Selected = 0;
					choLoad.Remove(curSel);
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("btnSave", "Click")]
		private void btnSave_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
		{
			try
			{
				string profileName = edtSaveAs.Text;
				char[] invalidChars = IOPath.GetInvalidFileNameChars();
				foreach (char invalid in invalidChars)
				{
					if (profileName.Contains(invalid.ToString()))
					{
						string err = "The profile name can't contain any of the following characters:";
						foreach (char invalid2 in invalidChars)
						{
							err += " " + invalid2;
						}
						Util.Error(err);
						return;
					}
				}

				SaveTabsXml(mSpellTabs, profileName, false);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("chkReplCrit", "Change")]
		private void chkReplCrit_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
		{
			try
			{
				RefreshSpellTabs();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("choLowCrit", "Change")]
		private void choLowCrit_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				if (chkReplCrit.Checked)
				{
					RefreshSpellTabs();
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("choHighCrit", "Change")]
		private void choHighCrit_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				if (chkReplCrit.Checked)
				{
					RefreshSpellTabs();
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("chkReplLife", "Change")]
		private void chkReplLife_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
		{
			try
			{
				RefreshSpellTabs();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("choLowLife", "Change")]
		private void choLowLife_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				if (chkReplLife.Checked)
				{
					RefreshSpellTabs();
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("choHighLife", "Change")]
		private void choHighLife_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				if (chkReplLife.Checked)
				{
					RefreshSpellTabs();
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("chkReplItem", "Change")]
		private void chkReplItem_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
		{
			try
			{
				RefreshSpellTabs();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("choLowItem", "Change")]
		private void choLowItem_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				if (chkReplItem.Checked)
				{
					RefreshSpellTabs();
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("choHighItem", "Change")]
		private void choHighItem_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				if (chkReplItem.Checked)
				{
					RefreshSpellTabs();
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("chkReplWar", "Change")]
		private void chkReplWar_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
		{
			try
			{
				RefreshSpellTabs();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("choLowWar", "Change")]
		private void choLowWar_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				if (chkReplWar.Checked)
				{
					RefreshSpellTabs();
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("choHighWar", "Change")]
		private void choHighWar_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				if (chkReplWar.Checked)
				{
					RefreshSpellTabs();
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("chkOnlyReplUnknown", "Change")]
		private void chkOnlyReplUnknown_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
		{
			try
			{
				RefreshSpellTabs();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("btnLoadAll", "Click")]
		private void btnLoadAll_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
		{
			try
			{
				if (IsDisplayingCurrentChar)
				{
					Util.Warning("You can't load the <Current> profile, that's what's already on your spellbars");
					return;
				}

				if (chkAutoBackup.Checked)
				{
					SaveTabsXml(mSpellTabs, Core.CharacterFilter.Name + "-backup", true);
				}

				for (int t = 0; t < NumSpellTabs; t++)
				{
					LoadBar(t, t);
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("edtToTab", "End")]
		private void edtToTab_End(object sender, MyClasses.MetaViewWrappers.MVTextBoxEndEventArgs e)
		{
			try
			{
				int tab;
				if (TryParseRomanNumeral(edtToTab.Text, out tab) && tab >= 1 && tab <= NumSpellTabs)
				{
					edtToTab.Text = ToRomanNumeral(tab);
				}
				else
				{
					tab = nbkTabs.ActiveTab + 1;
					Util.Error("Please enter a number or roman numeral betweeen I and " + ToRomanNumeral(NumSpellTabs) + ".");
				}
				edtToTab.Text = ToRomanNumeral(tab);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("btnLoadCurTab", "Click")]
		private void btnLoadCurTab_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
		{
			try
			{
				if (IsDisplayingCurrentChar)
				{
					Util.Warning("You can't load the <Current> profile, that's what's already on your spellbars");
					return;
				}

				if (chkAutoBackup.Checked)
				{
					SaveTabsXml(mSpellTabs, Core.CharacterFilter.Name + "-backup", true);
				}

				int tab;
				if (TryParseRomanNumeral(edtToTab.Text, out tab) && tab >= 1 && tab <= NumSpellTabs)
				{
					LoadBar(nbkTabs.ActiveTab, tab - 1);
				}
				else
				{
					Util.Error("Invalid destination tab. Must be a number or roman numeral betweeen I and " + ToRomanNumeral(NumSpellTabs) + ".");
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("chkShowAsSaved", "Change")]
		private void chkShowAsSaved_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
		{
			try
			{
				chkShowAsLoaded.Checked = !e.Checked;
				RefreshSpellTabs();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("chkShowAsLoaded", "Change")]
		private void chkShowAsLoaded_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
		{
			try
			{
				chkShowAsSaved.Checked = !e.Checked;
				RefreshSpellTabs();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}
	}
}