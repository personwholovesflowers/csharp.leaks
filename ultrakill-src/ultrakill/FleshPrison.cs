using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F6 RID: 502
public class FleshPrison : MonoBehaviour
{
	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00046F21 File Offset: 0x00045121
	private float maxDroneCooldown
	{
		get
		{
			return (float)(this.started ? ((this.difficulty == 2) ? 25 : 30) : 3);
		}
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x00046F40 File Offset: 0x00045140
	private void Awake()
	{
		if (!this.mainSimplifier)
		{
			this.mainSimplifier = base.GetComponentInChildren<EnemySimplifier>();
		}
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.stat = base.GetComponent<Statue>();
		this.aud = base.GetComponent<AudioSource>();
		this.anim = base.GetComponentInChildren<Animator>();
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x00046F98 File Offset: 0x00045198
	private void Start()
	{
		if (this.mainSimplifier && !this.eid.puppet)
		{
			this.defaultTexture = this.mainSimplifier.originalMaterial.mainTexture;
		}
		this.maxHealth = this.stat.health;
		this.health = this.stat.health;
		this.origPos = this.rotationBone.localPosition;
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		this.col = this.rotationBone.GetComponentInChildren<EnemyIdentifierIdentifier>().GetComponent<Collider>();
		this.bossHealth = base.GetComponent<BossHealthBar>();
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00047064 File Offset: 0x00045264
	private void Update()
	{
		if (this.eid.puppet)
		{
			this.mainSimplifier;
		}
		float num = Mathf.Abs(this.rotationSpeed);
		if (num < 45f)
		{
			num = 45f;
		}
		if (this.rotationSpeed != this.rotationSpeedTarget)
		{
			this.rotationSpeed = Mathf.MoveTowards(this.rotationSpeed, this.rotationSpeedTarget, Time.deltaTime * (num / 2f + 5f));
		}
		this.rotationBone.Rotate(Vector3.forward, Time.deltaTime * this.rotationSpeed * this.eid.totalSpeedModifier, Space.Self);
		if (this.eid.target == null)
		{
			return;
		}
		if (this.health > this.stat.health)
		{
			float num2 = this.health - this.stat.health;
			if (this.currentDrones.Count > 0)
			{
				for (int i = this.currentDrones.Count - 1; i >= 0; i--)
				{
					if (this.currentDrones[i] == null)
					{
						this.currentDrones.RemoveAt(i);
					}
				}
			}
			if (this.currentDrones.Count > 8)
			{
				this.fleshDroneCooldown -= num2 * 1.5f;
			}
			else if (this.currentDrones.Count > 5)
			{
				this.fleshDroneCooldown -= num2 / 2.5f;
			}
			else if (this.currentDrones.Count > 0)
			{
				this.fleshDroneCooldown -= num2 / 5f;
			}
			else
			{
				this.fleshDroneCooldown -= num2 / 7.5f;
			}
			this.health = this.stat.health;
		}
		else if (this.health < this.stat.health)
		{
			this.health = this.stat.health;
		}
		if (this.bossHealth == null)
		{
			this.bossHealth = base.GetComponent<BossHealthBar>();
		}
		if (this.bossHealth)
		{
			if (!this.healing)
			{
				this.secondaryBarValue = Mathf.MoveTowards(this.secondaryBarValue, this.fleshDroneCooldown / this.maxDroneCooldown, (Mathf.Abs(this.secondaryBarValue - this.fleshDroneCooldown / this.maxDroneCooldown) + 1f) * Time.deltaTime);
			}
			this.bossHealth.UpdateSecondaryBar(this.secondaryBarValue);
		}
		if (this.anim && !this.noDrones)
		{
			this.anim.speed = (float)(this.inAction ? 5 : 1);
		}
		if (!this.inAction)
		{
			if (this.health > this.stat.health)
			{
				this.idleTimer = 0.15f;
				if (!this.eid.puppet)
				{
					this.mainSimplifier.ChangeTexture(EnemySimplifier.MaterialState.normal, this.hurtTexture);
				}
				this.hurting = true;
			}
			else
			{
				this.idleTimer = Mathf.MoveTowards(this.idleTimer, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
				if (this.hurting)
				{
					if (this.idleTimer > 0f)
					{
						this.rotationBone.transform.localPosition = new Vector3(this.origPos.x + Random.Range(-this.idleTimer, this.idleTimer), this.origPos.y, this.origPos.z + Random.Range(-this.idleTimer, this.idleTimer));
					}
					else
					{
						this.rotationBone.transform.localPosition = this.origPos;
						this.hurting = false;
					}
				}
				if (this.idleTimer == 0f && !this.eid.puppet)
				{
					if (this.currentIdleTexture == this.defaultTexture)
					{
						this.idleTimer = 0.25f;
						this.mainSimplifier.ChangeTexture(EnemySimplifier.MaterialState.normal, this.idleTextures[Random.Range(0, this.idleTextures.Length)]);
					}
					else
					{
						this.idleTimer = Random.Range(0.5f, 1f);
						this.mainSimplifier.ChangeTexture(EnemySimplifier.MaterialState.normal, this.defaultTexture);
					}
					this.mainSimplifier.ChangeTexture(EnemySimplifier.MaterialState.normal, this.defaultTexture);
				}
				if (this.fleshDroneCooldown <= 0f)
				{
					this.started = true;
					for (int j = this.currentDrones.Count - 1; j >= 0; j--)
					{
						if (this.currentDrones[j] == null)
						{
							this.currentDrones.RemoveAt(j);
						}
					}
					if (!this.eid.puppet)
					{
						this.mainSimplifier.ChangeTexture(EnemySimplifier.MaterialState.normal, this.attackTexture);
					}
					this.idleTimer = 0f;
					this.fleshDroneCooldown = this.maxDroneCooldown;
					this.attackCooldown = 3f;
					this.inAction = true;
					this.rotationSpeed = 0f;
					this.rotationSpeedTarget = 0f;
					if (this.stat.health > this.maxHealth / 2f)
					{
						this.droneAmount = 10;
					}
					else
					{
						this.droneAmount = 12;
					}
					if (this.difficulty == 1)
					{
						this.droneAmount = Mathf.RoundToInt((float)this.droneAmount / 1.5f);
					}
					else if (this.difficulty == 0)
					{
						this.droneAmount /= 2;
					}
					if (this.altVersion)
					{
						this.droneAmount /= 2;
					}
					if (this.droneAmount < 3)
					{
						this.droneAmount = 3;
					}
					if (this.timesHealed == 1)
					{
						UltrakillEvent ultrakillEvent = this.onFirstHeal;
						if (ultrakillEvent != null)
						{
							ultrakillEvent.Invoke("");
						}
					}
					this.timesHealed++;
					if (this.currentDrones.Count <= 0)
					{
						this.healing = true;
						this.secondaryBarValue = 0f;
						base.Invoke("SpawnFleshDrones", 1f / this.eid.totalSpeedModifier);
					}
					else
					{
						this.StartHealing();
					}
					this.shakingCamera = true;
					this.aud.Play();
				}
				else if (this.fleshDroneCooldown > 3f)
				{
					if (this.attackCooldown > 0f)
					{
						float num3 = 1f;
						if (this.difficulty == 1)
						{
							num3 = 0.9f;
						}
						else if (this.difficulty == 0)
						{
							num3 = 0.75f;
						}
						this.attackCooldown = Mathf.MoveTowards(this.attackCooldown, 0f, Time.deltaTime * num3 * this.eid.totalSpeedModifier);
					}
					else
					{
						int num4 = 2;
						if (!this.currentBlackHole && this.difficulty > 0)
						{
							num4 = 3;
						}
						int num5 = Random.Range(0, num4);
						if (num5 == this.previousAttack)
						{
							num5++;
						}
						if (num5 >= num4)
						{
							num5 = 0;
						}
						this.inAction = true;
						Color white = Color.white;
						float num6 = 1f / this.eid.totalSpeedModifier;
						switch (num5)
						{
						case 0:
							base.Invoke("SpawnInsignia", num6);
							this.attackCooldown = 4f;
							break;
						case 1:
							base.Invoke("HomingProjectileAttack", num6);
							if (this.altVersion)
							{
								white = new Color(1f, 0.75f, 0f);
							}
							else
							{
								white = new Color(0f, 1f, 0.9f);
							}
							this.attackCooldown = 1f;
							break;
						case 2:
							base.Invoke("SpawnBlackHole", num6);
							white = new Color(1f, 0f, 1f);
							this.attackCooldown = 2f;
							break;
						}
						GameObject gameObject = Object.Instantiate<GameObject>(this.attackWindUp, this.rotationBone.position, Quaternion.LookRotation(this.eid.target.position - this.rotationBone.position));
						Light light;
						if (gameObject.TryGetComponent<Light>(out light))
						{
							light.color = white;
						}
						SpriteRenderer spriteRenderer;
						if (gameObject.TryGetComponent<SpriteRenderer>(out spriteRenderer))
						{
							spriteRenderer.color = white;
						}
						this.previousAttack = num5;
					}
				}
			}
		}
		else
		{
			if (!this.eid.puppet)
			{
				this.mainSimplifier.ChangeTexture(EnemySimplifier.MaterialState.normal, this.attackTexture);
			}
			this.idleTimer = 0f;
			if (this.currentProjectile < this.projectileAmount)
			{
				this.homingProjectileCooldown = Mathf.MoveTowards(this.homingProjectileCooldown, 0f, Time.deltaTime * (Mathf.Abs(this.rotationSpeed) / 10f) * this.eid.totalSpeedModifier);
				if (this.homingProjectileCooldown <= 0f)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.homingProjectile, this.rotationBone.position + this.rotationBone.up * 8f, this.rotationBone.rotation);
					Projectile component = gameObject2.GetComponent<Projectile>();
					component.target = this.eid.target;
					component.safeEnemyType = (this.altVersion ? EnemyType.FleshPanopticon : EnemyType.FleshPrison);
					if (this.difficulty >= 4)
					{
						component.turningSpeedMultiplier = 0.66f;
					}
					else if (this.difficulty >= 2)
					{
						component.turningSpeedMultiplier = 0.5f;
					}
					else if (this.difficulty == 1)
					{
						component.turningSpeedMultiplier = 0.45f;
					}
					else
					{
						component.turningSpeedMultiplier = 0.4f;
					}
					if (this.altVersion)
					{
						component.turnSpeed *= 4f;
						component.turningSpeedMultiplier *= 4f;
						component.predictiveHomingMultiplier = 1.25f;
					}
					component.damage *= this.eid.totalDamageModifier;
					this.homingProjectileCooldown = 1f;
					this.currentProjectile++;
					gameObject2.transform.SetParent(base.transform, true);
					Rigidbody rigidbody;
					if (this.altVersion && gameObject2.TryGetComponent<Rigidbody>(out rigidbody))
					{
						rigidbody.AddForce(Vector3.up * 50f, ForceMode.VelocityChange);
					}
				}
				if (this.currentProjectile >= this.projectileAmount)
				{
					this.inAction = false;
					Animator animator = this.anim;
					if (animator != null)
					{
						animator.SetBool("Shooting", false);
					}
					if (this.rotationSpeed >= 0f)
					{
						this.rotationSpeedTarget = 45f;
					}
					else
					{
						this.rotationSpeedTarget = -45f;
					}
					if (this.fleshDroneCooldown < 1f)
					{
						this.fleshDroneCooldown = 1f;
					}
				}
			}
		}
		if (this.fleshDroneCooldown > 0f)
		{
			this.fleshDroneCooldown = Mathf.MoveTowards(this.fleshDroneCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		if (this.shakingCamera)
		{
			MonoSingleton<CameraController>.Instance.CameraShake(0.25f);
		}
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00047AE0 File Offset: 0x00045CE0
	private void SpawnFleshDrones()
	{
		if (this.eid.target == null)
		{
			return;
		}
		float num = 360f / (float)this.droneAmount;
		if (this.currentDrone == 0)
		{
			this.targeter = new GameObject("Targeter");
			this.targeter.transform.position = this.rotationBone.position;
			Vector3 vector = (this.altVersion ? Vector3.up : (new Vector3(this.eid.target.position.x, this.targeter.transform.position.y, this.eid.target.position.z) - this.targeter.transform.position));
			Quaternion quaternion = (this.altVersion ? Quaternion.LookRotation(vector.normalized) : Quaternion.LookRotation(vector.normalized, Vector3.up));
			this.targeter.transform.rotation = quaternion;
			this.targeter.transform.Rotate(Vector3.forward * num / 2f);
		}
		if (this.currentDrone < this.droneAmount)
		{
			this.secondaryBarValue = (float)this.currentDrone / (float)this.droneAmount;
			GameObject gameObject;
			if ((this.difficulty == 3 && this.currentDrone % 5 == 0) || (this.difficulty == 4 && this.currentDrone % 3 == 0) || this.difficulty == 5)
			{
				gameObject = Object.Instantiate<GameObject>(this.skullDrone, this.targeter.transform.position + this.targeter.transform.up * (float)(this.altVersion ? 50 : 20), this.targeter.transform.rotation);
			}
			else
			{
				gameObject = Object.Instantiate<GameObject>(this.fleshDrone, this.targeter.transform.position + this.targeter.transform.up * (float)(this.altVersion ? 50 : 20), this.targeter.transform.rotation);
			}
			gameObject.transform.SetParent(base.transform, true);
			EnemyIdentifier enemyIdentifier;
			if (gameObject.TryGetComponent<EnemyIdentifier>(out enemyIdentifier))
			{
				enemyIdentifier.dontCountAsKills = true;
				enemyIdentifier.damageBuff = this.eid.damageBuff;
				enemyIdentifier.healthBuff = this.eid.healthBuff;
				enemyIdentifier.speedBuff = this.eid.speedBuff;
			}
			this.targeter.transform.Rotate(Vector3.forward * num);
			DroneFlesh droneFlesh;
			if (gameObject.TryGetComponent<DroneFlesh>(out droneFlesh))
			{
				this.currentDrones.Add(droneFlesh);
			}
			this.currentDrone++;
			base.Invoke("SpawnFleshDrones", 0.1f / this.eid.totalSpeedModifier);
			return;
		}
		this.inAction = false;
		if (Random.Range(0, 2) == 0)
		{
			this.rotationSpeedTarget = 45f;
		}
		else
		{
			this.rotationSpeedTarget = -45f;
		}
		this.aud.Stop();
		this.shakingCamera = false;
		this.currentDrone = 0;
		Object.Destroy(this.targeter);
		this.fleshDroneCooldown = (float)(this.altVersion ? 30 : 25);
		this.healing = false;
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x00047E28 File Offset: 0x00046028
	private void StartHealing()
	{
		this.healing = true;
		this.secondaryBarValue = 0f;
		for (int i = 0; i < this.currentDrones.Count; i++)
		{
			if (this.currentDrones[i] == null)
			{
				this.currentDrones.RemoveAt(i);
			}
			else
			{
				LineToPoint lineToPoint;
				if (Object.Instantiate<GameObject>(this.healingTargetEffect, this.currentDrones[i].transform).TryGetComponent<LineToPoint>(out lineToPoint))
				{
					lineToPoint.targets[1] = this.rotationBone;
				}
				Rigidbody rigidbody;
				if (this.currentDrones[i].TryGetComponent<Rigidbody>(out rigidbody))
				{
					rigidbody.isKinematic = true;
				}
			}
		}
		if (this.difficulty >= 1)
		{
			this.eid.totalDamageTakenMultiplier = 0.1f;
		}
		if (this.currentDrones.Count > 0)
		{
			base.Invoke("HealFromDrone", 5f / this.eid.totalSpeedModifier);
			return;
		}
		base.Invoke("SpawnFleshDrones", 1f / this.eid.totalSpeedModifier);
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x00047F34 File Offset: 0x00046134
	private void HealFromDrone()
	{
		if (this.stat.health <= 0f)
		{
			return;
		}
		if (this.currentDrones.Count <= 0)
		{
			this.eid.totalDamageTakenMultiplier = 1f;
			this.SpawnFleshDrones();
			return;
		}
		if (this.currentDrones[0] != null)
		{
			float num = 1f;
			if (this.difficulty == 1)
			{
				num = 0.75f;
			}
			else if (this.difficulty == 0)
			{
				num = 0.35f;
			}
			num /= this.eid.totalHealthModifier;
			if (this.altVersion)
			{
				num *= 2f;
			}
			if (!Physics.Raycast(this.rotationBone.position, this.currentDrones[0].transform.position - this.rotationBone.position, Vector3.Distance(this.rotationBone.position, this.currentDrones[0].transform.position), LayerMaskDefaults.Get(LMD.Environment)))
			{
				if (this.stat.health + 10f * num <= this.maxHealth)
				{
					this.stat.health += 10f * num;
				}
				else
				{
					this.stat.health = this.maxHealth;
				}
				this.eid.health = this.stat.health;
				Object.Instantiate<GameObject>(this.healingEffect, this.rotationBone);
			}
			this.currentDrones[0].Explode();
			this.currentDrones.RemoveAt(0);
			base.Invoke("HealFromDrone", 0.25f / this.eid.totalSpeedModifier);
			return;
		}
		this.currentDrones.RemoveAt(0);
		this.HealFromDrone();
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x000480FC File Offset: 0x000462FC
	private void HomingProjectileAttack()
	{
		this.inAction = true;
		if (!this.eid.puppet)
		{
			this.mainSimplifier.ChangeTexture(EnemySimplifier.MaterialState.normal, this.attackTexture);
		}
		this.idleTimer = 0f;
		this.homingProjectileCooldown = 1f;
		this.currentProjectile = 0;
		Animator animator = this.anim;
		if (animator != null)
		{
			animator.SetBool("Shooting", true);
		}
		if (Random.Range(0, 2) == 0)
		{
			this.rotationSpeedTarget = 360f;
		}
		else
		{
			this.rotationSpeedTarget = -360f;
		}
		if (this.altVersion)
		{
			this.rotationSpeedTarget /= 8f;
		}
		if ((this.rotationSpeedTarget > 0f && this.rotationSpeed < 0f) || (this.rotationSpeedTarget < 0f && this.rotationSpeed > 0f))
		{
			this.rotationSpeed = 0f;
		}
		if (this.stat.health > this.maxHealth / 2f)
		{
			if (this.difficulty >= 2)
			{
				this.projectileAmount = 50;
			}
			else
			{
				this.projectileAmount = 35;
			}
		}
		else if (this.difficulty >= 2)
		{
			this.projectileAmount = 75;
		}
		else
		{
			this.projectileAmount = 50;
		}
		if (this.altVersion)
		{
			this.projectileAmount /= 3;
		}
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x00048244 File Offset: 0x00046444
	private void SpawnInsignia()
	{
		if (this.eid.target == null)
		{
			return;
		}
		this.inAction = false;
		GameObject gameObject = Object.Instantiate<GameObject>(this.insignia, this.eid.target.position, Quaternion.identity);
		if (this.altVersion)
		{
			Vector3 velocity = this.eid.target.GetVelocity();
			velocity.y = 0f;
			if (velocity.magnitude > 0f)
			{
				gameObject.transform.LookAt(this.eid.target.position + velocity);
			}
			else
			{
				gameObject.transform.Rotate(Vector3.up * Random.Range(0f, 360f), Space.Self);
			}
			gameObject.transform.Rotate(Vector3.right * 90f, Space.Self);
		}
		VirtueInsignia virtueInsignia;
		if (gameObject.TryGetComponent<VirtueInsignia>(out virtueInsignia))
		{
			virtueInsignia.predictive = true;
			virtueInsignia.noTracking = true;
			virtueInsignia.otherParent = base.transform;
			if (this.stat.health > this.maxHealth / 2f)
			{
				virtueInsignia.charges = 2;
			}
			else
			{
				virtueInsignia.charges = 3;
			}
			if (this.difficulty >= 3)
			{
				virtueInsignia.charges += this.difficulty - 2;
			}
			virtueInsignia.windUpSpeedMultiplier = 0.5f;
			virtueInsignia.windUpSpeedMultiplier *= this.eid.totalSpeedModifier;
			virtueInsignia.damage = Mathf.RoundToInt((float)virtueInsignia.damage * this.eid.totalDamageModifier);
			virtueInsignia.target = this.eid.target;
			virtueInsignia.predictiveVersion = null;
			Light light = gameObject.AddComponent<Light>();
			light.range = 30f;
			light.intensity = 50f;
		}
		if (this.difficulty >= 2)
		{
			gameObject.transform.localScale = new Vector3(8f, 2f, 8f);
		}
		else if (this.difficulty == 1)
		{
			gameObject.transform.localScale = new Vector3(7f, 2f, 7f);
		}
		else
		{
			gameObject.transform.localScale = new Vector3(5f, 2f, 5f);
		}
		GoreZone componentInParent = base.GetComponentInParent<GoreZone>();
		if (componentInParent)
		{
			gameObject.transform.SetParent(componentInParent.transform, true);
		}
		else
		{
			gameObject.transform.SetParent(base.transform, true);
		}
		if (this.fleshDroneCooldown < 1f)
		{
			this.fleshDroneCooldown = 1f;
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x000484C4 File Offset: 0x000466C4
	private void SpawnBlackHole()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.blackHole, base.transform);
		gameObject.transform.position = this.rotationBone.position;
		this.currentBlackHole = gameObject.GetComponent<BlackHoleProjectile>();
		this.currentBlackHole.target = this.eid.target;
		if (this.currentBlackHole)
		{
			this.currentBlackHole.safeType = EnemyType.FleshPrison;
			this.currentBlackHole.Activate();
		}
		this.inAction = false;
		if (!this.eid.puppet)
		{
			this.mainSimplifier.ChangeTexture(EnemySimplifier.MaterialState.normal, this.attackTexture);
		}
		this.idleTimer = 0.5f;
		if (this.fleshDroneCooldown < 1f)
		{
			this.fleshDroneCooldown = 1f;
		}
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0004858C File Offset: 0x0004678C
	public void ForceDronesOff()
	{
		this.noDrones = true;
		base.CancelInvoke("HealFromDrone");
		base.CancelInvoke("SpawnFleshDrones");
		Animator animator = this.anim;
		if (animator != null)
		{
			animator.SetBool("Shooting", false);
		}
		if (this.currentDrones.Count > 0)
		{
			foreach (DroneFlesh droneFlesh in this.currentDrones)
			{
				droneFlesh.Explode();
			}
		}
		if (this.currentBlackHole)
		{
			this.currentBlackHole.Explode();
		}
		foreach (VirtueInsignia virtueInsignia in Object.FindObjectsOfType<VirtueInsignia>())
		{
			if (virtueInsignia.otherParent == base.transform)
			{
				Object.Destroy(virtueInsignia.gameObject);
			}
		}
		Projectile[] componentsInChildren = base.GetComponentsInChildren<Projectile>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i].gameObject);
		}
		if (this.anim)
		{
			this.anim.speed = 20f;
		}
	}

	// Token: 0x04000D64 RID: 3428
	public Transform rotationBone;

	// Token: 0x04000D65 RID: 3429
	private Collider col;

	// Token: 0x04000D66 RID: 3430
	private Animator anim;

	// Token: 0x04000D67 RID: 3431
	public bool altVersion;

	// Token: 0x04000D68 RID: 3432
	private Texture currentIdleTexture;

	// Token: 0x04000D69 RID: 3433
	private Texture defaultTexture;

	// Token: 0x04000D6A RID: 3434
	public Texture[] idleTextures;

	// Token: 0x04000D6B RID: 3435
	private float idleTimer = 0.5f;

	// Token: 0x04000D6C RID: 3436
	public Texture hurtTexture;

	// Token: 0x04000D6D RID: 3437
	public Texture attackTexture;

	// Token: 0x04000D6E RID: 3438
	[SerializeField]
	private EnemySimplifier mainSimplifier;

	// Token: 0x04000D6F RID: 3439
	private AudioSource aud;

	// Token: 0x04000D70 RID: 3440
	private BossHealthBar bossHealth;

	// Token: 0x04000D71 RID: 3441
	private float secondaryBarValue;

	// Token: 0x04000D72 RID: 3442
	private bool started;

	// Token: 0x04000D73 RID: 3443
	private bool inAction;

	// Token: 0x04000D74 RID: 3444
	private float health;

	// Token: 0x04000D75 RID: 3445
	private EnemyIdentifier eid;

	// Token: 0x04000D76 RID: 3446
	private Statue stat;

	// Token: 0x04000D77 RID: 3447
	private bool hurting;

	// Token: 0x04000D78 RID: 3448
	private bool shakingCamera;

	// Token: 0x04000D79 RID: 3449
	private Vector3 origPos;

	// Token: 0x04000D7A RID: 3450
	public GameObject fleshDrone;

	// Token: 0x04000D7B RID: 3451
	public GameObject skullDrone;

	// Token: 0x04000D7C RID: 3452
	private float fleshDroneCooldown = 3f;

	// Token: 0x04000D7D RID: 3453
	private int droneAmount = 10;

	// Token: 0x04000D7E RID: 3454
	private int currentDrone;

	// Token: 0x04000D7F RID: 3455
	private GameObject targeter;

	// Token: 0x04000D80 RID: 3456
	private bool healing;

	// Token: 0x04000D81 RID: 3457
	public List<DroneFlesh> currentDrones = new List<DroneFlesh>();

	// Token: 0x04000D82 RID: 3458
	public GameObject healingTargetEffect;

	// Token: 0x04000D83 RID: 3459
	public GameObject healingEffect;

	// Token: 0x04000D84 RID: 3460
	private float rotationSpeed = 45f;

	// Token: 0x04000D85 RID: 3461
	private float rotationSpeedTarget = 45f;

	// Token: 0x04000D86 RID: 3462
	private float attackCooldown = 5f;

	// Token: 0x04000D87 RID: 3463
	private int previousAttack = 666;

	// Token: 0x04000D88 RID: 3464
	public GameObject insignia;

	// Token: 0x04000D89 RID: 3465
	private float maxHealth;

	// Token: 0x04000D8A RID: 3466
	public GameObject homingProjectile;

	// Token: 0x04000D8B RID: 3467
	private int projectileAmount = 40;

	// Token: 0x04000D8C RID: 3468
	private int currentProjectile = 40;

	// Token: 0x04000D8D RID: 3469
	private float homingProjectileCooldown;

	// Token: 0x04000D8E RID: 3470
	public GameObject attackWindUp;

	// Token: 0x04000D8F RID: 3471
	public GameObject blackHole;

	// Token: 0x04000D90 RID: 3472
	private BlackHoleProjectile currentBlackHole;

	// Token: 0x04000D91 RID: 3473
	private int difficulty;

	// Token: 0x04000D92 RID: 3474
	public UltrakillEvent onFirstHeal;

	// Token: 0x04000D93 RID: 3475
	private int timesHealed;

	// Token: 0x04000D94 RID: 3476
	private bool noDrones;

	// Token: 0x04000D95 RID: 3477
	private MaterialPropertyBlock texOverride;
}
