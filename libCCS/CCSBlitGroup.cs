using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x02000016 RID: 22
	public class CCSBlitGroup : CCSBaseObject
	{
		// Token: 0x060000BB RID: 187 RVA: 0x0000C539 File Offset: 0x0000A739
		public CCSBlitGroup(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 4096;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000C55C File Offset: 0x0000A75C
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.DataSize = sectionSize * 4;
			this.Data = new byte[this.DataSize];
			bStream.Read(this.Data, 0, this.DataSize);
			return true;
		}

		// Token: 0x0400012F RID: 303
		public byte[] Data;

		// Token: 0x04000130 RID: 304
		public int DataSize;
	}
}
