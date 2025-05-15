using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000288 RID: 648
	public class Example_ChangeLanguage : MonoBehaviour
	{
		// Token: 0x0600104D RID: 4173 RVA: 0x00055B70 File Offset: 0x00053D70
		public void SetLanguage_English()
		{
			this.SetLanguage("English");
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00055B7D File Offset: 0x00053D7D
		public void SetLanguage_French()
		{
			this.SetLanguage("French");
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00055B8A File Offset: 0x00053D8A
		public void SetLanguage_Spanish()
		{
			this.SetLanguage("Spanish");
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00055B97 File Offset: 0x00053D97
		public void SetLanguage(string LangName)
		{
			if (LocalizationManager.HasLanguage(LangName, true, true, true))
			{
				LocalizationManager.CurrentLanguage = LangName;
			}
		}
	}
}
