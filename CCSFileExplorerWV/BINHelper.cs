using System;
using System.IO;
using System.Windows.Forms;
using StudioCCS.libETC;

namespace CCSFileExplorerWV
{
	// Token: 0x02000002 RID: 2
	public static class BINHelper
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public static void UnpackToFolder(string filename, string folder, ToolStripProgressBar pb1 = null, ToolStripStatusLabel strip = null)
		{
			FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
			fs.Seek(0L, SeekOrigin.End);
			long size = fs.Position;
			fs.Seek(0L, SeekOrigin.Begin);
			byte[] buff = new byte[4];
			bool flag = pb1 != null;
			if (flag)
			{
				pb1.Maximum = (int)size;
			}
			int fileindex = 0;
			while (fs.Position < size)
			{
				fs.Read(buff, 0, 4);
				bool flag2 = FileHelper.isGzipMagic(buff, 0);
				if (flag2)
				{
					int pos = (int)fs.Position - 4;
					int start = pos;
					while ((long)pos < size)
					{
						pos += 2048;
						fs.Seek(2044L, SeekOrigin.Current);
						fs.Read(buff, 0, 4);
						bool flag3 = FileHelper.isGzipMagic(buff, 0);
						if (flag3)
						{
							fs.Seek(-4L, SeekOrigin.Current);
							break;
						}
					}
					fs.Seek((long)start, SeekOrigin.Begin);
					buff = new byte[pos - start];
					fs.Read(buff, 0, pos - start);
					buff = FileHelper.unzipArray(buff);
					string name = "";
					int tpos = 12;
					while (buff[tpos] > 0)
					{
						string str = name;
						char c = (char)buff[tpos++];
						name = str + c.ToString();
					}
					File.WriteAllBytes(folder + name + ".tmp", buff);
					fileindex++;
					bool flag4 = pb1 != null;
					if (flag4)
					{
						pb1.Value = start;
						strip.Text = name;
						Application.DoEvents();
					}
					buff = new byte[4];
				}
				else
				{
					fs.Seek(2044L, SeekOrigin.Current);
				}
			}
			bool flag5 = pb1 != null;
			if (flag5)
			{
				pb1.Value = 0;
				strip.Text = "";
			}
			fs.Close();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000220C File Offset: 0x0000040C
		public static void Repack(string localtosave, string file)
		{
			FileStream fs = new FileStream(localtosave, FileMode.Create, FileAccess.Write);
			byte[] buff = File.ReadAllBytes(file);
			MemoryStream i = new MemoryStream();
			string infilename = Path.GetFileNameWithoutExtension(file).Substring(0) + ".tmp";
			buff = FileHelper.zipArray(buff, infilename);
			buff[8] = 0;
			buff[9] = 3;
			i.Write(buff, 0, buff.Length);
			while (i.Length % 2048L != 0L)
			{
				i.WriteByte(0);
			}
			i.Seek(-3L, SeekOrigin.Current);
			i.Read(buff, 0, 3);
			bool flag = buff[0] / 16 != 0 || buff[1] != 0 || buff[2] > 0;
			if (flag)
			{
				i.Write(new byte[2048], 0, 2048);
			}
			buff = i.ToArray();
			fs.Write(buff, 0, buff.Length);
			fs.Close();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000022EC File Offset: 0x000004EC
		public static void Repackccs(string localtosave, byte[] file, string filename)
		{
			FileStream fs = new FileStream(localtosave, FileMode.Create, FileAccess.Write);
			MemoryStream i = new MemoryStream();
			string infilename = filename + ".tmp";
			byte[] buff = FileHelper.zipArray(file, infilename);
			buff[8] = 0;
			buff[9] = 3;
			i.Write(buff, 0, buff.Length);
			while (i.Length % 2048L != 0L)
			{
				i.WriteByte(0);
			}
			i.Seek(-3L, SeekOrigin.Current);
			i.Read(buff, 0, 3);
			bool flag = buff[0] / 16 != 0 || buff[1] != 0 || buff[2] > 0;
			if (flag)
			{
				i.Write(new byte[2048], 0, 2048);
			}
			buff = i.ToArray();
			fs.Write(buff, 0, buff.Length);
			fs.Close();
		}
	}
}
