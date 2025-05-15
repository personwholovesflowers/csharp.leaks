using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class AddKill : MonoBehaviour
{
	// Token: 0x060000F4 RID: 244 RVA: 0x00005FC0 File Offset: 0x000041C0
	private void Start()
	{
		GoreZone goreZone = GoreZone.ResolveGoreZone(base.transform);
		if (goreZone != null && goreZone.checkpoint != null)
		{
			goreZone.AddDeath();
			goreZone.checkpoint.sm.kills++;
			return;
		}
		MonoSingleton<StatsManager>.Instance.kills++;
	}
}
