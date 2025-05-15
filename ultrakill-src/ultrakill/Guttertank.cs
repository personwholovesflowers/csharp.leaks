using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000243 RID: 579
public class Guttertank : MonoBehaviour, IHitTargetCallback
{
	// Token: 0x06000C9F RID: 3231 RVA: 0x0005CC6C File Offset: 0x0005AE6C
	private void Start()
	{
		this.GetValues();
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0005CC74 File Offset: 0x0005AE74
	private void GetValues()
	{
		if (this.gotValues)
		{
			return;
		}
		this.gotValues = true;
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.nma = base.GetComponent<NavMeshAgent>();
		this.mach = base.GetComponent<Machine>();
		this.rb = base.GetComponent<Rigidbody>();
		this.anim = base.GetComponent<Animator>();
		this.aud = base.GetComponent<AudioSource>();
		this.col = base.GetComponent<Collider>();
		this.shootCooldown = Random.Range(0.75f, 1.25f);
		this.mineCooldown = Random.Range(2f, 3f);
		this.stationaryPosition = base.transform.position;
		this.torsoDefaultRotation = Quaternion.Inverse(base.transform.rotation) * this.aimBone.rotation;
		this.path = new NavMeshPath();
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		this.SetSpeed();
		this.SlowUpdate();
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x0005CDA5 File Offset: 0x0005AFA5
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x0005CDB0 File Offset: 0x0005AFB0
	private void SetSpeed()
	{
		this.GetValues();
		if (this.difficulty >= 3)
		{
			this.anim.speed = 1f;
		}
		else if (this.difficulty == 2)
		{
			this.anim.speed = 0.9f;
		}
		else if (this.difficulty == 1)
		{
			this.anim.speed = 0.8f;
		}
		else if (this.difficulty == 0)
		{
			this.anim.speed = 0.6f;
		}
		this.anim.speed *= this.eid.totalSpeedModifier;
		this.nma.speed = 20f * this.anim.speed;
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x0005CE68 File Offset: 0x0005B068
	private void Update()
	{
		if (this.dead || this.eid.target == null)
		{
			return;
		}
		if (this.inAction)
		{
			Vector3 headPosition = this.eid.target.headPosition;
			if (this.overrideTarget)
			{
				headPosition = this.overrideTargetPosition;
			}
			if (this.trackInAction || this.moveForward)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(headPosition.x, base.transform.position.y, headPosition.z) - base.transform.position), (float)(this.trackInAction ? 360 : 90) * Time.deltaTime);
			}
		}
		else
		{
			RaycastHit raycastHit;
			bool flag = !Physics.Raycast(base.transform.position + Vector3.up, this.eid.target.headPosition - (base.transform.position + Vector3.up), out raycastHit, Vector3.Distance(this.eid.target.position, base.transform.position + Vector3.up), LayerMaskDefaults.Get(LMD.Environment));
			this.lineOfSightTimer = Mathf.MoveTowards(this.lineOfSightTimer, (float)(flag ? 1 : 0), Time.deltaTime * this.eid.totalSpeedModifier);
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown = Mathf.MoveTowards(this.shootCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if (this.mineCooldown > 0f)
			{
				this.mineCooldown = Mathf.MoveTowards(this.mineCooldown, 0f, Time.deltaTime * ((this.lineOfSightTimer >= 0.5f) ? 0.5f : 1f) * this.eid.totalSpeedModifier);
			}
			if (this.lineOfSightTimer >= 0.5f)
			{
				if (this.difficulty <= 1 && Vector3.Distance(base.transform.position, this.eid.target.position) > 10f && Vector3.Distance(base.transform.position, this.eid.target.PredictTargetPosition(0.5f, false)) > 10f)
				{
					this.punchCooldown = (float)((this.difficulty == 1) ? 1 : 2);
				}
				if (this.punchCooldown <= 0f && (Vector3.Distance(base.transform.position, this.eid.target.position) < 10f || Vector3.Distance(base.transform.position, this.eid.target.PredictTargetPosition(0.5f, false)) < 10f))
				{
					this.Punch();
				}
				else if (this.shootCooldown <= 0f && Vector3.Distance(base.transform.position, this.eid.target.PredictTargetPosition(1f, false)) > 15f)
				{
					this.PrepRocket();
				}
			}
			if (!this.inAction && this.mineCooldown <= 0f)
			{
				if (this.CheckMines())
				{
					this.PrepMine();
				}
				else
				{
					this.mineCooldown = 0.5f;
				}
			}
		}
		this.punchCooldown = Mathf.MoveTowards(this.punchCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		this.anim.SetBool("Walking", this.nma.velocity.magnitude > 2.5f);
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x0005D210 File Offset: 0x0005B410
	private void LateUpdate()
	{
		if (this.dead || this.eid.target == null)
		{
			return;
		}
		this.aimRotationLerp = Mathf.MoveTowards(this.aimRotationLerp, (float)((this.inAction && this.lookAtTarget) ? 1 : 0), Time.deltaTime * 5f);
		if (this.aimRotationLerp > 0f)
		{
			Vector3 vector = this.eid.target.headPosition;
			if (this.overrideTarget)
			{
				vector = this.overrideTargetPosition;
			}
			if (this.punching)
			{
				vector = this.eid.target.position;
			}
			Quaternion quaternion = Quaternion.LookRotation(this.aimBone.position - vector, Vector3.up);
			Quaternion quaternion2 = Quaternion.Inverse(base.transform.rotation * this.torsoDefaultRotation) * this.aimBone.rotation;
			this.aimBone.rotation = Quaternion.Lerp(this.aimBone.rotation, quaternion * quaternion2, this.aimRotationLerp);
			this.sc.knockBackDirection = this.aimBone.forward * -1f;
		}
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x0005D340 File Offset: 0x0005B540
	private void FixedUpdate()
	{
		if (this.dead)
		{
			return;
		}
		if (this.inAction)
		{
			this.rb.isKinematic = !this.moveForward;
			if (this.moveForward && !Physics.SphereCast(new Ray(base.transform.position + Vector3.up * 3f, base.transform.forward), 1.5f, 75f * Time.fixedDeltaTime * this.eid.totalSpeedModifier, LayerMaskDefaults.Get(LMD.Player)))
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(base.transform.position + Vector3.up + base.transform.forward, Vector3.down, out raycastHit, (this.eid.target == null) ? 22f : Mathf.Max(22f, base.transform.position.y - this.eid.target.position.y + 2.5f), LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
				{
					this.rb.velocity = base.transform.forward * 75f * this.anim.speed * this.eid.totalSpeedModifier;
					return;
				}
				this.rb.velocity = Vector3.zero;
			}
		}
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x0005D4BC File Offset: 0x0005B6BC
	private void SlowUpdate()
	{
		if (this.dead)
		{
			return;
		}
		base.Invoke("SlowUpdate", 0.25f);
		if (this.eid.target == null)
		{
			return;
		}
		if (!this.inAction && this.mach.grounded && this.nma.isOnNavMesh)
		{
			if (this.stationary)
			{
				if (Vector3.Distance(base.transform.position, this.stationaryPosition) <= 1f)
				{
					return;
				}
				NavMesh.CalculatePath(base.transform.position, this.stationaryPosition, this.nma.areaMask, this.path);
				if (this.path.status == NavMeshPathStatus.PathComplete)
				{
					this.nma.path = this.path;
					return;
				}
			}
			bool flag = false;
			RaycastHit raycastHit;
			if (Vector3.Distance(base.transform.position, this.eid.target.position) > 30f || Physics.CheckSphere(this.aimBone.position - Vector3.up * 0.5f, 1.5f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)) || Physics.SphereCast(this.aimBone.position - Vector3.up * 0.5f, 1.5f, this.eid.target.position + Vector3.up - this.aimBone.position, out raycastHit, Vector3.Distance(this.eid.target.position + Vector3.up, this.aimBone.position), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
			{
				if ((this.eid.target.isPlayer && ((MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS && !MonoSingleton<NewMovement>.Instance.gc.onGround) || (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && !MonoSingleton<PlatformerMovement>.Instance.groundCheck.onGround))) || (this.eid.target.isEnemy && (!this.eid.target.enemyIdentifier.gce || !this.eid.target.enemyIdentifier.gce.onGround)))
				{
					if (Physics.Raycast(this.eid.target.position, Vector3.down, out raycastHit, 120f, LayerMaskDefaults.Get(LMD.Environment)))
					{
						NavMesh.CalculatePath(base.transform.position, raycastHit.point, this.nma.areaMask, this.path);
					}
				}
				else
				{
					NavMesh.CalculatePath(base.transform.position, this.eid.target.position, this.nma.areaMask, this.path);
				}
				if (this.path.status == NavMeshPathStatus.PathComplete)
				{
					this.walking = false;
					flag = true;
					this.nma.path = this.path;
				}
			}
			if (!this.walking && !flag)
			{
				Vector3 onUnitSphere = Random.onUnitSphere;
				onUnitSphere = new Vector3(onUnitSphere.x, 0f, onUnitSphere.z);
				RaycastHit raycastHit2;
				RaycastHit raycastHit3;
				if (Physics.Raycast(this.aimBone.position, onUnitSphere, out raycastHit2, 25f, LayerMaskDefaults.Get(LMD.Environment)))
				{
					NavMeshHit navMeshHit;
					if (NavMesh.SamplePosition(raycastHit2.point, out navMeshHit, 5f, this.nma.areaMask))
					{
						this.walkTarget = navMeshHit.position;
					}
					else if (Physics.SphereCast(raycastHit2.point, 1f, Vector3.down, out raycastHit2, 25f, LayerMaskDefaults.Get(LMD.Environment)))
					{
						this.walkTarget = raycastHit2.point;
					}
				}
				else if (Physics.Raycast(this.aimBone.position + onUnitSphere * 25f, Vector3.down, out raycastHit3, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
				{
					this.walkTarget = raycastHit3.point;
				}
				NavMesh.CalculatePath(base.transform.position, this.walkTarget, this.nma.areaMask, this.path);
				this.nma.path = this.path;
				this.walking = true;
				return;
			}
			if (Vector3.Distance(base.transform.position, this.walkTarget) < 1f || this.nma.path.status != NavMeshPathStatus.PathComplete)
			{
				this.walking = false;
				return;
			}
		}
		else
		{
			this.walking = false;
		}
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x0005D94C File Offset: 0x0005BB4C
	private bool CheckMines()
	{
		if (this.placedMines.Count >= 5)
		{
			for (int i = this.placedMines.Count - 1; i >= 0; i--)
			{
				if (this.placedMines[i] == null)
				{
					this.placedMines.RemoveAt(i);
				}
			}
			if (this.placedMines.Count >= 5)
			{
				return false;
			}
		}
		for (int j = MonoSingleton<ObjectTracker>.Instance.landmineList.Count - 1; j >= 0; j--)
		{
			if (MonoSingleton<ObjectTracker>.Instance.landmineList[j] != null && Vector3.Distance(base.transform.position, MonoSingleton<ObjectTracker>.Instance.landmineList[j].transform.position) < 15f)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x0005DA18 File Offset: 0x0005BC18
	private void PrepMine()
	{
		this.anim.Play("Landmine", 0, 0f);
		Object.Instantiate<AudioSource>(this.minePrepSound, base.transform);
		this.inAction = true;
		this.nma.enabled = false;
		this.lookAtTarget = false;
		this.mineCooldown = Random.Range(2f, 3f);
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0005DA7C File Offset: 0x0005BC7C
	private void PlaceMine()
	{
		Landmine landmine = Object.Instantiate<Landmine>(this.landmine, base.transform.position, base.transform.rotation, this.gz.transform);
		this.placedMines.Add(landmine);
		Landmine landmine2;
		if (landmine.TryGetComponent<Landmine>(out landmine2))
		{
			landmine2.originEnemy = this.eid;
		}
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x0005DAD8 File Offset: 0x0005BCD8
	private void PrepRocket()
	{
		this.anim.Play("Shoot", 0, 0f);
		Object.Instantiate<AudioSource>(this.rocketPrepSound, base.transform);
		this.inAction = true;
		this.nma.enabled = false;
		this.trackInAction = true;
		this.lookAtTarget = true;
		this.punching = false;
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x0005DB38 File Offset: 0x0005BD38
	private void PredictTarget()
	{
		Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.unparryableFlash, this.shootPoint.position + base.transform.forward, base.transform.rotation).transform.localScale *= 10f;
		if (this.eid.target != null)
		{
			this.overrideTarget = true;
			float num = 1f;
			if (this.difficulty == 1)
			{
				num = 0.75f;
			}
			else if (this.difficulty == 0)
			{
				num = 0.5f;
			}
			this.overrideTargetPosition = this.eid.target.PredictTargetPosition((Random.Range(0.75f, 1f) + Vector3.Distance(this.shootPoint.position, this.eid.target.headPosition) / 150f) * num, false);
			if (Physics.Raycast(this.eid.target.position, Vector3.down, 15f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.overrideTargetPosition = new Vector3(this.overrideTargetPosition.x, this.eid.target.headPosition.y, this.overrideTargetPosition.z);
			}
			bool flag = false;
			RaycastHit raycastHit;
			Breakable breakable;
			if (Physics.Raycast(this.aimBone.position, this.overrideTargetPosition - this.aimBone.position, out raycastHit, Vector3.Distance(this.overrideTargetPosition, this.aimBone.position), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)) && (!raycastHit.transform.TryGetComponent<Breakable>(out breakable) || !breakable.playerOnly))
			{
				flag = true;
				this.overrideTargetPosition = this.eid.target.headPosition;
			}
			if (!flag && this.overrideTargetPosition != this.eid.target.headPosition && this.col.Raycast(new Ray(this.eid.target.headPosition, (this.overrideTargetPosition - this.eid.target.headPosition).normalized), out raycastHit, Vector3.Distance(this.eid.target.headPosition, this.overrideTargetPosition)))
			{
				this.overrideTargetPosition = this.eid.target.headPosition;
			}
		}
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x0005DD9C File Offset: 0x0005BF9C
	private void FireRocket()
	{
		Object.Instantiate<GameObject>(this.rocketParticle, this.shootPoint.position, Quaternion.LookRotation(this.overrideTargetPosition - this.shootPoint.position));
		Grenade grenade = Object.Instantiate<Grenade>(this.rocket, MonoSingleton<WeaponCharges>.Instance.rocketFrozen ? (this.shootPoint.position + this.shootPoint.forward * 2.5f) : this.shootPoint.position, Quaternion.LookRotation(this.overrideTargetPosition - this.shootPoint.position));
		grenade.proximityTarget = this.eid.target;
		grenade.ignoreEnemyType.Add(this.eid.enemyType);
		grenade.originEnemy = this.eid;
		if (this.eid.totalDamageModifier != 1f)
		{
			grenade.totalDamageMultiplier = this.eid.totalDamageModifier;
		}
		if (this.difficulty == 1)
		{
			grenade.rocketSpeed *= 0.8f;
		}
		else if (this.difficulty == 0)
		{
			grenade.rocketSpeed *= 0.6f;
		}
		this.shootCooldown = Random.Range(1.25f, 1.75f) - ((this.difficulty >= 4) ? 0.5f : 0f);
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x0005DEF8 File Offset: 0x0005C0F8
	private void Death()
	{
		this.PunchStop();
		this.dead = true;
		Collider collider;
		if (base.TryGetComponent<Collider>(out collider))
		{
			collider.enabled = false;
		}
		if (this.mach.gc.onGround)
		{
			this.rb.isKinematic = true;
		}
		else
		{
			this.rb.constraints = (RigidbodyConstraints)122;
		}
		this.mach.parryable = false;
		base.enabled = false;
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0005DF64 File Offset: 0x0005C164
	private void Punch()
	{
		if (this.difficulty <= 2)
		{
			this.punchCooldown = 4.5f - (float)this.difficulty;
		}
		else if (this.difficulty == 4)
		{
			this.punchCooldown = 1.5f;
		}
		this.anim.Play("Punch", 0, 0f);
		Object.Instantiate<AudioSource>(this.punchPrepSound, base.transform);
		Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.unparryableFlash, this.sc.transform.position + base.transform.forward, base.transform.rotation).transform.localScale *= 5f;
		this.inAction = true;
		this.nma.enabled = false;
		this.trackInAction = true;
		this.lookAtTarget = true;
		this.punching = true;
		this.punchHit = false;
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0005E04E File Offset: 0x0005C24E
	private void PunchActive()
	{
		this.sc.DamageStart();
		this.sc.knockBackDirectionOverride = true;
		this.sc.knockBackDirection = base.transform.forward;
		this.moveForward = true;
		this.trackInAction = false;
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x0005E08B File Offset: 0x0005C28B
	public void TargetBeenHit()
	{
		this.punchHit = true;
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x0005E094 File Offset: 0x0005C294
	private void PunchStop()
	{
		this.sc.DamageStop();
		this.moveForward = false;
		if (!this.punchHit || this.difficulty < 3)
		{
			bool flag = this.difficulty < 4 && !this.punchHit;
			if (!flag && (!this.punchHit || this.difficulty < 3))
			{
				Vector3Int vector3Int = StainVoxelManager.WorldToVoxelPosition(base.transform.position + Vector3.down * 1.8333334f);
				flag = MonoSingleton<StainVoxelManager>.Instance.HasProxiesAt(vector3Int, 3, VoxelCheckingShape.VerticalBox, ProxySearchMode.AnyFloor, true);
			}
			if (flag)
			{
				this.anim.Play("PunchStagger");
			}
		}
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x0005E138 File Offset: 0x0005C338
	private void FallImpact()
	{
		Object.Instantiate<AudioSource>(this.fallImpactSound, new Vector3(this.eid.weakPoint.transform.position.x, base.transform.position.y, this.eid.weakPoint.transform.position.z), Quaternion.identity);
		this.eid.hitter = "";
		this.eid.DeliverDamage(this.mach.chest, Vector3.zero, this.mach.chest.transform.position, 0.1f, false, 0f, null, false, false);
		if (!this.eid.dead)
		{
			this.mach.parryable = true;
			Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.parryableFlash, this.sc.transform.position + base.transform.forward * 5f, base.transform.rotation).transform.localScale *= 10f;
			return;
		}
		this.mach.parryable = false;
		MonoSingleton<StyleHUD>.Instance.AddPoints(50, "SLIPPED", null, null, -1, "", "");
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x0005E28F File Offset: 0x0005C48F
	private void GotParried()
	{
		if (!this.eid.dead && this.anim)
		{
			this.anim.Play("PunchStagger", -1, 0.7f);
		}
		this.mach.parryable = false;
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x0005E2CD File Offset: 0x0005C4CD
	private void StopParryable()
	{
		this.mach.parryable = false;
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x0005E2DB File Offset: 0x0005C4DB
	private void StopAction()
	{
		if (this.dead)
		{
			return;
		}
		this.inAction = false;
		this.nma.enabled = true;
		this.overrideTarget = false;
		this.punching = false;
		this.lookAtTarget = false;
	}

	// Token: 0x040010B7 RID: 4279
	private bool gotValues;

	// Token: 0x040010B8 RID: 4280
	private EnemyIdentifier eid;

	// Token: 0x040010B9 RID: 4281
	private NavMeshAgent nma;

	// Token: 0x040010BA RID: 4282
	private Machine mach;

	// Token: 0x040010BB RID: 4283
	private Rigidbody rb;

	// Token: 0x040010BC RID: 4284
	private Animator anim;

	// Token: 0x040010BD RID: 4285
	private AudioSource aud;

	// Token: 0x040010BE RID: 4286
	private Collider col;

	// Token: 0x040010BF RID: 4287
	private int difficulty;

	// Token: 0x040010C0 RID: 4288
	public bool stationary;

	// Token: 0x040010C1 RID: 4289
	private Vector3 stationaryPosition;

	// Token: 0x040010C2 RID: 4290
	private NavMeshPath path;

	// Token: 0x040010C3 RID: 4291
	private bool walking;

	// Token: 0x040010C4 RID: 4292
	private Vector3 walkTarget;

	// Token: 0x040010C5 RID: 4293
	private bool dead;

	// Token: 0x040010C6 RID: 4294
	[SerializeField]
	private SwingCheck2 sc;

	// Token: 0x040010C7 RID: 4295
	private bool inAction;

	// Token: 0x040010C8 RID: 4296
	private bool moveForward;

	// Token: 0x040010C9 RID: 4297
	private bool trackInAction;

	// Token: 0x040010CA RID: 4298
	private bool overrideTarget;

	// Token: 0x040010CB RID: 4299
	private bool lookAtTarget;

	// Token: 0x040010CC RID: 4300
	private bool punching;

	// Token: 0x040010CD RID: 4301
	private Vector3 overrideTargetPosition;

	// Token: 0x040010CE RID: 4302
	private float aimRotationLerp;

	// Token: 0x040010CF RID: 4303
	private float punchCooldown;

	// Token: 0x040010D0 RID: 4304
	private bool punchHit;

	// Token: 0x040010D1 RID: 4305
	public Transform shootPoint;

	// Token: 0x040010D2 RID: 4306
	public Grenade rocket;

	// Token: 0x040010D3 RID: 4307
	public GameObject rocketParticle;

	// Token: 0x040010D4 RID: 4308
	public Transform aimBone;

	// Token: 0x040010D5 RID: 4309
	private Quaternion torsoDefaultRotation;

	// Token: 0x040010D6 RID: 4310
	private float shootCooldown = 1f;

	// Token: 0x040010D7 RID: 4311
	private float lineOfSightTimer;

	// Token: 0x040010D8 RID: 4312
	public Landmine landmine;

	// Token: 0x040010D9 RID: 4313
	private float mineCooldown = 2f;

	// Token: 0x040010DA RID: 4314
	private List<Landmine> placedMines = new List<Landmine>();

	// Token: 0x040010DB RID: 4315
	private GoreZone gz;

	// Token: 0x040010DC RID: 4316
	public AudioSource punchPrepSound;

	// Token: 0x040010DD RID: 4317
	public AudioSource rocketPrepSound;

	// Token: 0x040010DE RID: 4318
	public AudioSource minePrepSound;

	// Token: 0x040010DF RID: 4319
	public AudioSource fallImpactSound;
}
