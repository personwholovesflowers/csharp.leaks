using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class AttributeChecker : MonoBehaviour
{
	// Token: 0x060001FB RID: 507 RVA: 0x0000A726 File Offset: 0x00008926
	public void DelayedActivate(float time = 0.5f)
	{
		base.Invoke("Activate", time);
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000A734 File Offset: 0x00008934
	public void Activate()
	{
		this.toActivate.gameObject.SetActive(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400021C RID: 540
	public HitterAttribute targetAttribute;

	// Token: 0x0400021D RID: 541
	public GameObject toActivate;
}
