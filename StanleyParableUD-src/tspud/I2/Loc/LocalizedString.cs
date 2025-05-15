using System;

namespace I2.Loc
{
	// Token: 0x020002C6 RID: 710
	[Serializable]
	public struct LocalizedString
	{
		// Token: 0x06001261 RID: 4705 RVA: 0x000632D1 File Offset: 0x000614D1
		public static implicit operator string(LocalizedString s)
		{
			return s.ToString();
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x000632E0 File Offset: 0x000614E0
		public static implicit operator LocalizedString(string term)
		{
			return new LocalizedString
			{
				mTerm = term
			};
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000632FE File Offset: 0x000614FE
		public LocalizedString(LocalizedString str)
		{
			this.mTerm = str.mTerm;
			this.mRTL_IgnoreArabicFix = str.mRTL_IgnoreArabicFix;
			this.mRTL_MaxLineLength = str.mRTL_MaxLineLength;
			this.mRTL_ConvertNumbers = str.mRTL_ConvertNumbers;
			this.m_DontLocalizeParameters = str.m_DontLocalizeParameters;
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0006333C File Offset: 0x0006153C
		public override string ToString()
		{
			string translation = LocalizationManager.GetTranslation(this.mTerm, !this.mRTL_IgnoreArabicFix, this.mRTL_MaxLineLength, !this.mRTL_ConvertNumbers, true, null, null);
			LocalizationManager.ApplyLocalizationParams(ref translation, !this.m_DontLocalizeParameters);
			return translation;
		}

		// Token: 0x04000E94 RID: 3732
		public string mTerm;

		// Token: 0x04000E95 RID: 3733
		public bool mRTL_IgnoreArabicFix;

		// Token: 0x04000E96 RID: 3734
		public int mRTL_MaxLineLength;

		// Token: 0x04000E97 RID: 3735
		public bool mRTL_ConvertNumbers;

		// Token: 0x04000E98 RID: 3736
		public bool m_DontLocalizeParameters;
	}
}
