using System;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class DeletePlayerPref : MonoBehaviour
{
	// Token: 0x060004FF RID: 1279 RVA: 0x00021F6C File Offset: 0x0002016C
	private void Start()
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		if (this.playerPref == "cg_custom_pool")
		{
			this.playerPref = "cyberGrind.customPool";
		}
		MonoSingleton<PrefsManager>.Instance.DeleteKey(this.playerPref);
	}

	// Token: 0x040006EE RID: 1774
	public string playerPref;
}
