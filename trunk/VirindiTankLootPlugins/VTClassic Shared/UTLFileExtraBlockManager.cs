///////////////////////////////////////////////////////////////////////////////
//File: UTLFileExtraBlockManager.cs
//
//Description: A class for VTClassic files to allow arbitrary length- and type- delimited
//  data blocks at the end of a profile file.
//  This file is shared between the VTClassic Plugin and the VTClassic Editor.
//
//This file is Copyright (c) 2010 VirindiPlugins
//
//The original copy of this code can be obtained from http://www.virindi.net/repos/virindi_public
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

namespace VTClassic
{
    internal interface eFileBlockHandler
    {
        string BlockTypeID { get; }
        void Read(System.IO.StreamReader inf, int len);
        void Write(CountedStreamWriter inf);
    }

    internal class UTLFileExtraBlockManager
    {
        static Dictionary<string, Type> BlockHandlerTypes = new Dictionary<string, Type>();
        static void AddHandlerType(Type t)
        {
            if (!typeof(eFileBlockHandler).IsAssignableFrom(t)) throw new Exception("UTLFileExtraBlockManager: Cannot add handler type.");

        }
        static UTLFileExtraBlockManager()
        {
            try
            {
                //Add the current handler types

            }
            catch (Exception exx)
            {
                System.Windows.Forms.MessageBox.Show("Exception in static constructor(" + System.Reflection.Assembly.GetExecutingAssembly().FullName + "): " + exx.ToString());
            }
        }

        List<eFileBlockHandler> FileBlocks = new List<eFileBlockHandler>();

        public void Read(System.IO.StreamReader inf)
        {
            FileBlocks.Clear();

            while (!inf.EndOfStream)
            {
                //Read blocks
                string blocktype = inf.ReadLine();
                int blocklen = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

                if (BlockHandlerTypes.ContainsKey(blocktype))
                {
                    //Known blocktype, create and add
                    eFileBlockHandler h = (eFileBlockHandler)BlockHandlerTypes[blocktype].GetConstructor(new Type[] { }).Invoke(null);
                    h.Read(inf, blocklen);
                    FileBlocks.Add(h);
                }
                else
                {
                    //Unknown blocktype
                    inf.BaseStream.Position += blocklen;
                }
            }
        }

        public void Write(CountedStreamWriter inf)
        {
            foreach (eFileBlockHandler h in FileBlocks)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CountedStreamWriter sw = new CountedStreamWriter(ms);
                h.Write(sw);
                sw.Flush();

                inf.WriteLine(h.BlockTypeID);
                inf.WriteLine(((int)sw.Count).ToString(System.Globalization.CultureInfo.InvariantCulture));

                ms.Position = 0;
                System.IO.StreamReader sr = new System.IO.StreamReader(ms);
                inf.Write(sr.ReadToEnd());
            }
        }
    }
}
