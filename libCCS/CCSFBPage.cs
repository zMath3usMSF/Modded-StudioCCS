using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x0200001E RID: 30
	public class CCSFBPage : CCSBaseObject
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x0000E4C7 File Offset: 0x0000C6C7
		public CCSFBPage(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 4352;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000E4EC File Offset: 0x0000C6EC
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.DataSize = sectionSize * 4;
			this.Data = new byte[this.DataSize];
			bStream.Read(this.Data, 0, this.DataSize);
			return true;
		}

		// Token: 0x04000162 RID: 354
		public byte[] Data;

		// Token: 0x04000163 RID: 355
		public int DataSize;
	}
}
