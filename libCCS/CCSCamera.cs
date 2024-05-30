using System;
using System.IO;
using OpenTK;

namespace StudioCCS.libCCS
{
	// Token: 0x02000018 RID: 24
	public class CCSCamera : CCSBaseObject
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x0000C938 File Offset: 0x0000AB38
		public CCSCamera(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 1280;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			return true;
		}

		// Token: 0x0400013B RID: 315
		public int Unk1 = 0;

		// Token: 0x0400013C RID: 316
		public Vector3 Position = Vector3.Zero;

		// Token: 0x0400013D RID: 317
		public Vector3 Rotation = Vector3.Zero;

		// Token: 0x0400013E RID: 318
		public float FOV = 45f;

		// Token: 0x0400013F RID: 319
		public float UnkFloat = 0f;
	}
}
