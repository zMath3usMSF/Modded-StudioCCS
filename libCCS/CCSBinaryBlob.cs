using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x02000015 RID: 21
	public class CCSBinaryBlob : CCSBaseObject
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x0000C4D0 File Offset: 0x0000A6D0
		public CCSBinaryBlob(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 9216;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000C4F8 File Offset: 0x0000A6F8
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.DataSize = sectionSize * 4;
			this.Data = new byte[this.DataSize];
			bStream.Read(this.Data, 0, this.DataSize);
			return true;
		}

		// Token: 0x0400012D RID: 301
		public byte[] Data;

		// Token: 0x0400012E RID: 302
		public int DataSize;
	}
}
