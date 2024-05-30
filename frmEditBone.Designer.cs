namespace StudioCCS
{
	// Token: 0x02000005 RID: 5
	public partial class frmEditBone : global::System.Windows.Forms.Form
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00003504 File Offset: 0x00001704
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000353C File Offset: 0x0000173C
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new global::System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.lblBoneName = new global::System.Windows.Forms.Label();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.txtRotZ = new global::System.Windows.Forms.TextBox();
			this.label8 = new global::System.Windows.Forms.Label();
			this.txtRotY = new global::System.Windows.Forms.TextBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.txtRotX = new global::System.Windows.Forms.TextBox();
			this.btnUpdate = new global::System.Windows.Forms.Button();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.label10 = new global::System.Windows.Forms.Label();
			this.txtScaleZ = new global::System.Windows.Forms.TextBox();
			this.label11 = new global::System.Windows.Forms.Label();
			this.txtScaleY = new global::System.Windows.Forms.TextBox();
			this.label12 = new global::System.Windows.Forms.Label();
			this.txtScaleX = new global::System.Windows.Forms.TextBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.txtPosZ = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtPosY = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.txtPosX = new global::System.Windows.Forms.TextBox();
			this.treeBones = new global::System.Windows.Forms.TreeView();
			this.menuStrip1 = new global::System.Windows.Forms.MenuStrip();
			this.editToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.loadPoseToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.savePoseToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.clearRotationValuesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.clearScaleValuesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 276f));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.treeBones, 0, 0);
			this.tableLayoutPanel1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new global::System.Drawing.Point(0, 24);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel1.Size = new global::System.Drawing.Size(449, 231);
			this.tableLayoutPanel1.TabIndex = 0;
			this.panel1.Controls.Add(this.lblBoneName);
			this.panel1.Controls.Add(this.groupBox3);
			this.panel1.Controls.Add(this.btnUpdate);
			this.panel1.Controls.Add(this.groupBox4);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new global::System.Drawing.Point(176, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(270, 225);
			this.panel1.TabIndex = 1;
			this.lblBoneName.Location = new global::System.Drawing.Point(5, 5);
			this.lblBoneName.Name = "lblBoneName";
			this.lblBoneName.Size = new global::System.Drawing.Size(256, 15);
			this.lblBoneName.TabIndex = 9;
			this.lblBoneName.Text = "Bone Name Here";
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.txtRotZ);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.txtRotY);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.txtRotX);
			this.groupBox3.Location = new global::System.Drawing.Point(4, 119);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new global::System.Drawing.Size(130, 90);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Rotation";
			this.label7.Location = new global::System.Drawing.Point(6, 67);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(18, 20);
			this.label7.TabIndex = 5;
			this.label7.Text = "Z";
			this.txtRotZ.Location = new global::System.Drawing.Point(30, 64);
			this.txtRotZ.Name = "txtRotZ";
			this.txtRotZ.Size = new global::System.Drawing.Size(90, 20);
			this.txtRotZ.TabIndex = 4;
			this.label8.Location = new global::System.Drawing.Point(6, 44);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(18, 20);
			this.label8.TabIndex = 3;
			this.label8.Text = "Y";
			this.txtRotY.Location = new global::System.Drawing.Point(30, 41);
			this.txtRotY.Name = "txtRotY";
			this.txtRotY.Size = new global::System.Drawing.Size(90, 20);
			this.txtRotY.TabIndex = 2;
			this.label9.Location = new global::System.Drawing.Point(6, 22);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(18, 20);
			this.label9.TabIndex = 1;
			this.label9.Text = "X";
			this.txtRotX.Location = new global::System.Drawing.Point(30, 19);
			this.txtRotX.Name = "txtRotX";
			this.txtRotX.Size = new global::System.Drawing.Size(90, 20);
			this.txtRotX.TabIndex = 0;
			this.btnUpdate.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.btnUpdate.Location = new global::System.Drawing.Point(194, 201);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new global::System.Drawing.Size(75, 23);
			this.btnUpdate.TabIndex = 8;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new global::System.EventHandler(this.BtnUpdateClick);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.txtScaleZ);
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.txtScaleY);
			this.groupBox4.Controls.Add(this.label12);
			this.groupBox4.Controls.Add(this.txtScaleX);
			this.groupBox4.Location = new global::System.Drawing.Point(140, 23);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new global::System.Drawing.Size(130, 90);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Scale";
			this.label10.Location = new global::System.Drawing.Point(6, 67);
			this.label10.Name = "label10";
			this.label10.Size = new global::System.Drawing.Size(18, 20);
			this.label10.TabIndex = 5;
			this.label10.Text = "Z";
			this.txtScaleZ.Location = new global::System.Drawing.Point(30, 64);
			this.txtScaleZ.Name = "txtScaleZ";
			this.txtScaleZ.Size = new global::System.Drawing.Size(90, 20);
			this.txtScaleZ.TabIndex = 4;
			this.label11.Location = new global::System.Drawing.Point(6, 44);
			this.label11.Name = "label11";
			this.label11.Size = new global::System.Drawing.Size(18, 20);
			this.label11.TabIndex = 3;
			this.label11.Text = "Y";
			this.txtScaleY.Location = new global::System.Drawing.Point(30, 41);
			this.txtScaleY.Name = "txtScaleY";
			this.txtScaleY.Size = new global::System.Drawing.Size(90, 20);
			this.txtScaleY.TabIndex = 2;
			this.label12.Location = new global::System.Drawing.Point(6, 22);
			this.label12.Name = "label12";
			this.label12.Size = new global::System.Drawing.Size(18, 20);
			this.label12.TabIndex = 1;
			this.label12.Text = "X";
			this.txtScaleX.Location = new global::System.Drawing.Point(30, 19);
			this.txtScaleX.Name = "txtScaleX";
			this.txtScaleX.Size = new global::System.Drawing.Size(90, 20);
			this.txtScaleX.TabIndex = 0;
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtPosZ);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtPosY);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtPosX);
			this.groupBox1.Location = new global::System.Drawing.Point(4, 23);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(130, 90);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Position";
			this.label3.Location = new global::System.Drawing.Point(6, 67);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(18, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "Z";
			this.txtPosZ.Location = new global::System.Drawing.Point(30, 64);
			this.txtPosZ.Name = "txtPosZ";
			this.txtPosZ.Size = new global::System.Drawing.Size(90, 20);
			this.txtPosZ.TabIndex = 4;
			this.label2.Location = new global::System.Drawing.Point(6, 44);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(18, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Y";
			this.txtPosY.Location = new global::System.Drawing.Point(30, 41);
			this.txtPosY.Name = "txtPosY";
			this.txtPosY.Size = new global::System.Drawing.Size(90, 20);
			this.txtPosY.TabIndex = 2;
			this.label1.Location = new global::System.Drawing.Point(6, 22);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(18, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "X";
			this.txtPosX.Location = new global::System.Drawing.Point(30, 19);
			this.txtPosX.Name = "txtPosX";
			this.txtPosX.Size = new global::System.Drawing.Size(90, 20);
			this.txtPosX.TabIndex = 0;
			this.treeBones.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.treeBones.Location = new global::System.Drawing.Point(3, 3);
			this.treeBones.Name = "treeBones";
			this.treeBones.Size = new global::System.Drawing.Size(167, 225);
			this.treeBones.TabIndex = 2;
			this.treeBones.AfterSelect += new global::System.Windows.Forms.TreeViewEventHandler(this.TreeBonesAfterSelect);
			this.menuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.editToolStripMenuItem
			});
			this.menuStrip1.Location = new global::System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new global::System.Drawing.Size(449, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			this.editToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.loadPoseToolStripMenuItem,
				this.savePoseToolStripMenuItem,
				this.clearRotationValuesToolStripMenuItem,
				this.clearScaleValuesToolStripMenuItem
			});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new global::System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			this.loadPoseToolStripMenuItem.Name = "loadPoseToolStripMenuItem";
			this.loadPoseToolStripMenuItem.Size = new global::System.Drawing.Size(185, 22);
			this.loadPoseToolStripMenuItem.Text = "Load Pose";
			this.loadPoseToolStripMenuItem.Click += new global::System.EventHandler(this.LoadPoseToolStripMenuItemClick);
			this.savePoseToolStripMenuItem.Name = "savePoseToolStripMenuItem";
			this.savePoseToolStripMenuItem.Size = new global::System.Drawing.Size(185, 22);
			this.savePoseToolStripMenuItem.Text = "Save Pose";
			this.savePoseToolStripMenuItem.Click += new global::System.EventHandler(this.SavePoseToolStripMenuItemClick);
			this.clearRotationValuesToolStripMenuItem.Name = "clearRotationValuesToolStripMenuItem";
			this.clearRotationValuesToolStripMenuItem.Size = new global::System.Drawing.Size(185, 22);
			this.clearRotationValuesToolStripMenuItem.Text = "Clear Rotation Values";
			this.clearRotationValuesToolStripMenuItem.Click += new global::System.EventHandler(this.ClearRotationValuesToolStripMenuItemClick);
			this.clearScaleValuesToolStripMenuItem.Name = "clearScaleValuesToolStripMenuItem";
			this.clearScaleValuesToolStripMenuItem.Size = new global::System.Drawing.Size(185, 22);
			this.clearScaleValuesToolStripMenuItem.Text = "Clear Scale Values";
			this.clearScaleValuesToolStripMenuItem.Click += new global::System.EventHandler(this.ClearScaleValuesToolStripMenuItemClick);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(449, 255);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.menuStrip1);
			base.Name = "frmEditBone";
			this.Text = "frmEditBone";
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.FrmEditBoneFormClosed);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000013 RID: 19
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000014 RID: 20
		private global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

		// Token: 0x04000015 RID: 21
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x04000016 RID: 22
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000017 RID: 23
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000018 RID: 24
		private global::System.Windows.Forms.TextBox txtPosX;

		// Token: 0x04000019 RID: 25
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x0400001A RID: 26
		private global::System.Windows.Forms.Label label10;

		// Token: 0x0400001B RID: 27
		private global::System.Windows.Forms.TextBox txtScaleZ;

		// Token: 0x0400001C RID: 28
		private global::System.Windows.Forms.Label label11;

		// Token: 0x0400001D RID: 29
		private global::System.Windows.Forms.TextBox txtScaleY;

		// Token: 0x0400001E RID: 30
		private global::System.Windows.Forms.Label label12;

		// Token: 0x0400001F RID: 31
		private global::System.Windows.Forms.TextBox txtScaleX;

		// Token: 0x04000020 RID: 32
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04000021 RID: 33
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04000022 RID: 34
		private global::System.Windows.Forms.TextBox txtRotZ;

		// Token: 0x04000023 RID: 35
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04000024 RID: 36
		private global::System.Windows.Forms.TextBox txtRotY;

		// Token: 0x04000025 RID: 37
		private global::System.Windows.Forms.Label label9;

		// Token: 0x04000026 RID: 38
		private global::System.Windows.Forms.TextBox txtRotX;

		// Token: 0x04000027 RID: 39
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000028 RID: 40
		private global::System.Windows.Forms.TextBox txtPosZ;

		// Token: 0x04000029 RID: 41
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400002A RID: 42
		private global::System.Windows.Forms.TextBox txtPosY;

		// Token: 0x0400002B RID: 43
		private global::System.Windows.Forms.Button btnUpdate;

		// Token: 0x0400002C RID: 44
		private global::System.Windows.Forms.MenuStrip menuStrip1;

		// Token: 0x0400002D RID: 45
		private global::System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;

		// Token: 0x0400002E RID: 46
		private global::System.Windows.Forms.ToolStripMenuItem loadPoseToolStripMenuItem;

		// Token: 0x0400002F RID: 47
		private global::System.Windows.Forms.ToolStripMenuItem savePoseToolStripMenuItem;

		// Token: 0x04000030 RID: 48
		private global::System.Windows.Forms.TreeView treeBones;

		// Token: 0x04000031 RID: 49
		private global::System.Windows.Forms.Label lblBoneName;

		// Token: 0x04000032 RID: 50
		private global::System.Windows.Forms.ToolStripMenuItem clearRotationValuesToolStripMenuItem;

		// Token: 0x04000033 RID: 51
		private global::System.Windows.Forms.ToolStripMenuItem clearScaleValuesToolStripMenuItem;
	}
}
