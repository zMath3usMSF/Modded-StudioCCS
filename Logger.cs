using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace StudioCCS
{
	// Token: 0x0200000D RID: 13
	public static class Logger
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00006905 File Offset: 0x00004B05
		public static void SetLogControl(RichTextBox r)
		{
			Logger.LogControl = r;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000690D File Offset: 0x00004B0D
		public static void LogError(string errorText, Logger.LogType logAs = Logger.LogType.LogAll, string callingMethod = "", int callingLine = 0)
		{
			Logger.LogGeneric(errorText, Color.DarkRed, logAs, callingMethod, callingLine);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000691F File Offset: 0x00004B1F
		public static void LogWarning(string warningText, Logger.LogType logAs = Logger.LogType.LogAll, string callingMethod = "", int callingLine = 0)
		{
			Logger.LogGeneric(warningText, Color.Orange, logAs, callingMethod, callingLine);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00006931 File Offset: 0x00004B31
		public static void LogInfo(string infoText, Logger.LogType logAs = Logger.LogType.LogAll, string callingMethod = "", int callingLine = 0)
		{
			Logger.LogGeneric(infoText, Color.White, logAs, callingMethod, callingLine);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00006944 File Offset: 0x00004B44
		private static void LogGeneric(string outputText, Color textColor, Logger.LogType logAs = Logger.LogType.LogAll, string callingMethod = "", int callingLine = 0)
		{
			if (logAs != Logger.LogType.LogOnceCode)
			{
				if (logAs == Logger.LogType.LogOnceValue)
				{
					int hashCode2 = outputText.GetHashCode();
					bool flag = Logger.FiredShots.ContainsKey(hashCode2);
					if (flag)
					{
						return;
					}
					Logger.FiredShots[hashCode2] = true;
				}
			}
			else
			{
				int hashCode3 = string.Format("{0}:{1}", callingMethod, callingLine).GetHashCode();
				bool flag2 = Logger.FiredWarnings.ContainsKey(hashCode3);
				if (flag2)
				{
					return;
				}
				Logger.FiredWarnings[hashCode3] = true;
			}
			bool flag3 = Logger.LogControl == null;
			if (!flag3)
			{
				Color selectionColor = Logger.LogControl.SelectionColor;
				Logger.LogControl.SelectionColor = textColor;
				Logger.LogControl.AppendText(outputText);
				Logger.LogControl.SelectionColor = selectionColor;
			}
		}

		// Token: 0x0400007A RID: 122
		private static Dictionary<int, bool> FiredWarnings = new Dictionary<int, bool>();

		// Token: 0x0400007B RID: 123
		private static Dictionary<int, bool> FiredShots = new Dictionary<int, bool>();

		// Token: 0x0400007C RID: 124
		public static RichTextBox LogControl = null;

		// Token: 0x02000035 RID: 53
		public enum LogType
		{
			// Token: 0x0400021A RID: 538
			LogAll,
			// Token: 0x0400021B RID: 539
			LogOnceCode,
			// Token: 0x0400021C RID: 540
			LogOnceValue
		}
	}
}
