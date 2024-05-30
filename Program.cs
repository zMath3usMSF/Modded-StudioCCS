using System;
using System.Windows.Forms;

namespace StudioCCS
{
	// Token: 0x0200000F RID: 15
	internal sealed class Program
	{
		// Token: 0x06000081 RID: 129 RVA: 0x0000A291 File Offset: 0x00008491
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
