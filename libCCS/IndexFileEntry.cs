using System;
using System.Collections.Generic;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x0200002D RID: 45
	public class IndexFileEntry
	{
		// Token: 0x06000174 RID: 372 RVA: 0x00015544 File Offset: 0x00013744
		public void Read(BinaryReader bStream)
		{
			this.FileName = Util.ReadString(bStream, 32);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00015554 File Offset: 0x00013754
		public void AddObjectID(int _objectID)
		{
			this.ObjectIDs.Add(_objectID);
		}

		// Token: 0x04000202 RID: 514
		public string FileName = "";

		// Token: 0x04000203 RID: 515
		public List<int> ObjectIDs = new List<int>();
	}
}
