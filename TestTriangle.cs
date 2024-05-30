using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace StudioCCS
{
	// Token: 0x02000011 RID: 17
	public class TestTriangle
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x0000B94C File Offset: 0x00009B4C
		public TestTriangle()
		{
			this.Vertices = new TestTriangle.TriangleVertex[3];
			this.Vertices[0].Position = new Vector3(0.374f, 2.792f, 0.186f);
			this.Vertices[0].Color = new Vector3(1f, 0f, 0f);
			this.Vertices[1].Position = new Vector3(0.28f, 2.792f, 0.186f);
			this.Vertices[1].Color = new Vector3(0f, 1f, 0f);
			this.Vertices[2].Position = new Vector3(0.28f, 2.792f, 0.411f);
			this.Vertices[2].Color = new Vector3(0f, 0f, 1f);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000BA6C File Offset: 0x00009C6C
		public bool Init()
		{
			this.ProgramID = Scene.LoadProgram("Triangle", false);
			bool flag = this.ProgramID != -1;
			if (flag)
			{
				this.AttribPos = GL.GetAttribLocation(this.ProgramID, "vPosition");
				this.AttribColor = GL.GetAttribLocation(this.ProgramID, "vColor");
				this.UniMatrix = GL.GetUniformLocation(this.ProgramID, "modelView");
				bool flag2 = this.AttribPos == -1 || this.AttribColor == -1 || this.UniMatrix == -1;
				if (flag2)
				{
					return false;
				}
				this.VAOIndex = GL.GenVertexArray();
				GL.BindVertexArray(this.VAOIndex);
				int stride = Marshal.SizeOf(this.Vertices[0].GetType());
				Type type = this.Vertices[0].GetType();
				this.VBOIndex = GL.GenBuffer();
				GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBOIndex);
				GL.BufferData<TestTriangle.TriangleVertex>(BufferTarget.ArrayBuffer, (IntPtr)(stride * 3), this.Vertices, BufferUsageHint.StaticDraw);
				GL.EnableVertexAttribArray(this.AttribPos);
				GL.VertexAttribPointer(this.AttribPos, 3, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Position"));
				GL.EnableVertexAttribArray(this.AttribColor);
				GL.VertexAttribPointer(this.AttribColor, 3, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Color"));
				GL.BindVertexArray(0);
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				this.WasInit = true;
			}
			return this.WasInit;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000BC0C File Offset: 0x00009E0C
		public void DeInit()
		{
			bool flag = this.ProgramID != -1;
			if (flag)
			{
				GL.DeleteProgram(this.ProgramID);
			}
			this.ProgramID = -1;
			bool flag2 = this.VBOIndex != -1;
			if (flag2)
			{
				GL.DeleteBuffer(this.VBOIndex);
			}
			this.VBOIndex = -1;
			bool flag3 = this.VAOIndex != -1;
			if (flag3)
			{
				GL.DeleteVertexArray(this.VAOIndex);
			}
			this.VAOIndex = -1;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000BC84 File Offset: 0x00009E84
		public void Render(Matrix4 ViewMtx)
		{
			bool flag = !this.WasInit;
			if (!flag)
			{
				Matrix4 matrix = Matrix4.CreateTranslation(-this.Position) * ViewMtx;
				GL.UseProgram(this.ProgramID);
				GL.UniformMatrix4(this.UniMatrix, false, ref matrix);
				GL.BindVertexArray(this.VAOIndex);
				GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBOIndex);
				GL.EnableVertexAttribArray(this.AttribPos);
				GL.EnableVertexAttribArray(this.AttribColor);
				GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
				GL.DisableVertexAttribArray(this.AttribPos);
				GL.DisableVertexAttribArray(this.AttribColor);
				GL.BindVertexArray(0);
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				GL.UseProgram(0);
			}
		}

		// Token: 0x04000109 RID: 265
		private int ProgramID;

		// Token: 0x0400010A RID: 266
		private int AttribPos;

		// Token: 0x0400010B RID: 267
		private int AttribColor;

		// Token: 0x0400010C RID: 268
		private int UniMatrix;

		// Token: 0x0400010D RID: 269
		private int VAOIndex = -1;

		// Token: 0x0400010E RID: 270
		private int VBOIndex = -1;

		// Token: 0x0400010F RID: 271
		public Vector3 Position = default(Vector3);

		// Token: 0x04000110 RID: 272
		public bool WasInit = false;

		// Token: 0x04000111 RID: 273
		private TestTriangle.TriangleVertex[] Vertices;

		// Token: 0x02000041 RID: 65
		public struct TriangleVertex
		{
			// Token: 0x0400025C RID: 604
			public Vector3 Position;

			// Token: 0x0400025D RID: 605
			public Vector3 Color;
		}
	}
}
