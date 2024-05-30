namespace StudioCCS
{
	// Token: 0x02000006 RID: 6
	public partial class frmExportToOBJ : global::System.Windows.Forms.Form
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000046C0 File Offset: 0x000028C0
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000046F8 File Offset: 0x000028F8
		private void InitializeComponent()
		{
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.txtExportPath = new global::System.Windows.Forms.TextBox();
			this.btnSelectDir = new global::System.Windows.Forms.Button();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.chkExportDummies = new global::System.Windows.Forms.CheckBox();
			this.chkModelWithNormals = new global::System.Windows.Forms.CheckBox();
			this.chkSplitCollision = new global::System.Windows.Forms.CheckBox();
			this.chkSplitSubModels = new global::System.Windows.Forms.CheckBox();
			this.chkExportCollision = new global::System.Windows.Forms.CheckBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnDoExport = new global::System.Windows.Forms.Button();
			this.chkDumpAnime = new global::System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.txtExportPath);
			this.groupBox1.Controls.Add(this.btnSelectDir);
			this.groupBox1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new global::System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(526, 40);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Ouput Path:";
			this.txtExportPath.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.txtExportPath.Location = new global::System.Drawing.Point(3, 16);
			this.txtExportPath.Name = "txtExportPath";
			this.txtExportPath.Size = new global::System.Drawing.Size(491, 20);
			this.txtExportPath.TabIndex = 1;
			this.btnSelectDir.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.btnSelectDir.Location = new global::System.Drawing.Point(494, 16);
			this.btnSelectDir.Name = "btnSelectDir";
			this.btnSelectDir.Size = new global::System.Drawing.Size(29, 21);
			this.btnSelectDir.TabIndex = 0;
			this.btnSelectDir.Text = "...";
			this.btnSelectDir.UseVisualStyleBackColor = true;
			this.btnSelectDir.Click += new global::System.EventHandler(this.BtnSelectDirClick);
			this.groupBox2.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox2.Controls.Add(this.chkDumpAnime);
			this.groupBox2.Controls.Add(this.chkExportDummies);
			this.groupBox2.Controls.Add(this.chkModelWithNormals);
			this.groupBox2.Controls.Add(this.chkSplitCollision);
			this.groupBox2.Controls.Add(this.chkSplitSubModels);
			this.groupBox2.Controls.Add(this.chkExportCollision);
			this.groupBox2.Location = new global::System.Drawing.Point(0, 40);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new global::System.Drawing.Size(523, 75);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Options:";
			this.chkExportDummies.Location = new global::System.Drawing.Point(312, 14);
			this.chkExportDummies.Name = "chkExportDummies";
			this.chkExportDummies.Size = new global::System.Drawing.Size(182, 24);
			this.chkExportDummies.TabIndex = 4;
			this.chkExportDummies.Text = "Export Dummies to .txt";
			this.chkExportDummies.UseVisualStyleBackColor = true;
			this.chkModelWithNormals.Location = new global::System.Drawing.Point(168, 45);
			this.chkModelWithNormals.Name = "chkModelWithNormals";
			this.chkModelWithNormals.Size = new global::System.Drawing.Size(137, 24);
			this.chkModelWithNormals.TabIndex = 3;
			this.chkModelWithNormals.Text = "Export Model Normals";
			this.chkModelWithNormals.UseVisualStyleBackColor = true;
			this.chkSplitCollision.Enabled = false;
			this.chkSplitCollision.Location = new global::System.Drawing.Point(13, 45);
			this.chkSplitCollision.Name = "chkSplitCollision";
			this.chkSplitCollision.Size = new global::System.Drawing.Size(149, 24);
			this.chkSplitCollision.TabIndex = 2;
			this.chkSplitCollision.Text = "Split Collision Meshes";
			this.chkSplitCollision.UseVisualStyleBackColor = true;
			this.chkSplitSubModels.Location = new global::System.Drawing.Point(168, 14);
			this.chkSplitSubModels.Name = "chkSplitSubModels";
			this.chkSplitSubModels.Size = new global::System.Drawing.Size(137, 24);
			this.chkSplitSubModels.TabIndex = 1;
			this.chkSplitSubModels.Text = "Split Sub Model pieces";
			this.chkSplitSubModels.UseVisualStyleBackColor = true;
			this.chkExportCollision.Location = new global::System.Drawing.Point(13, 14);
			this.chkExportCollision.Name = "chkExportCollision";
			this.chkExportCollision.Size = new global::System.Drawing.Size(149, 24);
			this.chkExportCollision.TabIndex = 0;
			this.chkExportCollision.Text = "Export Collision Meshes";
			this.chkExportCollision.UseVisualStyleBackColor = true;
			this.chkExportCollision.CheckedChanged += new global::System.EventHandler(this.ChkExportCollisionCheckedChanged);
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new global::System.Drawing.Point(446, 121);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new global::System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnDoExport.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.btnDoExport.Enabled = false;
			this.btnDoExport.Location = new global::System.Drawing.Point(365, 121);
			this.btnDoExport.Name = "btnDoExport";
			this.btnDoExport.Size = new global::System.Drawing.Size(75, 23);
			this.btnDoExport.TabIndex = 4;
			this.btnDoExport.Text = "Export";
			this.btnDoExport.UseVisualStyleBackColor = true;
			this.chkDumpAnime.Location = new global::System.Drawing.Point(312, 45);
			this.chkDumpAnime.Name = "chkDumpAnime";
			this.chkDumpAnime.Size = new global::System.Drawing.Size(202, 24);
			this.chkDumpAnime.TabIndex = 5;
			this.chkDumpAnime.Text = "Dump Anime (First Frame) to Text";
			this.chkDumpAnime.UseVisualStyleBackColor = true;
			base.AcceptButton = this.btnDoExport;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new global::System.Drawing.Size(526, 148);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnDoExport);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.MaximizeBox = false;
			this.MaximumSize = new global::System.Drawing.Size(542, 187);
			base.MinimizeBox = false;
			this.MinimumSize = new global::System.Drawing.Size(542, 187);
			base.Name = "frmExportToOBJ";
			this.Text = "Export To .OBJ...";
			base.TopMost = true;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000034 RID: 52
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000035 RID: 53
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000036 RID: 54
		private global::System.Windows.Forms.Button btnSelectDir;

		// Token: 0x04000037 RID: 55
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04000038 RID: 56
		public global::System.Windows.Forms.CheckBox chkExportCollision;

		// Token: 0x04000039 RID: 57
		public global::System.Windows.Forms.TextBox txtExportPath;

		// Token: 0x0400003A RID: 58
		public global::System.Windows.Forms.CheckBox chkSplitSubModels;

		// Token: 0x0400003B RID: 59
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400003C RID: 60
		private global::System.Windows.Forms.Button btnDoExport;

		// Token: 0x0400003D RID: 61
		public global::System.Windows.Forms.CheckBox chkSplitCollision;

		// Token: 0x0400003E RID: 62
		public global::System.Windows.Forms.CheckBox chkModelWithNormals;

		// Token: 0x0400003F RID: 63
		public global::System.Windows.Forms.CheckBox chkExportDummies;

		// Token: 0x04000040 RID: 64
		public global::System.Windows.Forms.CheckBox chkDumpAnime;
	}
}
