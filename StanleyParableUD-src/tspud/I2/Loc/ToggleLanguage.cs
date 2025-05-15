using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200028D RID: 653
	public class ToggleLanguage : MonoBehaviour
	{
		// Token: 0x06001064 RID: 4196 RVA: 0x00056091 File Offset: 0x00054291
		private void Start()
		{
			base.Invoke("test", 3f);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000560A4 File Offset: 0x000542A4
		private void test()
		{
			List<string> allLanguages = LocalizationManager.GetAllLanguages(true);
			int num = allLanguages.IndexOf(LocalizationManager.CurrentLanguage);
			if (num >= 0)
			{
				num = (num + 1) % allLanguages.Count;
			}
			base.Invoke("test", 3f);
		}
	}
}
