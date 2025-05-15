using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	// Token: 0x020002A6 RID: 678
	[AddComponentMenu("I2/Localization/I2 Localize")]
	public class Localize : MonoBehaviour
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x0005EEAC File Offset: 0x0005D0AC
		// (set) Token: 0x0600113E RID: 4414 RVA: 0x0005EEB4 File Offset: 0x0005D0B4
		public string Term
		{
			get
			{
				return this.mTerm;
			}
			set
			{
				this.SetTerm(value);
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600113F RID: 4415 RVA: 0x0005EEBD File Offset: 0x0005D0BD
		// (set) Token: 0x06001140 RID: 4416 RVA: 0x0005EEC5 File Offset: 0x0005D0C5
		public string SecondaryTerm
		{
			get
			{
				return this.mTermSecondary;
			}
			set
			{
				this.SetTerm(null, value);
			}
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0005EECF File Offset: 0x0005D0CF
		private void Awake()
		{
			this.UpdateAssetDictionary();
			this.FindTarget();
			if (this.LocalizeOnAwake)
			{
				this.OnLocalize(false);
			}
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0005EEED File Offset: 0x0005D0ED
		private void OnEnable()
		{
			this.OnLocalize(false);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0005EEF6 File Offset: 0x0005D0F6
		public bool HasCallback()
		{
			return this.LocalizeCallBack.HasCallback() || this.LocalizeEvent.GetPersistentEventCount() > 0;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0005EF18 File Offset: 0x0005D118
		public void OnLocalize(bool Force = false)
		{
			if (!Force && (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy))
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			if (!this.AlwaysForceLocalize && !Force && !this.HasCallback() && this.LastLocalizedLanguage == LocalizationManager.CurrentLanguage)
			{
				return;
			}
			this.LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
			if (string.IsNullOrEmpty(this.FinalTerm) || string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				this.GetFinalTerms(out this.FinalTerm, out this.FinalSecondaryTerm);
			}
			bool flag = I2Utils.IsPlaying() && this.HasCallback();
			if (!flag && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				return;
			}
			Localize.CallBackTerm = this.FinalTerm;
			Localize.CallBackSecondaryTerm = this.FinalSecondaryTerm;
			Localize.MainTranslation = ((string.IsNullOrEmpty(this.FinalTerm) || this.FinalTerm == "-") ? null : LocalizationManager.GetTranslation(this.FinalTerm, false, 0, true, false, null, null));
			Localize.SecondaryTranslation = ((string.IsNullOrEmpty(this.FinalSecondaryTerm) || this.FinalSecondaryTerm == "-") ? null : LocalizationManager.GetTranslation(this.FinalSecondaryTerm, false, 0, true, false, null, null));
			if (!flag && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(Localize.SecondaryTranslation))
			{
				return;
			}
			Localize.CurrentLocalizeComponent = this;
			this.LocalizeCallBack.Execute(this);
			this.LocalizeEvent.Invoke();
			LocalizationManager.ApplyLocalizationParams(ref Localize.MainTranslation, base.gameObject, this.AllowLocalizedParameters);
			if (!this.FindTarget())
			{
				return;
			}
			bool flag2 = LocalizationManager.IsRight2Left && !this.IgnoreRTL;
			if (Localize.MainTranslation != null)
			{
				switch (this.PrimaryTermModifier)
				{
				case Localize.TermModification.ToUpper:
					Localize.MainTranslation = Localize.MainTranslation.ToUpper();
					break;
				case Localize.TermModification.ToLower:
					Localize.MainTranslation = Localize.MainTranslation.ToLower();
					break;
				case Localize.TermModification.ToUpperFirst:
					Localize.MainTranslation = GoogleTranslation.UppercaseFirst(Localize.MainTranslation);
					break;
				case Localize.TermModification.ToTitle:
					Localize.MainTranslation = GoogleTranslation.TitleCase(Localize.MainTranslation);
					break;
				}
				if (!string.IsNullOrEmpty(this.TermPrefix))
				{
					Localize.MainTranslation = (flag2 ? (Localize.MainTranslation + this.TermPrefix) : (this.TermPrefix + Localize.MainTranslation));
				}
				if (!string.IsNullOrEmpty(this.TermSuffix))
				{
					Localize.MainTranslation = (flag2 ? (this.TermSuffix + Localize.MainTranslation) : (Localize.MainTranslation + this.TermSuffix));
				}
				if (this.AddSpacesToJoinedLanguages && LocalizationManager.HasJoinedWords && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(Localize.MainTranslation[0]);
					int i = 1;
					int length = Localize.MainTranslation.Length;
					while (i < length)
					{
						stringBuilder.Append(' ');
						stringBuilder.Append(Localize.MainTranslation[i]);
						i++;
					}
					Localize.MainTranslation = stringBuilder.ToString();
				}
				if (flag2 && this.mLocalizeTarget.AllowMainTermToBeRTL() && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					Localize.MainTranslation = LocalizationManager.ApplyRTLfix(Localize.MainTranslation, this.MaxCharactersInRTL, this.IgnoreNumbersInRTL);
				}
			}
			if (Localize.SecondaryTranslation != null)
			{
				switch (this.SecondaryTermModifier)
				{
				case Localize.TermModification.ToUpper:
					Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToUpper();
					break;
				case Localize.TermModification.ToLower:
					Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToLower();
					break;
				case Localize.TermModification.ToUpperFirst:
					Localize.SecondaryTranslation = GoogleTranslation.UppercaseFirst(Localize.SecondaryTranslation);
					break;
				case Localize.TermModification.ToTitle:
					Localize.SecondaryTranslation = GoogleTranslation.TitleCase(Localize.SecondaryTranslation);
					break;
				}
				if (flag2 && this.mLocalizeTarget.AllowSecondTermToBeRTL() && !string.IsNullOrEmpty(Localize.SecondaryTranslation))
				{
					Localize.SecondaryTranslation = LocalizationManager.ApplyRTLfix(Localize.SecondaryTranslation);
				}
			}
			if (LocalizationManager.HighlightLocalizedTargets)
			{
				Localize.MainTranslation = "LOC:" + this.FinalTerm;
			}
			this.mLocalizeTarget.DoLocalize(this, Localize.MainTranslation, Localize.SecondaryTranslation);
			Localize.CurrentLocalizeComponent = null;
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0005F334 File Offset: 0x0005D534
		public bool FindTarget()
		{
			if (this.mLocalizeTarget != null && this.mLocalizeTarget.IsValid(this))
			{
				return true;
			}
			if (this.mLocalizeTarget != null)
			{
				Object.DestroyImmediate(this.mLocalizeTarget);
				this.mLocalizeTarget = null;
				this.mLocalizeTargetName = null;
			}
			if (!string.IsNullOrEmpty(this.mLocalizeTargetName))
			{
				foreach (ILocalizeTargetDescriptor localizeTargetDescriptor in LocalizationManager.mLocalizeTargets)
				{
					if (this.mLocalizeTargetName == localizeTargetDescriptor.GetTargetType().ToString())
					{
						if (localizeTargetDescriptor.CanLocalize(this))
						{
							this.mLocalizeTarget = localizeTargetDescriptor.CreateTarget(this);
						}
						if (this.mLocalizeTarget != null)
						{
							return true;
						}
					}
				}
			}
			foreach (ILocalizeTargetDescriptor localizeTargetDescriptor2 in LocalizationManager.mLocalizeTargets)
			{
				if (localizeTargetDescriptor2.CanLocalize(this))
				{
					this.mLocalizeTarget = localizeTargetDescriptor2.CreateTarget(this);
					this.mLocalizeTargetName = localizeTargetDescriptor2.GetTargetType().ToString();
					if (this.mLocalizeTarget != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0005F48C File Offset: 0x0005D68C
		public void GetFinalTerms(out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = string.Empty;
			secondaryTerm = string.Empty;
			if (!this.FindTarget())
			{
				return;
			}
			if (this.mLocalizeTarget != null)
			{
				this.mLocalizeTarget.GetFinalTerms(this, this.mTerm, this.mTermSecondary, out primaryTerm, out secondaryTerm);
				primaryTerm = I2Utils.GetValidTermName(primaryTerm, false);
			}
			if (!string.IsNullOrEmpty(this.mTerm))
			{
				primaryTerm = this.mTerm;
			}
			if (!string.IsNullOrEmpty(this.mTermSecondary))
			{
				secondaryTerm = this.mTermSecondary;
			}
			if (primaryTerm != null)
			{
				primaryTerm = primaryTerm.Trim();
			}
			if (secondaryTerm != null)
			{
				secondaryTerm = secondaryTerm.Trim();
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0005F528 File Offset: 0x0005D728
		public string GetMainTargetsText()
		{
			string text = null;
			string text2 = null;
			if (this.mLocalizeTarget != null)
			{
				this.mLocalizeTarget.GetFinalTerms(this, null, null, out text, out text2);
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return this.mTerm;
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x0005F569 File Offset: 0x0005D769
		public void SetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm, bool RemoveNonASCII)
		{
			primaryTerm = (RemoveNonASCII ? I2Utils.GetValidTermName(Main, false) : Main);
			secondaryTerm = Secondary;
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0005F580 File Offset: 0x0005D780
		public void SetTerm(string primary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.OnLocalize(true);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0005F5AC File Offset: 0x0005D7AC
		public void SetTerm(string primary, string secondary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.mTermSecondary = secondary;
			this.FinalSecondaryTerm = secondary;
			this.OnLocalize(true);
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0005F5E8 File Offset: 0x0005D7E8
		internal T GetSecondaryTranslatedObj<T>(ref string mainTranslation, ref string secondaryTranslation) where T : Object
		{
			string text;
			string text2;
			this.DeserializeTranslation(mainTranslation, out text, out text2);
			T t = default(T);
			if (!string.IsNullOrEmpty(text2))
			{
				t = this.GetObject<T>(text2);
				if (t != null)
				{
					mainTranslation = text;
					secondaryTranslation = text2;
				}
			}
			if (t == null)
			{
				t = this.GetObject<T>(secondaryTranslation);
			}
			return t;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0005F648 File Offset: 0x0005D848
		public void UpdateAssetDictionary()
		{
			this.TranslatedObjects.RemoveAll((Object x) => x == null);
			this.mAssetDictionary = (from o in this.TranslatedObjects.Distinct<Object>()
				group o by o.name).ToDictionary((IGrouping<string, Object> g) => g.Key, (IGrouping<string, Object> g) => g.First<Object>());
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0005F6F8 File Offset: 0x0005D8F8
		internal T GetObject<T>(string Translation) where T : Object
		{
			if (string.IsNullOrEmpty(Translation))
			{
				return default(T);
			}
			return this.GetTranslatedObject<T>(Translation);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0005F71E File Offset: 0x0005D91E
		private T GetTranslatedObject<T>(string Translation) where T : Object
		{
			return this.FindTranslatedObject<T>(Translation);
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0005F728 File Offset: 0x0005D928
		private void DeserializeTranslation(string translation, out string value, out string secondary)
		{
			if (!string.IsNullOrEmpty(translation) && translation.Length > 1 && translation[0] == '[')
			{
				int num = translation.IndexOf(']');
				if (num > 0)
				{
					secondary = translation.Substring(1, num - 1);
					value = translation.Substring(num + 1);
					return;
				}
			}
			value = translation;
			secondary = string.Empty;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0005F780 File Offset: 0x0005D980
		public T FindTranslatedObject<T>(string value) where T : Object
		{
			if (string.IsNullOrEmpty(value))
			{
				T t = default(T);
				return t;
			}
			if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.TranslatedObjects.Count)
			{
				this.UpdateAssetDictionary();
			}
			foreach (KeyValuePair<string, Object> keyValuePair in this.mAssetDictionary)
			{
				if (keyValuePair.Value is T && value.EndsWith(keyValuePair.Key, StringComparison.OrdinalIgnoreCase) && string.Compare(value, keyValuePair.Key, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return (T)((object)keyValuePair.Value);
				}
			}
			T t2 = LocalizationManager.FindAsset(value) as T;
			if (t2)
			{
				return t2;
			}
			return ResourceManager.pInstance.GetAsset<T>(value);
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0005F870 File Offset: 0x0005DA70
		public bool HasTranslatedObject(Object Obj)
		{
			return this.TranslatedObjects.Contains(Obj) || ResourceManager.pInstance.HasAsset(Obj);
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0005F88D File Offset: 0x0005DA8D
		public void AddTranslatedObject(Object Obj)
		{
			if (this.TranslatedObjects.Contains(Obj))
			{
				return;
			}
			this.TranslatedObjects.Add(Obj);
			this.UpdateAssetDictionary();
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x0005F8B0 File Offset: 0x0005DAB0
		public void SetGlobalLanguage(string Language)
		{
			LocalizationManager.CurrentLanguage = Language;
		}

		// Token: 0x04000E34 RID: 3636
		public string mTerm = string.Empty;

		// Token: 0x04000E35 RID: 3637
		public string mTermSecondary = string.Empty;

		// Token: 0x04000E36 RID: 3638
		[NonSerialized]
		public string FinalTerm;

		// Token: 0x04000E37 RID: 3639
		[NonSerialized]
		public string FinalSecondaryTerm;

		// Token: 0x04000E38 RID: 3640
		public Localize.TermModification PrimaryTermModifier;

		// Token: 0x04000E39 RID: 3641
		public Localize.TermModification SecondaryTermModifier;

		// Token: 0x04000E3A RID: 3642
		public string TermPrefix;

		// Token: 0x04000E3B RID: 3643
		public string TermSuffix;

		// Token: 0x04000E3C RID: 3644
		public bool LocalizeOnAwake = true;

		// Token: 0x04000E3D RID: 3645
		private string LastLocalizedLanguage;

		// Token: 0x04000E3E RID: 3646
		public bool IgnoreRTL;

		// Token: 0x04000E3F RID: 3647
		public int MaxCharactersInRTL;

		// Token: 0x04000E40 RID: 3648
		public bool IgnoreNumbersInRTL = true;

		// Token: 0x04000E41 RID: 3649
		public bool CorrectAlignmentForRTL = true;

		// Token: 0x04000E42 RID: 3650
		public bool AddSpacesToJoinedLanguages;

		// Token: 0x04000E43 RID: 3651
		public bool AllowLocalizedParameters = true;

		// Token: 0x04000E44 RID: 3652
		public List<Object> TranslatedObjects = new List<Object>();

		// Token: 0x04000E45 RID: 3653
		[NonSerialized]
		public Dictionary<string, Object> mAssetDictionary = new Dictionary<string, Object>(StringComparer.Ordinal);

		// Token: 0x04000E46 RID: 3654
		public UnityEvent LocalizeEvent = new UnityEvent();

		// Token: 0x04000E47 RID: 3655
		public static string MainTranslation;

		// Token: 0x04000E48 RID: 3656
		public static string SecondaryTranslation;

		// Token: 0x04000E49 RID: 3657
		public static string CallBackTerm;

		// Token: 0x04000E4A RID: 3658
		public static string CallBackSecondaryTerm;

		// Token: 0x04000E4B RID: 3659
		public static Localize CurrentLocalizeComponent;

		// Token: 0x04000E4C RID: 3660
		public bool AlwaysForceLocalize;

		// Token: 0x04000E4D RID: 3661
		[SerializeField]
		public EventCallback LocalizeCallBack = new EventCallback();

		// Token: 0x04000E4E RID: 3662
		public bool mGUI_ShowReferences;

		// Token: 0x04000E4F RID: 3663
		public bool mGUI_ShowTems = true;

		// Token: 0x04000E50 RID: 3664
		public bool mGUI_ShowCallback;

		// Token: 0x04000E51 RID: 3665
		public ILocalizeTarget mLocalizeTarget;

		// Token: 0x04000E52 RID: 3666
		public string mLocalizeTargetName;

		// Token: 0x0200048D RID: 1165
		public enum TermModification
		{
			// Token: 0x0400174B RID: 5963
			DontModify,
			// Token: 0x0400174C RID: 5964
			ToUpper,
			// Token: 0x0400174D RID: 5965
			ToLower,
			// Token: 0x0400174E RID: 5966
			ToUpperFirst,
			// Token: 0x0400174F RID: 5967
			ToTitle
		}
	}
}
