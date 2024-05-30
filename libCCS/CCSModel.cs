using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace StudioCCS.libCCS
{
	// Token: 0x02000025 RID: 37
	public class CCSModel : CCSBaseObject
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00010F32 File Offset: 0x0000F132
		public CCSModel(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 2048;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00010F60 File Offset: 0x0000F160
		public override bool Init()
		{
			this.LoadShaders();
			CCSModel.ProgramRefs++;
			bool flag = (this.ModelType & 8) == 0;
			if (flag)
			{
				for (int index = 0; index < (int)this.SubModelCount; index++)
				{
					CCSModel.SubModel subModel = this.SubModels[index];
					int subObjectType = this.ParentFile.GetSubObjectType(subModel.MatTexID);
					int num = subObjectType;
					if (num != 512)
					{
						if (num == 768)
						{
							subModel.ParentMaterialRef = null;
							subModel.ParentTextureRef = this.ParentFile.GetObject<CCSTexture>(subModel.MatTexID);
						}
					}
					else
					{
						subModel.ParentMaterialRef = this.ParentFile.GetObject<CCSMaterial>(subModel.MatTexID);
						bool flag2 = subModel.ParentMaterialRef != null;
						if (flag2)
						{
							subModel.ParentTextureRef = this.ParentFile.GetObject<CCSTexture>(subModel.ParentMaterialRef.TextureID);
						}
					}
					bool flag3 = this.ModelType != 4 && this.ModelType != 4100 && this.ModelType != 14340;
					if (flag3)
					{
						subModel.ParentObjectRef = this.ParentFile.GetObject<CCSObject>(subModel.ParentID);
					}
					else
					{
						bool flag4 = this.ModelType == 4 || this.ModelType == 4100 || this.ModelType == 14340;
						if (flag4)
						{
							subModel.ParentObjectRef = this.ClumpRef.GetObject(subModel.ParentID);
							bool flag5 = subModel.ParentObjectRef == null;
							if (flag5)
							{
								subModel.ParentObjectRef = this.ObjectRef;
							}
						}
					}
					subModel.VertexArrayID = GL.GenVertexArray();
					GL.BindVertexArray(subModel.VertexArrayID);
					subModel.VertexBufferID = GL.GenBuffer();
					Type type = subModel.Vertices[0].GetType();
					int stride = Marshal.SizeOf(type);
					GL.BindBuffer(BufferTarget.ArrayBuffer, subModel.VertexBufferID);
					GL.BufferData<CCSModel.ModelVertex>(BufferTarget.ArrayBuffer, subModel.VertexCount * stride, subModel.Vertices, BufferUsageHint.StaticDraw);
					bool flag6 = CCSModel.AttribPosition != -1;
					if (flag6)
					{
						GL.EnableVertexAttribArray(CCSModel.AttribPosition);
						GL.VertexAttribPointer(CCSModel.AttribPosition, 3, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Position"));
					}
					bool flag7 = CCSModel.AttribPosition1 != -1;
					if (flag7)
					{
						GL.EnableVertexAttribArray(CCSModel.AttribPosition1);
						GL.VertexAttribPointer(CCSModel.AttribPosition1, 3, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Position2"));
					}
					bool flag8 = CCSModel.AttribPosition2 != -1;
					if (flag8)
					{
						GL.EnableVertexAttribArray(CCSModel.AttribPosition2);
						GL.VertexAttribPointer(CCSModel.AttribPosition2, 3, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Position3"));
					}
					bool flag9 = CCSModel.AttribPosition3 != -1;
					if (flag9)
					{
						GL.EnableVertexAttribArray(CCSModel.AttribPosition3);
						GL.VertexAttribPointer(CCSModel.AttribPosition3, 3, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Position4"));
					}
					GL.EnableVertexAttribArray(CCSModel.AttribTexCoord);
					GL.VertexAttribPointer(CCSModel.AttribTexCoord, 2, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "TexCoords"));
					bool flag10 = CCSModel.AttribNormal != -1;
					if (flag10)
					{
						GL.EnableVertexAttribArray(CCSModel.AttribNormal);
						GL.VertexAttribPointer(CCSModel.AttribNormal, 3, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Normal"));
					}
					GL.EnableVertexAttribArray(CCSModel.AttribColor);
					GL.VertexAttribPointer(CCSModel.AttribColor, 4, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Color"));
					bool flag11 = CCSModel.AttribBoneIDs != -1;
					if (flag11)
					{
						GL.EnableVertexAttribArray(CCSModel.AttribBoneIDs);
						GL.VertexAttribIPointer(CCSModel.AttribBoneIDs, 4, VertexAttribIntegerType.Int, stride, Marshal.OffsetOf(type, "BoneIDs"));
					}
					bool flag12 = CCSModel.AttribBoneWeights != -1;
					if (flag12)
					{
						GL.EnableVertexAttribArray(CCSModel.AttribBoneWeights);
						GL.VertexAttribPointer(CCSModel.AttribBoneWeights, 4, VertexAttribPointerType.Float, false, stride, Marshal.OffsetOf(type, "Weights"));
					}
					subModel.ElementArrayID = GL.GenBuffer();
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, subModel.ElementArrayID);
					GL.BufferData<CCSModel.ModelTriangle>(BufferTarget.ElementArrayBuffer, 12 * subModel.TriangleCount, subModel.Triangles, BufferUsageHint.StaticDraw);
					GL.BindVertexArray(0);
					GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
				}
			}
			return true;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000113CC File Offset: 0x0000F5CC
		public bool LoadShaders()
		{
			bool flag = CCSModel.ProgramID == -1;
			if (flag)
			{
				CCSModel.ProgramID = Scene.LoadProgram("CCSModel", false);
				bool flag2 = CCSModel.ProgramID == -1;
				if (flag2)
				{
					return false;
				}
				CCSModel.AttribPosition = GL.GetAttribLocation(CCSModel.ProgramID, "vPosition");
				CCSModel.AttribPosition1 = GL.GetAttribLocation(CCSModel.ProgramID, "vPosition1");
				CCSModel.AttribPosition2 = GL.GetAttribLocation(CCSModel.ProgramID, "vPosition2");
				CCSModel.AttribPosition3 = GL.GetAttribLocation(CCSModel.ProgramID, "vPosition3");
				CCSModel.AttribTexCoord = GL.GetAttribLocation(CCSModel.ProgramID, "vTexCoords");
				CCSModel.AttribColor = GL.GetAttribLocation(CCSModel.ProgramID, "vColor");
				CCSModel.AttribNormal = GL.GetAttribLocation(CCSModel.ProgramID, "vNormal");
				CCSModel.AttribBoneIDs = GL.GetAttribLocation(CCSModel.ProgramID, "vBoneIDs");
				CCSModel.AttribBoneWeights = GL.GetAttribLocation(CCSModel.ProgramID, "vBoneWeights");
				bool flag3 = CCSModel.AttribPosition == -1 || CCSModel.AttribTexCoord == -1 || CCSModel.AttribColor == -1 || CCSModel.AttribBoneIDs == -1 || CCSModel.AttribBoneWeights == -1;
				if (flag3)
				{
					Logger.LogError("Error getting CCSModel Shader Attributes:\n", Logger.LogType.LogAll, "LoadShaders", 386);
					Logger.LogError(string.Format("\tPosition: {0}, TexCoord: {1}, Normal: {2}, Color: {3}, BoneIDs: {4}, BoneWeights: {5}\n", new object[]
					{
						CCSModel.AttribPosition,
						CCSModel.AttribTexCoord,
						CCSModel.AttribNormal,
						CCSModel.AttribColor,
						CCSModel.AttribBoneIDs,
						CCSModel.AttribBoneWeights
					}), Logger.LogType.LogAll, "LoadShaders", 387);
				}
				CCSModel.UniformMatrix = GL.GetUniformLocation(CCSModel.ProgramID, "uMatrix");
				CCSModel.UniformAlpha = GL.GetUniformLocation(CCSModel.ProgramID, "uAlpha");
				CCSModel.UniformTextureOffset = GL.GetUniformLocation(CCSModel.ProgramID, "uTextureOffset");
				CCSModel.UniformDrawOptions = GL.GetUniformLocation(CCSModel.ProgramID, "uDrawOptions");
				CCSModel.UniformSelectionColor = GL.GetUniformLocation(CCSModel.ProgramID, "uSelectionColor");
				CCSModel.UniformRenderMode = GL.GetUniformLocation(CCSModel.ProgramID, "uRenderMode");
				CCSModel.UniformTexture = GL.GetUniformLocation(CCSModel.ProgramID, "uTexture");
				CCSModel.UniformMatrixList = GL.GetUniformLocation(CCSModel.ProgramID, "uMatrixList");
				bool flag4 = CCSModel.UniformMatrix == -1 || CCSModel.UniformAlpha == -1 || CCSModel.UniformTextureOffset == -1 || CCSModel.UniformDrawOptions == -1 || CCSModel.UniformSelectionColor == -1 || CCSModel.UniformRenderMode == -1 || CCSModel.UniformTexture == -1 || CCSModel.UniformMatrixList == -1;
				if (flag4)
				{
					Logger.LogError("Error getting CCSModel Shader Uniforms:\n", Logger.LogType.LogAll, "LoadShaders", 402);
					Logger.LogError(string.Format("\tMatrix: {0}, Alpha: {1}, TextureOffset: {2}, DrawOptions: {3}, Selection Color: {4}, Render Mode: {5}, Texture: {6}, Matrix List: {7}\n", new object[]
					{
						CCSModel.UniformMatrix,
						CCSModel.UniformAlpha,
						CCSModel.UniformTextureOffset,
						CCSModel.UniformDrawOptions,
						CCSModel.UniformSelectionColor,
						CCSModel.UniformRenderMode,
						CCSModel.UniformTexture,
						CCSModel.UniformMatrixList
					}), Logger.LogType.LogAll, "LoadShaders", 403);
				}
			}
			return true;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00011710 File Offset: 0x0000F910
		public override bool DeInit()
		{
			uint index = 0U;
			while ((ulong)index < (ulong)((long)this.SubModelCount))
			{
				CCSModel.SubModel subModel = this.SubModels[(int)((uint)((UIntPtr)index))];
				bool flag = subModel.VertexArrayID != -1;
				if (flag)
				{
					GL.DeleteVertexArray(subModel.VertexArrayID);
				}
				subModel.VertexArrayID = -1;
				bool flag2 = subModel.VertexBufferID != -1;
				if (flag2)
				{
					GL.DeleteBuffer(subModel.VertexBufferID);
				}
				subModel.VertexBufferID = -1;
				bool flag3 = subModel.ElementArrayID != -1;
				if (flag3)
				{
					GL.DeleteBuffer(subModel.ElementArrayID);
				}
				subModel.ElementArrayID = -1;
				index += 1U;
			}
			CCSModel.ProgramRefs--;
			bool flag4 = CCSModel.ProgramRefs <= 0;
			if (flag4)
			{
				bool flag5 = CCSModel.ProgramID != -1;
				if (flag5)
				{
					GL.DeleteProgram(CCSModel.ProgramID);
				}
				CCSModel.ProgramID = -1;
			}
			return true;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00011808 File Offset: 0x0000FA08
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.VertexScale = bStream.ReadSingle();
			this.ActualModelType = bStream.ReadInt16();
			this.ModelType = (short)((int)this.ActualModelType & 65534);
			this.SubModelCount = bStream.ReadInt16();
			this.DrawFlags = bStream.ReadInt16();
			this.UnkFlags = bStream.ReadInt16();
			int length = bStream.ReadInt32();
			this.Unk2 = length;
			bool flag = this.ParentFile.GetVersion() > CCSFileHeader.CCSVersion.Gen1;
			bool flag2 = flag;
			if (flag2)
			{
				double num = (double)bStream.ReadSingle();
				double num2 = (double)bStream.ReadSingle();
			}
			bool flag3 = this.SubModelCount > 0;
			if (flag3)
			{
				this.SubModels = new CCSModel.SubModel[(int)this.SubModelCount];
				bool flag4 = this.ModelType < 4 || this.ModelType == 1536 || this.ModelType == 512 || this.ModelType == 4096 || this.ModelType == 14336 || this.ModelType == 1024 || this.ModelType == 4608;
				if (flag4)
				{
					for (int index = 0; index < (int)this.SubModelCount; index++)
					{
						CCSModel.SubModel _subModel = new CCSModel.SubModel();
						_subModel.ParentID = bStream.ReadInt32();
						_subModel.MatTexID = bStream.ReadInt32();
						_subModel.VertexCount = bStream.ReadInt32();
						this.ReadRigidSubModel(bStream, _subModel, _subModel.VertexCount);
						this.SubModels[index] = _subModel;
					}
				}
				else
				{
					bool flag5 = this.ModelType == 4 || this.ModelType == 4100 || this.ModelType == 14340;
					if (flag5)
					{
						int[] lookupTbl = null;
						bool flag6 = this.ParentFile.GetVersion() > CCSFileHeader.CCSVersion.Gen1;
						if (flag6)
						{
							lookupTbl = new int[length];
							for (int index2 = 0; index2 < length; index2++)
							{
								lookupTbl[index2] = (int)bStream.ReadByte();
							}
							long position = bStream.BaseStream.Position;
							bool flag7 = position % 4L != 0L;
							if (flag7)
							{
								long offset = 4L - position % 4L;
								bStream.BaseStream.Seek(offset, SeekOrigin.Current);
							}
						}
						for (int index3 = 0; index3 < (int)(this.SubModelCount - 1); index3++)
						{
							CCSModel.SubModel _subModel2 = new CCSModel.SubModel();
							_subModel2.MatTexID = bStream.ReadInt32();
							_subModel2.VertexCount = bStream.ReadInt32();
							bStream.ReadInt32();
							_subModel2.ParentID = bStream.ReadInt32();
							bool flag8 = flag;
							if (flag8)
							{
								bool flag9 = _subModel2.ParentID > length;
								if (flag9)
								{
									Logger.LogError(string.Format("Model 0x{0}: {1}, Sub Model {2}, Lookup table indice out of bounds.", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID), index3), Logger.LogType.LogAll, "Read", 576);
								}
								else
								{
									_subModel2.ParentID = lookupTbl[_subModel2.ParentID];
								}
							}
							this.ReadDeform_RigidSubModel(bStream, _subModel2, _subModel2.VertexCount, lookupTbl);
							this.SubModels[index3] = _subModel2;
						}
						CCSModel.SubModel _subModel3 = new CCSModel.SubModel();
						_subModel3.MatTexID = bStream.ReadInt32();
						_subModel3.VertexCount = bStream.ReadInt32();
						int _weightedCount = bStream.ReadInt32();
						bool flag10 = this.ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen3;
						if (flag10)
						{
							this.ReadDeform_DeformSubModel(bStream, _subModel3, _subModel3.VertexCount, _weightedCount, lookupTbl);
						}
						else
						{
							this.ReadDeform_DeformSubModel_Gen3(bStream, _subModel3, _subModel3.VertexCount, _weightedCount, lookupTbl);
						}
						this.SubModels[(int)(this.SubModelCount - 1)] = _subModel3;
					}
					else
					{
						bool flag11 = this.ModelType != 8;
						if (flag11)
						{
							Logger.LogError(string.Format("Model {0}: Unknown model type: 0x{1:X}\n", this.ParentFile.GetSubObjectName(this.ObjectID), this.ModelType), Logger.LogType.LogAll, "Read", 622);
							return false;
						}
						for (int index4 = 0; index4 < (int)this.SubModelCount; index4++)
						{
							CCSModel.SubModel _subModel4 = new CCSModel.SubModel();
							_subModel4.VertexCount = bStream.ReadInt32();
							int num3 = bStream.ReadInt32();
							bool flag12 = num3 % 3 != 0;
							if (flag12)
							{
								Logger.LogWarning(string.Format("Shadow Model {0:X}:{1} has an bad number of triangle verts!", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID)), Logger.LogType.LogAll, "Read", 612);
							}
							_subModel4.TriangleCount = num3 / 3;
							this.ReadShadowModel(bStream, _subModel4, _subModel4.VertexCount, _subModel4.TriangleCount);
							this.SubModels[index4] = _subModel4;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00011CA8 File Offset: 0x0000FEA8
		public string GetModelTypeStr()
		{
			return (this.ModelType != 0 || this.SubModelCount != 0) ? ((this.ModelType >= 4) ? ((this.ModelType != 4) ? ((this.ModelType != 8) ? ((this.ModelType != 1536) ? ((this.ModelType != 4096) ? ((this.ModelType != 512) ? ((this.ModelType != 4100) ? ((this.ModelType != 1024) ? string.Format("Unknown Model Type: 0x{0:X}", this.ActualModelType) : "Morph Target (Gen2)") : "Deformable (Gen2)") : "Rigid(No Color) (Gen2)") : "Rigid (Gen2)") : "Morph Target") : "Shadow") : "Deformable") : "Rigid") : "None";
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00011D80 File Offset: 0x0000FF80
		public override TreeNode ToNode()
		{
			TreeNode node = base.ToNode();
			string str = string.Format(" Type: {0}", this.GetModelTypeStr());
			TreeNode treeNode = node;
			treeNode.Text += str;
			for (int _subID = 0; _subID < (int)this.SubModelCount; _subID++)
			{
				bool flag = (this.ModelType & 8) == 8;
				if (flag)
				{
					TreeNode node2 = new TreeNode(string.Format("Sub Model {0}", _subID))
					{
						Tag = new TreeNodeTag(this.ParentFile, this.ObjectID, this.ObjectType, TreeNodeTag.NodeType.SubNode, _subID)
					};
					node.Nodes.Add(node2);
				}
				else
				{
					bool flag2 = this.ModelType == 4 || this.ModelType == 4100 || this.ModelType == 14340;
					if (flag2)
					{
						CCSModel.SubModel subModel = this.SubModels[_subID];
						this.ClumpRef.GetObject(subModel.ParentID);
						string subObjectName = this.ParentFile.GetSubObjectName(this.ClumpRef.GetObject(subModel.ParentID).ObjectID);
						TreeNode node3 = new TreeNode(string.Format("Sub Model {0}: {1}", _subID, subObjectName))
						{
							Tag = new TreeNodeTag(this.ParentFile, this.ObjectID, this.ObjectType, TreeNodeTag.NodeType.SubNode, _subID)
						};
						node.Nodes.Add(node3);
					}
					else
					{
						string subObjectName2 = this.ParentFile.GetSubObjectName(this.SubModels[_subID].ParentID);
						TreeNode node4 = new TreeNode(string.Format("Sub Model {0}: {1}", _subID, subObjectName2))
						{
							Tag = new TreeNodeTag(this.ParentFile, this.ObjectID, this.ObjectType, TreeNodeTag.NodeType.SubNode, _subID)
						};
						node.Nodes.Add(node4);
					}
				}
			}
			return node;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00011F58 File Offset: 0x00010158
		public void Render(Matrix4 mtx, int extraOptions = 0, int single = -1)
		{
			bool flag = (this.ModelType & 8) != 0 || CCSModel.ProgramID == -1 || this.SubModelCount == 0;
			if (!flag)
			{
				GL.UseProgram(CCSModel.ProgramID);
				bool flag2 = single > -1;
				if (flag2)
				{
					bool flag3 = single < (int)this.SubModelCount;
					if (flag3)
					{
						this.RenderSubModel(mtx, single, extraOptions);
					}
				}
				else
				{
					for (int subModelID = 0; subModelID < (int)this.SubModelCount; subModelID++)
					{
						this.RenderSubModel(mtx, subModelID, extraOptions);
					}
				}
				GL.UseProgram(0);
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00011FE4 File Offset: 0x000101E4
		private void RenderSubModel(Matrix4 mtx, int subModelID, int extraOptions = 0)
		{
			CCSModel.SubModel subModel = this.SubModels[subModelID];
			GL.BindVertexArray(subModel.VertexArrayID);
			Matrix4 matrix = mtx;
			Vector2 vector = Vector2.Zero;
			float v0_ = 1f;
			GL.ActiveTexture(TextureUnit.Texture0);
			bool flag = subModel.ParentMaterialRef != null;
			if (flag)
			{
				CCSMaterial parentMaterialRef = subModel.ParentMaterialRef;
				vector = parentMaterialRef.TextureOffset;
				v0_ = parentMaterialRef.Alpha;
			}
			CCSTexture parentTextureRef = subModel.ParentTextureRef;
			bool flag2 = parentTextureRef != null;
			if (flag2)
			{
				GL.BindTexture(TextureTarget.Texture2D, parentTextureRef.TextureID);
				bool flag3 = parentTextureRef.TextureType == 135 || parentTextureRef.TextureType == 137;
				if (flag3)
				{
					extraOptions |= Scene.SCENE_DRAW_FLIP_TEXCOORDS;
				}
			}
			else
			{
				GL.BindTexture(TextureTarget.Texture2D, 0);
			}
			GL.UniformMatrix4(CCSModel.UniformMatrix, false, ref matrix);
			GL.Uniform1(CCSModel.UniformAlpha, v0_);
			GL.Uniform2(CCSModel.UniformTextureOffset, ref vector);
			bool flag4 = this.ParentFile.GetVersion() > CCSFileHeader.CCSVersion.Gen1;
			if (flag4)
			{
				GL.Uniform1(CCSModel.UniformDrawOptions, 0);
			}
			else
			{
				GL.Uniform1(CCSModel.UniformDrawOptions, (int)this.DrawFlags);
			}
			int v0_2 = extraOptions & -2;
			bool flag5 = v0_2 != 0;
			if (flag5)
			{
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				Vector4 vector2 = new Vector4(1f, 1f, 1f, 0f);
				bool flag6 = (v0_2 & Scene.SCENE_DRAW_VERTEX_COLORS) == 0 && (v0_2 & Scene.SCENE_DRAW_SMOOTH) == 0;
				if (flag6)
				{
					vector2 = new Vector4(0.5f, 0.5f, 0.5f, 0f);
				}
				GL.Uniform4(CCSModel.UniformSelectionColor, ref vector2);
				GL.Uniform1(CCSModel.UniformRenderMode, v0_2);
				GL.Uniform1(CCSModel.UniformMatrixList, 1);
				GL.Uniform1(CCSModel.UniformTexture, 0);
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, subModel.ElementArrayID);
				GL.DrawElements(PrimitiveType.Triangles, subModel.TriangleCount * 3, DrawElementsType.UnsignedInt, 0);
			}
			bool flag7 = (extraOptions & Scene.SCENE_DRAW_LINES) != 0;
			if (flag7)
			{
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				bool flag8 = (extraOptions & Scene.SCENE_DRAW_SELECTION) != 0;
				if (flag8)
				{
					GL.Uniform4(CCSModel.UniformSelectionColor, 1f, 1f, 0f, 1f);
				}
				else
				{
					GL.Uniform4(CCSModel.UniformSelectionColor, 1f, 1f, 1f, 1f);
				}
				int v0_3 = 1;
				GL.Uniform1(CCSModel.UniformRenderMode, v0_3);
				GL.DrawElements(PrimitiveType.Triangles, subModel.TriangleCount * 3, DrawElementsType.UnsignedInt, 0);
			}
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.DepthMask(true);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0001228C File Offset: 0x0001048C
		public void SetClump(CCSClump _parentClump, CCSObject _parentObject)
		{
			this.ClumpRef = _parentClump;
			this.ObjectRef = _parentObject;
			for (int index = 0; index < (int)this.SubModelCount; index++)
			{
				CCSModel.SubModel subModel = this.SubModels[index];
				bool flag = subModel != null;
				if (flag)
				{
					subModel.ParentObjectRef = ((this.ModelType == 4 || this.ModelType == 4100 || this.ModelType == 14340) ? this.ClumpRef.GetObject(subModel.ParentID) : this.ParentFile.GetObject<CCSObject>(subModel.ParentID));
				}
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00012320 File Offset: 0x00010520
		private bool ReadRigidSubModel(BinaryReader bStream, CCSModel.SubModel _subModel, int _vertexCount)
		{
			long position = bStream.BaseStream.Position;
			_subModel.VertexCount = _vertexCount;
			_subModel.Vertices = new CCSModel.ModelVertex[_vertexCount];
			int id = this.ParentFile.SearchClump(this.ParentFile.LastObject.ObjectID);
			bool flag = id == -1;
			if (flag)
			{
				Logger.LogError(string.Format("CCSModel::Read(): Model {0:X} at 0x{1:X}, Last Bone object ID is not part of a clump that have been read yet...\n", this.ObjectID, position), Logger.LogType.LogAll, "ReadRigidSubModel", 945);
				id = 0;
			}
			for (int index = 0; index < _vertexCount; index++)
			{
				CCSModel.ModelVertex modelVertex = default(CCSModel.ModelVertex);
				modelVertex.Color = new Vector4(0.5f, 0.5f, 0.5f, 1f);
				modelVertex.Position = Util.ReadVec3Half(bStream, this.VertexScale);
				modelVertex.BoneIDs = new CCSModel.BoneID(id, 0, 0, 0);
				modelVertex.Weights = new Vector4(1f, 0f, 0f, 0f);
				_subModel.Vertices[index] = modelVertex;
			}
			bool flag2 = bStream.BaseStream.Position % 4L == 2L;
			if (flag2)
			{
				int num = (int)bStream.ReadUInt16();
			}
			int position2 = (int)bStream.BaseStream.Position;
			int length = 0;
			uint index2 = 0U;
			while ((ulong)index2 < (ulong)((long)_vertexCount))
			{
				bool flag3 = (bStream.ReadInt32() >> 24 & 255) == 0;
				if (flag3)
				{
					length++;
				}
				index2 += 1U;
			}
			bStream.BaseStream.Seek((long)position2, SeekOrigin.Begin);
			_subModel.TriangleCount = length;
			_subModel.Triangles = new CCSModel.ModelTriangle[length];
			int index3 = 0;
			byte num2 = 1;
			int num3 = 0;
			for (int index4 = 0; index4 < _vertexCount; index4++)
			{
				_subModel.Vertices[index4].Normal = Util.ReadVec3Normal8(bStream);
				byte num4 = bStream.ReadByte();
				bool flag4 = num4 == 0;
				if (flag4)
				{
					_subModel.Triangles[index3] = ((num3 % 2 != 0) ? new CCSModel.ModelTriangle(index4 - 2, index4 - 1, index4) : new CCSModel.ModelTriangle(index4, index4 - 1, index4 - 2));
					index3++;
					num3++;
					num2 = num4;
				}
				else
				{
					bool flag5 = num2 == 0;
					if (flag5)
					{
						num3 = 0;
					}
				}
			}
			bool flag6 = this.ModelType < 4 || this.ModelType == 4096 || this.ModelType == 14336 || this.ModelType == 1024;
			if (flag6)
			{
				for (int index5 = 0; index5 < _vertexCount; index5++)
				{
					_subModel.Vertices[index5].Color = Util.ReadVec4RGBA32(bStream);
				}
			}
			bool flag7 = this.ModelType != 1536 && this.ModelType != 1024;
			if (flag7)
			{
				for (int index6 = 0; index6 < _vertexCount; index6++)
				{
					_subModel.Vertices[index6].TexCoords = ((this.ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen3) ? Util.ReadVec2UV(bStream) : Util.ReadVec2UV_Gen3(bStream));
				}
			}
			bool flag8 = this.ModelType == 0 && this.ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen3 && this.Unk2 != 0;
			if (flag8)
			{
				bStream.BaseStream.Seek((long)(_vertexCount * 8), SeekOrigin.Current);
			}
			return true;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00012680 File Offset: 0x00010880
		private bool ReadDeform_RigidSubModel(BinaryReader bStream, CCSModel.SubModel _subModel, int _vertexCount, int[] lookupTbl)
		{
			long position = bStream.BaseStream.Position;
			_subModel.VertexCount = _vertexCount;
			_subModel.Vertices = new CCSModel.ModelVertex[_vertexCount];
			for (int index = 0; index < _vertexCount; index++)
			{
				CCSModel.ModelVertex modelVertex = default(CCSModel.ModelVertex);
				modelVertex.Color = new Vector4(0.5f, 0.5f, 0.5f, 1f);
				modelVertex.Position = Util.ReadVec3Half(bStream, this.VertexScale);
				modelVertex.BoneIDs = new CCSModel.BoneID(_subModel.ParentID, 0, 0, 0);
				modelVertex.Weights = new Vector4(1f, 0f, 0f, 0f);
				_subModel.Vertices[index] = modelVertex;
			}
			bool flag = bStream.BaseStream.Position % 4L == 2L;
			if (flag)
			{
				int num = (int)bStream.ReadInt16();
			}
			int position2 = (int)bStream.BaseStream.Position;
			int length = 0;
			uint index2 = 0U;
			while ((ulong)index2 < (ulong)((long)_vertexCount))
			{
				bool flag2 = (bStream.ReadInt32() >> 24 & 255) == 0;
				if (flag2)
				{
					length++;
				}
				index2 += 1U;
			}
			bStream.BaseStream.Seek((long)position2, SeekOrigin.Begin);
			_subModel.TriangleCount = length;
			_subModel.Triangles = new CCSModel.ModelTriangle[length];
			int index3 = 0;
			byte num2 = 1;
			int num3 = 0;
			for (int index4 = 0; index4 < _vertexCount; index4++)
			{
				_subModel.Vertices[index4].Normal = Util.ReadVec3Normal8(bStream);
				byte num4 = bStream.ReadByte();
				bool flag3 = num4 == 0;
				if (flag3)
				{
					_subModel.Triangles[index3] = ((num3 % 2 != 0) ? new CCSModel.ModelTriangle(index4 - 2, index4 - 1, index4) : new CCSModel.ModelTriangle(index4, index4 - 1, index4 - 2));
					index3++;
					num3++;
					num2 = num4;
				}
				else
				{
					bool flag4 = num2 == 0;
					if (flag4)
					{
						num3 = 0;
					}
				}
			}
			for (int index5 = 0; index5 < _vertexCount; index5++)
			{
				_subModel.Vertices[index5].TexCoords = ((this.ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen3) ? Util.ReadVec2UV(bStream) : Util.ReadVec2UV_Gen3(bStream));
			}
			return true;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000128C4 File Offset: 0x00010AC4
		private bool ReadDeform_DeformSubModel(BinaryReader bStream, CCSModel.SubModel _subModel, int _vertexCount, int _weightedCount, int[] lookupTbl)
		{
			int position = (int)bStream.BaseStream.Position;
			int num = position + _weightedCount * 8;
			int num2 = num + _weightedCount * 4;
			_subModel.Vertices = new CCSModel.ModelVertex[_vertexCount];
			_subModel.Triangles = new CCSModel.ModelTriangle[_vertexCount];
			int num3 = 0;
			int length = 0;
			byte num4 = 1;
			int num5 = 0;
			for (int index = 0; index < _vertexCount; index++)
			{
				int offset = position + num3 * 8;
				int offset2 = num + num3 * 4;
				int offset3 = num2 + index * 4;
				bStream.BaseStream.Seek((long)offset, SeekOrigin.Begin);
				CCSModel.ModelVertex modelVertex = default(CCSModel.ModelVertex);
				modelVertex.Position = Util.ReadVec3Half(bStream, this.VertexScale);
				int num6 = (int)bStream.ReadUInt16();
				int id = num6 >> 10;
				int id2 = 0;
				float x = (float)(num6 & 511) * 0.00390625f;
				float y = 0f;
				bool flag = (num6 >> 9 & 1) == 0;
				if (flag)
				{
					num3++;
					modelVertex.Position2 = Util.ReadVec3Half(bStream, this.VertexScale);
					int num7 = (int)bStream.ReadUInt16();
					y = (float)(num7 & 511) * 0.00390625f;
					id2 = num7 >> 10;
				}
				modelVertex.BoneIDs = ((lookupTbl == null) ? new CCSModel.BoneID(id, id2, 0, 0) : new CCSModel.BoneID(lookupTbl[id], lookupTbl[id2], 0, 0));
				modelVertex.Weights = new Vector4(x, y, 0f, 0f);
				bStream.BaseStream.Seek((long)offset2, SeekOrigin.Begin);
				modelVertex.Normal = Util.ReadVec3Normal8(bStream);
				byte num8 = bStream.ReadByte();
				bool flag2 = num8 == 0;
				if (flag2)
				{
					_subModel.Triangles[length] = ((num5 % 2 != 0) ? new CCSModel.ModelTriangle(index - 2, index - 1, index) : new CCSModel.ModelTriangle(index, index - 1, index - 2));
					length++;
					num5++;
					num4 = num8;
				}
				else
				{
					bool flag3 = num4 == 0;
					if (flag3)
					{
						num5 = 0;
					}
				}
				bStream.BaseStream.Seek((long)offset3, SeekOrigin.Begin);
				modelVertex.TexCoords = ((this.ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen3) ? Util.ReadVec2UV(bStream) : Util.ReadVec2UV_Gen3(bStream));
				modelVertex.Color = new Vector4(0.5f, 0.5f, 0.5f, 1f);
				_subModel.Vertices[index] = modelVertex;
				num3++;
			}
			_subModel.TriangleCount = length;
			CCSModel.ModelTriangle[] triangles = _subModel.Triangles;
			_subModel.Triangles = new CCSModel.ModelTriangle[length];
			CCSModel.ModelTriangle[] triangles2 = _subModel.Triangles;
			int length2 = length;
			Array.Copy(triangles, triangles2, length2);
			return true;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00012B4C File Offset: 0x00010D4C
		private bool ReadDeform_DeformSubModel_Gen3(BinaryReader bStream, CCSModel.SubModel _subModel, int _vertexCount, int _weightedCount, int[] lookupTbl)
		{
			int position = (int)bStream.BaseStream.Position;
			int num = position + _weightedCount * 12;
			int num2 = num + _weightedCount * 4;
			_subModel.Vertices = new CCSModel.ModelVertex[_vertexCount];
			_subModel.Triangles = new CCSModel.ModelTriangle[_vertexCount];
			int num3 = 0;
			int length = 0;
			byte num4 = 1;
			int num5 = 0;
			for (int index = 0; index < _vertexCount; index++)
			{
				int offset = position + num3 * 12;
				int offset2 = num + num3 * 4;
				int offset3 = num2 + index * 8;
				bStream.BaseStream.Seek((long)offset, SeekOrigin.Begin);
				CCSModel.ModelVertex modelVertex = default(CCSModel.ModelVertex);
				Vector3[] vector3Array = new Vector3[]
				{
					Vector3.Zero,
					Vector3.Zero,
					Vector3.Zero,
					Vector3.Zero
				};
				int[] numArray = new int[4];
				float[] numArray2 = new float[4];
				for (int index2 = 0; index2 < 4; index2++)
				{
					vector3Array[index2] = Util.ReadVec3Half(bStream, this.VertexScale);
					numArray2[index2] = (float)bStream.ReadInt16() * 0.00390625f;
					int num6 = (int)bStream.ReadInt16();
					numArray[index2] = (int)bStream.ReadInt16();
					bool flag = num6 != 0;
					if (flag)
					{
						num3 += index2;
						break;
					}
				}
				modelVertex.BoneIDs = ((lookupTbl == null) ? new CCSModel.BoneID(numArray[0], numArray[1], numArray[2], numArray[3]) : new CCSModel.BoneID(lookupTbl[numArray[0]], lookupTbl[numArray[1]], lookupTbl[numArray[2]], lookupTbl[numArray[3]]));
				modelVertex.Weights = new Vector4(numArray2[0], numArray2[1], numArray2[2], numArray2[3]);
				modelVertex.Position = vector3Array[0];
				modelVertex.Position2 = vector3Array[1];
				modelVertex.Position3 = vector3Array[2];
				modelVertex.Position4 = vector3Array[3];
				bStream.BaseStream.Seek((long)offset2, SeekOrigin.Begin);
				modelVertex.Normal = Util.ReadVec3Normal8(bStream);
				byte num7 = bStream.ReadByte();
				bool flag2 = num7 == 0;
				if (flag2)
				{
					_subModel.Triangles[length] = ((num5 % 2 != 0) ? new CCSModel.ModelTriangle(index - 2, index - 1, index) : new CCSModel.ModelTriangle(index, index - 1, index - 2));
					length++;
					num5++;
					num4 = num7;
				}
				else
				{
					bool flag3 = num4 == 0;
					if (flag3)
					{
						num5 = 0;
					}
				}
				bStream.BaseStream.Seek((long)offset3, SeekOrigin.Begin);
				modelVertex.TexCoords = Util.ReadVec2UV_Gen3(bStream);
				modelVertex.Color = new Vector4(0.5f, 0.5f, 0.5f, 1f);
				_subModel.Vertices[index] = modelVertex;
				num3++;
			}
			_subModel.TriangleCount = length;
			CCSModel.ModelTriangle[] triangles = _subModel.Triangles;
			_subModel.Triangles = new CCSModel.ModelTriangle[length];
			CCSModel.ModelTriangle[] triangles2 = _subModel.Triangles;
			int length2 = length;
			Array.Copy(triangles, triangles2, length2);
			return true;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00012E40 File Offset: 0x00011040
		private bool ReadShadowModel(BinaryReader bStream, CCSModel.SubModel _subModel, int _vertexCount, int _triangleCount)
		{
			_subModel.Vertices = new CCSModel.ModelVertex[_vertexCount];
			_subModel.Triangles = new CCSModel.ModelTriangle[_triangleCount];
			for (int index = 0; index < _vertexCount; index++)
			{
				_subModel.Vertices[index] = new CCSModel.ModelVertex
				{
					Position = Util.ReadVec3Half(bStream, this.VertexScale)
				};
			}
			bool flag = bStream.BaseStream.Position % 4L == 2L;
			if (flag)
			{
				int num = (int)bStream.ReadInt16();
			}
			for (int index2 = 0; index2 < _triangleCount; index2++)
			{
				CCSModel.ModelTriangle modelTriangle = new CCSModel.ModelTriangle
				{
					ID1 = bStream.ReadInt32(),
					ID2 = bStream.ReadInt32(),
					ID3 = bStream.ReadInt32()
				};
				_subModel.Triangles[index2] = modelTriangle;
			}
			return true;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00012F24 File Offset: 0x00011124
		public string GetReport(int level = 0)
		{
			string report = Util.Indent(level, false) + string.Format("Model 0x{0:X}: {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID)) + Util.Indent(level + 1, false) + string.Format("Type: 0x{0:X}: {1}, {2} Vertices, {3} Triangles, {4} Sub Models\n", new object[]
			{
				this.ActualModelType,
				this.GetModelTypeStr(),
				this.VertexCount,
				this.TriangleCount,
				this.SubModelCount
			});
			for (int index = 0; index < (int)this.SubModelCount; index++)
			{
				CCSModel.SubModel subModel = this.SubModels[index];
				bool flag = (this.ModelType & 8) == 8;
				string str;
				if (flag)
				{
					str = string.Format("Shadow Sub Model {0}", index);
				}
				else
				{
					bool flag2 = (this.ModelType & 4) == 4 || (this.ModelType & 4100) == 4100 || (this.ModelType & 14340) == 14340;
					if (flag2)
					{
						this.ClumpRef.GetObject(subModel.ParentID);
						str = this.ParentFile.GetSubObjectName(this.ClumpRef.GetObject(subModel.ParentID).ObjectID);
					}
					else
					{
						str = this.ParentFile.GetSubObjectName(subModel.ParentID);
					}
				}
				string str2 = string.Concat(new string[]
				{
					report,
					Util.Indent(level + 1, false),
					string.Format("Sub Model {0}: {1}\n", index, str),
					Util.Indent(level + 2, false),
					string.Format("{0} Vertices, {1} Triangles\n", subModel.VertexCount, subModel.TriangleCount)
				});
				int subObjectType = this.ParentFile.GetSubObjectType(subModel.MatTexID);
				string str3 = Util.Indent(level + 2, false) + "No Texture or Matrial applied.\n";
				int num = subObjectType;
				int num2 = num;
				if (num2 != 512)
				{
					if (num2 == 768)
					{
						str3 = Util.Indent(level + 2, false) + string.Format("Texture: 0x{0}: {1}\n", subModel.MatTexID, this.ParentFile.GetSubObjectName(subModel.MatTexID));
					}
				}
				else
				{
					str3 = Util.Indent(level + 2, false) + string.Format("Material: 0x{0}: {1}\n", subModel.MatTexID, this.ParentFile.GetSubObjectName(subModel.MatTexID));
				}
				report = str2 + str3;
			}
			return report;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000131BC File Offset: 0x000113BC
		public int DumpToObj(StreamWriter fStream, int vOffset, bool split, bool withNormals = false)
		{
			int num = vOffset;
			bool flag = this.SubModelCount > 0 && this.ModelType != 8;
			if (flag)
			{
				fStream.WriteLine(string.Format("o {0}", this.ParentFile.GetSubObjectName(this.ObjectID)));
				for (int index = 0; index < (int)this.SubModelCount; index++)
				{
					CCSModel.SubModel subModel = this.SubModels[index];
					bool flag2 = subModel.VertexCount > 0;
					if (flag2)
					{
						Matrix4 identity = Matrix4.Identity;
						Matrix4 identity2 = Matrix4.Identity;
						Matrix4 identity3 = Matrix4.Identity;
						string str = (this.ModelType == 4 || this.ModelType == 4100 || this.ModelType == 14340) ? this.ParentFile.GetSubObjectName(this.ClumpRef.GetObject(subModel.ParentID).ObjectID) : this.ParentFile.GetSubObjectName(subModel.ParentID);
						fStream.WriteLine(string.Format("# {0}, {1} Vertices, {2} triangles", str, subModel.VertexCount, subModel.TriangleCount));
						if (split)
						{
							fStream.WriteLine(string.Format("g {0}", str));
						}
						bool flag3 = subModel.ParentTextureRef != null;
						if (flag3)
						{
							fStream.WriteLine(string.Format("usemtl {0}", this.ParentFile.GetSubObjectName(subModel.ParentTextureRef.ObjectID)));
						}
						for (int index2 = 0; index2 < subModel.VertexCount; index2++)
						{
							CCSModel.ModelVertex vertex = subModel.Vertices[index2];
							Vector3 vector3 = this.SoftSkinVertex(vertex);
							fStream.WriteLine(string.Format("v\t{0}\t{1}\t{2}", (float)(-(float)((double)vector3.X)), vector3.Y, (float)(-(float)((double)vector3.Z))));
							CCSTexture parentTextureRef = subModel.ParentTextureRef;
							bool flag4 = parentTextureRef != null;
							if (flag4)
							{
								bool flag5 = parentTextureRef.TextureType == 135 || parentTextureRef.TextureType == 137;
								if (flag5)
								{
									fStream.WriteLine(string.Format("vt\t{0}\t{1}", vertex.TexCoords.X, vertex.TexCoords.Y));
								}
								else
								{
									fStream.WriteLine(string.Format("vt\t{0}\t{1}", vertex.TexCoords.X, 1.0 - (double)vertex.TexCoords.Y));
								}
							}
							else
							{
								fStream.WriteLine(string.Format("vt\t{0}\t{1}", vertex.TexCoords.X, 1.0 - (double)vertex.TexCoords.Y));
							}
							fStream.WriteLine(string.Format("vn\t{0}\t{1}\t{2}", vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z));
						}
						for (int index3 = 0; index3 < subModel.TriangleCount; index3++)
						{
							CCSModel.ModelTriangle triangle = subModel.Triangles[index3];
							if (withNormals)
							{
								fStream.WriteLine(string.Format("f {0}/{0}/{0}\t{1}/{1}/{1}\t{2}/{2}/{2}", triangle.ID1 + num, triangle.ID2 + num, triangle.ID3 + num));
							}
							else
							{
								fStream.WriteLine(string.Format("f {0}/{0}\t{1}/{1}\t{2}/{2}", triangle.ID1 + num, triangle.ID2 + num, triangle.ID3 + num));
							}
						}
						num += subModel.VertexCount;
					}
				}
			}
			return num;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000135A0 File Offset: 0x000117A0
		public void DumpToSMD(StreamWriter outf, bool withNormals)
		{
			bool flag = this.SubModelCount == 0;
			if (!flag)
			{
				foreach (CCSModel.SubModel subModel in this.SubModels)
				{
					string str = "None";
					bool flag2 = subModel.ParentTextureRef != null;
					if (flag2)
					{
						str = this.ParentFile.GetSubObjectName(subModel.ParentTextureRef.ObjectID);
					}
					for (int index = 0; index < subModel.TriangleCount; index++)
					{
						CCSModel.ModelTriangle triangle = subModel.Triangles[index];
						outf.WriteLine(str);
						int[] numArray = new int[]
						{
							triangle.ID1,
							triangle.ID2,
							triangle.ID3
						};
						for (int index2 = 0; index2 < 3; index2++)
						{
							CCSModel.ModelVertex vertex = subModel.Vertices[numArray[index2]];
							int parentId = subModel.ParentID;
							Vector3 vector3 = this.SoftSkinVertex(vertex);
							string str2 = string.Format("{0} {1} {2}", vector3.X, vector3.Y, vector3.Z);
							string str3 = "0.0 0.0 0.0";
							if (withNormals)
							{
								str3 = string.Format("{0} {1} {2}", vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z);
							}
							string str4 = string.Format("{0} {1}", vertex.TexCoords.X, vertex.TexCoords.Y);
							int[] numArray2 = new int[]
							{
								vertex.BoneIDs.Bone1,
								vertex.BoneIDs.Bone2,
								vertex.BoneIDs.Bone3,
								vertex.BoneIDs.Bone4
							};
							float[] numArray3 = new float[]
							{
								vertex.Weights.X,
								vertex.Weights.Y,
								vertex.Weights.Z,
								vertex.Weights.W
							};
							string str5 = "";
							int num = 0;
							for (int index3 = 0; index3 < 4; index3++)
							{
								bool flag3 = (double)numArray3[index3] != 0.0;
								if (flag3)
								{
									num++;
									str5 += string.Format(" {0} {1}", numArray2[index3], numArray3[index3]);
								}
							}
							string str6 = num.ToString() + str5;
							outf.WriteLine(string.Format("{0} {1} {2} {3} {4}", new object[]
							{
								parentId,
								str2,
								str3,
								str4,
								str6
							}));
						}
					}
				}
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00013898 File Offset: 0x00011A98
		public Vector3 SoftSkinVertex(CCSModel.ModelVertex v)
		{
			Matrix4 finalMatrix = this.ClumpRef.GetFinalMatrix(v.BoneIDs.Bone1);
			Matrix4 finalMatrix2 = this.ClumpRef.GetFinalMatrix(v.BoneIDs.Bone2);
			Matrix4 finalMatrix3 = this.ClumpRef.GetFinalMatrix(v.BoneIDs.Bone3);
			Matrix4 finalMatrix4 = this.ClumpRef.GetFinalMatrix(v.BoneIDs.Bone4);
			Vector4 vector4_ = new Vector4(v.Position.X, v.Position.Y, v.Position.Z, 1f) * finalMatrix;
			Vector4 vector4_2 = new Vector4(v.Position2.X, v.Position2.Y, v.Position2.Z, 1f) * finalMatrix2;
			Vector4 vector4_3 = new Vector4(v.Position3.X, v.Position3.Y, v.Position3.Z, 1f) * finalMatrix3;
			Vector4 vector4_4 = new Vector4(v.Position4.X, v.Position4.Y, v.Position4.Z, 1f) * finalMatrix4;
			return new Vector3(vector4_.X * v.Weights.X, vector4_.Y * v.Weights.X, vector4_.Z * v.Weights.X) + new Vector3(vector4_2.X * v.Weights.Y, vector4_2.Y * v.Weights.Y, vector4_2.Z * v.Weights.Y) + new Vector3(vector4_3.X * v.Weights.Z, vector4_3.Y * v.Weights.Z, vector4_3.Z * v.Weights.Z) + new Vector3(vector4_4.X * v.Weights.W, vector4_4.Y * v.Weights.W, vector4_4.Z * v.Weights.W);
		}

		// Token: 0x040001B0 RID: 432
		public const int CCS_MODEL_DEFORMABLE = 4;

		// Token: 0x040001B1 RID: 433
		public const int CCS_MODEL_SHADOW = 8;

		// Token: 0x040001B2 RID: 434
		public const int CCS_MODEL_MORPHTARGET = 1536;

		// Token: 0x040001B3 RID: 435
		public const int CCS_MODEL_DEFORMABLE_GEN2 = 4100;

		// Token: 0x040001B4 RID: 436
		public const int CCS_MODEL_RIGID_GEN2_NO_COLOR = 512;

		// Token: 0x040001B5 RID: 437
		public const int CCS_MODEL_RIGID_GEN2_COLOR = 4096;

		// Token: 0x040001B6 RID: 438
		public const int CCS_MODEL_RIGID_GEN2_NO_COLOR2 = 4608;

		// Token: 0x040001B7 RID: 439
		public const int CCS_MODEL_MORPHTARGET_GEN2 = 1024;

		// Token: 0x040001B8 RID: 440
		public static int ProgramID = -1;

		// Token: 0x040001B9 RID: 441
		public static int ProgramRefs = 0;

		// Token: 0x040001BA RID: 442
		public static int AttribPosition = -1;

		// Token: 0x040001BB RID: 443
		public static int AttribPosition1 = -1;

		// Token: 0x040001BC RID: 444
		public static int AttribPosition2 = -1;

		// Token: 0x040001BD RID: 445
		public static int AttribPosition3 = -1;

		// Token: 0x040001BE RID: 446
		public static int AttribTexCoord = -1;

		// Token: 0x040001BF RID: 447
		public static int AttribNormal = -1;

		// Token: 0x040001C0 RID: 448
		public static int AttribColor = -1;

		// Token: 0x040001C1 RID: 449
		public static int AttribBoneIDs = -1;

		// Token: 0x040001C2 RID: 450
		public static int AttribBoneWeights = -1;

		// Token: 0x040001C3 RID: 451
		public static int UniformMatrix = -1;

		// Token: 0x040001C4 RID: 452
		public static int UniformAlpha = -1;

		// Token: 0x040001C5 RID: 453
		public static int UniformTextureOffset = -1;

		// Token: 0x040001C6 RID: 454
		public static int UniformTexture = -1;

		// Token: 0x040001C7 RID: 455
		public static int UniformDrawOptions = -1;

		// Token: 0x040001C8 RID: 456
		public static int UniformSelectionColor = -1;

		// Token: 0x040001C9 RID: 457
		public static int UniformRenderMode = -1;

		// Token: 0x040001CA RID: 458
		public static int UniformMatrixList = -1;

		// Token: 0x040001CB RID: 459
		public float VertexScale = 512f;

		// Token: 0x040001CC RID: 460
		public short ModelType;

		// Token: 0x040001CD RID: 461
		public short ActualModelType;

		// Token: 0x040001CE RID: 462
		public short SubModelCount;

		// Token: 0x040001CF RID: 463
		public int VertexCount;

		// Token: 0x040001D0 RID: 464
		public int TriangleCount;

		// Token: 0x040001D1 RID: 465
		public short DrawFlags;

		// Token: 0x040001D2 RID: 466
		public short UnkFlags;

		// Token: 0x040001D3 RID: 467
		public int Unk2;

		// Token: 0x040001D4 RID: 468
		public CCSModel.SubModel[] SubModels;

		// Token: 0x040001D5 RID: 469
		public CCSClump ClumpRef;

		// Token: 0x040001D6 RID: 470
		public CCSObject ObjectRef;

		// Token: 0x02000062 RID: 98
		public struct BoneID
		{
			// Token: 0x06000232 RID: 562 RVA: 0x00018B0B File Offset: 0x00016D0B
			public BoneID(int id1, int id2, int id3 = 0, int id4 = 0)
			{
				this.Bone1 = id1;
				this.Bone2 = id2;
				this.Bone3 = id3;
				this.Bone4 = id4;
			}

			// Token: 0x040002C9 RID: 713
			public int Bone1;

			// Token: 0x040002CA RID: 714
			public int Bone2;

			// Token: 0x040002CB RID: 715
			public int Bone3;

			// Token: 0x040002CC RID: 716
			public int Bone4;
		}

		// Token: 0x02000063 RID: 99
		public struct ModelVertex
		{
			// Token: 0x040002CD RID: 717
			public Vector3 Position;

			// Token: 0x040002CE RID: 718
			public Vector3 Position2;

			// Token: 0x040002CF RID: 719
			public Vector3 Position3;

			// Token: 0x040002D0 RID: 720
			public Vector3 Position4;

			// Token: 0x040002D1 RID: 721
			public Vector2 TexCoords;

			// Token: 0x040002D2 RID: 722
			public Vector4 Color;

			// Token: 0x040002D3 RID: 723
			public Vector3 Normal;

			// Token: 0x040002D4 RID: 724
			public CCSModel.BoneID BoneIDs;

			// Token: 0x040002D5 RID: 725
			public Vector4 Weights;
		}

		// Token: 0x02000064 RID: 100
		public struct ModelTriangle
		{
			// Token: 0x06000233 RID: 563 RVA: 0x00018B2B File Offset: 0x00016D2B
			public ModelTriangle(int _id1, int _id2, int _id3)
			{
				this.ID1 = _id1;
				this.ID2 = _id2;
				this.ID3 = _id3;
			}

			// Token: 0x040002D6 RID: 726
			public int ID1;

			// Token: 0x040002D7 RID: 727
			public int ID2;

			// Token: 0x040002D8 RID: 728
			public int ID3;
		}

		// Token: 0x02000065 RID: 101
		public class SubModel
		{
			// Token: 0x040002D9 RID: 729
			public int ParentID;

			// Token: 0x040002DA RID: 730
			public int VertexCount;

			// Token: 0x040002DB RID: 731
			public int TriangleCount;

			// Token: 0x040002DC RID: 732
			public int MatTexID;

			// Token: 0x040002DD RID: 733
			public CCSModel.ModelVertex[] Vertices;

			// Token: 0x040002DE RID: 734
			public CCSModel.ModelTriangle[] Triangles;

			// Token: 0x040002DF RID: 735
			public int VertexArrayID = -1;

			// Token: 0x040002E0 RID: 736
			public int VertexBufferID = -1;

			// Token: 0x040002E1 RID: 737
			public int ElementArrayID = -1;

			// Token: 0x040002E2 RID: 738
			public CCSObject ParentObjectRef;

			// Token: 0x040002E3 RID: 739
			public CCSTexture ParentTextureRef;

			// Token: 0x040002E4 RID: 740
			public CCSMaterial ParentMaterialRef;
		}
	}
}
