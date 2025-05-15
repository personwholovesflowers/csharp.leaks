using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002A0 RID: 672
	[AddComponentMenu("I2/Localization/Source")]
	[ExecuteInEditMode]
	public class LanguageSource : MonoBehaviour, ISerializationCallbackReceiver, ILanguageSource
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060010D4 RID: 4308 RVA: 0x0005BEF2 File Offset: 0x0005A0F2
		// (set) Token: 0x060010D5 RID: 4309 RVA: 0x0005BEFA File Offset: 0x0005A0FA
		public LanguageSourceData SourceData
		{
			get
			{
				return this.mSource;
			}
			set
			{
				this.mSource = value;
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060010D6 RID: 4310 RVA: 0x0005BF04 File Offset: 0x0005A104
		// (remove) Token: 0x060010D7 RID: 4311 RVA: 0x0005BF3C File Offset: 0x0005A13C
		public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

		// Token: 0x060010D8 RID: 4312 RVA: 0x0005BF71 File Offset: 0x0005A171
		private void Awake()
		{
			this.mSource.owner = this;
			this.mSource.Awake();
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0005BF8A File Offset: 0x0005A18A
		private void OnDestroy()
		{
			this.NeverDestroy = false;
			if (!this.NeverDestroy)
			{
				this.mSource.OnDestroy();
			}
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0005BFA8 File Offset: 0x0005A1A8
		public string GetSourceName()
		{
			string text = base.gameObject.name;
			Transform transform = base.transform.parent;
			while (transform)
			{
				text = transform.name + "_" + text;
				transform = transform.parent;
			}
			return text;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0005BFF1 File Offset: 0x0005A1F1
		public void OnBeforeSerialize()
		{
			this.version = 1;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0005BFFC File Offset: 0x0005A1FC
		public void OnAfterDeserialize()
		{
			if (this.version == 0 || this.mSource == null)
			{
				this.mSource = new LanguageSourceData();
				this.mSource.owner = this;
				this.mSource.UserAgreesToHaveItOnTheScene = this.UserAgreesToHaveItOnTheScene;
				this.mSource.UserAgreesToHaveItInsideThePluginsFolder = this.UserAgreesToHaveItInsideThePluginsFolder;
				this.mSource.IgnoreDeviceLanguage = this.IgnoreDeviceLanguage;
				this.mSource._AllowUnloadingLanguages = this._AllowUnloadingLanguages;
				this.mSource.CaseInsensitiveTerms = this.CaseInsensitiveTerms;
				this.mSource.OnMissingTranslation = this.OnMissingTranslation;
				this.mSource.mTerm_AppName = this.mTerm_AppName;
				this.mSource.GoogleLiveSyncIsUptoDate = this.GoogleLiveSyncIsUptoDate;
				this.mSource.Google_WebServiceURL = this.Google_WebServiceURL;
				this.mSource.Google_SpreadsheetKey = this.Google_SpreadsheetKey;
				this.mSource.Google_SpreadsheetName = this.Google_SpreadsheetName;
				this.mSource.Google_LastUpdatedVersion = this.Google_LastUpdatedVersion;
				this.mSource.GoogleUpdateFrequency = this.GoogleUpdateFrequency;
				this.mSource.GoogleUpdateDelay = this.GoogleUpdateDelay;
				this.mSource.Event_OnSourceUpdateFromGoogle += this.Event_OnSourceUpdateFromGoogle;
				if (this.mLanguages != null && this.mLanguages.Count > 0)
				{
					this.mSource.mLanguages.Clear();
					this.mSource.mLanguages.AddRange(this.mLanguages);
					this.mLanguages.Clear();
				}
				if (this.Assets != null && this.Assets.Count > 0)
				{
					this.mSource.Assets.Clear();
					this.mSource.Assets.AddRange(this.Assets);
					this.Assets.Clear();
				}
				if (this.mTerms != null && this.mTerms.Count > 0)
				{
					this.mSource.mTerms.Clear();
					for (int i = 0; i < this.mTerms.Count; i++)
					{
						this.mSource.mTerms.Add(this.mTerms[i]);
					}
					this.mTerms.Clear();
				}
				this.version = 1;
				this.Event_OnSourceUpdateFromGoogle = null;
			}
		}

		// Token: 0x04000DFE RID: 3582
		public LanguageSourceData mSource = new LanguageSourceData();

		// Token: 0x04000DFF RID: 3583
		public int version;

		// Token: 0x04000E00 RID: 3584
		public bool NeverDestroy;

		// Token: 0x04000E01 RID: 3585
		public bool UserAgreesToHaveItOnTheScene;

		// Token: 0x04000E02 RID: 3586
		public bool UserAgreesToHaveItInsideThePluginsFolder;

		// Token: 0x04000E03 RID: 3587
		public bool GoogleLiveSyncIsUptoDate = true;

		// Token: 0x04000E04 RID: 3588
		public List<Object> Assets = new List<Object>();

		// Token: 0x04000E05 RID: 3589
		public string Google_WebServiceURL;

		// Token: 0x04000E06 RID: 3590
		public string Google_SpreadsheetKey;

		// Token: 0x04000E07 RID: 3591
		public string Google_SpreadsheetName;

		// Token: 0x04000E08 RID: 3592
		public string Google_LastUpdatedVersion;

		// Token: 0x04000E09 RID: 3593
		public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;

		// Token: 0x04000E0A RID: 3594
		public float GoogleUpdateDelay = 5f;

		// Token: 0x04000E0C RID: 3596
		public List<LanguageData> mLanguages = new List<LanguageData>();

		// Token: 0x04000E0D RID: 3597
		public bool IgnoreDeviceLanguage;

		// Token: 0x04000E0E RID: 3598
		public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;

		// Token: 0x04000E0F RID: 3599
		public List<TermData> mTerms = new List<TermData>();

		// Token: 0x04000E10 RID: 3600
		public bool CaseInsensitiveTerms;

		// Token: 0x04000E11 RID: 3601
		public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;

		// Token: 0x04000E12 RID: 3602
		public string mTerm_AppName;

		// Token: 0x02000486 RID: 1158
		// (Invoke) Token: 0x06001994 RID: 6548
		public delegate void fnOnSourceUpdated(LanguageSourceData source, bool ReceivedNewData, string errorMsg);
	}
}
