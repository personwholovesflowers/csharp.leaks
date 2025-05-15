using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x020002C3 RID: 707
	public static class I2Utils
	{
		// Token: 0x0600124A RID: 4682 RVA: 0x00062C40 File Offset: 0x00060E40
		public static string ReverseText(string source)
		{
			int length = source.Length;
			char[] array = new char[length];
			for (int i = 0; i < length; i++)
			{
				array[length - 1 - i] = source[i];
			}
			return new string(array);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00062C7C File Offset: 0x00060E7C
		public static string RemoveNonASCII(string text, bool allowCategory = false)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			int num = 0;
			char[] array = new char[text.Length];
			bool flag = false;
			foreach (char c in text.Trim().ToCharArray())
			{
				char c2 = ' ';
				if ((allowCategory && (c == '\\' || c == '"' || c == '/')) || char.IsLetterOrDigit(c) || ".-_$#@*()[]{}+:?!&',^=<>~`".IndexOf(c) >= 0)
				{
					c2 = c;
				}
				if (char.IsWhiteSpace(c2))
				{
					if (!flag)
					{
						if (num > 0)
						{
							array[num++] = ' ';
						}
						flag = true;
					}
				}
				else
				{
					flag = false;
					array[num++] = c2;
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00062D2C File Offset: 0x00060F2C
		public static string GetValidTermName(string text, bool allowCategory = false)
		{
			if (text == null)
			{
				return null;
			}
			text = I2Utils.RemoveTags(text);
			return I2Utils.RemoveNonASCII(text, allowCategory);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00062D44 File Offset: 0x00060F44
		public static string SplitLine(string line, int maxCharacters)
		{
			if (maxCharacters <= 0 || line.Length < maxCharacters)
			{
				return line;
			}
			char[] array = line.ToCharArray();
			bool flag = true;
			bool flag2 = false;
			int i = 0;
			int num = 0;
			while (i < array.Length)
			{
				if (flag)
				{
					num++;
					if (array[i] == '\n')
					{
						num = 0;
					}
					if (num >= maxCharacters && char.IsWhiteSpace(array[i]))
					{
						array[i] = '\n';
						flag = false;
						flag2 = false;
					}
				}
				else if (!char.IsWhiteSpace(array[i]))
				{
					flag = true;
					num = 0;
				}
				else if (array[i] != '\n')
				{
					array[i] = '\0';
				}
				else
				{
					if (!flag2)
					{
						array[i] = '\0';
					}
					flag2 = true;
				}
				i++;
			}
			return new string(array.Where((char c) => c > '\0').ToArray<char>());
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00062E00 File Offset: 0x00061000
		public static bool FindNextTag(string line, int iStart, out int tagStart, out int tagEnd)
		{
			tagStart = -1;
			tagEnd = -1;
			int length = line.Length;
			tagStart = iStart;
			while (tagStart < length && line[tagStart] != '[' && line[tagStart] != '(' && line[tagStart] != '{' && line[tagStart] != '<')
			{
				tagStart++;
			}
			if (tagStart == length)
			{
				return false;
			}
			bool flag = false;
			for (tagEnd = tagStart + 1; tagEnd < length; tagEnd++)
			{
				char c = line[tagEnd];
				if (c == ']' || c == ')' || c == '}' || c == '>')
				{
					return !flag || I2Utils.FindNextTag(line, tagEnd + 1, out tagStart, out tagEnd);
				}
				if (c > 'ÿ')
				{
					flag = true;
				}
			}
			return false;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00062EB0 File Offset: 0x000610B0
		public static string RemoveTags(string text)
		{
			return Regex.Replace(text, "\\{\\[(.*?)]}|\\[(.*?)]|\\<(.*?)>", "");
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00062EC4 File Offset: 0x000610C4
		public static bool RemoveResourcesPath(ref string sPath)
		{
			int num = sPath.IndexOf("\\Resources\\");
			int num2 = sPath.IndexOf("\\Resources/");
			int num3 = sPath.IndexOf("/Resources\\");
			int num4 = sPath.IndexOf("/Resources/");
			int num5 = Mathf.Max(new int[] { num, num2, num3, num4 });
			bool flag = false;
			if (num5 >= 0)
			{
				sPath = sPath.Substring(num5 + 11);
				flag = true;
			}
			else
			{
				num5 = sPath.LastIndexOfAny(LanguageSourceData.CategorySeparators);
				if (num5 > 0)
				{
					sPath = sPath.Substring(num5 + 1);
				}
			}
			string extension = Path.GetExtension(sPath);
			if (!string.IsNullOrEmpty(extension))
			{
				sPath = sPath.Substring(0, sPath.Length - extension.Length);
			}
			return flag;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00062F8A File Offset: 0x0006118A
		public static bool IsPlaying()
		{
			return Application.isPlaying;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00062F98 File Offset: 0x00061198
		public static string GetPath(this Transform tr)
		{
			Transform parent = tr.parent;
			if (tr == null)
			{
				return tr.name;
			}
			return parent.GetPath() + "/" + tr.name;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00062FD2 File Offset: 0x000611D2
		public static Transform FindObject(string objectPath)
		{
			return I2Utils.FindObject(SceneManager.GetActiveScene(), objectPath);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00062FE0 File Offset: 0x000611E0
		public static Transform FindObject(Scene scene, string objectPath)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				Transform transform = rootGameObjects[i].transform;
				if (transform.name == objectPath)
				{
					return transform;
				}
				if (objectPath.StartsWith(transform.name + "/"))
				{
					return I2Utils.FindObject(transform, objectPath.Substring(transform.name.Length + 1));
				}
			}
			return null;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00063050 File Offset: 0x00061250
		public static Transform FindObject(Transform root, string objectPath)
		{
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				if (child.name == objectPath)
				{
					return child;
				}
				if (objectPath.StartsWith(child.name + "/"))
				{
					return I2Utils.FindObject(child, objectPath.Substring(child.name.Length + 1));
				}
			}
			return null;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x000630BC File Offset: 0x000612BC
		public static H FindInParents<H>(Transform tr) where H : Component
		{
			if (!tr)
			{
				return default(H);
			}
			H h = tr.GetComponent<H>();
			while (!h && tr)
			{
				h = tr.GetComponent<H>();
				tr = tr.parent;
			}
			return h;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0006310C File Offset: 0x0006130C
		public static string GetCaptureMatch(Match match)
		{
			for (int i = match.Groups.Count - 1; i >= 0; i--)
			{
				if (match.Groups[i].Success)
				{
					return match.Groups[i].ToString();
				}
			}
			return match.ToString();
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x0006315C File Offset: 0x0006135C
		public static void SendWebRequest(UnityWebRequest www)
		{
			www.SendWebRequest();
		}

		// Token: 0x04000E8F RID: 3727
		public const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

		// Token: 0x04000E90 RID: 3728
		public const string NumberChars = "0123456789";

		// Token: 0x04000E91 RID: 3729
		public const string ValidNameSymbols = ".-_$#@*()[]{}+:?!&',^=<>~`";
	}
}
