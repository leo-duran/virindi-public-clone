///////////////////////////////////////////////////////////////////////////////
//File: PluginCore.cs
//
//Description: The main portion of the SSSort Decal Plugin.
//
//Copyright (c) 2009 Digero (http://decal.acasylum.com/)
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
using System.Drawing;
using System.Xml;
using System.IO;
using WindowsTimer = System.Windows.Forms.Timer;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Decal.Filters;
using Decal.Interop.Inject;
using Decal.Interop.Core;

namespace SSSort
{
	[FriendlyName("SSSort")]
    [MyClasses.MetaViewWrappers.MVView("SSSort.SSSort.xml")]
    [WireUpBaseEvents]
	public class PluginCore : PluginBase
	{
		#region Constants
		private const int SettingsVersion = 1;
		private const string ChatCmd = "sort";
		private const string ChatCmd2 = "sssort";

		private struct Icons
		{
			public const int ArrowUp = 0x060028FC;
			public const int ArrowDown = 0x060028FD;
			public const int PackBackground = 0x060011CE;
			public const int FociBackground = 0x060011F3;
			public const int BlankBackground = 0x060011D0;
			public const int MainPack = 0x0600127E;
		}

		private struct RowColors
		{
			public static readonly Color Ascending = Color.FromArgb(unchecked((int)0xFFCCFFCC));
			public static readonly Color Descending = Color.FromArgb(unchecked((int)0xFFFFBBB5));
		}

		private class RuleInfo
		{
			public readonly string Name, AscText, DescText;
			public readonly bool DefaultAsc, DefaultEnabled;

			public RuleInfo(string name, string ascText, string descText, bool defaultAsc, bool defaultEnabled)
			{
				this.Name = name;
				this.AscText = ascText;
				this.DescText = descText;
				this.DefaultAsc = defaultAsc;
				this.DefaultEnabled = defaultEnabled;
			}
		}

		private static readonly RuleInfo[] ScrollRules = { 
			new RuleInfo("Level", "Low to High", "High to Low", true, false), 
			new RuleInfo("School", "Crit Item Life War", "War Life Item Crit", true, false),
			new RuleInfo("Self/Other", "Self before Other", "Other before Self", true, false), 
			new RuleInfo("Pos/Neg", "Positive Negative", "Negative Positive", true, false), 
			new RuleInfo("Name", "Alphabetical", "Reverse Alpha", true, true),
		};

		private struct ScrollRule
		{
			public const int Level = 0, School = 1, SelfOther = 2, PosNeg = 3, Name = 4;
		}

		private static readonly RuleInfo[] SalvageRules = {
			new RuleInfo("Bag Size", "Partial before Full", "Full before Partial", true, false),
			new RuleInfo("Tink Skill", "Mag Item Armor Wep", "Wep Armor Item Mag", true, true),
			new RuleInfo("Material", "ID asc", "ID desc", true, true),
			new RuleInfo("Workmanship", "Low to High", "High to Low", true, true),
		};

		private struct SalvageRule
		{
			public const int Size = 0, Skill = 1, Material = 2, Work = 3;
		}

		private static readonly RuleInfo[] ManaStoneRules = {
			new RuleInfo("Stone Type", "Charges before Stones", "Stones before Charges", true, false),
			new RuleInfo("Has Mana", "Empty before Filled", "Filled before Empty", true, true),
			new RuleInfo("Stored Mana", "Low to High", "High to Low", true, true),
		};

		private struct ManaStoneRule
		{
			public const int Type = 0, Charged = 1, Mana = 2;
		}

		private static readonly RuleInfo[] AllRules;

		static PluginCore()
		{
			AllRules = new RuleInfo[ScrollRules.Length + SalvageRules.Length + ManaStoneRules.Length];
			int a = 0;
			for (int i = 0; i < ScrollRules.Length; i++)
				AllRules[a++] = ScrollRules[i];
			for (int i = 0; i < SalvageRules.Length; i++)
				AllRules[a++] = SalvageRules[i];
			for (int i = 0; i < ManaStoneRules.Length; i++)
				AllRules[a++] = ManaStoneRules[i];
		}
		#endregion

		private FileService mFS;

		private WindowsTimer mSortTimer;
		private bool mSettingsLoaded = false;

		private List<WorldObject> mSortItems = new List<WorldObject>();
		private List<WorldObject> mWaitingForId = new List<WorldObject>();
		private Dictionary<int, int> mManaStoneCharges = new Dictionary<int, int>();
		private int mSortPackId;
		private bool mShowStart;
		private Queue<int> mQueuedPackIds = new Queue<int>();
		private int mIndex;
		private SortState mSortState = SortState.Idle;
		private bool mSortToBottom;

		private enum SortState { Idle, WaitingForIds, Sorting }

		private WorldObject[] mPlayerPacks = new WorldObject[9];

		private int mMainViewDefaultHeight;

		#region Control References
#pragma warning disable 649
		[MyClasses.MetaViewWrappers.MVControlReference("lblSortItemsIn")]
		private MyClasses.MetaViewWrappers.IStaticText lblSortItemsIn;

        [MyClasses.MetaViewWrappers.MVControlReference("btnAllPacks")]
        private MyClasses.MetaViewWrappers.IButton btnAllPacks;

		[MyClasses.MetaViewWrappers.MVControlReference("btnSort")]
		private MyClasses.MetaViewWrappers.IButton btnSort;

		[MyClasses.MetaViewWrappers.MVControlReference("nbkMode")]
		private MyClasses.MetaViewWrappers.INotebook nbkMode;

		private struct TabMode
		{
			public const int Scrolls = 0, Salvage = 1, ManaStones = 2;
			public const int Count = 3;
		}

		[MyClasses.MetaViewWrappers.MVControlReference("chkSortToBottom")]
		private MyClasses.MetaViewWrappers.ICheckBox chkSortToBottom;

		[MyClasses.MetaViewWrappers.MVControlReference("prgProgress")]
		private MyClasses.MetaViewWrappers.IProgressBar prgProgress;

		[MyClasses.MetaViewWrappers.MVControlReference("lstScroll")]
		private MyClasses.MetaViewWrappers.IList lstScroll;

		[MyClasses.MetaViewWrappers.MVControlReference("lstSalvage")]
		private MyClasses.MetaViewWrappers.IList lstSalvage;

		[MyClasses.MetaViewWrappers.MVControlReference("lstManaStones")]
		private MyClasses.MetaViewWrappers.IList lstManaStones;

		private struct PriorityList
		{
			public const int Enabled = 0, Name = 1, AscDesc = 2, MoveUp = 3, MoveDown = 4, Ascending = 5;
		}

		[MyClasses.MetaViewWrappers.MVControlReference("lblPluginNameVer")]
		private MyClasses.MetaViewWrappers.IStaticText lblPluginNameVer;

		[MyClasses.MetaViewWrappers.MVControlReferenceArray("btnMainPack",
			"btnPack1", "btnPack2", "btnPack3", "btnPack4",
			"btnPack5", "btnPack6", "btnPack7", "btnPack8")]
		private MyClasses.MetaViewWrappers.IImageButton[] packButtons;
#pragma warning restore 649
		#endregion

		#region Plugin Initialization and Termination
        internal MyClasses.MetaViewWrappers.IView MyDView
        {
            get
            {
                return MyClasses.MetaViewWrappers.MVWireupHelper.GetDefaultView(this);
            }
        }

		protected override void Startup()
		{
			try
			{
                MyClasses.MetaViewWrappers.MVWireupHelper.WireupStart(this, Host);

				Util.Initialize("SSSort", Host, base.Path);

				mFS = Core.Filter<FileService>();

				mSortTimer = new WindowsTimer();
				mSortTimer.Interval = 50;
				mSortTimer.Tick += new EventHandler(SortTimer_Tick);

				CommandLineText += new EventHandler<ChatParserInterceptEventArgs>(PluginCore_CommandLineText);

				ScrollComparer = new ScrollComparerClass(this);
				SalvageComparer = new SalvageComparerClass(this);
				ManaStoneComparer = new ManaStoneComparerClass(this);

				LoadSettings();
				UpdateSortButtonText();
				lblPluginNameVer.Text = Util.PluginNameVer;
                mMainViewDefaultHeight = MyDView.Position.Height;
			}
            catch (Exception ex) { Util.HandleException(ex); }
		}

		protected override void Shutdown()
		{
			try
			{
				SaveSettings();

				Util.Dispose();

				if (mSortTimer != null)
				{
					mSortTimer.Stop();
					mSortTimer.Tick -= SortTimer_Tick;
					mSortTimer.Dispose();
					mSortTimer = null;
				}

				CommandLineText -= PluginCore_CommandLineText;

                MyClasses.MetaViewWrappers.MVWireupHelper.WireupEnd(this);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[BaseEvent("LoginComplete", "CharacterFilter")]
		private void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				mPlayerPacks[0] = Core.WorldFilter[Core.CharacterFilter.Id];
				packButtons[0].Background = Icons.PackBackground;
				packButtons[0].SetImages(Icons.MainPack, Icons.MainPack);
				SetPackButtons();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		private void SetPackButtons()
		{
			for (int i = 1; i < mPlayerPacks.Length; i++)
			{
				mPlayerPacks[i] = null;
				packButtons[i].Background = Icons.BlankBackground;
				packButtons[i].SetImages(0, 0);
				packButtons[i].Matte = Color.Black;
			}

			foreach (WorldObject obj in Core.WorldFilter.GetByContainer(Core.CharacterFilter.Id))
			{
				if (obj.Values(LongValueKey.HouseOwner, 0) != 0)
					continue;

				int i = obj.Values(LongValueKey.Slot) + 1;
				if (i >= 1 && i < mPlayerPacks.Length)
				{
					if (obj.ObjectClass == ObjectClass.Container)
					{
						mPlayerPacks[i] = obj;
						packButtons[i].Background = Icons.PackBackground;
						packButtons[i].SetImages(obj.Icon | 0x06000000, obj.Icon | 0x06000000);
					}
					else if (obj.ObjectClass == ObjectClass.Foci)
					{
						packButtons[i].Background = Icons.FociBackground;
						packButtons[i].SetImages(obj.Icon | 0x06000000, obj.Icon | 0x06000000);
					}
				}
			}

			if (mPlayerPacks[8] != null)
			{
				SetViewHeight(mMainViewDefaultHeight);
			}
			else
			{
				SetViewHeight(mMainViewDefaultHeight - 34);
				packButtons[8].Background = 0;
				packButtons[8].SetImages(0, 0);
			}
		}

		private void SetViewRegion(Rectangle rect)
		{
			if (MyDView.Position != rect)
			{
				bool reactivate = MyDView.Activated && MyDView.Position.Size != rect.Size;
				MyDView.Position = rect;
				if (reactivate)
				{
					// Workaround for bug that causes OnDeactivate to be 
					// called when the view size changes
					MyDView.Deactivate();
					MyDView.Activate();
				}
			}
		}

		private void SetViewSize(Size size)
		{
			Rectangle rect = new Rectangle(MyDView.Position.Location, size);
			SetViewRegion(rect);
		}

		private void SetViewHeight(int height)
		{
			Rectangle rect = MyDView.Position;
			rect.Height = height;
			SetViewRegion(rect);
		}

		private void SetViewLocation(Point point)
		{
			Rectangle rect = new Rectangle(point, MyDView.Position.Size);
			SetViewRegion(rect);
		}
		#endregion

		#region Chat Command Handling
		private void PluginCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			try
			{
				if (e.Text.StartsWith("/") || e.Text.StartsWith("@"))
				{
					string text = e.Text.Substring(1);
					if (!text.StartsWith(ChatCmd, StringComparison.OrdinalIgnoreCase) &&
						!text.StartsWith(ChatCmd2, StringComparison.OrdinalIgnoreCase))
					{
						return;
					}
					text = text.ToLower();
					string[] args = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					if (args.Length > 0 && (args[0] == ChatCmd || args[0] == ChatCmd2))
					{
						bool unrecognized = false;
						e.Eat = true;
						if (args.Length == 1)
						{
							// @sssort
							WorldObject selection = Core.WorldFilter[Host.Actions.CurrentSelection];
							if (selection != null && (selection.ObjectClass == ObjectClass.Container
									|| selection.Id == Core.CharacterFilter.Id))
							{
								DoSort(Host.Actions.CurrentSelection, true, true);
							}
							else
							{
								ShowHelp();
							}
						}
						else if (args.Length == 2)
						{
							int packNumber;
							if (int.TryParse(args[1], out packNumber))
							{
								// @sssort <pack number>
								WorldObject pack = null;
								if (packNumber == 0)
								{
									pack = Core.WorldFilter[Core.CharacterFilter.Id];
								}
								else
								{
									foreach (WorldObject obj in Core.WorldFilter.GetByOwner(Core.CharacterFilter.Id))
									{
										if (obj.ObjectClass == ObjectClass.Container &&
												obj.Values(LongValueKey.Slot, 0) + 1 == packNumber)
										{
											pack = obj;
											break;
										}
									}
								}
								if (pack == null)
								{
									int packSlots = Core.WorldFilter[Core.CharacterFilter.Id].Values(LongValueKey.PackSlots, 0);
									Util.Error(packNumber + " is not a valid pack number. The pack number must "
										+ "be between 0 - " + packSlots + " and specify a pack (not a "
										+ "foci). The main pack is 0, and the other packs are numbered "
										+ "sequentially from 1.");
								}
								else
								{
									DoSort(pack.Id, true, true);
								}
							}
							else if (args[1] == "stop")
							{
								CancelSort();
							}
							else if (args[1] == "help")
							{
								ShowHelp();
							}
							else { unrecognized = true; }
						}
						else if (args.Length >= 3)
						{
							if (args[1] == "mode")
							{
								if (args[2] == "scrolls" || args[2] == "scroll")
								{
									nbkMode.ActiveTab = TabMode.Scrolls;
								}
								else if (args[2] == "salvage")
								{
									nbkMode.ActiveTab = TabMode.Salvage;
								}
								else if (args[2] == "mana" || args[2] == "stones" || args[2] == "manastones")
								{
									nbkMode.ActiveTab = TabMode.ManaStones;
								}
								else
								{
									Util.Error("Mode must be either scrolls or salvage.");
								}
							}
							else { unrecognized = true; }
						}
						else { unrecognized = true; }

						if (unrecognized)
							Util.Error("Unrecognized command. Type /" + ChatCmd + " help for a list of commands.");
					}
				}

			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		private void ShowHelp()
		{
			Util.Message("You're using " + Util.PluginNameVer + ". Here are the commands available:");
			Util.AddChatText(
				"/" + ChatCmd + "  -  If a pack is selected, sort the selected pack (equivalent to pressing the sort button), otherwise show this message.\n" +
				"/" + ChatCmd + " <pack number>  -  Sort the given pack (0 is the main pack, 1 is the first side pack, etc)\n" +
				"/" + ChatCmd + " stop  -  Stop sorting\n" +
				"/" + ChatCmd + " mode scrolls|salvage|mana  -  Set the sorting mode to scrolls, salvage, or mana stones\n" +
				"/" + ChatCmd + " help  -  Show this help message", 0);
		}
		#endregion

		#region Sorting
		private ScrollComparerClass ScrollComparer;
		private class ScrollComparerClass : IComparer<WorldObject>
		{
			private PluginCore parent;

			public ScrollComparerClass(PluginCore parent)
			{
				this.parent = parent;
			}

			public int Compare(WorldObject x, WorldObject y)
			{
				if ((object)x == null)
					return ((object)y == null) ? 0 : -1;
				if ((object)y == null)
					return 1;

				Spell xSpell = parent.mFS.SpellTable.GetById(x.Values(LongValueKey.AssociatedSpell, 0));
				Spell ySpell = parent.mFS.SpellTable.GetById(y.Values(LongValueKey.AssociatedSpell, 0));

				int retVal = 0;
				bool done = false;
				for (int r = 0; r < parent.lstScroll.RowCount; r++)
				{
					if ((bool)parent.lstScroll[r][PriorityList.Enabled][0])
					{
						string comparisonType = (string)parent.lstScroll[r][PriorityList.Name][0];

						if (comparisonType == ScrollRules[ScrollRule.School].Name)
						{
							retVal = -(xSpell.School.Id - ySpell.School.Id);
							done = (retVal != 0);
						}
						else if (comparisonType == ScrollRules[ScrollRule.Level].Name)
						{
							retVal = x.Values(LongValueKey.Value, 0) - y.Values(LongValueKey.Value, 0);
							done = (retVal != 0);
						}
						else if (comparisonType == ScrollRules[ScrollRule.SelfOther].Name)
						{
							retVal = -((xSpell.Flags & 0x8) - (ySpell.Flags & 0x8));
							done = (retVal != 0);
						}
						else if (comparisonType == ScrollRules[ScrollRule.PosNeg].Name)
						{
							retVal = (xSpell.Flags & 0x10) - (ySpell.Flags & 0x10);
							done = (retVal != 0);
						}
						else if (comparisonType == ScrollRules[ScrollRule.Name].Name)
						{
							retVal = StringComparer.Ordinal.Compare(x.Name, y.Name);
							done = (retVal != 0);
						}
						else
						{
							throw new Exception("Invalid compare method: " + comparisonType);
						}
					}
					if (done)
					{
						if (!(bool)parent.lstScroll[r][PriorityList.Ascending][0])
						{
							retVal = -retVal;
						}
						return retVal;
					}
				}

				// If they're equal, keep the order the same
				return x.Values(LongValueKey.Slot, 0) - y.Values(LongValueKey.Slot, 0);
			}
		}

		private SalvageComparerClass SalvageComparer;
		private class SalvageComparerClass : IComparer<WorldObject>
		{
			private PluginCore parent;

			public SalvageComparerClass(PluginCore parent)
			{
				this.parent = parent;
			}

			public int Compare(WorldObject x, WorldObject y)
			{
				if ((object)x == null)
					return ((object)y == null) ? 0 : -1;
				if ((object)y == null)
					return 1;

				int retVal = 0;
				bool done = false;
				for (int r = 0; r < parent.lstSalvage.RowCount; r++)
				{
					if ((bool)parent.lstSalvage[r][PriorityList.Enabled][0])
					{
						string comparisonType = (string)parent.lstSalvage[r][PriorityList.Name][0];

						if (comparisonType == SalvageRules[SalvageRule.Skill].Name)
						{
							retVal = x.Icon - y.Icon;
							done = (retVal != 0);
						}
						else if (comparisonType == SalvageRules[SalvageRule.Material].Name)
						{
							retVal = x.Values(LongValueKey.Material, 0) - y.Values(LongValueKey.Material, 0);
							done = (retVal != 0);
						}
						else if (comparisonType == SalvageRules[SalvageRule.Size].Name)
						{
							int xUses = x.Values(LongValueKey.UsesRemaining, 0);
							int yUses = y.Values(LongValueKey.UsesRemaining, 0);

							if ((xUses == 100) != (yUses == 100))
							{
								retVal = xUses - yUses;
								done = true;
							}
						}
						else if (comparisonType == SalvageRules[SalvageRule.Work].Name)
						{
							double xWork = x.Values(DoubleValueKey.SalvageWorkmanship, 0);
							double yWork = y.Values(DoubleValueKey.SalvageWorkmanship, 0);

							retVal = Math.Sign(xWork - yWork);
							done = (retVal != 0);
						}
						else
						{
							throw new Exception("Invalid compare method: " + comparisonType);
						}
					}
					if (done)
					{
						if (!(bool)parent.lstSalvage[r][PriorityList.Ascending][0])
							return -retVal;
						return retVal;
					}
				}

				// If they're equal, keep the order the same
				return x.Values(LongValueKey.Slot, 0) - y.Values(LongValueKey.Slot, 0);
			}
		}

		private ManaStoneComparerClass ManaStoneComparer;
		private class ManaStoneComparerClass : IComparer<WorldObject>
		{
			private PluginCore parent;

			public ManaStoneComparerClass(PluginCore parent)
			{
				this.parent = parent;
			}

			public int Compare(WorldObject x, WorldObject y)
			{
				if ((object)x == null)
					return ((object)y == null) ? 0 : -1;
				if ((object)y == null)
					return 1;

				int retVal = 0;
				bool done = false;
				for (int r = 0; r < parent.lstManaStones.RowCount; r++)
				{
					if ((bool)parent.lstManaStones[r][PriorityList.Enabled][0])
					{
						string comparisonType = (string)parent.lstManaStones[r][PriorityList.Name][0];

						if (comparisonType == ManaStoneRules[ManaStoneRule.Type].Name)
						{
							int xCharge = x.Name.EndsWith("Charge") ? 0 : 1;
							int yCharge = y.Name.EndsWith("Charge") ? 0 : 1;
							retVal = xCharge - yCharge;
							done = (retVal != 0);
						}
						else if (comparisonType == ManaStoneRules[ManaStoneRule.Charged].Name)
						{
							int xMana, yMana;
							parent.mManaStoneCharges.TryGetValue(x.Id, out xMana);
							parent.mManaStoneCharges.TryGetValue(y.Id, out yMana);
							if ((xMana == 0) != (yMana == 0))
							{
								retVal = xMana - yMana;
								done = (retVal != 0);
							}
						}
						else if (comparisonType == ManaStoneRules[ManaStoneRule.Mana].Name)
						{
							int xMana, yMana;
							parent.mManaStoneCharges.TryGetValue(x.Id, out xMana);
							parent.mManaStoneCharges.TryGetValue(y.Id, out yMana);
							retVal = xMana - yMana;
							done = (retVal != 0);
						}
						else
						{
							throw new Exception("Invalid compare method: " + comparisonType);
						}
					}
					if (done)
					{
						if (!(bool)parent.lstManaStones[r][PriorityList.Ascending][0])
							return -retVal;
						return retVal;
					}
				}

				// If they're equal, keep the order the same
				return x.Values(LongValueKey.Slot, 0) - y.Values(LongValueKey.Slot, 0);
			}
		}

		private void DoSort(int sortPackId, bool showStart, bool warnIfNothingToSort)
		{
			mSortItems.Clear();
			mWaitingForId.Clear();

			WorldObject pack = Core.WorldFilter[sortPackId];
			if (pack == null || (pack.ObjectClass != ObjectClass.Container && pack.Id != Core.CharacterFilter.Id))
			{
				if (warnIfNothingToSort)
				{
					Util.Warning("Select a pack to sort");
				}
			}
			else
			{
				Predicate<WorldObject> sortable;
				IComparer<WorldObject> comparer;
				string itemType;
				bool needsId;
				switch (nbkMode.ActiveTab)
				{
					case TabMode.Scrolls:
						sortable = delegate(WorldObject obj) { return obj.Name.Contains("Scroll"); };
						comparer = ScrollComparer;
						itemType = "scrolls";
						needsId = false;
						break;
					case TabMode.Salvage:
						sortable = delegate(WorldObject obj) { return obj.ObjectClass == ObjectClass.Salvage; };
						comparer = SalvageComparer;
						itemType = "salvage";
						needsId = false;
						break;
					case TabMode.ManaStones:
						sortable = delegate(WorldObject obj) { return obj.ObjectClass == ObjectClass.ManaStone; };
						comparer = ManaStoneComparer;
						itemType = "mana stones";
						needsId = true;
						break;
					default:
						throw new Exception("Invalid tab: " + nbkMode.ActiveTab);
				}

				if (needsId)
				{
					foreach (WorldObject obj in Core.WorldFilter.GetByContainer(sortPackId))
					{
						if (sortable(obj) && !mManaStoneCharges.ContainsKey(obj.Id))
						{
							mWaitingForId.Add(obj);
						}
					}
				}

				if (mWaitingForId.Count > 0)
				{
                    //TODO: Progress properties
					//prgProgress.PreText = "Identifying: ";
					//prgProgress.PostText = "/" + mWaitingForId.Count;
					prgProgress.Value = 0;
					//prgProgress.FillColor = Color.White;
					prgProgress.MaxValue = mWaitingForId.Count;
					//prgProgress.DrawText = true;
					mSortState = SortState.WaitingForIds;
					mSortPackId = sortPackId;
					mShowStart = showStart;
					for (int i = mWaitingForId.Count - 1; i >= 0; --i)
					{
						Host.Actions.RequestId(mWaitingForId[i].Id);
					}
					if (showStart)
					{
						Util.Message("Identifying " + mWaitingForId.Count + " items...");
					}
				}
				else
				{
					foreach (WorldObject obj in Core.WorldFilter.GetByContainer(sortPackId))
					{
						if (sortable(obj))
						{
							int idx = mSortItems.BinarySearch(obj, comparer);
							mSortItems.Insert((idx < 0) ? ~idx : idx, obj);
						}
					}

					mSortToBottom = chkSortToBottom.Checked;
					mIndex = mSortToBottom ? mSortItems.Count - 1 : 0;
					if (mSortItems.Count == 0)
					{
						if (warnIfNothingToSort)
						{
							Util.Warning("The selected pack doesn't contain any " + itemType + ".");
						}
					}
					else
					{
						int maxPackSlot = CalcMaxPackSlot(sortPackId);
						bool inOrder = true;
						for (int i = 0; i < mSortItems.Count; i++)
						{
							if (mSortItems[i].Values(LongValueKey.Slot, 0) != CalcPackSlot(i, maxPackSlot))
							{
								inOrder = false;
								break;
							}
						}
						if (inOrder)
						{
							if (warnIfNothingToSort)
							{
								Util.Warning("The pack is already sorted.");
							}
							mSortState = SortState.Idle;
						}
						else
						{
							if (showStart)
							{
								Util.Message("Sorting " + mSortItems.Count + " items...");
							}
                            //TODO: Progress properties
							//prgProgress.PreText = "Sorting: ";
							//prgProgress.PostText = "/" + mSortItems.Count;
							prgProgress.Value = 0;
							//prgProgress.FillColor = Color.White;
							prgProgress.MaxValue = mSortItems.Count;
							//prgProgress.DrawText = true;
							mSortState = SortState.Sorting;
							mSortTimer.Start();
							UpdateSortButtonText();
						}
					}
				}
			}

			if (mSortState == SortState.Idle)
			{
				if (mQueuedPackIds.Count > 0)
				{
					DoSort(mQueuedPackIds.Dequeue(), false, false);
				}
				else
				{
                    //TODO: Progress properties
                    prgProgress.Value = 0;
					//prgProgress.DrawText = false;
					UpdateSortButtonText();
					Util.Message("Sort complete");
				}
			}
		}

		private void CancelSort()
		{
			if (mSortState != SortState.Idle)
			{
				mWaitingForId.Clear();
				mSortState = SortState.Idle;
				Util.Message("Sorting Cancelled");

                //TODO: Progress properties
				prgProgress.Value = 0;
				//prgProgress.DrawText = false;
				mSortTimer.Stop();
				mQueuedPackIds.Clear();
				UpdateSortButtonText();
			}
			else
			{
				Util.Message("There is no sort in progress");
			}
		}

		[BaseEvent("ChangeObject", "WorldFilter")]
		private void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (e.Change == WorldChangeType.IdentReceived)
				{
					if (e.Changed.ObjectClass == ObjectClass.ManaStone)
					{
						mManaStoneCharges[e.Changed.Id] = e.Changed.Values(LongValueKey.CurrentMana);
					}

					if (mSortState == SortState.WaitingForIds)
					{
						for (int i = mWaitingForId.Count - 1; i >= 0; --i)
						{
							if (mWaitingForId[i].Id == e.Changed.Id)
							{
								mWaitingForId.RemoveAt(i);
								prgProgress.Value++;
								break;
							}
						}
						if (mWaitingForId.Count == 0)
						{
                            //TODO: Progress properties
							prgProgress.Value = 0;
							//prgProgress.DrawText = false;
							DoSort(mSortPackId, mShowStart, false);
						}
					}
				}
				else if (e.Change == WorldChangeType.StorageChange)
				{
					if (mSortState == SortState.Sorting && mSortItems[mIndex].Id == e.Changed.Id)
					{
						while (mSortItems[mIndex].Values(LongValueKey.Slot, 0) == CalcPackSlot(mIndex))
						{
							mIndex += chkSortToBottom.Checked ? -1 : 1;
							if (mIndex < 0 || mIndex >= mSortItems.Count)
							{
								mSortState = SortState.Idle;
								//prgProgress.Value = 0;
								//prgProgress.DrawText = false;
								mSortTimer.Stop();
								UpdateSortButtonText();
								mSortItems.Clear();
								if (mQueuedPackIds.Count > 0)
								{
									DoSort(mQueuedPackIds.Dequeue(), false, false);
								}
								else
								{
									Util.Message("Sort complete");
								}
								break;
							}
						}
						prgProgress.Value = mSortToBottom ? mSortItems.Count - 1 - mIndex : mIndex;
					}
					else if (e.Changed.ObjectClass == ObjectClass.Container
							|| e.Changed.ObjectClass == ObjectClass.Foci)
					{
						SetPackButtons();
					}
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[BaseEvent("ServerDispatch")]
		private void PluginCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				// Set Object DWORD
				if (e.Message.Type == 0x02CE)
				{
					// Charged boolean; set when the stone is filled or emptied
					if (e.Message.Value<int>("key") == 0x12)
					{
						// Remove cached mana value
						mManaStoneCharges.Remove(e.Message.Value<int>("object"));
					}
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[BaseEvent("ReleaseObject", "WorldFilter")]
		private void WorldFilter_ReleaseObject(object sender, ReleaseObjectEventArgs e)
		{
			try
			{
				mManaStoneCharges.Remove(e.Released.Id);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		private void SortTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (mSortState == SortState.Sorting)
				{
					Host.Actions.MoveItem(mSortItems[mIndex].Id, mSortItems[mIndex].Container, CalcPackSlot(mIndex), false);
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		private int CalcMaxPackSlot(int packId)
		{
			int maxPackSlot = 0;
			foreach (WorldObject obj in Core.WorldFilter.GetByContainer(packId))
			{
				int packSlot = obj.Values(LongValueKey.Slot, -1);
				if (packSlot > maxPackSlot)
					maxPackSlot = packSlot;
			}
			return maxPackSlot;
		}

		private int CalcPackSlot(int index)
		{
			if (mSortToBottom)
				return CalcMaxPackSlot(mSortItems[mIndex].Container) + 1 - mSortItems.Count + index;
			else
				return index;
		}

		private int CalcPackSlot(int index, int maxPackSlot)
		{
			if (mSortToBottom)
				return maxPackSlot + 1 - mSortItems.Count + index;
			else
				return index;
		}
		#endregion

		#region Control Event Handlers
		private void UpdateSortButtonText()
		{
            System.Drawing.Rectangle Rpos = btnSort.LayoutPosition;
            int x = Rpos.Left;
            int y = Rpos.Top;
            int w = Rpos.Width;
            int h = Rpos.Height;
            
			if (mSortState != SortState.Idle)
			{
				btnSort.Text = "Stop Sorting";
                w = 224;
                btnAllPacks.Visible = false;
			}
			else
			{
				btnSort.Text = "Selected Pack";
				w = 108;
                btnAllPacks.Visible = true;
			}
            btnSort.LayoutPosition = new Rectangle(x, y, w, h);

			switch (nbkMode.ActiveTab)
			{
				case TabMode.Scrolls:
					lblSortItemsIn.Text = "Sort Scrolls in:";
					break;
				case TabMode.Salvage:
					lblSortItemsIn.Text = "Sort Salvage in:";
					break;
				case TabMode.ManaStones:
					lblSortItemsIn.Text = "Sort Mana Stones in:";
					break;
				default:
					throw new Exception("Invalid mode tab: " + nbkMode.ActiveTab);
			}
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("nbkMode", "Change")]
		private void nbkMode_Change(object sender, MyClasses.MetaViewWrappers.MVIndexChangeEventArgs e)
		{
			try
			{
				UpdateSortButtonText();
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("btnSort", "Click")]
		private void btnSort_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
		{
			try
			{
				if (mSortState != SortState.Idle)
				{
					CancelSort();
				}
				else
				{
					DoSort(Host.Actions.CurrentSelection, false, true);
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("lstScroll", "Selected")]
		private void lstScroll_Selected(object sender, MyClasses.MetaViewWrappers.MVListSelectEventArgs e)
		{
			try
			{
				HandleListClick(lstScroll, e.Row, e.Column);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("lstSalvage", "Selected")]
		private void lstSalvage_Selected(object sender, MyClasses.MetaViewWrappers.MVListSelectEventArgs e)
		{
			try
			{
				HandleListClick(lstSalvage, e.Row, e.Column);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("lstManaStones", "Selected")]
		private void lstManaStones_Selected(object sender, MyClasses.MetaViewWrappers.MVListSelectEventArgs e)
		{
			try
			{
				HandleListClick(lstManaStones, e.Row, e.Column);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		private void HandleListClick(MyClasses.MetaViewWrappers.IList lst, int row, int col)
		{
			switch (col)
			{
				case PriorityList.Name:
					lst[row][PriorityList.Enabled][0] = !(bool)lst[row][PriorityList.Enabled][0];
					break;

				case PriorityList.AscDesc:
					foreach (RuleInfo rule in AllRules)
					{
						string clickedText = (string)lst[row][PriorityList.AscDesc][0];
						if (clickedText == rule.AscText)
						{
							lst[row][PriorityList.AscDesc][0] = rule.DescText;
							lst[row][PriorityList.Ascending][0] = false;
							lst[row][PriorityList.AscDesc].Color = RowColors.Descending;
							break;
						}
						else if (clickedText == rule.DescText)
						{
							lst[row][PriorityList.AscDesc][0] = rule.AscText;
							lst[row][PriorityList.Ascending][0] = true;
							lst[row][PriorityList.AscDesc].Color = RowColors.Ascending;
							break;
						}
					}
					break;

				case PriorityList.MoveUp:
					if (row > 0)
					{
						object enabled, name, ascDescText, ascending;
						Color ascDescColor;

						enabled = lst[row][PriorityList.Enabled][0];
						name = lst[row][PriorityList.Name][0];
						ascDescText = lst[row][PriorityList.AscDesc][0];
						ascending = lst[row][PriorityList.Ascending][0];
						ascDescColor = lst[row][PriorityList.AscDesc].Color;

						lst[row][PriorityList.Enabled][0] = lst[row - 1][PriorityList.Enabled][0];
						lst[row][PriorityList.Name][0] = lst[row - 1][PriorityList.Name][0];
						lst[row][PriorityList.AscDesc][0] = lst[row - 1][PriorityList.AscDesc][0];
						lst[row][PriorityList.Ascending][0] = lst[row - 1][PriorityList.Ascending][0];
						lst[row][PriorityList.AscDesc].Color = lst[row - 1][PriorityList.AscDesc].Color;

						lst[row - 1][PriorityList.Enabled][0] = enabled;
						lst[row - 1][PriorityList.Name][0] = name;
						lst[row - 1][PriorityList.AscDesc][0] = ascDescText;
						lst[row - 1][PriorityList.Ascending][0] = ascending;
						lst[row - 1][PriorityList.AscDesc].Color = ascDescColor;
					}
					break;

				case PriorityList.MoveDown:
					if (row < lst.RowCount - 1)
					{
						HandleListClick(lst, row + 1, PriorityList.MoveUp);
					}
					break;
			}
			if (mSortState != SortState.Idle)
			{
				Util.Warning("Warning: You must restart sorting for changes to take effect");
			}
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("btnMainPack", "Click")]
		[MyClasses.MetaViewWrappers.MVControlEvent("btnPack1", "Click")]
		[MyClasses.MetaViewWrappers.MVControlEvent("btnPack2", "Click")]
		[MyClasses.MetaViewWrappers.MVControlEvent("btnPack3", "Click")]
		[MyClasses.MetaViewWrappers.MVControlEvent("btnPack4", "Click")]
		[MyClasses.MetaViewWrappers.MVControlEvent("btnPack5", "Click")]
		[MyClasses.MetaViewWrappers.MVControlEvent("btnPack6", "Click")]
		[MyClasses.MetaViewWrappers.MVControlEvent("btnPack7", "Click")]
		[MyClasses.MetaViewWrappers.MVControlEvent("btnPack8", "Click")]
		private void btnMainPack_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
		{
			try
			{
				// Figure out which button was clicked
				for (int i = 0; i < packButtons.Length; i++)
				{
					if (packButtons[i].Id == e.Id)
					{
						if (mPlayerPacks[i] != null)
						{
							DoSort(mPlayerPacks[i].Id, false, true);
						}
						else
						{
							Util.Warning("No pack in slot #" + i);
						}
						break;
					}
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		[MyClasses.MetaViewWrappers.MVControlEvent("btnAllPacks", "Click")]
		private void btnAllPacks_Click(object sender, MyClasses.MetaViewWrappers.MVControlEventArgs e)
		{
			try
			{
				if (mSortState != SortState.Idle) { CancelSort(); }

				for (int i = 0; i < mPlayerPacks.Length; i++)
				{
					if (mPlayerPacks[i] != null)
					{
						mQueuedPackIds.Enqueue(mPlayerPacks[i].Id);
					}
				}
				DoSort(Core.CharacterFilter.Id, false, false);
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}
		#endregion

		#region Settings
		private void AddPriority(MyClasses.MetaViewWrappers.IList list, RuleInfo info, bool ascending, bool enabled, int choiceCount)
		{
			for (int r = 0; r < list.RowCount; r++)
			{
				if (info.Name.Equals(list[r][PriorityList.Name][0]))
					return;
			}

            MyClasses.MetaViewWrappers.IListRow row = list.Add();
			row[PriorityList.Enabled][0] = enabled;
			row[PriorityList.Name][0] = info.Name;
			row[PriorityList.Ascending][0] = ascending;
			if (ascending)
			{
				row[PriorityList.AscDesc][0] = info.AscText;
				row[PriorityList.AscDesc].Color = RowColors.Ascending;
			}
			else
			{
				row[PriorityList.AscDesc][0] = info.DescText;
				row[PriorityList.AscDesc].Color = RowColors.Descending;
			}
			if (list.RowCount > 1)
			{
				row[PriorityList.MoveUp][1] = Icons.ArrowUp;
			}
			if (list.RowCount < choiceCount)
			{
				row[PriorityList.MoveDown][1] = Icons.ArrowDown;
			}
		}

		private void LoadSettings()
		{
			try
			{
				FileInfo settingsFile = new FileInfo(Util.FullPath("settings_sssort.xml"));
				if (!settingsFile.Exists)
					return;

				XmlDocument doc = new XmlDocument();
				doc.Load(settingsFile.FullName);

				int fileVer;
				int.TryParse(doc.DocumentElement.GetAttribute("version"), out fileVer);

				string val;
				int intVal;
				bool boolVal;
				Point pointVal;

				foreach (XmlElement ele in doc.DocumentElement.SelectNodes("option"))
				{
					val = ele.GetAttribute("value");

					switch (ele.GetAttribute("name"))
					{
						case "ViewPosition":
							if (TryParsePoint(val, out pointVal))
							{
								MyDView.Position = new Rectangle(pointVal, MyDView.Position.Size);
							}
							break;
						case "chkSortToBottom":
							if (bool.TryParse(val, out boolVal))
							{
								chkSortToBottom.Checked = boolVal;
							}
							break;
						case "nbkMode":
							if (int.TryParse(val, out intVal) && intVal >= 0 && intVal < TabMode.Count)
							{
								nbkMode.ActiveTab = intVal;
							}
							break;
					}
				}

				bool asc, enabled;
				foreach (XmlElement ruleEle in doc.SelectNodes("settings/scrolls/rule"))
				{
					if (!bool.TryParse(ruleEle.GetAttribute("asc"), out asc))
						asc = true;
					if (!bool.TryParse(ruleEle.GetAttribute("checked"), out enabled))
						enabled = true;
					foreach (RuleInfo info in ScrollRules)
					{
						if (ruleEle.GetAttribute("name") == info.Name)
						{
							AddPriority(lstScroll, info, asc, enabled, ScrollRules.Length);
							break;
						}
					}
				}

				foreach (XmlElement ruleEle in doc.SelectNodes("settings/salvage/rule"))
				{
					if (!bool.TryParse(ruleEle.GetAttribute("asc"), out asc))
						asc = true;
					if (!bool.TryParse(ruleEle.GetAttribute("checked"), out enabled))
						enabled = true;
					foreach (RuleInfo info in SalvageRules)
					{
						if (ruleEle.GetAttribute("name") == info.Name)
						{
							AddPriority(lstSalvage, info, asc, enabled, SalvageRules.Length);
							break;
						}
					}
				}

				foreach (XmlElement ruleEle in doc.SelectNodes("settings/manaStones/rule"))
				{
					if (!bool.TryParse(ruleEle.GetAttribute("asc"), out asc))
						asc = true;
					if (!bool.TryParse(ruleEle.GetAttribute("checked"), out enabled))
						enabled = true;
					foreach (RuleInfo info in ManaStoneRules)
					{
						if (ruleEle.GetAttribute("name") == info.Name)
						{
							AddPriority(lstManaStones, info, asc, enabled, ManaStoneRules.Length);
							break;
						}
					}
				}
			}
			finally
			{
				// Set defaults if necessary (the function checks if they've already been added)
				foreach (RuleInfo info in ScrollRules)
				{
					AddPriority(lstScroll, info, info.DefaultAsc, info.DefaultEnabled, ScrollRules.Length);
				}
				foreach (RuleInfo info in SalvageRules)
				{
					AddPriority(lstSalvage, info, info.DefaultAsc, info.DefaultEnabled, SalvageRules.Length);
				}
				foreach (RuleInfo info in ManaStoneRules)
				{
					AddPriority(lstManaStones, info, info.DefaultAsc, info.DefaultEnabled, ManaStoneRules.Length);
				}

				mSettingsLoaded = true;
			}
		}

		private void AddSetting(XmlDocument doc, string name, string value)
		{
			XmlElement ele = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("option"));
			ele.SetAttribute("name", name);
			ele.SetAttribute("value", value);
		}
		private void AddSetting(XmlDocument doc, string name, bool value) { AddSetting(doc, name, value.ToString()); }
		private void AddSetting(XmlDocument doc, string name, int value) { AddSetting(doc, name, value.ToString()); }
		private void AddSetting(XmlDocument doc, string name, Point value) { AddSetting(doc, name, PointToString(value)); }

		private XmlElement SaveRule(XmlDocument doc, string name, bool asc, bool enabled)
		{
			XmlElement ele = doc.CreateElement("rule");
			ele.SetAttribute("name", name);
			ele.SetAttribute("asc", asc.ToString());
			ele.SetAttribute("checked", enabled.ToString());
			return ele;
		}

		private void SaveSettings()
		{
			if (!mSettingsLoaded)
				return;

			XmlDocument doc = new XmlDocument();
			XmlElement root = (XmlElement)doc.AppendChild(doc.CreateElement("settings"));
			root.SetAttribute("version", SettingsVersion.ToString());

			AddSetting(doc, "ViewPosition", MyDView.Position.Location);
			AddSetting(doc, "chkSortToBottom", chkSortToBottom.Checked);
			AddSetting(doc, "nbkMode", nbkMode.ActiveTab);

			XmlElement scrollsNode = (XmlElement)root.AppendChild(doc.CreateElement("scrolls"));
			for (int r = 0; r < lstScroll.RowCount; r++)
			{
				scrollsNode.AppendChild(SaveRule(doc,
					(string)lstScroll[r][PriorityList.Name][0],
					(bool)lstScroll[r][PriorityList.Ascending][0],
					(bool)lstScroll[r][PriorityList.Enabled][0]));
			}

			XmlElement salvageNode = (XmlElement)root.AppendChild(doc.CreateElement("salvage"));
			for (int r = 0; r < lstSalvage.RowCount; r++)
			{
				salvageNode.AppendChild(SaveRule(doc,
					(string)lstSalvage[r][PriorityList.Name][0],
					(bool)lstSalvage[r][PriorityList.Ascending][0],
					(bool)lstSalvage[r][PriorityList.Enabled][0]));
			}

			XmlElement manaStoneNode = (XmlElement)root.AppendChild(doc.CreateElement("manaStones"));
			for (int r = 0; r < lstManaStones.RowCount; r++)
			{
				manaStoneNode.AppendChild(SaveRule(doc,
					(string)lstManaStones[r][PriorityList.Name][0],
					(bool)lstManaStones[r][PriorityList.Ascending][0],
					(bool)lstManaStones[r][PriorityList.Enabled][0]));
			}

            Util.SaveXml(doc, Util.FullPath("settings_sssort.xml"));
		}

		private bool TryParsePoint(string val, out Point pt)
		{
			string[] xy = val.Split(',');
			int x, y;
			if (xy.Length == 2
					&& int.TryParse(xy[0], out x) && x >= 0
					&& int.TryParse(xy[1], out y) && y >= 0)
			{
				pt = new Point(x, y);
				return true;
			}
			pt = new Point();
			return false;
		}

		private string PointToString(Point pt)
		{
			return pt.X + "," + pt.Y;
		}
		#endregion
	}
}
