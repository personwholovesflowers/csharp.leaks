using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class BlackHoleTrigger : MonoBehaviour
{
	// Token: 0x06000250 RID: 592 RVA: 0x0000CFBC File Offset: 0x0000B1BC
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 12)
		{
			if (!this.bhp)
			{
				this.bhp = base.GetComponentInParent<BlackHoleProjectile>();
			}
			EnemyIdentifier component = other.GetComponent<EnemyIdentifier>();
			if (component && (!this.bhp.enemy || (component.enemyType != this.bhp.safeType && !component.immuneToFriendlyFire && !EnemyIdentifier.CheckHurtException(this.bhp.safeType, component.enemyType, null) && !this.bhp.shootList.Contains(component))))
			{
				this.bhp.shootList.Add(component);
			}
		}
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000D068 File Offset: 0x0000B268
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 12)
		{
			if (!this.bhp)
			{
				this.bhp = base.GetComponentInParent<BlackHoleProjectile>();
			}
			EnemyIdentifier component = other.GetComponent<EnemyIdentifier>();
			if (component && (!this.bhp.enemy || (component.enemyType != this.bhp.safeType && !component.immuneToFriendlyFire && !EnemyIdentifier.CheckHurtException(this.bhp.safeType, component.enemyType, null) && this.bhp.shootList.Contains(component))))
			{
				this.bhp.shootList.Remove(component);
			}
		}
	}

	// Token: 0x040002B3 RID: 691
	private BlackHoleProjectile bhp;
}
