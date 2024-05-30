using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x02000022 RID: 34
	public class CCSLayer : CCSBaseObject
	{
		// Token: 0x06000120 RID: 288 RVA: 0x00010BB6 File Offset: 0x0000EDB6
		public CCSLayer(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 5888;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00010BDC File Offset: 0x0000EDDC
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.DataSize = sectionSize * 4;
			this.Data = new byte[this.DataSize];
			bStream.Read(this.Data, 0, this.DataSize);
			return true;
		}

		// Token: 0x040001A2 RID: 418
		public byte[] Data;

		// Token: 0x040001A3 RID: 419
		public int DataSize;
	}
}
