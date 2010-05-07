using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

#if VTC_PLUGIN
using uTank2.LootPlugins;
#endif

namespace VTClassic
{
#if VTC_EDITOR
    public enum StringValueKey
    {
        Name = 1,
        Title = 5,
        Inscription = 7,
        InscribedBy = 8,
        FellowshipName = 10,
        UsageInstructions = 14,
        SimpleDescription = 15,
        FullDescription = 16,
        MonarchName = 21,
        OnlyActivatedBy = 25,
        Patron = 35,
        PortalDestination = 38,
        LastTinkeredBy = 39,
        ImbuedBy = 40,
        DateBorn = 43,
        SecondaryName = 184549376,
    }
    public enum IntValueKey
    {
        Species = 2,
        Burden = 5,
        EquippedSlots = 10,
        RareId = 17,
        Value = 19,
        TotalValue = 20,
        SkillCreditsAvail = 24,
        CreatureLevel = 25,
        RestrictedToToD = 26,
        ArmorLevel = 28,
        Rank = 30,
        Bonded = 33,
        NumberFollowers = 35,
        Unenchantable = 36,
        LockpickDifficulty = 38,
        Deaths = 43,
        WandElemDmgType = 45,
        MinLevelRestrict = 86,
        MaxLevelRestrict = 87,
        LockpickSkillBonus = 88,
        AffectsVitalId = 89,
        AffectsVitalAmt = 90,
        UsesTotal = 91,
        UsesRemaining = 92,
        DateOfBirth = 98,
        Workmanship = 105,
        Spellcraft = 106,
        CurrentMana = 107,
        MaximumMana = 108,
        LoreRequirement = 109,
        RankRequirement = 110,
        PortalRestrictions = 111,
        Gender = 113,
        Attuned = 114,
        SkillLevelReq = 115,
        ManaCost = 117,
        Age = 125,
        XPForVPReduction = 129,
        Material = 131,
        WieldReqType = 158,
        WieldReqAttribute = 159,
        WieldReqValue = 160,
        SlayerSpecies = 166,
        NumberItemsSalvagedFrom = 170,
        NumberTimesTinkered = 171,
        DescriptionFormat = 172,
        PagesUsed = 174,
        PagesTotal = 175,
        ActivationReqSkillId = 176,
        GemSettingQty = 177,
        GemSettingType = 178,
        Imbued = 179,
        Heritage = 188,
        FishingSkill = 192,
        KeysHeld = 193,
        ElementalDmgBonus = 204,
        ArmorSetID = 265,
        Type = 218103808,
        Icon = 218103809,
        Container = 218103810,
        Landblock = 218103811,
        ItemSlots = 218103812,
        PackSlots = 218103813,
        StackCount = 218103814,
        StackMax = 218103815,
        AssociatedSpell = 218103816,
        Slot = 218103817,
        Wielder = 218103818,
        WieldingSlot = 218103819,
        Monarch = 218103820,
        Coverage = 218103821,
        EquipableSlots = 218103822,
        EquipType = 218103823,
        IconOutline = 218103824,
        MissileType = 218103825,
        UsageMask = 218103826,
        HouseOwner = 218103827,
        HookMask = 218103828,
        HookType = 218103829,
        Model = 218103830,
        Flags = 218103831,
        CreateFlags1 = 218103832,
        CreateFlags2 = 218103833,
        Category = 218103834,
        Behavior = 218103835,
        MagicDef = 218103836,
        SpecialProps = 218103837,
        SpellCount = 218103838,
        WeapSpeed = 218103839,
        EquipSkill = 218103840,
        DamageType = 218103841,
        MaxDamage = 218103842,
        Unknown10 = 218103843,
        Unknown100000 = 218103844,
        Unknown800000 = 218103845,
        Unknown8000000 = 218103846,
        PhysicsDataFlags = 218103847,
        ActiveSpellCount = 218103848,
        IconOverlay = 218103849,
        IconUnderlay = 218103850,
    }
    public enum DoubleValueKey
    {
        ManaRateOfChange = 5,
        MeleeDefenseBonus = 29,
        ManaTransferEfficiency = 87,
        HealingKitRestoreBonus = 100,
        ManaStoneChanceDestruct = 137,
        ManaCBonus = 144,
        MissileDBonus = 149,
        MagicDBonus = 150,
        ElementalDamageVersusMonsters = 152,
        SlashProt = 167772160,
        PierceProt = 167772161,
        BludgeonProt = 167772162,
        AcidProt = 167772163,
        LightningProt = 167772164,
        FireProt = 167772165,
        ColdProt = 167772166,
        Heading = 167772167,
        ApproachDistance = 167772168,
        SalvageWorkmanship = 167772169,
        Scale = 167772170,
        Variance = 167772171,
        AttackBonus = 167772172,
        Range = 167772173,
        DamageBonus = 167772174,
    }

    public enum ObjectClass
    {
        Unknown = 0,
        MeleeWeapon = 1,
        Armor = 2,
        Clothing = 3,
        Jewelry = 4,
        Monster = 5,
        Food = 6,
        Money = 7,
        Misc = 8,
        MissileWeapon = 9,
        Container = 10,
        Gem = 11,
        SpellComponent = 12,
        Key = 13,
        Portal = 14,
        TradeNote = 15,
        ManaStone = 16,
        Plant = 17,
        BaseCooking = 18,
        BaseAlchemy = 19,
        BaseFletching = 20,
        CraftedCooking = 21,
        CraftedAlchemy = 22,
        CraftedFletching = 23,
        Player = 24,
        Vendor = 25,
        Door = 26,
        Corpse = 27,
        Lifestone = 28,
        HealingKit = 29,
        Lockpick = 30,
        WandStaffOrb = 31,
        Bundle = 32,
        Book = 33,
        Journal = 34,
        Sign = 35,
        Housing = 36,
        Npc = 37,
        Foci = 38,
        Salvage = 39,
        Ust = 40,
        Services = 41,
        Scroll = 42,
    }
#endif

    public enum eLootAction
    {
        NoLoot = 0,
        Keep = 1,
        Salvage = 2,
        Sell = 3,
        Read = 4,
        User1 = 5,
        User2 = 6,
        User3 = 7,
        User4 = 8,
        User5 = 9
    }

    internal interface iSettingsCollection
    {
        void Read(System.IO.StreamReader inf, int fileversion);
        void Write(CountedStreamWriter inf);
    }

    internal class cUniqueID : IComparable<cUniqueID>
    {
        static long last = 0;
        public static cUniqueID New(int t, TreeNode n)
        {
            return new cUniqueID(++last, t, n);
        }

        public TreeNode node;
        public int type;
        long v;
        cUniqueID(long z, int t, TreeNode n)
        {
            node = n;
            type = t;
            v = z;
        }

        #region IComparable<cUniqueID> Members

        public int CompareTo(cUniqueID other)
        {
            return v.CompareTo(other);
        }

        #endregion
    }

    internal static class GameInfo
    {
        public static double HaxConvertDouble(string s)
        {
            string ss = s.Replace(',', '.');
            double res;
            if (!double.TryParse(ss, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out res))
                return 0d;
            return res;
        }

        public static bool IsIDProperty(StringValueKey vk)
        {
            switch (vk)
            {
                case StringValueKey.Name: return false;
                case StringValueKey.SecondaryName: return false;
                default: return true;
            }
        }

        public static bool IsIDProperty(IntValueKey vk)
        {
            switch (vk)
            {
                case IntValueKey.CreateFlags1: return false;
                case IntValueKey.Type: return false;
                case IntValueKey.Icon: return false;
                case IntValueKey.Category: return false;
                case IntValueKey.Behavior: return false;
                case IntValueKey.CreateFlags2: return false;
                case IntValueKey.IconUnderlay: return false;
                case IntValueKey.ItemSlots: return false;
                case IntValueKey.PackSlots: return false;
                case IntValueKey.MissileType: return false;
                case IntValueKey.Value: return false;
                case IntValueKey.Unknown10: return false;
                case IntValueKey.UsageMask: return false;
                case IntValueKey.IconOutline: return false;
                case IntValueKey.EquipType: return false;
                case IntValueKey.UsesRemaining: return false;
                case IntValueKey.UsesTotal: return false;
                case IntValueKey.StackCount: return false;
                case IntValueKey.StackMax: return false;
                case IntValueKey.Container: return false;
                case IntValueKey.Slot: return false;
                case IntValueKey.EquipableSlots: return false;
                case IntValueKey.EquippedSlots: return false;
                case IntValueKey.Coverage: return false;
                case IntValueKey.Unknown100000: return false;
                case IntValueKey.Unknown800000: return false;
                case IntValueKey.Unknown8000000: return false;
                //case IntValueKey.Burden: return false;
                //case IntValueKey.OwnedBy: return false;
                case IntValueKey.Monarch: return false;
                case IntValueKey.HookMask: return false;
                case IntValueKey.IconOverlay: return false;
                case IntValueKey.Material: return false;
                default: return true;
            }
        }

        public static bool IsIDProperty(DoubleValueKey vk)
        {
            switch (vk)
            {
                case DoubleValueKey.ApproachDistance: return false;
                case DoubleValueKey.SalvageWorkmanship: return false;
                default: return true;
            }
        }

        public static SortedDictionary<string, int> getSetInfo()
        {
            int i = 32;
            SortedDictionary<string, int> r = new SortedDictionary<string, int>();
            r.Add("Protective Clothing", i--);
            r.Add("Gladiatorial Clothing", i--);
            r.Add("Dedication", i--);
            r.Add("Lightning Proof", i--);
            r.Add("Cold Proof", i--);
            r.Add("Acid Proof", i--);
            r.Add("Flame Proof", i--);
            r.Add("Interlocking", i--);
            r.Add("Reinforced", i--);
            r.Add("Hardenend", i--);
            r.Add("Swift", i--);
            r.Add("Wise", i--);
            r.Add("Dexterous", i--);
            r.Add("Hearty", i--);
            r.Add("Crafter's", i--);
            r.Add("Tinker's", i--);
            r.Add("Defender's", i--);
            r.Add("Archer's", i--);
            r.Add("Adept's", i--);
            r.Add("Soldier's", i--);
            r.Add("Leggings of Perfect Light", i--);
            r.Add("Coat of the Perfect Light", i--);
            r.Add("Arm, Mind, Heart", i--);
            r.Add("Empyrean Rings", i--);
            r.Add("Shou-jen", i--);
            r.Add("Relic Alduressa", i--);
            r.Add("Ancient Relic", i--);
            r.Add("Noble Relic", i--);

            return r;
        }

        public static SortedDictionary<string, int> getSkillInfo()
        {
            SortedDictionary<string, int> r = new SortedDictionary<string, int>();
            r.Add("Axe", 1);
            r.Add("Bow", 2);
            r.Add("Crossbow", 3);
            r.Add("Dagger", 4);
            r.Add("Mace", 5);
            r.Add("MeleeD", 6);
            r.Add("MissileD", 7);
            r.Add("Spear", 9);
            r.Add("Staff", 10);
            r.Add("Sword", 11);
            r.Add("Thrown Weapons", 12);
            r.Add("Unarmed Combat", 13);
            r.Add("MagicD", 15);
            r.Add("ManaCon", 16);
            r.Add("Creature", 31);
            r.Add("Item", 32);
            r.Add("Life", 33);
            r.Add("War", 34);
            r.Add("Two-Handed Combat", 41);

            return r;
        }

        private static SortedDictionary<string, int> matIds;
        public static SortedDictionary<string, int> getMaterialInfo()
        {
            if (matIds == null)
            {
                matIds = new SortedDictionary<string, int>();
                matIds.Add("Agate", 10);
                matIds.Add("Alabaster", 66);
                matIds.Add("Amber", 11);
                matIds.Add("Amethyst", 12);
                matIds.Add("Aquamarine", 13);
                matIds.Add("Armoredillo Hide", 53);
                matIds.Add("Azurite", 14);
                matIds.Add("Black Garnet", 15);
                matIds.Add("Black Opal", 16);
                matIds.Add("Bloodstone", 17);
                matIds.Add("Brass", 57);
                matIds.Add("Bronze", 58);
                matIds.Add("Carnelian", 18);
                matIds.Add("Ceramic", 1);
                matIds.Add("Citrine", 19);
                matIds.Add("Copper", 59);
                matIds.Add("Diamond", 20);
                matIds.Add("Ebony", 73);
                matIds.Add("Emerald", 21);
                matIds.Add("Fire Opal", 22);
                matIds.Add("Gold", 60);
                matIds.Add("Granite", 67);
                matIds.Add("Green Garnet", 23);
                matIds.Add("Green Jade", 24);
                matIds.Add("Gromnie Hide", 54);
                matIds.Add("Hematite", 25);
                matIds.Add("Imperial Topaz", 26);
                matIds.Add("Iron", 61);
                matIds.Add("Ivory", 51);
                matIds.Add("Jet", 27);
                matIds.Add("Lapis Lazuli", 28);
                matIds.Add("Lavender Jade", 29);
                matIds.Add("Leather", 52);
                matIds.Add("Linen", 4);
                matIds.Add("Mahogany", 74);
                matIds.Add("Malachite", 30);
                matIds.Add("Marble", 68);
                matIds.Add("Moonstone", 31);
                matIds.Add("Oak", 75);
                matIds.Add("Obsidian", 69);
                matIds.Add("Onyx", 32);
                matIds.Add("Opal", 33);
                matIds.Add("Peridot", 34);
                matIds.Add("Pine", 76);
                matIds.Add("Porcelain", 2);
                matIds.Add("Pyreal", 62);
                matIds.Add("Red Garnet", 35);
                matIds.Add("Red Jade", 36);
                matIds.Add("Reed Shark Hide", 55);
                matIds.Add("Rose Quartz", 37);
                matIds.Add("Ruby", 38);
                matIds.Add("Sandstone", 70);
                matIds.Add("Sapphire", 39);
                matIds.Add("Satin", 5);
                matIds.Add("Serpentine", 71);
                matIds.Add("Silk", 6);
                matIds.Add("Silver", 63);
                matIds.Add("Smokey Quartz", 40);
                matIds.Add("Steel", 64);
                matIds.Add("Sunstone", 41);
                matIds.Add("Teak", 77);
                matIds.Add("Tiger Eye", 42);
                matIds.Add("Tourmaline", 43);
                matIds.Add("Turquoise", 44);
                matIds.Add("Velvet", 7);
                matIds.Add("White Jade", 45);
                matIds.Add("White Quartz", 46);
                matIds.Add("White Sapphire", 47);
                matIds.Add("Wool", 8);
                matIds.Add("Yellow Garnet", 48);
                matIds.Add("Yellow Topaz", 49);
                matIds.Add("Zircon", 50);
            }
            return matIds;
        }

        public static string getMaterialName(int materialId)
        {
            if (matIds == null) getMaterialInfo();
            if (matIds.ContainsValue(materialId))
            {
                foreach (KeyValuePair<string, int> kv in matIds)
                {
                    if (kv.Value == materialId)
                    {
                        return kv.Key;
                    }
                }
            }

            return string.Empty;
        }

        public static SortedDictionary<string, int[]> getMaterialGroups()
        {
            SortedDictionary<string, int[]> r = new SortedDictionary<string, int[]>();
            // Armor Tinkering: Alabaster, Armoredillo Hide, Bronze, Ceramic, Marble, Peridot, Reedshark Hide, Steel, Wool, Yellow Topaz, Zircon
            r.Add("Armor Tinkering", new int[] { 66, 53, 58, 1, 68, 34, 55, 64, 8, 49, 50 });
            // Item Tinkering: Copper, Ebony, Gold, Linen, Pine, Porcelain, Moonstone, Silk, Silver, Teak
            r.Add("Item Tinkering", new int[] { 59, 73, 60, 4, 76, 2, 31, 6, 63, 77 });
            // Magic Item Tinkering: Agate, Azurite, Black Opal, Bloodstone, Carnelian, Citrine, Fire Opal, Hematite, Lavender Jade, Malachite, Opal, Red Jade, Rose Quartz, Sunstone
            r.Add("Magic Item Tinkering", new int[] { 10, 14, 16, 17, 18, 19, 22, 25, 29, 30, 33, 36, 37, 41 });
            // Weapon Tinkering: Aquamarine, Black Garnet, Brass, Emerald, Granite, Iron, Imperial Topaz, Jet, Mahogany, Oak, Red Garnet, Velvet, White Sapphire
            r.Add("Weapon Tinkering", new int[] { 13, 15, 57, 21, 67, 61, 26, 27, 74, 75, 35, 7, 47 });
            // Basic Tinkering: Ivory, Leather
            r.Add("Basic Tinkering", new int[] { 51, 52 });
            // Gearcrafting: Amber, Diamond, Gromnie Hide, Pyreal, Ruby, Sapphire
            r.Add("Gearcrafting", new int[] { 11, 20, 54, 62, 38, 39 });
            // Armor Imbues: Peridot, Yellow Topaz, Zircon
            r.Add("Armor Imbues", new int[] { 34, 49, 50 });
            // Protection Tinks: Alabaster, Armoredillo Hide, Bronze, Ceramic, Marble, Reedshark Hide, Steel, Wool
            r.Add("Protection Tinks", new int[] { 66, 53, 58, 1, 68, 55, 64, 8 });
            // Weapon Imbues: Aquamarine, Black Garnet, Emerald, Imperial Topaz, Jet, Red Garnet, White Sapphire, Sunstone, Black Opal
            r.Add("Weapon Imbues", new int[] { 13, 15, 21, 26, 27, 35, 47, 41, 16 });
            // Brass/Iron/Granite/Hog
            r.Add("Brass/Granite/Iron/Mahog", new int[] { 57, 67, 61, 74 });
            // RG/BG/Jet
            r.Add("RG/BG/Jet", new int[] { 35, 15, 27 });
            return r;
        }

    }

}