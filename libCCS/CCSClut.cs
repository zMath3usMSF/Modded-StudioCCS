using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StudioCCS.libCCS
{
	// Token: 0x0200001A RID: 26
	public class CCSClut : CCSBaseObject
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x0000E136 File Offset: 0x0000C336
		public CCSClut(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 1024;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000E168 File Offset: 0x0000C368
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.BlitGroup = bStream.ReadInt32();
			bStream.ReadInt32();
			bStream.ReadInt32();
			this.ColorCount = bStream.ReadInt32();
			this.Palette = new Color[this.ColorCount];
			for (int index = 0; index < this.ColorCount; index++)
			{
				this.Palette[index] = Util.ReadColorRGBA32(bStream);
				this.HasAlpha |= (this.Palette[index].A != byte.MaxValue);
			}
			return true;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000E204 File Offset: 0x0000C404
		public override TreeNode ToNode()
		{
			TreeNode node = base.ToNode();
			TreeNode treeNode = node;
			treeNode.Text += string.Format(" ({0} Colors)", this.ColorCount);
			return node;
		}

		// Token: 0x04000158 RID: 344
		public int ColorCount;

		// Token: 0x04000159 RID: 345
		public int BlitGroup;

		// Token: 0x0400015A RID: 346
		public Color[] Palette = null;

		// Token: 0x0400015B RID: 347
		public bool HasAlpha = false;
	}
}
