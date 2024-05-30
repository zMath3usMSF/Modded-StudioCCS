using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x0200001F RID: 31
	public class CCSFBRect : CCSBaseObject
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x0000E52D File Offset: 0x0000C72D
		public CCSFBRect(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 4608;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000E550 File Offset: 0x0000C750
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.DataSize = sectionSize * 4;
			this.Data = new byte[this.DataSize];
			bStream.Read(this.Data, 0, this.DataSize);
			return true;
		}

		// Token: 0x04000164 RID: 356
		public byte[] Data;

		// Token: 0x04000165 RID: 357
		public int DataSize;
	}
}
