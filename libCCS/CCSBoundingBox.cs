using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace StudioCCS.libCCS
{
	// Token: 0x02000017 RID: 23
	public class CCSBoundingBox : CCSBaseObject
	{
		// Token: 0x060000BF RID: 191 RVA: 0x0000C5A0 File Offset: 0x0000A7A0
		public CCSBoundingBox(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 3072;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		public override bool Init()
		{
			bool flag = CCSBoundingBox.ProgramID == -1;
			if (flag)
			{
				CCSBoundingBox.ProgramID = Scene.LoadProgram("BBox", true);
				bool flag2 = CCSBoundingBox.ProgramID == -1;
				if (flag2)
				{
					return false;
				}
				CCSBoundingBox.AttribVMin = GL.GetAttribLocation(CCSBoundingBox.ProgramID, "VMin");
				CCSBoundingBox.AttribVMax = GL.GetAttribLocation(CCSBoundingBox.ProgramID, "VMax");
				CCSBoundingBox.AttribVColor = GL.GetAttribLocation(CCSBoundingBox.ProgramID, "VColor");
				CCSBoundingBox.UniMatrix = GL.GetUniformLocation(CCSBoundingBox.ProgramID, "UMatrix");
				bool flag3 = CCSBoundingBox.AttribVMin == -1 || CCSBoundingBox.AttribVMax == -1 || CCSBoundingBox.AttribVColor == -1 || CCSBoundingBox.UniMatrix == -1;
				if (flag3)
				{
					Logger.LogError("CCSBBox: Error Getting Shader Attributes/Uniforms:\n", Logger.LogType.LogAll, "Init", 71);
					Logger.LogError(string.Format("\tVMin: {0}, VMax: {1}, VColor: {2}, UMatrix: {3}", new object[]
					{
						CCSBoundingBox.AttribVMin,
						CCSBoundingBox.AttribVMax,
						CCSBoundingBox.AttribVColor,
						CCSBoundingBox.UniMatrix
					}), Logger.LogType.LogAll, "Init", 72);
					return false;
				}
			}
			CCSBoundingBox.ProgramRefs++;
			this.Box[0] = default(CCSBoundingBox.BBox);
			this.VertexArrayID = GL.GenVertexArray();
			GL.BindVertexArray(this.VertexArrayID);
			this.VertexBufferID = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexBufferID);
			Type type = this.Box[0].GetType();
			int num = Marshal.SizeOf(type);
			GL.BufferData<CCSBoundingBox.BBox>(BufferTarget.ArrayBuffer, num, this.Box, BufferUsageHint.DynamicDraw);
			GL.EnableVertexAttribArray(CCSBoundingBox.AttribVMin);
			GL.VertexAttribPointer(CCSBoundingBox.AttribVMin, 3, VertexAttribPointerType.Float, false, num, Marshal.OffsetOf(type, "Min"));
			GL.EnableVertexAttribArray(CCSBoundingBox.AttribVMax);
			GL.VertexAttribPointer(CCSBoundingBox.AttribVMax, 3, VertexAttribPointerType.Float, false, num, Marshal.OffsetOf(type, "Max"));
			GL.EnableVertexAttribArray(CCSBoundingBox.AttribVColor);
			GL.VertexAttribPointer(CCSBoundingBox.AttribVColor, 4, VertexAttribPointerType.Float, false, num, Marshal.OffsetOf(type, "Color"));
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			return true;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000C838 File Offset: 0x0000AA38
		public override bool DeInit()
		{
			bool flag = this.VertexArrayID != -1;
			if (flag)
			{
				GL.DeleteVertexArray(this.VertexArrayID);
			}
			bool flag2 = this.VertexBufferID != -1;
			if (flag2)
			{
				GL.DeleteBuffer(this.VertexBufferID);
			}
			CCSBoundingBox.ProgramRefs--;
			bool flag3 = CCSBoundingBox.ProgramRefs <= 0;
			if (flag3)
			{
				bool flag4 = CCSBoundingBox.ProgramID != -1;
				if (flag4)
				{
					GL.DeleteProgram(CCSBoundingBox.ProgramID);
				}
				CCSBoundingBox.ProgramID = -1;
			}
			return true;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.ModelID = bStream.ReadInt32();
			this.Box[0].Minimum = Util.ReadVec3Position(bStream);
			this.Box[0].Maximum = Util.ReadVec3Position(bStream);
			return true;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000C187 File Offset: 0x0000A387
		public override TreeNode ToNode()
		{
			return base.ToNode();
		}

		// Token: 0x04000131 RID: 305
		public int ModelID = 0;

		// Token: 0x04000132 RID: 306
		public CCSBoundingBox.BBox[] Box = new CCSBoundingBox.BBox[1];

		// Token: 0x04000133 RID: 307
		public static int ProgramID = -1;

		// Token: 0x04000134 RID: 308
		public static int ProgramRefs = 0;

		// Token: 0x04000135 RID: 309
		public static int AttribVMin = -1;

		// Token: 0x04000136 RID: 310
		public static int AttribVMax = -1;

		// Token: 0x04000137 RID: 311
		public static int AttribVColor = -1;

		// Token: 0x04000138 RID: 312
		public static int UniMatrix = -1;

		// Token: 0x04000139 RID: 313
		public int VertexArrayID = -1;

		// Token: 0x0400013A RID: 314
		public int VertexBufferID = -1;

		// Token: 0x0200005F RID: 95
		public struct BBox
		{
			// Token: 0x040002BD RID: 701
			public Vector3 Minimum;

			// Token: 0x040002BE RID: 702
			public Vector3 Maximum;

			// Token: 0x040002BF RID: 703
			public Vector4 Color;
		}
	}
}
