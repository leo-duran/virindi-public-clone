///////////////////////////////////////////////////////////////////////////////
//File: LootCore.cs
//
//Description: The core of the VTClassic Virindi Tank Loot Plugin, implementing
//  old-style Virindi Tank looting.
//
//This file is Copyright (c) 2009 VirindiPlugins
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
using uTank2.LootPlugins;

namespace VTClassic
{
    public class LootCore: uTank2.LootPlugins.LootPluginBase
    {
        void ExceptionHandler(Exception ex)
        {
            Host.AddChatText("Exception: " + ex.ToString(), 6, 1);
        }

        public override bool DoesPotentialItemNeedID(uTank2.LootPlugins.GameItemInfo item)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return true;
        }

        public override uTank2.LootPlugins.LootAction GetLootDecision(uTank2.LootPlugins.GameItemInfo item)
        {
            try
            {
                if (LootRules == null) return uTank2.LootPlugins.LootAction.NoLoot;

                eLootAction act = LootRules.Classify(item);
                switch (act)
                {
                    case eLootAction.Keep:
                        return LootAction.Keep;
                    case eLootAction.NoLoot:
                        return LootAction.NoLoot;
                    case eLootAction.Salvage:
                        return LootAction.Salvage;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return uTank2.LootPlugins.LootAction.NoLoot;
        }

        public override void LoadProfile(string filename, bool newprofile)
        {
            try
            {
                if (newprofile)
                {
                    LootRules = new cLootRules();
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    {
                        using (System.IO.StreamWriter sr = new System.IO.StreamWriter(fs))
                        {
                            LootRules.Write(sr);
                        }
                    }

                    Host.AddChatText("Created blank profile " + filename + ".");
                }
                else
                {
                    if (!System.IO.File.Exists(filename)) return;

                    LootRules = new cLootRules();
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                        {
                            LootRules.Read(sr);
                        }
                    }

                    Host.AddChatText("Load profile " + filename + " successful.");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        public override void UnloadProfile()
        {
            try
            {
                LootRules = null;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        cLootRules LootRules = null;
        //MyClasses.MetaViewWrappers.IView view;

        public override void OpenEditorForProfile()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        public override void CloseEditorForProfile()
        {
            try
            {
                //view.Dispose();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        public override uTank2.LootPlugins.LootPluginInfo Startup()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return new uTank2.LootPlugins.LootPluginInfo("utl");
        }

        public override void Shutdown()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
    }
}
