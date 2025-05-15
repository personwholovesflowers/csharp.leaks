using System;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class SummonedSwords : MonoBehaviour
{
	// Token: 0x060019C8 RID: 6600 RVA: 0x000D36A8 File Offset: 0x000D18A8
	private void Start()
	{
		this.swords = base.GetComponentsInChildren<Projectile>();
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		base.Invoke("Begin", 5f * this.speed);
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x000D36E4 File Offset: 0x000D18E4
	private void FixedUpdate()
	{
		if (!this.inFormation)
		{
			if (this.target != null)
			{
				base.transform.position = this.target.position;
			}
			base.transform.Rotate(Vector3.up, Time.deltaTime * 720f * this.speed, Space.Self);
			return;
		}
		if (this.formation == SummonedSwordFormation.Spiral)
		{
			base.transform.position = this.target.position;
			if (this.spinning)
			{
				base.transform.Rotate(Vector3.up, Time.deltaTime * 720f * this.speed, Space.Self);
			}
		}
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x000D3784 File Offset: 0x000D1984
	private void Begin()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.difficulty > 3)
		{
			this.inFormation = true;
			this.target = this.targetEnemy;
			base.transform.position = this.target.position;
			foreach (Projectile projectile in this.swords)
			{
				if (projectile)
				{
					Collider collider;
					if (projectile.TryGetComponent<Collider>(out collider))
					{
						collider.enabled = false;
					}
					projectile.target = this.target;
					projectile.transform.localPosition += projectile.transform.forward * 5f;
					projectile.transform.Rotate(Vector3.up, 180f, Space.World);
				}
			}
			this.spinning = true;
			base.Invoke("StopSpinning", 0.75f * this.speed);
			base.Invoke("SpiralStab", 1f * this.speed);
			return;
		}
		foreach (Projectile projectile2 in this.swords)
		{
			if (projectile2)
			{
				Object.Instantiate<GameObject>(projectile2.explosionEffect, projectile2.transform.position, Quaternion.identity);
			}
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x000D38D8 File Offset: 0x000D1AD8
	private void SpiralStab()
	{
		foreach (Projectile projectile in this.swords)
		{
			if (projectile)
			{
				Collider collider;
				if (projectile.TryGetComponent<Collider>(out collider))
				{
					collider.enabled = true;
				}
				projectile.ignoreEnvironment = false;
				projectile.speed = 150f;
			}
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x000D3934 File Offset: 0x000D1B34
	private void StopSpinning()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.spinning = false;
		Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.parryableFlash, base.transform.position, Quaternion.identity);
		foreach (Projectile projectile in this.swords)
		{
			if (projectile)
			{
				Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.parryableFlash, projectile.transform.position, projectile.transform.rotation).transform.localScale *= 10f;
				projectile.transform.SetParent(null, true);
				projectile.unparryable = false;
				projectile.undeflectable = false;
			}
		}
	}

	// Token: 0x04002417 RID: 9239
	public EnemyTarget target;

	// Token: 0x04002418 RID: 9240
	private bool inFormation;

	// Token: 0x04002419 RID: 9241
	private SummonedSwordFormation formation;

	// Token: 0x0400241A RID: 9242
	private Projectile[] swords;

	// Token: 0x0400241B RID: 9243
	public float speed = 1f;

	// Token: 0x0400241C RID: 9244
	[HideInInspector]
	public EnemyTarget targetEnemy;

	// Token: 0x0400241D RID: 9245
	private int difficulty;

	// Token: 0x0400241E RID: 9246
	private bool spinning;
}
