using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002A8 RID: 680
	public static class LocalizationManager
	{
		// Token: 0x0600115E RID: 4446 RVA: 0x0005FBD3 File Offset: 0x0005DDD3
		public static void InitializeIfNeeded()
		{
			if (string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage) || LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.AutoLoadGlobalParamManagers();
				LocalizationManager.UpdateSources();
				LocalizationManager.SelectStartupLanguage();
			}
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0005FBFD File Offset: 0x0005DDFD
		public static string GetVersion()
		{
			return "2.8.13 f2";
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0005FC04 File Offset: 0x0005DE04
		public static int GetRequiredWebServiceVersion()
		{
			return 5;
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0005FC08 File Offset: 0x0005DE08
		public static string GetWebServiceURL(LanguageSourceData source = null)
		{
			if (source != null && !string.IsNullOrEmpty(source.Google_WebServiceURL))
			{
				return source.Google_WebServiceURL;
			}
			LocalizationManager.InitializeIfNeeded();
			for (int i = 0; i < LocalizationManager.Sources.Count; i++)
			{
				if (LocalizationManager.Sources[i] != null && !string.IsNullOrEmpty(LocalizationManager.Sources[i].Google_WebServiceURL))
				{
					return LocalizationManager.Sources[i].Google_WebServiceURL;
				}
			}
			return string.Empty;
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x0005FC80 File Offset: 0x0005DE80
		// (set) Token: 0x06001163 RID: 4451 RVA: 0x0005FC8C File Offset: 0x0005DE8C
		public static string CurrentLanguage
		{
			get
			{
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mCurrentLanguage;
			}
			set
			{
				LocalizationManager.InitializeIfNeeded();
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(value, false);
				if (!string.IsNullOrEmpty(supportedLanguage) && LocalizationManager.mCurrentLanguage != supportedLanguage)
				{
					LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), true, false);
				}
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x0005FCC9 File Offset: 0x0005DEC9
		// (set) Token: 0x06001165 RID: 4453 RVA: 0x0005FCD8 File Offset: 0x0005DED8
		public static string CurrentLanguageCode
		{
			get
			{
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mLanguageCode;
			}
			set
			{
				LocalizationManager.InitializeIfNeeded();
				if (LocalizationManager.mLanguageCode != value)
				{
					string languageFromCode = LocalizationManager.GetLanguageFromCode(value, true);
					if (!string.IsNullOrEmpty(languageFromCode))
					{
						LocalizationManager.SetLanguageAndCode(languageFromCode, value, true, false);
					}
				}
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x0005FD10 File Offset: 0x0005DF10
		// (set) Token: 0x06001167 RID: 4455 RVA: 0x0005FD80 File Offset: 0x0005DF80
		public static string CurrentRegion
		{
			get
			{
				string currentLanguage = LocalizationManager.CurrentLanguage;
				int num = currentLanguage.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					return currentLanguage.Substring(num + 1);
				}
				num = currentLanguage.IndexOfAny("[(".ToCharArray());
				int num2 = currentLanguage.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					return currentLanguage.Substring(num + 1, num2 - num - 1);
				}
				return string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguage;
				int num = text.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					LocalizationManager.CurrentLanguage = text.Substring(num + 1) + value;
					return;
				}
				num = text.IndexOfAny("[(".ToCharArray());
				int num2 = text.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					text = text.Substring(num);
				}
				LocalizationManager.CurrentLanguage = text + "(" + value + ")";
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06001168 RID: 4456 RVA: 0x0005FE08 File Offset: 0x0005E008
		// (set) Token: 0x06001169 RID: 4457 RVA: 0x0005FE40 File Offset: 0x0005E040
		public static string CurrentRegionCode
		{
			get
			{
				string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
				int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
				if (num >= 0)
				{
					return currentLanguageCode.Substring(num + 1);
				}
				return string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguageCode;
				int num = text.IndexOfAny(" -_/\\".ToCharArray());
				if (num > 0)
				{
					text = text.Substring(0, num);
				}
				LocalizationManager.CurrentLanguageCode = text + "-" + value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x0005FE82 File Offset: 0x0005E082
		public static CultureInfo CurrentCulture
		{
			get
			{
				return LocalizationManager.mCurrentCulture;
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0005FE8C File Offset: 0x0005E08C
		public static void SetLanguageAndCode(string LanguageName, string LanguageCode, bool RememberLanguage = true, bool Force = false)
		{
			if (LocalizationManager.mCurrentLanguage != LanguageName || LocalizationManager.mLanguageCode != LanguageCode || Force)
			{
				if (RememberLanguage)
				{
					PersistentStorage.SetSetting_String("I2 Language", LanguageName);
				}
				LocalizationManager.mCurrentLanguage = LanguageName;
				LocalizationManager.mLanguageCode = LanguageCode;
				LocalizationManager.mCurrentCulture = LocalizationManager.CreateCultureForCode(LanguageCode);
				if (LocalizationManager.mChangeCultureInfo)
				{
					LocalizationManager.SetCurrentCultureInfo();
				}
				LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
				LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
				LocalizationManager.LocalizeAll(Force);
			}
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0005FF10 File Offset: 0x0005E110
		private static CultureInfo CreateCultureForCode(string code)
		{
			CultureInfo cultureInfo;
			try
			{
				cultureInfo = CultureInfo.CreateSpecificCulture(code);
			}
			catch (Exception)
			{
				cultureInfo = CultureInfo.InvariantCulture;
			}
			return cultureInfo;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0005FF40 File Offset: 0x0005E140
		public static void EnableChangingCultureInfo(bool bEnable)
		{
			if (!LocalizationManager.mChangeCultureInfo && bEnable)
			{
				LocalizationManager.SetCurrentCultureInfo();
			}
			LocalizationManager.mChangeCultureInfo = bEnable;
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0005FF59 File Offset: 0x0005E159
		private static void SetCurrentCultureInfo()
		{
			Thread.CurrentThread.CurrentCulture = LocalizationManager.mCurrentCulture;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0005FF6C File Offset: 0x0005E16C
		private static void SelectStartupLanguage()
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				return;
			}
			string setting_String = PersistentStorage.GetSetting_String("I2 Language", string.Empty);
			string currentDeviceLanguage = LocalizationManager.GetCurrentDeviceLanguage(false);
			if (!string.IsNullOrEmpty(setting_String) && LocalizationManager.HasLanguage(setting_String, true, false, true))
			{
				LocalizationManager.SetLanguageAndCode(setting_String, LocalizationManager.GetLanguageCode(setting_String), true, false);
				return;
			}
			if (!LocalizationManager.Sources[0].IgnoreDeviceLanguage)
			{
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(currentDeviceLanguage, true);
				if (!string.IsNullOrEmpty(supportedLanguage))
				{
					LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), false, false);
					return;
				}
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].mLanguages.Count > 0)
				{
					for (int j = 0; j < LocalizationManager.Sources[i].mLanguages.Count; j++)
					{
						if (LocalizationManager.Sources[i].mLanguages[j].IsEnabled())
						{
							LocalizationManager.SetLanguageAndCode(LocalizationManager.Sources[i].mLanguages[j].Name, LocalizationManager.Sources[i].mLanguages[j].Code, false, false);
							return;
						}
					}
				}
				i++;
			}
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x000600AC File Offset: 0x0005E2AC
		public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true, bool Initialize = true, bool SkipDisabled = true)
		{
			if (Initialize)
			{
				LocalizationManager.InitializeIfNeeded();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].GetLanguageIndex(Language, false, SkipDisabled) >= 0)
				{
					return true;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					if (LocalizationManager.Sources[j].GetLanguageIndex(Language, true, SkipDisabled) >= 0)
					{
						return true;
					}
					j++;
				}
			}
			return false;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00060124 File Offset: 0x0005E324
		public static string GetSupportedLanguage(string Language, bool ignoreDisabled = false)
		{
			string languageCode = GoogleLanguages.GetLanguageCode(Language, false);
			if (!string.IsNullOrEmpty(languageCode))
			{
				int i = 0;
				int count = LocalizationManager.Sources.Count;
				while (i < count)
				{
					int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(languageCode, true, ignoreDisabled);
					if (languageIndexFromCode >= 0)
					{
						return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
					}
					i++;
				}
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					int languageIndexFromCode2 = LocalizationManager.Sources[j].GetLanguageIndexFromCode(languageCode, false, ignoreDisabled);
					if (languageIndexFromCode2 >= 0)
					{
						return LocalizationManager.Sources[j].mLanguages[languageIndexFromCode2].Name;
					}
					j++;
				}
			}
			int k = 0;
			int count3 = LocalizationManager.Sources.Count;
			while (k < count3)
			{
				int languageIndex = LocalizationManager.Sources[k].GetLanguageIndex(Language, false, ignoreDisabled);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[k].mLanguages[languageIndex].Name;
				}
				k++;
			}
			int l = 0;
			int count4 = LocalizationManager.Sources.Count;
			while (l < count4)
			{
				int languageIndex2 = LocalizationManager.Sources[l].GetLanguageIndex(Language, true, ignoreDisabled);
				if (languageIndex2 >= 0)
				{
					return LocalizationManager.Sources[l].mLanguages[languageIndex2].Name;
				}
				l++;
			}
			return string.Empty;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00060298 File Offset: 0x0005E498
		public static string GetLanguageCode(string Language)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, true, true);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndex].Code;
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00060308 File Offset: 0x0005E508
		public static string GetLanguageFromCode(string Code, bool exactMatch = true)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(Code, exactMatch, false);
				if (languageIndexFromCode >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00060378 File Offset: 0x0005E578
		public static List<string> GetAllLanguages(bool SkipDisabled = true)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			List<string> Languages = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			Func<string, bool> <>9__0;
			while (i < count)
			{
				List<string> languages = Languages;
				IEnumerable<string> languages2 = LocalizationManager.Sources[i].GetLanguages(SkipDisabled);
				Func<string, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (string x) => !Languages.Contains(x));
				}
				languages.AddRange(languages2.Where(func));
				i++;
			}
			return Languages;
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00060408 File Offset: 0x0005E608
		public static List<string> GetAllLanguagesCode(bool allowRegions = true, bool SkipDisabled = true)
		{
			List<string> Languages = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			Func<string, bool> <>9__0;
			while (i < count)
			{
				List<string> languages = Languages;
				IEnumerable<string> languagesCode = LocalizationManager.Sources[i].GetLanguagesCode(allowRegions, SkipDisabled);
				Func<string, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (string x) => !Languages.Contains(x));
				}
				languages.AddRange(languagesCode.Where(func));
				i++;
			}
			return Languages;
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00060484 File Offset: 0x0005E684
		public static bool IsLanguageEnabled(string Language)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (!LocalizationManager.Sources[i].IsLanguageEnabled(Language))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x000604C0 File Offset: 0x0005E6C0
		private static void LoadCurrentLanguage()
		{
			for (int i = 0; i < LocalizationManager.Sources.Count; i++)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(LocalizationManager.mCurrentLanguage, true, false);
				LocalizationManager.Sources[i].LoadLanguage(languageIndex, true, true, true, false);
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0006050F File Offset: 0x0005E70F
		public static void PreviewLanguage(string NewLanguage)
		{
			LocalizationManager.mCurrentLanguage = NewLanguage;
			LocalizationManager.mLanguageCode = LocalizationManager.GetLanguageCode(LocalizationManager.mCurrentLanguage);
			LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
			LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00060544 File Offset: 0x0005E744
		public static void AutoLoadGlobalParamManagers()
		{
			foreach (LocalizationParamsManager localizationParamsManager in Object.FindObjectsOfType<LocalizationParamsManager>())
			{
				if (localizationParamsManager._IsGlobalManager && !LocalizationManager.ParamManagers.Contains(localizationParamsManager))
				{
					Debug.Log(localizationParamsManager);
					LocalizationManager.ParamManagers.Add(localizationParamsManager);
				}
			}
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0006058F File Offset: 0x0005E78F
		public static void ApplyLocalizationParams(ref string translation, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, null), allowLocalizedParameters);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x000605B8 File Offset: 0x0005E7B8
		public static void ApplyLocalizationParams(ref string translation, GameObject root, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, root), allowLocalizedParameters);
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000605E8 File Offset: 0x0005E7E8
		public static void ApplyLocalizationParams(ref string translation, Dictionary<string, object> parameters, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, delegate(string p)
			{
				object obj = null;
				if (parameters.TryGetValue(p, out obj))
				{
					return obj;
				}
				return null;
			}, allowLocalizedParameters);
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00060618 File Offset: 0x0005E818
		public static void ApplyLocalizationParams(ref string translation, LocalizationManager._GetParam getParam, bool allowLocalizedParameters = true)
		{
			if (translation == null)
			{
				return;
			}
			string text = null;
			int num = translation.Length;
			int num2 = 0;
			while (num2 >= 0 && num2 < translation.Length)
			{
				int num3 = translation.IndexOf("{[", num2);
				if (num3 < 0)
				{
					break;
				}
				int num4 = translation.IndexOf("]}", num3);
				if (num4 < 0)
				{
					break;
				}
				int num5 = translation.IndexOf("{[", num3 + 1);
				if (num5 > 0 && num5 < num4)
				{
					num2 = num5;
				}
				else
				{
					int num6 = ((translation[num3 + 2] == '#') ? 3 : 2);
					string text2 = translation.Substring(num3 + num6, num4 - num3 - num6);
					string text3 = (string)getParam(text2);
					if (text3 != null && allowLocalizedParameters)
					{
						LanguageSourceData languageSourceData;
						TermData termData = LocalizationManager.GetTermData(text3, out languageSourceData);
						if (termData != null)
						{
							int languageIndex = languageSourceData.GetLanguageIndex(LocalizationManager.CurrentLanguage, true, true);
							if (languageIndex >= 0)
							{
								text3 = termData.GetTranslation(languageIndex, null, false);
							}
						}
						string text4 = translation.Substring(num3, num4 - num3 + 2);
						translation = translation.Replace(text4, text3);
						int num7 = 0;
						if (int.TryParse(text3, out num7))
						{
							text = GoogleLanguages.GetPluralType(LocalizationManager.CurrentLanguageCode, num7).ToString();
						}
						num2 = num3 + text3.Length;
					}
					else
					{
						num2 = num4 + 2;
					}
				}
			}
			if (text != null)
			{
				string text5 = "[i2p_" + text + "]";
				int num8 = translation.IndexOf(text5, StringComparison.OrdinalIgnoreCase);
				if (num8 < 0)
				{
					num8 = 0;
				}
				else
				{
					num8 += text5.Length;
				}
				num = translation.IndexOf("[i2p_", num8 + 1, StringComparison.OrdinalIgnoreCase);
				if (num < 0)
				{
					num = translation.Length;
				}
				translation = translation.Substring(num8, num - num8);
			}
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000607D0 File Offset: 0x0005E9D0
		internal static string GetLocalizationParam(string ParamName, GameObject root)
		{
			if (root)
			{
				MonoBehaviour[] components = root.GetComponents<MonoBehaviour>();
				int i = 0;
				int num = components.Length;
				while (i < num)
				{
					ILocalizationParamsManager localizationParamsManager = components[i] as ILocalizationParamsManager;
					if (localizationParamsManager != null && components[i].enabled)
					{
						string text = localizationParamsManager.GetParameterValue(ParamName);
						if (text != null)
						{
							return text;
						}
					}
					i++;
				}
			}
			int j = 0;
			int count = LocalizationManager.ParamManagers.Count;
			while (j < count)
			{
				string text = LocalizationManager.ParamManagers[j].GetParameterValue(ParamName);
				if (text != null)
				{
					return text;
				}
				j++;
			}
			return null;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0006085C File Offset: 0x0005EA5C
		private static string GetPluralType(MatchCollection matches, string langCode, LocalizationManager._GetParam getParam)
		{
			int i = 0;
			int count = matches.Count;
			while (i < count)
			{
				Match match = matches[i];
				string value = match.Groups[match.Groups.Count - 1].Value;
				string text = (string)getParam(value);
				if (text != null)
				{
					int num = 0;
					if (int.TryParse(text, out num))
					{
						return GoogleLanguages.GetPluralType(langCode, num).ToString();
					}
				}
				i++;
			}
			return null;
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000608DB File Offset: 0x0005EADB
		public static string ApplyRTLfix(string line)
		{
			return LocalizationManager.ApplyRTLfix(line, 0, true);
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x000608E8 File Offset: 0x0005EAE8
		public static string ApplyRTLfix(string line, int maxCharacters, bool ignoreNumbers)
		{
			if (string.IsNullOrEmpty(line))
			{
				return line;
			}
			char c = line[0];
			if (c == '!' || c == '.' || c == '?')
			{
				line = line.Substring(1) + c.ToString();
			}
			int num = -1;
			int num2 = 0;
			int num3 = 40000;
			num2 = 0;
			List<string> list = new List<string>();
			while (I2Utils.FindNextTag(line, num2, out num, out num2))
			{
				string text = "@@" + ((char)(num3 + list.Count)).ToString() + "@@";
				list.Add(line.Substring(num, num2 - num + 1));
				line = line.Substring(0, num) + text + line.Substring(num2 + 1);
				num2 = num + 5;
			}
			line = line.Replace("\r\n", "\n");
			line = I2Utils.SplitLine(line, maxCharacters);
			line = RTLFixer.Fix(line, true, !ignoreNumbers);
			for (int i = 0; i < list.Count; i++)
			{
				int length = line.Length;
				for (int j = 0; j < length; j++)
				{
					if (line[j] == '@' && line[j + 1] == '@' && (int)line[j + 2] >= num3 && line[j + 3] == '@' && line[j + 4] == '@')
					{
						int num4 = (int)line[j + 2] - num3;
						if (num4 % 2 == 0)
						{
							num4++;
						}
						else
						{
							num4--;
						}
						if (num4 >= list.Count)
						{
							num4 = list.Count - 1;
						}
						line = line.Substring(0, j) + list[num4] + line.Substring(j + 5);
						break;
					}
				}
			}
			return line;
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00060AAC File Offset: 0x0005ECAC
		public static string FixRTL_IfNeeded(string text, int maxCharacters = 0, bool ignoreNumber = false)
		{
			if (LocalizationManager.IsRight2Left)
			{
				return LocalizationManager.ApplyRTLfix(text, maxCharacters, ignoreNumber);
			}
			return text;
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x00060ABF File Offset: 0x0005ECBF
		public static bool IsRTL(string Code)
		{
			return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00060AD2 File Offset: 0x0005ECD2
		public static bool UpdateSources()
		{
			LocalizationManager.UnregisterDeletededSources();
			LocalizationManager.RegisterSourceInResources();
			LocalizationManager.RegisterSceneSources();
			return LocalizationManager.Sources.Count > 0;
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00060AF0 File Offset: 0x0005ECF0
		private static void UnregisterDeletededSources()
		{
			for (int i = LocalizationManager.Sources.Count - 1; i >= 0; i--)
			{
				if (LocalizationManager.Sources[i] == null)
				{
					LocalizationManager.RemoveSource(LocalizationManager.Sources[i]);
				}
			}
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00060B34 File Offset: 0x0005ED34
		private static void RegisterSceneSources()
		{
			foreach (LanguageSource languageSource in (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource)))
			{
				if (!LocalizationManager.Sources.Contains(languageSource.mSource))
				{
					if (languageSource.mSource.owner == null)
					{
						languageSource.mSource.owner = languageSource;
					}
					LocalizationManager.AddSource(languageSource.mSource);
				}
			}
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00060BA0 File Offset: 0x0005EDA0
		private static void RegisterSourceInResources()
		{
			foreach (string text in LocalizationManager.GlobalSources)
			{
				LanguageSourceAsset asset = ResourceManager.pInstance.GetAsset<LanguageSourceAsset>(text);
				if (asset && !LocalizationManager.Sources.Contains(asset.mSource))
				{
					if (!asset.mSource.mIsGlobalSource)
					{
						asset.mSource.mIsGlobalSource = true;
					}
					asset.mSource.owner = asset;
					LocalizationManager.AddSource(asset.mSource);
				}
			}
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00060C1C File Offset: 0x0005EE1C
		internal static void AddSource(LanguageSourceData Source)
		{
			if (LocalizationManager.Sources.Contains(Source))
			{
				return;
			}
			LocalizationManager.Sources.Add(Source);
			if (Source.HasGoogleSpreadsheet() && Source.GoogleUpdateFrequency != LanguageSourceData.eGoogleUpdateFrequency.Never)
			{
				Source.Import_Google_FromCache();
				bool flag = false;
				if (Source.GoogleUpdateDelay > 0f)
				{
					CoroutineManager.Start(LocalizationManager.Delayed_Import_Google(Source, Source.GoogleUpdateDelay, flag));
				}
				else
				{
					Source.Import_Google(false, flag);
				}
			}
			for (int i = 0; i < Source.mLanguages.Count<LanguageData>(); i++)
			{
				Source.mLanguages[i].SetLoaded(true);
			}
			if (Source.mDictionary.Count == 0)
			{
				Source.UpdateDictionary(true);
			}
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x00060CC1 File Offset: 0x0005EEC1
		private static IEnumerator Delayed_Import_Google(LanguageSourceData source, float delay, bool justCheck)
		{
			yield return new WaitForSeconds(delay);
			if (source != null)
			{
				source.Import_Google(false, justCheck);
			}
			yield break;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00060CDE File Offset: 0x0005EEDE
		internal static void RemoveSource(LanguageSourceData Source)
		{
			LocalizationManager.Sources.Remove(Source);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00060CEC File Offset: 0x0005EEEC
		public static bool IsGlobalSource(string SourceName)
		{
			return Array.IndexOf<string>(LocalizationManager.GlobalSources, SourceName) >= 0;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00060D00 File Offset: 0x0005EF00
		public static LanguageSourceData GetSourceContaining(string term, bool fallbackToFirst = true)
		{
			if (!string.IsNullOrEmpty(term))
			{
				int i = 0;
				int count = LocalizationManager.Sources.Count;
				while (i < count)
				{
					if (LocalizationManager.Sources[i].GetTermData(term, false) != null)
					{
						return LocalizationManager.Sources[i];
					}
					i++;
				}
			}
			if (!fallbackToFirst || LocalizationManager.Sources.Count <= 0)
			{
				return null;
			}
			return LocalizationManager.Sources[0];
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00060D6C File Offset: 0x0005EF6C
		public static Object FindAsset(string value)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				Object @object = LocalizationManager.Sources[i].FindAsset(value);
				if (@object)
				{
					return @object;
				}
				i++;
			}
			return null;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x00060DB0 File Offset: 0x0005EFB0
		public static void ApplyDownloadedDataFromGoogle()
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LocalizationManager.Sources[i].ApplyDownloadedDataFromGoogle();
				i++;
			}
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00060DE4 File Offset: 0x0005EFE4
		public static string GetCurrentDeviceLanguage(bool force = false)
		{
			if (force || string.IsNullOrEmpty(LocalizationManager.mCurrentDeviceLanguage))
			{
				LocalizationManager.DetectDeviceLanguage();
			}
			return LocalizationManager.mCurrentDeviceLanguage;
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00060E00 File Offset: 0x0005F000
		private static void DetectDeviceLanguage()
		{
			LocalizationManager.mCurrentDeviceLanguage = Application.systemLanguage.ToString();
			if (LocalizationManager.mCurrentDeviceLanguage == "ChineseSimplified")
			{
				LocalizationManager.mCurrentDeviceLanguage = "Chinese (Simplified)";
			}
			if (LocalizationManager.mCurrentDeviceLanguage == "ChineseTraditional")
			{
				LocalizationManager.mCurrentDeviceLanguage = "Chinese (Traditional)";
			}
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00060E5C File Offset: 0x0005F05C
		public static void RegisterTarget(ILocalizeTargetDescriptor desc)
		{
			if (LocalizationManager.mLocalizeTargets.FindIndex((ILocalizeTargetDescriptor x) => x.Name == desc.Name) != -1)
			{
				return;
			}
			for (int i = 0; i < LocalizationManager.mLocalizeTargets.Count; i++)
			{
				if (LocalizationManager.mLocalizeTargets[i].Priority > desc.Priority)
				{
					LocalizationManager.mLocalizeTargets.Insert(i, desc);
					return;
				}
			}
			LocalizationManager.mLocalizeTargets.Add(desc);
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06001192 RID: 4498 RVA: 0x00060EE4 File Offset: 0x0005F0E4
		// (remove) Token: 0x06001193 RID: 4499 RVA: 0x00060F18 File Offset: 0x0005F118
		public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent;

		// Token: 0x06001194 RID: 4500 RVA: 0x00060F4C File Offset: 0x0005F14C
		public static string GetTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
		{
			string text = null;
			LocalizationManager.TryGetTranslation(Term, out text, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage);
			return text;
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00060F6E File Offset: 0x0005F16E
		public static string GetTermTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
		{
			return LocalizationManager.GetTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00060F80 File Offset: 0x0005F180
		public static bool TryGetTranslation(string Term, out string Translation, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
		{
			Translation = null;
			if (string.IsNullOrEmpty(Term))
			{
				return false;
			}
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].TryGetTranslation(Term, out Translation, overrideLanguage, null, false, false))
				{
					if (applyParameters)
					{
						LocalizationManager.ApplyLocalizationParams(ref Translation, localParametersRoot, true);
					}
					if (LocalizationManager.IsRight2Left && FixForRTL)
					{
						Translation = LocalizationManager.ApplyRTLfix(Translation, maxLineLengthForRTL, ignoreRTLnumbers);
					}
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00060FF4 File Offset: 0x0005F1F4
		public static T GetTranslatedObject<T>(string AssetName, Localize optionalLocComp = null) where T : Object
		{
			if (optionalLocComp != null)
			{
				return optionalLocComp.FindTranslatedObject<T>(AssetName);
			}
			T t = LocalizationManager.FindAsset(AssetName) as T;
			if (t)
			{
				return t;
			}
			return ResourceManager.pInstance.GetAsset<T>(AssetName);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0006103F File Offset: 0x0005F23F
		public static T GetTranslatedObjectByTermName<T>(string Term, Localize optionalLocComp = null) where T : Object
		{
			return LocalizationManager.GetTranslatedObject<T>(LocalizationManager.GetTranslation(Term, false, 0, true, false, null, null), null);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00061054 File Offset: 0x0005F254
		public static string GetAppName(string languageCode)
		{
			if (!string.IsNullOrEmpty(languageCode))
			{
				for (int i = 0; i < LocalizationManager.Sources.Count; i++)
				{
					if (!string.IsNullOrEmpty(LocalizationManager.Sources[i].mTerm_AppName))
					{
						int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(languageCode, false, false);
						if (languageIndexFromCode >= 0)
						{
							TermData termData = LocalizationManager.Sources[i].GetTermData(LocalizationManager.Sources[i].mTerm_AppName, false);
							if (termData != null)
							{
								string translation = termData.GetTranslation(languageIndexFromCode, null, false);
								if (!string.IsNullOrEmpty(translation))
								{
									return translation;
								}
							}
						}
					}
				}
			}
			return Application.productName;
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x000610EB File Offset: 0x0005F2EB
		public static void LocalizeAll(bool Force = false)
		{
			LocalizationManager.LoadCurrentLanguage();
			if (!Application.isPlaying)
			{
				LocalizationManager.DoLocalizeAll(Force);
				return;
			}
			LocalizationManager.mLocalizeIsScheduledWithForcedValue = LocalizationManager.mLocalizeIsScheduledWithForcedValue || Force;
			if (LocalizationManager.mLocalizeIsScheduled)
			{
				return;
			}
			CoroutineManager.Start(LocalizationManager.Coroutine_LocalizeAll());
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0006111F File Offset: 0x0005F31F
		private static IEnumerator Coroutine_LocalizeAll()
		{
			LocalizationManager.mLocalizeIsScheduled = true;
			yield return null;
			LocalizationManager.mLocalizeIsScheduled = false;
			bool flag = LocalizationManager.mLocalizeIsScheduledWithForcedValue;
			LocalizationManager.mLocalizeIsScheduledWithForcedValue = false;
			LocalizationManager.DoLocalizeAll(flag);
			yield break;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00061128 File Offset: 0x0005F328
		private static void DoLocalizeAll(bool Force = false)
		{
			Localize[] array = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				array[i].OnLocalize(Force);
				i++;
			}
			if (LocalizationManager.OnLocalizeEvent != null)
			{
				LocalizationManager.OnLocalizeEvent();
			}
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00061174 File Offset: 0x0005F374
		public static List<string> GetCategories()
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LocalizationManager.Sources[i].GetCategories(false, list);
				i++;
			}
			return list;
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x000611B4 File Offset: 0x0005F3B4
		public static List<string> GetTermsList(string Category = null)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			if (LocalizationManager.Sources.Count == 1)
			{
				return LocalizationManager.Sources[0].GetTermsList(Category);
			}
			HashSet<string> hashSet = new HashSet<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				hashSet.UnionWith(LocalizationManager.Sources[i].GetTermsList(Category));
				i++;
			}
			return new List<string>(hashSet);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0006122C File Offset: 0x0005F42C
		public static TermData GetTermData(string term)
		{
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
				if (termData != null)
				{
					return termData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00061270 File Offset: 0x0005F470
		public static TermData GetTermData(string term, out LanguageSourceData source)
		{
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
				if (termData != null)
				{
					source = LocalizationManager.Sources[i];
					return termData;
				}
				i++;
			}
			source = null;
			return null;
		}

		// Token: 0x04000E54 RID: 3668
		private static string mCurrentLanguage;

		// Token: 0x04000E55 RID: 3669
		private static string mLanguageCode;

		// Token: 0x04000E56 RID: 3670
		private static CultureInfo mCurrentCulture;

		// Token: 0x04000E57 RID: 3671
		private static bool mChangeCultureInfo = false;

		// Token: 0x04000E58 RID: 3672
		public static bool IsRight2Left = false;

		// Token: 0x04000E59 RID: 3673
		public static bool HasJoinedWords = false;

		// Token: 0x04000E5A RID: 3674
		public static List<ILocalizationParamsManager> ParamManagers = new List<ILocalizationParamsManager>();

		// Token: 0x04000E5B RID: 3675
		private static string[] LanguagesRTL = new string[]
		{
			"ar-DZ", "ar", "ar-BH", "ar-EG", "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA",
			"ar-OM", "ar-QA", "ar-SA", "ar-SY", "ar-TN", "ar-AE", "ar-YE", "fa", "he", "ur",
			"ji"
		};

		// Token: 0x04000E5C RID: 3676
		public static List<LanguageSourceData> Sources = new List<LanguageSourceData>();

		// Token: 0x04000E5D RID: 3677
		public static string[] GlobalSources = new string[] { "I2Languages" };

		// Token: 0x04000E5E RID: 3678
		private static string mCurrentDeviceLanguage;

		// Token: 0x04000E5F RID: 3679
		public static List<ILocalizeTargetDescriptor> mLocalizeTargets = new List<ILocalizeTargetDescriptor>();

		// Token: 0x04000E61 RID: 3681
		private static bool mLocalizeIsScheduled = false;

		// Token: 0x04000E62 RID: 3682
		private static bool mLocalizeIsScheduledWithForcedValue = false;

		// Token: 0x04000E63 RID: 3683
		public static bool HighlightLocalizedTargets = false;

		// Token: 0x0200048F RID: 1167
		// (Invoke) Token: 0x060019AC RID: 6572
		public delegate object _GetParam(string param);

		// Token: 0x02000490 RID: 1168
		// (Invoke) Token: 0x060019B0 RID: 6576
		public delegate void OnLocalizeCallback();
	}
}
