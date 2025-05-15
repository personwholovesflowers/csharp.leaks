using System;
using UnityEngine;

// Token: 0x0200038F RID: 911
public class RevolverBeamParticle : MonoBehaviour
{
	// Token: 0x060014F7 RID: 5367 RVA: 0x000AB47C File Offset: 0x000A967C
	private void Awake()
	{
		if (this.type == 0)
		{
			this.rev = MonoSingleton<CameraController>.Instance.GetComponentInChildren<Revolver>();
		}
		else if (this.type == 1)
		{
			this.secRev = MonoSingleton<CameraController>.Instance.GetComponentInChildren<SecondaryRevolver>();
		}
		if (this.rev != null)
		{
			base.transform.forward = this.rev.hit.normal;
		}
		else if (this.secRev != null)
		{
			base.transform.forward = this.secRev.hit.normal;
		}
		base.Invoke("Destroy", 2f);
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x0000A719 File Offset: 0x00008919
	private void Destroy()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001D17 RID: 7447
	public int type;

	// Token: 0x04001D18 RID: 7448
	private Revolver rev;

	// Token: 0x04001D19 RID: 7449
	private SecondaryRevolver secRev;
}
