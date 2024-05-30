using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace StudioCCS.libETC
{
	// Token: 0x02000031 RID: 49
	public static class FileHelper
	{
		// Token: 0x06000193 RID: 403 RVA: 0x00015CC4 File Offset: 0x00013EC4
		public static bool isGzipMagic(byte[] data, int start = 0)
		{
			return data[start++] == 31 && data[start++] == 139 && data[start++] == 8 && (data[start] == 8 || data[start] == 0);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00015D10 File Offset: 0x00013F10
		public static byte[] unzipArray(byte[] data)
		{
			MemoryStream result = new MemoryStream();
			GZipStream gzipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
			gzipStream.CopyTo(result);
			gzipStream.Close();
			return result.ToArray();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00015D4C File Offset: 0x00013F4C
		public static byte[] zipArray(byte[] data, string filename)
		{
			MemoryStream i = new MemoryStream();
			GZipStream stream = new GZipStream(i, CompressionMode.Compress);
			new MemoryStream(data).CopyTo(stream);
			stream.Close();
			byte[] cdata = i.ToArray();
			i = new MemoryStream();
			bool flag = filename != null && filename != "";
			if (flag)
			{
				byte[] buff = Encoding.ASCII.GetBytes(filename);
				i.Write(cdata, 0, 3);
				i.WriteByte(8);
				i.Write(cdata, 4, 6);
				i.Write(buff, 0, buff.Length);
				i.WriteByte(0);
				i.Write(cdata, 10, cdata.Length - 10);
			}
			else
			{
				i.Write(cdata, 0, cdata.Length);
			}
			return i.ToArray();
		}
	}
}
