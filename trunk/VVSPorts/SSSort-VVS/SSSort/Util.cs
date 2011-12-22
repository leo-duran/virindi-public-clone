///////////////////////////////////////////////////////////////////////////////
//File: Util.cs
//
//Description: Helper methods for the SSSort Decal Plugin.
//
//Copyright (c) 2009 Digero (http://decal.acasylum.com/)
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
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;
using WindowsTimer = System.Windows.Forms.Timer;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace SSSort
{
	public delegate void QueuedAction();

	[Flags]
	enum ChatWindow
	{
		None = 0,
		MainChat = 0x01,
		One = 0x02,
		Two = 0x04,
		Three = 0x08,
		Four = 0x10,
		All = MainChat | One | Two | Three | Four
	}

	static class Util
	{
		public const int MainChat = 1;

		// Chat Color Constants
		public const int MessageColor = 13;
		public const int HelpColor = 0;
		public const int WarningColor = 3;
		public const int ErrorColor = 8;
		public const int SevereErrorColor = 6;

		[DllImport("user32")]
		private static extern short GetAsyncKeyState(int vKey);
		private const int VK_SHIFT = 0x10, VK_CTRL = 0x11, VK_ALT = 0x12;

		[DllImport("user32", SetLastError = true)]
		private static extern int GetClientRect(IntPtr hWnd, ref RECT lpRect);

		[StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			public int left, top, right, bottom;
		}

		private static string mPluginName = "";
		private static string mBasePath = null;
		private static PluginHost mHost = null;
		private static Thread mMainPluginThread = null;
		//private static int mDefaultTargetWindow = MainChat;
		private static ChatWindow mTargetWindows;
		private static bool mWriteErrorsToMainChat;
		private static int mNumExceptionsWritten;

		private static Queue<QueuedAction> mQueuedActions;
		private static WindowsTimer mActionQueueTimer;

		private static SortedList<int, QueuedAction> mChatActions;
		private static int mNextChatActionId;
		private const int MaxChatActions = 32;
		private static string mChatActionCommand;

		public static void Initialize(string pluginName, PluginHost host, string basePath)
		{
			mPluginName = pluginName;
			mChatActionCommand = Regex.Replace(pluginName, @"[^A-Za-z]", "") + "_ChatCommand";
			mHost = host;
			BasePath = basePath;
			mMainPluginThread = Thread.CurrentThread;

			mTargetWindows = ChatWindow.MainChat;
			mWriteErrorsToMainChat = true;
			mNumExceptionsWritten = 0;

			mQueuedActions = new Queue<QueuedAction>();

			mChatActions = new SortedList<int, QueuedAction>();
			mNextChatActionId = 0;

			mActionQueueTimer = null;
		}

		public static void Dispose()
		{
			mHost = null;
			mMainPluginThread = null;

			mQueuedActions.Clear();
			mChatActions.Clear();

			if (mActionQueueTimer != null)
			{
				mActionQueueTimer.Dispose();
				mActionQueueTimer = null;
			}

			if (mDebugWriter != null)
			{
				mDebugWriter.Dispose();
				mDebugWriter = null;
			}
		}

		public static string PluginName
		{
			get { return mPluginName; }
		}

		public static string PluginVer
		{
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(4); }
		}

		public static string PluginNameVer
		{
			get { return PluginName + " v" + PluginVer; }
		}

		public static PluginHost Host
		{
			get { return mHost; }
			set { mHost = value; }
		}

		public static string BasePath
		{
			get
			{
				if (mBasePath == null)
				{
					mBasePath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
					int slash = mBasePath.LastIndexOf('\\');
					if (slash > 0)
						mBasePath = mBasePath.Substring(0, slash + 1);
				}
				return mBasePath;
			}
			set
			{
				if (value.EndsWith("\\"))
					mBasePath = value;
				else
					mBasePath = value + "\\";
			}
		}

		public static Thread MainPluginThread
		{
			get { return mMainPluginThread; }
			set { mMainPluginThread = value; }
		}

		public static void SetTargetWindow(ChatWindow window, bool enabled)
		{
			if (enabled)
				DefaultTargetWindows |= window;
			else
				DefaultTargetWindows &= ~window;
		}

		public static ChatWindow DefaultTargetWindows
		{
			get { return mTargetWindows; }
			set { mTargetWindows = value; }
		}

		public static bool WriteErrorsToMainChat
		{
			get { return mWriteErrorsToMainChat; }
			set { mWriteErrorsToMainChat = value; }
		}

		public static bool IsControlDown()
		{
			return GetAsyncKeyState(VK_CTRL) != 0;
		}

		public static bool IsShiftDown()
		{
			return GetAsyncKeyState(VK_SHIFT) != 0;
		}

		public static bool TryParseEnum<T>(string s, out T result) where T : struct
		{
			return TryParseEnum(s, true, out result);
		}

		public static bool TryParseEnum<T>(string s, bool ignoreCase, out T result) where T : struct
		{
			try
			{
				result = (T)Enum.Parse(typeof(T), s, ignoreCase);
				return true;
			}
			catch
			{
				result = default(T);
				return false;
			}
		}

		public static System.Drawing.Rectangle RegionWindow
		{
			get { return Host.Actions.RegionWindow; }
		}

		public static System.Drawing.Rectangle Region3D
		{
			get
			{
#if USING_D3D_CONTAINER
				return new System.Drawing.Rectangle(0, 28, 1024, 641);
#else
				return Host.Actions.Region3D;
#endif
			}
		}

		public static void Message(string msg)
		{
			Message(msg, DefaultTargetWindows);
		}

		public static void Message(string msg, ChatWindow targetWindows)
		{
			AddChatText("<{ " + PluginName + " }> " + msg, MessageColor, targetWindows);
		}

		public static void HelpMessage(string msg)
		{
			HelpMessage(msg, DefaultTargetWindows);
		}

		public static void HelpMessage(string msg, ChatWindow targetWindows)
		{
			AddChatText("<{ " + PluginName + " }> " + msg, HelpColor, targetWindows);
		}

		public static void Warning(string msg)
		{
			Warning(msg, DefaultTargetWindows);
		}

		public static void Warning(string msg, ChatWindow targetWindows)
		{
			//wtcw("«3»<{ «2»" + PluginName + "«3» }> «d»" + msg, 3, targetWindow);
			AddChatText("<{ " + PluginName + " }> " + msg, WarningColor, targetWindows);
		}

		public static void Error(string msg)
		{
			Error(msg, false, DefaultTargetWindows);
		}

		public static void Error(string msg, bool includePluginVersion)
		{
			Error(msg, includePluginVersion, DefaultTargetWindows);
		}

		public static void Error(string msg, bool includePluginVersion, ChatWindow targetWindows)
		{
			try
			{
				if (WriteErrorsToMainChat) { targetWindows |= ChatWindow.MainChat; }
				string nameVer = includePluginVersion ? PluginNameVer : PluginName;
				AddChatText("<{ " + nameVer + " }> " + msg, ErrorColor, targetWindows);
			}
			catch (Exception ex) { Util.LogException(ex); }
		}

		public static void SevereError(string msg)
		{
			SevereError(msg, DefaultTargetWindows);
		}

		public static void SevereError(string msg, ChatWindow targetWindows)
		{
			try
			{
				AddChatText("<{ " + PluginNameVer + " }> " + msg, SevereErrorColor,
					targetWindows | ChatWindow.MainChat);
			}
			catch (Exception ex) { Util.LogException(ex); }
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Debug(string msg)
		{
			Debug(msg, DefaultTargetWindows);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Debug(string msg, ChatWindow targetWindows)
		{
			AddChatText("<{ " + PluginName + " Debug }> " + msg, 12, targetWindows);
		}

		public static void AddChatText(string msg, int color)
		{
			AddChatText(msg, color, DefaultTargetWindows);
		}

		public static void AddChatText(string msg, int color, ChatWindow targetWindows)
		{
			if (targetWindows == ChatWindow.None)
			{
				AddChatText(msg, color, 0);
			}
			else
			{
				if ((targetWindows & ChatWindow.MainChat) != 0)
					AddChatText(msg, color, 1);
				if ((targetWindows & ChatWindow.One) != 0)
					AddChatText(msg, color, 2);
				if ((targetWindows & ChatWindow.Two) != 0)
					AddChatText(msg, color, 3);
				if ((targetWindows & ChatWindow.Three) != 0)
					AddChatText(msg, color, 4);
				if ((targetWindows & ChatWindow.Four) != 0)
					AddChatText(msg, color, 5);
			}
		}

		public static void AddChatText(string msg, int color, int targetWindow)
		{
			if (Host != null)
			{
				if (Thread.CurrentThread == MainPluginThread || MainPluginThread == null)
				{
					Host.Actions.AddChatText(msg, color, targetWindow);
				}
				else
				{
					// Queue the message to be sent in the main plugin thread
					QueueAction(delegate() { Host.Actions.AddChatText(msg, color, targetWindow); });
				}
			}
			else
			{
				FileInfo messagesFile = new FileInfo(FullPath("messages.txt"));
				// Delete and recreate the file if it's over 1MB in size or over 7 days old.
				if (messagesFile.Exists && (messagesFile.Length > 1024 * 1024 ||
						DateTime.Now.Subtract(messagesFile.LastWriteTime).Days > 7))
				{
					messagesFile.Delete();
				}
				LogLine("messages.txt", msg);
			}
		}

		public static string CreateChatCommand(string text, QueuedAction action)
		{
			if (mChatActions.Count >= MaxChatActions)
			{
				mChatActions.RemoveAt(0);
			}
			int id = mNextChatActionId++;
			mChatActions[id] = action;
			return "<Tell:IIDString:" + id + ":" + mChatActionCommand + ">" + text + @"<\Tell>";
		}

		public static bool HandleChatCommand(ChatClickInterceptEventArgs e)
		{
			if (e.Text == mChatActionCommand)
			{
				QueuedAction action;
				if (mChatActions.TryGetValue(e.Id, out action))
				{
					action();
				}
				else
				{
					Util.Error("Invalid chat action ID. Only " + MaxChatActions
						+ " chat actions can be active at once");
				}
				return true;
			}
			return false;
		}

		/// <summary>Queues an action from another thread to happen on the MainPluginThread.</summary>
		/// <param name="action">The action to take.</param>
		public static void QueueAction(QueuedAction action)
		{
			lock (mQueuedActions)
			{
				mQueuedActions.Enqueue(action);

				if (mActionQueueTimer == null)
				{
					mActionQueueTimer = new WindowsTimer();
					mActionQueueTimer.Tick += new EventHandler(ActionQueueTimer_Tick);
					mActionQueueTimer.Interval = 100;
				}
				mActionQueueTimer.Start();
			}
		}

		private static void ActionQueueTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Thread.CurrentThread == MainPluginThread || MainPluginThread == null)
				{
					lock (mQueuedActions)
					{
						while (mQueuedActions.Count > 0)
						{
							mQueuedActions.Dequeue()();
						}
						mActionQueueTimer.Stop();
					}
				}
			}
			catch (Exception ex) { Util.HandleException(ex); }
		}

		public static void HandleException(Exception ex)
		{
			HandleException(ex, "Error", false);
		}

		public static void HandleException(Exception ex, string messagePrefix, bool severe)
		{
			try
			{
                System.Diagnostics.Debug.Print(ex.ToString());

				Exception fileWriteException = null;
				try
				{
					LogException(ex);
				}
				catch (Exception fwe) { fileWriteException = fwe; }

				string errMsg = messagePrefix + " [" + ex.GetType().Name + "]";
				if (ex.Message.Length < 100)
					errMsg += ": " + ex.Message;
				if (fileWriteException == null)
				{
					errMsg += " See errors.txt in the " + PluginName + " folder for more info.";
				}
				else
				{
					errMsg += "\nAlso, an error occurred while trying to write to errors.txt ["
						+ fileWriteException.GetType().Name + "]: " + fileWriteException.Message;
				}
				if (mNumExceptionsWritten < 20)
				{
					if (severe)
						SevereError(errMsg);
					else
						Error(errMsg, true);
				}
				if (mNumExceptionsWritten == 20)
				{
					SevereError("Over 20 errors encountered; further error messages suppressed. "
						+ "Error messages will continue to be written to errors.txt in the "
						+ PluginName + " folder, which may cause lag. It is highly recommended "
						+ "that you log off and either fix or disable " + PluginName + ".");
				}
				mNumExceptionsWritten++;
			}
			catch { /* Ignore... */ }
		}

		public static void LogException(Exception ex)
		{
			try
			{
				FileInfo errorFile = new FileInfo(FullPath("errors.txt"));
				// Delete and recreate the file if it's over 1MB in size or over 7 days old.
				if (errorFile.Exists && (errorFile.Length > 1024 * 1024 ||
						DateTime.Now.Subtract(errorFile.LastWriteTime).Days > 7))
					errorFile.Delete();

				using (StreamWriter sw = new StreamWriter(FullPath("errors.txt"), true))
				{
					sw.WriteLine();
					sw.WriteLine("===[ " + System.DateTime.Now.ToString() + " - "
						+ PluginNameVer + " ]========================");
					sw.WriteLine(ex.GetType().ToString() + ": " + ex.Message);
					if (ex.StackTrace != null)
					{
						sw.WriteLine(StripFullPaths(ex.StackTrace));
					}
					if (ex.InnerException != null)
					{
						Exception iex = ex.InnerException;
						sw.WriteLine("[Inner Exception] " + iex.GetType().ToString() + ": " + iex.Message);
						if (iex.StackTrace != null)
							sw.WriteLine(StripFullPaths(iex.StackTrace));
					}
				}
			}
			catch { /* Ignore... */ }
		}

		public static void LogLine(string fileName, string message) { LogLine(fileName, message, true); }
		public static void LogLine(string fileName, string message, bool includeDateTime)
		{
			FileInfo logFile = new FileInfo(FullPath(fileName));
			// Delete and recreate the file if it's over 1MB in size or over 30 days old.
			if (logFile.Exists && (logFile.Length > 1024 * 1024 ||
					DateTime.Now.Subtract(logFile.LastWriteTime).Days > 30))
			{
				logFile.Delete();
			}

			using (StreamWriter sw = new StreamWriter(FullPath(fileName), true))
			{
				if (includeDateTime)
					message = "[" + System.DateTime.Now.ToString() + "] " + message;
				sw.WriteLine(message);
			}
		}

		private static StreamWriter mDebugWriter;

		[System.Diagnostics.Conditional("DEBUG")]
		public static void DebugLog(string message)
		{
			if (mDebugWriter == null)
			{
				FileInfo logFile = new FileInfo(FullPath("debug.log"));
				// Delete and recreate the file if it's over 4MB in size or over 2 days old.
				if (logFile.Exists && (logFile.Length > 4 * 1024 * 1024 || DateTime.Now.Subtract(logFile.LastWriteTime).Days > 2))
				{
					logFile.Delete();
				}

				mDebugWriter = new StreamWriter(FullPath("debug.log"), true);
				mDebugWriter.WriteLine();
				mDebugWriter.WriteLine();
				mDebugWriter.WriteLine("############## Debugging started ##############");
			}
			mDebugWriter.WriteLine("[" + System.DateTime.Now.ToString() + "] " + message);
			mDebugWriter.Flush();
		}

		public static string FullPath(string fileName)
		{
			return System.IO.Path.Combine(BasePath, fileName);
		}

		public static void SaveXml(XmlDocument doc, string filePath)
		{
			XmlWriterSettings writerSettings = new XmlWriterSettings();
			writerSettings.Indent = true;
			writerSettings.IndentChars = "    ";
			using (XmlWriter writer = XmlWriter.Create(filePath, writerSettings))
			{
				doc.Save(writer);
			}
		}

		/*** Private Methods ***/
		private static string StripFullPaths(string stackTrace)
		{
			try
			{
				string[] lines = stackTrace.Split('\n');
				for (int i = 0; i < lines.Length; i++)
				{
					int inPos = lines[i].IndexOf(" in ");
					if (inPos > 0 && lines[i].IndexOf(".cs:line") > inPos)
					{
						int slashPos = lines[i].LastIndexOf('\\');
						if (slashPos > inPos)
						{
							lines[i] = lines[i].Substring(0, inPos + 4) + lines[i].Substring(slashPos + 1);
						}
					}
				}
				return string.Join("\n", lines);
			}
			catch { return stackTrace; }
		}

#if FALSE
		private static string StripColorTags(string msg)
		{
			if (!msg.Contains("«"))
				return msg;
			return Regex.Replace(msg, "«([0-9]{1,2}|d)»", "");
		}

		private static void WriteToChat(string msg, int defaultColor, int chatTextTarget) {
			if (mHooks == null)
				return;

			msg = msg.Replace("«d»", "«" + defaultColor + "»");

			if (!msg.EndsWith("\r"))
				msg += "\r";

			string[] pieces = msg.Split(new char[] { '«' }, StringSplitOptions.RemoveEmptyEntries);
			int[] colors = new int[pieces.Length];

			for (int i = 0; i < pieces.Length; i++) {
				string[] colorAndText = pieces[i].Split(new char[] { '»' }, 2);
				try {
					colors[i] = int.Parse(colorAndText[0]);
					pieces[i] = colorAndText[1];
				}
				catch {
					mHooks.AddChatTextRaw(PluginName +
						" Error - Malformatted string specified to print to screen ::\r" + msg, 8, chatTextTarget);
					return;
				}
			}

			for (int i = 0; i < pieces.Length; i++) {
				if (pieces[i] != "")
					mHooks.AddChatTextRaw(pieces[i], colors[i], chatTextTarget);
			}
		}
#endif
	}
}
