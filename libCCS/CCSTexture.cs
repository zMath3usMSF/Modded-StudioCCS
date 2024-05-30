using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace StudioCCS.libCCS
{
	// Token: 0x0200002B RID: 43
	public class CCSTexture : CCSBaseObject
	{
		// Token: 0x06000164 RID: 356 RVA: 0x0001430D File Offset: 0x0001250D
		public CCSTexture(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 768;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00014330 File Offset: 0x00012530
		public override bool Init()
		{
			return this.InitTexture(this.CLUTID);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00014340 File Offset: 0x00012540
		public override bool DeInit()
		{
			bool flag = this.TextureID != -1;
			if (flag)
			{
				GL.DeleteTexture(this.TextureID);
			}
			this.TextureID = -1;
			return true;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00014378 File Offset: 0x00012578
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			long position = bStream.BaseStream.Position;
			this.CLUTID = bStream.ReadInt32();
			this.BlitGroupID = bStream.ReadInt32();
			this.TextureFlags = (int)bStream.ReadByte();
			this.TextureType = (int)bStream.ReadByte();
			bool flag = this.TextureType != 20 && this.TextureType != 19 && this.TextureType != 0 && this.TextureType != 135 && this.TextureType != 137;
			bool result;
			if (flag)
			{
				Logger.LogError(string.Format("CCSTexture::Read(): Unknown texture type {0:X} at 0x{1:X}\n", this.TextureType, position), Logger.LogType.LogAll, "Read", 87);
				result = false;
			}
			else
			{
				this.MipCount = (int)bStream.ReadByte();
				int num = (int)bStream.ReadByte();
				this.Width = (int)bStream.ReadByte();
				this.Height = (int)bStream.ReadByte();
				int num2 = (int)bStream.ReadInt16();
				bool flag2 = this.ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
				if (flag2)
				{
					this.Width = 1 << this.Width;
					this.Height = 1 << this.Height;
					bStream.ReadInt32();
				}
				else
				{
					bool flag3 = this.Width == 255 || this.Height == 255;
					if (flag3)
					{
						this.NonP2 = true;
						bool flag4 = this.TextureType == 135 || this.TextureType == 137;
						if (flag4)
						{
							Logger.LogError(string.Format("Error, CCSTexture 0x{0:X}: {1} has non-power of two width or height", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID)), Logger.LogType.LogAll, "Read", 110);
							return false;
						}
						this.Width = (int)bStream.ReadInt16();
						this.Height = (int)bStream.ReadInt16();
					}
					else
					{
						this.Width = 1 << this.Width;
						this.Height = 1 << this.Height;
						bool flag5 = this.TextureType == 135 || this.TextureType == 137;
						if (flag5)
						{
							bStream.BaseStream.Seek(16L, SeekOrigin.Current);
							int num3 = (int)bStream.ReadInt16();
							int num4 = (int)bStream.ReadInt16();
							int width = this.Width;
							bool flag6 = num3 != width || num4 != this.Height;
							if (flag6)
							{
								Logger.LogWarning(string.Format("Warning, CCSTexture 0x{0:X}: {1} has mismatched Width/Height values..", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID)), Logger.LogType.LogAll, "Read", 130);
							}
							bStream.BaseStream.Seek(20L, SeekOrigin.Current);
						}
						else
						{
							bStream.ReadInt32();
						}
					}
				}
				int num5 = bStream.ReadInt32();
				bool flag7 = this.TextureType == 135 || this.TextureType == 137;
				int count;
				if (flag7)
				{
					count = num5 - 64;
					bStream.BaseStream.Seek(28L, SeekOrigin.Current);
				}
				else
				{
					count = num5 << 2;
				}
				this.TextureIndices = new byte[count];
				bool flag8 = this.TextureType == 0;
				if (flag8)
				{
					for (int index = 0; index < count / 4; index++)
					{
						byte num6 = bStream.ReadByte();
						byte num7 = bStream.ReadByte();
						byte num8 = bStream.ReadByte();
						byte num9 = bStream.ReadByte();
						byte num10 = (byte)((num9 < 127) ? (num9 * 2) : byte.MaxValue);
						int index2 = index * 4;
						this.TextureIndices[index2] = num6;
						this.TextureIndices[index2 + 1] = num7;
						this.TextureIndices[index2 + 2] = num8;
						this.TextureIndices[index2 + 3] = num10;
					}
				}
				else
				{
					bStream.Read(this.TextureIndices, 0, count);
				}
				bool flag9 = (this.TextureType == 135 || this.TextureType == 137) && this.MipCount > 0;
				if (flag9)
				{
					Logger.LogError(string.Format("Error, Texture {0:X} at 0x{1:X} has MipLevels. Please investigate.", this.ObjectID, position), Logger.LogType.LogAll, "Read", 183);
					result = false;
				}
				else
				{
					for (int index3 = 0; index3 < this.MipCount; index3++)
					{
						bStream.ReadInt32();
						int offset = bStream.ReadInt32() << 2;
						bStream.BaseStream.Seek((long)offset, SeekOrigin.Current);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000147E4 File Offset: 0x000129E4
		public bool InitTexture(int _clutID)
		{
			bool nonP = this.NonP2;
			bool result;
			if (nonP)
			{
				Logger.LogError("CCSTexture::InitTexture(): Non-Power of Two textures are currently unsupported\n", Logger.LogType.LogAll, "InitTexture", 251);
				result = false;
			}
			else
			{
				bool flag = this.TextureType == 135 || this.TextureType == 137;
				if (flag)
				{
					bool flag2 = this.TextureID == 0;
					if (flag2)
					{
						this.TextureID = GL.GenTexture();
					}
					GL.BindTexture(TextureTarget.Texture2D, this.TextureID);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, 10497);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, 10497);
					PixelInternalFormat internalformat = PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;
					bool flag3 = this.TextureType == 137;
					if (flag3)
					{
						internalformat = PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
					}
					GL.CompressedTexImage2D<byte>(TextureTarget.Texture2D, 0, internalformat, this.Width, this.Height, 0, this.TextureIndices.Length, this.TextureIndices);
					GL.BindTexture(TextureTarget.Texture2D, 0);
					result = true;
				}
				else
				{
					Bitmap bitmap = this.ToBitmap(_clutID);
					bool flag4 = this.TextureID == 0;
					if (flag4)
					{
						this.TextureID = GL.GenTexture();
					}
					this.CurrentCLUTID = _clutID;
					GL.BindTexture(TextureTarget.Texture2D, this.TextureID);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, 10497);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, 10497);
					BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, this.Width, this.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
					GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, this.Width, this.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, bitmapdata.Scan0);
					bitmap.UnlockBits(bitmapdata);
					GL.BindTexture(TextureTarget.Texture2D, 0);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00014A14 File Offset: 0x00012C14
		public bool InitTexture_Gen3()
		{
			bool flag = this.TextureID == 0;
			if (flag)
			{
				this.TextureID = GL.GenTexture();
			}
			GL.BindTexture(TextureTarget.Texture2D, this.TextureID);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, 10497);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, 10497);
			bool flag2 = this.TextureType == 135;
			PixelInternalFormat internalformat;
			if (flag2)
			{
				internalformat = PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;
			}
			else
			{
				bool flag3 = this.TextureType != 137;
				if (flag3)
				{
					GL.BindTexture(TextureTarget.Texture2D, 0);
					return true;
				}
				internalformat = PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
			}
			GL.CompressedTexImage2D<byte>(TextureTarget.Texture2D, 0, internalformat, this.Width, this.Height, 0, this.TextureIndices.Length, this.TextureIndices);
			GL.BindTexture(TextureTarget.Texture2D, 0);
			return true;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00014B24 File Offset: 0x00012D24
		public Bitmap ToBitmap(int _clutID)
		{
			Bitmap bitmap = new Bitmap(this.Width, this.Height);
			bool flag = this.TextureType == 20 || this.TextureType == 19;
			if (flag)
			{
				CCSClut ccsClut = this.ParentFile.GetObject<CCSClut>(_clutID);
				bool flag2 = ccsClut == null;
				if (flag2)
				{
					return null;
				}
				this.HasAlpha = ccsClut.HasAlpha;
				bool flag3 = this.TextureType == 20;
				if (flag3)
				{
					for (int y = 0; y < this.Height; y++)
					{
						for (int index = 0; index < this.Width / 2; index++)
						{
							byte textureIndex = this.TextureIndices[y * (this.Width / 2) + index];
							bitmap.SetPixel(index * 2, y, ccsClut.Palette[(int)(textureIndex & 15)]);
							bitmap.SetPixel(index * 2 + 1, y, ccsClut.Palette[textureIndex >> 4 & 15]);
						}
					}
				}
				else
				{
					bool flag4 = this.TextureType == 19;
					if (flag4)
					{
						for (int y2 = 0; y2 < this.Height; y2++)
						{
							for (int x = 0; x < this.Width; x++)
							{
								int index2 = y2 * this.Width + x;
								bitmap.SetPixel(x, y2, ccsClut.Palette[(int)this.TextureIndices[index2]]);
							}
						}
					}
				}
			}
			else
			{
				bool flag5 = this.TextureType == 0;
				if (flag5)
				{
					for (int y3 = 0; y3 < this.Height; y3++)
					{
						for (int x2 = 0; x2 < this.Width; x2++)
						{
							int index3 = (y3 * this.Width + x2) * 4;
							bitmap.SetPixel(x2, y3, Color.FromArgb((int)this.TextureIndices[index3 + 3], (int)this.TextureIndices[index3 + 2], (int)this.TextureIndices[index3 + 1], (int)this.TextureIndices[index3]));
						}
					}
				}
			}
			return bitmap;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00014D58 File Offset: 0x00012F58
		public string GetTextureTypeStr()
		{
			bool flag = this.TextureType == 20;
			string result;
			if (flag)
			{
				result = "4bit Indexed";
			}
			else
			{
				bool flag2 = this.TextureType == 19;
				if (flag2)
				{
					result = "8bit Indexed";
				}
				else
				{
					bool flag3 = this.TextureType == 0;
					if (flag3)
					{
						result = "32bit RGBA";
					}
					else
					{
						bool flag4 = this.TextureType == 135;
						if (flag4)
						{
							result = "DXT1 Compressed";
						}
						else
						{
							result = ((this.TextureType == 137) ? "DXT5 Compressed" : "Unknown");
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00014DE0 File Offset: 0x00012FE0
		public override TreeNode ToNode()
		{
			TreeNode node = base.ToNode();
			string str = string.Format("  Type: {0}", this.GetTextureTypeStr());
			TreeNode treeNode = node;
			treeNode.Text += str;
			IndexFileEntry parentSubFile = this.ParentFile.GetParentSubFile(this.ObjectID);
			bool flag = parentSubFile != null;
			if (flag)
			{
				for (int index = 0; index < parentSubFile.ObjectIDs.Count; index++)
				{
					int objectId = parentSubFile.ObjectIDs[index];
					bool flag2 = this.ParentFile.GetSubObjectType(objectId) == 1024;
					if (flag2)
					{
						CCSClut ccsClut = this.ParentFile.GetObject<CCSClut>(objectId);
						TreeNode node2 = (ccsClut != null) ? ccsClut.ToNode() : Util.NonExistantNode(this.ParentFile, objectId);
						bool flag3 = objectId == this.CLUTID;
						if (flag3)
						{
							TreeNode treeNode2 = node2;
							treeNode2.Text += " (Default)";
						}
						bool flag4 = objectId == this.CurrentCLUTID;
						if (flag4)
						{
							TreeNode treeNode3 = node2;
							treeNode3.Text += " (Current)";
						}
						node.Nodes.Add(node2);
					}
				}
			}
			return node;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00014F20 File Offset: 0x00013120
		public string GetReport(int level = 0)
		{
			string report = Util.Indent(level, false) + string.Format("Texture 0x{0:X}: {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID)) + Util.Indent(level + 1, false) + string.Format("Type: {0}, {1}x{2}, {3} MipMap Levels\n", new object[]
			{
				this.GetTextureTypeStr(),
				this.Width,
				this.Height,
				this.MipCount
			});
			bool flag = this.TextureType != 0;
			if (flag)
			{
				report = report + Util.Indent(level + 1, false) + string.Format("CLUT: {0}: {1}\n", this.CLUTID, this.ParentFile.GetSubObjectName(this.CLUTID));
			}
			return report;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00014FF8 File Offset: 0x000131F8
		public void DumpToMtl(StreamWriter fStream, string outputPath)
		{
			string subObjectName = this.ParentFile.GetSubObjectName(this.ObjectID);
			fStream.WriteLine(string.Format("newmtl {0}", subObjectName));
			fStream.WriteLine("Ka 1.0 1.0 1.0");
			fStream.WriteLine("Kd 1.0 1.0 1.0");
			fStream.WriteLine("Ks 0.0 0.0 0.0");
			bool flag = this.TextureType == 135 || this.TextureType == 137;
			if (flag)
			{
				fStream.WriteLine(string.Format("map_Kd {0}.dds", subObjectName));
				this.DumpToDDS(string.Format("{0}.dds", Path.Combine(outputPath, subObjectName)));
			}
			else
			{
				fStream.WriteLine(string.Format("map_Kd {0}.png", subObjectName));
				string filename = string.Format("{0}.png", Path.Combine(outputPath, subObjectName));
				Logger.LogInfo(string.Format("\tDumping {0}...\n", filename), Logger.LogType.LogAll, "DumpToMtl", 479);
				Bitmap bitmap = this.ToBitmap(this.CurrentCLUTID);
				BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				byte[] numArray = new byte[bitmapdata.Stride * bitmapdata.Height];
				Marshal.Copy(bitmapdata.Scan0, numArray, 0, numArray.Length);
				for (int index = 0; index < numArray.Length; index += 4)
				{
					byte num = numArray[index];
					numArray[index] = numArray[index + 2];
					numArray[index + 2] = num;
				}
				Marshal.Copy(numArray, 0, bitmapdata.Scan0, numArray.Length);
				bitmap.UnlockBits(bitmapdata);
				bitmap.Save(filename, ImageFormat.Png);
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0001519C File Offset: 0x0001339C
		public void DumpToDDS(string outputFileName)
		{
			using (FileStream output = new FileStream(outputFileName, FileMode.OpenOrCreate))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(output))
				{
					binaryWriter.Write(542327876);
					binaryWriter.Write(124);
					binaryWriter.Write(659463);
					binaryWriter.Write(this.Height);
					binaryWriter.Write(this.Width);
					int num = 16;
					bool flag = this.TextureType == 135;
					if (flag)
					{
						num = 8;
					}
					int num2 = Math.Max(1, (this.Width + 3) / 4) * num;
					binaryWriter.Write(num2);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					for (int index = 0; index < 11; index++)
					{
						binaryWriter.Write(0);
					}
					binaryWriter.Write(32);
					binaryWriter.Write(4);
					int num3 = 827611204;
					bool flag2 = this.TextureType == 137;
					if (flag2)
					{
						num3 = 894720068;
					}
					binaryWriter.Write(num3);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(4198408);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(this.TextureIndices);
				}
			}
		}

		// Token: 0x040001E8 RID: 488
		public const int CCS_TEXTURE_I4 = 20;

		// Token: 0x040001E9 RID: 489
		public const int CCS_TEXTURE_I8 = 19;

		// Token: 0x040001EA RID: 490
		public const int CCS_TEXTURE_RGBA32 = 0;

		// Token: 0x040001EB RID: 491
		public const int CCS_TEXTURE_DXT1 = 135;

		// Token: 0x040001EC RID: 492
		public const int CCS_TEXTURE_DXT5 = 137;

		// Token: 0x040001ED RID: 493
		public byte[] TextureIndices;

		// Token: 0x040001EE RID: 494
		public int CLUTID;

		// Token: 0x040001EF RID: 495
		public int MipCount;

		// Token: 0x040001F0 RID: 496
		public int TextureType;

		// Token: 0x040001F1 RID: 497
		public int Width;

		// Token: 0x040001F2 RID: 498
		public int Height;

		// Token: 0x040001F3 RID: 499
		public int BlitGroupID;

		// Token: 0x040001F4 RID: 500
		public int TextureFlags;

		// Token: 0x040001F5 RID: 501
		public bool NonP2;

		// Token: 0x040001F6 RID: 502
		public bool HasAlpha;

		// Token: 0x040001F7 RID: 503
		public int CurrentCLUTID;

		// Token: 0x040001F8 RID: 504
		public int TextureID;
	}
}
