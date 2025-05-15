using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class Bleeder : MonoBehaviour
{
	// Token: 0x06000253 RID: 595 RVA: 0x0000D118 File Offset: 0x0000B318
	public void GetHit(Vector3 point, GoreType type, bool fromExplosion = false)
	{
		if (this.gz == null)
		{
			this.gz = GoreZone.ResolveGoreZone(base.transform);
		}
		GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(type, false, false, false, null, fromExplosion);
		if (!gore)
		{
			return;
		}
		gore.transform.position = point;
		gore.transform.SetParent(this.gz.goreZone, true);
		Bloodsplatter component = gore.GetComponent<Bloodsplatter>();
		gore.SetActive(true);
		if (!component)
		{
			return;
		}
		component.GetReady();
	}

	// Token: 0x040002B4 RID: 692
	private GoreZone gz;

	// Token: 0x040002B5 RID: 693
	public EnemyType[] ignoreTypes;
}
