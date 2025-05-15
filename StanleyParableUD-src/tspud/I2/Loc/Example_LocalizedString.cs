using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000289 RID: 649
	public class Example_LocalizedString : MonoBehaviour
	{
		// Token: 0x06001052 RID: 4178 RVA: 0x00055BAC File Offset: 0x00053DAC
		public void Start()
		{
			Debug.Log(this._MyLocalizedString);
			Debug.Log(LocalizationManager.GetTranslation(this._NormalString, true, 0, true, false, null, null));
			Debug.Log(LocalizationManager.GetTranslation(this._StringWithTermPopup, true, 0, true, false, null, null));
			Debug.Log("Term2");
			Debug.Log(this._MyLocalizedString);
			Debug.Log("Term3");
			LocalizedString localizedString = "Term3";
			localizedString.mRTL_IgnoreArabicFix = true;
			Debug.Log(localizedString);
			LocalizedString localizedString2 = "Term3";
			localizedString2.mRTL_ConvertNumbers = true;
			localizedString2.mRTL_MaxLineLength = 20;
			Debug.Log(localizedString2);
			Debug.Log(localizedString2);
		}

		// Token: 0x04000DC5 RID: 3525
		public LocalizedString _MyLocalizedString;

		// Token: 0x04000DC6 RID: 3526
		public string _NormalString;

		// Token: 0x04000DC7 RID: 3527
		[TermsPopup("")]
		public string _StringWithTermPopup;
	}
}
