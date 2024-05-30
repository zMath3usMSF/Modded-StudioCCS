using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x02000028 RID: 40
	public class CCSParticle : CCSBaseObject
	{
		// Token: 0x06000158 RID: 344 RVA: 0x000141DF File Offset: 0x000123DF
		public CCSParticle(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 3328;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00014204 File Offset: 0x00012404
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.DataSize = sectionSize * 4;
			this.Data = new byte[this.DataSize];
			bStream.Read(this.Data, 0, this.DataSize);
			return true;
		}

		// Token: 0x040001E2 RID: 482
		public byte[] Data;

		// Token: 0x040001E3 RID: 483
		public int DataSize;
	}
}
