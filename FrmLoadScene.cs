using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StudioCCS
{
	// Token: 0x02000008 RID: 8
	public partial class FrmLoadScene : Form
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00005084 File Offset: 0x00003284
		public FrmLoadScene()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000050B4 File Offset: 0x000032B4
		private void BtnSelectDirClick(object sender, EventArgs e)
		{
			bool flag = this.folderDialog.ShowDialog() != DialogResult.OK;
			if (!flag)
			{
				this.txtDir.Text = this.folderDialog.SelectedPath;
				this.Path = this.folderDialog.SelectedPath;
				bool flag2 = this.CheckDir();
				if (flag2)
				{
					this.btnOk.Enabled = true;
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00005118 File Offset: 0x00003318
		private bool CheckDir()
		{
			int num = 0;
			foreach (string fileName in this.FileNames)
			{
				string str = this.Path + "/" + fileName;
				ListViewItem listViewItem = new ListViewItem(str);
				bool flag = File.Exists(str);
				if (flag)
				{
					num++;
					ListViewItem listViewItem2 = listViewItem;
					listViewItem2.Text += " - FOUND";
				}
				else
				{
					ListViewItem listViewItem3 = listViewItem;
					listViewItem3.Text += " - NOT FOUND";
					listViewItem.ForeColor = Color.Red;
				}
				this.sceneList.Items.Add(listViewItem);
			}
			return num == this.FileNames.Count;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00005208 File Offset: 0x00003408
		private void FrmLoadSceneLoad(object sender, EventArgs e)
		{
			this.CheckDir();
		}

		// Token: 0x04000043 RID: 67
		public List<string> FileNames = new List<string>();

		// Token: 0x04000044 RID: 68
		public string Path = "";
	}
}
