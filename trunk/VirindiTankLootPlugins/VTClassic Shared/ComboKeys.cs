using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#if VTC_PLUGIN
using uTank2.LootPlugins;
#endif

namespace VTClassic
{
//#if VTC_EDITOR
    internal static class ComboKeys
    {
        static List<StringValueKey> SVKOptions = new List<StringValueKey>();
        static List<IntValueKey> LVKOptions = new List<IntValueKey>();
        static List<DoubleValueKey> DVKOptions = new List<DoubleValueKey>();
        static List<ObjectClass> OCOptions = new List<ObjectClass>();
        static List<VTCSkillID> SkillOptions = new List<VTCSkillID>();

        static List<string> SVKNames = new List<string>();
        static List<string> LVKNames = new List<string>();
        static List<string> DVKNames = new List<string>();
        static List<string> OCNames = new List<string>();
        static List<string> SkillNames = new List<string>();

        static ComboKeys()
        {
            {
                Array arr = Enum.GetValues(typeof(StringValueKey));
                foreach (int v in arr)
                {
                    StringValueKey kv = (StringValueKey)v;
                    SVKOptions.Add(kv);
                    SVKNames.Add(kv.ToString());
                }
            }
            {
                Array arr = Enum.GetValues(typeof(IntValueKey));
                foreach (int v in arr)
                {
                    IntValueKey kv = (IntValueKey)v;
                    LVKOptions.Add(kv);
                    LVKNames.Add(kv.ToString());
                }
            }
            {
                Array arr = Enum.GetValues(typeof(DoubleValueKey));
                foreach (int v in arr)
                {
                    DoubleValueKey kv = (DoubleValueKey)v;
                    DVKOptions.Add(kv);
                    DVKNames.Add(kv.ToString());
                }
            }
            {
                Array arr = Enum.GetValues(typeof(ObjectClass));
                foreach (int v in arr)
                {
                    ObjectClass kv = (ObjectClass)v;
                    OCOptions.Add(kv);
                    OCNames.Add(kv.ToString());
                }
            }
            {
                Array arr = Enum.GetValues(typeof(VTCSkillID));
                foreach (int v in arr)
                {
                    VTCSkillID kv = (VTCSkillID)v;
                    SkillOptions.Add(kv);
                    SkillNames.Add(kv.ToString());
                }
            }

            /*
            SVKOptions.Add(StringValueKey.FullDescription);
            SVKOptions.Add(StringValueKey.Name);

            LVKOptions.Add(IntValueKey.ActivationReqSkillId);
            LVKOptions.Add(IntValueKey.AffectsVitalAmt);
            LVKOptions.Add(IntValueKey.ArmorLevel);
            LVKOptions.Add(IntValueKey.ArmorSetID);
            LVKOptions.Add(IntValueKey.Burden);
            LVKOptions.Add(IntValueKey.Category);
            LVKOptions.Add(IntValueKey.Coverage);
            LVKOptions.Add(IntValueKey.CurrentMana);
            LVKOptions.Add(IntValueKey.DamageType);
            LVKOptions.Add(IntValueKey.ElementalDmgBonus);
            LVKOptions.Add(IntValueKey.EquipableSlots);
            LVKOptions.Add(IntValueKey.EquipSkill);
            LVKOptions.Add(IntValueKey.EquipType);
            LVKOptions.Add(IntValueKey.Heritage);
            LVKOptions.Add(IntValueKey.IconOutline);
            LVKOptions.Add(IntValueKey.IconOverlay);
            LVKOptions.Add(IntValueKey.IconUnderlay);
            LVKOptions.Add(IntValueKey.LockpickSkillBonus);
            LVKOptions.Add(IntValueKey.LoreRequirement);
            LVKOptions.Add(IntValueKey.ManaCost);
            LVKOptions.Add(IntValueKey.Material);
            LVKOptions.Add(IntValueKey.MaxDamage);
            LVKOptions.Add(IntValueKey.MaximumMana);
            LVKOptions.Add(IntValueKey.MissileType);
            LVKOptions.Add(IntValueKey.RankRequirement);
            LVKOptions.Add(IntValueKey.RareId);
            LVKOptions.Add(IntValueKey.SkillLevelReq);
            LVKOptions.Add(IntValueKey.SpellCount);
            LVKOptions.Add(IntValueKey.TotalValue);
            LVKOptions.Add(IntValueKey.Type);
            LVKOptions.Add(IntValueKey.Value);
            LVKOptions.Add(IntValueKey.WandElemDmgType);
            LVKOptions.Add(IntValueKey.WeapSpeed);
            LVKOptions.Add(IntValueKey.WieldReqAttribute);
            LVKOptions.Add(IntValueKey.WieldReqType);
            LVKOptions.Add(IntValueKey.WieldReqValue);
            LVKOptions.Add(IntValueKey.Workmanship);

            DVKOptions.Add(DoubleValueKey.AcidProt);
            DVKOptions.Add(DoubleValueKey.AttackBonus);
            DVKOptions.Add(DoubleValueKey.BludgeonProt);
            DVKOptions.Add(DoubleValueKey.ColdProt);
            DVKOptions.Add(DoubleValueKey.DamageBonus);
            DVKOptions.Add(DoubleValueKey.ElementalDamageVersusMonsters);
            DVKOptions.Add(DoubleValueKey.FireProt);
            DVKOptions.Add(DoubleValueKey.HealingKitRestoreBonus);
            DVKOptions.Add(DoubleValueKey.LightningProt);
            DVKOptions.Add(DoubleValueKey.MagicDBonus);
            DVKOptions.Add(DoubleValueKey.ManaCBonus);
            DVKOptions.Add(DoubleValueKey.ManaRateOfChange);
            DVKOptions.Add(DoubleValueKey.ManaStoneChanceDestruct);
            DVKOptions.Add(DoubleValueKey.ManaTransferEfficiency);
            DVKOptions.Add(DoubleValueKey.MeleeDefenseBonus);
            DVKOptions.Add(DoubleValueKey.MissileDBonus);
            DVKOptions.Add(DoubleValueKey.PierceProt);
            DVKOptions.Add(DoubleValueKey.Range);
            DVKOptions.Add(DoubleValueKey.SalvageWorkmanship);
            DVKOptions.Add(DoubleValueKey.SlashProt);
            DVKOptions.Add(DoubleValueKey.Variance);

            OCOptions.Add(ObjectClass.Armor);
            OCOptions.Add(ObjectClass.BaseAlchemy);
            OCOptions.Add(ObjectClass.BaseCooking);
            OCOptions.Add(ObjectClass.BaseFletching);
            OCOptions.Add(ObjectClass.Book);
            OCOptions.Add(ObjectClass.Bundle);
            OCOptions.Add(ObjectClass.Clothing);
            OCOptions.Add(ObjectClass.Container);
            OCOptions.Add(ObjectClass.CraftedAlchemy);
            OCOptions.Add(ObjectClass.CraftedCooking);
            OCOptions.Add(ObjectClass.CraftedFletching);
            OCOptions.Add(ObjectClass.Foci);
            OCOptions.Add(ObjectClass.Food);
            OCOptions.Add(ObjectClass.Gem);
            OCOptions.Add(ObjectClass.HealingKit);
            OCOptions.Add(ObjectClass.Jewelry);
            OCOptions.Add(ObjectClass.Journal);
            OCOptions.Add(ObjectClass.Key);
            OCOptions.Add(ObjectClass.Lockpick);
            OCOptions.Add(ObjectClass.ManaStone);
            OCOptions.Add(ObjectClass.MeleeWeapon);
            OCOptions.Add(ObjectClass.Misc);
            OCOptions.Add(ObjectClass.MissileWeapon);
            OCOptions.Add(ObjectClass.Money);
            OCOptions.Add(ObjectClass.Plant);
            OCOptions.Add(ObjectClass.Salvage);
            OCOptions.Add(ObjectClass.Scroll);
            OCOptions.Add(ObjectClass.SpellComponent);
            OCOptions.Add(ObjectClass.TradeNote);
            OCOptions.Add(ObjectClass.Ust);
            OCOptions.Add(ObjectClass.WandStaffOrb);

            //Fill the names
            foreach (StringValueKey k in SVKOptions)
                SVKNames.Add(k.ToString());
            foreach (IntValueKey k in LVKOptions)
                LVKNames.Add(k.ToString());
            foreach (DoubleValueKey k in DVKOptions)
                DVKNames.Add(k.ToString());
            foreach (ObjectClass k in OCOptions)
                OCNames.Add(k.ToString());
            foreach (VTCSkillID k in OCOptions)
                SkillNames.Add(k.ToString());
            */
        }

        public static StringValueKey SVKFromIndex(int i)
        {
            return SVKOptions[i];
        }
        public static int IndexFromSVK(StringValueKey k)
        {
            return SVKOptions.IndexOf(k);
        }
        public static ReadOnlyCollection<string> GetSVKEntries()
        {
            return SVKNames.AsReadOnly();
        }

        public static IntValueKey LVKFromIndex(int i)
        {
            return LVKOptions[i];
        }
        public static int IndexFromLVK(IntValueKey k)
        {
            return LVKOptions.IndexOf(k);
        }
        public static ReadOnlyCollection<string> GetLVKEntries()
        {
            return LVKNames.AsReadOnly();
        }

        public static DoubleValueKey DVKFromIndex(int i)
        {
            return DVKOptions[i];
        }
        public static int IndexFromDVK(DoubleValueKey k)
        {
            return DVKOptions.IndexOf(k);
        }
        public static ReadOnlyCollection<string> GetDVKEntries()
        {
            return DVKNames.AsReadOnly();
        }

        public static ObjectClass OCFromIndex(int i)
        {
            return OCOptions[i];
        }
        public static int IndexFromOC(ObjectClass k)
        {
            return OCOptions.IndexOf(k);
        }
        public static ReadOnlyCollection<string> GetOCEntries()
        {
            return OCNames.AsReadOnly();
        }

        public static VTCSkillID SkillFromIndex(int i)
        {
            return SkillOptions[i];
        }
        public static int IndexFromSkill(VTCSkillID k)
        {
            return SkillOptions.IndexOf(k);
        }
        public static ReadOnlyCollection<string> GetSkillEntries()
        {
            return SkillNames.AsReadOnly();
        }

    }
//#endif
}