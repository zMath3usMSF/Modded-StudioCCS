namespace StudioCCS
{
	// Token: 0x02000008 RID: 8
	public partial class FrmLoadScene : global::System.Windows.Forms.Form
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00005214 File Offset: 0x00003414
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000524C File Offset: 0x0000344C
		private void InitializeComponent()
		{
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.btnSelectDir = new global::System.Windows.Forms.Button();
			this.txtDir = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.folderDialog = new global::System.Windows.Forms.FolderBrowserDialog();
			this.panel2 = new global::System.Windows.Forms.Panel();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.panel3 = new global::System.Windows.Forms.Panel();
			this.sceneList = new global::System.Windows.Forms.ListView();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.btnSelectDir);
			this.panel1.Controls.Add(this.txtDir);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new global::System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(633, 23);
			this.panel1.TabIndex = 0;
			this.btnSelectDir.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.btnSelectDir.Location = new global::System.Drawing.Point(606, 0);
			this.btnSelectDir.Name = "btnSelectDir";
			this.btnSelectDir.Size = new global::System.Drawing.Size(27, 23);
			this.btnSelectDir.TabIndex = 2;
			this.btnSelectDir.Text = "...";
			this.btnSelectDir.UseVisualStyleBackColor = true;
			this.btnSelectDir.Click += new global::System.EventHandler(this.BtnSelectDirClick);
			this.txtDir.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.txtDir.Location = new global::System.Drawing.Point(49, 0);
			this.txtDir.Name = "txtDir";
			this.txtDir.ReadOnly = true;
			this.txtDir.Size = new global::System.Drawing.Size(584, 20);
			this.txtDir.TabIndex = 1;
			this.label1.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label1.Location = new global::System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(49, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Directory:";
			this.folderDialog.Description = "Select Directory";
			this.panel2.Controls.Add(this.btnOk);
			this.panel2.Controls.Add(this.btnCancel);
			this.panel2.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new global::System.Drawing.Point(0, 190);
			this.panel2.Name = "panel2";
			this.panel2.Size = new global::System.Drawing.Size(633, 32);
			this.panel2.TabIndex = 2;
			this.btnOk.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.btnOk.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.btnOk.Location = new global::System.Drawing.Point(483, 0);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new global::System.Drawing.Size(75, 32);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Location = new global::System.Drawing.Point(558, 0);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new global::System.Drawing.Size(75, 32);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.panel3.Controls.Add(this.sceneList);
			this.panel3.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new global::System.Drawing.Point(0, 23);
			this.panel3.Name = "panel3";
			this.panel3.Size = new global::System.Drawing.Size(633, 167);
			this.panel3.TabIndex = 4;
			this.sceneList.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.sceneList.HideSelection = false;
			this.sceneList.Location = new global::System.Drawing.Point(0, 0);
			this.sceneList.Name = "sceneList";
			this.sceneList.Size = new global::System.Drawing.Size(633, 167);
			this.sceneList.TabIndex = 0;
			this.sceneList.UseCompatibleStateImageBehavior = false;
			base.AcceptButton = this.btnOk;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new global::System.Drawing.Size(633, 222);
			base.ControlBox = false;
			base.Controls.Add(this.panel3);
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.Enabled = false;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmLoadScene";
			this.Text = "Load Scene:";
			base.TopMost = true;
			base.Load += new global::System.EventHandler(this.FrmLoadSceneLoad);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000045 RID: 69
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000046 RID: 70
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x04000047 RID: 71
		private global::System.Windows.Forms.Button btnSelectDir;

		// Token: 0x04000048 RID: 72
		private global::System.Windows.Forms.TextBox txtDir;

		// Token: 0x04000049 RID: 73
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400004A RID: 74
		private global::System.Windows.Forms.FolderBrowserDialog folderDialog;

		// Token: 0x0400004B RID: 75
		private global::System.Windows.Forms.Panel panel2;

		// Token: 0x0400004C RID: 76
		private global::System.Windows.Forms.Button btnOk;

		// Token: 0x0400004D RID: 77
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400004E RID: 78
		private global::System.Windows.Forms.Panel panel3;

		// Token: 0x0400004F RID: 79
		private global::System.Windows.Forms.ListView sceneList;
	}
}
