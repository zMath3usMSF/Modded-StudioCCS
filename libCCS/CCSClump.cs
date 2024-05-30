using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace StudioCCS.libCCS
{
	// Token: 0x02000019 RID: 25
	public class CCSClump : CCSBaseObject
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x0000C99C File Offset: 0x0000AB9C
		public CCSClump(int _objectID, CCSFile _parentFile)
		{
			this.ParentFile = _parentFile;
			this.ObjectID = _objectID;
			this.ObjectType = 2304;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000CA48 File Offset: 0x0000AC48
		public override bool DeInit()
		{
			for (int _nodeID = 0; _nodeID < this.NodeCount; _nodeID++)
			{
				this.GetObject(_nodeID).DeInit();
			}
			GL.DeleteBuffer(this.MatrixBuffer);
			this.MatrixBuffer = -1;
			GL.DeleteTexture(this.MatrixTexture);
			this.MatrixTexture = -1;
			GL.DeleteBuffer(this.BoneVisVBO);
			GL.DeleteVertexArray(this.BoneVisVAO);
			this.BoneVisVBO = -1;
			this.BoneVisVAO = -1;
			CCSClump.ProgramRefs--;
			bool flag = CCSClump.ProgramRefs <= 0;
			if (flag)
			{
				bool flag2 = CCSClump.ProgramID != -1;
				if (flag2)
				{
					GL.DeleteProgram(CCSClump.ProgramID);
				}
				CCSClump.ProgramID = -1;
			}
			return true;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000CB08 File Offset: 0x0000AD08
		public override bool Init()
		{
			bool flag = CCSClump.ProgramID == -1;
			if (flag)
			{
				CCSClump.ProgramID = Scene.LoadProgram("CCSClump", true);
				bool flag2 = CCSClump.ProgramID == -1;
				if (flag2)
				{
					Logger.LogError("Error loading Shader program for CCSClump\n", Logger.LogType.LogAll, "Init", 163);
					return false;
				}
				CCSClump.AttribEndpoints = GL.GetAttribLocation(CCSClump.ProgramID, "vEndpoints");
				bool flag3 = CCSClump.AttribEndpoints == -1;
				if (flag3)
				{
					Logger.LogError("Error getting CCSClump Shader Attributes:\n", Logger.LogType.LogAll, "Init", 171);
					Logger.LogError(string.Format("\tEndpoints: {0}", CCSClump.AttribEndpoints), Logger.LogType.LogAll, "Init", 172);
					return false;
				}
				CCSClump.UniformMatrix = GL.GetUniformLocation(CCSClump.ProgramID, "uMatrix");
				CCSClump.UniformSelectionID = GL.GetUniformLocation(CCSClump.ProgramID, "selectedID");
				CCSClump.UniformMatrixList = GL.GetUniformLocation(CCSClump.ProgramID, "uMatrixList");
				bool flag4 = CCSClump.UniformMatrix == -1 || CCSClump.UniformSelectionID == -1 || CCSClump.UniformMatrix == -1;
				if (flag4)
				{
					Logger.LogError("Error getting CCSClump Shader Uniforms:\n", Logger.LogType.LogAll, "Init", 182);
					Logger.LogError(string.Format("\tMatrix: {0}, SelectionID: {1}, UniformMatrix: {2}", CCSClump.UniformMatrix, CCSClump.UniformSelectionID, CCSClump.UniformMatrixList), Logger.LogType.LogAll, "Init", 183);
					return false;
				}
			}
			CCSClump.ProgramRefs++;
			this.BoneVisVAO = GL.GenVertexArray();
			GL.BindVertexArray(this.BoneVisVAO);
			this.BoneVisVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.BoneVisVBO);
			GL.EnableVertexAttribArray(CCSClump.AttribEndpoints);
			GL.VertexAttribIPointer(CCSClump.AttribEndpoints, 2, VertexAttribIntegerType.Int, Marshal.SizeOf(this.BoneVisBones[0].GetType()), Marshal.OffsetOf(this.BoneVisBones[0].GetType(), "HeadID"));
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			this.MatrixBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.TextureBuffer, this.MatrixBuffer);
			this.MatrixTexture = GL.GenTexture();
			GL.BindTexture(TextureTarget.TextureBuffer, this.MatrixTexture);
			GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.Rgba32f, this.MatrixBuffer);
			GL.BufferData<Matrix4>(BufferTarget.TextureBuffer, Marshal.SizeOf(this.FinalMatrixList[0].GetType()) * this.NodeCount, this.FinalMatrixList, BufferUsageHint.DynamicDraw);
			GL.BindTexture(TextureTarget.TextureBuffer, 0);
			for (int _nodeID = 0; _nodeID < this.NodeCount; _nodeID++)
			{
				int subObjectType = this.ParentFile.GetSubObjectType(this.NodeIDs[_nodeID]);
				int num = subObjectType;
				if (num != 256)
				{
					if (num == 3584)
					{
						CCSEffect ccsEffect = this.ParentFile.GetObject<CCSEffect>(this.NodeIDs[_nodeID]);
						bool flag5 = ccsEffect != null;
						if (!flag5)
						{
							Logger.LogError(string.Format("Clump {0:X}: {1} Init: could not get child Effect {2:X} {3}", new object[]
							{
								this.ObjectID,
								this.ParentFile.GetSubObjectName(this.ObjectID),
								this.NodeIDs[_nodeID],
								this.ParentFile.GetSubObjectName(this.NodeIDs[_nodeID])
							}), Logger.LogType.LogAll, "Init", 250);
							return false;
						}
						ccsEffect.Init();
					}
				}
				else
				{
					CCSObject ccsObject = this.ParentFile.GetObject<CCSObject>(this.NodeIDs[_nodeID]);
					bool flag6 = ccsObject != null;
					if (flag6)
					{
						ccsObject.SetParentClump(this, _nodeID);
						ccsObject.Init();
						bool flag7 = ccsObject.ParentObjectID != 0;
						if (flag7)
						{
							CCSObject ccsObject2 = this.ParentFile.GetObject<CCSObject>(ccsObject.ParentObjectID);
							bool flag8 = ccsObject2 != null;
							if (flag8)
							{
								CCSClump.BoneVis boneVisBone = this.BoneVisBones[ccsObject2.NodeID];
								bool flag9 = boneVisBone.TailID == boneVisBone.HeadID;
								if (flag9)
								{
									this.BoneVisBones[ccsObject2.NodeID].TailID = _nodeID;
								}
							}
						}
					}
				}
			}
			GL.BindVertexArray(this.BoneVisVAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.BoneVisVBO);
			GL.BufferData<CCSClump.BoneVis>(BufferTarget.ArrayBuffer, 8 * this.NodeCount, this.BoneVisBones, BufferUsageHint.StaticDraw);
			GL.BindVertexArray(0);
			return true;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000CFA8 File Offset: 0x0000B1A8
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.NodeCount = bStream.ReadInt32();
			this.NodeIDs = new int[this.NodeCount];
			this.PoseMatrixList = new Matrix4[this.NodeCount];
			this.FinalMatrixList = new Matrix4[this.NodeCount];
			this.BindPositions = new Vector3[this.NodeCount];
			this.BindRotations = new Vector3[this.NodeCount];
			this.BindScales = new Vector3[this.NodeCount];
			this.PosePositions = new Vector3[this.NodeCount];
			this.BoneVisBones = new CCSClump.BoneVis[this.NodeCount];
			bool flag = this.ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
			if (flag)
			{
				this.PoseRotations = new Vector3[this.NodeCount];
			}
			else
			{
				this.PoseQuats = new Quaternion[this.NodeCount];
			}
			this.PoseScales = new Vector3[this.NodeCount];
			for (int index = 0; index < this.NodeCount; index++)
			{
				this.NodeIDs[index] = bStream.ReadInt32();
			}
			bool flag2 = this.ParentFile.GetVersion() > CCSFileHeader.CCSVersion.Gen1;
			if (flag2)
			{
				for (int index2 = 0; index2 < this.NodeCount; index2++)
				{
					Vector3 vector3_ = Util.ReadVec3Position(bStream);
					Vector3 vector3_2 = Util.ReadVec3Rotation(bStream);
					Vector3 vector3_3 = Util.ReadVec3Scale(bStream);
					this.BindPositions[index2] = vector3_;
					this.BindRotations[index2] = vector3_2;
					this.BindScales[index2] = vector3_3;
					this.PosePositions[index2] = Vector3.Zero;
					this.PoseQuats[index2] = Quaternion.Identity;
					this.PoseScales[index2] = Vector3.One;
					this.BoneVisBones[index2].HeadID = index2;
					this.BoneVisBones[index2].TailID = index2;
				}
			}
			else
			{
				for (int index3 = 0; index3 < this.NodeCount; index3++)
				{
					this.BindPositions[index3] = Vector3.Zero;
					this.PosePositions[index3] = Vector3.Zero;
					this.BindRotations[index3] = Vector3.Zero;
					this.PoseRotations[index3] = Vector3.Zero;
					this.BindScales[index3] = Vector3.One;
					this.PoseScales[index3] = Vector3.One;
					this.BoneVisBones[index3].HeadID = index3;
					this.BoneVisBones[index3].TailID = index3;
				}
			}
			return true;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000D258 File Offset: 0x0000B458
		public void SetPose(int _nodeID, Vector3 Positon, Vector3 Rotation, Vector3 Scale)
		{
			bool flag = _nodeID >= this.NodeCount;
			if (!flag)
			{
				this.PosePositions[_nodeID] = Positon;
				this.PoseRotations[_nodeID] = Rotation;
				this.PoseScales[_nodeID] = Scale;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		public void SetPose(int _nodeID, Vector3 Position, Quaternion Rotation, Vector3 Scale)
		{
			bool flag = _nodeID >= this.NodeCount;
			if (!flag)
			{
				this.PosePositions[_nodeID] = Position;
				this.PoseQuats[_nodeID] = Rotation;
				this.PoseScales[_nodeID] = Scale;
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000D2E8 File Offset: 0x0000B4E8
		public Matrix4 GetPoseMatrix(int _nodeID)
		{
			bool flag = _nodeID >= this.NodeCount;
			Matrix4 result;
			if (flag)
			{
				result = Matrix4.Identity;
			}
			else
			{
				bool flag2 = this.ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
				if (flag2)
				{
					this.PoseMatrixList[_nodeID] = Matrix4.CreateScale(this.PoseScales[_nodeID]) * Matrix4.CreateFromQuaternion(new Quaternion(this.PoseRotations[_nodeID])) * Matrix4.CreateTranslation(this.PosePositions[_nodeID]);
					result = this.PoseMatrixList[_nodeID];
				}
				else
				{
					this.PoseMatrixList[_nodeID] = Matrix4.CreateScale(this.PoseScales[_nodeID]) * Matrix4.CreateFromQuaternion(this.PoseQuats[_nodeID]) * Matrix4.CreateTranslation(this.PosePositions[_nodeID]);
					this.PoseMatrixList[_nodeID] *= Matrix4.CreateScale(this.BindScales[_nodeID]) * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(this.BindRotations[_nodeID])) * Matrix4.CreateTranslation(this.BindPositions[_nodeID]);
					result = this.PoseMatrixList[_nodeID];
				}
			}
			return result;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000D43C File Offset: 0x0000B63C
		public void SetFinalMatrix(int _nodeID, Matrix4 _matrix)
		{
			bool flag = _nodeID >= this.NodeCount;
			if (!flag)
			{
				this.FinalMatrixList[_nodeID] = _matrix;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000D46C File Offset: 0x0000B66C
		public Matrix4 GetFinalMatrix(int _nodeID)
		{
			bool flag = _nodeID < this.NodeCount;
			Matrix4 result;
			if (flag)
			{
				result = this.FinalMatrixList[_nodeID];
			}
			else
			{
				Logger.LogWarning(string.Format("Clump {0}: GetFinalMatrix({1}), Invalid NodeID", this.ParentFile.GetSubObjectName(this.ObjectID), _nodeID), Logger.LogType.LogOnceValue, "GetFinalMatrix", 436);
				result = Matrix4.Identity;
			}
			return result;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000D4D1 File Offset: 0x0000B6D1
		public CCSObject GetObject(int _nodeID)
		{
			return (_nodeID < this.NodeCount) ? this.ParentFile.GetObject<CCSObject>(this.NodeIDs[_nodeID]) : null;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		public void BindMatrixList()
		{
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.TextureBuffer, this.MatrixTexture);
			GL.BindBuffer(BufferTarget.TextureBuffer, this.MatrixBuffer);
			GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.Rgba32f, this.MatrixBuffer);
			GL.BufferData<Matrix4>(BufferTarget.TextureBuffer, Marshal.SizeOf(this.FinalMatrixList[0].GetType()) * this.NodeCount, this.FinalMatrixList, BufferUsageHint.DynamicDraw);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000D57D File Offset: 0x0000B77D
		public void UpdateMatrixList()
		{
			GL.BufferData<Matrix4>(BufferTarget.TextureBuffer, Marshal.SizeOf(this.FinalMatrixList[0].GetType()) * this.NodeCount, this.FinalMatrixList, BufferUsageHint.DynamicDraw);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000D5B8 File Offset: 0x0000B7B8
		public void Render(Matrix4 projView, int extraOptions = 0)
		{
			this.FrameForward();
			this.BindMatrixList();
			for (int index = 0; index < this.NodeCount; index++)
			{
				bool flag = this.ParentFile.GetSubObjectType(this.NodeIDs[index]) == 256;
				if (flag)
				{
					CCSObject @object = this.ParentFile.GetObject<CCSObject>(this.NodeIDs[index]);
					if (@object != null)
					{
						@object.Render(projView, extraOptions);
					}
				}
			}
			bool flag2 = !this.RenderBones;
			if (!flag2)
			{
				GL.UseProgram(CCSClump.ProgramID);
				GL.BindVertexArray(this.BoneVisVAO);
				GL.Uniform1(CCSClump.UniformSelectionID, this.SelectedBoneID);
				GL.UniformMatrix4(CCSClump.UniformMatrix, false, ref projView);
				GL.Uniform1(CCSClump.UniformMatrixList, 1);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				GL.DrawArrays(PrimitiveType.Points, 0, this.NodeCount);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.UseProgram(0);
				GL.BindVertexArray(0);
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000D6B8 File Offset: 0x0000B8B8
		public override TreeNode ToNode()
		{
			TreeNode node = base.ToNode();
			List<TreeNode> treeNodeList = new List<TreeNode>();
			for (int index = 0; index < this.NodeCount; index++)
			{
				CCSObject ccsObject = this.ParentFile.GetObject<CCSObject>(this.NodeIDs[index]);
				TreeNode node2 = (ccsObject == null) ? Util.NonExistantNode(this.ParentFile, this.NodeIDs[index]) : ccsObject.ToNode();
				treeNodeList.Add(node2);
				int index2 = (ccsObject != null) ? this.SearchNodeID(ccsObject.ParentObjectID) : -1;
				bool flag = index2 == -1;
				if (flag)
				{
					node.Nodes.Add(node2);
				}
				else
				{
					treeNodeList[index2].Nodes.Add(node2);
				}
			}
			return node;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000D77C File Offset: 0x0000B97C
		public string GetReport(int level = 0)
		{
			string str = Util.Indent(level, false) + string.Format("Clump: 0x{0:X}: {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID)) + Util.Indent(level, false) + string.Format("0x{0:X}({0}) Nodes\n", this.NodeCount);
			for (int _nodeID = 0; _nodeID < this.NodeCount; _nodeID++)
			{
				int subObjectType = this.ParentFile.GetSubObjectType(this.NodeIDs[_nodeID]);
				int num = subObjectType;
				if (num != 256)
				{
					if (num == 3584)
					{
						str = string.Concat(new string[]
						{
							str,
							Util.Indent(level + 2, false),
							string.Format("Effect 0x{0:X}: {1}\n", this.NodeIDs[_nodeID], this.ParentFile.GetSubObjectName(this.NodeIDs[_nodeID])),
							Util.Indent(level + 3, false),
							string.Format("No Information currently Available\n", Array.Empty<object>())
						});
					}
				}
				else
				{
					bool flag = this.ParentFile.GetVersion() > CCSFileHeader.CCSVersion.Gen1;
					if (flag)
					{
						Vector3 bindPosition = this.BindPositions[_nodeID];
						Vector3 bindRotation = this.BindRotations[_nodeID];
						Vector3 bindScale = this.BindScales[_nodeID];
						str = string.Concat(new string[]
						{
							str,
							Util.Indent(level + 1, false),
							string.Format("Node {0}\n", _nodeID),
							Util.Indent(level + 1, false),
							string.Format("{0}, {1}, {2}\n", bindPosition.X, bindPosition.Y, bindPosition.Z),
							Util.Indent(level + 1, false),
							string.Format("{0}, {1}, {2}\n", bindRotation.X, bindRotation.Y, bindRotation.Z),
							Util.Indent(level + 1, false),
							string.Format("{0}, {1}, {2}\n", bindScale.X, bindScale.Y, bindScale.Z)
						});
					}
					str += this.GetObject(_nodeID).GetReport(level + 1);
				}
			}
			return str + "\n";
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000D9E4 File Offset: 0x0000BBE4
		public void FrameForward()
		{
			for (int _nodeID = 0; _nodeID < this.NodeCount; _nodeID++)
			{
				CCSObject @object = this.GetObject(_nodeID);
				if (@object != null)
				{
					@object.FrameForward();
				}
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000DA1C File Offset: 0x0000BC1C
		public int DumpToObj(StreamWriter fStream, int vOffset, bool split, bool withNormals)
		{
			this.FrameForward();
			int vOffset2 = vOffset;
			for (int _nodeID = 0; _nodeID < this.NodeCount; _nodeID++)
			{
				CCSObject ccsObject = this.GetObject(_nodeID);
				bool flag = ccsObject != null;
				if (flag)
				{
					vOffset2 = ccsObject.DumpToObj(fStream, vOffset2, split, withNormals);
				}
			}
			return vOffset2;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000DA70 File Offset: 0x0000BC70
		public void DumpToSMD(string outputPath, bool withNormals)
		{
			this.FrameForward();
			string path = Path.Combine(outputPath, this.ParentFile.GetSubObjectName(this.ObjectID)) + ".smd";
			using (FileStream fileStream = new FileStream(path, FileMode.Truncate))
			{
				using (StreamWriter outf = new StreamWriter(fileStream))
				{
					Logger.LogInfo(string.Format("Dumping {0} to {1}...\n", this.ParentFile.GetSubObjectName(this.ObjectID), path), Logger.LogType.LogAll, "DumpToSMD", 630);
					outf.WriteLine("version 1");
					outf.WriteLine("nodes");
					for (int _nodeID = 0; _nodeID < this.NodeCount; _nodeID++)
					{
						string str = this.ParentFile.GetSubObjectName(this.NodeIDs[_nodeID]).Replace(" ", "_");
						int num = this.SearchNodeID(this.GetObject(_nodeID).ParentObjectID);
						outf.WriteLine(string.Format("{0} \"{1}\" {2}", _nodeID, str, num));
					}
					outf.WriteLine("end");
					outf.WriteLine("skeleton");
					outf.WriteLine("time 0");
					for (int index = 0; index < this.NodeCount; index++)
					{
						string str2 = string.Format("{0} {1} {2}", this.BindPositions[index].X, this.BindPositions[index].Y, this.BindPositions[index].Z);
						string str3 = string.Format("{0} {1} {2}", this.BindRotations[index].X, this.BindRotations[index].Y, (float)(-(float)((double)this.BindRotations[index].Z)));
						outf.WriteLine(string.Format("{0} {1} {2}", index, str2, str3));
					}
					outf.WriteLine("end");
					outf.WriteLine("triangles");
					for (int _nodeID2 = 0; _nodeID2 < this.NodeCount; _nodeID2++)
					{
						this.GetObject(_nodeID2).DumpToSMD(outf, withNormals);
					}
					outf.WriteLine("end");
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000DD18 File Offset: 0x0000BF18
		public int SearchNodeID(int _objectID)
		{
			for (int index = 0; index < this.NodeCount; index++)
			{
				bool flag = this.NodeIDs[index] == _objectID;
				if (flag)
				{
					return index;
				}
			}
			return -1;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000DD58 File Offset: 0x0000BF58
		public void LoadMatrixList(string fileName)
		{
			using (FileStream input = new FileStream(fileName, FileMode.Open))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					for (int index = 0; index < this.NodeCount; index++)
					{
						float num = 0.00625f;
						Matrix4 matrix4 = new Matrix4(new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()), new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()), new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()), new Vector4(binaryReader.ReadSingle() * num, binaryReader.ReadSingle() * num, binaryReader.ReadSingle() * num, binaryReader.ReadSingle()));
						matrix4.Invert();
						this.FinalMatrixList[index] = matrix4;
					}
				}
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000DE6C File Offset: 0x0000C06C
		public void LoadPose(string fileName)
		{
			using (FileStream input = new FileStream(fileName, FileMode.Open))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					for (int index = 0; index < this.NodeCount; index++)
					{
						Vector3 vector3_ = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
						Vector3 vector3_2 = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
						Vector3 vector3_3 = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
						bool flag = this.ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
						if (flag)
						{
							this.PosePositions[index] = vector3_;
							this.PoseRotations[index] = vector3_2;
							this.PoseScales[index] = vector3_3;
						}
						else
						{
							this.BindPositions[index] = vector3_;
							this.BindRotations[index] = vector3_2;
							this.BindScales[index] = vector3_3;
						}
					}
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000DF9C File Offset: 0x0000C19C
		public void SavePose(string fileName)
		{
			using (FileStream output = new FileStream(fileName, FileMode.OpenOrCreate))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(output))
				{
					for (int index = 0; index < this.NodeCount; index++)
					{
						Vector3 vector3_ = this.BindPositions[index];
						Vector3 vector3_2 = this.BindRotations[index];
						Vector3 vector3_3 = this.BindScales[index];
						bool flag = this.ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
						if (flag)
						{
							vector3_ = this.PosePositions[index];
							vector3_2 = this.PoseRotations[index];
							vector3_3 = this.PoseScales[index];
						}
						binaryWriter.Write(vector3_.X);
						binaryWriter.Write(vector3_.Y);
						binaryWriter.Write(vector3_.Z);
						binaryWriter.Write(vector3_2.X);
						binaryWriter.Write(vector3_2.Y);
						binaryWriter.Write(vector3_2.Z);
						binaryWriter.Write(vector3_3.X);
						binaryWriter.Write(vector3_3.Y);
						binaryWriter.Write(vector3_3.Z);
					}
				}
			}
		}

		// Token: 0x04000140 RID: 320
		public Matrix4[] PoseMatrixList = null;

		// Token: 0x04000141 RID: 321
		public Matrix4[] FinalMatrixList = null;

		// Token: 0x04000142 RID: 322
		public int[] NodeIDs = null;

		// Token: 0x04000143 RID: 323
		public int NodeCount = 0;

		// Token: 0x04000144 RID: 324
		public Vector3[] BindPositions = null;

		// Token: 0x04000145 RID: 325
		public Vector3[] BindRotations = null;

		// Token: 0x04000146 RID: 326
		public Vector3[] BindScales = null;

		// Token: 0x04000147 RID: 327
		public Vector3[] PosePositions = null;

		// Token: 0x04000148 RID: 328
		public Vector3[] PoseRotations = null;

		// Token: 0x04000149 RID: 329
		public Quaternion[] PoseQuats = null;

		// Token: 0x0400014A RID: 330
		public Vector3[] PoseScales = null;

		// Token: 0x0400014B RID: 331
		public int MatrixBuffer = -1;

		// Token: 0x0400014C RID: 332
		public int MatrixTexture = -1;

		// Token: 0x0400014D RID: 333
		public static int ProgramID = -1;

		// Token: 0x0400014E RID: 334
		public static int ProgramRefs = 0;

		// Token: 0x0400014F RID: 335
		public static int AttribEndpoints = -1;

		// Token: 0x04000150 RID: 336
		public static int UniformMatrix = -1;

		// Token: 0x04000151 RID: 337
		public static int UniformSelectionID = -1;

		// Token: 0x04000152 RID: 338
		public static int UniformMatrixList = -1;

		// Token: 0x04000153 RID: 339
		public int BoneVisVAO = -1;

		// Token: 0x04000154 RID: 340
		public int BoneVisVBO = -1;

		// Token: 0x04000155 RID: 341
		public CCSClump.BoneVis[] BoneVisBones = null;

		// Token: 0x04000156 RID: 342
		public bool RenderBones = false;

		// Token: 0x04000157 RID: 343
		public int SelectedBoneID = -1;

		// Token: 0x02000060 RID: 96
		public struct BoneVis
		{
			// Token: 0x040002C0 RID: 704
			public int HeadID;

			// Token: 0x040002C1 RID: 705
			public int TailID;
		}
	}
}
