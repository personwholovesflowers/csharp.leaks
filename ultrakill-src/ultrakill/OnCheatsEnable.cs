using System;
using UnityEngine;

// Token: 0x02000320 RID: 800
public class OnCheatsEnable : MonoBehaviour
{
	// Token: 0x06001279 RID: 4729 RVA: 0x000941CA File Offset: 0x000923CA
	private void Update()
	{
		if (MonoSingleton<CheatsController>.Instance.cheatsEnabled || (this.includeMajorAssists && MonoSingleton<StatsManager>.Instance.majorUsed))
		{
			this.onCheatsEnable.Invoke("");
			base.enabled = false;
		}
	}

	// Token: 0x0400196C RID: 6508
	public bool includeMajorAssists;

	// Token: 0x0400196D RID: 6509
	public UltrakillEvent onCheatsEnable;
}
