using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudioCCS
{
	// Token: 0x0200000B RID: 11
	internal class ViewportPicbox : PictureBox
	{
		// Token: 0x06000034 RID: 52 RVA: 0x0000623D File Offset: 0x0000443D
		public ViewportPicbox()
		{
			base.SetStyle(ControlStyles.Selectable, true);
			base.TabStop = true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000625C File Offset: 0x0000445C
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.Focus();
			base.OnMouseDown(e);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000626E File Offset: 0x0000446E
		protected override void OnEnter(EventArgs e)
		{
			base.Invalidate();
			base.OnEnter(e);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00006280 File Offset: 0x00004480
		protected override void OnLeave(EventArgs e)
		{
			base.Invalidate();
			base.OnLeave(e);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00006294 File Offset: 0x00004494
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			bool flag = !this.Focused;
			if (!flag)
			{
				Rectangle clientRectangle = base.ClientRectangle;
				clientRectangle.Inflate(-2, -2);
				ControlPaint.DrawFocusRectangle(pe.Graphics, clientRectangle);
			}
		}
	}
}
