﻿using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x02000292 RID: 658
	public class SpecializationManager : BaseSpecializationManager
	{
		// Token: 0x06001082 RID: 4226 RVA: 0x00056762 File Offset: 0x00054962
		private SpecializationManager()
		{
			this.InitializeSpecializations();
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00056770 File Offset: 0x00054970
		public static string GetSpecializedText(string text, string specialization = null)
		{
			int num = text.IndexOf("[i2s_");
			if (num < 0)
			{
				return text;
			}
			if (string.IsNullOrEmpty(specialization))
			{
				specialization = SpecializationManager.Singleton.GetCurrentSpecialization();
			}
			while (!string.IsNullOrEmpty(specialization) && specialization != "Any")
			{
				string text2 = "[i2s_" + specialization + "]";
				int num2 = text.IndexOf(text2);
				if (num2 >= 0)
				{
					num2 += text2.Length;
					int num3 = text.IndexOf("[i2s_", num2);
					if (num3 < 0)
					{
						num3 = text.Length;
					}
					return text.Substring(num2, num3 - num2);
				}
				specialization = SpecializationManager.Singleton.GetFallbackSpecialization(specialization);
			}
			return text.Substring(0, num);
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0005681C File Offset: 0x00054A1C
		public static string SetSpecializedText(string text, string newText, string specialization)
		{
			if (string.IsNullOrEmpty(specialization))
			{
				specialization = "Any";
			}
			if ((text == null || !text.Contains("[i2s_")) && specialization == "Any")
			{
				return newText;
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			specializations[specialization] = newText;
			return SpecializationManager.SetSpecializedText(specializations);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0005686C File Offset: 0x00054A6C
		public static string SetSpecializedText(Dictionary<string, string> specializations)
		{
			string text;
			if (!specializations.TryGetValue("Any", out text))
			{
				text = string.Empty;
			}
			foreach (KeyValuePair<string, string> keyValuePair in specializations)
			{
				if (keyValuePair.Key != "Any" && !string.IsNullOrEmpty(keyValuePair.Value))
				{
					text = string.Concat(new string[] { text, "[i2s_", keyValuePair.Key, "]", keyValuePair.Value });
				}
			}
			return text;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00056920 File Offset: 0x00054B20
		public static Dictionary<string, string> GetSpecializations(string text, Dictionary<string, string> buffer = null)
		{
			if (buffer == null)
			{
				buffer = new Dictionary<string, string>();
			}
			else
			{
				buffer.Clear();
			}
			if (text == null)
			{
				buffer["Any"] = "";
				return buffer;
			}
			int num = text.IndexOf("[i2s_");
			if (num < 0)
			{
				num = text.Length;
			}
			buffer["Any"] = text.Substring(0, num);
			for (int i = num; i < text.Length; i = num)
			{
				i += "[i2s_".Length;
				int num2 = text.IndexOf(']', i);
				if (num2 < 0)
				{
					break;
				}
				string text2 = text.Substring(i, num2 - i);
				i = num2 + 1;
				num = text.IndexOf("[i2s_", i);
				if (num < 0)
				{
					num = text.Length;
				}
				string text3 = text.Substring(i, num - i);
				buffer[text2] = text3;
			}
			return buffer;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x000569E8 File Offset: 0x00054BE8
		public static void AppendSpecializations(string text, List<string> list = null)
		{
			if (text == null)
			{
				return;
			}
			if (list == null)
			{
				list = new List<string>();
			}
			if (!list.Contains("Any"))
			{
				list.Add("Any");
			}
			int i = 0;
			while (i < text.Length)
			{
				i = text.IndexOf("[i2s_", i);
				if (i < 0)
				{
					break;
				}
				i += "[i2s_".Length;
				int num = text.IndexOf(']', i);
				if (num < 0)
				{
					break;
				}
				string text2 = text.Substring(i, num - i);
				if (!list.Contains(text2))
				{
					list.Add(text2);
				}
			}
		}

		// Token: 0x04000DCE RID: 3534
		public static SpecializationManager Singleton = new SpecializationManager();
	}
}
