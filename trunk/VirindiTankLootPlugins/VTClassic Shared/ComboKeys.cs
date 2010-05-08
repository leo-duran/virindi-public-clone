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
                Array arr = Enum.GetNames(typeof(StringValueKey));
                Array.Sort(arr);
                foreach (string v in arr)
                {
                    SVKOptions.Add((StringValueKey)Enum.Parse(typeof(StringValueKey), v));
                    SVKNames.Add(v);
                }
            }
            {
                Array arr = Enum.GetNames(typeof(IntValueKey));
                Array.Sort(arr);
                foreach (string v in arr)
                {
                    LVKOptions.Add((IntValueKey)Enum.Parse(typeof(IntValueKey), v));
                    LVKNames.Add(v);
                }
            }
            {
                Array arr = Enum.GetNames(typeof(DoubleValueKey));
                Array.Sort(arr);
                foreach (string v in arr)
                {
                    DVKOptions.Add((DoubleValueKey)Enum.Parse(typeof(DoubleValueKey), v));
                    DVKNames.Add(v);
                }
            }
            {
                Array arr = Enum.GetNames(typeof(ObjectClass));
                Array.Sort(arr);
                foreach (string v in arr)
                {
                    OCOptions.Add((ObjectClass)Enum.Parse(typeof(ObjectClass), v));
                    OCNames.Add(v);
                }
            }
            {
                Array arr = Enum.GetNames(typeof(VTCSkillID));
                Array.Sort(arr);
                foreach (string v in arr)
                {
                    SkillOptions.Add((VTCSkillID)Enum.Parse(typeof(VTCSkillID), v));
                    SkillNames.Add(v);
                }
            }
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


#if VTC_EDITOR
        public static System.Drawing.Color GetSVKColor(int ind)
        {
            if (GameInfo.IsIDProperty(SVKOptions[ind]))
                return (System.Drawing.Color.DarkRed);
            else
                return (System.Drawing.Color.White);
        }
        public static System.Drawing.Color GetLVKColor(int ind)
        {
            if (GameInfo.IsIDProperty(LVKOptions[ind]))
                return (System.Drawing.Color.DarkRed);
            else
                return (System.Drawing.Color.White);
        }
        public static System.Drawing.Color GetDVKColor(int ind)
        {
            if (GameInfo.IsIDProperty(DVKOptions[ind]))
                return (System.Drawing.Color.DarkRed);
            else
                return (System.Drawing.Color.White);
        }
        public static System.Drawing.Color GetOCColor(int ind)
        {
            return (System.Drawing.Color.White);
        }
        public static System.Drawing.Color GetSkillColor(int ind)
        {
            return (System.Drawing.Color.White);
        }
#endif

    }
//#endif
}