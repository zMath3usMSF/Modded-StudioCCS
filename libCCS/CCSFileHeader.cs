using System;
using System.IO;

namespace StudioCCS.libCCS
{
	// Token: 0x0200002C RID: 44
	public class CCSFileHeader
	{
		// Token: 0x06000170 RID: 368 RVA: 0x00015344 File Offset: 0x00013544
		public bool Read(BinaryReader bStream)
		{
			bool flag2 = (bStream.ReadInt32() & 65535) != 1;
			bool flag;
			if (flag2)
			{
				Logger.LogError("Header Section Mismatch!\n", Logger.LogType.LogAll, "Read", 44);
				flag = false;
			}
			else
			{
				bStream.ReadInt32();
				bool flag3 = bStream.ReadInt32() != 1179861827;
				if (flag3)
				{
					Logger.LogError("Invalid CCS Magic.\n", Logger.LogType.LogAll, "Read", 51);
					flag = false;
				}
				else
				{
					this.CCSFName = Util.ReadString(bStream, 32);
					this.CCSFVersion = bStream.ReadInt32();
					this.Unk1 = bStream.ReadInt32();
					this.Unk2 = bStream.ReadInt32();
					this.Unk3 = bStream.ReadInt32();
					bool flag4 = this.CCSFVersion >= 293;
					if (flag4)
					{
						Logger.LogError("Support for Generation 3 (Last Recode) CCS Files is currently experimental...\n", Logger.LogType.LogAll, "Read", 64);
					}
					else
					{
						bool flag5 = this.CCSFVersion >= 288;
						if (flag5)
						{
							Logger.LogError("Support for Generation 2 (GU/Link) CCS Files is currently experimental...\n", Logger.LogType.LogAll, "Read", 64);
						}
						else
						{
							int ccsfVersion = this.CCSFVersion;
						}
					}
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00015466 File Offset: 0x00013666
		public CCSFileHeader.CCSVersion GetVersionType()
		{
			return (this.CCSFVersion < 293) ? ((this.CCSFVersion < 288) ? CCSFileHeader.CCSVersion.Gen1 : CCSFileHeader.CCSVersion.Gen2) : CCSFileHeader.CCSVersion.Gen3;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0001548C File Offset: 0x0001368C
		public string GetVersionString(bool generation = false)
		{
			byte num = (byte)(this.CCSFVersion >> 16 & 15);
			byte num2 = (byte)(this.CCSFVersion >> 8 & 15);
			byte num3 = (byte)(this.CCSFVersion & 15);
			string str = "";
			if (generation)
			{
				str = ((this.CCSFVersion <= 288) ? ((this.CCSFVersion <= 272) ? "(Gen. 1)" : "(Gen. 2)") : "(Gen. 3)");
			}
			return string.Format("{0}.{1}.{2}{3}", new object[]
			{
				num,
				num2,
				num3,
				str
			});
		}

		// Token: 0x040001F9 RID: 505
		public const int CCS_MAGIC = 1179861827;

		// Token: 0x040001FA RID: 506
		public const int CCS_VERSION_ONE = 272;

		// Token: 0x040001FB RID: 507
		public const int CCS_VERSION_TWO = 288;

		// Token: 0x040001FC RID: 508
		public const int CCS_VERSION_THREE = 293;

		// Token: 0x040001FD RID: 509
		public string CCSFName = "";

		// Token: 0x040001FE RID: 510
		public int CCSFVersion;

		// Token: 0x040001FF RID: 511
		public int Unk1;

		// Token: 0x04000200 RID: 512
		public int Unk2;

		// Token: 0x04000201 RID: 513
		public int Unk3;

		// Token: 0x02000066 RID: 102
		public enum CCSVersion
		{
			// Token: 0x040002E6 RID: 742
			Gen1,
			// Token: 0x040002E7 RID: 743
			Gen2,
			// Token: 0x040002E8 RID: 744
			Gen3
		}
	}
}
