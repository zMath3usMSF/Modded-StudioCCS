using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x02000029 RID: 41
	public class CCSPCM : CCSBaseObject
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00014245 File Offset: 0x00012445
		public CCSPCM(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 8704;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00014268 File Offset: 0x00012468
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.DataSize = sectionSize * 4;
			this.Data = new byte[this.DataSize];
			bStream.Read(this.Data, 0, this.DataSize);
			return true;
		}

		// Token: 0x040001E4 RID: 484
		public byte[] Data;

		// Token: 0x040001E5 RID: 485
		public int DataSize;
	}
}
