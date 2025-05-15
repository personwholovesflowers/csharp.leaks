using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ferr
{
	// Token: 0x020002E5 RID: 741
	public static class DataStringUtil
	{
		// Token: 0x0600135D RID: 4957 RVA: 0x00066E40 File Offset: 0x00065040
		public static string Encrypt(string aData, string aKey = null)
		{
			if (string.IsNullOrEmpty(aKey))
			{
				aKey = DataStringUtil._key;
			}
			byte[] bytes = Encoding.Unicode.GetBytes(aData);
			using (Aes aes = Aes.Create())
			{
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(aKey, new byte[]
				{
					73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
					100, 101, 118
				});
				aes.Key = rfc2898DeriveBytes.GetBytes(32);
				aes.IV = rfc2898DeriveBytes.GetBytes(16);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytes, 0, bytes.Length);
						cryptoStream.Close();
					}
					aData = Convert.ToBase64String(memoryStream.ToArray());
				}
			}
			return aData;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00066F28 File Offset: 0x00065128
		public static string Decrypt(string aData, string aKey = null)
		{
			if (string.IsNullOrEmpty(aKey))
			{
				aKey = DataStringUtil._key;
			}
			aData = aData.Replace(" ", "+");
			byte[] array = Convert.FromBase64String(aData);
			using (Aes aes = Aes.Create())
			{
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(aKey, new byte[]
				{
					73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
					100, 101, 118
				});
				aes.Key = rfc2898DeriveBytes.GetBytes(32);
				aes.IV = rfc2898DeriveBytes.GetBytes(16);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(array, 0, array.Length);
						cryptoStream.Close();
					}
					aData = Encoding.Unicode.GetString(memoryStream.ToArray());
				}
			}
			return aData;
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00067020 File Offset: 0x00065220
		public static List<string> SplitSmart(string aData, char aSeparator)
		{
			List<string> list = new List<string>();
			string text = "";
			char c = ' ';
			int num = 0;
			string text2 = aData.Trim();
			if (text2.StartsWith("{"))
			{
				text2 = text2.Substring(1, text2.Length - 2);
			}
			int i = 0;
			while (i < text2.Length)
			{
				char c2 = text2[i];
				if (c == ' ')
				{
					if (c2 == aSeparator)
					{
						list.Add(text);
						text = "";
					}
					else
					{
						if (c2 == '{')
						{
							c = '}';
							goto IL_00A1;
						}
						if (c2 == '"')
						{
							c = '"';
							goto IL_00A1;
						}
						if (c2 == '\'')
						{
							c = '\'';
							goto IL_00A1;
						}
						goto IL_00A1;
					}
				}
				else if (c2 == c)
				{
					if (num == 0)
					{
						c = ' ';
						goto IL_00A1;
					}
					num--;
					goto IL_00A1;
				}
				else
				{
					if (c2 == '{')
					{
						num++;
						goto IL_00A1;
					}
					goto IL_00A1;
				}
				IL_00AF:
				i++;
				continue;
				IL_00A1:
				text += c2.ToString();
				goto IL_00AF;
			}
			if (text.Length > 0)
			{
				list.Add(text);
			}
			if (c != ' ')
			{
				return null;
			}
			return list;
		}

		// Token: 0x04000F1B RID: 3867
		private static string _key = "FerrDataStringUtilDefaultKey";
	}
}
