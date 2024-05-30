using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StudioCCS
{
	// Token: 0x02000006 RID: 6
	public partial class frmExportToOBJ : Form
	{
		// Token: 0x0600001B RID: 27 RVA: 0x000045DF File Offset: 0x000027DF
		public frmExportToOBJ()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000045F8 File Offset: 0x000027F8
		private void BtnSelectDirClick(object sender, EventArgs e)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.FileName = "Export Here";
				saveFileDialog.CheckFileExists = false;
				saveFileDialog.Title = "Select directory to export to...";
				bool flag = saveFileDialog.ShowDialog() == DialogResult.OK;
				if (flag)
				{
					this.txtExportPath.Text = Path.GetDirectoryName(saveFileDialog.FileName);
				}
				bool flag2 = this.txtExportPath.Text == "";
				if (flag2)
				{
					this.btnDoExport.Enabled = false;
				}
				else
				{
					this.btnDoExport.Enabled = true;
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000046A4 File Offset: 0x000028A4
		private void ChkExportCollisionCheckedChanged(object sender, EventArgs e)
		{
			this.chkSplitCollision.Enabled = this.chkExportCollision.Checked;
		}
	}
}
