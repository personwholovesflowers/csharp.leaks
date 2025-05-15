using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002BF RID: 703
	public class AutoChangeCultureInfo : MonoBehaviour
	{
		// Token: 0x0600123E RID: 4670 RVA: 0x00062998 File Offset: 0x00060B98
		public void Start()
		{
			LocalizationManager.EnableChangingCultureInfo(true);
		}
	}
}
