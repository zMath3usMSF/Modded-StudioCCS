using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace StudioCCS
{
	// Token: 0x02000009 RID: 9
	public static class Grid
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00005878 File Offset: 0x00003A78
		public static bool Init()
		{
			bool wasInit = Grid.WasInit;
			bool result;
			if (wasInit)
			{
				result = true;
			}
			else
			{
				Grid.InitGridVerts(16);
				Grid.ProgramID = Scene.LoadProgram("Grid", false);
				bool flag = Grid.ProgramID == -1;
				if (flag)
				{
					Logger.LogError("Error loading Grid shader program", Logger.LogType.LogAll, "Init", 47);
					result = false;
				}
				else
				{
					Grid.AttribPosition = GL.GetAttribLocation(Grid.ProgramID, "Position");
					Grid.UniformMatrix = GL.GetUniformLocation(Grid.ProgramID, "Matrix");
					Grid.UniformColor = GL.GetUniformLocation(Grid.ProgramID, "Color");
					Grid.UniformScale = GL.GetUniformLocation(Grid.ProgramID, "Scale");
					bool flag2 = Grid.AttribPosition == -1 || Grid.UniformMatrix == -1 || Grid.UniformColor == -1 || Grid.UniformScale == -1;
					if (flag2)
					{
						Logger.LogError("Error getting Grid Shader Attribute/Uniform Locations:\n", Logger.LogType.LogAll, "Init", 58);
						Logger.LogError(string.Format("\tPosition: {0}, Matrix: {1}, Color: {2}, Scale: {3}", new object[]
						{
							Grid.AttribPosition,
							Grid.UniformMatrix,
							Grid.UniformColor,
							Grid.UniformScale
						}), Logger.LogType.LogAll, "Init", 59);
						result = false;
					}
					else
					{
						Grid.ArrayID = GL.GenVertexArray();
						GL.BindVertexArray(Grid.ArrayID);
						Grid.BufferID = GL.GenBuffer();
						Grid.UpdateGridVerts(false);
						int stride = Marshal.SizeOf(Grid.Vertices[0].GetType());
						GL.EnableVertexAttribArray(Grid.AttribPosition);
						GL.VertexAttribPointer(Grid.AttribPosition, 3, VertexAttribPointerType.Float, false, stride, IntPtr.Zero);
						GL.BindVertexArray(0);
						GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
						Grid.WasInit = true;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00005A40 File Offset: 0x00003C40
		private static void InitGridVerts(int _gridSize)
		{
			int num = _gridSize * 2 + 1;
			Grid.GridSize = _gridSize;
			Grid.VertexCount = num * 4;
			Grid.Vertices = new Vector3[Grid.VertexCount];
			float z = (float)(-(float)Grid.GridSize);
			float x = (float)(-(float)Grid.GridSize);
			int index = 0;
			for (int index2 = 0; index2 < num; index2++)
			{
				float z2 = z + (float)index2;
				Grid.Vertices[index] = new Vector3(x, 0f, z2);
				Grid.Vertices[index + 1] = new Vector3(-x, 0f, z2);
				index += 2;
			}
			for (int index3 = 0; index3 < num; index3++)
			{
				float x2 = x + (float)index3;
				Grid.Vertices[index] = new Vector3(x2, 0f, z);
				Grid.Vertices[index + 1] = new Vector3(x2, 0f, -z);
				index += 2;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00005B30 File Offset: 0x00003D30
		private static void UpdateGridVerts(bool unbindAfter = true)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, Grid.BufferID);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(Marshal.SizeOf(Grid.Vertices[0].GetType()) * Grid.VertexCount), Grid.Vertices, BufferUsageHint.StaticDraw);
			bool flag = !unbindAfter;
			if (!flag)
			{
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00005BA0 File Offset: 0x00003DA0
		public static void DeInit()
		{
			bool flag = Grid.ProgramID != -1;
			if (flag)
			{
				GL.DeleteProgram(Grid.ProgramID);
			}
			Grid.ProgramID = -1;
			bool flag2 = Grid.ArrayID != -1;
			if (flag2)
			{
				GL.DeleteVertexArray(Grid.ArrayID);
			}
			Grid.ArrayID = -1;
			bool flag3 = Grid.BufferID != -1;
			if (flag3)
			{
				GL.DeleteBuffer(Grid.BufferID);
			}
			Grid.BufferID = -1;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00005C10 File Offset: 0x00003E10
		public static void Render(Matrix4 matrix, float scale)
		{
			bool flag = !Grid.WasInit || Grid.ProgramID == -1;
			if (!flag)
			{
				GL.UseProgram(Grid.ProgramID);
				GL.BindVertexArray(Grid.ArrayID);
				GL.Uniform1(Grid.UniformScale, scale);
				GL.Uniform4(Grid.UniformColor, 0.5f, 0.5f, 0.5f, 1f);
				GL.UniformMatrix4(Grid.UniformMatrix, false, ref matrix);
				GL.DrawArrays(PrimitiveType.Lines, 0, Grid.VertexCount);
				GL.BindVertexArray(0);
				GL.UseProgram(0);
			}
		}

		// Token: 0x04000050 RID: 80
		public static bool WasInit = false;

		// Token: 0x04000051 RID: 81
		public static Vector3[] Vertices;

		// Token: 0x04000052 RID: 82
		public static int VertexCount = 0;

		// Token: 0x04000053 RID: 83
		public static int ProgramID = -1;

		// Token: 0x04000054 RID: 84
		public static int AttribPosition = -1;

		// Token: 0x04000055 RID: 85
		public static int UniformMatrix = -1;

		// Token: 0x04000056 RID: 86
		public static int UniformColor = -1;

		// Token: 0x04000057 RID: 87
		public static int UniformScale = -1;

		// Token: 0x04000058 RID: 88
		public static int ArrayID = -1;

		// Token: 0x04000059 RID: 89
		public static int BufferID = -1;

		// Token: 0x0400005A RID: 90
		public static int GridSize = 0;

		// Token: 0x0400005B RID: 91
		public static float GridSpacing = 1f;
	}
}
