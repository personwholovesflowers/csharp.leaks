using System;
using System.IO;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000285 RID: 645
	public static class MB_TGAWriter
	{
		// Token: 0x06001038 RID: 4152 RVA: 0x0005513C File Offset: 0x0005333C
		public static void Write(Color[] pixels, int width, int height, string path)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			FileStream fileStream = File.Create(path);
			MB_TGAWriter.Write(pixels, width, height, fileStream);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00055168 File Offset: 0x00053368
		public static void Write(Color[] pixels, int width, int height, Stream output)
		{
			byte[] array = new byte[pixels.Length * 4];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					Color color = pixels[num];
					array[num2] = (byte)(color.b * 255f);
					array[num2 + 1] = (byte)(color.g * 255f);
					array[num2 + 2] = (byte)(color.r * 255f);
					array[num2 + 3] = (byte)(color.a * 255f);
					num++;
					num2 += 4;
				}
			}
			byte[] array2 = new byte[18];
			array2[2] = 2;
			array2[12] = (byte)(width & 255);
			array2[13] = (byte)((width & 65280) >> 8);
			array2[14] = (byte)(height & 255);
			array2[15] = (byte)((height & 65280) >> 8);
			array2[16] = 32;
			byte[] array3 = array2;
			using (BinaryWriter binaryWriter = new BinaryWriter(output))
			{
				binaryWriter.Write(array3);
				binaryWriter.Write(array);
			}
		}
	}
}
