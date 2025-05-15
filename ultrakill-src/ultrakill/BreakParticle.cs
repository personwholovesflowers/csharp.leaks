using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class BreakParticle : MonoBehaviour
{
	// Token: 0x060002EE RID: 750 RVA: 0x000114C1 File Offset: 0x0000F6C1
	private void OnDestroy()
	{
		if (base.gameObject.activeInHierarchy)
		{
			Object.Instantiate<GameObject>(this.particle, base.transform.position, base.transform.rotation);
		}
	}

	// Token: 0x0400038E RID: 910
	public GameObject particle;
}
