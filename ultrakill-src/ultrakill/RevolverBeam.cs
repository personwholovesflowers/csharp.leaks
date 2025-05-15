using System;
using System.Collections.Generic;
using CustomRay;
using Sandbox;
using UnityEngine;

// Token: 0x0200038E RID: 910
public class RevolverBeam : MonoBehaviour
{
	// Token: 0x060014EC RID: 5356 RVA: 0x000A9408 File Offset: 0x000A7608
	private void Start()
	{
		if (this.aimAssist)
		{
			this.RicochetAimAssist(base.gameObject, this.intentionalRicochet);
		}
		if (this.ricochetAmount > 0)
		{
			this.hasBeenRicocheter = true;
		}
		this.muzzleLight = base.GetComponent<Light>();
		this.lr = base.GetComponent<LineRenderer>();
		this.cc = MonoSingleton<CameraController>.Instance;
		this.gc = this.cc.GetComponentInChildren<GunControl>();
		if (this.beamType == BeamType.Enemy)
		{
			this.enemyLayerMask |= 4;
		}
		this.enemyLayerMask |= 1024;
		this.enemyLayerMask |= 2048;
		if (this.canHitProjectiles)
		{
			this.enemyLayerMask |= 16384;
		}
		this.pierceLayerMask |= 64;
		this.pierceLayerMask |= 128;
		this.pierceLayerMask |= 256;
		this.pierceLayerMask |= 16777216;
		this.pierceLayerMask |= 67108864;
		this.ignoreEnemyTrigger = this.enemyLayerMask | this.pierceLayerMask;
		if (!this.fake)
		{
			this.Shoot();
		}
		else
		{
			this.fadeOut = true;
		}
		if (this.maxHitsPerTarget == 0)
		{
			this.maxHitsPerTarget = 99;
		}
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x000A95C4 File Offset: 0x000A77C4
	private void Update()
	{
		if (this.fadeOut)
		{
			this.lr.widthMultiplier -= Time.deltaTime * 1.5f;
			if (this.muzzleLight != null)
			{
				this.muzzleLight.intensity -= Time.deltaTime * 100f;
			}
			if (this.lr.widthMultiplier <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x000A9640 File Offset: 0x000A7840
	public void FakeShoot(Vector3 target)
	{
		Vector3 position = base.transform.position;
		if (this.alternateStartPoint != Vector3.zero)
		{
			position = this.alternateStartPoint;
		}
		this.lr.SetPosition(0, position);
		this.lr.SetPosition(1, target);
		Transform child = base.transform.GetChild(0);
		if (!this.noMuzzleflash)
		{
			child.SetPositionAndRotation(position, base.transform.rotation);
			return;
		}
		child.gameObject.SetActive(false);
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x000A96C0 File Offset: 0x000A78C0
	private void Shoot()
	{
		if (this.hitAmount == 1)
		{
			this.fadeOut = true;
			if (this.beamType != BeamType.Enemy)
			{
				if (this.beamType == BeamType.Railgun)
				{
					this.cc.CameraShake(2f * this.screenshakeMultiplier);
				}
				else if (this.strongAlt)
				{
					this.cc.CameraShake(0.25f * this.screenshakeMultiplier);
				}
			}
			bool flag = Physics.Raycast(base.transform.position, base.transform.forward, out this.hit, float.PositiveInfinity, this.ignoreEnemyTrigger);
			this.CheckWater(this.hit.distance);
			bool flag2 = false;
			RaycastHit raycastHit = default(RaycastHit);
			if (flag && LayerMaskDefaults.IsMatchingLayer(this.hit.transform.gameObject.layer, LMD.Environment))
			{
				flag2 = Physics.SphereCast(base.transform.position, (this.beamType == BeamType.Enemy) ? 0.1f : 0.4f, base.transform.forward, out raycastHit, Vector3.Distance(base.transform.position, this.hit.point) - ((this.beamType == BeamType.Enemy) ? 0.1f : 0.4f), this.enemyLayerMask);
			}
			if (flag2)
			{
				this.HitSomething(raycastHit);
			}
			else if (flag)
			{
				this.HitSomething(this.hit);
			}
			else
			{
				this.shotHitPoint = base.transform.position + base.transform.forward * 1000f;
			}
		}
		else
		{
			if (Physics.Raycast(base.transform.position, base.transform.forward, out this.hit, float.PositiveInfinity, this.pierceLayerMask))
			{
				this.shotHitPoint = this.hit.point;
			}
			else
			{
				this.shotHitPoint = base.transform.position + base.transform.forward * 999f;
				this.didntHit = true;
			}
			this.CheckWater(Vector3.Distance(base.transform.position, this.shotHitPoint));
			float num = 0.6f;
			if (this.beamType == BeamType.Railgun)
			{
				num = 1.2f;
			}
			else if (this.beamType == BeamType.Enemy)
			{
				num = 0.3f;
			}
			this.allHits = Physics.SphereCastAll(base.transform.position, num, base.transform.forward, Vector3.Distance(base.transform.position, this.shotHitPoint), this.enemyLayerMask, QueryTriggerInteraction.Collide);
		}
		Vector3 position = base.transform.position;
		if (this.alternateStartPoint != Vector3.zero)
		{
			position = this.alternateStartPoint;
		}
		this.lr.SetPosition(0, position);
		this.lr.SetPosition(1, this.shotHitPoint);
		if (this.hitAmount != 1)
		{
			this.PiercingShotOrder();
		}
		Transform child = base.transform.GetChild(0);
		if (!this.noMuzzleflash)
		{
			child.SetPositionAndRotation(position, base.transform.rotation);
			return;
		}
		child.gameObject.SetActive(false);
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x000A99E8 File Offset: 0x000A7BE8
	private void CheckWater(float distance)
	{
		if (this.attributes.Length == 0)
		{
			return;
		}
		bool flag = false;
		HitterAttribute[] array = this.attributes;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == HitterAttribute.Electricity)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		Water water = null;
		List<Water> list = new List<Water>();
		List<GameObject> list2 = new List<GameObject>();
		Collider[] array2 = Physics.OverlapSphere(base.transform.position, 0.01f, 16, QueryTriggerInteraction.Collide);
		if (array2.Length != 0)
		{
			for (int j = 0; j < array2.Length; j++)
			{
				if ((!array2[j].attachedRigidbody && !array2[j].TryGetComponent<Water>(out water)) || (array2[j].attachedRigidbody && !array2[j].attachedRigidbody.TryGetComponent<Water>(out water)))
				{
					return;
				}
				if (list.Contains(water))
				{
					return;
				}
				list.Add(water);
				EnemyIdentifier.Zap(base.transform.position, 2f, list2, this.sourceWeapon, null, water, true);
			}
		}
		RaycastHit[] array3 = Physics.RaycastAll(base.transform.position, base.transform.forward, distance, 16, QueryTriggerInteraction.Collide);
		if (array3.Length == 0)
		{
			return;
		}
		for (int k = 0; k < array3.Length; k++)
		{
			if (!array3[k].transform.TryGetComponent<Water>(out water))
			{
				return;
			}
			if (list.Contains(water))
			{
				return;
			}
			list.Add(water);
			EnemyIdentifier.Zap(array3[k].point, 2f, list2, this.sourceWeapon, null, water, true);
		}
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x000A9B6C File Offset: 0x000A7D6C
	private void HitSomething(RaycastHit hit)
	{
		bool flag = false;
		if (LayerMaskDefaults.IsMatchingLayer(hit.transform.gameObject.layer, LMD.Environment))
		{
			this.ExecuteHits(hit);
		}
		else if (this.beamType != BeamType.Revolver && hit.transform.gameObject.CompareTag("Coin"))
		{
			flag = true;
			this.lr.SetPosition(1, hit.transform.position);
			GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject, hit.point, base.transform.rotation);
			gameObject.SetActive(false);
			RevolverBeam component = gameObject.GetComponent<RevolverBeam>();
			component.bodiesPierced = 0;
			component.noMuzzleflash = true;
			component.alternateStartPoint = Vector3.zero;
			if (this.beamType == BeamType.MaliciousFace || this.beamType == BeamType.Enemy)
			{
				component.deflected = true;
			}
			Coin component2 = hit.transform.gameObject.GetComponent<Coin>();
			if (component2 != null)
			{
				if (component.deflected)
				{
					component2.ignoreBlessedEnemies = true;
				}
				this.sourceWeapon = component2.sourceWeapon ?? this.sourceWeapon;
				component2.DelayedReflectRevolver(hit.point, gameObject);
			}
			this.fadeOut = true;
		}
		else
		{
			this.ExecuteHits(hit);
		}
		this.shotHitPoint = hit.point;
		if (!hit.transform.gameObject.CompareTag("Armor") && !flag && this.hitParticle != null)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.hitParticle, this.shotHitPoint, base.transform.rotation);
			gameObject2.transform.forward = hit.normal;
			Explosion[] componentsInChildren = gameObject2.GetComponentsInChildren<Explosion>();
			foreach (Explosion explosion in componentsInChildren)
			{
				explosion.sourceWeapon = this.sourceWeapon ?? explosion.sourceWeapon;
				if (explosion.damage > 0 && this.addedDamage > 0f)
				{
					explosion.playerDamageOverride = explosion.damage;
					explosion.damage += Mathf.RoundToInt(this.addedDamage * 20f);
				}
			}
			if ((this.beamType == BeamType.MaliciousFace || (this.beamType == BeamType.Railgun && this.maliciousIgnorePlayer)) && componentsInChildren.Length != 0)
			{
				int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
				if (this.beamType == BeamType.MaliciousFace)
				{
					foreach (Explosion explosion2 in componentsInChildren)
					{
						if (this.deflected || this.maliciousIgnorePlayer)
						{
							explosion2.unblockable = true;
							explosion2.canHit = AffectedSubjects.EnemiesOnly;
						}
						else
						{
							explosion2.enemy = true;
						}
						if (@int < 2)
						{
							explosion2.maxSize *= 0.65f;
							explosion2.speed *= 0.65f;
						}
					}
					return;
				}
				foreach (Explosion explosion3 in componentsInChildren)
				{
					explosion3.sourceWeapon = this.sourceWeapon ?? explosion3.sourceWeapon;
					explosion3.canHit = AffectedSubjects.EnemiesOnly;
				}
			}
		}
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x000A9E80 File Offset: 0x000A8080
	private void PiercingShotOrder()
	{
		this.hitList.Clear();
		foreach (RaycastHit raycastHit in this.allHits)
		{
			if (raycastHit.transform != this.previouslyHitTransform)
			{
				this.hitList.Add(new RaycastResult(raycastHit));
			}
		}
		bool flag = true;
		Transform transform = this.hit.transform;
		if (!this.didntHit)
		{
			GameObject gameObject = transform.gameObject;
			if (LayerMaskDefaults.IsMatchingLayer(gameObject.layer, LMD.Environment))
			{
				SandboxProp sandboxProp;
				if (gameObject.TryGetComponent<SandboxProp>(out sandboxProp) && this.hit.rigidbody != null)
				{
					this.hit.rigidbody.AddForceAtPosition(base.transform.forward * (float)this.bulletForce * 0.005f, this.hit.point, ForceMode.VelocityChange);
				}
				Breakable breakable;
				Bleeder bleeder;
				AttributeChecker attributeChecker;
				if (transform.TryGetComponent<Breakable>(out breakable) || gameObject.TryGetComponent<Bleeder>(out bleeder))
				{
					flag = true;
				}
				else if (transform.TryGetComponent<AttributeChecker>(out attributeChecker))
				{
					flag = true;
				}
			}
			if (flag || gameObject.CompareTag("Glass") || gameObject.CompareTag("GlassFloor") || gameObject.CompareTag("Armor"))
			{
				this.hitList.Add(new RaycastResult(this.hit));
			}
		}
		this.hitList.Sort();
		this.PiercingShotCheck();
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x000A9FE4 File Offset: 0x000A81E4
	private void PiercingShotCheck()
	{
		if (this.enemiesPierced >= this.hitList.Count)
		{
			this.enemiesPierced = 0;
			this.fadeOut = true;
			return;
		}
		RaycastResult raycastResult = this.hitList[this.enemiesPierced];
		RaycastHit rrhit = raycastResult.rrhit;
		Transform transform = raycastResult.transform;
		if (transform == null)
		{
			this.enemiesPierced++;
			this.PiercingShotCheck();
			return;
		}
		GameObject gameObject = transform.gameObject;
		if (gameObject.CompareTag("Armor") || (this.ricochetAmount > 0 && (LayerMaskDefaults.IsMatchingLayer(gameObject.layer, LMD.Environment) || gameObject.layer == 0)))
		{
			bool flag = !gameObject.CompareTag("Armor");
			GameObject gameObject2 = Object.Instantiate<GameObject>(base.gameObject, rrhit.point, base.transform.rotation);
			gameObject2.transform.forward = Vector3.Reflect(base.transform.forward, rrhit.normal);
			this.lr.SetPosition(1, rrhit.point);
			RevolverBeam component = gameObject2.GetComponent<RevolverBeam>();
			component.noMuzzleflash = true;
			component.alternateStartPoint = Vector3.zero;
			component.bodiesPierced = this.bodiesPierced;
			component.previouslyHitTransform = transform;
			component.aimAssist = true;
			component.intentionalRicochet = flag;
			if (flag)
			{
				this.ricochetAmount--;
				if (this.beamType != BeamType.Revolver || component.maxHitsPerTarget < 3 || (this.strongAlt && component.maxHitsPerTarget < 4))
				{
					component.maxHitsPerTarget++;
				}
				if (SceneHelper.IsStaticEnvironment(rrhit))
				{
					MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(rrhit, 3, 1f);
				}
				component.hitEids.Clear();
			}
			component.ricochetAmount = this.ricochetAmount;
			GameObject gameObject3 = Object.Instantiate<GameObject>(this.ricochetSound, rrhit.point, Quaternion.identity);
			gameObject3.SetActive(false);
			gameObject2.SetActive(false);
			MonoSingleton<DelayedActivationManager>.Instance.Add(gameObject2, 0.1f);
			MonoSingleton<DelayedActivationManager>.Instance.Add(gameObject3, 0.1f);
			Glass glass;
			if (gameObject.TryGetComponent<Glass>(out glass) && !glass.broken)
			{
				glass.Shatter();
			}
			Breakable breakable;
			if (gameObject.TryGetComponent<Breakable>(out breakable) && !breakable.specialCaseOnly && (this.strongAlt || breakable.weak || this.beamType == BeamType.Railgun))
			{
				breakable.Break();
			}
			this.fadeOut = true;
			this.enemiesPierced = this.hitList.Count;
			return;
		}
		if (gameObject.CompareTag("Coin") && this.bodiesPierced < this.hitAmount)
		{
			Coin coin;
			if (!gameObject.TryGetComponent<Coin>(out coin))
			{
				this.enemiesPierced++;
				this.PiercingShotCheck();
				return;
			}
			this.lr.SetPosition(1, transform.position);
			GameObject gameObject4 = Object.Instantiate<GameObject>(base.gameObject, rrhit.point, base.transform.rotation);
			gameObject4.SetActive(false);
			RevolverBeam component2 = gameObject4.GetComponent<RevolverBeam>();
			component2.bodiesPierced = 0;
			component2.noMuzzleflash = true;
			component2.alternateStartPoint = Vector3.zero;
			component2.hitEids.Clear();
			Revolver revolver;
			if (this.beamType == BeamType.Enemy)
			{
				coin.ignoreBlessedEnemies = true;
				component2.deflected = true;
			}
			else if (this.beamType == BeamType.Revolver && this.strongAlt && coin.hitTimes > 1 && this.sourceWeapon && this.sourceWeapon.TryGetComponent<Revolver>(out revolver) && revolver.altVersion)
			{
				revolver.InstaClick();
			}
			coin.DelayedReflectRevolver(rrhit.point, gameObject4);
			this.fadeOut = true;
			return;
		}
		else if ((gameObject.layer == 10 || gameObject.layer == 11) && this.bodiesPierced < this.hitAmount && !gameObject.CompareTag("Breakable"))
		{
			EnemyIdentifierIdentifier componentInParent = gameObject.GetComponentInParent<EnemyIdentifierIdentifier>();
			if (!componentInParent)
			{
				AttributeChecker attributeChecker;
				if (this.attributes.Length != 0 && transform.TryGetComponent<AttributeChecker>(out attributeChecker))
				{
					HitterAttribute[] array = this.attributes;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] == attributeChecker.targetAttribute)
						{
							attributeChecker.DelayedActivate(0.5f);
							break;
						}
					}
				}
				this.enemiesPierced++;
				this.currentHits = 0;
				this.PiercingShotCheck();
				return;
			}
			EnemyIdentifier eid = componentInParent.eid;
			if (!(eid != null))
			{
				this.ExecuteHits(rrhit);
				this.enemiesPierced++;
				this.PiercingShotCheck();
				return;
			}
			if ((this.hitEids.Contains(eid) && (!eid.dead || this.beamType != BeamType.Revolver || this.enemiesPierced != this.hitList.Count - 1)) || ((this.beamType == BeamType.Enemy || this.beamType == BeamType.MaliciousFace) && !this.deflected && (eid.enemyType == this.ignoreEnemyType || eid.immuneToFriendlyFire || EnemyIdentifier.CheckHurtException(this.ignoreEnemyType, eid.enemyType, this.target))))
			{
				this.enemiesPierced++;
				this.currentHits = 0;
				this.PiercingShotCheck();
				return;
			}
			bool dead = eid.dead;
			this.ExecuteHits(rrhit);
			if (!dead || gameObject.layer == 11 || (this.beamType == BeamType.Revolver && this.enemiesPierced == this.hitList.Count - 1))
			{
				this.currentHits++;
				this.bodiesPierced++;
				Object.Instantiate<GameObject>(this.hitParticle, rrhit.point, base.transform.rotation);
				MonoSingleton<TimeController>.Instance.HitStop(0.05f);
			}
			else
			{
				if (this.beamType == BeamType.Revolver)
				{
					this.hitEids.Add(eid);
				}
				this.enemiesPierced++;
				this.currentHits = 0;
			}
			if (this.currentHits >= this.maxHitsPerTarget)
			{
				this.hitEids.Add(eid);
				this.currentHits = 0;
				this.enemiesPierced++;
			}
			if (this.beamType == BeamType.Revolver && !dead)
			{
				base.Invoke("PiercingShotCheck", 0.05f);
				return;
			}
			if (this.beamType == BeamType.Revolver)
			{
				this.PiercingShotCheck();
				return;
			}
			if (!dead)
			{
				base.Invoke("PiercingShotCheck", 0.025f);
				return;
			}
			base.Invoke("PiercingShotCheck", 0.01f);
			return;
		}
		else
		{
			if (this.canHitProjectiles && gameObject.layer == 14)
			{
				if (!this.hasHitProjectile)
				{
					base.Invoke("PiercingShotCheck", 0.01f);
				}
				else
				{
					MonoSingleton<TimeController>.Instance.HitStop(0.05f);
					base.Invoke("PiercingShotCheck", 0.05f);
				}
				this.ExecuteHits(rrhit);
				this.enemiesPierced++;
				return;
			}
			if (gameObject.CompareTag("Glass") || gameObject.CompareTag("GlassFloor"))
			{
				Glass glass2;
				gameObject.TryGetComponent<Glass>(out glass2);
				if (!glass2.broken)
				{
					glass2.Shatter();
				}
				this.enemiesPierced++;
				this.PiercingShotCheck();
				return;
			}
			if (this.beamType == BeamType.Enemy && this.bodiesPierced < this.hitAmount && !rrhit.collider.isTrigger && gameObject.CompareTag("Player"))
			{
				this.ExecuteHits(rrhit);
				this.bodiesPierced++;
				this.enemiesPierced++;
				this.PiercingShotCheck();
				return;
			}
			Breakable breakable2;
			if (transform.TryGetComponent<Breakable>(out breakable2) && !breakable2.specialCaseOnly && (this.beamType == BeamType.Railgun || breakable2.weak))
			{
				if (breakable2.interrupt)
				{
					MonoSingleton<StyleHUD>.Instance.AddPoints(100, "ultrakill.interruption", this.sourceWeapon, null, -1, "", "");
					MonoSingleton<TimeController>.Instance.ParryFlash();
					if (this.canHitProjectiles)
					{
						breakable2.breakParticle = MonoSingleton<DefaultReferenceManager>.Instance.superExplosion;
					}
					if (breakable2.interruptEnemy && !breakable2.interruptEnemy.blessed)
					{
						breakable2.interruptEnemy.Explode(true);
					}
				}
				breakable2.Break();
			}
			else if (this.bodiesPierced < this.hitAmount)
			{
				this.ExecuteHits(rrhit);
			}
			Object.Instantiate<GameObject>(this.hitParticle, rrhit.point, Quaternion.LookRotation(rrhit.normal));
			this.enemiesPierced++;
			this.PiercingShotCheck();
			return;
		}
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x000AA83C File Offset: 0x000A8A3C
	public void ExecuteHits(RaycastHit currentHit)
	{
		Transform transform = currentHit.transform;
		if (transform != null)
		{
			GameObject gameObject = transform.gameObject;
			Breakable breakable;
			if (transform.TryGetComponent<Breakable>(out breakable) && !breakable.specialCaseOnly && (this.strongAlt || this.beamType == BeamType.Railgun || breakable.weak))
			{
				if (breakable.interrupt)
				{
					MonoSingleton<StyleHUD>.Instance.AddPoints(100, "ultrakill.interruption", this.sourceWeapon, null, -1, "", "");
					MonoSingleton<TimeController>.Instance.ParryFlash();
					if (this.canHitProjectiles)
					{
						breakable.breakParticle = MonoSingleton<DefaultReferenceManager>.Instance.superExplosion;
					}
					if (breakable.interruptEnemy && !breakable.interruptEnemy.blessed)
					{
						breakable.interruptEnemy.Explode(true);
					}
				}
				breakable.Break();
			}
			if (SceneHelper.IsStaticEnvironment(currentHit))
			{
				MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(currentHit, Mathf.RoundToInt(3f * this.damage), this.damage);
			}
			Glass glass;
			if (gameObject.TryGetComponent<Glass>(out glass) && !glass.broken && this.beamType == BeamType.Enemy)
			{
				glass.Shatter();
			}
			Projectile projectile;
			if (this.canHitProjectiles && gameObject.layer == 14 && gameObject.TryGetComponent<Projectile>(out projectile) && (projectile.speed != 0f || projectile.turnSpeed != 0f || projectile.decorative))
			{
				Object.Instantiate<GameObject>((!this.hasHitProjectile) ? MonoSingleton<DefaultReferenceManager>.Instance.superExplosion : projectile.explosionEffect, projectile.transform.position, Quaternion.identity);
				Object.Destroy(projectile.gameObject);
				if (!this.hasHitProjectile)
				{
					MonoSingleton<TimeController>.Instance.ParryFlash();
				}
				this.hasHitProjectile = true;
			}
			Bleeder bleeder;
			if (gameObject.TryGetComponent<Bleeder>(out bleeder))
			{
				if (this.beamType == BeamType.Railgun || this.strongAlt)
				{
					bleeder.GetHit(currentHit.point, GoreType.Head, false);
				}
				else
				{
					bleeder.GetHit(currentHit.point, GoreType.Body, false);
				}
			}
			SandboxProp sandboxProp;
			if (gameObject.TryGetComponent<SandboxProp>(out sandboxProp) && currentHit.rigidbody != null)
			{
				currentHit.rigidbody.AddForceAtPosition(base.transform.forward * (float)this.bulletForce * 0.005f, this.hit.point, ForceMode.VelocityChange);
			}
			Coin coin;
			if (transform.TryGetComponent<Coin>(out coin) && this.beamType == BeamType.Revolver)
			{
				if (this.quickDraw)
				{
					coin.quickDraw = true;
				}
				coin.DelayedReflectRevolver(currentHit.point, null);
			}
			if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Body") || gameObject.CompareTag("Limb") || gameObject.CompareTag("EndLimb") || gameObject.CompareTag("Head"))
			{
				EnemyIdentifier eid = transform.GetComponentInParent<EnemyIdentifierIdentifier>().eid;
				if (eid && !this.deflected && (this.beamType == BeamType.MaliciousFace || this.beamType == BeamType.Enemy) && (eid.enemyType == this.ignoreEnemyType || eid.immuneToFriendlyFire || EnemyIdentifier.CheckHurtException(this.ignoreEnemyType, eid.enemyType, this.target)))
				{
					this.enemiesPierced++;
					return;
				}
				if (this.beamType != BeamType.Enemy)
				{
					if (this.hitAmount > 1)
					{
						this.cc.CameraShake(1f * this.screenshakeMultiplier);
					}
					else
					{
						this.cc.CameraShake(0.5f * this.screenshakeMultiplier);
					}
				}
				if (eid && !eid.dead && this.quickDraw && !eid.blessed && !eid.puppet)
				{
					MonoSingleton<StyleHUD>.Instance.AddPoints(50, "ultrakill.quickdraw", this.sourceWeapon, eid, -1, "", "");
					this.quickDraw = false;
				}
				string text = "";
				if (this.beamType == BeamType.Revolver)
				{
					text = "revolver";
				}
				else if (this.beamType == BeamType.Railgun)
				{
					text = "railcannon";
				}
				else if (this.beamType == BeamType.MaliciousFace || this.beamType == BeamType.Enemy)
				{
					text = "enemy";
				}
				if (eid)
				{
					eid.hitter = text;
					if (this.attributes != null && this.attributes.Length != 0)
					{
						foreach (HitterAttribute hitterAttribute in this.attributes)
						{
							eid.hitterAttributes.Add(hitterAttribute);
						}
					}
					if (!eid.hitterWeapons.Contains(text + this.gunVariation.ToString()))
					{
						eid.hitterWeapons.Add(text + this.gunVariation.ToString());
					}
				}
				float num = 1f;
				if (this.beamType != BeamType.Revolver)
				{
					num = 0f;
				}
				if (this.critDamageOverride != 0f || this.strongAlt)
				{
					num = this.critDamageOverride;
				}
				float num2 = ((this.enemyDamageOverride != 0f) ? this.enemyDamageOverride : this.damage);
				if (eid && this.deflected)
				{
					if (this.beamType == BeamType.MaliciousFace && eid.enemyType == EnemyType.MaliciousFace)
					{
						num2 = 999f;
					}
					else if (this.beamType == BeamType.Enemy)
					{
						num2 *= 2.5f;
					}
					if (!this.chargeBacked)
					{
						this.chargeBacked = true;
						if (!eid.blessed)
						{
							MonoSingleton<StyleHUD>.Instance.AddPoints(400, "ultrakill.chargeback", this.sourceWeapon, eid, -1, "", "");
						}
					}
				}
				bool flag = false;
				if (this.strongAlt)
				{
					flag = true;
				}
				if (eid)
				{
					eid.DeliverDamage(gameObject, (transform.position - base.transform.position).normalized * (float)this.bulletForce, currentHit.point, num2, flag, num, this.sourceWeapon, false, false);
				}
				if (this.beamType != BeamType.MaliciousFace && this.beamType != BeamType.Enemy)
				{
					if (eid && !eid.dead && this.beamType == BeamType.Revolver && !eid.blessed && gameObject.CompareTag("Head"))
					{
						this.gc.headshots++;
						this.gc.headShotComboTime = 3f;
					}
					else if (this.beamType == BeamType.Railgun || !gameObject.CompareTag("Head"))
					{
						this.gc.headshots = 0;
						this.gc.headShotComboTime = 0f;
					}
					if (this.gc.headshots > 1 && eid && !eid.blessed)
					{
						StyleHUD instance = MonoSingleton<StyleHUD>.Instance;
						int num3 = this.gc.headshots * 20;
						string text2 = "ultrakill.headshotcombo";
						int i = this.gc.headshots;
						instance.AddPoints(num3, text2, this.sourceWeapon, eid, i, "", "");
					}
				}
				if (this.enemyHitSound)
				{
					Object.Instantiate<GameObject>(this.enemyHitSound, currentHit.point, Quaternion.identity);
					return;
				}
			}
			else if (gameObject.layer == 10)
			{
				Grenade componentInParent = transform.GetComponentInParent<Grenade>();
				if (componentInParent != null)
				{
					if (this.beamType != BeamType.Enemy || !componentInParent.enemy || componentInParent.playerRiding)
					{
						MonoSingleton<TimeController>.Instance.ParryFlash();
					}
					if ((this.beamType == BeamType.Railgun && this.hitAmount == 1) || this.beamType == BeamType.MaliciousFace)
					{
						this.maliciousIgnorePlayer = true;
						componentInParent.Explode(componentInParent.rocket, false, !componentInParent.rocket, 2f, true, this.sourceWeapon, false);
						return;
					}
					componentInParent.Explode(componentInParent.rocket, false, !componentInParent.rocket, 1f, false, this.sourceWeapon, false);
					return;
				}
				else
				{
					Cannonball componentInParent2 = transform.GetComponentInParent<Cannonball>();
					if (componentInParent2)
					{
						MonoSingleton<TimeController>.Instance.ParryFlash();
						componentInParent2.Explode();
						return;
					}
				}
			}
			else if (this.beamType == BeamType.Enemy && !currentHit.collider.isTrigger && gameObject.CompareTag("Player"))
			{
				if (this.enemyHitSound)
				{
					Object.Instantiate<GameObject>(this.enemyHitSound, currentHit.point, Quaternion.identity);
				}
				if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
				{
					MonoSingleton<NewMovement>.Instance.GetHurt(Mathf.RoundToInt(this.damage * 10f), true, 1f, false, false, 0.35f, false);
					return;
				}
				MonoSingleton<PlatformerMovement>.Instance.Explode(false);
				return;
			}
			else
			{
				if (this.gc)
				{
					this.gc.headshots = 0;
					this.gc.headShotComboTime = 0f;
				}
				if (gameObject.CompareTag("Armor"))
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(base.gameObject, currentHit.point, base.transform.rotation);
					gameObject2.transform.forward = Vector3.Reflect(base.transform.forward, currentHit.normal);
					RevolverBeam component = gameObject2.GetComponent<RevolverBeam>();
					component.noMuzzleflash = true;
					component.alternateStartPoint = Vector3.zero;
					component.aimAssist = true;
					GameObject gameObject3 = Object.Instantiate<GameObject>(this.ricochetSound, currentHit.point, Quaternion.identity);
					gameObject3.SetActive(false);
					gameObject2.SetActive(false);
					MonoSingleton<DelayedActivationManager>.Instance.Add(gameObject2, 0.1f);
					MonoSingleton<DelayedActivationManager>.Instance.Add(gameObject3, 0.1f);
				}
			}
		}
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x000AB180 File Offset: 0x000A9380
	private void RicochetAimAssist(GameObject beam, bool aimAtHead = false)
	{
		RaycastHit[] array = Physics.SphereCastAll(beam.transform.position, 5f, beam.transform.forward, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Enemies));
		if (array == null || array.Length == 0)
		{
			return;
		}
		Vector3 vector = beam.transform.forward * 1000f;
		float num = float.PositiveInfinity;
		GameObject gameObject = null;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			Coin coin;
			bool flag2 = MonoSingleton<CoinList>.Instance.revolverCoinsList.Count > 0 && array[i].transform.TryGetComponent<Coin>(out coin) && (!coin.shot || coin.shotByEnemy);
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if ((!flag || flag2) && (array[i].distance <= num || (!flag && flag2)) && (array[i].distance >= 0.1f || flag2) && !Physics.Raycast(beam.transform.position, array[i].point - beam.transform.position, array[i].distance, LayerMaskDefaults.Get(LMD.Environment)) && (flag2 || (array[i].transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && !enemyIdentifierIdentifier.eid.dead)))
			{
				if (flag2)
				{
					flag = true;
				}
				vector = (flag2 ? array[i].transform.position : array[i].point);
				num = array[i].distance;
				gameObject = array[i].transform.gameObject;
			}
		}
		if (gameObject)
		{
			EnemyIdentifierIdentifier enemyIdentifierIdentifier2;
			if (aimAtHead && !flag && (this.critDamageOverride != 0f || (this.beamType == BeamType.Revolver && !this.strongAlt)) && gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier2) && enemyIdentifierIdentifier2.eid && enemyIdentifierIdentifier2.eid.weakPoint && !Physics.Raycast(beam.transform.position, enemyIdentifierIdentifier2.eid.weakPoint.transform.position - beam.transform.position, Vector3.Distance(enemyIdentifierIdentifier2.eid.weakPoint.transform.position, beam.transform.position), LayerMaskDefaults.Get(LMD.Environment)))
			{
				vector = enemyIdentifierIdentifier2.eid.weakPoint.transform.position;
			}
			beam.transform.LookAt(vector);
		}
	}

	// Token: 0x04001CE3 RID: 7395
	private const float ForceBulletPropMulti = 0.005f;

	// Token: 0x04001CE4 RID: 7396
	public EnemyTarget target;

	// Token: 0x04001CE5 RID: 7397
	public BeamType beamType;

	// Token: 0x04001CE6 RID: 7398
	public HitterAttribute[] attributes;

	// Token: 0x04001CE7 RID: 7399
	private LineRenderer lr;

	// Token: 0x04001CE8 RID: 7400
	private AudioSource aud;

	// Token: 0x04001CE9 RID: 7401
	private Light muzzleLight;

	// Token: 0x04001CEA RID: 7402
	public Vector3 alternateStartPoint;

	// Token: 0x04001CEB RID: 7403
	public GameObject sourceWeapon;

	// Token: 0x04001CEC RID: 7404
	[HideInInspector]
	public int bodiesPierced;

	// Token: 0x04001CED RID: 7405
	private int enemiesPierced;

	// Token: 0x04001CEE RID: 7406
	private RaycastHit[] allHits;

	// Token: 0x04001CEF RID: 7407
	[HideInInspector]
	public List<RaycastResult> hitList = new List<RaycastResult>();

	// Token: 0x04001CF0 RID: 7408
	private GunControl gc;

	// Token: 0x04001CF1 RID: 7409
	private RaycastHit hit;

	// Token: 0x04001CF2 RID: 7410
	private Vector3 shotHitPoint;

	// Token: 0x04001CF3 RID: 7411
	public CameraController cc;

	// Token: 0x04001CF4 RID: 7412
	private bool maliciousIgnorePlayer;

	// Token: 0x04001CF5 RID: 7413
	public GameObject hitParticle;

	// Token: 0x04001CF6 RID: 7414
	public int bulletForce;

	// Token: 0x04001CF7 RID: 7415
	public bool quickDraw;

	// Token: 0x04001CF8 RID: 7416
	public int gunVariation;

	// Token: 0x04001CF9 RID: 7417
	public float damage;

	// Token: 0x04001CFA RID: 7418
	[HideInInspector]
	public float addedDamage;

	// Token: 0x04001CFB RID: 7419
	public float enemyDamageOverride;

	// Token: 0x04001CFC RID: 7420
	public float critDamageOverride;

	// Token: 0x04001CFD RID: 7421
	public float screenshakeMultiplier = 1f;

	// Token: 0x04001CFE RID: 7422
	public int hitAmount;

	// Token: 0x04001CFF RID: 7423
	public int maxHitsPerTarget;

	// Token: 0x04001D00 RID: 7424
	private int currentHits;

	// Token: 0x04001D01 RID: 7425
	public bool noMuzzleflash;

	// Token: 0x04001D02 RID: 7426
	private bool fadeOut;

	// Token: 0x04001D03 RID: 7427
	private bool didntHit;

	// Token: 0x04001D04 RID: 7428
	private LayerMask ignoreEnemyTrigger;

	// Token: 0x04001D05 RID: 7429
	private LayerMask enemyLayerMask;

	// Token: 0x04001D06 RID: 7430
	private LayerMask pierceLayerMask;

	// Token: 0x04001D07 RID: 7431
	public int ricochetAmount;

	// Token: 0x04001D08 RID: 7432
	[HideInInspector]
	public bool hasBeenRicocheter;

	// Token: 0x04001D09 RID: 7433
	public GameObject ricochetSound;

	// Token: 0x04001D0A RID: 7434
	public GameObject enemyHitSound;

	// Token: 0x04001D0B RID: 7435
	public bool fake;

	// Token: 0x04001D0C RID: 7436
	public EnemyType ignoreEnemyType;

	// Token: 0x04001D0D RID: 7437
	public bool deflected;

	// Token: 0x04001D0E RID: 7438
	private bool chargeBacked;

	// Token: 0x04001D0F RID: 7439
	public bool strongAlt;

	// Token: 0x04001D10 RID: 7440
	public bool ultraRicocheter = true;

	// Token: 0x04001D11 RID: 7441
	public bool canHitProjectiles;

	// Token: 0x04001D12 RID: 7442
	private bool hasHitProjectile;

	// Token: 0x04001D13 RID: 7443
	[HideInInspector]
	public List<EnemyIdentifier> hitEids = new List<EnemyIdentifier>();

	// Token: 0x04001D14 RID: 7444
	[HideInInspector]
	public Transform previouslyHitTransform;

	// Token: 0x04001D15 RID: 7445
	[HideInInspector]
	public bool aimAssist;

	// Token: 0x04001D16 RID: 7446
	[HideInInspector]
	public bool intentionalRicochet;
}
