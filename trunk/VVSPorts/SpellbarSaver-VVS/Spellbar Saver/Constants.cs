namespace Decal.Constants
{
	static class MessageTypes
	{
		/// <summary>Sent every time an object you are aware of ceases to exist. Merely running out of range does not generate this message - in that case, the client just automatically stops tracking it after receiving no updates for a while (which I presume is a very short while).</summary>
		public const int DestroyObject = 0x0024;

		/// <summary>For stackable items, this changes the number of items in the stack.</summary>
		public const int AdjustStackSize = 0x0197;

		/// <summary>A Player Kill occurred nearby (also sent for suicides). This could be interesting to monitor for tournements.</summary>
		public const int PlayerKilled = 0x019E;

		/// <summary>Indirect '/e' text.</summary>
		public const int IndirectText = 0x01E0;

		/// <summary>Contains the text associated with an emote action.</summary>
		public const int EmoteText = 0x01E2;

		/// <summary>A message to be displayed in the chat window, spoken by a nearby player, NPC or creature</summary>
		public const int CreatureMessage = 0x02BB;

		/// <summary>A message to be displayed in the chat window, spoken by a nearby player, NPC or creature</summary>
		public const int CreatureMessage_Ranged = 0x02BC;

		/// <summary>Set or update a Character DWORD property value</summary>
		public const int SetCharacterDword = 0x02CD;

		/// <summary>Set or update an Object DWORD property value</summary>
		public const int SetObjectDword = 0x02CE;

		/// <summary>Set or update a Character QWORD property value</summary>
		public const int SetCharacterQword = 0x02CF;

		/// <summary>Set or update a Character Boolean property value</summary>
		public const int SetCharacterBoolean = 0x02D1;

		/// <summary>Set or update an Object Boolean property value</summary>
		public const int SetObjectBoolean = 0x02D2;

		/// <summary>Set or update an Object String property value</summary>
		public const int SetObjectString = 0x02D6;

		/// <summary>Set or update an Object Resource property value</summary>
		public const int SetObjectResource = 0x02D8;

		/// <summary>Set or update a Character Link property value</summary>
		public const int SetCharacterLink = 0x02D9;

		/// <summary>Set or update an Object Link property value</summary>
		public const int SetObjectLink = 0x02DA;

		/// <summary>Set or update a Character Position property value</summary>
		public const int SetCharacterPosition = 0x02DB;

		/// <summary>Set or update a Character Skill value</summary>
		public const int SetCharacterSkillLevel = 0x02DD;

		/// <summary>Set or update a Character Skill state</summary>
		public const int SetCharacterSkillState = 0x02E1;

		/// <summary>Set or update a Character Attribute value</summary>
		public const int SetCharacterAttribute = 0x02E3;

		/// <summary>Set or update a Character Vital value</summary>
		public const int SetCharacterVital = 0x02E7;

		/// <summary>Set or update a Character Vital value</summary>
		public const int SetCharacterCurrentVital = 0x02E9;

		/// <summary>Sent when a character rematerializes at the lifestone after death.</summary>
		public const int LifestoneMaterialize = 0xF619;

		/// <summary>Sent whenever a character changes their clothes. It contains the entire description of what their wearing (and possibly their facial features as well). This message is only sent for changes, when the character is first created, the body of this message is included inside the creation message.</summary>
		public const int ChangeModel = 0xF625;

		/// <summary>Uncracked - Character creation screen initilised.</summary>
		public const int CharCreationInitilisation = 0xF643;

		/// <summary>Instructs the client to return to 2D mode - the character list.</summary>
		public const int End3DMode = 0xF653;

		/// <summary>A character was marked for  delete.</summary>
		public const int CharDeletion = 0xF655;

		/// <summary>The character to log in.</summary>
		public const int RequestLogin = 0xF657;

		/// <summary>The list of characters on the current account.</summary>
		public const int CharacterList = 0xF658;

		/// <summary>Failure to log in</summary>
		public const int CharacterLoginFailure = 0xF659;

		/// <summary>Create an object somewhere in the world</summary>
		public const int CreateObject = 0xF745;

		/// <summary></summary>
		public const int LoginCharacter = 0xF746;

		/// <summary>Sent whenever an object is removed from the scene.</summary>
		public const int RemoveItem = 0xF747;

		/// <summary>Set position - the server pathologically sends these after every actions - sometimes more than once. If has options for setting a fixed velocity or an arc for thrown weapons and arrows.</summary>
		public const int SetPositionAndMotion = 0xF748;

		/// <summary>Multipurpose message.  So far object wielding has been decoded.  Lots of unknowns</summary>
		public const int WieldObject = 0xF749;

		/// <summary></summary>
		public const int MoveObjectIntoInventory = 0xF74A;

		/// <summary>Signals your client to end the portal animation for you or another char and also is fired when war spells dissapear as they hit an object blocking their path.</summary>
		public const int ToggleObjectVisibility = 0xF74B;

		/// <summary>These are animations. Whenever a human, monster or object moves - one of these little messages is sent. Even idle emotes (like head scratching and nodding) are sent in this manner.</summary>
		public const int Animation = 0xF74C;

		/// <summary>An object has jumped</summary>
		public const int Jumping = 0xF74E;

		/// <summary>Applies a sound effect.</summary>
		public const int ApplySoundEffect = 0xF750;

		/// <summary>Instructs the client to show the portal graphic.</summary>
		public const int EnterPortalMode = 0xF751;

		/// <summary>Applies an effect with visual and sound.</summary>
		public const int ApplyVisualOrSoundEffect = 0xF755;

		/// <summary>Game Events are messages that are sequenced.</summary>
		public const int GameEvent = 0xF7B0;

		/// <summary>Game Actions are outgoing messages that are sequenced.</summary>
		public const int GameAction = 0xF7B1;

		/// <summary>The user has clicked 'Enter'. This message does not contain the ID of the character logging on; that comes later.</summary>
		public const int EnterGame = 0xF7C8;

		/// <summary>Update an existing object's data.</summary>
		public const int UpdateObject = 0xF7DB;

		/// <summary>Send or receive a message using Turbine Chat.</summary>
		public const int TurbineChat = 0xF7DE;

		/// <summary>Switch from the character display to the game display.</summary>
		public const int Start3DMode = 0xF7DF;

		/// <summary>Display a message in the chat window.</summary>
		public const int ServerMessage = 0xF7E0;

		/// <summary>The name of the current world.</summary>
		public const int ServerName = 0xF7E1;

		/// <summary>Add or update a dat file Resource.</summary>
		public const int UpdateResource = 0xF7E2;

		/// <summary>A list of dat files that need to be patched</summary>
		public const int DatFilePatchList = 0xF7E7;
	}

	static class GameEvents
	{
		/// <summary>Display a message in a popup message window.</summary>
		public const int MessageBox = 0x0004;

		/// <summary>Information describing your character.</summary>
		public const int LoginCharacter = 0x0013;

		/// <summary>Returns info related to your monarch, patron and vassals.</summary>
		public const int AllegianceInfo = 0x0020;

		/// <summary>Store an item in a container.</summary>
		public const int InsertInventoryItem = 0x0022;

		/// <summary>Equip an item.</summary>
		public const int WearItem = 0x0023;

		/// <summary>Titles for the current character.</summary>
		public const int TitleList = 0x0029;

		/// <summary>Set a title for the current character.</summary>
		public const int SetTitle = 0x002b;

		/// <summary>Close Container - Only sent when explicitly closed</summary>
		public const int CloseContainer = 0x0052;

		/// <summary>Open the buy/sell panel for a merchant.</summary>
		public const int ApproachVendor = 0x0062;

		/// <summary>Failure to give an item</summary>
		public const int FailureToGiveItem = 0x00A0;

		/// <summary>Member left fellowship</summary>
		public const int FellowshipMemberQuit = 0x00A3;

		/// <summary>Member dismissed from fellowship</summary>
		public const int FellowshipMemberDismissed = 0x00A4;

		/// <summary>Sent when you first open a book, contains the entire table of contents.</summary>
		public const int ReadTableOfContents = 0x00B4;

		/// <summary>Contains the text of a single page of a book, parchment or sign.</summary>
		public const int ReadPage = 0x00B8;

		/// <summary>The result of an attempt to assess an item or creature.</summary>
		public const int IdentifyObject = 0x00C9;

		/// <summary>Group Chat</summary>
		public const int GroupChat = 0x0147;

		/// <summary>Set Pack Contents</summary>
		public const int SetPackContents = 0x0196;

		/// <summary>Removes an item from inventory (when you place it on the ground or give it away)</summary>
		public const int DropFromInventory = 0x019A;

		/// <summary>Melee attack completed</summary>
		public const int AttackCompleted = 0x01A7;

		/// <summary>Delete a spell from your spellbook.</summary>
		public const int DeleteSpellFromSpellbook = 0x01A8;

		/// <summary>You just died.</summary>
		public const int YourDeath = 0x01AC;

		/// <summary>Message for a death, something you killed or your own death message.</summary>
		public const int KillOrDeathMessage = 0x01AD;

		/// <summary>You have hit your target with a melee attack.</summary>
		public const int InflictMeleeDamage = 0x01B1;

		/// <summary>You have been hit by a creature's melee attack.</summary>
		public const int ReceiveMeleeDamage = 0x01B2;

		/// <summary>Your target has evaded your melee attack.</summary>
		public const int OtherMeleeEvade = 0x01B3;

		/// <summary>You have evaded a creature's melee attack.</summary>
		public const int SelfMeleeEvade = 0x01B4;

		/// <summary>Start melee attack</summary>
		public const int StartMeleeAttack = 0x01B8;

		/// <summary>Update a creature's health bar.</summary>
		public const int UpdateHealth = 0x01C0;

		/// <summary>Age Command Result - happens when you do /age in the game</summary>
		public const int AgeCommandResult = 0x01C3;

		/// <summary>Ready.  Previous action complete</summary>
		public const int ReadyPreviousActionComplete = 0x01C7;

		/// <summary>Update Allegiance info, sent when allegiance panel is open</summary>
		public const int UpdateAllegianceInfo = 0x01C8;

		/// <summary>Close Assess Panel</summary>
		public const int CloseAssessPanel = 0x01CB;

		/// <summary>Ping Reply</summary>
		public const int PingReply = 0x01EA;

		/// <summary>Squelched Users List</summary>
		public const int SquelchedUsersList = 0x01F4;

		/// <summary>Send to begin a trade and display the trade window</summary>
		public const int EnterTrade = 0x01FD;

		/// <summary>End trading</summary>
		public const int EndTrade = 0x01FF;

		/// <summary>Item was added to trade window - you will receive a Create Object (0xF745) with details of the item</summary>
		public const int AddTradeItem = 0x0200;

		/// <summary>The trade was accepted</summary>
		public const int AcceptTrade = 0x0202;

		/// <summary>The trade was un-accepted</summary>
		public const int UnacceptTrade = 0x0203;

		/// <summary>The trade window was reset</summary>
		public const int ResetTrade = 0x0205;

		/// <summary>Failure to add a trade item</summary>
		public const int FailureToAddATradeItem = 0x0207;

		/// <summary>Failure to complete a trade</summary>
		public const int FailureToCompleteATrade = 0x0208;

		/// <summary>Buy a dwelling or pay maintenance</summary>
		public const int DisplayDwellingPurchaseOrMaintenancePanel = 0x021D;

		/// <summary>House panel information for owners.</summary>
		public const int HouseInformationForOwners = 0x0225;

		/// <summary>House panel information for non-owners.</summary>
		public const int HouseInformationForNonowners = 0x0226;

		/// <summary>House Guest List, Sent in response to asking for one.</summary>
		public const int HouseGuestList = 0x0257;

		/// <summary>Update an item's mana bar.</summary>
		public const int UpdateItemManaBar = 0x0264;

		/// <summary>Display a list of available dwellings in the chat window.</summary>
		public const int HousesAvailable = 0x0271;

		/// <summary>Display a confirmation panel.</summary>
		public const int ConfirmationPanel = 0x0274;

		/// <summary>A player has closed your confirmation panel.</summary>
		public const int ConfirmationPanelClosed = 0x0276;

		/// <summary>Display an allegiance login/logout message in the chat window.</summary>
		public const int AllegianceMemberLoginOrOut = 0x027A;

		/// <summary>Display a status message in the chat window.</summary>
		public const int DisplayStatusMessage = 0x028A;

		/// <summary>Display a parameterized status message in the chat window.</summary>
		public const int DisplayParameterizedStatusMessage = 0x028B;

		/// <summary>Set Turbine Chat channel numbers.</summary>
		public const int SetTurbineChatChannels = 0x0295;

		/// <summary>Someone has sent you a @tell.</summary>
		public const int Tell = 0x02BD;

		/// <summary>Create or join a fellowship</summary>
		public const int CreateFellowship = 0x02BE;

		/// <summary>Disband your fellowship.</summary>
		public const int DisbandFellowship = 0x02BF;

		/// <summary>Add a member to your fellowship.</summary>
		public const int AddFellowshipMember = 0x02C0;

		/// <summary>Add a spell to your spellbook.</summary>
		public const int AddSpellToSpellbook = 0x02C1;

		/// <summary>Apply an enchantment to your character.</summary>
		public const int AddCharacterEnchantment = 0x02C2;

		/// <summary>Remove an enchantment from your character.</summary>
		public const int RemoveCharacterEnchantment = 0x02C3;

		/// <summary>Remove multiple enchantments from your character.</summary>
		public const int RemoveMultipleCharacterEnchantments = 0x02C5;

		/// <summary>Silently remove all enchantments from your character, e.g. when you die (no message in the chat window).</summary>
		public const int RemoveAllCharacterEnchantments_Silent = 0x02C6;

		/// <summary>Silently remove An enchantment from your character.</summary>
		public const int RemoveCharacterEnchantment_Silent = 0x02C7;

		/// <summary>Silently remove multiple enchantments from your character (no message in the chat window).</summary>
		public const int RemoveMultipleCharacterEnchantments_Silent = 0x02C8;

		/// <summary>A portal storm is brewing.</summary>
		public const int MildPortalStorm = 0x02C9;

		/// <summary>A portal storm is imminent.</summary>
		public const int HeavyPortalStorm = 0x02CA;

		/// <summary>You have been portal stormed.</summary>
		public const int PortalStormed = 0x02CB;

		/// <summary>The portal storm has subsided.</summary>
		public const int EndPortalStorm = 0x02CC;

		/// <summary>Display a status message on the Action Viewscreen (the red text overlaid on the 3D area).</summary>
		public const int StatusMessage = 0x02EB;
	}

	static class GameActions
	{
		/// <summary>Set a single character option.</summary>
		public const int SetSingleCharacterOption = 0x0005;

		/// <summary>Set AFK message.</summary>
		public const int SetAfkMessage = 0x0010;

		/// <summary>Store an item in a container.</summary>
		public const int StoreItem = 0x0019;

		/// <summary>Equip an item.</summary>
		public const int EquipItem = 0x001A;

		/// <summary>Drop an item.</summary>
		public const int DropItem = 0x001B;

		/// <summary>Attempt to use an item.</summary>
		public const int UseItem = 0x0036;

		/// <summary>Spend XP to raise a vital.</summary>
		public const int RaiseVital = 0x0044;

		/// <summary>Spend XP to raise an attribute.</summary>
		public const int RaiseAttribute = 0x0045;

		/// <summary>Spend XP to raise a skill.</summary>
		public const int RaiseSkill = 0x0046;

		/// <summary>Spend skill credits to train a skill.</summary>
		public const int TrainSkill = 0x0047;

		/// <summary>Cast a spell.</summary>
		public const int CastSpell = 0x0048;

		/// <summary>Cast a spell.</summary>
		public const int CastSpellOnObject = 0x004A;

		/// <summary>The client is ready for the character to materialize after portalling or logging on.</summary>
		public const int Materialize = 0x00A1;

		/// <summary>Give an item to someone.</summary>
		public const int GiveItem = 0x00CD;

		/// <summary>Add an item to the shortcut bar.</summary>
		public const int MakeShortcut = 0x019C;

		/// <summary>Remove an item from the shortcut bar.</summary>
		public const int RemoveShortcut = 0x019D;

		/// <summary>Set multiple character options.</summary>
		public const int SetCharacterOptions = 0x01A1;

		/// <summary>Add a spell to a spell bar.</summary>
		public const int AddSpellToSpellbar = 0x01E3;

		/// <summary>Remove a spell from a spell bar.</summary>
		public const int RemoveSpellFromSpellbar = 0x01E4;
	}
}