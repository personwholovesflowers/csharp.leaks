using System;
using UnityEngine;

// Token: 0x0200038C RID: 908
public class RevolverAnimationReceiver : MonoBehaviour
{
	// Token: 0x060014E8 RID: 5352 RVA: 0x000A93B5 File Offset: 0x000A75B5
	private void Start()
	{
		this.rev = base.GetComponentInParent<Revolver>();
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x000A93C3 File Offset: 0x000A75C3
	public void ReadyGun()
	{
		this.rev.ReadyGun();
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x000A93D0 File Offset: 0x000A75D0
	public void Click()
	{
		if (this.click)
		{
			Object.Instantiate<GameObject>(this.click);
		}
		this.rev.cylinder.DoTurn();
		this.rev.Click();
	}

	// Token: 0x04001CDC RID: 7388
	private Revolver rev;

	// Token: 0x04001CDD RID: 7389
	public GameObject click;
}
