///////////////////////////////////////////////////////////////////////////////
//This file is Copyright (c) 2013 VirindiPlugins
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
using System.Reflection;

namespace VTankFreeMethodsTest
{
    public enum VT_eDamageElement
    {
        Pierce = 0,
        Bludgeon = 1,
        Slash = 2,
        Acid = 3,
        Lightning = 4,
        Cold = 5,
        Fire = 6,
        Harm = 7,
        Auto = 8,
        Void = 9,
        DrainAuto = 10,
        Prismatic = 11,
        Random = 12,
        Fists = 13,

        LISTEDTYPES_END = 14,

        None = 98,
        Physical = 99,
        PrismaticDatabaseEntryOld = 100,
    }

    static class VirindiTank_FreeConnector
    {
        public static uTank2.PluginCore pc;
        public const string ConnectorVersion = "0.3.1.132";
        static bool Cached_VTAssemblyLoaded = false;

        public static bool IsVTankPresent(Version minimumversion)
        {
            if (Cached_VTAssemblyLoaded)
            {
                try
                {
                    //The assembly is loaded, but that doesn't mean the plugin is running.
                    return Curtain_IsVTankPresent();
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();

                foreach (System.Reflection.Assembly a in asms)
                {
                    AssemblyName nmm = a.GetName();
                    if ((nmm.Name == "uTank2") && (nmm.Version >= minimumversion))
                    {
                        Cached_VTAssemblyLoaded = true;
                        try
                        {
                            //The assembly is loaded, but that doesn't mean the plugin is running.
                            return Curtain_IsVTankPresent();
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }

            return false;
        }

        static bool Curtain_IsVTankPresent()
        {
            return uTank2.PluginCore.PC != null;
        }

        //Safe methods. These can be called directly from plugin code without bothering to check if VT is running.

        #region FMonsterList_QueryFinalDamageType

        /// <summary>
        /// Query the damage type the macro would use on a target, taking into account the player's monster list settings.
        /// Returns VT_eDamageElement.None on failure.
        /// </summary>
        /// <param name="monsterid"></param>
        /// <returns></returns>
        public static VT_eDamageElement FMonsterList_QueryFinalDamageType(int monsterid)
        {
            if (!IsVTankPresent(new Version(ConnectorVersion))) return VT_eDamageElement.None;
            return Curtain_FMonsterList_QueryFinalDamageType(monsterid);
        }

        static VT_eDamageElement Curtain_FMonsterList_QueryFinalDamageType(int monsterid)
        {
            //VTank is running.
            uTank2.LootPlugins.GameItemInfo mon = uTank2.PluginCore.PC.FWorldTracker_GetWithID(monsterid);
            if (mon == null) return VT_eDamageElement.None;
            return (VT_eDamageElement)(int)uTank2.PluginCore.PC.FMonsterList_QueryFinalDamageType(mon);
        }

        #endregion FMonsterList_QueryFinalDamageType

        #region FGameInfo_QueryAutoDamageElementList

        /// <summary>
        /// Query the auto-damage element list for a particular monster name.
        /// Returns null on failure.
        /// </summary>
        /// <param name="monstername"></param>
        /// <returns></returns>
        public static VT_eDamageElement[] FGameInfo_QueryAutoDamageElementList(string monstername)
        {
            if (!IsVTankPresent(new Version(ConnectorVersion))) return null;
            return Curtain_FGameInfo_QueryAutoDamageElementList(monstername);
        }

        static VT_eDamageElement[] Curtain_FGameInfo_QueryAutoDamageElementList(string monstername)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<uTank2.eDamageElement> elist = uTank2.PluginCore.PC.FGameInfo_QueryAutoDamageElementList(monstername);
            if (elist == null) return null;
            VT_eDamageElement[] ret = new VT_eDamageElement[elist.Count];
            for (int i = 0; i < elist.Count; ++i)
            {
                ret[i] = (VT_eDamageElement)(int)elist[i];
            }
            return ret;
        }

        #endregion FGameInfo_QueryAutoDamageElementList

    }
}
