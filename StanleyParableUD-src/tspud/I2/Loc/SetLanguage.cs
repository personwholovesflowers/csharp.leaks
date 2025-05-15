using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002D1 RID: 721
	[AddComponentMenu("I2/Localization/SetLanguage Button")]
	public class SetLanguage : MonoBehaviour
	{
		// Token: 0x06001285 RID: 4741 RVA: 0x00064C27 File Offset: 0x00062E27
		private void OnClick()
		{
			this.ApplyLanguage();
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00064C2F File Offset: 0x00062E2F
		public void ApplyLanguage()
		{
			if (LocalizationManager.HasLanguage(this._Language, true, true, true))
			{
				LocalizationManager.CurrentLanguage = this._Language;
			}
		}

		// Token: 0x04000EF9 RID: 3833
		public string _Language;
	}
}
