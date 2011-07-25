using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using WindowsTimer = System.Windows.Forms.Timer;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Decal.Filters;
using Decal.Constants;
using System.IO;

namespace SpellbarSaver
{
	[FriendlyName("Spellbar Saver")]
	[WireUpBaseEvents]
	public partial class PluginCore : PluginBase
	{
		private const string EmptyTabString = "No spells on this tab";
		private const int NumSpellTabs = 7;

		private delegate void QueuedAction();

		private FileService mFS;

		private WindowsTimer mSpellbarChangeTimer;
		private Queue<QueuedAction> mSpellbarChangeQueue;

		private List<Spell>[] mSpellTabs;
		private static Dictionary<int, List<SpellAndLevel>> mSpellIdToGroup = null;

		protected override void Startup()
		{
			try
			{
                MyClasses.MetaViewWrappers.MVWireupHelper.WireupStart(this, Host);



				Util.Initialize("Spellbar Saver", Host, Core, base.Path);

				mSettingsLoaded = false;

				mFS = (FileService)Core.FileService;

				mSpellbarChangeTimer = new WindowsTimer();
				mSpellbarChangeTimer.Interval = 50;
				mSpellbarChangeTimer.Tick += new EventHandler(SpellbarChangeTimer_Tick);

				mSpellbarChangeQueue = new Queue<QueuedAction>();

				mSpellTabs = new List<Spell>[NumSpellTabs];
				for (int t = 0; t < mSpellTabs.Length; t++)
				{
					mSpellTabs[t] = new List<Spell>();
				}

				MainView_InitializeBeforeSettings();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[BaseEvent("CommandLineText")]
		private void Core_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			try
			{
				const string cmd = "/spellbarsaver setinterval";
				if (e.Text.StartsWith(cmd))
				{
					string numStr = e.Text.Substring(cmd.Length).Trim();
					int num = int.Parse(numStr);
					mSpellbarChangeTimer.Interval = num;
					Util.Message("Interval set to " + num + "ms.");
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		protected override void Shutdown()
		{
			try
			{
				SaveSettings();

				if (mSpellbarChangeTimer != null)
				{
					mSpellbarChangeTimer.Dispose();
					mSpellbarChangeTimer = null;
				}

				MainView_Dispose();

				Util.Dispose();


                MyClasses.MetaViewWrappers.MVWireupHelper.WireupEnd(this);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[BaseEvent("Login", "CharacterFilter")]
		private void CharacterFilter_Login(object sender, LoginEventArgs e)
		{
			try
			{
				LoadPlayerSpellbars();
				LoadSettings();
				MainView_InitializeAfterSettings();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		private static Dictionary<int, List<SpellAndLevel>> SpellIdToGroup
		{
			get
			{
				// Lazy-load spell groups XML
				if (mSpellIdToGroup == null)
				{
					mSpellIdToGroup = new Dictionary<int, List<SpellAndLevel>>();
					XmlDocument spellGroupsDoc = new XmlDocument();
					spellGroupsDoc.Load(Util.FullPath("SpellGroups.xml"));
					foreach (XmlElement groupEle in spellGroupsDoc.DocumentElement.GetElementsByTagName("group"))
					{
						XmlNodeList spellEleList = groupEle.GetElementsByTagName("spell");
						List<SpellAndLevel> group = new List<SpellAndLevel>(spellEleList.Count);
						foreach (XmlElement spellEle in spellEleList)
						{
							int id = int.Parse(spellEle.GetAttribute("id"));
							int level = int.Parse(spellEle.GetAttribute("level"));
							group.Add(new SpellAndLevel(id, level));
							mSpellIdToGroup[id] = group;
						}
					}
				}
				return mSpellIdToGroup;
			}
		}

		[BaseEvent("ChangeSpellbar", "CharacterFilter")]
		private void CharacterFilter_ChangeSpellbar(object sender, ChangeSpellbarEventArgs e)
		{
			try
			{
                MyClasses.MetaViewWrappers.IList list = lstSpells[e.Tab];
				switch (e.Type)
				{
					case AddRemoveEventType.Add:
						// Update spellbar structure
						Spell spell = mFS.SpellTable.GetById(e.SpellId);
						if (spell != null)
						{
							try
							{
								mSpellTabs[e.Tab].Insert(e.Slot, spell);
							}
							catch (Exception ex)
							{
								Util.Debug("List size: " + mSpellTabs[e.Tab] + "; Slot: " + e.Slot + "; Spell: " + spell.Name);
								throw;
							}

							// Update spell list if <Current> is selected
							if (IsDisplayingCurrentChar)
							{
								if (mSpellTabs[e.Tab].Count == 1)
								{
									// This was the first spell added to this tab
									list.Clear();
								}

								InsertSpellRow(list, e.Slot, spell);

								// Update the slot numbers on all of the following spells
								for (int r = e.Slot + 1; r < list.RowCount; r++)
								{
									UpdateSpellRow(list[r], r);
								}
							}
						}
						break;

					case AddRemoveEventType.Delete:
						// Update spellbar structure
						for (int i = 0; i < mSpellTabs[e.Tab].Count; i++)
						{
							if (mSpellTabs[e.Tab][i].Id == e.SpellId)
							{
								mSpellTabs[e.Tab].RemoveAt(i);

								// Update spell list if <Current> is selected
								if (IsDisplayingCurrentChar)
								{
									list.Delete(i);

									if (mSpellTabs[e.Tab].Count == 0)
									{
										InsertEmptyTabRow(list);
									}
									else
									{
										for (int r = i; r < list.RowCount; r++)
										{
											// Update the slot numbers on all of the following spells
											UpdateSpellRow(list[r], r);
										}
									}
								}
								break;
							}
						}
						break;
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		private void LoadPlayerSpellbars()
		{
			for (int i = NumSpellTabs - 1; i >= 0; --i)
			{
				ICollection<int> spellIds = Core.CharacterFilter.SpellBar(i);
				List<Spell> spellTab = mSpellTabs[i];
				spellTab.Clear();
				spellTab.Capacity = spellIds.Count;
				foreach (int spellId in spellIds)
				{
					Spell spell = mFS.SpellTable.GetById(spellId);
					if (spell != null)
						spellTab.Add(spell);
				}
			}

			if (IsDisplayingCurrentChar)
			{
				DisplaySpellTabs(mSpellTabs);
			}
		}

		private List<Spell>[] LoadTabsXml(string profileName)
		{
			XmlDocument doc = new XmlDocument();

			doc.Load(Util.FullPath(@"Spellbars\" + profileName + ".xml"));
			XmlNodeList tabNodes = doc.SelectNodes("/spellbars/tab[@number]");
			if (doc.DocumentElement.LocalName != "spellbars")
			{
				Util.Error("Invalid XML file for profile: " + profileName);
				return null;
			}

			List<Spell>[] tabs = new List<Spell>[NumSpellTabs];
			int tabNum = 0;
			foreach (XmlElement tabEle in doc.SelectNodes("/spellbars/tab"))
			{
				if (tabNum >= 0 && tabNum < NumSpellTabs)
				{
					tabs[tabNum] = new List<Spell>();
					foreach (XmlElement spellEle in tabEle.SelectNodes("spell[@id]"))
					{
						int spellId = int.Parse(spellEle.GetAttribute("id"));
						Spell spell = mFS.SpellTable.GetById(spellId);
						if (spell != null)
						{
							tabs[tabNum].Add(spell);
						}
					}
				}
				tabNum++;
			}

			for (int i = 0; i < NumSpellTabs; i++)
			{
				if (tabs[i] == null)
				{
					tabs[i] = new List<Spell>();
					Util.Warning("Spell tab " + (i + 1) + " is missing from profile " + profileName + ".");
				}
			}

			return tabs;
		}

		private void SaveTabsXml(List<Spell>[] spellTabs, string profileName, bool isBackup)
		{
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("spellbars"));
			for (int i = 0; i < spellTabs.Length; i++)
			{
				XmlElement tabEle = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("tab"));
				foreach (Spell spell in spellTabs[i])
				{
					XmlElement spellEle = (XmlElement)tabEle.AppendChild(doc.CreateElement("spell"));
					spellEle.SetAttribute("id", spell.Id.ToString());
					spellEle.SetAttribute("name", spell.Name);
				}
			}

			if (!Directory.Exists(Util.FullPath("Spellbars")))
			{
				Directory.CreateDirectory(Util.FullPath("Spellbars"));
			}

			Util.SaveXml(doc, Util.FullPath(@"Spellbars\" + profileName + ".xml"));
			Util.Message("Spellbars saved: " + profileName);

			// Check if this profile is already in the list
			int index = -1;
			string profileNameLower = profileName.ToLower();
			for (int i = 0; i < choLoad.Count; i++)
			{
				if (choLoad.Text[i].ToLower() == profileNameLower)
				{
					index = i;
					break;
				}
			}

			// If not, add it to the list
			if (index == -1)
			{
				choLoad.Add(profileName, Util.FullPath(@"Spellbars\" + profileName + ".xml"));
				if (!isBackup)
				{
					// Select the newly added profile
					choLoad.Selected = choLoad.Count - 1;
				}
			}
			// If so, select it or refresh the display
			else if (!isBackup)
			{
				if (choLoad.Selected == index)
				{
					// refresh
					DisplaySpellTabs(spellTabs);
				}
				else
				{
					// select, which will cause the tabs to be 
					// loaded in the choLoad_Change event handler
					choLoad.Selected = index;
				}
			}
		}

		private void LoadBar(int sourceTab, int destTab)
		{
			// Clear the destination tab
			foreach (Spell spell in mSpellTabs[destTab])
			{
				DeleteFromSpellTab(destTab, spell.Id);
			}

			// Check if tab is empty
			if ((lstSpells[sourceTab].RowCount == 0) || IsEmptyTab(lstSpells[sourceTab]))
			{
				//Util.Message("Source tab " + ToRomanNumeral(sourceTab + 1) + " is empty... skipping tab.");
				return;
			}

			for (int i = 0; i < lstSpells[sourceTab].RowCount; i++)
			{
				object spellIdString = lstSpells[sourceTab][i][SpellList.SpellId][0];
				int spellId;
				if (spellIdString is string && int.TryParse((string)spellIdString, out spellId) && mFS.SpellTable.GetById(spellId) != null)
				{
					AddToSpellTab(destTab, i, spellId);
				}
				else
				{
					Util.Warning("Could not obtain spell information for spell: " + lstSpells[sourceTab][i][SpellList.Name][0]);
				}
			}
		}

		private void AddToSpellTab(int tab, int index, int spellId)
		{
			mSpellbarChangeQueue.Enqueue(delegate() { Host.Actions.SpellTabAdd(tab, index, spellId); });
			mSpellbarChangeTimer.Start();
		}

		private void DeleteFromSpellTab(int tab, int spellId)
		{
			Host.Actions.SpellTabDelete(tab, spellId);
			//mSpellbarChangeQueue.Enqueue(delegate() { Host.Actions.SpellTabDelete(tab, spellId); });
			//mSpellbarChangeTimer.Start();
		}

		//private void SpellbarChangeTimer_Tick(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        mLoadingSpells = true;
		//        while (mSpellbarChangeQueue.Count > 0)
		//        {
		//            QueuedAction spellbarChange = mSpellbarChangeQueue.Dequeue();
		//            spellbarChange();
		//        }
		//        Util.Message("Spellbar load complete.");
		//        mUpdateRefreshTimer.Start();
		//    }
		//    catch (Exception ex)
		//    {
		//        Util.HandleException(ex);
		//        mLoadingSpells = false;
		//    }
		//    finally
		//    {
		//        mSpellbarChangeTimer.Stop();
		//    }
		//}

		private void SpellbarChangeTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (mSpellbarChangeQueue.Count == 0)
				{
					mSpellbarChangeTimer.Stop();
					Util.Message("Spellbar load complete.");
				}
				else
				{
					QueuedAction spellbarChange = mSpellbarChangeQueue.Dequeue();
					spellbarChange();
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		//        #region Spellbar Tracking
		//        private void NetEcho_EchoServer(Decal.Interop.Net.IMessage2 pMsg)
		//        {
		//            try
		//            {
		//                if (pMsg.Type == MessageTypes.GameEvent)
		//                {
		//                    switch ((int)pMsg.get_Value("event"))
		//                    {
		//                        case GameEvents.LoginCharacter:
		//                            Decal.Interop.Net.IMessageMember options = pMsg.get_Struct("options");

		//                            int flags = (int)options.get_Value("flags");
		//                            int numTabs = ((flags & 0x10) != 0) ? mSpellTabs.Length : 1;

		//                            for (int t = 0; t < numTabs; t++)
		//                            {
		//                                int count = (int)options.get_Value("tab" + (t + 1) + "Count");
		//                                Decal.Interop.Net.IMessageMember tab = options.get_Struct("tab" + (t + 1));
		//                                mSpellTabs[t] = new List<Spell>(count);
		//                                for (int i = 0; i < count; i++)
		//                                {
		//                                    Spell spell = mFS.SpellTable.GetById((int)tab.get_Struct(i).get_Value("spell"));
		//                                    if (spell != null)
		//                                    {
		//                                        mSpellTabs[t].Add(spell);
		//                                    }
		//                                }
		//                            }

		//                            LoginCharacter();
		//                            break;
		//                    }
		//                }
		//            }
		//            catch (Exception ex) { Util.HandleException(ex); }
		//        }

		//        private void NetEcho_EchoClient(Decal.Interop.Net.IMessage2 pMsg)
		//        {
		//#if false
		//            try {
		//                int t;
		//                if (pMsg.Type == MessageTypes.GameAction) {
		//                    switch ((int)pMsg.get_Value("action")) {
		//                        case GameActions.AddSpellToSpellbar:
		//                            t = (int)pMsg.get_Value("spellbar");
		//                            Spell spell = mFS.SpellTable.GetById((int)pMsg.get_Value("spell"));
		//                            if (spell != null) {
		//                                int i = (int)pMsg.get_Value("position");
		//                                mSpellTabs[t].Insert(i, spell);
		//                            }
		//                            break;

		//                        case GameActions.RemoveSpellFromSpellbar:
		//                            t = (int)pMsg.get_Value("spellbar");
		//                            int spellId = (int)pMsg.get_Value("spell");
		//                            for (int i = 0; i < mSpellTabs[t].Count; i++) {
		//                                if (mSpellTabs[t][i].Id == spellId) {
		//                                    mSpellTabs[t].RemoveAt(i);
		//                                    break;
		//                                }
		//                            }
		//                            break;
		//                    }
		//                }
		//            }
		//            catch (Exception ex) { Util.HandleException(ex); }
		//#endif
		//        }
		//        #endregion

	}

	class SpellAndLevel
	{
		int mSpellId, mLevel;
		Spell mCachedSpell;

		public SpellAndLevel(int spellId, int level)
		{
			mSpellId = spellId;
			mLevel = level;
			mCachedSpell = null;
		}

		public int SpellId
		{
			get { return mSpellId; }
			set
			{
				mSpellId = value;
				mCachedSpell = null;
			}
		}

		public int Level
		{
			get { return mLevel; }
			set { mLevel = value; }
		}

		public Spell GetSpell(FileService fs)
		{
			if (mCachedSpell == null)
			{
				mCachedSpell = fs.SpellTable.GetById(mSpellId);
			}
			return mCachedSpell;
		}

		public override string ToString()
		{
			if (mCachedSpell != null)
			{
				return mCachedSpell.Name + " (Level " + Level + ")";
			}
			return "Spell ID: " + SpellId + " (Level " + Level + ")";
		}

		public override bool Equals(object obj)
		{
			if (obj is SpellAndLevel)
			{
				return Equals((SpellAndLevel)obj);
			}
			return false;
		}

		public bool Equals(SpellAndLevel snl)
		{
			return (snl.SpellId == this.SpellId);
		}

		public override int GetHashCode()
		{
			return SpellId;
		}

		public static bool operator ==(SpellAndLevel a, SpellAndLevel b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(SpellAndLevel a, SpellAndLevel b)
		{
			return !a.Equals(b);
		}
	}
}
