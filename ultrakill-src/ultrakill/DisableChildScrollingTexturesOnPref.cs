using System;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class DisableChildScrollingTexturesOnPref : MonoBehaviour
{
	// Token: 0x0600051C RID: 1308 RVA: 0x00022450 File Offset: 0x00020650
	private void OnEnable()
	{
		bool flag = (this.localPref ? MonoSingleton<PrefsManager>.Instance.GetBoolLocal(this.prefName, false) : MonoSingleton<PrefsManager>.Instance.GetBool(this.prefName, false));
		if (!(this.disableIfTrue ? flag : (!flag)))
		{
			return;
		}
		ScrollingTexture[] componentsInChildren = base.GetComponentsInChildren<ScrollingTexture>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
	}

	// Token: 0x04000707 RID: 1799
	public bool localPref;

	// Token: 0x04000708 RID: 1800
	public string prefName;

	// Token: 0x04000709 RID: 1801
	public bool disableIfTrue = true;
}
