using System;
using System.IO;
using System.Windows.Forms;
using OpenTK;

namespace StudioCCS.libCCS
{
	// Token: 0x02000023 RID: 35
	public class CCSLight : CCSBaseObject
	{
		// Token: 0x06000124 RID: 292 RVA: 0x00010C20 File Offset: 0x0000EE20
		public CCSLight(int _objectID, CCSFile _parentFile)
		{
			this.ObjectType = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 1536;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00010CA0 File Offset: 0x0000EEA0
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.LightType = bStream.ReadInt32();
			return true;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00010CC0 File Offset: 0x0000EEC0
		public override TreeNode ToNode()
		{
			TreeNode node = base.ToNode();
			string str;
			switch (this.LightType)
			{
			case 0:
				str = " Type: None";
				break;
			case 1:
				str = " Type: Directional";
				break;
			case 2:
				str = " Type: Omni";
				break;
			default:
				str = string.Format(" Type: Unknown {0}", this.LightType);
				break;
			}
			TreeNode treeNode = node;
			treeNode.Text += str;
			return node;
		}

		// Token: 0x040001A4 RID: 420
		public const int CCS_LIGHT_DIRECTIONAL = 1;

		// Token: 0x040001A5 RID: 421
		public const int CCS_LIGHT_OMNI = 2;

		// Token: 0x040001A6 RID: 422
		public int LightType = 0;

		// Token: 0x040001A7 RID: 423
		public Vector4 Color = new Vector4(1f, 1f, 1f, 1f);

		// Token: 0x040001A8 RID: 424
		public Vector3 Position = Vector3.Zero;

		// Token: 0x040001A9 RID: 425
		public Vector3 Rotation = Vector3.Zero;

		// Token: 0x040001AA RID: 426
		public float Unk1 = 0f;

		// Token: 0x040001AB RID: 427
		public float Unk2 = 0f;
	}
}
