using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class LODGroupStaticerBiasMultiplier : MonoBehaviour
{
	// Token: 0x060004AE RID: 1198 RVA: 0x0001B3C4 File Offset: 0x000195C4
	public float GetBias()
	{
		if (Application.platform != RuntimePlatform.Switch)
		{
			return this.normalBiasMultiplier;
		}
		return this.switchBiasMultiplier;
	}

	// Token: 0x0400047C RID: 1148
	public float normalBiasMultiplier = 1f;

	// Token: 0x0400047D RID: 1149
	public float switchBiasMultiplier = 0.67f;
}
