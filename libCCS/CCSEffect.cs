using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x0200001C RID: 28
	public class CCSEffect : CCSBaseObject
	{
		// Token: 0x060000EC RID: 236 RVA: 0x0000E414 File Offset: 0x0000C614
		public CCSEffect(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 3584;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000E438 File Offset: 0x0000C638
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.DataSize = sectionSize * 4;
			this.Data = new byte[this.DataSize];
			bStream.Read(this.Data, 0, this.DataSize);
			return true;
		}

		// Token: 0x0400015E RID: 350
		public byte[] Data;

		// Token: 0x0400015F RID: 351
		public int DataSize;
	}
}
