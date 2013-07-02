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

namespace VTankFreeMethodsTest
{
    public class PluginCore : Decal.Adapter.PluginBase
    {
        protected override void Startup()
        {
            Core.CommandLineText += new EventHandler<Decal.Adapter.ChatParserInterceptEventArgs>(Core_CommandLineText);
        }

        protected override void Shutdown()
        {
            Core.CommandLineText -= new EventHandler<Decal.Adapter.ChatParserInterceptEventArgs>(Core_CommandLineText);
        }

        void Core_CommandLineText(object sender, Decal.Adapter.ChatParserInterceptEventArgs e)
        {
            try
            {
                if (e.Text == "/vtmon")
                {
                    e.Eat = true;

                    Decal.Adapter.Wrappers.WorldObject sel = Core.WorldFilter[Host.Actions.CurrentSelection];
                    if (sel == null) { Chat("Select a monster first."); return; }
                    if (sel.ObjectClass != Decal.Adapter.Wrappers.ObjectClass.Monster) { Chat("Select a monster first."); return; }

                    //Fetch the info about the monster.
                    VT_eDamageElement[] autodamages = VirindiTank_FreeConnector.FGameInfo_QueryAutoDamageElementList(sel.Name);
                    if (autodamages == null) autodamages = new VT_eDamageElement[0];

                    VT_eDamageElement monsterlistvalue = VirindiTank_FreeConnector.FMonsterList_QueryFinalDamageType(sel.Id);

                    Chat("Monster \"" + sel.Name + "\" will be attacked using " + monsterlistvalue.ToString());

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < autodamages.Length; ++i)
                    {
                        sb.Append(autodamages[i].ToString());
                        if (i < autodamages.Length - 1) sb.Append(", ");
                    }

                    Chat("Auto damage list: " + sb.ToString());
                }
            }
            catch (Exception ex)
            {
                Chat(ex.ToString());
            }
        }

        void Chat(string msg)
        {
            Host.Actions.AddChatText("[VTMON] " + msg, 0, 1);
        }

    }
}
