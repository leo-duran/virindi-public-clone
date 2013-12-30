///////////////////////////////////////////////////////////////////////////////
//File: VHS_Connector.cs
//
//Description: Connector class for Virindi Hotkey System.
//
//References required:
//  VirindiHotkeySystem
//
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
using Decal.Adapter;
using Decal.Adapter.Wrappers;
using System.Reflection;

namespace MyClasses
{
	static class VHS_Connector
	{
		public static bool IsVHSPresent()
		{
			try
			{
				//See if VCS assembly is loaded
				System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
				bool l = false;
				foreach (System.Reflection.Assembly a in asms)
				{
					AssemblyName nmm = a.GetName();
					if ((nmm.Name == "VirindiHotkeySystem") && (nmm.Version >= new System.Version("1.0.0.0")))
					{
						l = true;
						break;
					}
				}

				if (l)
					if (Curtain_VHSInstanceEnabled())
						return true;
			}
			catch
			{

			}

			return false;
		}

		static bool Curtain_VHSInstanceEnabled()
		{
			return VirindiHotkeySystem.VHotkeySystem.Running;
		}
	}
}