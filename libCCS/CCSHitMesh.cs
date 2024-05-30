using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace StudioCCS.libCCS
{
	// Token: 0x02000021 RID: 33
	public class CCSHitMesh : CCSBaseObject
	{
		// Token: 0x06000117 RID: 279 RVA: 0x000102BC File Offset: 0x0000E4BC
		public CCSHitMesh(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 2816;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000102FC File Offset: 0x0000E4FC
		public override bool Init()
		{
			bool flag = CCSHitMesh.ProgramID == -1;
			if (flag)
			{
				CCSHitMesh.ProgramID = Scene.LoadProgram("CCSHitMesh", false);
				bool flag2 = CCSHitMesh.ProgramID == -1;
				if (flag2)
				{
					return false;
				}
				CCSHitMesh.AttribPosition = GL.GetAttribLocation(CCSHitMesh.ProgramID, "vPosition");
				CCSHitMesh.UniformMatrix = GL.GetUniformLocation(CCSHitMesh.ProgramID, "mMatrix");
				CCSHitMesh.UniformColor = GL.GetUniformLocation(CCSHitMesh.ProgramID, "vColor");
				bool flag3 = CCSHitMesh.AttribPosition == -1 || CCSHitMesh.UniformMatrix == -1 || CCSHitMesh.UniformColor == -1;
				if (flag3)
				{
					Logger.LogError("CCSHitMesh: Error Getting Shader Attributes/Uniforms:\n", Logger.LogType.LogAll, "Init", 77);
					Logger.LogError(string.Format("\tPosition: {0}, Matrix: {1}, Color: {2}\n", CCSHitMesh.AttribPosition, CCSHitMesh.UniformMatrix, CCSHitMesh.UniformColor), Logger.LogType.LogAll, "Init", 78);
					return false;
				}
			}
			CCSHitMesh.ProgramRefs++;
			for (int index = 0; index < this.HitGroupCount; index++)
			{
				CCSHitMesh.HitGroup hitGroup = this.HitGroups[index];
				bool flag4 = hitGroup.VertexCount > 0;
				if (flag4)
				{
					hitGroup.VertexArrayID = GL.GenVertexArray();
					GL.BindVertexArray(hitGroup.VertexArrayID);
					int stride = Marshal.SizeOf(hitGroup.Vertices[0].GetType());
					hitGroup.VertexBufferID = GL.GenBuffer();
					GL.BindBuffer(BufferTarget.ArrayBuffer, hitGroup.VertexBufferID);
					GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(hitGroup.VertexCount * stride), hitGroup.Vertices, BufferUsageHint.StaticDraw);
					GL.EnableVertexAttribArray(CCSHitMesh.AttribPosition);
					GL.VertexAttribPointer(CCSHitMesh.AttribPosition, 3, VertexAttribPointerType.Float, false, stride, IntPtr.Zero);
					GL.BindVertexArray(0);
					GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				}
				else
				{
					Logger.LogWarning(string.Format("HitMesh {0}: HitGroup{1} has zero vertices.\n", this.ParentFile.GetSubObjectName(this.ObjectID), index), Logger.LogType.LogAll, "Init", 110);
				}
			}
			return true;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00010524 File Offset: 0x0000E724
		public void RenderAll(Matrix4 _matrix, int _selectedID = -1)
		{
			bool flag = CCSHitMesh.ProgramID == -1;
			if (!flag)
			{
				Matrix4 matrix = Matrix4.CreateTranslation(0f, 0f, 0f) * _matrix;
				GL.UseProgram(CCSHitMesh.ProgramID);
				Console.WriteLine("Wtf 1");
				for (int index = 0; index < this.HitGroupCount; index++)
				{
					try
					{
						CCSHitMesh.HitGroup hitGroup = this.HitGroups[index];
						Console.WriteLine("Wtf 1");
						bool flag2 = hitGroup == null;
						if (flag2)
						{
							return;
						}
						Console.WriteLine("Wtf 1");
						GL.BindVertexArray(hitGroup.VertexArrayID);
						GL.UniformMatrix4(CCSHitMesh.UniformMatrix, false, ref matrix);
						PolygonMode integer = (PolygonMode)GL.GetInteger(GetPName.PolygonMode);
						bool flag3 = integer == PolygonMode.Fill;
						if (flag3)
						{
							GL.Uniform4(CCSHitMesh.UniformColor, hitGroup.Color);
							GL.DrawArrays(PrimitiveType.Triangles, 0, hitGroup.VertexCount);
						}
						GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
						bool flag4 = index == _selectedID;
						if (flag4)
						{
							GL.Uniform4(CCSHitMesh.UniformColor, 1f, 1f, 1f, 1f);
						}
						else
						{
							GL.Uniform4(CCSHitMesh.UniformColor, 0f, 0f, 0f, 1f);
						}
						GL.DrawArrays(PrimitiveType.Triangles, 0, hitGroup.VertexCount);
						GL.PolygonMode(MaterialFace.FrontAndBack, integer);
						GL.BindVertexArray(0);
					}
					catch (NullReferenceException)
					{
						Logger.LogError(string.Format("Null Reference expetion caught when attempting render of Hitmesh {0} Submesh {1}...\n", this.ParentFile.GetSubObjectName(this.ObjectID), index), Logger.LogType.LogOnceValue, "RenderAll", 156);
					}
				}
				GL.UseProgram(0);
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000106F4 File Offset: 0x0000E8F4
		public void RenderOne(Matrix4 _matrix, int groupID)
		{
			bool flag = CCSHitMesh.ProgramID == -1 || groupID < 0 || groupID > this.HitGroupCount - 1;
			if (!flag)
			{
				CCSHitMesh.HitGroup hitGroup = this.HitGroups[groupID];
				GL.UseProgram(CCSHitMesh.ProgramID);
				GL.BindVertexArray(hitGroup.VertexArrayID);
				GL.UniformMatrix4(CCSHitMesh.UniformMatrix, false, ref _matrix);
				GL.Uniform4(CCSHitMesh.UniformColor, hitGroup.Color);
				PolygonMode integer = (PolygonMode)GL.GetInteger(GetPName.PolygonMode);
				bool flag2 = integer == PolygonMode.Fill;
				if (flag2)
				{
					GL.DrawArrays(PrimitiveType.Triangles, 0, hitGroup.VertexCount);
				}
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				GL.Uniform4(CCSHitMesh.UniformColor, 1f, 1f, 1f, 1f);
				GL.DrawArrays(PrimitiveType.Triangles, 0, hitGroup.VertexCount);
				GL.PolygonMode(MaterialFace.FrontAndBack, integer);
				GL.BindVertexArray(0);
				GL.UseProgram(0);
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000107E0 File Offset: 0x0000E9E0
		public override bool DeInit()
		{
			for (int index = 0; index < this.HitGroupCount; index++)
			{
				CCSHitMesh.HitGroup hitGroup = this.HitGroups[index];
				bool flag = hitGroup.VertexArrayID != -1;
				if (flag)
				{
					GL.DeleteVertexArray(hitGroup.VertexArrayID);
				}
				bool flag2 = hitGroup.VertexBufferID != -1;
				if (flag2)
				{
					GL.DeleteBuffer(hitGroup.VertexBufferID);
				}
			}
			CCSHitMesh.ProgramRefs--;
			bool flag3 = CCSHitMesh.ProgramRefs <= 0 && CCSHitMesh.ProgramID != -1;
			if (flag3)
			{
				GL.DeleteProgram(CCSHitMesh.ProgramID);
				CCSHitMesh.ProgramID = -1;
			}
			return true;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0001088C File Offset: 0x0000EA8C
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.UnkID = bStream.ReadInt32();
			this.HitGroupCount = bStream.ReadInt32();
			this.HitGroups = new CCSHitMesh.HitGroup[this.HitGroupCount];
			this.VertexCount = bStream.ReadInt32();
			for (int index = 0; index < this.HitGroupCount; index++)
			{
				CCSHitMesh.HitGroup hitGroup = new CCSHitMesh.HitGroup
				{
					VertexCount = bStream.ReadInt32() * 2,
					Color = Util.ReadVec4RGBA32(bStream)
				};
				hitGroup.Vertices = new Vector3[hitGroup.VertexCount];
				for (int index2 = 0; index2 < hitGroup.VertexCount; index2++)
				{
					Vector3 vector3 = Util.ReadVec3Position(bStream);
					hitGroup.Vertices[index2] = vector3;
					bool flag = Util.Vector3LessThan(vector3, hitGroup.Minimum);
					if (flag)
					{
						hitGroup.Minimum = vector3;
					}
					else
					{
						bool flag2 = Util.Vector3LessThan(hitGroup.Maximum, vector3);
						if (flag2)
						{
							hitGroup.Maximum = vector3;
						}
					}
				}
				this.HitGroups[index] = hitGroup;
			}
			return true;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00010990 File Offset: 0x0000EB90
		public override TreeNode ToNode()
		{
			TreeNode node = base.ToNode();
			for (int _subID = 0; _subID < this.HitGroupCount; _subID++)
			{
				TreeNode node2 = new TreeNode(string.Format("HitGroup {0}", _subID))
				{
					Tag = new TreeNodeTag(this.ParentFile, this.ObjectID, this.ObjectType, TreeNodeTag.NodeType.SubNode, _subID)
				};
				node.Nodes.Add(node2);
			}
			return node;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00010A08 File Offset: 0x0000EC08
		public int DumpToObj(StreamWriter fStream, int vOffset, bool split = false)
		{
			int num = vOffset;
			bool flag = this.HitGroupCount > 0;
			if (flag)
			{
				string subObjectName = this.ParentFile.GetSubObjectName(this.ObjectID);
				fStream.WriteLine(string.Format("# {0}, {1} Vertices, {2} triangles", subObjectName, this.VertexCount, this.VertexCount / 3));
				fStream.WriteLine(string.Format("o {0}", subObjectName));
				for (int index = 0; index < this.HitGroupCount; index++)
				{
					CCSHitMesh.HitGroup hitGroup = this.HitGroups[index];
					if (split)
					{
						fStream.WriteLine(string.Format("g {0}_{1}", subObjectName, index));
					}
					for (int index2 = 0; index2 < hitGroup.VertexCount; index2++)
					{
						Vector3 vertex = hitGroup.Vertices[index2];
						fStream.WriteLine(string.Format("v\t{0}\t{1}\t{2}", vertex.X, vertex.Y, vertex.Z));
					}
					for (int index3 = 0; index3 < hitGroup.VertexCount / 3; index3++)
					{
						int num2 = index3 * 3;
						fStream.WriteLine(string.Format("f {0}\t{1}\t{2}", num2 + num, num2 + 1 + num, num2 + 2 + num));
					}
					num += hitGroup.VertexCount;
				}
			}
			return num;
		}

		// Token: 0x04000197 RID: 407
		public CCSHitMesh.HitGroup[] HitGroups = null;

		// Token: 0x04000198 RID: 408
		public int HitGroupCount = 0;

		// Token: 0x04000199 RID: 409
		public int UnkID = 0;

		// Token: 0x0400019A RID: 410
		public int VertexCount = 0;

		// Token: 0x0400019B RID: 411
		public static int ProgramID = -1;

		// Token: 0x0400019C RID: 412
		public static int ProgramRefs = 0;

		// Token: 0x0400019D RID: 413
		public static int AttribPosition = -1;

		// Token: 0x0400019E RID: 414
		public static int UniformMatrix = -1;

		// Token: 0x0400019F RID: 415
		public static int UniformColor = -1;

		// Token: 0x040001A0 RID: 416
		public static int UniformDisplayColor = -1;

		// Token: 0x040001A1 RID: 417
		public static int DerpArrayID = -1;

		// Token: 0x02000061 RID: 97
		public class HitGroup
		{
			// Token: 0x040002C2 RID: 706
			public Vector3[] Vertices = null;

			// Token: 0x040002C3 RID: 707
			public int VertexCount = 0;

			// Token: 0x040002C4 RID: 708
			public Vector4 Color = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);

			// Token: 0x040002C5 RID: 709
			public Vector3 Minimum = new Vector3(0f);

			// Token: 0x040002C6 RID: 710
			public Vector3 Maximum = new Vector3(0f);

			// Token: 0x040002C7 RID: 711
			public int VertexArrayID = -1;

			// Token: 0x040002C8 RID: 712
			public int VertexBufferID = -1;
		}
	}
}
