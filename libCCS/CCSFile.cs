using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace StudioCCS.libCCS
{
	// Token: 0x02000020 RID: 32
	public class CCSFile
	{
		// Token: 0x060000FC RID: 252 RVA: 0x0000E594 File Offset: 0x0000C794
		~CCSFile()
		{
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000E5C0 File Offset: 0x0000C7C0
		public bool Init()
		{
			foreach (CCSBaseObject hit in this.HitList)
			{
				bool flag = !hit.Init();
				if (flag)
				{
					return false;
				}
			}
			foreach (CCSBaseObject clump in this.ClumpList)
			{
				bool flag2 = !clump.Init();
				if (flag2)
				{
					return false;
				}
			}
			foreach (CCSBaseObject texture in this.TextureList)
			{
				bool flag3 = !texture.Init();
				if (flag3)
				{
					return false;
				}
			}
			this.NeedsInit = false;
			return true;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000E6DC File Offset: 0x0000C8DC
		public bool DeInit()
		{
			foreach (CCSBaseObject hit in this.HitList)
			{
				hit.DeInit();
			}
			return true;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000E738 File Offset: 0x0000C938
		public bool Read(string _fileName)
		{
			this.FileName = _fileName;
			BinaryReader bStream = new BinaryReader(new FileStream(_fileName, FileMode.Open, FileAccess.Read));
			Logger.LogInfo(string.Format("Loading {0}...\n", _fileName), Logger.LogType.LogAll, "Read", 163);
			int num = this.Read(bStream) ? 1 : 0;
			Logger.LogInfo("Done.\n", Logger.LogType.LogAll, "Read", 165);
			return num != 0;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000E7A4 File Offset: 0x0000C9A4
		public bool Read(BinaryReader bStream)
		{
			string str = string.Format("Error reading CCS File {0}, ", this.FileName);
			bool flag2 = !this.Header.Read(bStream);
			bool flag;
			if (flag2)
			{
				Logger.LogError(str + "Header section could not be read\n", Logger.LogType.LogAll, "Read", 174);
				flag = false;
			}
			else
			{
				bool flag3 = !this.ReadIndexSection(bStream);
				if (flag3)
				{
					Logger.LogError(str + "Index section could not be read\n", Logger.LogType.LogAll, "Read", 180);
					flag = false;
				}
				else
				{
					bool flag4 = !this.ReadSetupSection(bStream);
					if (flag4)
					{
						Logger.LogError(str + "Setup section could not be read\n", Logger.LogType.LogAll, "Read", 186);
						flag = false;
					}
					else
					{
						bool flag5 = !this.ReadStreamSection(bStream);
						if (flag5)
						{
							Logger.LogError(str + "Stream section could not be read\n", Logger.LogType.LogAll, "Read", 192);
							flag = false;
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000E898 File Offset: 0x0000CA98
		public TreeNode ToNode()
		{
			TreeNode node = new TreeNode(this.Header.CCSFName)
			{
				Tag = new TreeNodeTag(this, 0, 0, TreeNodeTag.NodeType.File, 0)
			};
			TreeNode node2 = new TreeNode("Clumps");
			foreach (CCSClump clump in this.ClumpList)
			{
				node2.Nodes.Add(clump.ToNode());
			}
			node.Nodes.Add(node2);
			TreeNode node3 = new TreeNode("Materials");
			foreach (CCSMaterial material in this.MaterialList)
			{
				node3.Nodes.Add(material.ToNode());
			}
			node.Nodes.Add(node3);
			TreeNode node4 = new TreeNode("Textures");
			foreach (CCSTexture texture in this.TextureList)
			{
				node4.Nodes.Add(texture.ToNode());
			}
			node.Nodes.Add(node4);
			TreeNode node5 = new TreeNode("HitMeshes");
			foreach (CCSHitMesh hit in this.HitList)
			{
				node5.Nodes.Add(hit.ToNode());
			}
			node.Nodes.Add(node5);
			TreeNode node6 = new TreeNode("Bounding Boxes");
			foreach (CCSBoundingBox bbox in this.BBoxList)
			{
				node6.Nodes.Add(bbox.ToNode());
			}
			node.Nodes.Add(node6);
			TreeNode node7 = new TreeNode("Dummies");
			foreach (CCSDummy dummy in this.DummyList)
			{
				node7.Nodes.Add(dummy.ToNode());
			}
			node.Nodes.Add(node7);
			TreeNode node8 = new TreeNode("Animations");
			foreach (CCSAnime anime in this.AnimeList)
			{
				node8.Nodes.Add(anime.ToNode());
			}
			node.Nodes.Add(node8);
			return node;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000EBC8 File Offset: 0x0000CDC8
		public CCSFileHeader.CCSVersion GetVersion()
		{
			return this.Header.GetVersionType();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000EBD5 File Offset: 0x0000CDD5
		public string GetVersionString()
		{
			return this.Header.GetVersionString(false);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000EBE4 File Offset: 0x0000CDE4
		private bool ReadIndexSection(BinaryReader bStream)
		{
			bool flag2 = (bStream.ReadInt32() & 65535) != 2;
			bool flag;
			if (flag2)
			{
				Logger.LogError("Index section mismatch\n", Logger.LogType.LogAll, "ReadIndexSection", 275);
				flag = false;
			}
			else
			{
				bStream.ReadInt32();
				this.FileIndexCount = bStream.ReadInt32();
				this.ObjectIndexCount = bStream.ReadInt32();
				this.FileIndex = new IndexFileEntry[this.FileIndexCount];
				for (int index = 0; index < this.FileIndexCount; index++)
				{
					IndexFileEntry indexFileEntry = new IndexFileEntry();
					indexFileEntry.Read(bStream);
					this.FileIndex[index] = indexFileEntry;
				}
				this.ObjectIndex = new IndexObjectEntry[this.ObjectIndexCount];
				for (int index2 = 0; index2 < this.ObjectIndexCount; index2++)
				{
					IndexObjectEntry indexObjectEntry = new IndexObjectEntry();
					indexObjectEntry.Read(bStream);
					this.ObjectIndex[index2] = indexObjectEntry;
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		private bool ReadSetupSection(BinaryReader bStream)
		{
			bool flag2 = (bStream.ReadInt32() & 65535) == 3;
			bool result;
			if (flag2)
			{
				bStream.ReadInt32();
				int position;
				int num;
				int sectionSize;
				int _objectID;
				for (;;)
				{
					position = (int)bStream.BaseStream.Position;
					num = (bStream.ReadInt32() & 65535);
					bool flag3 = num != 5;
					if (!flag3)
					{
						goto IL_6BC;
					}
					sectionSize = bStream.ReadInt32() - 1;
					_objectID = bStream.ReadInt32();
					bool flag4 = _objectID <= this.ObjectIndexCount;
					if (flag4)
					{
						bool flag = true;
						int num2 = num;
						int num3 = num2;
						if (num3 <= 3072)
						{
							if (num3 <= 1536)
							{
								if (num3 <= 768)
								{
									if (num3 != 256)
									{
										if (num3 != 512)
										{
											if (num3 != 768)
											{
												goto IL_676;
											}
											CCSTexture _objectRef3 = new CCSTexture(_objectID, this);
											flag = _objectRef3.Read(bStream, sectionSize);
											this.SetObjectRef(_objectID, num, position, _objectRef3);
											this.TextureList.Add(_objectRef3);
										}
										else
										{
											CCSMaterial _objectRef4 = new CCSMaterial(_objectID, this);
											flag = _objectRef4.Read(bStream, sectionSize);
											this.SetObjectRef(_objectID, num, position, _objectRef4);
											this.MaterialList.Add(_objectRef4);
										}
									}
									else
									{
										CCSObject _objectRef5 = new CCSObject(_objectID, this);
										flag = _objectRef5.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef5);
										this.LastObject = _objectRef5;
									}
								}
								else if (num3 != 1024)
								{
									if (num3 != 1280)
									{
										if (num3 != 1536)
										{
											goto IL_676;
										}
										CCSLight _objectRef6 = new CCSLight(_objectID, this);
										flag = _objectRef6.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef6);
									}
									else
									{
										CCSCamera _objectRef7 = new CCSCamera(_objectID, this);
										flag = _objectRef7.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef7);
									}
								}
								else
								{
									CCSClut _objectRef8 = new CCSClut(_objectID, this);
									flag = _objectRef8.Read(bStream, sectionSize);
									this.SetObjectRef(_objectID, num, position, _objectRef8);
								}
							}
							else if (num3 <= 2304)
							{
								if (num3 != 1792)
								{
									if (num3 != 2048)
									{
										if (num3 != 2304)
										{
											goto IL_676;
										}
										CCSClump _objectRef9 = new CCSClump(_objectID, this);
										flag = _objectRef9.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef9);
										this.ClumpList.Add(_objectRef9);
										this.LastClump = _objectRef9;
									}
									else
									{
										CCSModel _objectRef10 = new CCSModel(_objectID, this);
										flag = _objectRef10.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef10);
									}
								}
								else
								{
									CCSAnime _objectRef11 = new CCSAnime(_objectID, this);
									flag = _objectRef11.Read(bStream, sectionSize);
									this.SetObjectRef(_objectID, num, position, _objectRef11);
									this.AnimeList.Add(_objectRef11);
								}
							}
							else if (num3 != 2560)
							{
								if (num3 != 2816)
								{
									if (num3 != 3072)
									{
										goto IL_676;
									}
									CCSBoundingBox _objectRef12 = new CCSBoundingBox(_objectID, this);
									flag = _objectRef12.Read(bStream, sectionSize);
									this.SetObjectRef(_objectID, num, position, _objectRef12);
									this.BBoxList.Add(_objectRef12);
								}
								else
								{
									CCSHitMesh _objectRef13 = new CCSHitMesh(_objectID, this);
									flag = _objectRef13.Read(bStream, sectionSize);
									this.SetObjectRef(_objectID, num, position, _objectRef13);
									this.HitList.Add(_objectRef13);
								}
							}
							else
							{
								CCSExt _objectRef14 = new CCSExt(_objectID, this);
								flag = _objectRef14.Read(bStream, sectionSize);
								this.SetObjectRef(_objectID, num, position, _objectRef14);
							}
						}
						else
						{
							if (num3 <= 4864)
							{
								if (num3 <= 4096)
								{
									if (num3 == 3328)
									{
										CCSParticle _objectRef15 = new CCSParticle(_objectID, this);
										flag = _objectRef15.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef15);
										goto IL_6B1;
									}
									if (num3 == 3584)
									{
										CCSEffect _objectRef16 = new CCSEffect(_objectID, this);
										flag = _objectRef16.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef16);
										goto IL_6B1;
									}
									if (num3 != 4096)
									{
										goto IL_676;
									}
									CCSBlitGroup _objectRef17 = new CCSBlitGroup(_objectID, this);
									flag = _objectRef17.Read(bStream, sectionSize);
									this.SetObjectRef(_objectID, num, position, _objectRef17);
									goto IL_6B1;
								}
								else
								{
									if (num3 == 4352)
									{
										CCSFBPage _objectRef18 = new CCSFBPage(_objectID, this);
										flag = _objectRef18.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef18);
										goto IL_6B1;
									}
									if (num3 == 4608)
									{
										CCSFBRect _objectRef19 = new CCSFBRect(_objectID, this);
										flag = _objectRef19.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef19);
										goto IL_6B1;
									}
									if (num3 != 4864)
									{
										goto IL_676;
									}
								}
							}
							else if (num3 <= 6144)
							{
								if (num3 != 5120)
								{
									if (num3 == 5888)
									{
										CCSLayer _objectRef20 = new CCSLayer(_objectID, this);
										flag = _objectRef20.Read(bStream, sectionSize);
										this.SetObjectRef(_objectID, num, position, _objectRef20);
										goto IL_6B1;
									}
									if (num3 != 6144)
									{
										goto IL_676;
									}
									CCSShadow _objectRef21 = new CCSShadow(_objectID, this);
									flag = _objectRef21.Read(bStream, sectionSize);
									this.SetObjectRef(_objectID, num, position, _objectRef21);
									goto IL_6B1;
								}
							}
							else if (num3 <= 8192)
							{
								if (num3 == 6400)
								{
									CCSMorpher _objectRef22 = new CCSMorpher(_objectID, this);
									flag = _objectRef22.Read(bStream, sectionSize);
									this.SetObjectRef(_objectID, num, position, _objectRef22);
									goto IL_6B1;
								}
								if (num3 != 8192)
								{
									goto IL_676;
								}
								Util.SkipSection(bStream, sectionSize);
								goto IL_6B1;
							}
							else
							{
								if (num3 == 8704)
								{
									CCSPCM _objectRef23 = new CCSPCM(_objectID, this);
									flag = _objectRef23.Read(bStream, sectionSize);
									this.SetObjectRef(_objectID, num, position, _objectRef23);
									goto IL_6B1;
								}
								if (num3 != 9216)
								{
									goto IL_676;
								}
								bool flag5 = this.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
								if (flag5)
								{
									Logger.LogWarning(string.Format("Binary Blob (Type 0x2400) with ID of {0} found in Generation 1 CCS File at 0x{1:X}, File may fail in game!\n", _objectID, position), Logger.LogType.LogAll, "ReadSetupSection", 512);
								}
								CCSBinaryBlob _objectRef24 = new CCSBinaryBlob(_objectID, this);
								flag = _objectRef24.Read(bStream, sectionSize);
								this.SetObjectRef(_objectID, num, position, _objectRef24);
								goto IL_6B1;
							}
							CCSDummy _objectRef25 = new CCSDummy(_objectID, this, num);
							flag = _objectRef25.Read(bStream, sectionSize);
							this.SetObjectRef(_objectID, num, position, _objectRef25);
							this.DummyList.Add(_objectRef25);
						}
						IL_6B1:
						if (!flag)
						{
							goto Block_38;
						}
						continue;
						IL_676:
						Logger.LogError(string.Format("Unknown section of type {0:X} found at 0x{2:X}, skipping...\n", num, _objectID, position), Logger.LogType.LogAll, "ReadSetupSection", 521);
						flag = true;
						Util.SkipSection(bStream, sectionSize);
						goto IL_6B1;
					}
					break;
				}
				Logger.LogError(string.Format("Error reading sub object, ID out of bounds ({0} > {1}) at 0x{2:X}\n", _objectID, this.ObjectIndexCount, position), Logger.LogType.LogAll, "ReadSetupSection", 331);
				return false;
				IL_6BC:
				bStream.BaseStream.Seek(-4L, SeekOrigin.Current);
				this.LastClump = null;
				this.LastObject = null;
				return true;
				Block_38:
				Logger.LogError(string.Format("Error reading section {0} of type {1}({2}) at 0x{3:X}, size: 0x{4:X}\n", new object[]
				{
					_objectID,
					num,
					this.GetObjectTypeString(num),
					position,
					sectionSize
				}), Logger.LogType.LogAll, "ReadSetupSection", 530);
				result = false;
			}
			else
			{
				Logger.LogError("Setup section mismatch\n", Logger.LogType.LogAll, "ReadSetupSection", 307);
				result = false;
			}
			return result;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000F484 File Offset: 0x0000D684
		public int SearchClump(int _objectID)
		{
			foreach (CCSClump clump in this.ClumpList)
			{
				int num = clump.SearchNodeID(_objectID);
				bool flag = num != -1;
				if (flag)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000F4F4 File Offset: 0x0000D6F4
		private void SetObjectRef(int _objectID, int _objectType, int _objectOffset, CCSBaseObject _objectRef)
		{
			IndexObjectEntry indexObjectEntry = this.ObjectIndex[_objectID];
			bool flag = indexObjectEntry.ObjectRef != null;
			if (flag)
			{
				Logger.LogWarning(string.Format("Duplicate sub object definition for object {0}:\n", _objectID), Logger.LogType.LogAll, "SetObjectRef", 559);
				Logger.LogWarning(string.Format("\tType: {0}({1}) at 0x{2:X}\n", _objectType, this.GetObjectTypeString(_objectType), _objectOffset), Logger.LogType.LogAll, "SetObjectRef", 560);
				Logger.LogWarning(string.Format("\tOriginally defined as Type: {0}({1}) at 0x{2:X}\n", indexObjectEntry.ObjectType, this.GetObjectTypeString(indexObjectEntry.ObjectType), indexObjectEntry.ObjectOffset), Logger.LogType.LogAll, "SetObjectRef", 561);
			}
			indexObjectEntry.ObjectOffset = _objectOffset;
			indexObjectEntry.ObjectType = _objectType;
			indexObjectEntry.ObjectRef = _objectRef;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000F5BF File Offset: 0x0000D7BF
		public string GetObjectTypeString(int _objectType)
		{
			return (!CCSFile.ObjectTypeNames.ContainsKey(_objectType)) ? string.Format("Unknown Object Type: 0x{0:X}", _objectType) : CCSFile.ObjectTypeNames[_objectType];
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000F5EC File Offset: 0x0000D7EC
		private bool ReadStreamSection(BinaryReader bStream)
		{
			this.StreamOffset = (int)bStream.BaseStream.Position;
			bool flag2 = (bStream.ReadInt32() & 65535) != 5;
			bool flag;
			if (flag2)
			{
				Logger.LogError("Stream section mismatch!\n", Logger.LogType.LogAll, "ReadStreamSection", 585);
				flag = false;
			}
			else
			{
				bStream.ReadInt32();
				this.StreamFrameCount = bStream.ReadInt32();
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000F65A File Offset: 0x0000D85A
		public string GetSubFileName(int _fileID)
		{
			return (_fileID >= this.FileIndexCount) ? string.Format("<Invalid File ID: {0}>", _fileID) : this.FileIndex[_fileID].FileName;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000F684 File Offset: 0x0000D884
		public string GetSubObjectName(int _objectID)
		{
			return (_objectID >= this.ObjectIndexCount) ? string.Format("<Invalid Object ID: {0}>", _objectID) : this.ObjectIndex[_objectID].ObjectName;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000F6AE File Offset: 0x0000D8AE
		public int GetSubObjectType(int _objectID)
		{
			return (_objectID >= this.ObjectIndexCount) ? 0 : this.ObjectIndex[_objectID].ObjectType;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000F6CC File Offset: 0x0000D8CC
		public int GetIDByNameAndType(string _name, int _objectType)
		{
			for (int idByNameAndType = 0; idByNameAndType < this.ObjectIndexCount; idByNameAndType++)
			{
				bool flag = this.ObjectIndex[idByNameAndType].ObjectType == _objectType;
				if (flag)
				{
					return idByNameAndType;
				}
			}
			return 0;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000F710 File Offset: 0x0000D910
		public T GetObject<T>(int _objectID) where T : CCSBaseObject
		{
			return (_objectID >= this.ObjectIndexCount) ? default(T) : (this.ObjectIndex[_objectID].ObjectRef as T);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000F748 File Offset: 0x0000D948
		public T GetObjectByNameAndType<T>(int _objectType, string _objectName) where T : CCSBaseObject
		{
			for (int index = 0; index < this.ObjectIndexCount; index++)
			{
				IndexObjectEntry indexObjectEntry = this.ObjectIndex[index];
				bool flag = indexObjectEntry.ObjectType == _objectType && indexObjectEntry.ObjectName == _objectName;
				if (flag)
				{
					return indexObjectEntry.ObjectRef as T;
				}
			}
			return default(T);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000F7B5 File Offset: 0x0000D9B5
		public IndexFileEntry GetParentSubFile(int _objectID)
		{
			return (_objectID >= this.ObjectIndexCount) ? null : this.FileIndex[(int)this.ObjectIndex[_objectID].FileID];
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000F7D8 File Offset: 0x0000D9D8
		public string GetReport()
		{
			string str = string.Format("CCS File {0}:\n", this.Header.CCSFName) + string.Format("\tVersion: {0}, {1} Files, {2} Objects\n", this.Header.GetVersionString(false), this.FileIndexCount, this.ObjectIndexCount) + string.Format("\t{0} Stream Animation frames\n", this.StreamFrameCount - 1) + "Sub Files:\n";
			for (int index = 0; index < this.FileIndexCount; index++)
			{
				string str2 = this.FileIndex[index].FileName;
				bool flag = str2 == string.Empty;
				if (flag)
				{
					str2 = "<NONE>";
				}
				str += string.Format("\t0x{0:X}: {1}\n", index, str2);
			}
			string str3 = str + "Sub Objects:\n";
			for (int index2 = 0; index2 < this.ObjectIndexCount; index2++)
			{
				IndexObjectEntry indexObjectEntry = this.ObjectIndex[index2];
				string str4 = "";
				string str5 = indexObjectEntry.ObjectName;
				bool flag2 = str5 == string.Empty;
				if (flag2)
				{
					str5 = "<NONE>";
				}
				bool flag3 = indexObjectEntry.ObjectRef == null;
				if (flag3)
				{
					str4 = "(Not present)";
				}
				str3 += string.Format("\t0x{0:X4}: {1,-30}{2:X4}{3}\n", new object[]
				{
					index2,
					str5,
					indexObjectEntry.FileID,
					str4
				});
			}
			string str6 = str3 + "Materials:\n";
			foreach (CCSMaterial material in this.MaterialList)
			{
				str6 += material.GetReport(1);
			}
			string str7 = str6 + "Textures:\n";
			foreach (CCSTexture texture in this.TextureList)
			{
				str7 += texture.GetReport(1);
			}
			string str8 = str7 + "Clumps:\n";
			foreach (CCSClump clump in this.ClumpList)
			{
				str8 += clump.GetReport(1);
			}
			string report = str8 + "Animations:\n";
			foreach (CCSAnime anime in this.AnimeList)
			{
				report += anime.GetReport(1);
			}
			return report;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000FAD8 File Offset: 0x0000DCD8
		public void DumpToObj(string outputPath, bool collision, bool splitSubModels, bool splitCollision, bool withNormals, bool dummies)
		{
			string str = Path.Combine(outputPath, this.Header.CCSFName);
			bool flag = !Directory.Exists(str);
			if (flag)
			{
				bool flag2 = File.Exists(str);
				if (flag2)
				{
					Logger.LogError(string.Format("Error, Cannot dump CCS File, {0} exists as file", this.Header.CCSFName), Logger.LogType.LogAll, "DumpToObj", 741);
					return;
				}
				Directory.CreateDirectory(str);
			}
			string str2 = Path.Combine(str, this.Header.CCSFName);
			Logger.LogInfo(string.Format("Dumping {0} to {0}.obj...\n", this.Header.CCSFName, str2), Logger.LogType.LogAll, "DumpToObj", 753);
			using (StreamWriter fStream = new StreamWriter(str2 + ".obj", false))
			{
				fStream.WriteLine(string.Format("mtllib {0}.mtl", this.Header.CCSFName));
				int vOffset = 1;
				foreach (CCSClump clump in this.ClumpList)
				{
					vOffset = clump.DumpToObj(fStream, vOffset, splitSubModels, withNormals);
				}
			}
			Logger.LogInfo("Creating MTL file and dumping textures...\n", Logger.LogType.LogAll, "DumpToObj", 766);
			using (StreamWriter fStream2 = new StreamWriter(str2 + ".mtl", false))
			{
				foreach (CCSTexture texture in this.TextureList)
				{
					texture.DumpToMtl(fStream2, Path.GetDirectoryName(str2 + ".mtl"));
				}
			}
			if (collision)
			{
				Logger.LogInfo(string.Format("Dumping Collision Meshes to {0}_collision.obj...\n", str2), Logger.LogType.LogAll, "DumpToObj", 777);
				using (StreamWriter fStream3 = new StreamWriter(str2 + "_collison.obj", false))
				{
					int vOffset2 = 1;
					foreach (CCSHitMesh hit in this.HitList)
					{
						vOffset2 = hit.DumpToObj(fStream3, vOffset2, splitCollision);
					}
				}
			}
			if (dummies)
			{
				Logger.LogInfo(string.Format("Dumping Dummies to {0}_dummies.txt...\n", str2), Logger.LogType.LogAll, "DumpToObj", 790);
				using (StreamWriter fStream4 = new StreamWriter(str2 + "_dummies.txt", false))
				{
					fStream4.WriteLine(string.Format("{0}", this.DummyList.Count));
					foreach (CCSDummy dummy in this.DummyList)
					{
						dummy.DumpToTxt(fStream4);
					}
				}
			}
			Logger.LogInfo("Done.", Logger.LogType.LogAll, "DumpToObj", 802);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000FE44 File Offset: 0x0000E044
		public void DumpToSMD(string outputPath, bool withNormals)
		{
			string str = Path.Combine(outputPath, this.Header.CCSFName);
			bool flag = !Directory.Exists(str);
			if (flag)
			{
				bool flag2 = File.Exists(str);
				if (flag2)
				{
					Logger.LogError(string.Format("Error, Cannot dump CCS File, {0} exists as file", this.Header.CCSFName), Logger.LogType.LogAll, "DumpToSMD", 812);
					return;
				}
				Directory.CreateDirectory(str);
			}
			foreach (CCSClump clump in this.ClumpList)
			{
				clump.DumpToSMD(str, withNormals);
			}
			Logger.LogInfo("Done.", Logger.LogType.LogAll, "DumpToSMD", 826);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000FF10 File Offset: 0x0000E110
		public void DumpAnimationsToText(string outPath)
		{
			string str = Path.Combine(outPath, this.Header.CCSFName);
			bool flag = !Directory.Exists(str);
			if (flag)
			{
				bool flag2 = File.Exists(str);
				if (flag2)
				{
					Logger.LogError(string.Format("Error, Cannot dump Animations to file, '{0}' exists as file", outPath), Logger.LogType.LogAll, "DumpAnimationsToText", 838);
					return;
				}
				Directory.CreateDirectory(str);
			}
			string path = Path.Combine(str, "animations.txt");
			FileMode mode = FileMode.OpenOrCreate;
			bool flag3 = File.Exists(path);
			if (flag3)
			{
				mode = FileMode.Truncate;
			}
			using (FileStream fileStream = new FileStream(path, mode))
			{
				using (StreamWriter outf = new StreamWriter(fileStream))
				{
					outf.WriteLine(this.AnimeList.Count.ToString());
					foreach (CCSAnime anime in this.AnimeList)
					{
						anime.DumpToText(outf);
					}
				}
			}
		}

		// Token: 0x04000166 RID: 358
		public const int SECTION_HEADER = 1;

		// Token: 0x04000167 RID: 359
		public const int SECTION_INDEX = 2;

		// Token: 0x04000168 RID: 360
		public const int SECTION_SETUP = 3;

		// Token: 0x04000169 RID: 361
		public const int SECTION_STREAM = 5;

		// Token: 0x0400016A RID: 362
		public const int SECTION_OBJECT = 256;

		// Token: 0x0400016B RID: 363
		public const int SECTION_MATERIAL = 512;

		// Token: 0x0400016C RID: 364
		public const int SECTION_TEXTURE = 768;

		// Token: 0x0400016D RID: 365
		public const int SECTION_CLUT = 1024;

		// Token: 0x0400016E RID: 366
		public const int SECTION_CAMERA = 1280;

		// Token: 0x0400016F RID: 367
		public const int SECTION_LIGHT = 1536;

		// Token: 0x04000170 RID: 368
		public const int SECTION_ANIME = 1792;

		// Token: 0x04000171 RID: 369
		public const int SECTION_MODEL = 2048;

		// Token: 0x04000172 RID: 370
		public const int SECTION_CLUMP = 2304;

		// Token: 0x04000173 RID: 371
		public const int SECTION_EXTERNAL = 2560;

		// Token: 0x04000174 RID: 372
		public const int SECTION_HITMESH = 2816;

		// Token: 0x04000175 RID: 373
		public const int SECTION_BBOX = 3072;

		// Token: 0x04000176 RID: 374
		public const int SECTION_PARTICLE = 3328;

		// Token: 0x04000177 RID: 375
		public const int SECTION_EFFECT = 3584;

		// Token: 0x04000178 RID: 376
		public const int SECTION_BLTGROUP = 4096;

		// Token: 0x04000179 RID: 377
		public const int SECTION_FBPAGE = 4352;

		// Token: 0x0400017A RID: 378
		public const int SECTION_FBRECT = 4608;

		// Token: 0x0400017B RID: 379
		public const int SECTION_DUMMYPOS = 4864;

		// Token: 0x0400017C RID: 380
		public const int SECTION_DUMMYPOSROT = 5120;

		// Token: 0x0400017D RID: 381
		public const int SECTION_LAYER = 5888;

		// Token: 0x0400017E RID: 382
		public const int SECTION_SHADOW = 6144;

		// Token: 0x0400017F RID: 383
		public const int SECTION_MORPHER = 6400;

		// Token: 0x04000180 RID: 384
		public const int SECTION_OBJECT2 = 8192;

		// Token: 0x04000181 RID: 385
		public const int SECTION_PCM = 8704;

		// Token: 0x04000182 RID: 386
		public const int SECTION_BINARYBLOB = 9216;

		// Token: 0x04000183 RID: 387
		public static Dictionary<int, string> ObjectTypeNames = new Dictionary<int, string>
		{
			{
				1,
				"Header"
			},
			{
				2,
				"Index"
			},
			{
				3,
				"Setup"
			},
			{
				5,
				"Stream"
			},
			{
				256,
				"Object"
			},
			{
				512,
				"Material"
			},
			{
				768,
				"Texture"
			},
			{
				1024,
				"CLUT"
			},
			{
				1280,
				"Camera"
			},
			{
				1536,
				"Light"
			},
			{
				1792,
				"Animation"
			},
			{
				2048,
				"Model"
			},
			{
				2304,
				"Clump"
			},
			{
				2560,
				"External"
			},
			{
				2816,
				"Hit Mesh"
			},
			{
				3072,
				"Bounding Box"
			},
			{
				3328,
				"Particle"
			},
			{
				3584,
				"Effect"
			},
			{
				4096,
				"Blit Group"
			},
			{
				4352,
				"FrameBuffer Page"
			},
			{
				4608,
				"FrameBuffer Rect"
			},
			{
				4864,
				"Dummy(Position)"
			},
			{
				5120,
				"Dummy(Position & Rotation)"
			},
			{
				5888,
				"Layer"
			},
			{
				6144,
				"Shadow"
			},
			{
				6400,
				"Mopher"
			},
			{
				8192,
				"Object 2"
			},
			{
				8704,
				"PCM Audio"
			},
			{
				9216,
				"Binary Blob"
			}
		};

		// Token: 0x04000184 RID: 388
		public CCSFileHeader Header = new CCSFileHeader();

		// Token: 0x04000185 RID: 389
		public IndexFileEntry[] FileIndex;

		// Token: 0x04000186 RID: 390
		public IndexObjectEntry[] ObjectIndex;

		// Token: 0x04000187 RID: 391
		public int FileIndexCount;

		// Token: 0x04000188 RID: 392
		public int ObjectIndexCount;

		// Token: 0x04000189 RID: 393
		public CCSFileHeader.CCSVersion Version;

		// Token: 0x0400018A RID: 394
		public int StreamFrameCount;

		// Token: 0x0400018B RID: 395
		public int StreamOffset;

		// Token: 0x0400018C RID: 396
		public string FileName = "";

		// Token: 0x0400018D RID: 397
		public bool NeedsInit = true;

		// Token: 0x0400018E RID: 398
		public List<CCSClump> ClumpList = new List<CCSClump>();

		// Token: 0x0400018F RID: 399
		public List<CCSMaterial> MaterialList = new List<CCSMaterial>();

		// Token: 0x04000190 RID: 400
		public List<CCSTexture> TextureList = new List<CCSTexture>();

		// Token: 0x04000191 RID: 401
		public List<CCSHitMesh> HitList = new List<CCSHitMesh>();

		// Token: 0x04000192 RID: 402
		public List<CCSBoundingBox> BBoxList = new List<CCSBoundingBox>();

		// Token: 0x04000193 RID: 403
		public List<CCSDummy> DummyList = new List<CCSDummy>();

		// Token: 0x04000194 RID: 404
		public List<CCSAnime> AnimeList = new List<CCSAnime>();

		// Token: 0x04000195 RID: 405
		public CCSClump LastClump;

		// Token: 0x04000196 RID: 406
		public CCSObject LastObject;
	}
}
