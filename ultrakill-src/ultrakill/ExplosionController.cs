using System;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class ExplosionController : MonoBehaviour
{
	// Token: 0x06000908 RID: 2312 RVA: 0x0003BF08 File Offset: 0x0003A108
	private void Start()
	{
		string text = this.playerPref;
		if (!(text == "SimFir"))
		{
			if (text == "SimExp")
			{
				this.playerPref = "simpleExplosions";
			}
		}
		else
		{
			this.playerPref = "simpleFire";
		}
		if (!MonoSingleton<PrefsManager>.Instance.GetBoolLocal(this.playerPref, false) && !this.forceSimple)
		{
			foreach (GameObject gameObject in this.toActivate)
			{
				if (gameObject)
				{
					gameObject.SetActive(true);
				}
			}
		}
		if (this.tryIgniteGasoline)
		{
			MonoSingleton<StainVoxelManager>.Instance.TryIgniteAt(base.transform.position, (this.overrideVoxelCheckSize > 0) ? this.overrideVoxelCheckSize : 3);
		}
	}

	// Token: 0x04000B77 RID: 2935
	public bool forceSimple;

	// Token: 0x04000B78 RID: 2936
	public bool tryIgniteGasoline = true;

	// Token: 0x04000B79 RID: 2937
	public int overrideVoxelCheckSize;

	// Token: 0x04000B7A RID: 2938
	public GameObject[] toActivate;

	// Token: 0x04000B7B RID: 2939
	public string playerPref;
}
