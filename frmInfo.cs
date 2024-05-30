using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StudioCCS
{
	// Token: 0x02000007 RID: 7
	public partial class frmInfo : Form
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00004EFC File Offset: 0x000030FC
		public frmInfo()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004F13 File Offset: 0x00003113
		public void SetReportText(string _reportText)
		{
			this.reportText.Text = _reportText;
		}
	}
}
