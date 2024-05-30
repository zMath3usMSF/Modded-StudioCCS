using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x0200002E RID: 46
	public class IndexObjectEntry
	{
		// Token: 0x06000177 RID: 375 RVA: 0x00015582 File Offset: 0x00013782
		public void Read(BinaryReader bStream)
		{
			this.ObjectName = Util.ReadString(bStream, 30);
			this.FileID = bStream.ReadInt16();
		}

		// Token: 0x04000204 RID: 516
		public string ObjectName = "";

		// Token: 0x04000205 RID: 517
		public short FileID = 0;

		// Token: 0x04000206 RID: 518
		public CCSBaseObject ObjectRef = null;

		// Token: 0x04000207 RID: 519
		public int ObjectType = 0;

		// Token: 0x04000208 RID: 520
		public int ObjectOffset = 0;
	}
}
