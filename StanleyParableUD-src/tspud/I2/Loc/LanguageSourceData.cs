using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x020002A3 RID: 675
	[ExecuteInEditMode]
	[Serializable]
	public class LanguageSourceData
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x0005C2B7 File Offset: 0x0005A4B7
		public Object ownerObject
		{
			get
			{
				return this.owner as Object;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060010E4 RID: 4324 RVA: 0x0005C2C4 File Offset: 0x0005A4C4
		// (remove) Token: 0x060010E5 RID: 4325 RVA: 0x0005C2FC File Offset: 0x0005A4FC
		public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

		// Token: 0x060010E6 RID: 4326 RVA: 0x0005C331 File Offset: 0x0005A531
		public void Awake()
		{
			LocalizationManager.AddSource(this);
			this.UpdateDictionary(false);
			this.UpdateAssetDictionary();
			LocalizationManager.LocalizeAll(true);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0005C34C File Offset: 0x0005A54C
		public void OnDestroy()
		{
			LocalizationManager.RemoveSource(this);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0005C354 File Offset: 0x0005A554
		public bool IsEqualTo(LanguageSourceData Source)
		{
			if (Source.mLanguages.Count != this.mLanguages.Count)
			{
				return false;
			}
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (Source.GetLanguageIndex(this.mLanguages[i].Name, true, true) < 0)
				{
					return false;
				}
				i++;
			}
			if (Source.mTerms.Count != this.mTerms.Count)
			{
				return false;
			}
			for (int j = 0; j < this.mTerms.Count; j++)
			{
				if (Source.GetTermData(this.mTerms[j].Term, false) == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0005C400 File Offset: 0x0005A600
		internal bool ManagerHasASimilarSource()
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LanguageSourceData languageSourceData = LocalizationManager.Sources[i];
				if (languageSourceData != null && languageSourceData.IsEqualTo(this) && languageSourceData != this)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0005C443 File Offset: 0x0005A643
		public void ClearAllData()
		{
			this.mTerms.Clear();
			this.mLanguages.Clear();
			this.mDictionary.Clear();
			this.mAssetDictionary.Clear();
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0005C471 File Offset: 0x0005A671
		public bool IsGlobalSource()
		{
			return this.mIsGlobalSource;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00005444 File Offset: 0x00003644
		public void Editor_SetDirty()
		{
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0005C47C File Offset: 0x0005A67C
		public void UpdateAssetDictionary()
		{
			this.Assets.RemoveAll((Object x) => x == null);
			this.mAssetDictionary = (from o in this.Assets.Distinct<Object>()
				group o by o.name).ToDictionary((IGrouping<string, Object> g) => g.Key, (IGrouping<string, Object> g) => g.First<Object>());
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0005C52C File Offset: 0x0005A72C
		public Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.Assets.Count)
				{
					this.UpdateAssetDictionary();
				}
				Object @object;
				if (this.mAssetDictionary.TryGetValue(Name, out @object))
				{
					return @object;
				}
			}
			return null;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0005C57A File Offset: 0x0005A77A
		public bool HasAsset(Object Obj)
		{
			return this.Assets.Contains(Obj);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0005C588 File Offset: 0x0005A788
		public void AddAsset(Object Obj)
		{
			if (this.Assets.Contains(Obj))
			{
				return;
			}
			this.Assets.Add(Obj);
			this.UpdateAssetDictionary();
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0005C5AC File Offset: 0x0005A7AC
		public string Export_I2CSV(string Category, char Separator = ',', bool specializationsAsRows = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Key[*]Type[*]Desc");
			foreach (LanguageData languageData in this.mLanguages)
			{
				stringBuilder.Append("[*]");
				if (!languageData.IsEnabled())
				{
					stringBuilder.Append('$');
				}
				stringBuilder.Append(GoogleLanguages.GetCodedLanguage(languageData.Name, languageData.Code));
			}
			stringBuilder.Append("[ln]");
			this.mTerms.Sort((TermData a, TermData b) => string.CompareOrdinal(a.Term, b.Term));
			int count = this.mLanguages.Count;
			bool flag = true;
			foreach (TermData termData in this.mTerms)
			{
				string text;
				if (string.IsNullOrEmpty(Category) || (Category == LanguageSourceData.EmptyCategory && termData.Term.IndexOfAny(LanguageSourceData.CategorySeparators) < 0))
				{
					text = termData.Term;
				}
				else
				{
					if (!termData.Term.StartsWith(Category + "/") || !(Category != termData.Term))
					{
						continue;
					}
					text = termData.Term.Substring(Category.Length + 1);
				}
				if (!flag)
				{
					stringBuilder.Append("[ln]");
				}
				flag = false;
				if (!specializationsAsRows)
				{
					LanguageSourceData.AppendI2Term(stringBuilder, count, text, termData, Separator, null);
				}
				else
				{
					List<string> allSpecializations = termData.GetAllSpecializations();
					for (int i = 0; i < allSpecializations.Count; i++)
					{
						if (i != 0)
						{
							stringBuilder.Append("[ln]");
						}
						string text2 = allSpecializations[i];
						LanguageSourceData.AppendI2Term(stringBuilder, count, text, termData, Separator, text2);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0005C7B4 File Offset: 0x0005A9B4
		private static void AppendI2Term(StringBuilder Builder, int nLanguages, string Term, TermData termData, char Separator, string forceSpecialization)
		{
			LanguageSourceData.AppendI2Text(Builder, Term);
			if (!string.IsNullOrEmpty(forceSpecialization) && forceSpecialization != "Any")
			{
				Builder.Append("[");
				Builder.Append(forceSpecialization);
				Builder.Append("]");
			}
			Builder.Append("[*]");
			Builder.Append(termData.TermType.ToString());
			Builder.Append("[*]");
			Builder.Append(termData.Description);
			for (int i = 0; i < Mathf.Min(nLanguages, termData.Languages.Length); i++)
			{
				Builder.Append("[*]");
				string text = termData.Languages[i];
				if (!string.IsNullOrEmpty(forceSpecialization))
				{
					text = termData.GetTranslation(i, forceSpecialization, false);
				}
				LanguageSourceData.AppendI2Text(Builder, text);
			}
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0005C886 File Offset: 0x0005AA86
		private static void AppendI2Text(StringBuilder Builder, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (text.StartsWith("'") || text.StartsWith("="))
			{
				Builder.Append('\'');
			}
			Builder.Append(text);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0005C8BC File Offset: 0x0005AABC
		private string Export_Language_to_Cache(int langIndex, bool fillTermWithFallback)
		{
			if (!this.mLanguages[langIndex].IsLoaded())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.mTerms.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("[i2t]");
				}
				TermData termData = this.mTerms[i];
				stringBuilder.Append(termData.Term);
				stringBuilder.Append("=");
				string text = termData.Languages[langIndex];
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Fallback && string.IsNullOrEmpty(text) && this.TryGetFallbackTranslation(termData, out text, langIndex, null, true))
				{
					stringBuilder.Append("[i2fb]");
					if (fillTermWithFallback)
					{
						termData.Languages[langIndex] = text;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0005C98C File Offset: 0x0005AB8C
		public string Export_CSV(string Category, char Separator = ',', bool specializationsAsRows = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.mLanguages.Count;
			stringBuilder.AppendFormat("Key{0}Type{0}Desc", Separator);
			foreach (LanguageData languageData in this.mLanguages)
			{
				stringBuilder.Append(Separator);
				if (!languageData.IsEnabled())
				{
					stringBuilder.Append('$');
				}
				LanguageSourceData.AppendString(stringBuilder, GoogleLanguages.GetCodedLanguage(languageData.Name, languageData.Code), Separator);
			}
			stringBuilder.Append("\n");
			this.mTerms.Sort((TermData a, TermData b) => string.CompareOrdinal(a.Term, b.Term));
			foreach (TermData termData in this.mTerms)
			{
				string text;
				if (string.IsNullOrEmpty(Category) || (Category == LanguageSourceData.EmptyCategory && termData.Term.IndexOfAny(LanguageSourceData.CategorySeparators) < 0))
				{
					text = termData.Term;
				}
				else
				{
					if (!termData.Term.StartsWith(Category + "/") || !(Category != termData.Term))
					{
						continue;
					}
					text = termData.Term.Substring(Category.Length + 1);
				}
				if (specializationsAsRows)
				{
					using (List<string>.Enumerator enumerator3 = termData.GetAllSpecializations().GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							string text2 = enumerator3.Current;
							LanguageSourceData.AppendTerm(stringBuilder, count, text, termData, text2, Separator);
						}
						continue;
					}
				}
				LanguageSourceData.AppendTerm(stringBuilder, count, text, termData, null, Separator);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0005CB7C File Offset: 0x0005AD7C
		private static void AppendTerm(StringBuilder Builder, int nLanguages, string Term, TermData termData, string specialization, char Separator)
		{
			LanguageSourceData.AppendString(Builder, Term, Separator);
			if (!string.IsNullOrEmpty(specialization) && specialization != "Any")
			{
				Builder.AppendFormat("[{0}]", specialization);
			}
			Builder.Append(Separator);
			Builder.Append(termData.TermType.ToString());
			Builder.Append(Separator);
			LanguageSourceData.AppendString(Builder, termData.Description, Separator);
			for (int i = 0; i < Mathf.Min(nLanguages, termData.Languages.Length); i++)
			{
				Builder.Append(Separator);
				string text = termData.Languages[i];
				if (!string.IsNullOrEmpty(specialization))
				{
					text = termData.GetTranslation(i, specialization, false);
				}
				LanguageSourceData.AppendTranslation(Builder, text, Separator, null);
			}
			Builder.Append("\n");
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0005CC44 File Offset: 0x0005AE44
		private static void AppendString(StringBuilder Builder, string Text, char Separator)
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny((Separator.ToString() + "\n\"").ToCharArray()) >= 0)
			{
				Text = Text.Replace("\"", "\"\"");
				Builder.AppendFormat("\"{0}\"", Text);
				return;
			}
			Builder.Append(Text);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0005CCB4 File Offset: 0x0005AEB4
		private static void AppendTranslation(StringBuilder Builder, string Text, char Separator, string tags)
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny((Separator.ToString() + "\n\"").ToCharArray()) >= 0)
			{
				Text = Text.Replace("\"", "\"\"");
				Builder.AppendFormat("\"{0}{1}\"", tags, Text);
				return;
			}
			Builder.Append(tags);
			Builder.Append(Text);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0005CD2C File Offset: 0x0005AF2C
		public UnityWebRequest Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string text = this.Export_Google_CreateData();
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("key", this.Google_SpreadsheetKey);
			wwwform.AddField("action", "SetLanguageSource");
			wwwform.AddField("data", text);
			wwwform.AddField("updateMode", UpdateMode.ToString());
			UnityWebRequest unityWebRequest = UnityWebRequest.Post(LocalizationManager.GetWebServiceURL(this), wwwform);
			I2Utils.SendWebRequest(unityWebRequest);
			return unityWebRequest;
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0005CDA0 File Offset: 0x0005AFA0
		private string Export_Google_CreateData()
		{
			List<string> categories = this.GetCategories(true, null);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string text in categories)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append("<I2Loc>");
				}
				bool flag2 = true;
				string text2 = this.Export_I2CSV(text, ',', flag2);
				stringBuilder.Append(text);
				stringBuilder.Append("<I2Loc>");
				stringBuilder.Append(text2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0005CE3C File Offset: 0x0005B03C
		public string Import_CSV(string Category, string CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace, char Separator = ',')
		{
			List<string[]> list = LocalizationReader.ReadCSV(CSVstring, Separator);
			return this.Import_CSV(Category, list, UpdateMode);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0005CE5C File Offset: 0x0005B05C
		public string Import_I2CSV(string Category, string I2CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			List<string[]> list = LocalizationReader.ReadI2CSV(I2CSVstring);
			return this.Import_CSV(Category, list, UpdateMode);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0005CE7C File Offset: 0x0005B07C
		public string Import_CSV(string Category, List<string[]> CSV, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string[] array = CSV[0];
			int num = 1;
			int num2 = -1;
			int num3 = -1;
			string[] array2 = new string[] { "Key" };
			string[] array3 = new string[] { "Type" };
			string[] array4 = new string[] { "Desc", "Description" };
			if (array.Length > 1 && this.ArrayContains(array[0], array2))
			{
				if (UpdateMode == eSpreadsheetUpdateMode.Replace)
				{
					this.ClearAllData();
				}
				if (array.Length > 2)
				{
					if (this.ArrayContains(array[1], array3))
					{
						num2 = 1;
						num = 2;
					}
					if (this.ArrayContains(array[1], array4))
					{
						num3 = 1;
						num = 2;
					}
				}
				if (array.Length > 3)
				{
					if (this.ArrayContains(array[2], array3))
					{
						num2 = 2;
						num = 3;
					}
					if (this.ArrayContains(array[2], array4))
					{
						num3 = 2;
						num = 3;
					}
				}
				int num4 = Mathf.Max(array.Length - num, 0);
				int[] array5 = new int[num4];
				for (int i = 0; i < num4; i++)
				{
					if (string.IsNullOrEmpty(array[i + num]))
					{
						array5[i] = -1;
					}
					else
					{
						string text = array[i + num];
						bool flag = true;
						if (text.StartsWith("$"))
						{
							flag = false;
							text = text.Substring(1);
						}
						string text2;
						string text3;
						GoogleLanguages.UnPackCodeFromLanguageName(text, out text2, out text3);
						int num5;
						if (!string.IsNullOrEmpty(text3))
						{
							num5 = this.GetLanguageIndexFromCode(text3, true, false);
						}
						else
						{
							num5 = this.GetLanguageIndex(text2, true, false);
						}
						if (num5 < 0)
						{
							LanguageData languageData = new LanguageData();
							languageData.Name = text2;
							languageData.Code = text3;
							languageData.Flags = (byte)(0 | (flag ? 0 : 1));
							this.mLanguages.Add(languageData);
							num5 = this.mLanguages.Count - 1;
						}
						array5[i] = num5;
					}
				}
				num4 = this.mLanguages.Count;
				int j = 0;
				int count = this.mTerms.Count;
				while (j < count)
				{
					TermData termData = this.mTerms[j];
					if (termData.Languages.Length < num4)
					{
						Array.Resize<string>(ref termData.Languages, num4);
						Array.Resize<byte>(ref termData.Flags, num4);
					}
					j++;
				}
				int k = 1;
				int count2 = CSV.Count;
				while (k < count2)
				{
					array = CSV[k];
					string text4 = (string.IsNullOrEmpty(Category) ? array[0] : (Category + "/" + array[0]));
					string text5 = null;
					if (text4.EndsWith("]"))
					{
						int num6 = text4.LastIndexOf('[');
						if (num6 > 0)
						{
							text5 = text4.Substring(num6 + 1, text4.Length - num6 - 2);
							if (text5 == "touch")
							{
								text5 = "Touch";
							}
							text4 = text4.Remove(num6);
						}
					}
					LanguageSourceData.ValidateFullTerm(ref text4);
					if (!string.IsNullOrEmpty(text4))
					{
						TermData termData2 = this.GetTermData(text4, false);
						if (termData2 == null)
						{
							termData2 = new TermData();
							termData2.Term = text4;
							termData2.Languages = new string[this.mLanguages.Count];
							termData2.Flags = new byte[this.mLanguages.Count];
							for (int l = 0; l < this.mLanguages.Count; l++)
							{
								termData2.Languages[l] = string.Empty;
							}
							this.mTerms.Add(termData2);
							this.mDictionary.Add(text4, termData2);
						}
						else if (UpdateMode == eSpreadsheetUpdateMode.AddNewTerms)
						{
							goto IL_03E1;
						}
						if (num2 > 0)
						{
							termData2.TermType = LanguageSourceData.GetTermType(array[num2]);
						}
						if (num3 > 0)
						{
							termData2.Description = array[num3];
						}
						int num7 = 0;
						while (num7 < array5.Length && num7 < array.Length - num)
						{
							if (!string.IsNullOrEmpty(array[num7 + num]))
							{
								int num8 = array5[num7];
								if (num8 >= 0)
								{
									string text6 = array[num7 + num];
									if (text6 == "-")
									{
										text6 = string.Empty;
									}
									else if (text6 == "")
									{
										text6 = null;
									}
									termData2.SetTranslation(num8, text6, text5);
								}
							}
							num7++;
						}
					}
					IL_03E1:
					k++;
				}
				if (Application.isPlaying)
				{
					this.SaveLanguages(this.HasUnloadedLanguages(), PersistentStorage.eFileType.Temporal);
				}
				return string.Empty;
			}
			return "Bad Spreadsheet Format.\nFirst columns should be 'Key', 'Type' and 'Desc'";
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0005D294 File Offset: 0x0005B494
		private bool ArrayContains(string MainText, params string[] texts)
		{
			int i = 0;
			int num = texts.Length;
			while (i < num)
			{
				if (MainText.IndexOf(texts[i], StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0005D2C4 File Offset: 0x0005B4C4
		public static eTermType GetTermType(string type)
		{
			int i = 0;
			int num = 10;
			while (i <= num)
			{
				eTermType eTermType = (eTermType)i;
				if (string.Equals(eTermType.ToString(), type, StringComparison.OrdinalIgnoreCase))
				{
					return (eTermType)i;
				}
				i++;
			}
			return eTermType.Text;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0005D2FC File Offset: 0x0005B4FC
		private void Import_Language_from_Cache(int langIndex, string langData, bool useFallback, bool onlyCurrentSpecialization)
		{
			int num;
			for (int i = 0; i < langData.Length; i = num + 5)
			{
				num = langData.IndexOf("[i2t]", i);
				if (num < 0)
				{
					num = langData.Length;
				}
				int num2 = langData.IndexOf("=", i);
				if (num2 >= num)
				{
					return;
				}
				string text = langData.Substring(i, num2 - i);
				i = num2 + 1;
				TermData termData = this.GetTermData(text, false);
				if (termData != null)
				{
					string text2 = null;
					if (i != num)
					{
						text2 = langData.Substring(i, num - i);
						if (text2.StartsWith("[i2fb]"))
						{
							text2 = (useFallback ? text2.Substring(6) : null);
						}
						if (onlyCurrentSpecialization && text2 != null)
						{
							text2 = SpecializationManager.GetSpecializedText(text2, null);
						}
					}
					termData.Languages[langIndex] = text2;
				}
			}
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0005D3B8 File Offset: 0x0005B5B8
		public static void FreeUnusedLanguages()
		{
			LanguageSourceData languageSourceData = LocalizationManager.Sources[0];
			int languageIndex = languageSourceData.GetLanguageIndex(LocalizationManager.CurrentLanguage, true, true);
			for (int i = 0; i < languageSourceData.mTerms.Count; i++)
			{
				TermData termData = languageSourceData.mTerms[i];
				for (int j = 0; j < termData.Languages.Length; j++)
				{
					if (j != languageIndex)
					{
						termData.Languages[j] = null;
					}
				}
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0005D428 File Offset: 0x0005B628
		public void Import_Google_FromCache()
		{
			if (this.GoogleUpdateFrequency == LanguageSourceData.eGoogleUpdateFrequency.Never)
			{
				return;
			}
			if (!I2Utils.IsPlaying())
			{
				return;
			}
			string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
			string text = PersistentStorage.LoadFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", false);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (text.StartsWith("[i2e]", StringComparison.Ordinal))
			{
				text = StringObfucator.Decode(text.Substring(5, text.Length - 5));
			}
			bool flag = false;
			string text2 = this.Google_LastUpdatedVersion;
			if (PersistentStorage.HasSetting("I2SourceVersion_" + sourcePlayerPrefName))
			{
				text2 = PersistentStorage.GetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, this.Google_LastUpdatedVersion);
				flag = this.IsNewerVersion(this.Google_LastUpdatedVersion, text2);
			}
			if (!flag)
			{
				PersistentStorage.DeleteFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", false);
				PersistentStorage.DeleteSetting("I2SourceVersion_" + sourcePlayerPrefName);
				return;
			}
			if (text2.Length > 19)
			{
				text2 = string.Empty;
			}
			this.Google_LastUpdatedVersion = text2;
			this.Import_Google_Result(text, eSpreadsheetUpdateMode.Replace, false);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0005D520 File Offset: 0x0005B720
		private bool IsNewerVersion(string currentVersion, string newVersion)
		{
			long num;
			long num2;
			return !string.IsNullOrEmpty(newVersion) && (string.IsNullOrEmpty(currentVersion) || (!long.TryParse(newVersion, out num) || !long.TryParse(currentVersion, out num2)) || num > num2);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0005D55C File Offset: 0x0005B75C
		public void Import_Google(bool ForceUpdate, bool justCheck)
		{
			if (!ForceUpdate && this.GoogleUpdateFrequency == LanguageSourceData.eGoogleUpdateFrequency.Never)
			{
				return;
			}
			if (!I2Utils.IsPlaying())
			{
				return;
			}
			LanguageSourceData.eGoogleUpdateFrequency googleUpdateFrequency = this.GoogleUpdateFrequency;
			string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
			if (!ForceUpdate && googleUpdateFrequency != LanguageSourceData.eGoogleUpdateFrequency.Always)
			{
				string setting_String = PersistentStorage.GetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, "");
				try
				{
					DateTime dateTime;
					if (DateTime.TryParse(setting_String, out dateTime))
					{
						double totalDays = (DateTime.Now - dateTime).TotalDays;
						switch (googleUpdateFrequency)
						{
						case LanguageSourceData.eGoogleUpdateFrequency.Daily:
							if (totalDays >= 1.0)
							{
								goto IL_00BF;
							}
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.Weekly:
							if (totalDays >= 8.0)
							{
								goto IL_00BF;
							}
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.Monthly:
							if (totalDays >= 31.0)
							{
								goto IL_00BF;
							}
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.OnlyOnce:
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.EveryOtherDay:
							if (totalDays >= 2.0)
							{
								goto IL_00BF;
							}
							break;
						default:
							goto IL_00BF;
						}
						return;
					}
					IL_00BF:;
				}
				catch (Exception)
				{
				}
			}
			PersistentStorage.SetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, DateTime.Now.ToString());
			CoroutineManager.Start(this.Import_Google_Coroutine(justCheck));
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0005D668 File Offset: 0x0005B868
		private string GetSourcePlayerPrefName()
		{
			if (this.owner == null)
			{
				return null;
			}
			string text = (this.owner as Object).name;
			if (!string.IsNullOrEmpty(this.Google_SpreadsheetKey))
			{
				text += this.Google_SpreadsheetKey;
			}
			if (Array.IndexOf<string>(LocalizationManager.GlobalSources, (this.owner as Object).name) >= 0)
			{
				return text;
			}
			return SceneManager.GetActiveScene().name + "_" + text;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0005D6E1 File Offset: 0x0005B8E1
		private IEnumerator Import_Google_Coroutine(bool JustCheck)
		{
			UnityWebRequest www = this.Import_Google_CreateWWWcall(false, JustCheck);
			if (www == null)
			{
				yield break;
			}
			while (!www.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(www.error))
			{
				byte[] data = www.downloadHandler.data;
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				bool flag = string.IsNullOrEmpty(@string) || @string == "\"\"";
				if (JustCheck)
				{
					if (!flag)
					{
						Debug.LogWarning("Spreadsheet is not up-to-date and Google Live Synchronization is enabled\nWhen playing in the device the Spreadsheet will be downloaded and translations may not behave as what you see in the editor.\nTo fix this, Import or Export replace to Google");
						this.GoogleLiveSyncIsUptoDate = false;
					}
					yield break;
				}
				if (!flag)
				{
					this.mDelayedGoogleData = @string;
					switch (this.GoogleUpdateSynchronization)
					{
					case LanguageSourceData.eGoogleUpdateSynchronization.OnSceneLoaded:
						SceneManager.sceneLoaded += this.ApplyDownloadedDataOnSceneLoaded;
						break;
					case LanguageSourceData.eGoogleUpdateSynchronization.AsSoonAsDownloaded:
						this.ApplyDownloadedDataFromGoogle();
						break;
					}
					yield break;
				}
			}
			if (this.Event_OnSourceUpdateFromGoogle != null)
			{
				this.Event_OnSourceUpdateFromGoogle(this, false, www.error);
			}
			Debug.Log("Language Source was up-to-date with Google Spreadsheet");
			yield break;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0005D6F7 File Offset: 0x0005B8F7
		private void ApplyDownloadedDataOnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			SceneManager.sceneLoaded -= this.ApplyDownloadedDataOnSceneLoaded;
			this.ApplyDownloadedDataFromGoogle();
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0005D710 File Offset: 0x0005B910
		public void ApplyDownloadedDataFromGoogle()
		{
			if (string.IsNullOrEmpty(this.mDelayedGoogleData))
			{
				return;
			}
			if (string.IsNullOrEmpty(this.Import_Google_Result(this.mDelayedGoogleData, eSpreadsheetUpdateMode.Replace, true)))
			{
				if (this.Event_OnSourceUpdateFromGoogle != null)
				{
					this.Event_OnSourceUpdateFromGoogle(this, true, "");
				}
				LocalizationManager.LocalizeAll(true);
				Debug.Log("Done Google Sync");
				return;
			}
			if (this.Event_OnSourceUpdateFromGoogle != null)
			{
				this.Event_OnSourceUpdateFromGoogle(this, false, "");
			}
			Debug.Log("Done Google Sync: source was up-to-date");
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0005D790 File Offset: 0x0005B990
		public UnityWebRequest Import_Google_CreateWWWcall(bool ForceUpdate, bool justCheck)
		{
			if (!this.HasGoogleSpreadsheet())
			{
				return null;
			}
			string text = PersistentStorage.GetSetting_String("I2SourceVersion_" + this.GetSourcePlayerPrefName(), this.Google_LastUpdatedVersion);
			if (text.Length > 19)
			{
				text = string.Empty;
			}
			if (this.IsNewerVersion(text, this.Google_LastUpdatedVersion))
			{
				this.Google_LastUpdatedVersion = text;
			}
			UnityWebRequest unityWebRequest = UnityWebRequest.Get(string.Format("{0}?key={1}&action=GetLanguageSource&version={2}", LocalizationManager.GetWebServiceURL(this), this.Google_SpreadsheetKey, ForceUpdate ? "0" : this.Google_LastUpdatedVersion));
			I2Utils.SendWebRequest(unityWebRequest);
			return unityWebRequest;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0005D81A File Offset: 0x0005BA1A
		public bool HasGoogleSpreadsheet()
		{
			return !string.IsNullOrEmpty(this.Google_WebServiceURL) && !string.IsNullOrEmpty(this.Google_SpreadsheetKey) && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(this));
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0005D848 File Offset: 0x0005BA48
		public string Import_Google_Result(string JsonString, eSpreadsheetUpdateMode UpdateMode, bool saveInPlayerPrefs = false)
		{
			string text;
			try
			{
				string empty = string.Empty;
				if (string.IsNullOrEmpty(JsonString) || JsonString == "\"\"")
				{
					text = empty;
				}
				else
				{
					int num = JsonString.IndexOf("version=", StringComparison.Ordinal);
					int num2 = JsonString.IndexOf("script_version=", StringComparison.Ordinal);
					if (num < 0 || num2 < 0)
					{
						text = "Invalid Response from Google, Most likely the WebService needs to be updated";
					}
					else
					{
						num += "version=".Length;
						num2 += "script_version=".Length;
						string text2 = JsonString.Substring(num, JsonString.IndexOf(",", num, StringComparison.Ordinal) - num);
						int num3 = int.Parse(JsonString.Substring(num2, JsonString.IndexOf(",", num2, StringComparison.Ordinal) - num2));
						if (text2.Length > 19)
						{
							text2 = string.Empty;
						}
						if (num3 != LocalizationManager.GetRequiredWebServiceVersion())
						{
							text = "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
						}
						else if (saveInPlayerPrefs && !this.IsNewerVersion(this.Google_LastUpdatedVersion, text2))
						{
							text = "LanguageSource is up-to-date";
						}
						else
						{
							if (saveInPlayerPrefs)
							{
								string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
								PersistentStorage.SaveFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", "[i2e]" + StringObfucator.Encode(JsonString), true);
								PersistentStorage.SetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, text2);
								PersistentStorage.ForceSaveSettings();
							}
							this.Google_LastUpdatedVersion = text2;
							if (UpdateMode == eSpreadsheetUpdateMode.Replace)
							{
								this.ClearAllData();
							}
							int i = JsonString.IndexOf("[i2category]", StringComparison.Ordinal);
							while (i > 0)
							{
								i += "[i2category]".Length;
								int num4 = JsonString.IndexOf("[/i2category]", i, StringComparison.Ordinal);
								string text3 = JsonString.Substring(i, num4 - i);
								num4 += "[/i2category]".Length;
								int num5 = JsonString.IndexOf("[/i2csv]", num4, StringComparison.Ordinal);
								string text4 = JsonString.Substring(num4, num5 - num4);
								i = JsonString.IndexOf("[i2category]", num5, StringComparison.Ordinal);
								this.Import_I2CSV(text3, text4, UpdateMode);
								if (UpdateMode == eSpreadsheetUpdateMode.Replace)
								{
									UpdateMode = eSpreadsheetUpdateMode.Merge;
								}
							}
							this.GoogleLiveSyncIsUptoDate = true;
							if (I2Utils.IsPlaying())
							{
								this.SaveLanguages(true, PersistentStorage.eFileType.Temporal);
							}
							if (!string.IsNullOrEmpty(empty))
							{
								this.Editor_SetDirty();
							}
							text = empty;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex);
				text = ex.ToString();
			}
			return text;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0005DA7C File Offset: 0x0005BC7C
		public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true, bool SkipDisabled = true)
		{
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if ((!SkipDisabled || this.mLanguages[i].IsEnabled()) && string.Compare(this.mLanguages[i].Name, language, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int num = -1;
				int num2 = 0;
				int j = 0;
				int count2 = this.mLanguages.Count;
				while (j < count2)
				{
					if (!SkipDisabled || this.mLanguages[j].IsEnabled())
					{
						int commonWordInLanguageNames = LanguageSourceData.GetCommonWordInLanguageNames(this.mLanguages[j].Name, language);
						if (commonWordInLanguageNames > num2)
						{
							num2 = commonWordInLanguageNames;
							num = j;
						}
					}
					j++;
				}
				if (num >= 0)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0005DB3C File Offset: 0x0005BD3C
		public LanguageData GetLanguageData(string language, bool AllowDiscartingRegion = true)
		{
			int languageIndex = this.GetLanguageIndex(language, AllowDiscartingRegion, false);
			if (languageIndex >= 0)
			{
				return this.mLanguages[languageIndex];
			}
			return null;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0005DB65 File Offset: 0x0005BD65
		public bool IsCurrentLanguage(int languageIndex)
		{
			return LocalizationManager.CurrentLanguage == this.mLanguages[languageIndex].Name;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0005DB84 File Offset: 0x0005BD84
		public int GetLanguageIndexFromCode(string Code, bool exactMatch = true, bool ignoreDisabled = false)
		{
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if ((!ignoreDisabled || this.mLanguages[i].IsEnabled()) && string.Compare(this.mLanguages[i].Code, Code, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			if (!exactMatch)
			{
				int j = 0;
				int count2 = this.mLanguages.Count;
				while (j < count2)
				{
					if ((!ignoreDisabled || this.mLanguages[j].IsEnabled()) && string.Compare(this.mLanguages[j].Code, 0, Code, 0, 2, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return j;
					}
					j++;
				}
			}
			return -1;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0005DC2C File Offset: 0x0005BE2C
		public static int GetCommonWordInLanguageNames(string Language1, string Language2)
		{
			if (string.IsNullOrEmpty(Language1) || string.IsNullOrEmpty(Language2))
			{
				return 0;
			}
			char[] array = "( )-/\\".ToCharArray();
			string[] array2 = Language1.ToLower().Split(array);
			string[] array3 = Language2.ToLower().Split(array);
			int num = 0;
			foreach (string text in array2)
			{
				if (!string.IsNullOrEmpty(text) && array3.Contains(text))
				{
					num++;
				}
			}
			foreach (string text2 in array3)
			{
				if (!string.IsNullOrEmpty(text2) && array2.Contains(text2))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0005DCDB File Offset: 0x0005BEDB
		public static bool AreTheSameLanguage(string Language1, string Language2)
		{
			Language1 = LanguageSourceData.GetLanguageWithoutRegion(Language1);
			Language2 = LanguageSourceData.GetLanguageWithoutRegion(Language2);
			return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0005DCF8 File Offset: 0x0005BEF8
		public static string GetLanguageWithoutRegion(string Language)
		{
			int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
			if (num < 0)
			{
				return Language;
			}
			return Language.Substring(0, num).Trim();
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0005DD29 File Offset: 0x0005BF29
		public void AddLanguage(string LanguageName)
		{
			this.AddLanguage(LanguageName, GoogleLanguages.GetLanguageCode(LanguageName, false));
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0005DD3C File Offset: 0x0005BF3C
		public void AddLanguage(string LanguageName, string LanguageCode)
		{
			if (this.GetLanguageIndex(LanguageName, false, true) >= 0)
			{
				return;
			}
			LanguageData languageData = new LanguageData();
			languageData.Name = LanguageName;
			languageData.Code = LanguageCode;
			this.mLanguages.Add(languageData);
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				Array.Resize<string>(ref this.mTerms[i].Languages, count);
				Array.Resize<byte>(ref this.mTerms[i].Flags, count);
				i++;
			}
			this.Editor_SetDirty();
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0005DDD0 File Offset: 0x0005BFD0
		public void RemoveLanguage(string LanguageName)
		{
			int languageIndex = this.GetLanguageIndex(LanguageName, false, false);
			if (languageIndex < 0)
			{
				return;
			}
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				for (int j = languageIndex + 1; j < count; j++)
				{
					this.mTerms[i].Languages[j - 1] = this.mTerms[i].Languages[j];
					this.mTerms[i].Flags[j - 1] = this.mTerms[i].Flags[j];
				}
				Array.Resize<string>(ref this.mTerms[i].Languages, count - 1);
				Array.Resize<byte>(ref this.mTerms[i].Flags, count - 1);
				i++;
			}
			this.mLanguages.RemoveAt(languageIndex);
			this.Editor_SetDirty();
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0005DEC0 File Offset: 0x0005C0C0
		public List<string> GetLanguages(bool skipDisabled = true)
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (!skipDisabled || this.mLanguages[i].IsEnabled())
				{
					list.Add(this.mLanguages[i].Name);
				}
				i++;
			}
			return list;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0005DF1C File Offset: 0x0005C11C
		public List<string> GetLanguagesCode(bool allowRegions = true, bool skipDisabled = true)
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (!skipDisabled || this.mLanguages[i].IsEnabled())
				{
					string text = this.mLanguages[i].Code;
					if (!allowRegions && text != null && text.Length > 2)
					{
						text = text.Substring(0, 2);
					}
					if (!string.IsNullOrEmpty(text) && !list.Contains(text))
					{
						list.Add(text);
					}
				}
				i++;
			}
			return list;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0005DFA0 File Offset: 0x0005C1A0
		public bool IsLanguageEnabled(string Language)
		{
			int languageIndex = this.GetLanguageIndex(Language, false, true);
			return languageIndex >= 0 && this.mLanguages[languageIndex].IsEnabled();
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0005DFD0 File Offset: 0x0005C1D0
		public void EnableLanguage(string Language, bool bEnabled)
		{
			int languageIndex = this.GetLanguageIndex(Language, false, false);
			if (languageIndex >= 0)
			{
				this.mLanguages[languageIndex].SetEnabled(bEnabled);
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0005DFFD File Offset: 0x0005C1FD
		public bool AllowUnloadingLanguages()
		{
			return this._AllowUnloadingLanguages > LanguageSourceData.eAllowUnloadLanguages.Never;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0005E008 File Offset: 0x0005C208
		private string GetSavedLanguageFileName(int languageIndex)
		{
			if (languageIndex < 0)
			{
				return null;
			}
			return string.Concat(new string[]
			{
				"LangSource_",
				this.GetSourcePlayerPrefName(),
				"_",
				this.mLanguages[languageIndex].Name,
				".loc"
			});
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0005E05C File Offset: 0x0005C25C
		public void LoadLanguage(int languageIndex, bool UnloadOtherLanguages, bool useFallback, bool onlyCurrentSpecialization, bool forceLoad)
		{
			if (!this.AllowUnloadingLanguages())
			{
				return;
			}
			if (!PersistentStorage.CanAccessFiles())
			{
				return;
			}
			if (languageIndex >= 0 && (forceLoad || !this.mLanguages[languageIndex].IsLoaded()))
			{
				string savedLanguageFileName = this.GetSavedLanguageFileName(languageIndex);
				string text = PersistentStorage.LoadFile(PersistentStorage.eFileType.Temporal, savedLanguageFileName, false);
				if (!string.IsNullOrEmpty(text))
				{
					this.Import_Language_from_Cache(languageIndex, text, useFallback, onlyCurrentSpecialization);
					this.mLanguages[languageIndex].SetLoaded(true);
				}
			}
			if (UnloadOtherLanguages && I2Utils.IsPlaying())
			{
				for (int i = 0; i < this.mLanguages.Count; i++)
				{
					if (i != languageIndex)
					{
						this.UnloadLanguage(i);
					}
				}
			}
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0005E0F8 File Offset: 0x0005C2F8
		public void LoadAllLanguages(bool forceLoad = false)
		{
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				this.LoadLanguage(i, false, false, false, forceLoad);
			}
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0005E128 File Offset: 0x0005C328
		public void UnloadLanguage(int languageIndex)
		{
			if (!this.AllowUnloadingLanguages())
			{
				return;
			}
			if (!PersistentStorage.CanAccessFiles())
			{
				return;
			}
			if (!I2Utils.IsPlaying() || !this.mLanguages[languageIndex].IsLoaded() || !this.mLanguages[languageIndex].CanBeUnloaded() || this.IsCurrentLanguage(languageIndex) || !PersistentStorage.HasFile(PersistentStorage.eFileType.Temporal, this.GetSavedLanguageFileName(languageIndex), true))
			{
				return;
			}
			foreach (TermData termData in this.mTerms)
			{
				termData.Languages[languageIndex] = null;
			}
			this.mLanguages[languageIndex].SetLoaded(false);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0005E1E8 File Offset: 0x0005C3E8
		public void SaveLanguages(bool unloadAll, PersistentStorage.eFileType fileLocation = PersistentStorage.eFileType.Temporal)
		{
			if (!this.AllowUnloadingLanguages())
			{
				return;
			}
			if (!PersistentStorage.CanAccessFiles())
			{
				return;
			}
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				string text = this.Export_Language_to_Cache(i, this.IsCurrentLanguage(i));
				if (!string.IsNullOrEmpty(text))
				{
					PersistentStorage.SaveFile(PersistentStorage.eFileType.Temporal, this.GetSavedLanguageFileName(i), text, true);
				}
			}
			if (unloadAll)
			{
				for (int j = 0; j < this.mLanguages.Count; j++)
				{
					if (unloadAll && !this.IsCurrentLanguage(j))
					{
						this.UnloadLanguage(j);
					}
				}
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0005E270 File Offset: 0x0005C470
		public bool HasUnloadedLanguages()
		{
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				if (!this.mLanguages[i].IsLoaded())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0005E2AC File Offset: 0x0005C4AC
		public List<string> GetCategories(bool OnlyMainCategory = false, List<string> Categories = null)
		{
			if (Categories == null)
			{
				Categories = new List<string>();
			}
			foreach (TermData termData in this.mTerms)
			{
				string categoryFromFullTerm = LanguageSourceData.GetCategoryFromFullTerm(termData.Term, OnlyMainCategory);
				if (!Categories.Contains(categoryFromFullTerm))
				{
					Categories.Add(categoryFromFullTerm);
				}
			}
			Categories.Sort();
			return Categories;
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0005E324 File Offset: 0x0005C524
		public static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators));
			if (num >= 0)
			{
				return FullTerm.Substring(num + 1);
			}
			return FullTerm;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0005E35C File Offset: 0x0005C55C
		public static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators));
			if (num >= 0)
			{
				return FullTerm.Substring(0, num);
			}
			return LanguageSourceData.EmptyCategory;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0005E398 File Offset: 0x0005C598
		public static void DeserializeFullTerm(string FullTerm, out string Key, out string Category, bool OnlyMainCategory = false)
		{
			int num = (OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators));
			if (num < 0)
			{
				Category = LanguageSourceData.EmptyCategory;
				Key = FullTerm;
				return;
			}
			Category = FullTerm.Substring(0, num);
			Key = FullTerm.Substring(num + 1);
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0005E3E8 File Offset: 0x0005C5E8
		public void UpdateDictionary(bool force = false)
		{
			if (!force && this.mDictionary != null && this.mDictionary.Count == this.mTerms.Count)
			{
				return;
			}
			StringComparer stringComparer = (this.CaseInsensitiveTerms ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
			if (this.mDictionary.Comparer != stringComparer)
			{
				this.mDictionary = new Dictionary<string, TermData>(stringComparer);
			}
			else
			{
				this.mDictionary.Clear();
			}
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				TermData termData = this.mTerms[i];
				LanguageSourceData.ValidateFullTerm(ref termData.Term);
				this.mDictionary[termData.Term] = this.mTerms[i];
				this.mTerms[i].Validate();
				i++;
			}
			if (I2Utils.IsPlaying())
			{
				this.SaveLanguages(true, PersistentStorage.eFileType.Temporal);
			}
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0005E4C4 File Offset: 0x0005C6C4
		public string GetTranslation(string term, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
		{
			string text;
			if (this.TryGetTranslation(term, out text, overrideLanguage, overrideSpecialization, skipDisabled, allowCategoryMistmatch))
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0005E4EC File Offset: 0x0005C6EC
		public bool TryGetTranslation(string term, out string Translation, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
		{
			int languageIndex = this.GetLanguageIndex((overrideLanguage == null) ? LocalizationManager.CurrentLanguage : overrideLanguage, true, false);
			if (languageIndex >= 0 && (!skipDisabled || this.mLanguages[languageIndex].IsEnabled()))
			{
				TermData termData = this.GetTermData(term, allowCategoryMistmatch);
				if (termData != null)
				{
					Translation = termData.GetTranslation(languageIndex, overrideSpecialization, true);
					if (Translation == "---")
					{
						Translation = string.Empty;
						return true;
					}
					if (!string.IsNullOrEmpty(Translation))
					{
						return true;
					}
					Translation = null;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowWarning)
				{
					Translation = string.Format("<!-Missing Translation [{0}]-!>", term);
					return true;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Fallback && termData != null)
				{
					return this.TryGetFallbackTranslation(termData, out Translation, languageIndex, overrideSpecialization, skipDisabled);
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Empty)
				{
					Translation = string.Empty;
					return true;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowTerm)
				{
					Translation = term;
					return true;
				}
			}
			Translation = null;
			return false;
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0005E5C4 File Offset: 0x0005C7C4
		private bool TryGetFallbackTranslation(TermData termData, out string Translation, int langIndex, string overrideSpecialization = null, bool skipDisabled = false)
		{
			string text = this.mLanguages[langIndex].Code;
			if (!string.IsNullOrEmpty(text))
			{
				if (text.Contains('-'))
				{
					text = text.Substring(0, text.IndexOf('-'));
				}
				for (int i = 0; i < this.mLanguages.Count; i++)
				{
					if (i != langIndex && this.mLanguages[i].Code.StartsWith(text) && (!skipDisabled || this.mLanguages[i].IsEnabled()))
					{
						Translation = termData.GetTranslation(i, overrideSpecialization, true);
						if (!string.IsNullOrEmpty(Translation))
						{
							return true;
						}
					}
				}
			}
			for (int j = 0; j < this.mLanguages.Count; j++)
			{
				if (j != langIndex && (!skipDisabled || this.mLanguages[j].IsEnabled()) && (text == null || !this.mLanguages[j].Code.StartsWith(text)))
				{
					Translation = termData.GetTranslation(j, overrideSpecialization, true);
					if (!string.IsNullOrEmpty(Translation))
					{
						return true;
					}
				}
			}
			Translation = null;
			return false;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0005E6CE File Offset: 0x0005C8CE
		public TermData AddTerm(string term)
		{
			return this.AddTerm(term, eTermType.Text, true);
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0005E6DC File Offset: 0x0005C8DC
		public TermData GetTermData(string term, bool allowCategoryMistmatch = false)
		{
			if (string.IsNullOrEmpty(term))
			{
				return null;
			}
			if (this.mDictionary.Count == 0)
			{
				this.UpdateDictionary(false);
			}
			TermData termData;
			if (this.mDictionary.TryGetValue(term, out termData))
			{
				return termData;
			}
			TermData termData2 = null;
			if (allowCategoryMistmatch)
			{
				string keyFromFullTerm = LanguageSourceData.GetKeyFromFullTerm(term, false);
				foreach (KeyValuePair<string, TermData> keyValuePair in this.mDictionary)
				{
					if (keyValuePair.Value.IsTerm(keyFromFullTerm, true))
					{
						if (termData2 != null)
						{
							return null;
						}
						termData2 = keyValuePair.Value;
					}
				}
				return termData2;
			}
			return termData2;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0005E78C File Offset: 0x0005C98C
		public bool ContainsTerm(string term)
		{
			return this.GetTermData(term, false) != null;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0005E79C File Offset: 0x0005C99C
		public List<string> GetTermsList(string Category = null)
		{
			if (this.mDictionary.Count != this.mTerms.Count)
			{
				this.UpdateDictionary(false);
			}
			if (string.IsNullOrEmpty(Category))
			{
				return new List<string>(this.mDictionary.Keys);
			}
			List<string> list = new List<string>();
			for (int i = 0; i < this.mTerms.Count; i++)
			{
				TermData termData = this.mTerms[i];
				if (LanguageSourceData.GetCategoryFromFullTerm(termData.Term, false) == Category)
				{
					list.Add(termData.Term);
				}
			}
			return list;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0005E82C File Offset: 0x0005CA2C
		public TermData AddTerm(string NewTerm, eTermType termType, bool SaveSource = true)
		{
			LanguageSourceData.ValidateFullTerm(ref NewTerm);
			NewTerm = NewTerm.Trim();
			if (this.mLanguages.Count == 0)
			{
				this.AddLanguage("English", "en");
			}
			TermData termData = this.GetTermData(NewTerm, false);
			if (termData == null)
			{
				termData = new TermData();
				termData.Term = NewTerm;
				termData.TermType = termType;
				termData.Languages = new string[this.mLanguages.Count];
				termData.Flags = new byte[this.mLanguages.Count];
				this.mTerms.Add(termData);
				this.mDictionary.Add(NewTerm, termData);
			}
			return termData;
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0005E8CC File Offset: 0x0005CACC
		public void RemoveTerm(string term)
		{
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				if (this.mTerms[i].Term == term)
				{
					this.mTerms.RemoveAt(i);
					this.mDictionary.Remove(term);
					return;
				}
				i++;
			}
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0005E924 File Offset: 0x0005CB24
		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			Term = Term.Trim();
			if (Term.StartsWith(LanguageSourceData.EmptyCategory, StringComparison.Ordinal) && Term.Length > LanguageSourceData.EmptyCategory.Length && Term[LanguageSourceData.EmptyCategory.Length] == '/')
			{
				Term = Term.Substring(LanguageSourceData.EmptyCategory.Length + 1);
			}
			Term = I2Utils.GetValidTermName(Term, true);
		}

		// Token: 0x04000E14 RID: 3604
		[NonSerialized]
		public ILanguageSource owner;

		// Token: 0x04000E15 RID: 3605
		public bool UserAgreesToHaveItOnTheScene;

		// Token: 0x04000E16 RID: 3606
		public bool UserAgreesToHaveItInsideThePluginsFolder;

		// Token: 0x04000E17 RID: 3607
		public bool GoogleLiveSyncIsUptoDate = true;

		// Token: 0x04000E18 RID: 3608
		[NonSerialized]
		public bool mIsGlobalSource;

		// Token: 0x04000E19 RID: 3609
		public List<TermData> mTerms = new List<TermData>();

		// Token: 0x04000E1A RID: 3610
		public bool CaseInsensitiveTerms;

		// Token: 0x04000E1B RID: 3611
		[NonSerialized]
		public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>(StringComparer.Ordinal);

		// Token: 0x04000E1C RID: 3612
		public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;

		// Token: 0x04000E1D RID: 3613
		public string mTerm_AppName;

		// Token: 0x04000E1E RID: 3614
		public List<LanguageData> mLanguages = new List<LanguageData>();

		// Token: 0x04000E1F RID: 3615
		public bool IgnoreDeviceLanguage;

		// Token: 0x04000E20 RID: 3616
		public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;

		// Token: 0x04000E21 RID: 3617
		public string Google_WebServiceURL;

		// Token: 0x04000E22 RID: 3618
		public string Google_SpreadsheetKey;

		// Token: 0x04000E23 RID: 3619
		public string Google_SpreadsheetName;

		// Token: 0x04000E24 RID: 3620
		public string Google_LastUpdatedVersion;

		// Token: 0x04000E25 RID: 3621
		public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;

		// Token: 0x04000E26 RID: 3622
		public LanguageSourceData.eGoogleUpdateFrequency GoogleInEditorCheckFrequency = LanguageSourceData.eGoogleUpdateFrequency.Daily;

		// Token: 0x04000E27 RID: 3623
		public LanguageSourceData.eGoogleUpdateSynchronization GoogleUpdateSynchronization = LanguageSourceData.eGoogleUpdateSynchronization.OnSceneLoaded;

		// Token: 0x04000E28 RID: 3624
		public float GoogleUpdateDelay;

		// Token: 0x04000E2A RID: 3626
		public List<Object> Assets = new List<Object>();

		// Token: 0x04000E2B RID: 3627
		[NonSerialized]
		public Dictionary<string, Object> mAssetDictionary = new Dictionary<string, Object>(StringComparer.Ordinal);

		// Token: 0x04000E2C RID: 3628
		private string mDelayedGoogleData;

		// Token: 0x04000E2D RID: 3629
		public static string EmptyCategory = "Default";

		// Token: 0x04000E2E RID: 3630
		public static char[] CategorySeparators = "/\\".ToCharArray();

		// Token: 0x02000487 RID: 1159
		public enum MissingTranslationAction
		{
			// Token: 0x0400172A RID: 5930
			Empty,
			// Token: 0x0400172B RID: 5931
			Fallback,
			// Token: 0x0400172C RID: 5932
			ShowWarning,
			// Token: 0x0400172D RID: 5933
			ShowTerm
		}

		// Token: 0x02000488 RID: 1160
		public enum eAllowUnloadLanguages
		{
			// Token: 0x0400172F RID: 5935
			Never,
			// Token: 0x04001730 RID: 5936
			OnlyInDevice,
			// Token: 0x04001731 RID: 5937
			EditorAndDevice
		}

		// Token: 0x02000489 RID: 1161
		public enum eGoogleUpdateFrequency
		{
			// Token: 0x04001733 RID: 5939
			Always,
			// Token: 0x04001734 RID: 5940
			Never,
			// Token: 0x04001735 RID: 5941
			Daily,
			// Token: 0x04001736 RID: 5942
			Weekly,
			// Token: 0x04001737 RID: 5943
			Monthly,
			// Token: 0x04001738 RID: 5944
			OnlyOnce,
			// Token: 0x04001739 RID: 5945
			EveryOtherDay
		}

		// Token: 0x0200048A RID: 1162
		public enum eGoogleUpdateSynchronization
		{
			// Token: 0x0400173B RID: 5947
			Manual,
			// Token: 0x0400173C RID: 5948
			OnSceneLoaded,
			// Token: 0x0400173D RID: 5949
			AsSoonAsDownloaded
		}
	}
}
