using System;
using StudioCCS.libCCS;

namespace StudioCCS
{
	// Token: 0x02000012 RID: 18
	public class TreeNodeTag
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x0000BD48 File Offset: 0x00009F48
		public TreeNodeTag(CCSFile _File, int _objectID, int _objectType)
		{
			this.File = _File;
			this.ObjectID = _objectID;
			this.ObjectType = _objectType;
			this.Type = TreeNodeTag.NodeType.Main;
			this.SubID = 0;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000BDA4 File Offset: 0x00009FA4
		public TreeNodeTag(CCSFile _file, int _objectID, int _objectType, TreeNodeTag.NodeType _nodeType, int _subID)
		{
			this.File = _file;
			this.ObjectID = _objectID;
			this.ObjectType = _objectType;
			this.Type = _nodeType;
			this.SubID = _subID;
		}

		// Token: 0x04000112 RID: 274
		public CCSFile File = null;

		// Token: 0x04000113 RID: 275
		public int ObjectID = 0;

		// Token: 0x04000114 RID: 276
		public int ObjectType = 0;

		// Token: 0x04000115 RID: 277
		public int SubID = 0;

		// Token: 0x04000116 RID: 278
		public TreeNodeTag.NodeType Type = TreeNodeTag.NodeType.SubNode;

		// Token: 0x02000042 RID: 66
		public enum NodeType
		{
			// Token: 0x0400025F RID: 607
			File,
			// Token: 0x04000260 RID: 608
			Main,
			// Token: 0x04000261 RID: 609
			SubNode
		}
	}
}
