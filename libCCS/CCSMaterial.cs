using System;
using System.IO;
using System.Windows.Forms;
using OpenTK;

namespace StudioCCS.libCCS
{
	// Token: 0x02000024 RID: 36
	public class CCSMaterial : CCSBaseObject
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00010D3C File Offset: 0x0000EF3C
		public CCSMaterial(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 512;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00010D90 File Offset: 0x0000EF90
		public override bool Init()
		{
			this.TextureRef = this.ParentFile.GetObject<CCSTexture>(this.TextureID);
			return true;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00010DBC File Offset: 0x0000EFBC
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.TextureID = bStream.ReadInt32();
			this.Alpha = bStream.ReadSingle();
			this.TextureOffset = ((this.ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen1) ? new Vector2(bStream.ReadSingle(), bStream.ReadSingle()) : Util.ReadVec2UV(bStream));
			int num = sectionSize - 3;
			bool flag = this.ParentFile.GetVersion() > CCSFileHeader.CCSVersion.Gen1;
			if (flag)
			{
				num--;
			}
			bool flag2 = num > 0;
			if (flag2)
			{
				bStream.BaseStream.Seek((long)(num * 4), SeekOrigin.Current);
			}
			return true;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00010E48 File Offset: 0x0000F048
		public override TreeNode ToNode()
		{
			TreeNode node = base.ToNode();
			TreeNode treeNode = node;
			treeNode.Text += string.Format(" Texture: {0}", this.ParentFile.GetSubObjectName(this.TextureID));
			return node;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00010E90 File Offset: 0x0000F090
		public string GetReport(int level = 0)
		{
			string str = Util.Indent(level, false) + string.Format("Material 0x{0:X}: {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID));
			return (this.TextureID == 0) ? (str + Util.Indent(level + 1, false) + string.Format("Texture: None\n", Array.Empty<object>())) : (str + Util.Indent(level + 1, false) + string.Format("Texture: 0x{0:X}: {1}\n", this.TextureID, this.ParentFile.GetSubObjectName(this.TextureID)));
		}

		// Token: 0x040001AC RID: 428
		public int TextureID = 0;

		// Token: 0x040001AD RID: 429
		public float Alpha = 1f;

		// Token: 0x040001AE RID: 430
		public Vector2 TextureOffset = Vector2.Zero;

		// Token: 0x040001AF RID: 431
		public CCSTexture TextureRef = null;
	}
}
