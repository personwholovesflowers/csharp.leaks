using System;
using UnityEngine;

// Token: 0x0200022C RID: 556
public class GoreDisabler : MonoBehaviour
{
	// Token: 0x06000BE5 RID: 3045 RVA: 0x00053981 File Offset: 0x00051B81
	private void Awake()
	{
		if (!MonoSingleton<BloodsplatterManager>.Instance.forceGibs && !MonoSingleton<PrefsManager>.Instance.GetBoolLocal("bloodEnabled", false))
		{
			base.gameObject.SetActive(false);
		}
	}
}
