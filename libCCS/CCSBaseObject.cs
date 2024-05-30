using System;
using System.IO;
using System.Windows.Forms;

namespace StudioCCS.libCCS
{
	// Token: 0x02000013 RID: 19
	public abstract class CCSBaseObject
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x0000BE04 File Offset: 0x0000A004
		public virtual TreeNode ToNode()
		{
			return new TreeNode(string.Format("{0}: {1}", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID)))
			{
				Tag = new TreeNodeTag(this.ParentFile, this.ObjectID, this.ObjectType)
			};
		}

		// Token: 0x060000A8 RID: 168
		public abstract bool Init();

		// Token: 0x060000A9 RID: 169
		public abstract bool DeInit();

		// Token: 0x060000AA RID: 170
		public abstract bool Read(BinaryReader bStream, int sectionSize);

		// Token: 0x04000117 RID: 279
		public int ObjectID = 0;

		// Token: 0x04000118 RID: 280
		public int ObjectType = 0;

		// Token: 0x04000119 RID: 281
		public CCSFile ParentFile;
	}
}
