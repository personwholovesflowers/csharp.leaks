using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000362 RID: 866
public class ProjectileSpread : MonoBehaviour
{
	// Token: 0x06001416 RID: 5142 RVA: 0x000A18A4 File Offset: 0x0009FAA4
	private void Start()
	{
		if (!this.dontSpawn && this.spreadAmount > 0f)
		{
			Projectile componentInChildren = base.GetComponentInChildren<Projectile>();
			if (componentInChildren.target == null || (this.target != null && componentInChildren.target != this.target))
			{
				componentInChildren.target = this.target;
			}
			this.projectile = componentInChildren.gameObject;
			GameObject gameObject = new GameObject();
			gameObject.transform.SetPositionAndRotation(base.transform.position, base.transform.rotation);
			for (int i = 0; i <= this.projectileAmount; i++)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.projectile, gameObject.transform.position + gameObject.transform.up * 0.1f, gameObject.transform.rotation);
				gameObject2.transform.Rotate(Vector3.right * this.spreadAmount);
				gameObject2.transform.SetParent(base.transform, true);
				gameObject.transform.Rotate(Vector3.forward * (float)(360 / this.projectileAmount));
			}
			Object.Destroy(gameObject);
		}
		if (this.timeUntilDestroy == 0f)
		{
			this.timeUntilDestroy = 5f;
		}
		base.Invoke("Remove", this.timeUntilDestroy);
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x000A19FF File Offset: 0x0009FBFF
	public void ParriedProjectile()
	{
		this.parried = true;
		base.CancelInvoke("NoLongerParried");
		base.Invoke("NoLongerParried", 1f);
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x000A1A23 File Offset: 0x0009FC23
	private void NoLongerParried()
	{
		this.parried = false;
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x0000A719 File Offset: 0x00008919
	private void Remove()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001B91 RID: 7057
	private GameObject projectile;

	// Token: 0x04001B92 RID: 7058
	public float spreadAmount;

	// Token: 0x04001B93 RID: 7059
	public int projectileAmount;

	// Token: 0x04001B94 RID: 7060
	public float timeUntilDestroy;

	// Token: 0x04001B95 RID: 7061
	public bool parried;

	// Token: 0x04001B96 RID: 7062
	public bool dontSpawn;

	// Token: 0x04001B97 RID: 7063
	public EnemyTarget target;

	// Token: 0x04001B98 RID: 7064
	[HideInInspector]
	public List<EnemyIdentifier> hitEnemies = new List<EnemyIdentifier>();
}
