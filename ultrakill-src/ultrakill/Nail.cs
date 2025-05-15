using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000310 RID: 784
public class Nail : MonoBehaviour
{
	// Token: 0x060011D5 RID: 4565 RVA: 0x0008AAAC File Offset: 0x00088CAC
	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
		if (!this.rb)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x0008AAD4 File Offset: 0x00088CD4
	private void Start()
	{
		if (this.sawblade)
		{
			this.removeTimeMultiplier = 3f;
		}
		if (this.magnets.Count == 0)
		{
			base.Invoke("RemoveTime", 5f * this.removeTimeMultiplier);
		}
		this.sinceLastEnviroParticle = 1f;
		base.Invoke("MasterRemoveTime", 60f);
		this.startPosition = base.transform.position;
		base.Invoke("SlowUpdate", 2f);
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x0008AB59 File Offset: 0x00088D59
	private void OnDestroy()
	{
		if (this.zapped)
		{
			Object.Instantiate<GameObject>(this.zapParticle, base.transform.position, base.transform.rotation);
		}
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x0008AB85 File Offset: 0x00088D85
	private void SlowUpdate()
	{
		if (Vector3.Distance(base.transform.position, this.startPosition) > 1000f)
		{
			this.RemoveTime();
			return;
		}
		base.Invoke("SlowUpdate", 2f);
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x0008ABBC File Offset: 0x00088DBC
	private void Update()
	{
		if (!this.hit)
		{
			if (!this.rb)
			{
				this.rb = base.GetComponent<Rigidbody>();
			}
			if (this.rb)
			{
				base.transform.LookAt(base.transform.position + this.rb.velocity * -1f);
			}
		}
		if (this.sameEnemyHitCooldown > 0f && !this.stopped)
		{
			this.sameEnemyHitCooldown = Mathf.MoveTowards(this.sameEnemyHitCooldown, 0f, Time.deltaTime);
			if (this.sameEnemyHitCooldown <= 0f)
			{
				this.currentHitEnemy = null;
			}
		}
		if (this.multiHitAmount <= 1)
		{
			return;
		}
		if (this.multiHitCooldown > 0f)
		{
			this.multiHitCooldown = Mathf.MoveTowards(this.multiHitCooldown, 0f, Time.deltaTime);
		}
		else if (this.stopped)
		{
			if (!this.currentHitEnemy.dead && this.currentMultiHitAmount > 0)
			{
				this.currentMultiHitAmount--;
				this.hitAmount -= 1f;
				this.DamageEnemy(this.hitTarget, this.currentHitEnemy);
			}
			if (this.currentHitEnemy.dead || this.currentMultiHitAmount <= 0)
			{
				this.stopped = false;
				this.rb.velocity = this.originalVelocity;
				if (this.hitAmount <= 0f)
				{
					this.SawBreak();
				}
				return;
			}
			this.multiHitCooldown = 0.15f;
		}
		if (this.stoppedAud)
		{
			if (this.stopped)
			{
				this.stoppedAud.pitch = 2f;
				this.stoppedAud.volume = 0.5f;
				return;
			}
			this.stoppedAud.pitch = 1f;
			this.stoppedAud.volume = 0.25f;
		}
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x0008AD98 File Offset: 0x00088F98
	private void FixedUpdate()
	{
		if (!this.sawblade || !this.rb || this.hit)
		{
			return;
		}
		if (this.stopped)
		{
			this.rb.velocity = Vector3.zero;
			return;
		}
		if (this.magnets.Count > 0)
		{
			this.magnets.RemoveAll((Magnet magnet) => magnet == null);
			if (this.magnets.Count == 0)
			{
				return;
			}
			Magnet targetMagnet = this.GetTargetMagnet();
			if (!targetMagnet)
			{
				return;
			}
			if (this.punched)
			{
				if (Vector3.Distance(base.transform.position, targetMagnet.transform.position) > this.punchDistance)
				{
					this.punched = false;
					this.punchDistance = 0f;
					this.rb.velocity = Vector3.RotateTowards(this.rb.velocity, Quaternion.Euler(0f, (float)(85 * this.magnetRotationDirection), 0f) * (targetMagnet.transform.position - base.transform.position).normalized * this.rb.velocity.magnitude, float.PositiveInfinity, this.rb.velocity.magnitude);
				}
			}
			else
			{
				this.rb.velocity = Vector3.RotateTowards(this.rb.velocity, Quaternion.Euler(0f, (float)(85 * this.magnetRotationDirection), 0f) * (targetMagnet.transform.position - base.transform.position).normalized * this.rb.velocity.magnitude, float.PositiveInfinity, this.rb.velocity.magnitude);
			}
		}
		RaycastHit[] array = this.rb.SweepTestAll(this.rb.velocity.normalized, this.rb.velocity.magnitude * Time.fixedDeltaTime, QueryTriggerInteraction.Ignore);
		if (array == null || array.Length == 0)
		{
			return;
		}
		Array.Sort<RaycastHit>(array, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i].transform.gameObject;
			if (!this.hit && (gameObject.layer == 10 || gameObject.layer == 11) && (gameObject.gameObject.CompareTag("Head") || gameObject.gameObject.CompareTag("Body") || gameObject.gameObject.CompareTag("Limb") || gameObject.gameObject.CompareTag("EndLimb") || gameObject.gameObject.CompareTag("Enemy")))
			{
				this.TouchEnemy(gameObject.transform);
			}
			else if (LayerMaskDefaults.IsMatchingLayer(gameObject.layer, LMD.Environment) || gameObject.layer == 26 || gameObject.CompareTag("Armor"))
			{
				Breakable breakable;
				if (gameObject.TryGetComponent<Breakable>(out breakable) && ((breakable.weak && !breakable.specialCaseOnly) || (breakable.forceSawbladeable && !this.chainsaw)))
				{
					if (breakable.forceSawbladeable)
					{
						breakable.ForceBreak();
						return;
					}
					breakable.Break();
					return;
				}
				else
				{
					if (this.hitAmount <= 0f)
					{
						this.SawBreak();
						return;
					}
					base.transform.position = array[i].point;
					if (this.bounceToSurfaceNormal)
					{
						this.rb.velocity = array[i].normal * this.rb.velocity.magnitude;
					}
					else
					{
						this.rb.velocity = Vector3.Reflect(this.rb.velocity.normalized, array[i].normal) * this.rb.velocity.magnitude;
					}
					flag = true;
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.sawBounceEffect, array[i].point, Quaternion.LookRotation(array[i].normal));
					AudioSource audioSource;
					if (flag2 && gameObject2.TryGetComponent<AudioSource>(out audioSource))
					{
						audioSource.enabled = false;
					}
					else
					{
						flag2 = true;
					}
					if (SceneHelper.IsStaticEnvironment(array[i]) && this.sinceLastEnviroParticle > 0.25f)
					{
						MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(array[i], 3, 0.5f);
						this.sinceLastEnviroParticle = 0f;
					}
					this.punched = false;
					this.punchable = true;
					if (this.magnets.Count > 0)
					{
						this.magnetRotationDirection *= -1;
						this.hitAmount -= 0.1f;
						break;
					}
					this.hitAmount -= 0.25f;
					break;
				}
			}
		}
		if (flag)
		{
			int num = 0;
			RaycastHit raycastHit;
			while (num < 3 && Physics.Raycast(base.transform.position, this.rb.velocity.normalized, out raycastHit, 5f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				Breakable breakable2;
				if (raycastHit.transform.TryGetComponent<Breakable>(out breakable2) && ((breakable2.weak && !breakable2.specialCaseOnly) || (breakable2.forceSawbladeable && !this.chainsaw)))
				{
					if (breakable2.forceSawbladeable)
					{
						breakable2.ForceBreak();
						return;
					}
					breakable2.Break();
					return;
				}
				else
				{
					base.transform.position = raycastHit.point;
					if (this.bounceToSurfaceNormal)
					{
						this.rb.velocity = raycastHit.normal * this.rb.velocity.magnitude;
					}
					else
					{
						this.rb.velocity = Vector3.Reflect(this.rb.velocity.normalized, raycastHit.normal) * this.rb.velocity.magnitude;
					}
					this.hitAmount -= 0.125f;
					GameObject gameObject3 = Object.Instantiate<GameObject>(this.sawBounceEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
					AudioSource audioSource2;
					if (flag2 && gameObject3.TryGetComponent<AudioSource>(out audioSource2))
					{
						audioSource2.enabled = false;
					}
					else
					{
						flag2 = true;
					}
					if (SceneHelper.IsStaticEnvironment(raycastHit) && this.sinceLastEnviroParticle > 0.25f)
					{
						MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(raycastHit, 3, 0.5f);
						this.sinceLastEnviroParticle = 0f;
					}
					this.punched = false;
					this.punchable = true;
					num++;
				}
			}
		}
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x0008B494 File Offset: 0x00089694
	public Magnet GetTargetMagnet()
	{
		Magnet magnet = null;
		float num = float.PositiveInfinity;
		for (int i = 0; i < this.magnets.Count; i++)
		{
			Vector3 vector = this.magnets[i].transform.position - base.transform.position;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				magnet = this.magnets[i];
				Vector3 normalized = new Vector3(this.rb.velocity.z, this.rb.velocity.y, -this.rb.velocity.x).normalized;
				if (Vector3.Dot(vector, normalized) > 0f)
				{
					this.magnetRotationDirection = -1;
				}
				else
				{
					this.magnetRotationDirection = 1;
				}
			}
		}
		return magnet;
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x0008B56C File Offset: 0x0008976C
	private void OnCollisionEnter(Collision other)
	{
		if (this.hit)
		{
			return;
		}
		GameObject gameObject = other.gameObject;
		if ((gameObject.layer == 10 || gameObject.layer == 11) && (gameObject.CompareTag("Head") || gameObject.CompareTag("Body") || gameObject.CompareTag("Limb") || gameObject.CompareTag("EndLimb") || gameObject.CompareTag("Enemy")))
		{
			this.TouchEnemy(other.transform);
			return;
		}
		if (this.enemy && gameObject.layer == 2)
		{
			MonoSingleton<NewMovement>.Instance.GetHurt(8, true, 1f, false, false, 0.35f, false);
			this.hit = true;
			Object.Destroy(base.gameObject);
			return;
		}
		if (this.magnets.Count == 0 && LayerMaskDefaults.IsMatchingLayer(gameObject.layer, LMD.Environment))
		{
			this.hit = true;
			base.CancelInvoke("RemoveTime");
			base.Invoke("RemoveTime", 1f);
			if (SceneHelper.IsStaticEnvironment(other.collider) && this.sinceLastEnviroParticle > 0.25f)
			{
				MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(other.GetContact(0), 1, 0.5f);
				this.sinceLastEnviroParticle = 0f;
			}
			if (this.aud == null)
			{
				this.aud = base.GetComponent<AudioSource>();
			}
			this.aud.clip = this.environmentHitSound;
			this.aud.pitch = Random.Range(0.9f, 1.1f);
			this.aud.volume = 0.2f;
			this.aud.Play();
			Breakable component = gameObject.GetComponent<Breakable>();
			if (component != null && (((component.weak || this.heated) && !component.precisionOnly && !component.specialCaseOnly && !component.forceSawbladeable) || (this.sawblade && !this.chainsaw && component.forceSawbladeable)))
			{
				if (component.forceSawbladeable)
				{
					component.ForceBreak();
				}
				else
				{
					component.Break();
				}
			}
			Bleeder bleeder;
			if (gameObject.TryGetComponent<Bleeder>(out bleeder))
			{
				bleeder.GetHit(base.transform.position, GoreType.Small, false);
			}
			if (this.heated)
			{
				Flammable componentInChildren = gameObject.GetComponentInChildren<Flammable>();
				if (componentInChildren != null && (this.enemy || !componentInChildren.enemyOnly) && (!this.enemy || !componentInChildren.playerOnly))
				{
					componentInChildren.Burn(2f, false);
				}
			}
		}
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x0008B7D8 File Offset: 0x000899D8
	private void OnTriggerEnter(Collider other)
	{
		if (this.sawblade)
		{
			return;
		}
		if (!this.hit && (other.gameObject.layer == 10 || other.gameObject.layer == 11) && (other.gameObject.CompareTag("Head") || other.gameObject.CompareTag("Body") || other.gameObject.CompareTag("Limb") || other.gameObject.CompareTag("EndLimb") || other.gameObject.CompareTag("Enemy")))
		{
			this.hit = true;
			this.TouchEnemy(other.transform);
		}
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x0008B884 File Offset: 0x00089A84
	private void TouchEnemy(Transform other)
	{
		if (!this.sawblade || this.multiHitAmount <= 1)
		{
			this.HitEnemy(other, null);
			return;
		}
		if (this.stopped)
		{
			return;
		}
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (!other.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) || !enemyIdentifierIdentifier.eid)
		{
			return;
		}
		if (enemyIdentifierIdentifier.eid.dead)
		{
			this.HitEnemy(other, enemyIdentifierIdentifier);
			return;
		}
		if (this.sameEnemyHitCooldown > 0f && this.currentHitEnemy != null && this.currentHitEnemy == enemyIdentifierIdentifier.eid)
		{
			return;
		}
		this.stopped = true;
		this.currentMultiHitAmount = this.multiHitAmount;
		this.hitTarget = other;
		this.currentHitEnemy = enemyIdentifierIdentifier.eid;
		this.originalVelocity = this.rb.velocity;
		this.sameEnemyHitCooldown = 0.05f;
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x0008B95C File Offset: 0x00089B5C
	private void HitEnemy(Transform other, EnemyIdentifierIdentifier eidid = null)
	{
		if (!eidid && !other.TryGetComponent<EnemyIdentifierIdentifier>(out eidid))
		{
			return;
		}
		if (!eidid.eid)
		{
			return;
		}
		if (this.enemy && eidid && eidid.eid && eidid.eid.enemyType == this.safeEnemyType)
		{
			return;
		}
		if (this.sawblade)
		{
			if (this.sameEnemyHitCooldown > 0f && this.currentHitEnemy != null && this.currentHitEnemy == eidid.eid)
			{
				return;
			}
			if (this.hitLimbs.Contains(other))
			{
				return;
			}
		}
		if (!this.sawblade)
		{
			this.hit = true;
		}
		else if (!eidid.eid.dead)
		{
			this.sameEnemyHitCooldown = 0.05f;
			this.currentHitEnemy = eidid.eid;
			this.hitAmount -= 1f;
		}
		if (this.aud == null)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		this.aud.clip = this.enemyHitSound;
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		this.aud.volume = 0.2f;
		this.aud.Play();
		if (eidid && eidid.eid)
		{
			if (this.sawblade && eidid.eid.zapperer != null)
			{
				eidid.eid.zapperer.damage += 0.5f;
				eidid.eid.zapperer.ChargeBoost(0.5f);
			}
			this.DamageEnemy(other, eidid.eid);
		}
		if (this.sawblade)
		{
			if (this.hitAmount < 1f)
			{
				this.SawBreak();
			}
			return;
		}
		if (this.rb == null)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
		Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		this.rb.isKinematic = true;
		Object.Destroy(this.rb);
		base.transform.position += base.transform.forward * -0.5f;
		base.transform.SetParent(other.transform, true);
		TrailRenderer trailRenderer;
		if (base.TryGetComponent<TrailRenderer>(out trailRenderer))
		{
			trailRenderer.enabled = false;
		}
		base.CancelInvoke("RemoveTime");
	}

	// Token: 0x060011E0 RID: 4576 RVA: 0x0008BBDC File Offset: 0x00089DDC
	private void DamageEnemy(Transform other, EnemyIdentifier eid)
	{
		if (!this.sawblade)
		{
			eid.hitter = "nail";
		}
		else if (this.chainsaw)
		{
			eid.hitter = "chainsawprojectile";
		}
		else
		{
			eid.hitter = "sawblade";
		}
		if (!eid.hitterWeapons.Contains(this.weaponType))
		{
			eid.hitterWeapons.Add(this.weaponType);
		}
		if (this.sawHitEffect != null)
		{
			Object.Instantiate<GameObject>(this.sawHitEffect, other.transform.position, Quaternion.identity).transform.localScale *= 3f;
		}
		bool flag = false;
		if (this.magnets.Count > 0)
		{
			using (List<Magnet>.Enumerator enumerator = this.magnets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ignoredEids.Contains(eid))
					{
						flag = true;
						break;
					}
				}
			}
		}
		bool dead = eid.dead;
		if (this.fodderDamageBoost && !eid.dead)
		{
			this.damage *= this.GetFodderDamageMultiplier(eid.enemyType);
		}
		if (this.nbc && !this.sawblade)
		{
			if (!this.nbc.damagedEnemies.Contains(eid))
			{
				eid.DeliverDamage(other.gameObject, (other.transform.position - base.transform.position).normalized * 3000f, base.transform.position, this.damage * (float)(this.nbc.nails.Count / 2) * (float)(this.punched ? 2 : 1), true, 0f, this.sourceWeapon, false, false);
				this.nbc.damagedEnemies.Add(eid);
			}
		}
		else
		{
			eid.DeliverDamage(other.gameObject, (other.transform.position - base.transform.position).normalized * 3000f, base.transform.position, this.damage * (float)(this.punched ? 2 : 1), false, 0f, this.sourceWeapon, false, false);
		}
		if (!dead && eid.dead && !flag && this.magnets.Count > 0)
		{
			if (this.magnets.Count > 1)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(Mathf.RoundToInt(120f), "ultrakill.bipolar", this.sourceWeapon, eid, -1, "", "");
			}
			else
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(Mathf.RoundToInt(60f), "ultrakill.attripator", this.sourceWeapon, eid, -1, "", "");
			}
		}
		else if (this.launched && !this.sawblade)
		{
			if (!dead && eid.dead)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(Mathf.RoundToInt(120f), "ultrakill.nailbombed", this.sourceWeapon, eid, -1, "", "");
			}
			else if (!eid.dead)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(Mathf.RoundToInt(10f), "ultrakill.nailbombedalive", this.sourceWeapon, eid, -1, "", "");
			}
		}
		if (!dead && !this.sawblade)
		{
			eid.nailsAmount++;
			eid.nails.Add(this);
		}
		else if (dead && this.sawblade)
		{
			this.hitLimbs.Add(other);
		}
		if (this.heated)
		{
			Flammable componentInChildren = eid.GetComponentInChildren<Flammable>();
			if (componentInChildren != null && (this.enemy || !componentInChildren.enemyOnly) && (!this.enemy || !componentInChildren.playerOnly))
			{
				componentInChildren.Burn(2f, componentInChildren.burning);
			}
		}
		if (dead)
		{
			int count = this.magnets.Count;
			return;
		}
	}

	// Token: 0x060011E1 RID: 4577 RVA: 0x0008BFF4 File Offset: 0x0008A1F4
	public void MagnetCaught(Magnet mag)
	{
		base.CancelInvoke("RemoveTime");
		this.launched = false;
		this.enemy = false;
		if (this.sawblade)
		{
			this.punchable = true;
		}
		if (!this.magnets.Contains(mag))
		{
			this.magnets.Add(mag);
		}
		if (this.nbc)
		{
			this.nbc.nails.Remove(this);
			this.nbc = null;
		}
	}

	// Token: 0x060011E2 RID: 4578 RVA: 0x0008C06C File Offset: 0x0008A26C
	public void MagnetRelease(Magnet mag)
	{
		base.CancelInvoke("RemoveTime");
		if (this.magnets.Contains(mag))
		{
			this.magnets.Remove(mag);
			if (this.magnets.Count == 0)
			{
				SphereCollider sphereCollider;
				if (base.TryGetComponent<SphereCollider>(out sphereCollider))
				{
					sphereCollider.enabled = true;
				}
				this.launched = true;
			}
		}
		if (this.magnets.Count == 0)
		{
			base.Invoke("RemoveTime", 5f * this.removeTimeMultiplier);
		}
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x0008C0E8 File Offset: 0x0008A2E8
	public void Zap()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component)
		{
			component.material = this.zapMaterial;
		}
		this.zapped = true;
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x0000A719 File Offset: 0x00008919
	private void RemoveTime()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x0008C117 File Offset: 0x0008A317
	private void MasterRemoveTime()
	{
		this.RemoveTime();
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x0008C11F File Offset: 0x0008A31F
	public void SawBreak()
	{
		this.hit = true;
		Object.Instantiate<GameObject>(this.sawBreakEffect, base.transform.position, Quaternion.identity);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x0008C150 File Offset: 0x0008A350
	private float GetFodderDamageMultiplier(EnemyType et)
	{
		if (et == EnemyType.Filth)
		{
			return 2f;
		}
		switch (et)
		{
		case EnemyType.Stalker:
			return 1.5f;
		case EnemyType.Stray:
			return 2f;
		case EnemyType.Schism:
			return 1.5f;
		case EnemyType.Soldier:
			return 1.5f;
		default:
			return 1f;
		}
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x0008C1A0 File Offset: 0x0008A3A0
	public void ForceCheckSawbladeRicochet()
	{
		if (!this.rb)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
		bool flag = false;
		int num = 0;
		RaycastHit raycastHit;
		while (num < 3 && Physics.Raycast(base.transform.position, this.rb.velocity.normalized, out raycastHit, 5f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			Breakable breakable;
			if (raycastHit.transform.TryGetComponent<Breakable>(out breakable) && ((breakable.weak && !breakable.specialCaseOnly) || (breakable.forceSawbladeable && !this.chainsaw)))
			{
				if (breakable.forceSawbladeable)
				{
					breakable.ForceBreak();
					return;
				}
				breakable.Break();
				return;
			}
			else
			{
				base.transform.position = raycastHit.point;
				if (this.bounceToSurfaceNormal)
				{
					this.rb.velocity = raycastHit.normal * this.rb.velocity.magnitude;
				}
				else
				{
					this.rb.velocity = Vector3.Reflect(this.rb.velocity.normalized, raycastHit.normal) * this.rb.velocity.magnitude;
				}
				this.hitAmount -= 0.125f;
				GameObject gameObject = Object.Instantiate<GameObject>(this.sawBounceEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
				AudioSource audioSource;
				if (flag && gameObject.TryGetComponent<AudioSource>(out audioSource))
				{
					audioSource.enabled = false;
				}
				else
				{
					flag = true;
				}
				num++;
			}
		}
		Collider[] array = Physics.OverlapSphere(base.transform.position, 1.5f, LayerMaskDefaults.Get(LMD.Enemies));
		if (array.Length != 0)
		{
			this.TouchEnemy(array[0].transform);
		}
	}

	// Token: 0x04001841 RID: 6209
	public GameObject sourceWeapon;

	// Token: 0x04001842 RID: 6210
	[HideInInspector]
	public bool hit;

	// Token: 0x04001843 RID: 6211
	public float damage;

	// Token: 0x04001844 RID: 6212
	private AudioSource aud;

	// Token: 0x04001845 RID: 6213
	[HideInInspector]
	public Rigidbody rb;

	// Token: 0x04001846 RID: 6214
	public AudioClip environmentHitSound;

	// Token: 0x04001847 RID: 6215
	public AudioClip enemyHitSound;

	// Token: 0x04001848 RID: 6216
	public Material zapMaterial;

	// Token: 0x04001849 RID: 6217
	public GameObject zapParticle;

	// Token: 0x0400184A RID: 6218
	private bool zapped;

	// Token: 0x0400184B RID: 6219
	public bool fodderDamageBoost;

	// Token: 0x0400184C RID: 6220
	public string weaponType;

	// Token: 0x0400184D RID: 6221
	public bool heated;

	// Token: 0x0400184E RID: 6222
	[HideInInspector]
	public List<Magnet> magnets = new List<Magnet>();

	// Token: 0x0400184F RID: 6223
	private bool launched;

	// Token: 0x04001850 RID: 6224
	[HideInInspector]
	public NailBurstController nbc;

	// Token: 0x04001851 RID: 6225
	public bool enemy;

	// Token: 0x04001852 RID: 6226
	public EnemyType safeEnemyType;

	// Token: 0x04001853 RID: 6227
	private Vector3 startPosition;

	// Token: 0x04001854 RID: 6228
	[Header("Sawblades")]
	public bool sawblade;

	// Token: 0x04001855 RID: 6229
	public bool chainsaw;

	// Token: 0x04001856 RID: 6230
	public float hitAmount = 3.9f;

	// Token: 0x04001857 RID: 6231
	private EnemyIdentifier currentHitEnemy;

	// Token: 0x04001858 RID: 6232
	private float sameEnemyHitCooldown;

	// Token: 0x04001859 RID: 6233
	[SerializeField]
	private GameObject sawBreakEffect;

	// Token: 0x0400185A RID: 6234
	[SerializeField]
	private GameObject sawBounceEffect;

	// Token: 0x0400185B RID: 6235
	[SerializeField]
	private GameObject sawHitEffect;

	// Token: 0x0400185C RID: 6236
	[HideInInspector]
	public int magnetRotationDirection;

	// Token: 0x0400185D RID: 6237
	private List<Transform> hitLimbs = new List<Transform>();

	// Token: 0x0400185E RID: 6238
	private float removeTimeMultiplier = 1f;

	// Token: 0x0400185F RID: 6239
	public bool bounceToSurfaceNormal;

	// Token: 0x04001860 RID: 6240
	[HideInInspector]
	public bool stopped;

	// Token: 0x04001861 RID: 6241
	public int multiHitAmount = 1;

	// Token: 0x04001862 RID: 6242
	private int currentMultiHitAmount;

	// Token: 0x04001863 RID: 6243
	private float multiHitCooldown;

	// Token: 0x04001864 RID: 6244
	private Transform hitTarget;

	// Token: 0x04001865 RID: 6245
	[HideInInspector]
	public Vector3 originalVelocity;

	// Token: 0x04001866 RID: 6246
	public AudioSource stoppedAud;

	// Token: 0x04001867 RID: 6247
	[HideInInspector]
	public bool punchable;

	// Token: 0x04001868 RID: 6248
	[HideInInspector]
	public bool punched;

	// Token: 0x04001869 RID: 6249
	[HideInInspector]
	public float punchDistance;

	// Token: 0x0400186A RID: 6250
	private TimeSince sinceLastEnviroParticle;
}
