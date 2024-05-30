using System;
using System.IO;
using OpenTK;

namespace StudioCCS.libCCS
{
	// Token: 0x0200001B RID: 27
	public class CCSDummy : CCSBaseObject
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0000E248 File Offset: 0x0000C448
		public CCSDummy(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 4864;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000E2AC File Offset: 0x0000C4AC
		public CCSDummy(int _objectID, CCSFile _parentFile, int _objType)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = _objType;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool Init()
		{
			return true;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public override bool DeInit()
		{
			return true;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000E30C File Offset: 0x0000C50C
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.Position = Util.ReadVec3Position(bStream);
			bool flag = this.ObjectType == 5120;
			if (flag)
			{
				this.Rotation = Util.ReadVec3Rotation(bStream);
			}
			return true;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000E348 File Offset: 0x0000C548
		public Matrix4 Matrix()
		{
			return Matrix4.CreateFromQuaternion(new Quaternion(this.Rotation)) * Matrix4.CreateTranslation(this.Position);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000E36C File Offset: 0x0000C56C
		public void DumpToTxt(StreamWriter fStream)
		{
			fStream.WriteLine(this.ParentFile.GetSubObjectName(this.ObjectID));
			fStream.WriteLine(string.Format("{0}\t{1}\t{2}", this.Position.X, this.Position.Y, this.Position.Z));
			fStream.WriteLine(string.Format("{0}\t{1}\t{2}", this.Rotation.X, this.Rotation.Y, this.Rotation.Z));
		}

		// Token: 0x0400015C RID: 348
		public Vector3 Position = new Vector3(0f, 0f, 0f);

		// Token: 0x0400015D RID: 349
		public Vector3 Rotation = new Vector3(0f, 0f, 0f);
	}
}
