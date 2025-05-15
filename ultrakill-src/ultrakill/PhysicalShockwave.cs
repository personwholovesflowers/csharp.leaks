using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000333 RID: 819
public class PhysicalShockwave : MonoBehaviour
{
	// Token: 0x060012E7 RID: 4839 RVA: 0x000965E4 File Offset: 0x000947E4
	private void Start()
	{
		if (this.soundEffect != null)
		{
			Object.Instantiate<GameObject>(this.soundEffect, base.transform.position, Quaternion.identity);
		}
		this.faders = base.GetComponentsInChildren<ScaleNFade>();
		if (!this.fading)
		{
			foreach (ScaleNFade scaleNFade in this.faders)
			{
				scaleNFade.enabled = false;
				scaleNFade.fade = true;
				scaleNFade.fadeSpeed = this.speed / 10f;
			}
		}
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x00096668 File Offset: 0x00094868
	private void Update()
	{
		base.transform.localScale = new Vector3(base.transform.localScale.x + Time.deltaTime * this.speed, base.transform.localScale.y, base.transform.localScale.z + Time.deltaTime * this.speed);
		if (!this.fading && (base.transform.localScale.x > this.maxSize || base.transform.localScale.z > this.maxSize))
		{
			this.fading = true;
			ScaleNFade[] array = this.faders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
			base.Invoke("GetDestroyed", this.speed / 10f);
		}
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x00096743 File Offset: 0x00094943
	private void OnCollisionEnter(Collision collision)
	{
		if (this.fading)
		{
			return;
		}
		this.CheckCollision(collision.collider);
	}

	// Token: 0x060012EA RID: 4842 RVA: 0x0009675A File Offset: 0x0009495A
	private void OnTriggerEnter(Collider collision)
	{
		if (this.fading)
		{
			return;
		}
		this.CheckCollision(collision);
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x0009676C File Offset: 0x0009496C
	private void CheckCollision(Collider col)
	{
		if (this.hasHurtPlayer || col.gameObject.layer == 15 || !col.gameObject.CompareTag("Player"))
		{
			Landmine landmine;
			if (col.gameObject.layer == 10)
			{
				EnemyIdentifierIdentifier component = col.gameObject.GetComponent<EnemyIdentifierIdentifier>();
				if (component != null && component.eid != null && (!this.enemy || (component.eid.enemyType != this.enemyType && !component.eid.immuneToFriendlyFire && !EnemyIdentifier.CheckHurtException(this.enemyType, component.eid.enemyType, this.target))))
				{
					Collider component2 = component.eid.GetComponent<Collider>();
					float num = (float)this.damage / 10f;
					if (this.noDamageToEnemy || base.transform.localScale.x > 10f || base.transform.localScale.z > 10f)
					{
						num = 0f;
					}
					if (component2 != null && !this.hitColliders.Contains(component2) && !component.eid.dead)
					{
						this.hitColliders.Add(component2);
						if (this.enemy)
						{
							component.eid.hitter = "enemy";
						}
						else
						{
							component.eid.hitter = "explosion";
						}
						Turret turret;
						if (component.eid.enemyType == EnemyType.Turret && component.eid.TryGetComponent<Turret>(out turret) && turret.lodged)
						{
							turret.Unlodge();
						}
						component.eid.DeliverDamage(col.gameObject, Vector3.up * this.force * 2f, col.transform.position, num, false, 0f, null, false, false);
						return;
					}
					if (component2 != null && component.eid.dead)
					{
						this.hitColliders.Add(component2);
						component.eid.hitter = "explosion";
						component.eid.DeliverDamage(col.gameObject, Vector3.up * 2000f, col.transform.position, num, false, 0f, null, false, false);
						return;
					}
				}
			}
			else if (!this.enemy && col.attachedRigidbody && col.attachedRigidbody.TryGetComponent<Landmine>(out landmine))
			{
				landmine.Activate(1.5f);
			}
			return;
		}
		this.hasHurtPlayer = true;
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
		{
			NewMovement instance = MonoSingleton<NewMovement>.Instance;
			instance.GetHurt(this.damage, true, 1f, false, false, 0.35f, false);
			instance.LaunchFromPoint(instance.transform.position + Vector3.down, 30f, 30f);
			return;
		}
		if (this.damage == 0)
		{
			MonoSingleton<PlatformerMovement>.Instance.Jump(false, 1f);
			return;
		}
		MonoSingleton<PlatformerMovement>.Instance.Explode(false);
	}

	// Token: 0x060012EC RID: 4844 RVA: 0x0000A719 File Offset: 0x00008919
	private void GetDestroyed()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040019DC RID: 6620
	public EnemyTarget target;

	// Token: 0x040019DD RID: 6621
	public int damage;

	// Token: 0x040019DE RID: 6622
	public float speed;

	// Token: 0x040019DF RID: 6623
	public float maxSize;

	// Token: 0x040019E0 RID: 6624
	public float force;

	// Token: 0x040019E1 RID: 6625
	public bool hasHurtPlayer;

	// Token: 0x040019E2 RID: 6626
	public bool enemy;

	// Token: 0x040019E3 RID: 6627
	public bool noDamageToEnemy;

	// Token: 0x040019E4 RID: 6628
	private List<Collider> hitColliders = new List<Collider>();

	// Token: 0x040019E5 RID: 6629
	public EnemyType enemyType;

	// Token: 0x040019E6 RID: 6630
	public GameObject soundEffect;

	// Token: 0x040019E7 RID: 6631
	[HideInInspector]
	public bool fading;

	// Token: 0x040019E8 RID: 6632
	private ScaleNFade[] faders;
}
