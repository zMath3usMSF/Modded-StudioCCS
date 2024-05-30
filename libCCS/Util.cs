using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace StudioCCS.libCCS
{
	// Token: 0x0200002F RID: 47
	public static class Util
	{
		// Token: 0x06000179 RID: 377 RVA: 0x000155CF File Offset: 0x000137CF
		public static bool Vector3LessThan(Vector3 a, Vector3 b)
		{
			return (double)a.X < (double)b.X && (double)a.Y < (double)b.Y && (double)a.Z < (double)b.Z;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00015604 File Offset: 0x00013804
		public static Vector3 FixAxis(Vector3 input)
		{
			return new Vector3(input.X, input.Y, input.Z);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0001561D File Offset: 0x0001381D
		public static Vector3 FixAxisRotation(Vector3 input)
		{
			return new Vector3(input.Z, -input.Y, input.X);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00015637 File Offset: 0x00013837
		public static Vector3 UnFixAxisRotation(Vector3 input)
		{
			return new Vector3(input.X, -input.Y, input.Z);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00015651 File Offset: 0x00013851
		public static Vector4 FixAxisRotation4(Vector4 input)
		{
			return input;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00015654 File Offset: 0x00013854
		public static Vector3 ReadVec3Position(BinaryReader bStream)
		{
			float num = 1.6f;
			return Util.FixAxis(new Vector3(bStream.ReadSingle() * num * 0.00625f, bStream.ReadSingle() * num * 0.00625f, bStream.ReadSingle() * num * 0.00625f));
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000156A0 File Offset: 0x000138A0
		public static Vector3 ReadVec3Half(BinaryReader bStream, float scale)
		{
			scale /= 256f;
			float num = (float)bStream.ReadInt16() * 0.000625f;
			float num2 = (float)bStream.ReadInt16() * 0.000625f;
			float num3 = (float)bStream.ReadInt16() * 0.000625f;
			return Util.FixAxis(new Vector3(num * scale, num2 * scale, num3 * scale));
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000156F8 File Offset: 0x000138F8
		public static Vector3 ReadVec3Rotation(BinaryReader bStream)
		{
			float num = bStream.ReadSingle();
			float num2 = bStream.ReadSingle();
			float num3 = bStream.ReadSingle();
			float num4 = 3.141593f;
			return Util.FixAxisRotation(new Vector3((float)((double)num * (double)num4 / 180.0), (float)((double)num2 * (double)num4 / 180.0), (float)((double)num3 * (double)num4 / 180.0)));
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00015761 File Offset: 0x00013961
		public static Vector3 ReadVec3Scale(BinaryReader bStream)
		{
			return new Vector3(bStream.ReadSingle(), bStream.ReadSingle(), bStream.ReadSingle());
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0001577A File Offset: 0x0001397A
		public static Vector3 ReadVec3Normal8(BinaryReader bStream)
		{
			return new Vector3((float)(-(float)bStream.ReadByte()) * 0.015625f, (float)bStream.ReadByte() * 0.015625f, (float)bStream.ReadByte() * 0.015625f);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000157A9 File Offset: 0x000139A9
		public static Vector2 ReadVec2UV(BinaryReader bStream)
		{
			return new Vector2((float)bStream.ReadInt16() * 0.00390625f, (float)bStream.ReadInt16() * 0.00390625f);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000157CC File Offset: 0x000139CC
		public static Vector2 ReadVec2UV_Gen3(BinaryReader bStream)
		{
			float num = 1.525902E-05f;
			return new Vector2((float)bStream.ReadInt32() * num, (float)bStream.ReadInt32() * num);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000157FC File Offset: 0x000139FC
		public static Vector4 ReadVec4RGBA32(BinaryReader bStream)
		{
			byte num = bStream.ReadByte();
			byte num2 = bStream.ReadByte();
			byte num3 = bStream.ReadByte();
			byte num4 = bStream.ReadByte();
			byte num5 = (num4 < 127) ? ((byte)(num4 << 1)) : byte.MaxValue;
			return new Vector4((float)num * 0.003921569f, (float)num2 * 0.003921569f, (float)num3 * 0.003921569f, (float)num5 * 0.003921569f);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00015864 File Offset: 0x00013A64
		public static Color ReadColorRGBA32(BinaryReader bStream)
		{
			byte blue = bStream.ReadByte();
			byte green = bStream.ReadByte();
			byte red = bStream.ReadByte();
			byte num = bStream.ReadByte();
			return Color.FromArgb((int)((num < 127) ? (num * 2) : byte.MaxValue), (int)red, (int)green, (int)blue);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000158AC File Offset: 0x00013AAC
		public static void SkipSection(BinaryReader bStream, int sectionSize)
		{
			bStream.BaseStream.Seek((long)(sectionSize * 4), SeekOrigin.Current);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000158C0 File Offset: 0x00013AC0
		public static string ReadString(BinaryReader bStream, int stringSize = 32)
		{
			string str = Encoding.Default.GetString(bStream.ReadBytes(stringSize));
			int length = str.IndexOf('\0');
			return (length > 0) ? str.Substring(0, length) : string.Empty;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000158FF File Offset: 0x00013AFF
		public static TreeNode NonExistantNode(CCSFile _file, int _objectID)
		{
			return new TreeNode(string.Format("{0}: {1}", _objectID, _file.GetSubObjectName(_objectID)))
			{
				Tag = new TreeNodeTag(_file, _objectID, 0),
				ForeColor = Color.Red
			};
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00015938 File Offset: 0x00013B38
		public static string Indent(int count, bool treeIt = false)
		{
			return (treeIt && count > 1) ? ("".PadLeft((count - 1) * 4 - 1) + "|" + "".PadLeft(4, '-')) : "".PadLeft(count * 4);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00015978 File Offset: 0x00013B78
		public static Vector3 SkinVertex(Vector3 v, Matrix4 m1, Matrix4 m2, float w1, float w2)
		{
			Vector4 vector4_ = new Vector4(v, 1f);
			Vector4 vector4_2 = new Vector4(v, 1f) * m2 * w2;
			Vector4 vector4_3 = vector4_ * m1 * w1 + vector4_2;
			return new Vector3(vector4_3.X, vector4_3.Y, vector4_3.Z);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000159DC File Offset: 0x00013BDC
		public static Vector3 q2e(Vector4 q)
		{
			float num = (float)((double)q.X * (double)q.Y + (double)q.Z * (double)q.W);
			bool flag = (double)num > 0.499;
			float x;
			float y;
			float z;
			if (flag)
			{
				x = (float)(2.0 * Math.Atan2((double)q.X, (double)q.W));
				y = (float)(3.141592653589793 * Math.Atan2((double)q.X, (double)q.W));
				z = 0f;
			}
			else
			{
				float num2 = q.X * q.X;
				float num3 = q.Y * q.Y;
				float num4 = q.Z * q.Z;
				x = (float)Math.Atan2(2.0 * (double)q.Y * (double)q.W - 2.0 * (double)q.X * (double)q.Z, 1.0 - 2.0 * (double)num3 - 2.0 * (double)num4);
				y = (float)Math.Asin(2.0 * (double)num);
				z = (float)Math.Atan2(2.0 * (double)q.X * (double)q.W - 2.0 * (double)q.Y * (double)q.Z, 1.0 - 2.0 * (double)num2 - 2.0 * (double)num4);
			}
			return new Vector3(x, y, z);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00015B78 File Offset: 0x00013D78
		public static float Clampf(float a, float min, float max)
		{
			bool flag = (double)a < (double)min;
			float result;
			if (flag)
			{
				result = min;
			}
			else
			{
				result = (((double)a > (double)max) ? max : a);
			}
			return result;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00015BA4 File Offset: 0x00013DA4
		public static Vector3 V3Slerp(float t, Vector3 a, Vector3 b)
		{
			float d = Vector3.Dot(a, b);
			t /= 2f;
			float a2 = (float)Math.Acos((double)d);
			bool flag = (double)a2 < 0.0;
			if (flag)
			{
				a2 = -a2;
			}
			float num = (float)Math.Sin((double)a2);
			float num2 = (float)Math.Sin((1.0 - (double)t) * (double)a2) / num;
			float num3 = (float)Math.Sin((double)t * (double)a2) / num;
			return a * num2 + b * num3;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00015C2C File Offset: 0x00013E2C
		public static Vector3 Slerp(float t, Vector3 a, Vector3 b)
		{
			float d = Vector3.Dot(a, b);
			float num = (float)Math.Acos((double)d) * t;
			Vector3 vector3 = b - a * d;
			vector3.Normalize();
			float num2 = (float)Math.Cos((double)num);
			float num3 = (float)Math.Sin((double)num);
			return a * num2 + vector3 * num3;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00015C90 File Offset: 0x00013E90
		public static int GetRangeOfFrames(int curKeyNum, int nextKeyNum, int frameCount)
		{
			int rangeOfFrames = nextKeyNum - curKeyNum;
			bool flag = rangeOfFrames < 0;
			if (flag)
			{
				rangeOfFrames = frameCount - curKeyNum;
			}
			return rangeOfFrames;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00015CB3 File Offset: 0x00013EB3
		public static float toRads(float what)
		{
			return what * 3.141593f / 180f;
		}

		// Token: 0x04000209 RID: 521
		public const float UV_SCALE = 0.00390625f;

		// Token: 0x0400020A RID: 522
		public const float WEIGHT_SCALE = 0.00390625f;

		// Token: 0x0400020B RID: 523
		public const float COLOR_SCALE = 0.003921569f;

		// Token: 0x0400020C RID: 524
		public const float NORMAL_SCALE = 0.015625f;

		// Token: 0x0400020D RID: 525
		public const float VTEX_SCALE = 0.000625f;

		// Token: 0x0400020E RID: 526
		public const float CCS_GLOBAL_SCALE = 0.00625f;

		// Token: 0x0400020F RID: 527
		public const float NINETY_RADS = -1.570796f;
	}
}
