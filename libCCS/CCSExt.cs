using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x0200001D RID: 29
	public class CCSExt : CCSBaseObject
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x0000E479 File Offset: 0x0000C679
		public CCSExt(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 2560;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000E49C File Offset: 0x0000C69C
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.ReferencedParentID = bStream.ReadInt32();
			this.ReferencedObjectID = bStream.ReadInt32();
			return true;
		}

		// Token: 0x04000160 RID: 352
		public int ReferencedParentID;

		// Token: 0x04000161 RID: 353
		public int ReferencedObjectID;
	}
}
