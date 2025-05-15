using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002A5 RID: 677
	public class LocalizationReader
	{
		// Token: 0x06001132 RID: 4402 RVA: 0x0005EA34 File Offset: 0x0005CC34
		public static Dictionary<string, string> ReadTextAsset(TextAsset asset)
		{
			StringReader stringReader = new StringReader(Encoding.UTF8.GetString(asset.bytes, 0, asset.bytes.Length).Replace("\r\n", "\n").Replace("\r", "\n"));
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				string text2;
				string text3;
				string text4;
				string text5;
				string text6;
				if (LocalizationReader.TextAsset_ReadLine(text, out text2, out text3, out text4, out text5, out text6) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
				{
					dictionary[text2] = text3;
				}
			}
			return dictionary;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0005EAC4 File Offset: 0x0005CCC4
		public static bool TextAsset_ReadLine(string line, out string key, out string value, out string category, out string comment, out string termType)
		{
			key = string.Empty;
			category = string.Empty;
			comment = string.Empty;
			termType = string.Empty;
			value = string.Empty;
			int num = line.LastIndexOf("//");
			if (num >= 0)
			{
				comment = line.Substring(num + 2).Trim();
				comment = LocalizationReader.DecodeString(comment);
				line = line.Substring(0, num);
			}
			int num2 = line.IndexOf("=");
			if (num2 < 0)
			{
				return false;
			}
			key = line.Substring(0, num2).Trim();
			value = line.Substring(num2 + 1).Trim();
			value = value.Replace("\r\n", "\n").Replace("\n", "\\n");
			value = LocalizationReader.DecodeString(value);
			if (key.Length > 2 && key[0] == '[')
			{
				int num3 = key.IndexOf(']');
				if (num3 >= 0)
				{
					termType = key.Substring(1, num3 - 1);
					key = key.Substring(num3 + 1);
				}
			}
			LocalizationReader.ValidateFullTerm(ref key);
			return true;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0005EBD0 File Offset: 0x0005CDD0
		public static string ReadCSVfile(string Path, Encoding encoding)
		{
			string text = string.Empty;
			using (StreamReader streamReader = new StreamReader(Path, encoding))
			{
				text = streamReader.ReadToEnd();
			}
			text = text.Replace("\r\n", "\n");
			text = text.Replace("\r", "\n");
			return text;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0005EC34 File Offset: 0x0005CE34
		public static List<string[]> ReadCSV(string Text, char Separator = ',')
		{
			int i = 0;
			List<string[]> list = new List<string[]>();
			while (i < Text.Length)
			{
				string[] array = LocalizationReader.ParseCSVline(Text, ref i, Separator);
				if (array == null)
				{
					break;
				}
				list.Add(array);
			}
			return list;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x0005EC6C File Offset: 0x0005CE6C
		private static string[] ParseCSVline(string Line, ref int iStart, char Separator)
		{
			List<string> list = new List<string>();
			int length = Line.Length;
			int num = iStart;
			bool flag = false;
			while (iStart < length)
			{
				char c = Line[iStart];
				if (flag)
				{
					if (c == '"')
					{
						if (iStart + 1 >= length || Line[iStart + 1] != '"')
						{
							flag = false;
						}
						else if (iStart + 2 < length && Line[iStart + 2] == '"')
						{
							flag = false;
							iStart += 2;
						}
						else
						{
							iStart++;
						}
					}
				}
				else if (c == '\n' || c == Separator)
				{
					LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref num);
					if (c == '\n')
					{
						iStart++;
						break;
					}
				}
				else if (c == '"')
				{
					flag = true;
				}
				iStart++;
			}
			if (iStart > num)
			{
				LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref num);
			}
			return list.ToArray();
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0005ED38 File Offset: 0x0005CF38
		private static void AddCSVtoken(ref List<string> list, ref string Line, int iEnd, ref int iWordStart)
		{
			string text = Line.Substring(iWordStart, iEnd - iWordStart);
			iWordStart = iEnd + 1;
			text = text.Replace("\"\"", "\"");
			if (text.Length > 1 && text[0] == '"' && text[text.Length - 1] == '"')
			{
				text = text.Substring(1, text.Length - 2);
			}
			list.Add(text);
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0005EDA8 File Offset: 0x0005CFA8
		public static List<string[]> ReadI2CSV(string Text)
		{
			string[] array = new string[] { "[*]" };
			string[] array2 = new string[] { "[ln]" };
			List<string[]> list = new List<string[]>();
			foreach (string text in Text.Split(array2, StringSplitOptions.None))
			{
				list.Add(text.Split(array, StringSplitOptions.None));
			}
			return list;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0005EE0C File Offset: 0x0005D00C
		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			int num = Term.IndexOf('/');
			if (num < 0)
			{
				return;
			}
			int num2;
			while ((num2 = Term.LastIndexOf('/')) != num)
			{
				Term = Term.Remove(num2, 1);
			}
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0005EE4E File Offset: 0x0005D04E
		public static string EncodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("\r\n", "<\\n>").Replace("\r", "<\\n>").Replace("\n", "<\\n>");
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0005EE8C File Offset: 0x0005D08C
		public static string DecodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("<\\n>", "\r\n");
		}
	}
}
