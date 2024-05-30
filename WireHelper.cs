using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using StudioCCS.libCCS;

namespace StudioCCS
{
	// Token: 0x0200000C RID: 12
	public static class WireHelper
	{
		// Token: 0x06000039 RID: 57 RVA: 0x000062DC File Offset: 0x000044DC
		public static bool Init()
		{
			bool wasInit = WireHelper.WasInit;
			bool result;
			if (wasInit)
			{
				result = true;
			}
			else
			{
				WireHelper.ReadBin();
				WireHelper.ProgramID = Scene.LoadProgram("WireHelper", false);
				bool flag = WireHelper.ProgramID == -1;
				if (flag)
				{
					Logger.LogError("Error loading LightHelper shader program", Logger.LogType.LogAll, "Init", 56);
					result = false;
				}
				else
				{
					WireHelper.AttribPosition = GL.GetAttribLocation(WireHelper.ProgramID, "Position");
					WireHelper.UniformMatrix = GL.GetUniformLocation(WireHelper.ProgramID, "Matrix");
					WireHelper.UniformColor = GL.GetUniformLocation(WireHelper.ProgramID, "Color");
					WireHelper.UniformScale = GL.GetUniformLocation(WireHelper.ProgramID, "Scale");
					bool flag2 = WireHelper.AttribPosition == -1 || WireHelper.UniformMatrix == -1 || WireHelper.UniformColor == -1 || WireHelper.UniformScale == -1;
					if (flag2)
					{
						Logger.LogError("Error getting LightHelper Shader Attribute/Uniform Locations:\n", Logger.LogType.LogAll, "Init", 67);
						Logger.LogError(string.Format("\tPosition: {0}, Matrix: {1}, Color: {2}, Scale: {3}\n", new object[]
						{
							WireHelper.AttribPosition,
							WireHelper.UniformMatrix,
							WireHelper.UniformColor,
							WireHelper.UniformScale
						}), Logger.LogType.LogAll, "Init", 68);
						result = false;
					}
					else
					{
						WireHelper.ArrayID = GL.GenVertexArray();
						GL.BindVertexArray(WireHelper.ArrayID);
						WireHelper.BufferID = GL.GenBuffer();
						GL.BindBuffer(BufferTarget.ArrayBuffer, WireHelper.BufferID);
						int stride = Marshal.SizeOf(WireHelper.Vertices[0].GetType());
						GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(WireHelper.VertexCount * stride), WireHelper.Vertices, BufferUsageHint.StaticDraw);
						GL.EnableVertexAttribArray(WireHelper.AttribPosition);
						GL.VertexAttribPointer(WireHelper.AttribPosition, 3, VertexAttribPointerType.Float, false, stride, IntPtr.Zero);
						GL.BindVertexArray(0);
						GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
						WireHelper.WasInit = true;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000064CC File Offset: 0x000046CC
		public static void DeInit()
		{
			bool flag = WireHelper.ProgramID != -1;
			if (flag)
			{
				GL.DeleteProgram(WireHelper.ProgramID);
			}
			WireHelper.ProgramID = -1;
			bool flag2 = WireHelper.ArrayID != -1;
			if (flag2)
			{
				GL.DeleteVertexArray(WireHelper.ArrayID);
			}
			WireHelper.ArrayID = -1;
			bool flag3 = WireHelper.BufferID != -1;
			if (flag3)
			{
				GL.DeleteBuffer(WireHelper.BufferID);
			}
			WireHelper.BufferID = -1;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000653C File Offset: 0x0000473C
		public static void RenderDirectionalHelper(Matrix4 matrix)
		{
			bool flag = !WireHelper.WasInit || WireHelper.ProgramID == -1;
			if (!flag)
			{
				GL.UseProgram(WireHelper.ProgramID);
				GL.BindVertexArray(WireHelper.ArrayID);
				GL.BindBuffer(BufferTarget.ArrayBuffer, WireHelper.BufferID);
				GL.Uniform1(WireHelper.UniformScale, 1f);
				GL.Uniform4(WireHelper.UniformColor, 1f, 1f, 0f, 1f);
				GL.UniformMatrix4(WireHelper.UniformMatrix, false, ref matrix);
				GL.DrawArrays(PrimitiveType.Lines, WireHelper.DirectionalHelperOffset, WireHelper.DirectionalHelperCount);
				GL.BindVertexArray(0);
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				GL.UseProgram(0);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000065F4 File Offset: 0x000047F4
		public static void RenderOmniHelper(Matrix4 matrix, float scale)
		{
			bool flag = !WireHelper.WasInit || WireHelper.ProgramID == -1;
			if (!flag)
			{
				GL.UseProgram(WireHelper.ProgramID);
				GL.BindVertexArray(WireHelper.ArrayID);
				GL.Uniform1(WireHelper.UniformScale, 1f);
				GL.Uniform4(WireHelper.UniformColor, 1f, 1f, 0f, 1f);
				GL.UniformMatrix4(WireHelper.UniformMatrix, false, ref matrix);
				GL.DrawArrays(PrimitiveType.Lines, WireHelper.OmniHelperOffset, WireHelper.OmniHelperCount);
				GL.Uniform1(WireHelper.UniformScale, scale);
				GL.DrawArrays(PrimitiveType.Lines, WireHelper.OmniRingHelperOffset, WireHelper.OmniRingHelperCount);
				GL.BindVertexArray(0);
				GL.UseProgram(0);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000066AC File Offset: 0x000048AC
		public static void RenderDummyHelper(Matrix4 ProjView, CCSDummy Dummy)
		{
			bool flag = !WireHelper.WasInit || WireHelper.ProgramID == -1;
			if (!flag)
			{
				GL.UseProgram(WireHelper.ProgramID);
				GL.BindVertexArray(WireHelper.ArrayID);
				GL.Uniform1(WireHelper.UniformScale, 0.5f);
				GL.Uniform4(WireHelper.UniformColor, 0f, 1f, 0f, 1f);
				Matrix4 matrix = Dummy.Matrix() * ProjView;
				GL.UniformMatrix4(WireHelper.UniformMatrix, false, ref matrix);
				GL.DrawArrays(PrimitiveType.Lines, WireHelper.DummyHelperOffset, WireHelper.DummyHelperCount);
				GL.BindVertexArray(0);
				GL.UseProgram(0);
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00006754 File Offset: 0x00004954
		public static void ReadBin()
		{
			using (FileStream input = new FileStream("data/bin/WireHelpers.bin", FileMode.Open))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					WireHelper.VertexCount = binaryReader.ReadInt32();
					WireHelper.DirectionalHelperOffset = 0;
					WireHelper.DirectionalHelperCount = binaryReader.ReadInt32();
					WireHelper.OmniHelperOffset = WireHelper.DirectionalHelperCount;
					WireHelper.OmniHelperCount = binaryReader.ReadInt32();
					WireHelper.OmniRingHelperOffset = WireHelper.OmniHelperOffset + WireHelper.OmniHelperCount;
					WireHelper.OmniRingHelperCount = binaryReader.ReadInt32();
					WireHelper.DummyHelperOffset = WireHelper.OmniRingHelperOffset + WireHelper.OmniRingHelperCount;
					WireHelper.DummyHelperCount = binaryReader.ReadInt32();
					bool flag = WireHelper.Vertices == null || WireHelper.Vertices.Length != WireHelper.VertexCount;
					if (flag)
					{
						WireHelper.Vertices = new Vector3[WireHelper.VertexCount];
					}
					for (int index = 0; index < WireHelper.VertexCount; index++)
					{
						float x = binaryReader.ReadSingle();
						float y = binaryReader.ReadSingle();
						float z = binaryReader.ReadSingle();
						WireHelper.Vertices[index] = new Vector3(x, y, z);
					}
				}
			}
		}

		// Token: 0x04000068 RID: 104
		public static bool WasInit = false;

		// Token: 0x04000069 RID: 105
		public static Vector3[] Vertices = null;

		// Token: 0x0400006A RID: 106
		public static int VertexCount = 0;

		// Token: 0x0400006B RID: 107
		public static int ProgramID = -1;

		// Token: 0x0400006C RID: 108
		public static int AttribPosition = -1;

		// Token: 0x0400006D RID: 109
		public static int UniformMatrix = -1;

		// Token: 0x0400006E RID: 110
		public static int UniformColor = -1;

		// Token: 0x0400006F RID: 111
		public static int UniformScale = -1;

		// Token: 0x04000070 RID: 112
		public static int ArrayID = -1;

		// Token: 0x04000071 RID: 113
		public static int BufferID = -1;

		// Token: 0x04000072 RID: 114
		public static int DirectionalHelperOffset = 0;

		// Token: 0x04000073 RID: 115
		public static int DirectionalHelperCount = 0;

		// Token: 0x04000074 RID: 116
		public static int OmniHelperOffset = 0;

		// Token: 0x04000075 RID: 117
		public static int OmniHelperCount = 0;

		// Token: 0x04000076 RID: 118
		public static int OmniRingHelperOffset = 0;

		// Token: 0x04000077 RID: 119
		public static int OmniRingHelperCount = 0;

		// Token: 0x04000078 RID: 120
		public static int DummyHelperOffset = 0;

		// Token: 0x04000079 RID: 121
		public static int DummyHelperCount = 0;
	}
}
