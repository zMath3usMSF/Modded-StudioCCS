using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace StudioCCS
{
	// Token: 0x0200000A RID: 10
	public static class TexturePreview
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public static bool Init()
		{
			bool wasInit = TexturePreview.WasInit;
			bool result;
			if (wasInit)
			{
				result = true;
			}
			else
			{
				TexturePreview.Vertices = new TexturePreview.TextureVertex[4];
				TexturePreview.TextureVertex[] vertices = TexturePreview.Vertices;
				TexturePreview.TextureVertex textureVertex2 = new TexturePreview.TextureVertex
				{
					Position = new Vector3(-0.5f, -0.5f, 0f),
					TexCoord = new Vector2(0f, 0f)
				};
				vertices[0] = textureVertex2;
				TexturePreview.TextureVertex[] vertices2 = TexturePreview.Vertices;
				TexturePreview.TextureVertex textureVertex3 = new TexturePreview.TextureVertex
				{
					Position = new Vector3(-0.5f, 0.5f, 0f),
					TexCoord = new Vector2(0f, 1f)
				};
				vertices2[1] = textureVertex3;
				TexturePreview.TextureVertex[] vertices3 = TexturePreview.Vertices;
				TexturePreview.TextureVertex textureVertex4 = new TexturePreview.TextureVertex
				{
					Position = new Vector3(0.5f, -0.5f, 0f),
					TexCoord = new Vector2(1f, 0f)
				};
				vertices3[2] = textureVertex4;
				TexturePreview.TextureVertex[] vertices4 = TexturePreview.Vertices;
				TexturePreview.TextureVertex textureVertex5 = new TexturePreview.TextureVertex
				{
					Position = new Vector3(0.5f, 0.5f, 0f),
					TexCoord = new Vector2(1f, 1f)
				};
				vertices4[3] = textureVertex5;
				TexturePreview.ProgramID = Scene.LoadProgram("TexturePreview", false);
				bool flag = TexturePreview.ProgramID == -1;
				if (flag)
				{
					Logger.LogError("Error loading Texture Preview Shader Program", Logger.LogType.LogAll, "Init", 82);
					result = false;
				}
				else
				{
					TexturePreview.AttribPosition = GL.GetAttribLocation(TexturePreview.ProgramID, "vPosition");
					TexturePreview.AttribTexCoord = GL.GetAttribLocation(TexturePreview.ProgramID, "vTexCoord");
					TexturePreview.UniformMatrix = GL.GetUniformLocation(TexturePreview.ProgramID, "mMatrix");
					TexturePreview.UniformTexture = GL.GetUniformLocation(TexturePreview.ProgramID, "fTexture");
					TexturePreview.UniformTextureSize = GL.GetUniformLocation(TexturePreview.ProgramID, "mSize");
					bool flag2 = TexturePreview.AttribPosition == -1 || TexturePreview.AttribTexCoord == -1;
					if (flag2)
					{
						Logger.LogError("Error getting Texture Preview Shader Attributes:", Logger.LogType.LogAll, "Init", 94);
						Logger.LogError(string.Format("\tPosition: {0}, TexCoord: {1}", TexturePreview.AttribPosition, TexturePreview.AttribTexCoord), Logger.LogType.LogAll, "Init", 95);
						result = false;
					}
					else
					{
						bool flag3 = TexturePreview.UniformMatrix == -1 || TexturePreview.UniformTexture == -1 || TexturePreview.UniformTextureSize == -1;
						if (flag3)
						{
							Logger.LogError("Error getting Texture Preview Shader Uniform Locations:", Logger.LogType.LogAll, "Init", 101);
							Logger.LogError(string.Format("\tMatrix: {0}, Texture: {1}, Size: {2}", TexturePreview.UniformMatrix, TexturePreview.UniformTexture, TexturePreview.UniformTextureSize), Logger.LogType.LogAll, "Init", 102);
							result = false;
						}
						else
						{
							TexturePreview.ArrayID = GL.GenVertexArray();
							GL.BindVertexArray(TexturePreview.ArrayID);
							TexturePreview.BufferID = GL.GenBuffer();
							GL.BindBuffer(BufferTarget.ArrayBuffer, TexturePreview.BufferID);
							Type type = TexturePreview.Vertices[0].GetType();
							int stride = Marshal.SizeOf(type);
							GL.BufferData<TexturePreview.TextureVertex>(BufferTarget.ArrayBuffer, (IntPtr)(stride * TexturePreview.VertexCount), TexturePreview.Vertices, BufferUsageHint.StaticDraw);
							GL.EnableVertexAttribArray(TexturePreview.AttribPosition);
							GL.VertexAttribPointer(TexturePreview.AttribPosition, 3, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Position"));
							GL.EnableVertexAttribArray(TexturePreview.AttribTexCoord);
							GL.VertexAttribPointer(TexturePreview.AttribTexCoord, 2, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "TexCoord"));
							GL.BindVertexArray(0);
							GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
							TexturePreview.WasInit = true;
							result = TexturePreview.WasInit;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000060AC File Offset: 0x000042AC
		public static void DeInit()
		{
			bool flag = TexturePreview.ProgramID != -1;
			if (flag)
			{
				GL.DeleteProgram(TexturePreview.ProgramID);
			}
			TexturePreview.ProgramID = -1;
			bool flag2 = TexturePreview.ArrayID != -1;
			if (flag2)
			{
				GL.DeleteVertexArray(TexturePreview.ArrayID);
			}
			TexturePreview.ArrayID = -1;
			bool flag3 = TexturePreview.BufferID != -1;
			if (flag3)
			{
				GL.DeleteBuffer(TexturePreview.BufferID);
			}
			TexturePreview.BufferID = -1;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000611C File Offset: 0x0000431C
		public static void Render(Matrix4 matrix, int textureID, float textureWidth, float textureHeight)
		{
			bool flag = !TexturePreview.WasInit || TexturePreview.ProgramID == -1;
			if (!flag)
			{
				PolygonMode integer = (PolygonMode)GL.GetInteger(GetPName.PolygonMode);
				GL.UseProgram(TexturePreview.ProgramID);
				GL.BindVertexArray(TexturePreview.ArrayID);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.Uniform1(TexturePreview.UniformTexture, 1);
				GL.Uniform3(TexturePreview.UniformTextureSize, textureWidth, textureHeight, 0.1f);
				Matrix4 identity = Matrix4.Identity;
				GL.UniformMatrix4(TexturePreview.UniformMatrix, false, ref matrix);
				GL.ActiveTexture(TextureUnit.Texture1);
				GL.BindTexture(TextureTarget.Texture2D, textureID);
				GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
				GL.BindVertexArray(0);
				GL.UseProgram(0);
				GL.PolygonMode(MaterialFace.FrontAndBack, integer);
			}
		}

		// Token: 0x0400005C RID: 92
		public static bool WasInit = false;

		// Token: 0x0400005D RID: 93
		public static int VertexCount = 4;

		// Token: 0x0400005E RID: 94
		public static int ProgramID = -1;

		// Token: 0x0400005F RID: 95
		public static int AttribPosition = -1;

		// Token: 0x04000060 RID: 96
		public static int AttribTexCoord = -1;

		// Token: 0x04000061 RID: 97
		public static int UniformMatrix = -1;

		// Token: 0x04000062 RID: 98
		public static int UniformTexture = -1;

		// Token: 0x04000063 RID: 99
		public static int UniformTextureSize = -1;

		// Token: 0x04000064 RID: 100
		public static float Scale = 0.01f;

		// Token: 0x04000065 RID: 101
		public static int ArrayID = -1;

		// Token: 0x04000066 RID: 102
		public static int BufferID = -1;

		// Token: 0x04000067 RID: 103
		public static TexturePreview.TextureVertex[] Vertices = null;

		// Token: 0x02000034 RID: 52
		public struct TextureVertex
		{
			// Token: 0x04000217 RID: 535
			public Vector3 Position;

			// Token: 0x04000218 RID: 536
			public Vector2 TexCoord;
		}
	}
}
