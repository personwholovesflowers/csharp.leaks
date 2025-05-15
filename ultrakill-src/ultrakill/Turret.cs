using System;
using System.Collections.Generic;
using plog;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200048B RID: 1163
public class Turret : MonoBehaviour
{
	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06001AA4 RID: 6820 RVA: 0x000DAF9F File Offset: 0x000D919F
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x000DAFAC File Offset: 0x000D91AC
	private void Awake()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x000DAFBC File Offset: 0x000D91BC
	private void Start()
	{
		this.currentBodyRotation = base.transform.rotation;
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		this.path = new NavMeshPath();
		if (this.stationary)
		{
			this.stationaryPosition = base.transform.position;
		}
		if (this.quickStart)
		{
			this.cooldown = 0.5f;
		}
		base.Invoke("SlowUpdate", 0.5f);
		switch (this.difficulty)
		{
		case 0:
			this.maxAimTime = 7.5f;
			this.anim.speed = 0.5f;
			break;
		case 1:
			this.maxAimTime = 5f;
			this.anim.speed = 0.75f;
			break;
		case 2:
			this.maxAimTime = 5f;
			break;
		case 3:
			this.maxAimTime = 4f;
			break;
		case 4:
		case 5:
			this.maxAimTime = 3f;
			break;
		}
		if (this.difficulty >= 2)
		{
			this.anim.speed = 1f;
		}
		this.anim.speed *= this.eid.totalSpeedModifier;
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x000DB0F4 File Offset: 0x000D92F4
	private void UpdateBuff()
	{
		if (this.difficulty >= 2)
		{
			this.anim.speed = 1f;
		}
		else if (this.difficulty == 1)
		{
			this.anim.speed = 0.75f;
		}
		else
		{
			this.anim.speed = 0.5f;
		}
		this.anim.speed *= this.eid.totalSpeedModifier;
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000DB164 File Offset: 0x000D9364
	private void OnEnable()
	{
		this.Unlodge();
		this.CancelAim(true);
		this.DamageStop();
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000DB17C File Offset: 0x000D937C
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.25f);
		if (this.target == null)
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
			if (Physics.CheckSphere(this.torso.position - Vector3.up * 0.5f, 1.5f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)) || Physics.SphereCast(this.torso.position - Vector3.up * 0.5f, 1.5f, this.target.position + Vector3.up - this.torso.position, out raycastHit, Vector3.Distance(this.target.position + Vector3.up, this.torso.position), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
			{
				NavMesh.CalculatePath(base.transform.position, this.target.position, this.nma.areaMask, this.path);
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
				if (Physics.Raycast(this.torso.position, onUnitSphere, out raycastHit2, 25f, LayerMaskDefaults.Get(LMD.Environment)))
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
				else if (Physics.Raycast(this.torso.position + onUnitSphere * 25f, Vector3.down, out raycastHit3, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
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

	// Token: 0x06001AAA RID: 6826 RVA: 0x000DB4CC File Offset: 0x000D96CC
	private void Update()
	{
		if (this.target == null)
		{
			this.anim.SetBool("Running", false);
			if (this.currentSound)
			{
				Object.Destroy(this.currentSound.gameObject);
			}
			if (this.aiming)
			{
				this.CancelAim(false);
			}
			return;
		}
		if (this.aiming && !this.mach.grounded)
		{
			this.CancelAim(true);
		}
		if (!this.inAction)
		{
			this.rubbleLeft.SetActive(false);
			this.rubbleRight.SetActive(false);
		}
		if (!this.inAction && this.mach.grounded && this.eid.target != null)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			this.kickCooldown = Mathf.MoveTowards(this.kickCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			if (this.stationary && Vector3.Distance(base.transform.position, this.stationaryPosition) <= 1f)
			{
				base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
			}
			if (Vector3.Distance(this.target.position, base.transform.position) < 5f && this.kickCooldown <= 0f && this.difficulty >= 2)
			{
				this.Kick();
			}
			else
			{
				bool flag = Physics.CheckSphere(this.torso.position - Vector3.up * 0.5f, 1.5f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies));
				if (EnemyIdentifierDebug.Active && flag)
				{
					Turret.Log.Fine(string.Format("Turret windup for <b>{0}</b> blocked by <color=red>sphere check</color>", this.eid.target), null, null, null);
				}
				RaycastHit raycastHit;
				bool flag2 = Physics.SphereCast(this.torso.position - Vector3.up * 0.5f, 1.5f, this.target.position + Vector3.up - this.torso.position, out raycastHit, Vector3.Distance(this.target.position + Vector3.up, this.torso.position), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies));
				EnemyIdentifierIdentifier enemyIdentifierIdentifier;
				if (flag2 && this.target.isEnemy && raycastHit.collider.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid == this.target.enemyIdentifier)
				{
					flag2 = false;
				}
				if (EnemyIdentifierDebug.Active && flag2)
				{
					Turret.Log.Fine(string.Format("Turret windup for <b>{0}</b> blocked by <color=blue>sphere cast</color>\n{1}", this.eid.target, raycastHit.collider.gameObject.name), null, null, null);
				}
				if (this.cooldown <= 0f && !flag && !flag2)
				{
					this.StartWindup();
				}
				else if (this.nma.velocity.magnitude >= 1f)
				{
					this.anim.SetBool("Running", true);
				}
				else
				{
					this.anim.SetBool("Running", false);
				}
			}
		}
		else
		{
			this.anim.SetBool("Running", false);
		}
		if (this.aiming)
		{
			if (this.eid.target == null)
			{
				this.CancelAim(false);
				return;
			}
			RaycastHit raycastHit3;
			if (this.difficulty < 2 && this.aimTime >= this.maxAimTime)
			{
				if (this.difficulty == 1)
				{
					this.lastPlayerPosition = Vector3.MoveTowards(this.lastPlayerPosition, this.target.position, Time.deltaTime * Vector3.Distance(this.lastPlayerPosition, this.target.position) * 5f * this.eid.totalSpeedModifier);
				}
				RaycastHit raycastHit2;
				if (Physics.Raycast(this.torso.position, this.lastPlayerPosition - this.torso.position, out raycastHit2, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.EnvironmentAndPlayer)))
				{
					this.aimPos = raycastHit2.point;
				}
				else
				{
					this.aimPos = this.torso.position + (this.lastPlayerPosition - this.torso.position).normalized * 10000f;
				}
				this.outOfSightTimer = 0f;
			}
			else if (Physics.Raycast(this.torso.position, this.target.position - this.torso.position, out raycastHit3, Vector3.Distance(this.torso.position, this.target.position), LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.aimPos = raycastHit3.point;
				if (this.flashTime == 0f)
				{
					this.outOfSightTimer += Time.deltaTime;
				}
			}
			else
			{
				this.aimPos = this.target.position;
				this.outOfSightTimer = 0f;
			}
			this.aimTime = Mathf.MoveTowards(this.aimTime, this.maxAimTime, Time.deltaTime * this.eid.totalSpeedModifier);
			if (this.outOfSightTimer >= 1f)
			{
				if (this.currentSound)
				{
					Object.Destroy(this.currentSound.gameObject);
				}
				this.currentSound = Object.Instantiate<AudioSource>(this.cancelSound, this.torso);
				this.CancelAim(false);
			}
			else if (this.aimTime >= this.maxAimTime && (this.outOfSightTimer == 0f || this.flashTime != 0f))
			{
				if (this.flashTime == 0f)
				{
					Object.Instantiate<GameObject>(this.warningFlash, this.shootPoint.transform).transform.localScale *= 2.5f;
					this.ChangeLineColor(new Color(1f, 0.75f, 0.5f));
					this.lastPlayerPosition = this.target.position;
					this.ChangeLightsColor(this.attackingLightsColor);
					this.mach.ParryableCheck(false);
				}
				this.flashTime = Mathf.MoveTowards(this.flashTime, 1f, Time.deltaTime * (float)((this.difficulty >= 2) ? 2 : 1) * this.eid.totalSpeedModifier);
				if (this.flashTime >= 1f)
				{
					this.Shoot();
				}
			}
			else if (this.aimTime >= this.nextBeepTime && this.sinceLastBeep >= 0.075f)
			{
				if (this.whiteLine)
				{
					this.ChangeLineColor(this.defaultColor);
				}
				else
				{
					this.ChangeLineColor(Color.white);
				}
				ParticleSystem particleSystem = this.antennaFlash;
				if (particleSystem != null)
				{
					particleSystem.Play();
				}
				AudioSource audioSource = this.antennaSound;
				if (audioSource != null)
				{
					audioSource.Play();
				}
				this.whiteLine = !this.whiteLine;
				this.nextBeepTime = this.aimTime + (this.maxAimTime - this.aimTime) / 6f;
				this.sinceLastBeep = 0f;
			}
		}
		this.currentLightsIntensity = Mathf.MoveTowards(this.currentLightsIntensity, this.lightsIntensityTarget, Time.deltaTime / 4f);
		if (this.currentLightsIntensity == this.lightsIntensityTarget)
		{
			this.lightsIntensityTarget = ((this.lightsIntensityTarget == 1.5f) ? (this.lightsIntensityTarget = 1.25f) : (this.lightsIntensityTarget = 1.5f));
		}
		this.ChangeLightsIntensity(this.currentLightsIntensity);
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x000DBC94 File Offset: 0x000D9E94
	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		if (!this.aiming)
		{
			if (this.bodyRotate)
			{
				if (this.bodyTrackPlayer || this.bodyReset)
				{
					Quaternion quaternion = Quaternion.LookRotation(this.target.position - this.torso.position);
					if (this.bodyReset)
					{
						quaternion = base.transform.rotation;
					}
					float num = 10f;
					if (this.bodyTrackPlayer)
					{
						num = 35f;
					}
					this.currentBodyRotation = Quaternion.RotateTowards(this.currentBodyRotation, quaternion, Time.deltaTime * (Quaternion.Angle(quaternion, this.currentBodyRotation) * num + num) * this.eid.totalSpeedModifier);
					if (this.bodyReset && this.currentBodyRotation == quaternion)
					{
						this.bodyRotate = false;
						this.bodyReset = false;
					}
				}
				this.torso.rotation = this.currentBodyRotation;
				this.torso.Rotate(Vector3.up * -90f, Space.Self);
			}
			return;
		}
		if (this.difficulty < 2 && this.aimTime >= this.maxAimTime)
		{
			this.AimAt(this.lastPlayerPosition);
			return;
		}
		this.AimAt(this.target.position);
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x000DBDD4 File Offset: 0x000D9FD4
	private void StartWindup()
	{
		this.anim.SetBool("Aiming", true);
		if (this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(base.transform.position);
		}
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.inAction = true;
		this.kickCooldown = 0f;
		if (this.currentSound)
		{
			Object.Destroy(this.currentSound.gameObject);
		}
		this.currentSound = Object.Instantiate<AudioSource>(this.aimWarningSound, this.torso);
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x000DBE9C File Offset: 0x000DA09C
	private void BodyTrack()
	{
		this.bodyRotate = true;
		this.bodyTrackPlayer = true;
		this.bodyReset = false;
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x000DBEB3 File Offset: 0x000DA0B3
	private void BodyFreeze()
	{
		this.bodyRotate = true;
		this.bodyTrackPlayer = false;
		this.bodyReset = false;
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000DBECA File Offset: 0x000DA0CA
	private void BodyReset()
	{
		this.bodyRotate = true;
		this.bodyTrackPlayer = false;
		this.bodyReset = true;
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000DBEE4 File Offset: 0x000DA0E4
	private void StartAiming()
	{
		this.aiming = true;
		this.whiteLine = false;
		this.ChangeLineColor(this.defaultColor);
		this.nextBeepTime = this.aimTime + (this.maxAimTime - this.aimTime) / 6f;
		this.flashTime = 0f;
		this.eid.weakPoint = this.antenna;
		this.shotsInARow = 0;
		ParticleSystem particleSystem = this.antennaFlash;
		if (particleSystem != null)
		{
			particleSystem.Play();
		}
		AudioSource audioSource = this.antennaSound;
		if (audioSource == null)
		{
			return;
		}
		audioSource.Play();
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000DBF70 File Offset: 0x000DA170
	private void Kick()
	{
		this.anim.SetTrigger("Kick");
		if (this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(base.transform.position);
		}
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.inAction = true;
		this.ChangeLightsColor(new Color(0.35f, 0.55f, 1f));
		this.kickCooldown = 1f;
		if (this.currentSound)
		{
			Object.Destroy(this.currentSound.gameObject);
		}
		this.currentSound = Object.Instantiate<AudioSource>(this.kickWarningSound, this.torso);
		this.UnparryableFlash();
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000DC057 File Offset: 0x000DA257
	private void StopAction()
	{
		this.inAction = false;
		this.rubbleLeft.SetActive(false);
		this.rubbleRight.SetActive(false);
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x000DC078 File Offset: 0x000DA278
	private void AimAt(Vector3 position)
	{
		this.torso.LookAt(position);
		this.currentBodyRotation = this.torso.rotation;
		this.torso.Rotate(Vector3.up * -90f, Space.Self);
		this.turret.LookAt(position, this.torso.up);
		this.turret.Rotate(Vector3.up * -90f, Space.Self);
		this.aimLine.enabled = true;
		this.aimLine.SetPosition(0, this.shootPoint.position);
		this.aimLine.SetPosition(1, this.aimPos);
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x000DC124 File Offset: 0x000DA324
	private void Shoot()
	{
		RevolverBeam revolverBeam = Object.Instantiate<RevolverBeam>(this.beam, new Vector3(base.transform.position.x, this.shootPoint.transform.position.y, base.transform.position.z), this.shootPoint.transform.rotation);
		revolverBeam.alternateStartPoint = this.shootPoint.transform.position;
		RevolverBeam revolverBeam2;
		if (revolverBeam.TryGetComponent<RevolverBeam>(out revolverBeam2))
		{
			revolverBeam2.target = this.eid.target;
			if (this.eid.totalDamageModifier != 1f)
			{
				revolverBeam2.damage *= this.eid.totalDamageModifier;
			}
		}
		this.anim.Play("Shoot");
		this.CancelAim(false);
		this.BodyFreeze();
		this.cooldown = Random.Range(2.5f, 3.5f);
		this.shotsInARow++;
		if ((this.difficulty == 4 && this.shotsInARow < 2) || this.difficulty == 5)
		{
			base.Invoke("PreReAim", 0.25f);
		}
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x000DC24B File Offset: 0x000DA44B
	private void PreReAim()
	{
		this.anim.SetBool("Aiming", true);
		this.anim.Play("Aiming", -1, 0f);
		base.Invoke("ReAim", 0.25f);
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x000DC284 File Offset: 0x000DA484
	private void ReAim()
	{
		this.flashTime = 0f;
		this.aiming = true;
		this.aimLine.enabled = true;
		this.aimTime = this.maxAimTime;
		this.eid.weakPoint = this.antenna;
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x000DC2C4 File Offset: 0x000DA4C4
	private void ChangeLineColor(Color clr)
	{
		Gradient gradient = new Gradient();
		GradientColorKey[] array = new GradientColorKey[1];
		array[0].color = clr;
		GradientAlphaKey[] array2 = new GradientAlphaKey[1];
		array2[0].alpha = 1f;
		gradient.SetKeys(array, array2);
		this.aimLine.colorGradient = gradient;
		this.nextBeepTime = (this.maxAimTime - this.aimTime) / 2f;
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x000DC330 File Offset: 0x000DA530
	public void CancelAim(bool instant = false)
	{
		this.ChangeLightsColor(this.defaultLightsColor);
		this.aiming = false;
		this.aimLine.enabled = false;
		this.aimTime = 0f;
		this.outOfSightTimer = 0f;
		this.anim.SetBool("Aiming", false);
		this.BodyReset();
		this.eid.weakPoint = this.head;
		this.mach.parryable = false;
		if (instant)
		{
			this.inAction = false;
			if (this.mach.grounded)
			{
				this.anim.Play("Idle");
			}
		}
		if (this.cooldown < 1f)
		{
			this.cooldown = 1f;
		}
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x000DC3E8 File Offset: 0x000DA5E8
	public void LodgeFoot(int type)
	{
		if (type == 0)
		{
			this.leftLodged = true;
			this.rubbleLeft.SetActive(true);
		}
		else
		{
			this.rightLodged = true;
			this.rubbleRight.SetActive(true);
		}
		if (this.leftLodged && this.rightLodged)
		{
			this.lodged = true;
		}
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x000DC438 File Offset: 0x000DA638
	public void UnlodgeFoot(int type)
	{
		if (type == 0 && this.leftLodged)
		{
			this.leftLodged = false;
			this.rubbleLeft.SetActive(false);
			Object.Instantiate<GameObject>(this.rubble, this.rubbleLeft.transform.position, base.transform.rotation);
		}
		else if (type == 1 && this.rightLodged)
		{
			this.rightLodged = false;
			this.rubbleRight.SetActive(false);
			Object.Instantiate<GameObject>(this.rubble, this.rubbleRight.transform.position, base.transform.rotation);
		}
		this.lodged = false;
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x000DC4D9 File Offset: 0x000DA6D9
	public void Unlodge()
	{
		this.UnlodgeFoot(0);
		this.UnlodgeFoot(1);
		this.kickCooldown = 0.25f;
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x000DC4F4 File Offset: 0x000DA6F4
	public void Interrupt()
	{
		if (this.mach.limp)
		{
			return;
		}
		this.anim.SetTrigger("Interrupt");
		this.CancelAim(false);
		this.BodyFreeze();
		this.cooldown = 3f;
		if (this.currentSound)
		{
			Object.Destroy(this.currentSound.gameObject);
		}
		this.currentSound = Object.Instantiate<AudioSource>(this.interruptSound, this.torso);
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x000DC56C File Offset: 0x000DA76C
	public void OnDeath()
	{
		this.CancelAim(false);
		if (this.currentSound)
		{
			Object.Destroy(this.currentSound.gameObject);
		}
		this.ChangeLightsColor(new Color(0.05f, 0.05f, 0.05f, 1f));
		if (this.antennaLight)
		{
			this.antennaLight.enabled = false;
		}
		this.Unlodge();
		if (this.sc)
		{
			this.sc.gameObject.SetActive(false);
		}
		Object.Destroy(this);
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x000DC600 File Offset: 0x000DA800
	private void FootStep(float targetPitch)
	{
		if (targetPitch == 0f)
		{
			targetPitch = 1.5f;
		}
		Object.Instantiate<AudioSource>(this.footStep, base.transform.position, Quaternion.identity).pitch = Random.Range(targetPitch - 0.1f, targetPitch + 0.1f);
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x000DC64F File Offset: 0x000DA84F
	private void Thunk()
	{
		Object.Instantiate<AudioSource>(this.thunkSound, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x000DC66D File Offset: 0x000DA86D
	private void ExtendBarrel()
	{
		Object.Instantiate<AudioSource>(this.extendSound, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x000DC68B File Offset: 0x000DA88B
	private void GotParried()
	{
		this.Interrupt();
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x000DC694 File Offset: 0x000DA894
	public void UnparryableFlash()
	{
		Object.Instantiate<GameObject>(this.unparryableFlash, this.torso.position + base.transform.forward, base.transform.rotation).transform.localScale *= 2.5f;
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x000DC6EC File Offset: 0x000DA8EC
	public void DamageStart()
	{
		this.sc.DamageStart();
		this.tr.enabled = true;
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x000DC705 File Offset: 0x000DA905
	public void DamageStop()
	{
		this.sc.DamageStop();
		this.tr.enabled = false;
		this.ChangeLightsColor(this.defaultLightsColor);
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x000DC72C File Offset: 0x000DA92C
	public void ChangeLightsColor(Color target)
	{
		if (!this.smr || !this.smr.sharedMaterial || !this.smr.sharedMaterial.HasProperty("_EmissiveColor"))
		{
			return;
		}
		this.smr.material.SetColor("_EmissiveColor", target);
		if (this.antennaLight)
		{
			this.antennaLight.color = target;
		}
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000DC7A0 File Offset: 0x000DA9A0
	public void ChangeLightsIntensity(float amount)
	{
		if (!this.smr || !this.smr.sharedMaterial || !this.smr.sharedMaterial.HasProperty("_EmissiveIntensity"))
		{
			return;
		}
		this.smr.material.SetFloat("_EmissiveIntensity", amount);
		if (this.antennaLight)
		{
			this.antennaLight.intensity = amount * 8f;
		}
	}

	// Token: 0x0400255A RID: 9562
	private static readonly global::plog.Logger Log = new global::plog.Logger("Turret");

	// Token: 0x0400255B RID: 9563
	public bool stationary;

	// Token: 0x0400255C RID: 9564
	public bool quickStart;

	// Token: 0x0400255D RID: 9565
	private Vector3 stationaryPosition;

	// Token: 0x0400255E RID: 9566
	private NavMeshPath path;

	// Token: 0x0400255F RID: 9567
	private Vector3 aimPos;

	// Token: 0x04002560 RID: 9568
	[HideInInspector]
	public bool lodged;

	// Token: 0x04002561 RID: 9569
	[HideInInspector]
	public bool aiming;

	// Token: 0x04002562 RID: 9570
	private float outOfSightTimer;

	// Token: 0x04002563 RID: 9571
	private float aimTime;

	// Token: 0x04002564 RID: 9572
	private float maxAimTime = 5f;

	// Token: 0x04002565 RID: 9573
	private float flashTime;

	// Token: 0x04002566 RID: 9574
	private float nextBeepTime;

	// Token: 0x04002567 RID: 9575
	private bool whiteLine;

	// Token: 0x04002568 RID: 9576
	private Color defaultColor = new Color(1f, 0.44f, 0.74f);

	// Token: 0x04002569 RID: 9577
	private Vector3 lastPlayerPosition;

	// Token: 0x0400256A RID: 9578
	private int shotsInARow;

	// Token: 0x0400256B RID: 9579
	private TimeSince sinceLastBeep;

	// Token: 0x0400256C RID: 9580
	private int difficulty;

	// Token: 0x0400256D RID: 9581
	private float cooldown = 2f;

	// Token: 0x0400256E RID: 9582
	private float kickCooldown = 1f;

	// Token: 0x0400256F RID: 9583
	[HideInInspector]
	public bool inAction;

	// Token: 0x04002570 RID: 9584
	private bool bodyRotate;

	// Token: 0x04002571 RID: 9585
	private bool bodyTrackPlayer;

	// Token: 0x04002572 RID: 9586
	private bool bodyReset;

	// Token: 0x04002573 RID: 9587
	private Quaternion currentBodyRotation;

	// Token: 0x04002574 RID: 9588
	private bool walking;

	// Token: 0x04002575 RID: 9589
	private Vector3 walkTarget;

	// Token: 0x04002576 RID: 9590
	public Color defaultLightsColor;

	// Token: 0x04002577 RID: 9591
	public Color attackingLightsColor;

	// Token: 0x04002578 RID: 9592
	private float lightsIntensityTarget = 1.5f;

	// Token: 0x04002579 RID: 9593
	private float currentLightsIntensity = 1.25f;

	// Token: 0x0400257A RID: 9594
	[Header("Defaults")]
	[SerializeField]
	private Transform torso;

	// Token: 0x0400257B RID: 9595
	[SerializeField]
	private Transform turret;

	// Token: 0x0400257C RID: 9596
	[SerializeField]
	private Transform shootPoint;

	// Token: 0x0400257D RID: 9597
	[SerializeField]
	private LineRenderer aimLine;

	// Token: 0x0400257E RID: 9598
	[SerializeField]
	private RevolverBeam beam;

	// Token: 0x0400257F RID: 9599
	[SerializeField]
	private GameObject warningFlash;

	// Token: 0x04002580 RID: 9600
	[SerializeField]
	private ParticleSystem antennaFlash;

	// Token: 0x04002581 RID: 9601
	[SerializeField]
	private Light antennaLight;

	// Token: 0x04002582 RID: 9602
	[SerializeField]
	private AudioSource antennaSound;

	// Token: 0x04002583 RID: 9603
	[SerializeField]
	private Animator anim;

	// Token: 0x04002584 RID: 9604
	[SerializeField]
	private Machine mach;

	// Token: 0x04002585 RID: 9605
	[SerializeField]
	private EnemyIdentifier eid;

	// Token: 0x04002586 RID: 9606
	[SerializeField]
	private GameObject head;

	// Token: 0x04002587 RID: 9607
	[SerializeField]
	private NavMeshAgent nma;

	// Token: 0x04002588 RID: 9608
	public GameObject antenna;

	// Token: 0x04002589 RID: 9609
	public List<Transform> interruptables = new List<Transform>();

	// Token: 0x0400258A RID: 9610
	[SerializeField]
	private AudioSource interruptSound;

	// Token: 0x0400258B RID: 9611
	[SerializeField]
	private AudioSource cancelSound;

	// Token: 0x0400258C RID: 9612
	[SerializeField]
	private AudioSource footStep;

	// Token: 0x0400258D RID: 9613
	[SerializeField]
	private AudioSource extendSound;

	// Token: 0x0400258E RID: 9614
	[SerializeField]
	private AudioSource thunkSound;

	// Token: 0x0400258F RID: 9615
	[SerializeField]
	private AudioSource kickWarningSound;

	// Token: 0x04002590 RID: 9616
	[SerializeField]
	private AudioSource aimWarningSound;

	// Token: 0x04002591 RID: 9617
	private AudioSource currentSound;

	// Token: 0x04002592 RID: 9618
	[SerializeField]
	private GameObject rubble;

	// Token: 0x04002593 RID: 9619
	[SerializeField]
	private GameObject rubbleLeft;

	// Token: 0x04002594 RID: 9620
	[SerializeField]
	private GameObject rubbleRight;

	// Token: 0x04002595 RID: 9621
	private bool leftLodged;

	// Token: 0x04002596 RID: 9622
	private bool rightLodged;

	// Token: 0x04002597 RID: 9623
	[SerializeField]
	private SkinnedMeshRenderer smr;

	// Token: 0x04002598 RID: 9624
	[SerializeField]
	private GameObject unparryableFlash;

	// Token: 0x04002599 RID: 9625
	[SerializeField]
	private SwingCheck2 sc;

	// Token: 0x0400259A RID: 9626
	[SerializeField]
	private TrailRenderer tr;
}
