using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Xml;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace SpellbarSaver
{
	public partial class PluginCore : PluginBase
	{
		private const int SettingsVer = 1;

		/// <summary>Set to FALSE until the LoadSettings() function is complete</summary>
		private bool mSettingsLoaded = false;

		private void LoadSettings()
		{
			FileInfo settingsFile;
			try
			{
				settingsFile = new FileInfo(Util.FullPath("settings.xml"));
				if (!settingsFile.Exists)
				{
					mSettingsLoaded = true;
					return;
				}
			}
			catch (Exception ex)
			{
				Util.HandleException(ex);
				mSettingsLoaded = true;
				return;
			}

			try
			{
#pragma warning disable 168 // Variable declared but not used
				string strVal;
				int intVal;
				double dblVal;
				bool boolVal;
				Point ptVal;
				Rectangle rectVal;
#pragma warning restore 168

				int fileVer = 0;

				XmlDocument doc = new XmlDocument();
				doc.Load(settingsFile.FullName);

				if (doc.DocumentElement.HasAttribute("version"))
				{
					int.TryParse(doc.DocumentElement.GetAttribute("version"), out fileVer);
				}

				if (TryGetSetting(doc, "ViewPosition", out ptVal) &&
						ptVal.X + 10 < Host.Actions.Region3D.Right &&
						ptVal.Y + 10 < Host.Actions.Region3D.Bottom)
				{
					DefaultView.Position = new Rectangle(ptVal, DefaultView.Position.Size);
				}

				if (TryGetSetting(doc, "chkReplCrit", out boolVal)) { chkReplCrit.Checked = boolVal; }
				if (TryGetSetting(doc, "choLowCrit", out intVal)) { choLowCrit.Selected = intVal; }
				if (TryGetSetting(doc, "choHighCrit", out intVal)) { choHighCrit.Selected = intVal; }

				if (TryGetSetting(doc, "chkReplLife", out boolVal)) { chkReplLife.Checked = boolVal; }
				if (TryGetSetting(doc, "choLowLife", out intVal)) { choLowLife.Selected = intVal; }
				if (TryGetSetting(doc, "choHighLife", out intVal)) { choHighLife.Selected = intVal; }

				if (TryGetSetting(doc, "chkReplItem", out boolVal)) { chkReplItem.Checked = boolVal; }
				if (TryGetSetting(doc, "choLowItem", out intVal)) { choLowItem.Selected = intVal; }
				if (TryGetSetting(doc, "choHighItem", out intVal)) { choHighItem.Selected = intVal; }

				if (TryGetSetting(doc, "chkReplWar", out boolVal)) { chkReplWar.Checked = boolVal; }
				if (TryGetSetting(doc, "choLowWar", out intVal)) { choLowWar.Selected = intVal; }
				if (TryGetSetting(doc, "choHighWar", out intVal)) { choHighWar.Selected = intVal; }

				if (TryGetSetting(doc, "chkShowAsSaved", out boolVal)) { chkShowAsSaved.Checked = boolVal; }
				if (TryGetSetting(doc, "chkShowAsLoaded", out boolVal)) { chkShowAsLoaded.Checked = boolVal; }
				if (TryGetSetting(doc, "chkOnlyReplUnknown", out boolVal)) { chkOnlyReplUnknown.Checked = boolVal; }
				if (TryGetSetting(doc, "chkAutoBackup", out boolVal)) { chkAutoBackup.Checked = boolVal; }
			}
			catch (Exception ex)
			{
				Util.HandleException(ex, "Error encountered while loading settings.xml file", true);
				string errorPath = Util.FullPath("settings_error.xml");
				if (File.Exists(errorPath))
					File.Delete(errorPath);
				settingsFile.MoveTo(errorPath);
				Util.SevereError("The old settings.xml file has been renamed to settings_error.xml "
					+ "and a new settings.xml will be created with the defaults.");
			}
			finally
			{
				mSettingsLoaded = true;
			}
		}

		private void SaveSettings()
		{
			if (!mSettingsLoaded)
				return;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.AppendChild(doc.CreateElement("settings"));
				doc.DocumentElement.SetAttribute("version", SettingsVer.ToString());

				AddSetting(doc, "ViewPosition", DefaultView.Position.Location);

				AddSetting(doc, "chkReplCrit", chkReplCrit.Checked);
				AddSetting(doc, "choLowCrit", choLowCrit.Selected);
				AddSetting(doc, "choHighCrit", choHighCrit.Selected);

				AddSetting(doc, "chkReplLife", chkReplLife.Checked);
				AddSetting(doc, "choLowLife", choLowLife.Selected);
				AddSetting(doc, "choHighLife", choHighLife.Selected);

				AddSetting(doc, "chkReplItem", chkReplItem.Checked);
				AddSetting(doc, "choLowItem", choLowItem.Selected);
				AddSetting(doc, "choHighItem", choHighItem.Selected);

				AddSetting(doc, "chkReplWar", chkReplWar.Checked);
				AddSetting(doc, "choLowWar", choLowWar.Selected);
				AddSetting(doc, "choHighWar", choHighWar.Selected);

				AddSetting(doc, "chkShowAsSaved", chkShowAsSaved.Checked);
				AddSetting(doc, "chkShowAsLoaded", chkShowAsLoaded.Checked);
				AddSetting(doc, "chkOnlyReplUnknown", chkOnlyReplUnknown.Checked);
				AddSetting(doc, "chkAutoBackup", chkAutoBackup.Checked);

				Util.SaveXml(doc, Util.FullPath("settings.xml"));
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		private bool TryGetSetting(XmlDocument doc, string name, out string value)
		{
			XmlElement ele = (XmlElement)doc.SelectSingleNode("/settings/setting[@name='" + name + "']");
			if (ele != null)
			{
				value = ele.GetAttribute("value");
				return true;
			}
			value = null;
			return false;
		}

		private bool TryGetSetting(XmlDocument doc, string name, out int value)
		{
			value = 0;
			string s;
			return TryGetSetting(doc, name, out s) && int.TryParse(s, out value);
		}

		private bool TryGetSetting(XmlDocument doc, string name, out double value)
		{
			value = 0;
			string s;
			return TryGetSetting(doc, name, out s) && double.TryParse(s, out value);
		}

		private bool TryGetSetting(XmlDocument doc, string name, out bool value)
		{
			value = false;
			string s;
			return TryGetSetting(doc, name, out s) && bool.TryParse(s, out value);
		}

		private bool TryGetSetting(XmlDocument doc, string name, out Point value)
		{
			value = new Point();
			string s;
			return TryGetSetting(doc, name, out s) && TryParsePoint(s, out value);
		}

		private bool TryGetSetting(XmlDocument doc, string name, out Rectangle value)
		{
			value = new Rectangle();
			string s;
			return TryGetSetting(doc, name, out s) && TryParseRectangle(s, out value);
		}

		private void AddSetting(XmlDocument doc, string name, string value)
		{
			XmlElement ele = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("setting"));
			ele.SetAttribute("name", name);
			ele.SetAttribute("value", value);
		}
		private void AddSetting(XmlDocument doc, string name, bool value) { AddSetting(doc, name, value.ToString()); }
		private void AddSetting(XmlDocument doc, string name, int value) { AddSetting(doc, name, value.ToString()); }
		private void AddSetting(XmlDocument doc, string name, double value) { AddSetting(doc, name, value.ToString()); }
		private void AddSetting(XmlDocument doc, string name, Point value) { AddSetting(doc, name, PointToString(value)); }
		private void AddSetting(XmlDocument doc, string name, Rectangle value) { AddSetting(doc, name, RectangleToString(value)); }

		private string PointToString(Point pt)
		{
			return pt.X + "," + pt.Y;
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

		private string RectangleToString(Rectangle r)
		{
			return r.X + "," + r.Y + ";" + r.Width + "," + r.Height;
		}

		private bool TryParseRectangle(string val, out Rectangle r)
		{
			string[] pt_sz = val.Split(';');
			Point pt, sz;
			if (pt_sz.Length == 2 && TryParsePoint(pt_sz[0], out pt) && TryParsePoint(pt_sz[1], out sz))
			{
				r = new Rectangle(pt, (Size)sz);
				return true;
			}
			r = new Rectangle();
			return false;
		}
	}
}
