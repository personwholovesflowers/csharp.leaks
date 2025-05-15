using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x02000296 RID: 662
	public static class GoogleTranslation
	{
		// Token: 0x0600109C RID: 4252 RVA: 0x0005A8A7 File Offset: 0x00058AA7
		public static bool CanTranslate()
		{
			return LocalizationManager.Sources.Count > 0 && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(null));
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0005A8C8 File Offset: 0x00058AC8
		public static void Translate(string text, string LanguageCodeFrom, string LanguageCodeTo, GoogleTranslation.fnOnTranslated OnTranslationReady)
		{
			LocalizationManager.InitializeIfNeeded();
			if (!GoogleTranslation.CanTranslate())
			{
				OnTranslationReady(null, "WebService is not set correctly or needs to be reinstalled");
				return;
			}
			if (LanguageCodeTo == LanguageCodeFrom)
			{
				OnTranslationReady(text, null);
				return;
			}
			Dictionary<string, TranslationQuery> queries = new Dictionary<string, TranslationQuery>();
			if (string.IsNullOrEmpty(LanguageCodeTo))
			{
				OnTranslationReady(string.Empty, null);
				return;
			}
			GoogleTranslation.CreateQueries(text, LanguageCodeFrom, LanguageCodeTo, queries);
			GoogleTranslation.Translate(queries, delegate(Dictionary<string, TranslationQuery> results, string error)
			{
				if (!string.IsNullOrEmpty(error) || results.Count == 0)
				{
					OnTranslationReady(null, error);
					return;
				}
				string text2 = GoogleTranslation.RebuildTranslation(text, queries, LanguageCodeTo);
				OnTranslationReady(text2, null);
			}, true);
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0005A98C File Offset: 0x00058B8C
		public static string ForceTranslate(string text, string LanguageCodeFrom, string LanguageCodeTo)
		{
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery(text, LanguageCodeFrom, LanguageCodeTo, dictionary);
			TranslationJob_Main translationJob_Main = new TranslationJob_Main(dictionary, null);
			TranslationJob.eJobState state;
			do
			{
				state = translationJob_Main.GetState();
			}
			while (state == TranslationJob.eJobState.Running);
			if (state == TranslationJob.eJobState.Failed)
			{
				return null;
			}
			return GoogleTranslation.GetQueryResult(text, "", dictionary);
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0005A9CC File Offset: 0x00058BCC
		public static void Translate(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady, bool usePOST = true)
		{
			GoogleTranslation.AddTranslationJob(new TranslationJob_Main(requests, OnTranslationReady));
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0005A9DC File Offset: 0x00058BDC
		public static bool ForceTranslate(Dictionary<string, TranslationQuery> requests, bool usePOST = true)
		{
			TranslationJob_Main translationJob_Main = new TranslationJob_Main(requests, null);
			TranslationJob.eJobState state;
			do
			{
				state = translationJob_Main.GetState();
			}
			while (state == TranslationJob.eJobState.Running);
			return state != TranslationJob.eJobState.Failed;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0005AA04 File Offset: 0x00058C04
		public static List<string> ConvertTranslationRequest(Dictionary<string, TranslationQuery> requests, bool encodeGET)
		{
			List<string> list = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, TranslationQuery> keyValuePair in requests)
			{
				TranslationQuery value = keyValuePair.Value;
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("<I2Loc>");
				}
				stringBuilder.Append(GoogleLanguages.GetGoogleLanguageCode(value.LanguageCode));
				stringBuilder.Append(":");
				for (int i = 0; i < value.TargetLanguagesCode.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(GoogleLanguages.GetGoogleLanguageCode(value.TargetLanguagesCode[i]));
				}
				stringBuilder.Append("=");
				string text = ((GoogleTranslation.TitleCase(value.Text) == value.Text) ? value.Text.ToLowerInvariant() : value.Text);
				if (!encodeGET)
				{
					stringBuilder.Append(text);
				}
				else
				{
					stringBuilder.Append(Uri.EscapeDataString(text));
					if (stringBuilder.Length > 4000)
					{
						list.Add(stringBuilder.ToString());
						stringBuilder.Length = 0;
					}
				}
			}
			list.Add(stringBuilder.ToString());
			return list;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0005AB6C File Offset: 0x00058D6C
		private static void AddTranslationJob(TranslationJob job)
		{
			GoogleTranslation.mTranslationJobs.Add(job);
			if (GoogleTranslation.mTranslationJobs.Count == 1)
			{
				CoroutineManager.Start(GoogleTranslation.WaitForTranslations());
			}
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0005AB91 File Offset: 0x00058D91
		private static IEnumerator WaitForTranslations()
		{
			while (GoogleTranslation.mTranslationJobs.Count > 0)
			{
				foreach (TranslationJob translationJob in GoogleTranslation.mTranslationJobs.ToArray())
				{
					if (translationJob.GetState() != TranslationJob.eJobState.Running)
					{
						GoogleTranslation.mTranslationJobs.Remove(translationJob);
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0005AB9C File Offset: 0x00058D9C
		public static string ParseTranslationResult(string html, Dictionary<string, TranslationQuery> requests)
		{
			if (!html.StartsWith("<!DOCTYPE html>") && !html.StartsWith("<HTML>"))
			{
				string[] array = html.Split(new string[] { "<I2Loc>" }, StringSplitOptions.None);
				string[] array2 = new string[] { "<i2>" };
				int num = 0;
				foreach (string text in requests.Keys.ToArray<string>())
				{
					TranslationQuery translationQuery = GoogleTranslation.FindQueryFromOrigText(text, requests);
					string text2 = array[num++];
					if (translationQuery.Tags != null)
					{
						for (int j = translationQuery.Tags.Length - 1; j >= 0; j--)
						{
							text2 = text2.Replace(GoogleTranslation.GetGoogleNoTranslateTag(j), translationQuery.Tags[j]);
						}
					}
					translationQuery.Results = text2.Split(array2, StringSplitOptions.None);
					if (GoogleTranslation.TitleCase(text) == text)
					{
						for (int k = 0; k < translationQuery.Results.Length; k++)
						{
							translationQuery.Results[k] = GoogleTranslation.TitleCase(translationQuery.Results[k]);
						}
					}
					requests[translationQuery.OrigText] = translationQuery;
				}
				return null;
			}
			if (html.Contains("The script completed but did not return anything"))
			{
				return "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
			}
			if (html.Contains("Service invoked too many times in a short time"))
			{
				return "";
			}
			return "There was a problem contacting the WebService. Please try again later\n" + html;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0005ACF7 File Offset: 0x00058EF7
		public static bool IsTranslating()
		{
			return GoogleTranslation.mCurrentTranslations.Count > 0 || GoogleTranslation.mTranslationJobs.Count > 0;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0005AD18 File Offset: 0x00058F18
		public static void CancelCurrentGoogleTranslations()
		{
			GoogleTranslation.mCurrentTranslations.Clear();
			foreach (TranslationJob translationJob in GoogleTranslation.mTranslationJobs)
			{
				translationJob.Dispose();
			}
			GoogleTranslation.mTranslationJobs.Clear();
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0005AD7C File Offset: 0x00058F7C
		public static void CreateQueries(string text, string LanguageCodeFrom, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (!text.Contains("[i2s_"))
			{
				GoogleTranslation.CreateQueries_Plurals(text, LanguageCodeFrom, LanguageCodeTo, dict);
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in SpecializationManager.GetSpecializations(text, null))
			{
				GoogleTranslation.CreateQueries_Plurals(keyValuePair.Value, LanguageCodeFrom, LanguageCodeTo, dict);
			}
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0005ADF0 File Offset: 0x00058FF0
		private static void CreateQueries_Plurals(string text, string LanguageCodeFrom, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			bool flag = text.Contains("{[#");
			bool flag2 = text.Contains("[i2p_");
			if (!GoogleTranslation.HasParameters(text) || (!flag && !flag2))
			{
				GoogleTranslation.AddQuery(text, LanguageCodeFrom, LanguageCodeTo, dict);
				return;
			}
			bool flag3 = flag;
			for (ePluralType ePluralType = ePluralType.Zero; ePluralType <= ePluralType.Plural; ePluralType++)
			{
				string text2 = ePluralType.ToString();
				if (GoogleLanguages.LanguageHasPluralType(LanguageCodeTo, text2))
				{
					string text3 = GoogleTranslation.GetPluralText(text, text2);
					int pluralTestNumber = GoogleLanguages.GetPluralTestNumber(LanguageCodeTo, ePluralType);
					string pluralParameter = GoogleTranslation.GetPluralParameter(text3, flag3);
					if (!string.IsNullOrEmpty(pluralParameter))
					{
						text3 = text3.Replace(pluralParameter, pluralTestNumber.ToString());
					}
					GoogleTranslation.AddQuery(text3, LanguageCodeFrom, LanguageCodeTo, dict);
				}
			}
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0005AE98 File Offset: 0x00059098
		public static void AddQuery(string text, string LanguageCodeFrom, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (!dict.ContainsKey(text))
			{
				TranslationQuery translationQuery = new TranslationQuery
				{
					OrigText = text,
					LanguageCode = LanguageCodeFrom,
					TargetLanguagesCode = new string[] { LanguageCodeTo }
				};
				translationQuery.Text = text;
				GoogleTranslation.ParseNonTranslatableElements(ref translationQuery);
				dict[text] = translationQuery;
				return;
			}
			TranslationQuery translationQuery2 = dict[text];
			if (Array.IndexOf<string>(translationQuery2.TargetLanguagesCode, LanguageCodeTo) < 0)
			{
				translationQuery2.TargetLanguagesCode = translationQuery2.TargetLanguagesCode.Concat(new string[] { LanguageCodeTo }).Distinct<string>().ToArray<string>();
			}
			dict[text] = translationQuery2;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0005AF40 File Offset: 0x00059140
		private static string GetTranslation(string text, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (!dict.ContainsKey(text))
			{
				return null;
			}
			TranslationQuery translationQuery = dict[text];
			int num = Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo);
			if (num < 0)
			{
				return "";
			}
			if (translationQuery.Results == null)
			{
				return "";
			}
			return translationQuery.Results[num];
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0005AF90 File Offset: 0x00059190
		private static TranslationQuery FindQueryFromOrigText(string origText, Dictionary<string, TranslationQuery> dict)
		{
			foreach (KeyValuePair<string, TranslationQuery> keyValuePair in dict)
			{
				if (keyValuePair.Value.OrigText == origText)
				{
					return keyValuePair.Value;
				}
			}
			return default(TranslationQuery);
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0005B000 File Offset: 0x00059200
		public static bool HasParameters(string text)
		{
			int num = text.IndexOf("{[");
			return num >= 0 && text.IndexOf("]}", num) > 0;
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0005B030 File Offset: 0x00059230
		public static string GetPluralParameter(string text, bool forceTag)
		{
			int num = text.IndexOf("{[#");
			if (num < 0)
			{
				if (forceTag)
				{
					return null;
				}
				num = text.IndexOf("{[");
			}
			if (num < 0)
			{
				return null;
			}
			int num2 = text.IndexOf("]}", num + 2);
			if (num2 < 0)
			{
				return null;
			}
			return text.Substring(num, num2 - num + 2);
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0005B088 File Offset: 0x00059288
		public static string GetPluralText(string text, string pluralType)
		{
			pluralType = "[i2p_" + pluralType + "]";
			int num = text.IndexOf(pluralType);
			if (num >= 0)
			{
				num += pluralType.Length;
				int num2 = text.IndexOf("[i2p_", num);
				if (num2 < 0)
				{
					num2 = text.Length;
				}
				return text.Substring(num, num2 - num);
			}
			num = text.IndexOf("[i2p_");
			if (num < 0)
			{
				return text;
			}
			if (num > 0)
			{
				return text.Substring(0, num);
			}
			num = text.IndexOf("]");
			if (num < 0)
			{
				return text;
			}
			num++;
			int num3 = text.IndexOf("[i2p_", num);
			if (num3 < 0)
			{
				num3 = text.Length;
			}
			return text.Substring(num, num3 - num);
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0005B138 File Offset: 0x00059338
		private static int FindClosingTag(string tag, MatchCollection matches, int startIndex)
		{
			int i = startIndex;
			int count = matches.Count;
			while (i < count)
			{
				string captureMatch = I2Utils.GetCaptureMatch(matches[i]);
				if (captureMatch[0] == '/' && tag.StartsWith(captureMatch.Substring(1)))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0005B184 File Offset: 0x00059384
		private static string GetGoogleNoTranslateTag(int tagNumber)
		{
			if (tagNumber < 70)
			{
				return "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++".Substring(0, tagNumber + 1);
			}
			string text = "";
			for (int i = -1; i < tagNumber; i++)
			{
				text += "+";
			}
			return text;
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0005B1C4 File Offset: 0x000593C4
		private static void ParseNonTranslatableElements(ref TranslationQuery query)
		{
			MatchCollection matchCollection = Regex.Matches(query.Text, "\\{\\[(.*?)]}|\\[(.*?)]|\\<(.*?)>");
			if (matchCollection == null || matchCollection.Count == 0)
			{
				return;
			}
			string text = query.Text;
			List<string> list = new List<string>();
			int i = 0;
			int count = matchCollection.Count;
			while (i < count)
			{
				string captureMatch = I2Utils.GetCaptureMatch(matchCollection[i]);
				int num = GoogleTranslation.FindClosingTag(captureMatch, matchCollection, i);
				if (num < 0)
				{
					string text2 = matchCollection[i].ToString();
					if (text2.StartsWith("{[") && text2.EndsWith("]}"))
					{
						text = text.Replace(text2, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
						list.Add(text2);
					}
				}
				else if (captureMatch == "i2nt")
				{
					string text3 = query.Text.Substring(matchCollection[i].Index, matchCollection[num].Index - matchCollection[i].Index + matchCollection[num].Length);
					text = text.Replace(text3, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
					list.Add(text3);
				}
				else
				{
					string text4 = matchCollection[i].ToString();
					text = text.Replace(text4, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
					list.Add(text4);
					string text5 = matchCollection[num].ToString();
					text = text.Replace(text5, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
					list.Add(text5);
				}
				i++;
			}
			query.Text = text;
			query.Tags = list.ToArray();
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x0005B384 File Offset: 0x00059584
		public static string GetQueryResult(string text, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (!dict.ContainsKey(text))
			{
				return null;
			}
			TranslationQuery translationQuery = dict[text];
			if (translationQuery.Results == null || translationQuery.Results.Length < 0)
			{
				return null;
			}
			if (string.IsNullOrEmpty(LanguageCodeTo))
			{
				return translationQuery.Results[0];
			}
			int num = Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo);
			if (num < 0)
			{
				return null;
			}
			return translationQuery.Results[num];
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0005B3E8 File Offset: 0x000595E8
		public static string RebuildTranslation(string text, Dictionary<string, TranslationQuery> dict, string LanguageCodeTo)
		{
			if (!text.Contains("[i2s_"))
			{
				return GoogleTranslation.RebuildTranslation_Plural(text, dict, LanguageCodeTo);
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> keyValuePair in specializations)
			{
				dictionary[keyValuePair.Key] = GoogleTranslation.RebuildTranslation_Plural(keyValuePair.Value, dict, LanguageCodeTo);
			}
			return SpecializationManager.SetSpecializedText(dictionary);
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0005B474 File Offset: 0x00059674
		private static string RebuildTranslation_Plural(string text, Dictionary<string, TranslationQuery> dict, string LanguageCodeTo)
		{
			bool flag = text.Contains("{[#");
			bool flag2 = text.Contains("[i2p_");
			if (!GoogleTranslation.HasParameters(text) || (!flag && !flag2))
			{
				return GoogleTranslation.GetTranslation(text, LanguageCodeTo, dict);
			}
			StringBuilder stringBuilder = new StringBuilder();
			string text2 = null;
			bool flag3 = flag;
			for (ePluralType ePluralType = ePluralType.Plural; ePluralType >= ePluralType.Zero; ePluralType--)
			{
				string text3 = ePluralType.ToString();
				if (GoogleLanguages.LanguageHasPluralType(LanguageCodeTo, text3))
				{
					string text4 = GoogleTranslation.GetPluralText(text, text3);
					int pluralTestNumber = GoogleLanguages.GetPluralTestNumber(LanguageCodeTo, ePluralType);
					string pluralParameter = GoogleTranslation.GetPluralParameter(text4, flag3);
					if (!string.IsNullOrEmpty(pluralParameter))
					{
						text4 = text4.Replace(pluralParameter, pluralTestNumber.ToString());
					}
					string text5 = GoogleTranslation.GetTranslation(text4, LanguageCodeTo, dict);
					if (!string.IsNullOrEmpty(pluralParameter))
					{
						text5 = text5.Replace(pluralTestNumber.ToString(), pluralParameter);
					}
					if (ePluralType == ePluralType.Plural)
					{
						text2 = text5;
					}
					else
					{
						if (text5 == text2)
						{
							goto IL_00E9;
						}
						stringBuilder.AppendFormat("[i2p_{0}]", text3);
					}
					stringBuilder.Append(text5);
				}
				IL_00E9:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0005B580 File Offset: 0x00059780
		public static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			char[] array = s.ToLower().ToCharArray();
			array[0] = char.ToUpper(array[0]);
			return new string(array);
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0005B5B8 File Offset: 0x000597B8
		public static string TitleCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
		}

		// Token: 0x04000DD9 RID: 3545
		private static List<UnityWebRequest> mCurrentTranslations = new List<UnityWebRequest>();

		// Token: 0x04000DDA RID: 3546
		private static List<TranslationJob> mTranslationJobs = new List<TranslationJob>();

		// Token: 0x02000480 RID: 1152
		// (Invoke) Token: 0x0600197F RID: 6527
		public delegate void fnOnTranslated(string Translation, string Error);

		// Token: 0x02000481 RID: 1153
		// (Invoke) Token: 0x06001983 RID: 6531
		public delegate void fnOnTranslationReady(Dictionary<string, TranslationQuery> dict, string error);
	}
}
