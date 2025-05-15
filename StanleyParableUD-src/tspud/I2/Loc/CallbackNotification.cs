using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000287 RID: 647
	public class CallbackNotification : MonoBehaviour
	{
		// Token: 0x0600104B RID: 4171 RVA: 0x00055B30 File Offset: 0x00053D30
		public void OnModifyLocalization()
		{
			if (string.IsNullOrEmpty(Localize.MainTranslation))
			{
				return;
			}
			string translation = LocalizationManager.GetTranslation("Color/Red", true, 0, true, false, null, null);
			Localize.MainTranslation = Localize.MainTranslation.Replace("{PLAYER_COLOR}", translation);
		}
	}
}
