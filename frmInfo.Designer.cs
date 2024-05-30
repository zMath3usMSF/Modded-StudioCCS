namespace StudioCCS
{
	// Token: 0x02000007 RID: 7
	public partial class frmInfo : global::System.Windows.Forms.Form
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00004F24 File Offset: 0x00003124
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00004F5C File Offset: 0x0000315C
		private void InitializeComponent()
		{
			this.reportText = new global::System.Windows.Forms.RichTextBox();
			base.SuspendLayout();
			this.reportText.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.reportText.Font = new global::System.Drawing.Font("Lucida Console", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.reportText.Location = new global::System.Drawing.Point(0, 0);
			this.reportText.Name = "reportText";
			this.reportText.ReadOnly = true;
			this.reportText.Size = new global::System.Drawing.Size(547, 402);
			this.reportText.TabIndex = 1;
			this.reportText.Text = "";
			this.reportText.WordWrap = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(547, 402);
			base.Controls.Add(this.reportText);
			base.Name = "frmInfo";
			this.Text = "File Info Report";
			base.ResumeLayout(false);
		}

		// Token: 0x04000041 RID: 65
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000042 RID: 66
		private global::System.Windows.Forms.RichTextBox reportText;
	}
}
