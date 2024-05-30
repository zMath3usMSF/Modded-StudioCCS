using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace StudioCCS
{
	// Token: 0x02000004 RID: 4
	public static class AxisMarker
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002774 File Offset: 0x00000974
		public static bool Init()
		{
			bool wasInit = AxisMarker.WasInit;
			bool result;
			if (wasInit)
			{
				result = true;
			}
			else
			{
				AxisMarker.ReadBin();
				AxisMarker.ProgramID = Scene.LoadProgram("AxisMarker", false);
				bool flag = AxisMarker.ProgramID == -1;
				if (flag)
				{
					Logger.LogError("Error loading AxisMarker shader program", Logger.LogType.LogAll, "Init", 53);
					result = false;
				}
				else
				{
					AxisMarker.AttribPosition = GL.GetAttribLocation(AxisMarker.ProgramID, "Position");
					AxisMarker.AttribColor = GL.GetAttribLocation(AxisMarker.ProgramID, "Color");
					AxisMarker.UniMatrix = GL.GetUniformLocation(AxisMarker.ProgramID, "Matrix");
					AxisMarker.UniScale = GL.GetUniformLocation(AxisMarker.ProgramID, "Scale");
					bool flag2 = AxisMarker.AttribPosition == -1 || AxisMarker.AttribColor == -1 || AxisMarker.UniMatrix == -1 || AxisMarker.UniScale == -1;
					if (flag2)
					{
						Logger.LogError("Error getting AxisVertex Shader Attribute/Uniform Locations:\n", Logger.LogType.LogAll, "Init", 64);
						Logger.LogError(string.Format("\tPosition: {0}, Color: {1}, Matrix: {2}, Scale: {3}\n", new object[]
						{
							AxisMarker.AttribPosition,
							AxisMarker.AttribColor,
							AxisMarker.UniMatrix,
							AxisMarker.UniScale
						}), Logger.LogType.LogAll, "Init", 65);
						result = false;
					}
					else
					{
						AxisMarker.ArrayID = GL.GenVertexArray();
						GL.BindVertexArray(AxisMarker.ArrayID);
						AxisMarker.BufferID = GL.GenBuffer();
						GL.BindBuffer(BufferTarget.ArrayBuffer, AxisMarker.BufferID);
						Type type = AxisMarker.Vertices[0].GetType();
						int stride = Marshal.SizeOf(type);
						GL.BufferData<AxisMarker.AxisVertex>(BufferTarget.ArrayBuffer, (IntPtr)(AxisMarker.VertexCount * stride), AxisMarker.Vertices, BufferUsageHint.StaticDraw);
						GL.EnableVertexAttribArray(AxisMarker.AttribPosition);
						GL.VertexAttribPointer(AxisMarker.AttribPosition, 3, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Position"));
						GL.EnableVertexAttribArray(AxisMarker.AttribColor);
						GL.VertexAttribPointer(AxisMarker.AttribColor, 4, VertexAttribPointerType.UnsignedByte, true, stride, Marshal.OffsetOf(type, "Color"));
						GL.BindVertexArray(0);
						GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
						AxisMarker.WasInit = true;
						result = AxisMarker.WasInit;
					}
				}
			}
			return result;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002998 File Offset: 0x00000B98
		private static void ReadBin()
		{
			using (FileStream input = new FileStream("data/bin/AxisMarker.bin", FileMode.Open))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					AxisMarker.VertexCount = binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					bool flag = AxisMarker.Vertices == null || AxisMarker.Vertices.Length != AxisMarker.VertexCount;
					if (flag)
					{
						AxisMarker.Vertices = new AxisMarker.AxisVertex[AxisMarker.VertexCount];
					}
					for (int index = 0; index < AxisMarker.VertexCount; index++)
					{
						float x = binaryReader.ReadSingle();
						float y = binaryReader.ReadSingle();
						float z = binaryReader.ReadSingle();
						uint num = binaryReader.ReadUInt32();
						AxisMarker.Vertices[index] = new AxisMarker.AxisVertex
						{
							Position = new Vector3(x, y, z),
							Color = num
						};
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002AAC File Offset: 0x00000CAC
		public static void Reload()
		{
			AxisMarker.ReadBin();
			GL.BindBuffer(BufferTarget.ArrayBuffer, AxisMarker.BufferID);
			int num = Marshal.SizeOf(AxisMarker.Vertices[0].GetType());
			GL.BufferData<AxisMarker.AxisVertex>(BufferTarget.ArrayBuffer, (IntPtr)(AxisMarker.VertexCount * num), AxisMarker.Vertices, BufferUsageHint.StaticDraw);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002B0C File Offset: 0x00000D0C
		public static void DeInit()
		{
			bool flag = AxisMarker.ProgramID != -1;
			if (flag)
			{
				GL.DeleteProgram(AxisMarker.ProgramID);
			}
			AxisMarker.ProgramID = -1;
			bool flag2 = AxisMarker.ArrayID != -1;
			if (flag2)
			{
				GL.DeleteVertexArray(AxisMarker.ArrayID);
			}
			AxisMarker.ArrayID = -1;
			bool flag3 = AxisMarker.BufferID != -1;
			if (flag3)
			{
				GL.DeleteBuffer(AxisMarker.BufferID);
			}
			AxisMarker.BufferID = -1;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002B7C File Offset: 0x00000D7C
		public static void Render(Matrix4 matrix, float scale)
		{
			bool flag = !AxisMarker.WasInit || AxisMarker.ProgramID == -1;
			if (!flag)
			{
				PolygonMode integer = (PolygonMode)GL.GetInteger(GetPName.PolygonMode);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.UseProgram(AxisMarker.ProgramID);
				GL.BindVertexArray(AxisMarker.ArrayID);
				GL.BindBuffer(BufferTarget.ArrayBuffer, AxisMarker.BufferID);
				GL.UniformMatrix4(AxisMarker.UniMatrix, false, ref matrix);
				GL.Uniform1(AxisMarker.UniScale, scale);
				GL.DrawArrays(PrimitiveType.Triangles, 0, AxisMarker.VertexCount);
				GL.BindVertexArray(0);
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				GL.UseProgram(0);
				GL.PolygonMode(MaterialFace.FrontAndBack, integer);
			}
		}

		// Token: 0x04000005 RID: 5
		private static AxisMarker.AxisVertex[] Vertices = null;

		// Token: 0x04000006 RID: 6
		private static int VertexCount = 0;

		// Token: 0x04000007 RID: 7
		private static bool WasInit = false;

		// Token: 0x04000008 RID: 8
		private static int ArrayID = -1;

		// Token: 0x04000009 RID: 9
		private static int BufferID = -1;

		// Token: 0x0400000A RID: 10
		private static int ProgramID = -1;

		// Token: 0x0400000B RID: 11
		private static int AttribPosition = -1;

		// Token: 0x0400000C RID: 12
		private static int AttribColor = -1;

		// Token: 0x0400000D RID: 13
		private static int UniMatrix = -1;

		// Token: 0x0400000E RID: 14
		private static int UniScale = -1;

		// Token: 0x02000032 RID: 50
		public struct AxisVertex
		{
			// Token: 0x04000214 RID: 532
			public Vector3 Position;

			// Token: 0x04000215 RID: 533
			public uint Color;
		}
	}
}
