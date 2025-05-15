using System;
using UnityEngine;

// Token: 0x02000314 RID: 788
public class NailgunAnimationReceiver : MonoBehaviour
{
	// Token: 0x06001204 RID: 4612 RVA: 0x0008E89E File Offset: 0x0008CA9E
	private void Start()
	{
		this.ng = base.GetComponentInParent<Nailgun>();
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x0008E8AC File Offset: 0x0008CAAC
	public void CanShoot()
	{
		this.ng.CanShoot();
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x0008E8B9 File Offset: 0x0008CAB9
	public void SnapSound()
	{
		this.ng.SnapSound();
	}

	// Token: 0x040018AF RID: 6319
	private Nailgun ng;
}
