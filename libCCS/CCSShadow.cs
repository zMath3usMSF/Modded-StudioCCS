using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x0200002A RID: 42
	public class CCSShadow : CCSBaseObject
	{
		// Token: 0x06000160 RID: 352 RVA: 0x000142A9 File Offset: 0x000124A9
		public CCSShadow(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 6144;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000142CC File Offset: 0x000124CC
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.DataSize = sectionSize * 4;
			this.Data = new byte[this.DataSize];
			bStream.Read(this.Data, 0, this.DataSize);
			return true;
		}

		// Token: 0x040001E6 RID: 486
		public byte[] Data;

		// Token: 0x040001E7 RID: 487
		public int DataSize;
	}
}
