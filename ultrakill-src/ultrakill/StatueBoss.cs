using System;
using Sandbox;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

// Token: 0x02000451 RID: 1105
public class StatueBoss : MonoBehaviour, IEnrage, IAlter, IAlterOptions<bool>
{
	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06001915 RID: 6421 RVA: 0x000CD3B7 File Offset: 0x000CB5B7
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x000CD3C4 File Offset: 0x000CB5C4
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.part = base.transform.Find("DodgeParticle").GetComponent<ParticleSystem>();
		this.partAud = this.part.GetComponent<AudioSource>();
		this.st = base.GetComponent<Statue>();
		this.nma = base.GetComponentInChildren<NavMeshAgent>();
		this.anim = base.GetComponentInChildren<Animator>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.gc = base.GetComponentInChildren<GroundCheckEnemy>();
		this.enemyCollider = base.GetComponent<Collider>();
		this.orbLight = base.GetComponentInChildren<Light>();
		this.originalLightRange = this.orbLight.range;
		this.originalNmaRange = this.nma.stoppingDistance;
		this.originalNmaSpeed = this.nma.speed;
		this.originalNmaAcceleration = this.nma.acceleration;
		this.originalNmaAngularSpeed = this.nma.angularSpeed;
		this.nmp = new NavMeshPath();
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x000CD4BD File Offset: 0x000CB6BD
	private void Start()
	{
		this.cc = MonoSingleton<CameraController>.Instance;
		this.SetSpeed();
		if (this.inAction)
		{
			this.StopAction();
		}
		this.SlowUpdate();
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x000CD4E4 File Offset: 0x000CB6E4
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x000CD4EC File Offset: 0x000CB6EC
	private void SetSpeed()
	{
		if (!this.nma)
		{
			this.nma = base.GetComponentInChildren<NavMeshAgent>();
		}
		if (!this.anim)
		{
			this.anim = base.GetComponentInChildren<Animator>();
		}
		if (!this.eid)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (this.difficulty < 0)
		{
			if (this.eid.difficultyOverride >= 0)
			{
				this.difficulty = this.eid.difficultyOverride;
			}
			else
			{
				this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
			}
		}
		if (this.difficulty >= 4)
		{
			this.anim.speed = 1.35f;
		}
		else if (this.difficulty == 3)
		{
			this.anim.speed = 1.2f;
		}
		else if (this.difficulty == 1)
		{
			this.anim.speed = 0.8f;
		}
		else if (this.difficulty == 0)
		{
			this.anim.speed = 0.6f;
		}
		else
		{
			this.anim.speed = 1f;
		}
		this.realSpeedModifier = this.eid.totalSpeedModifier;
		if (this.difficulty == 4 && this.eid.totalSpeedModifier > 1.2f)
		{
			this.realSpeedModifier -= 0.2f;
		}
		this.anim.speed *= this.realSpeedModifier;
		if (this.enraged)
		{
			if (this.difficulty <= 2)
			{
				this.anim.speed *= 1.2f;
			}
			else if (this.difficulty > 3)
			{
				this.anim.speed = 1.5f * this.realSpeedModifier;
			}
			else
			{
				this.anim.speed = 1.25f * this.realSpeedModifier;
			}
			this.anim.SetFloat("WalkSpeed", 1.5f);
		}
		if (this.nma)
		{
			this.nma.speed = (this.enraged ? (this.originalNmaSpeed * 5f) : this.originalNmaSpeed) * this.realSpeedModifier;
			this.nma.acceleration = (float)(this.enraged ? 120 : 24) * this.realSpeedModifier;
			this.nma.angularSpeed = (float)(this.enraged ? 6000 : 1200) * this.realSpeedModifier;
		}
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x000CD750 File Offset: 0x000CB950
	private void OnEnable()
	{
		if (this.st)
		{
			this.StopAction();
			this.StopDamage();
			this.StopDash();
		}
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x000CD771 File Offset: 0x000CB971
	private void OnDisable()
	{
		if (this.currentStompWave != null)
		{
			Object.Destroy(this.currentStompWave);
		}
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x000CD78C File Offset: 0x000CB98C
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.1f);
		if (this.stationary)
		{
			return;
		}
		Vector3 vector = ((this.target != null) ? new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z) : base.transform.position);
		if (!this.inAction && this.nma.isOnNavMesh)
		{
			if (Vector3.Distance(vector, base.transform.position) > 3f)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(this.target.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
				{
					this.SetDestination(raycastHit.point);
					return;
				}
				this.SetDestination(this.target.position);
				return;
			}
			else
			{
				this.nma.SetDestination(base.transform.position);
			}
		}
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x000CD8A4 File Offset: 0x000CBAA4
	private void SetDestination(Vector3 position)
	{
		if (!this.nma || !this.nma.isOnNavMesh)
		{
			return;
		}
		NavMesh.CalculatePath(base.transform.position, position, this.nma.areaMask, this.nmp);
		this.nma.SetPath(this.nmp);
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x000CD904 File Offset: 0x000CBB04
	private void Update()
	{
		if (this.target == null)
		{
			this.StopAction();
			this.StopDamage();
			this.anim.SetBool("Walking", false);
			if (this.nma.isOnNavMesh && !this.nma.isStopped)
			{
				this.nma.isStopped = true;
			}
			return;
		}
		Vector3 vector = new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z);
		if (!this.inAction)
		{
			if (this.nma.isOnNavMesh && Vector3.Distance(vector, base.transform.position) <= 3f)
			{
				base.transform.LookAt(vector);
			}
			if (this.nma.enabled && this.nma.velocity.magnitude > 1f)
			{
				this.anim.SetBool("Walking", true);
			}
			else
			{
				this.anim.SetBool("Walking", false);
			}
		}
		if (this.attackCheckCooldown > 0f)
		{
			this.attackCheckCooldown = Mathf.MoveTowards(this.attackCheckCooldown, 0f, Time.deltaTime);
		}
		if (!this.inAction && this.gc.onGround && this.attackCheckCooldown <= 0f && this.target != null)
		{
			this.attackCheckCooldown = 0.2f;
			if (!Physics.Raycast(this.st.chest.transform.position, this.target.position - this.st.chest.transform.position, Vector3.Distance(this.target.position, this.st.chest.transform.position), LayerMaskDefaults.Get(LMD.Environment)))
			{
				if (this.meleeRecharge >= 2f || (this.meleeRecharge >= 1f && Vector3.Distance(base.transform.position, vector) < 15f && (Mathf.Abs(base.transform.position.y - this.target.position.y) < 9f || (Mathf.Abs(MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).y) > 2f && Mathf.Abs(base.transform.position.y - this.target.position.y) < 19f))))
				{
					int num = Random.Range(0, 100);
					this.meleeRecharge = 0f;
					if (this.stationary || (this.target.position.y < base.transform.position.y + 5f && num > this.tackleChance))
					{
						if (this.tackleChance < 50)
						{
							this.tackleChance = 50;
						}
						this.tackleChance += 20;
						this.inAction = true;
						this.Stomp();
					}
					else
					{
						if (this.tackleChance > 50)
						{
							this.tackleChance = 50;
						}
						this.tackleChance -= 20;
						this.inAction = true;
						this.Tackle();
					}
				}
				else if (this.rangedRecharge >= 1f && Vector3.Distance(base.transform.position, vector) >= 9f)
				{
					this.rangedRecharge = 0f;
					this.inAction = true;
					this.Throw();
				}
			}
		}
		if (this.tracking)
		{
			base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		}
		if (this.backUp != null && this.st.health < 40f)
		{
			this.backUp.SetActive(true);
			this.backUp = null;
		}
		if (this.orbGrowing)
		{
			this.orbLight.range = Mathf.MoveTowards(this.orbLight.range, this.originalLightRange, Time.deltaTime * 20f * this.realSpeedModifier);
			if (this.orbLight.range == this.originalLightRange)
			{
				this.orbGrowing = false;
			}
		}
		if (this.rangedRecharge < 1f)
		{
			float num2 = 1f;
			if (Vector3.Distance(base.transform.position, vector) < 15f)
			{
				num2 = 0.5f;
			}
			if (this.difficulty >= 4)
			{
				num2 += 0.5f;
			}
			else if (this.difficulty == 1)
			{
				num2 -= 0.2f;
			}
			else if (this.difficulty == 0)
			{
				num2 -= 0.35f;
			}
			num2 *= this.realSpeedModifier;
			if (this.enraged)
			{
				this.rangedRecharge = Mathf.MoveTowards(this.rangedRecharge, 1f, Time.deltaTime * 0.4f * num2);
			}
			else if (this.st.health < 60f)
			{
				this.rangedRecharge = Mathf.MoveTowards(this.rangedRecharge, 1f, Time.deltaTime * 0.15f * num2);
			}
			else if (this.difficulty > 3)
			{
				this.rangedRecharge = Mathf.MoveTowards(this.rangedRecharge, 1f, Time.deltaTime * 0.32f * num2);
			}
			else if (this.difficulty == 3)
			{
				this.rangedRecharge = Mathf.MoveTowards(this.rangedRecharge, 1f, Time.deltaTime * 0.285f * num2);
			}
			else
			{
				this.rangedRecharge = Mathf.MoveTowards(this.rangedRecharge, 1f, Time.deltaTime * 0.275f * num2);
			}
		}
		if (this.meleeRecharge < 1f)
		{
			float num3 = 1f;
			if (Vector3.Distance(base.transform.position, vector) < 9f)
			{
				this.playerInCloseRange = Mathf.MoveTowards(this.playerInCloseRange, 1f, Time.deltaTime);
				if (this.playerInCloseRange >= 1f)
				{
					num3 = 2f;
				}
			}
			else
			{
				this.playerInCloseRange = Mathf.MoveTowards(this.playerInCloseRange, 0f, Time.deltaTime);
			}
			if (this.difficulty >= 4)
			{
				num3 += 0.5f;
			}
			else if (this.difficulty == 1)
			{
				num3 -= 0.25f;
			}
			else if (this.difficulty == 0)
			{
				num3 -= 0.5f;
			}
			num3 *= this.realSpeedModifier;
			if (this.enraged)
			{
				if (this.meleeRecharge < 1f && this.difficulty >= 2)
				{
					this.meleeRecharge = 1f;
					return;
				}
				this.meleeRecharge = Mathf.MoveTowards(this.meleeRecharge, 2f, Time.deltaTime * 0.4f);
				return;
			}
			else
			{
				if (this.st.health < 60f)
				{
					this.meleeRecharge = Mathf.MoveTowards(this.meleeRecharge, 2f, Time.deltaTime * 0.25f * num3);
					return;
				}
				if (this.difficulty > 3)
				{
					this.meleeRecharge = Mathf.MoveTowards(this.meleeRecharge, 2f, Time.deltaTime * 0.4f * num3);
					return;
				}
				if (this.difficulty == 3)
				{
					this.meleeRecharge = Mathf.MoveTowards(this.meleeRecharge, 2f, Time.deltaTime * 0.385f * num3);
					return;
				}
				this.meleeRecharge = Mathf.MoveTowards(this.meleeRecharge, 2f, Time.deltaTime * 0.375f * num3);
			}
		}
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x000CE084 File Offset: 0x000CC284
	private void FixedUpdate()
	{
		if (this.dashPower > 1f)
		{
			Vector3 vector;
			if (this.difficulty > 2)
			{
				float num = 1f;
				if (this.difficulty >= 4)
				{
					num = 1.25f;
				}
				vector = new Vector3(base.transform.forward.x * this.dashPower * num * this.realSpeedModifier, this.rb.velocity.y, base.transform.forward.z * this.dashPower * num * this.realSpeedModifier);
				this.dashPower /= 1.075f;
			}
			else if (this.difficulty == 2)
			{
				vector = new Vector3(base.transform.forward.x * this.dashPower / 1.25f * this.realSpeedModifier, this.rb.velocity.y, base.transform.forward.z * this.dashPower / 1.25f * this.realSpeedModifier);
				this.dashPower /= 1.065625f;
			}
			else if (this.difficulty == 1)
			{
				vector = new Vector3(base.transform.forward.x * this.dashPower / 1.5f * this.realSpeedModifier, this.rb.velocity.y, base.transform.forward.z * this.dashPower / 1.5f * this.realSpeedModifier);
				this.dashPower /= 1.05625f;
			}
			else
			{
				vector = new Vector3(base.transform.forward.x * this.dashPower / 2f * this.realSpeedModifier, this.rb.velocity.y, base.transform.forward.z * this.dashPower / 2f * this.realSpeedModifier);
				this.dashPower /= 1.0375f;
			}
			RaycastHit raycastHit;
			if (this.enraged || Physics.Raycast(base.transform.position + Vector3.up + base.transform.forward * Mathf.Max(1f, vector.magnitude * Time.fixedDeltaTime), Vector3.down, out raycastHit, 12f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				this.rb.velocity = vector;
			}
			else
			{
				this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
			}
			if (this.rb.velocity.y > 0f || this.dontFall)
			{
				this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
				return;
			}
		}
		else
		{
			if (this.dashPower != 0f)
			{
				this.rb.velocity = Vector3.zero;
				this.dashPower = 0f;
				this.damaging = false;
				return;
			}
			if (this.dontFall)
			{
				this.rb.velocity = Vector3.zero;
			}
		}
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x000CE3DC File Offset: 0x000CC5DC
	private void Stomp()
	{
		if (this.target == null)
		{
			return;
		}
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.anim.SetTrigger("Stomp");
		this.launching = false;
		Object.Instantiate<GameObject>(this.statueChargeSound, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x000CE48C File Offset: 0x000CC68C
	private void Tackle()
	{
		if (this.target == null)
		{
			return;
		}
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.tracking = true;
		this.anim.SetTrigger("Tackle");
		if (this.difficulty >= 4)
		{
			this.extraTackles = 1;
		}
		this.damage = 25;
		this.launching = true;
		Object.Instantiate<GameObject>(this.statueChargeSound3, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x000CE558 File Offset: 0x000CC758
	private void Throw()
	{
		if (this.target == null)
		{
			return;
		}
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.tracking = true;
		this.anim.SetTrigger("Throw");
		Object.Instantiate<GameObject>(this.statueChargeSound2, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x000CE608 File Offset: 0x000CC808
	public void StompHit()
	{
		this.cc.CameraShake(1f);
		if (this.currentStompWave != null)
		{
			Object.Destroy(this.currentStompWave);
		}
		int num = 1;
		if (this.difficulty == 4)
		{
			num = 2;
		}
		if (this.difficulty == 5)
		{
			num = 3;
		}
		for (int i = 0; i < num; i++)
		{
			this.currentStompWave = Object.Instantiate<GameObject>(this.stompWave.ToAsset(), new Vector3(this.stompPos.position.x, base.transform.position.y, this.stompPos.position.z), Quaternion.identity);
			PhysicalShockwave component = this.currentStompWave.GetComponent<PhysicalShockwave>();
			component.damage = 25;
			if (this.difficulty >= 4)
			{
				component.speed = 75f;
			}
			else if (this.difficulty == 3)
			{
				component.speed = 50f;
			}
			else if (this.difficulty == 2)
			{
				component.speed = 35f;
			}
			else if (this.difficulty == 1)
			{
				component.speed = 25f;
			}
			else if (this.difficulty == 0)
			{
				component.speed = 15f;
			}
			if (i != 0)
			{
				component.speed /= (float)(1 + i * 2);
				AudioSource audioSource;
				if (component.TryGetComponent<AudioSource>(out audioSource))
				{
					audioSource.enabled = false;
				}
			}
			component.damage = Mathf.RoundToInt((float)component.damage * this.eid.totalDamageModifier);
			component.maxSize = 100f;
			component.enemy = true;
			component.enemyType = EnemyType.Cerberus;
		}
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x000CE798 File Offset: 0x000CC998
	public void OrbSpawn()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.orbProjectile.ToAsset(), new Vector3(this.orbLight.transform.position.x, base.transform.position.y + 3.5f, this.orbLight.transform.position.z), Quaternion.identity);
		gameObject.transform.LookAt(this.projectedPlayerPos);
		if (this.difficulty > 2)
		{
			gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 20000f);
		}
		else if (this.difficulty == 2)
		{
			gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 15000f);
		}
		else
		{
			gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 10000f);
		}
		Projectile projectile;
		if (gameObject.TryGetComponent<Projectile>(out projectile))
		{
			projectile.target = this.eid.target;
			if (this.difficulty <= 2)
			{
				if (this.difficulty <= 2)
				{
					projectile.bigExplosion = false;
				}
				projectile.damage *= this.eid.totalDamageModifier;
			}
		}
		this.orbGrowing = false;
		this.orbLight.range = 0f;
		this.part.Play();
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x000CE8F5 File Offset: 0x000CCAF5
	public void OrbRespawn()
	{
		this.orbGrowing = true;
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x000CE900 File Offset: 0x000CCB00
	public void StopAction()
	{
		if (this.gc.onGround)
		{
			this.nma.updatePosition = true;
			this.nma.updateRotation = true;
			this.nma.enabled = true;
		}
		this.tracking = false;
		this.inAction = false;
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x000CE94C File Offset: 0x000CCB4C
	public void StopTracking()
	{
		this.tracking = false;
		if (this.target == null)
		{
			return;
		}
		if (this.target.GetVelocity().magnitude == 0f)
		{
			base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
			this.projectedPlayerPos = this.target.position;
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(this.target.position, MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false), out raycastHit, MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).magnitude * 0.35f / this.realSpeedModifier, 4096, QueryTriggerInteraction.Collide) && raycastHit.collider == this.enemyCollider)
		{
			this.projectedPlayerPos = this.target.position;
		}
		else if (Physics.Raycast(this.target.position, MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false), out raycastHit, MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).magnitude * 0.35f / this.realSpeedModifier, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Collide))
		{
			this.projectedPlayerPos = raycastHit.point;
		}
		else
		{
			this.projectedPlayerPos = this.target.position + MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false) * 0.35f / this.realSpeedModifier;
			this.projectedPlayerPos = new Vector3(this.projectedPlayerPos.x, this.target.position.y + (this.target.position.y - this.projectedPlayerPos.y) * 0.5f, this.projectedPlayerPos.z);
		}
		base.transform.LookAt(new Vector3(this.projectedPlayerPos.x, base.transform.position.y, this.projectedPlayerPos.z));
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x000CEB64 File Offset: 0x000CCD64
	public void Dash()
	{
		if (this.difficulty >= 4)
		{
			this.dontFall = true;
		}
		this.rb.isKinematic = false;
		this.rb.velocity = Vector3.zero;
		this.dashPower = 200f;
		this.damaging = true;
		this.part.Play();
		this.partAud.Play();
		this.StartDamage();
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x000CEBCC File Offset: 0x000CCDCC
	public void StopDash()
	{
		this.dashPower = 0f;
		if (this.gc.onGround)
		{
			this.rb.isKinematic = true;
		}
		else
		{
			this.rb.velocity = Vector3.zero;
		}
		this.damaging = false;
		this.partAud.Stop();
		this.StopDamage();
		if (this.extraTackles > 0)
		{
			this.dontFall = true;
			this.extraTackles--;
			this.tracking = true;
			this.anim.speed = 0.1f;
			GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.unparryableFlash, base.transform.position + Vector3.up * 6f + base.transform.forward * 3f, base.transform.rotation);
			gameObject.transform.localScale *= 5f;
			gameObject.transform.SetParent(base.transform, true);
			this.anim.Play("Tackle", -1, 0.4f);
			base.Invoke("DelayedTackle", 0.5f / this.realSpeedModifier);
			return;
		}
		this.dontFall = false;
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x000CED13 File Offset: 0x000CCF13
	private void DelayedTackle()
	{
		this.dontFall = false;
		this.SetSpeed();
		this.StopTracking();
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x000CED28 File Offset: 0x000CCF28
	public void ForceStopDashSound()
	{
		this.partAud.Stop();
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x000CED35 File Offset: 0x000CCF35
	public void StartDamage()
	{
		this.damaging = true;
		if (this.swingCheck == null)
		{
			this.swingCheck = base.GetComponentInChildren<SwingCheck2>();
		}
		this.swingCheck.damage = this.damage;
		this.swingCheck.DamageStart();
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x000CED74 File Offset: 0x000CCF74
	public void StopDamage()
	{
		this.damaging = false;
		if (this.swingCheck == null)
		{
			this.swingCheck = base.GetComponentInChildren<SwingCheck2>();
		}
		this.swingCheck.DamageStop();
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x000CEDA2 File Offset: 0x000CCFA2
	public void Step()
	{
		Object.Instantiate<GameObject>(this.stepSound, base.transform.position, Quaternion.identity).GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x000CEDD8 File Offset: 0x000CCFD8
	public void EnrageDelayed()
	{
		if (!this.enraged)
		{
			base.Invoke("Enrage", 1f / (this.eid ? this.realSpeedModifier : 1f));
		}
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x000CEE10 File Offset: 0x000CD010
	public void UnEnrage()
	{
		if (this.eid.dead)
		{
			return;
		}
		if (!this.enraged)
		{
			return;
		}
		this.enraged = false;
		if (this.ensims == null || this.ensims.Length == 0)
		{
			this.ensims = base.GetComponentsInChildren<EnemySimplifier>();
		}
		EnemySimplifier[] array = this.ensims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enraged = false;
		}
		if (this.currentEnrageEffect != null)
		{
			Object.Destroy(this.currentEnrageEffect);
		}
		if (this.difficulty <= 2)
		{
			this.anim.speed /= 1.2f;
		}
		else if (this.difficulty > 3)
		{
			this.anim.speed = 1.5f * this.realSpeedModifier;
		}
		else
		{
			this.anim.speed = 1.25f * this.realSpeedModifier;
		}
		this.orbLight.range = this.originalLightRange;
		this.nma.stoppingDistance = this.originalNmaRange;
		this.nma.speed = this.originalNmaSpeed * this.realSpeedModifier;
		this.nma.angularSpeed = this.originalNmaAngularSpeed * this.realSpeedModifier;
		this.nma.acceleration = this.originalNmaAcceleration * this.realSpeedModifier;
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06001931 RID: 6449 RVA: 0x000CEF54 File Offset: 0x000CD154
	public bool isEnraged
	{
		get
		{
			return this.enraged;
		}
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x000CEF5C File Offset: 0x000CD15C
	public void Enrage()
	{
		if (this.eid.dead)
		{
			return;
		}
		if (this.enraged)
		{
			return;
		}
		this.enraged = true;
		base.CancelInvoke("Enrage");
		GameObject gameObject = Object.Instantiate<GameObject>(this.statueChargeSound2, base.transform.position, Quaternion.identity);
		gameObject.GetComponent<AudioSource>().pitch = 0.3f;
		gameObject.GetComponent<AudioDistortionFilter>().distortionLevel = 0.5f;
		if (this.difficulty <= 2)
		{
			this.anim.speed *= 1.2f;
		}
		else if (this.difficulty > 3)
		{
			this.anim.speed = 1.5f * this.realSpeedModifier;
		}
		else
		{
			this.anim.speed = 1.25f * this.realSpeedModifier;
		}
		this.orbLight.range *= 2f;
		this.originalLightRange *= 2f;
		this.nma.speed = 25f * this.realSpeedModifier;
		this.nma.acceleration = 120f * this.realSpeedModifier;
		this.nma.angularSpeed = 6000f * this.realSpeedModifier;
		this.anim.SetFloat(StatueBoss.WalkSpeed, 1.5f);
		this.currentEnrageEffect = Object.Instantiate<GameObject>(this.enrageEffect, this.st.chest.transform);
		this.currentEnrageEffect.transform.localScale = Vector3.one * 0.04f;
		this.currentEnrageEffect.transform.localPosition = new Vector3(0f, 0.025f, 0f);
		if (this.ensims == null || this.ensims.Length == 0)
		{
			this.ensims = base.GetComponentsInChildren<EnemySimplifier>(true);
		}
		EnemySimplifier[] array = this.ensims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enraged = true;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06001933 RID: 6451 RVA: 0x000CF14A File Offset: 0x000CD34A
	public string alterKey
	{
		get
		{
			return "statue";
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06001934 RID: 6452 RVA: 0x000CF14A File Offset: 0x000CD34A
	public string alterCategoryName
	{
		get
		{
			return "statue";
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06001935 RID: 6453 RVA: 0x000CF154 File Offset: 0x000CD354
	public AlterOption<bool>[] options
	{
		get
		{
			return new AlterOption<bool>[]
			{
				new AlterOption<bool>
				{
					value = this.enraged,
					callback = delegate(bool value)
					{
						if (value)
						{
							this.Enrage();
							return;
						}
						this.UnEnrage();
					},
					key = "enraged",
					name = "Enraged"
				}
			};
		}
	}

	// Token: 0x04002330 RID: 9008
	private Animator anim;

	// Token: 0x04002331 RID: 9009
	private NavMeshAgent nma;

	// Token: 0x04002332 RID: 9010
	private NavMeshPath nmp;

	// Token: 0x04002333 RID: 9011
	private CameraController cc;

	// Token: 0x04002334 RID: 9012
	private Rigidbody rb;

	// Token: 0x04002335 RID: 9013
	[HideInInspector]
	public bool inAction;

	// Token: 0x04002336 RID: 9014
	public bool stationary;

	// Token: 0x04002337 RID: 9015
	public Transform stompPos;

	// Token: 0x04002338 RID: 9016
	public AssetReference stompWave;

	// Token: 0x04002339 RID: 9017
	private bool tracking;

	// Token: 0x0400233A RID: 9018
	private bool dashing;

	// Token: 0x0400233B RID: 9019
	private float dashPower;

	// Token: 0x0400233C RID: 9020
	private GameObject currentStompWave;

	// Token: 0x0400233D RID: 9021
	private float meleeRecharge = 1f;

	// Token: 0x0400233E RID: 9022
	private float playerInCloseRange;

	// Token: 0x0400233F RID: 9023
	private bool dontFall;

	// Token: 0x04002340 RID: 9024
	[HideInInspector]
	public bool damaging;

	// Token: 0x04002341 RID: 9025
	[HideInInspector]
	public bool launching;

	// Token: 0x04002342 RID: 9026
	[HideInInspector]
	public int damage;

	// Token: 0x04002343 RID: 9027
	private int tackleChance = 50;

	// Token: 0x04002344 RID: 9028
	private int extraTackles;

	// Token: 0x04002345 RID: 9029
	private float rangedRecharge = 1f;

	// Token: 0x04002346 RID: 9030
	private int throwChance = 50;

	// Token: 0x04002347 RID: 9031
	public float attackCheckCooldown = 1f;

	// Token: 0x04002348 RID: 9032
	public AssetReference orbProjectile;

	// Token: 0x04002349 RID: 9033
	private Light orbLight;

	// Token: 0x0400234A RID: 9034
	private Vector3 projectedPlayerPos;

	// Token: 0x0400234B RID: 9035
	private bool orbGrowing;

	// Token: 0x0400234C RID: 9036
	public GameObject stepSound;

	// Token: 0x0400234D RID: 9037
	private ParticleSystem part;

	// Token: 0x0400234E RID: 9038
	private AudioSource partAud;

	// Token: 0x0400234F RID: 9039
	private Statue st;

	// Token: 0x04002350 RID: 9040
	public GameObject backUp;

	// Token: 0x04002351 RID: 9041
	public GameObject statueChargeSound;

	// Token: 0x04002352 RID: 9042
	public GameObject statueChargeSound2;

	// Token: 0x04002353 RID: 9043
	public GameObject statueChargeSound3;

	// Token: 0x04002354 RID: 9044
	public bool enraged;

	// Token: 0x04002355 RID: 9045
	public GameObject enrageEffect;

	// Token: 0x04002356 RID: 9046
	public GameObject currentEnrageEffect;

	// Token: 0x04002357 RID: 9047
	private EnemySimplifier[] ensims;

	// Token: 0x04002358 RID: 9048
	private int difficulty = -1;

	// Token: 0x04002359 RID: 9049
	private SwingCheck2 swingCheck;

	// Token: 0x0400235A RID: 9050
	private GroundCheckEnemy gc;

	// Token: 0x0400235B RID: 9051
	private EnemyIdentifier eid;

	// Token: 0x0400235C RID: 9052
	private Collider enemyCollider;

	// Token: 0x0400235D RID: 9053
	private float originalLightRange;

	// Token: 0x0400235E RID: 9054
	private float originalNmaRange;

	// Token: 0x0400235F RID: 9055
	private float originalNmaSpeed;

	// Token: 0x04002360 RID: 9056
	private float originalNmaAcceleration;

	// Token: 0x04002361 RID: 9057
	private float originalNmaAngularSpeed;

	// Token: 0x04002362 RID: 9058
	private float realSpeedModifier;

	// Token: 0x04002363 RID: 9059
	private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
}
