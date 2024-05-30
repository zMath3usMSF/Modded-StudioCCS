using System;
using System.IO;
using System.Windows.Forms;

namespace StudioCCS.libCCS
{
	// Token: 0x02000026 RID: 38
	public class CCSMorpher : CCSBaseObject
	{
		// Token: 0x06000143 RID: 323 RVA: 0x00013B5B File Offset: 0x00011D5B
		public CCSMorpher(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 6400;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00013B8C File Offset: 0x00011D8C
		public override bool Init()
		{
			this.BaseModelRef = this.ParentFile.GetObject<CCSModel>(this.BaseModelID);
			return true;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00013BB8 File Offset: 0x00011DB8
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.BaseModelID = bStream.ReadInt32();
			return true;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00013BD8 File Offset: 0x00011DD8
		public override TreeNode ToNode()
		{
			TreeNode node = base.ToNode();
			TreeNode treeNode = node;
			treeNode.Text += string.Format(" Base: {0}", this.ParentFile.GetSubObjectName(this.BaseModelID));
			return node;
		}

		// Token: 0x040001D7 RID: 471
		public int BaseModelID = 0;

		// Token: 0x040001D8 RID: 472
		private CCSModel BaseModelRef = null;
	}
}
