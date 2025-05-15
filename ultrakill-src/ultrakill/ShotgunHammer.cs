using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003FB RID: 1019
public class ShotgunHammer : MonoBehaviour
{
	// Token: 0x060016E8 RID: 5864 RVA: 0x000B8624 File Offset: 0x000B6824
	private void Awake()
	{
		if (this.defaultModelPosition == Vector3.zero)
		{
			this.defaultModelPosition = this.modelTransform.localPosition;
		}
		if (this.hammerDefaultPosition == Vector3.zero)
		{
			this.hammerDefaultPosition = this.hammerPullable.localPosition;
		}
		this.targeter = Camera.main.GetComponent<CameraFrustumTargeter>();
		this.wid = base.GetComponent<WeaponIdentifier>();
		this.gc = base.GetComponentInParent<GunControl>();
		this.wpos = base.GetComponent<WeaponPos>();
		this.anim = base.GetComponent<Animator>();
		this.block = new MaterialPropertyBlock();
		if (this.chainsawBladeScroll)
		{
			this.chainsawBladeRenderer = this.chainsawBladeScroll.GetComponent<MeshRenderer>();
			this.chainsawBladeMaterial = this.chainsawBladeRenderer.sharedMaterial;
		}
		if (this.sawZone)
		{
			this.sawZone.sourceWeapon = base.gameObject;
		}
	}

	// Token: 0x060016E9 RID: 5865 RVA: 0x000B8710 File Offset: 0x000B6910
	private void OnEnable()
	{
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
		{
			this.fireHeldOnPullOut = true;
		}
		this.impactRoutine = null;
		this.pulledOut = 0f;
		this.UpdateMeter(true);
		if (this.variation == 2)
		{
			foreach (Chainsaw chainsaw in this.currentChainsaws)
			{
				chainsaw.lineStartTransform = this.chainsawAttachPoint;
			}
			this.chainsawAttachPoint.gameObject.SetActive(MonoSingleton<WeaponCharges>.Instance.shoSawCharge == 1f);
		}
	}

	// Token: 0x060016EA RID: 5866 RVA: 0x000B87CC File Offset: 0x000B69CC
	private void OnDisable()
	{
		if (this.impactRoutine != null)
		{
			if (this.hitGrenade != null)
			{
				this.HitNade();
			}
			if (this.hitEnemy != null)
			{
				this.DeliverDamage();
			}
			this.ImpactEffects();
		}
		this.impactRoutine = null;
		this.gunReady = false;
		this.primaryCharge = 0;
		this.chargingSwing = false;
		this.charging = false;
		this.swingCharge = 0f;
		this.hammerPullable.localPosition = this.hammerDefaultPosition;
		this.tier = 0;
		this.hammerCooldown = 0f;
		this.chargeForce = 0f;
		if (this.tempChargeSound != null)
		{
			Object.Destroy(this.tempChargeSound);
		}
		foreach (Chainsaw chainsaw in this.currentChainsaws)
		{
			chainsaw.lineStartTransform = MonoSingleton<NewMovement>.Instance.transform;
		}
		if (this.sawZone)
		{
			this.sawZone.enabled = false;
		}
		if (this.environmentalSawSound)
		{
			this.environmentalSawSound.Stop();
		}
		if (this.environmentalSawSpark)
		{
			this.environmentalSawSpark.Stop();
		}
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x000B891C File Offset: 0x000B6B1C
	private void UpdateMeter(bool forceUpdateTexture = false)
	{
		int num = 0;
		if (this.currentSpeed > 0.66f)
		{
			num = 2;
		}
		else if (this.currentSpeed > 0.33f)
		{
			num = 1;
		}
		if (MonoSingleton<HookArm>.Instance.beingPulled && this.tier == 2)
		{
			num = 1;
		}
		else if (num < this.tier)
		{
			if (this.tierDownTimer <= 0.5f)
			{
				num = this.tier;
			}
		}
		else
		{
			this.tierDownTimer = 0f;
		}
		this.meter.GetPropertyBlock(this.block, 1);
		if (this.tier != num || forceUpdateTexture)
		{
			this.block.SetTexture("_EmissiveTex", this.meterEmissives[num]);
			this.tier = num;
		}
		if (this.overheated)
		{
			this.block.SetFloat("_EmissiveIntensity", 0f);
		}
		else if (this.tierDownTimer > 0f)
		{
			this.block.SetFloat("_EmissiveIntensity", (float)((this.tierDownTimer % 0.1f >= 0.05f) ? 1 : 0));
		}
		else
		{
			this.block.SetFloat("_EmissiveIntensity", 1f);
		}
		this.meter.SetPropertyBlock(this.block, 1);
		if (this.variation != 0)
		{
			if (this.variation == 1)
			{
				if (this.timeToBeep != 0f)
				{
					this.timeToBeep = Mathf.MoveTowards(this.timeToBeep, 0f, Time.deltaTime * 5f);
				}
				this.secondaryMeterFill = (float)this.primaryCharge / 3f;
				if (this.primaryCharge == 3)
				{
					this.secondaryMeterFill = 1f;
					if (this.timeToBeep == 0f)
					{
						this.timeToBeep = 1f;
						Object.Instantiate<GameObject>(this.warningBeep);
						this.secondaryMeter.color = Color.red;
						return;
					}
					if (this.timeToBeep < 0.5f)
					{
						this.secondaryMeter.color = Color.black;
						return;
					}
				}
				else
				{
					if (this.primaryCharge == 1)
					{
						this.secondaryMeter.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[1];
						return;
					}
					if (this.primaryCharge == 2)
					{
						this.secondaryMeter.color = Color.yellow;
						return;
					}
				}
			}
			else if (this.variation == 2)
			{
				if (MonoSingleton<WeaponCharges>.Instance.shoSawCharge == 1f && !this.chainsawAttachPoint.gameObject.activeSelf)
				{
					this.chainsawAttachPoint.gameObject.SetActive(true);
				}
				if (this.charging)
				{
					this.secondaryMeterFill = this.chargeForce / 60f;
					this.secondaryMeter.color = Color.Lerp(MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation], new Color(1f, 0.25f, 0.25f), this.chargeForce / 60f);
					return;
				}
				this.secondaryMeterFill = MonoSingleton<WeaponCharges>.Instance.shoSawCharge;
				this.secondaryMeter.color = ((MonoSingleton<WeaponCharges>.Instance.shoSawCharge == 1f) ? MonoSingleton<ColorBlindSettings>.Instance.variationColors[2] : Color.white);
			}
			return;
		}
		this.secondaryMeterFill = MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge;
		if (this.secondaryMeterFill >= 1f)
		{
			this.secondaryMeter.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[0];
			return;
		}
		this.secondaryMeter.color = Color.white;
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x000B8C90 File Offset: 0x000B6E90
	private void Update()
	{
		this.overheated = MonoSingleton<WeaponCharges>.Instance.shoaltcooldowns[this.variation] > 0f;
		if (this.overheated && !this.overheatAud.isPlaying)
		{
			this.overheatAud.Play();
			this.overheatParticle.Play();
			this.anim.SetBool("Cooldown", true);
		}
		else if (!this.overheated && this.overheatAud.isPlaying)
		{
			this.overheatAud.Stop();
			this.overheatParticle.Stop();
			this.anim.SetBool("Cooldown", false);
		}
		float num = Mathf.Min(MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).magnitude / 60f, 1f);
		this.currentSpeed = (this.overheated ? 0f : Mathf.MoveTowards(this.currentSpeed, num, Time.deltaTime * 2f));
		if (MonoSingleton<HookArm>.Instance.beingPulled)
		{
			this.currentSpeed = Mathf.Min(this.currentSpeed, 0.5f);
		}
		this.UpdateMeter(false);
		if (this.pulledOut >= 0.5f)
		{
			this.gunReady = true;
		}
		if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && !this.chargingSwing && this.hammerCooldown <= 0f && !this.overheated && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && (!this.fireHeldOnPullOut || this.pulledOut >= 0.25f) && this.gc.activated)
		{
			this.fireHeldOnPullOut = false;
			this.chargingSwing = true;
		}
		else if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && !MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && this.swingCharge == 1f && this.gunReady && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			this.chargingSwing = false;
			this.swingCharge = 0f;
			if (!this.wid || this.wid.delay == 0f)
			{
				this.Impact();
			}
			else
			{
				this.gunReady = false;
				base.Invoke("Impact", this.wid.delay);
			}
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && this.variation == 2 && this.gunReady && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked && MonoSingleton<WeaponCharges>.Instance.shoSawCharge >= 1f)
		{
			this.charging = true;
			if (this.chargeForce < 60f)
			{
				this.chargeForce = Mathf.MoveTowards(this.chargeForce, 60f, Time.deltaTime * 60f);
			}
			float num2 = 12000f;
			base.transform.localPosition = new Vector3(this.wpos.currentDefault.x + Random.Range(this.chargeForce / num2 * -1f, this.chargeForce / num2), this.wpos.currentDefault.y + Random.Range(this.chargeForce / num2 * -1f, this.chargeForce / num2), this.wpos.currentDefault.z + Random.Range(this.chargeForce / num2 * -1f, this.chargeForce / num2));
			if (this.tempChargeSound == null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.chargeSoundBubble);
				this.tempChargeSound = gameObject.GetComponent<AudioSource>();
				if (this.wid && this.wid.delay > 0f)
				{
					this.tempChargeSound.volume -= this.wid.delay * 2f;
					if (this.tempChargeSound.volume < 0f)
					{
						this.tempChargeSound.volume = 0f;
					}
				}
			}
			MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.ShotgunCharge, this.tempChargeSound.gameObject).intensityMultiplier = this.chargeForce / 60f;
			this.tempChargeSound.pitch = (this.chargeForce / 2f + 30f) / 60f;
		}
		if (!MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && this.variation == 2 && this.gunReady && this.gc.activated && this.charging)
		{
			this.charging = false;
			MonoSingleton<WeaponCharges>.Instance.shoSawCharge = 0f;
			if (!this.wid || this.wid.delay == 0f)
			{
				this.ShootSaw();
			}
			else
			{
				this.gunReady = false;
				base.Invoke("ShootSaw", this.wid.delay);
			}
			if (this.tempChargeSound)
			{
				Object.Destroy(this.tempChargeSound.gameObject);
			}
		}
		if (this.variation == 2)
		{
			if (this.charging && this.chainsawBladeScroll.scrollSpeedX == 0f)
			{
				this.chainsawBladeRenderer.material = this.chainsawBladeMotionMaterial;
			}
			else if (!this.charging && this.chainsawBladeScroll.scrollSpeedX > 0f)
			{
				this.chainsawBladeRenderer.material = this.chainsawBladeMaterial;
			}
			this.chainsawBladeScroll.scrollSpeedX = this.chargeForce / 6f;
			this.anim.SetBool("Sawing", this.charging);
			this.sawZone.enabled = this.charging;
			RaycastHit raycastHit;
			if (this.charging && Physics.Raycast(MonoSingleton<CameraController>.Instance.GetDefaultPos(), MonoSingleton<CameraController>.Instance.transform.forward, out raycastHit, 3f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				this.environmentalSawSpark.transform.position = raycastHit.point;
				if (!this.environmentalSawSpark.isEmitting)
				{
					this.environmentalSawSpark.Play();
				}
				if (!this.environmentalSawSound.isPlaying)
				{
					this.environmentalSawSound.Play();
				}
				MonoSingleton<CameraController>.Instance.CameraShake(0.1f);
				if (this.enviroGibSpawnCooldown > 0.1f)
				{
					if (SceneHelper.IsStaticEnvironment(raycastHit))
					{
						MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(raycastHit, 3, 1f);
					}
					this.enviroGibSpawnCooldown = 0f;
				}
			}
			else
			{
				if (this.environmentalSawSpark.isEmitting)
				{
					this.environmentalSawSpark.Stop();
				}
				if (this.environmentalSawSound.isPlaying)
				{
					this.environmentalSawSound.Stop();
				}
			}
		}
		if (this.chargingSwing)
		{
			this.swingCharge = Mathf.MoveTowards(this.swingCharge, 1f, Time.deltaTime * 2f);
		}
		this.modelTransform.localPosition = new Vector3(this.defaultModelPosition.x + Random.Range(-this.swingCharge / 30f, this.swingCharge / 30f), this.defaultModelPosition.y + Random.Range(-this.swingCharge / 30f, this.swingCharge / 30f), this.defaultModelPosition.z + Random.Range(-this.swingCharge / 30f, this.swingCharge / 30f));
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && this.variation != 2 && (this.variation == 1 || MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge >= 1f) && !this.aboutToSecondary && this.gunReady && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			this.gunReady = false;
			if (!this.wid || this.wid.delay == 0f)
			{
				if (this.variation == 0)
				{
					this.ThrowNade();
				}
				else
				{
					this.Pump();
				}
			}
			else
			{
				this.aboutToSecondary = true;
				base.Invoke((this.variation == 0) ? "ThrowNade" : "Pump", this.wid.delay);
			}
		}
		if (this.secondaryMeterFill >= 1f)
		{
			this.secondaryMeter.fillAmount = 1f;
		}
		else if (this.secondaryMeterFill <= 0f)
		{
			this.secondaryMeter.fillAmount = 0f;
		}
		else
		{
			this.secondaryMeter.fillAmount = Mathf.Lerp(0.275f, 0.625f, this.secondaryMeterFill);
		}
		if (this.hammerCooldown > 0f)
		{
			this.hammerCooldown = Mathf.MoveTowards(this.hammerCooldown, 0f, Time.deltaTime);
		}
		if (MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge < 1f)
		{
			this.nadeCharging = true;
			return;
		}
		if (this.nadeCharging)
		{
			this.nadeCharging = false;
			Object.Instantiate<AudioSource>(this.nadeReadySound);
		}
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x000B95A8 File Offset: 0x000B77A8
	private void FixedUpdate()
	{
		float magnitude = MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).magnitude;
		if (magnitude >= this.storedSpeed - 5f || this.speedStorageTimer > 0.5f)
		{
			this.storedSpeed = magnitude;
			this.speedStorageTimer = 0f;
		}
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x000B9604 File Offset: 0x000B7804
	private void LateUpdate()
	{
		float num = Time.timeScale;
		if (this.impactRoutine != null)
		{
			num = 1f;
		}
		this.hammerPullable.localPosition = Vector3.Lerp(this.hammerDefaultPosition, this.hammerDefaultPosition + Vector3.up * -0.65f, this.swingCharge);
		this.hammerPullSound.volume = this.swingCharge * 0.6f;
		this.hammerPullSound.pitch = this.swingCharge + this.currentSpeed * 0.2f * num * Time.unscaledDeltaTime * 150f;
		if (this.overheated)
		{
			this.meterHand.localRotation = Quaternion.Euler(0f, 170f - MonoSingleton<WeaponCharges>.Instance.shoaltcooldowns[this.variation] / 7f * 200f, -90f);
		}
		else
		{
			this.meterHand.localRotation = Quaternion.Euler(0f, 170f - this.currentSpeed * 200f, -90f);
		}
		this.rotatingMotor.localRotation = this.motorPreviousRotation;
		this.rotatingMotor.Rotate(Vector3.up * this.currentSpeed * 10f * num * Time.unscaledDeltaTime * 150f, Space.Self);
		this.motorPreviousRotation = this.rotatingMotor.localRotation;
		this.motorSprite.color = new Color(1f, 1f, 1f, this.currentSpeed / 3f);
		this.motorSound.volume = this.currentSpeed / 2f;
		if (this.impactRoutine == null)
		{
			this.motorSound.pitch = this.currentSpeed * num;
			return;
		}
		this.motorSound.pitch = (float)(this.tier + 1);
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x000B97E5 File Offset: 0x000B79E5
	private void Impact()
	{
		this.impactRoutine = base.StartCoroutine(this.ImpactRoutine());
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x000B97F9 File Offset: 0x000B79F9
	private IEnumerator ImpactRoutine()
	{
		this.hitEnemy = null;
		this.hitGrenade = null;
		this.target = null;
		this.hitPosition = Vector3.zero;
		this.hammerCooldown = 0.5f;
		Vector3 position = MonoSingleton<CameraController>.Instance.GetDefaultPos();
		this.direction = MonoSingleton<CameraController>.Instance.transform.forward;
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			this.direction = (this.targeter.CurrentTarget.bounds.center - MonoSingleton<CameraController>.Instance.GetDefaultPos()).normalized;
		}
		if (MonoSingleton<ObjectTracker>.Instance.grenadeList.Count > 0 || MonoSingleton<WeaponCharges>.Instance.shoSawAmount > 0 || MonoSingleton<ObjectTracker>.Instance.landmineList.Count > 0)
		{
			Collider[] cols = Physics.OverlapSphere(position, 0.01f);
			if (cols.Length != 0)
			{
				int num;
				for (int i = 0; i < cols.Length; i = num + 1)
				{
					Transform transform = cols[i].transform;
					ParryHelper parryHelper;
					if (transform.TryGetComponent<ParryHelper>(out parryHelper))
					{
						transform = parryHelper.target;
					}
					if (MonoSingleton<ObjectTracker>.Instance.grenadeList.Count > 0 && transform.gameObject.layer == 10)
					{
						Grenade componentInParent = transform.GetComponentInParent<Grenade>();
						if (componentInParent)
						{
							this.hitGrenade = componentInParent;
							cols[i].enabled = false;
							Object.Instantiate<AudioSource>(this.hitSound, base.transform.position, Quaternion.identity);
							MonoSingleton<TimeController>.Instance.TrueStop(0.25f);
							yield return new WaitForSeconds(0.01f);
							this.HitNade();
						}
					}
					else if (MonoSingleton<WeaponCharges>.Instance.shoSawAmount > 0 || MonoSingleton<ObjectTracker>.Instance.landmineList.Count > 0)
					{
						Chainsaw chainsaw;
						Landmine landmine;
						if (MonoSingleton<WeaponCharges>.Instance.shoSawAmount > 0 && transform.TryGetComponent<Chainsaw>(out chainsaw))
						{
							Object.Instantiate<AudioSource>(this.hitSound, base.transform.position, Quaternion.identity);
							chainsaw.GetPunched();
							chainsaw.transform.position = MonoSingleton<CameraController>.Instance.GetDefaultPos() + this.direction;
							chainsaw.rb.velocity = (Punch.GetParryLookTarget() - chainsaw.transform.position).normalized * 105f;
						}
						else if (MonoSingleton<ObjectTracker>.Instance.landmineList.Count > 0 && transform.TryGetComponent<Landmine>(out landmine))
						{
							landmine.transform.LookAt(Punch.GetParryLookTarget());
							landmine.Parry();
							Object.Instantiate<AudioSource>(this.hitSound, base.transform.position, Quaternion.identity);
							this.anim.Play("Fire", -1, 0f);
							MonoSingleton<TimeController>.Instance.TrueStop(0.25f);
							yield return new WaitForSeconds(0.01f);
						}
					}
					num = i;
				}
			}
			cols = null;
		}
		if (MonoSingleton<WeaponCharges>.Instance.shoSawAmount > 0 || MonoSingleton<ObjectTracker>.Instance.landmineList.Count > 0)
		{
			RaycastHit[] rhits = Physics.RaycastAll(position, this.direction, 8f, 16384, QueryTriggerInteraction.Collide);
			int num;
			for (int i = 0; i < rhits.Length; i = num + 1)
			{
				Transform transform2 = rhits[i].transform;
				ParryHelper parryHelper2;
				if (transform2.TryGetComponent<ParryHelper>(out parryHelper2))
				{
					transform2 = parryHelper2.target;
				}
				Chainsaw chainsaw2;
				Landmine landmine2;
				if (transform2.TryGetComponent<Chainsaw>(out chainsaw2))
				{
					Object.Instantiate<AudioSource>(this.hitSound, base.transform.position, Quaternion.identity);
					chainsaw2.GetPunched();
					chainsaw2.transform.position = MonoSingleton<CameraController>.Instance.GetDefaultPos() + this.direction;
					chainsaw2.rb.velocity = (Punch.GetParryLookTarget() - chainsaw2.transform.position).normalized * 105f;
				}
				else if (transform2.TryGetComponent<Landmine>(out landmine2))
				{
					landmine2.transform.LookAt(Punch.GetParryLookTarget());
					landmine2.Parry();
					Object.Instantiate<AudioSource>(this.hitSound, base.transform.position, Quaternion.identity);
					this.anim.Play("Fire", -1, 0f);
					MonoSingleton<TimeController>.Instance.TrueStop(0.25f);
					yield return new WaitForSeconds(0.01f);
				}
				num = i;
			}
			rhits = null;
		}
		RaycastHit rhit;
		if (Physics.Raycast(position, this.direction, out rhit, 8f, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment), QueryTriggerInteraction.Collide))
		{
			if (rhit.transform.gameObject.layer == 11 || rhit.transform.gameObject.layer == 10)
			{
				ParryHelper parryHelper3;
				EnemyIdentifierIdentifier enemyIdentifierIdentifier2;
				if (rhit.transform.gameObject.TryGetComponent<ParryHelper>(out parryHelper3))
				{
					EnemyIdentifierIdentifier enemyIdentifierIdentifier;
					EnemyIdentifier enemyIdentifier;
					if (parryHelper3.target.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && !enemyIdentifierIdentifier.eid.dead)
					{
						this.hitEnemy = enemyIdentifierIdentifier.eid;
					}
					else if (parryHelper3.target.TryGetComponent<EnemyIdentifier>(out enemyIdentifier) && !enemyIdentifier.dead)
					{
						this.hitEnemy = enemyIdentifier;
					}
				}
				else if (rhit.transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier2) && enemyIdentifierIdentifier2.eid && !enemyIdentifierIdentifier2.eid.dead)
				{
					this.hitEnemy = enemyIdentifierIdentifier2.eid;
				}
				else if (MonoSingleton<ObjectTracker>.Instance.grenadeList.Count > 0 && rhit.transform.gameObject.layer == 10)
				{
					Grenade componentInParent2 = rhit.transform.GetComponentInParent<Grenade>();
					if (componentInParent2)
					{
						this.hitGrenade = componentInParent2;
						rhit.collider.enabled = false;
						Object.Instantiate<AudioSource>(this.hitSound, base.transform.position, Quaternion.identity);
						this.anim.Play("Fire", -1, 0f);
						MonoSingleton<TimeController>.Instance.TrueStop(0.25f);
						yield return new WaitForSeconds(0.01f);
						this.HitNade();
					}
				}
			}
			this.target = rhit.transform;
			this.hitPosition = rhit.point;
		}
		if (this.hitEnemy == null)
		{
			Vector3 vector = position + this.direction * 2.5f;
			Collider[] array = Physics.OverlapSphere(vector, 2.5f);
			if (array.Length != 0)
			{
				float num2 = 2.5f;
				for (int j = 0; j < array.Length; j++)
				{
					ParryHelper parryHelper4;
					Collider collider;
					if (array[j].TryGetComponent<ParryHelper>(out parryHelper4) && parryHelper4.target.TryGetComponent<Collider>(out collider))
					{
						array[j] = collider;
					}
					if (array[j].gameObject.layer == 10 || array[j].gameObject.layer == 11)
					{
						Vector3 vector2 = array[j].ClosestPoint(vector);
						if (!Physics.Raycast(position, vector2 - position, out rhit, Vector3.Distance(vector2, position), LayerMaskDefaults.Get(LMD.Environment)))
						{
							float num3 = Vector3.Distance(vector, vector2);
							if (num3 < num2)
							{
								Transform transform3 = ((array[j].attachedRigidbody != null) ? array[j].attachedRigidbody.transform : array[j].transform);
								EnemyIdentifier enemyIdentifier2 = null;
								EnemyIdentifierIdentifier enemyIdentifierIdentifier3;
								if (transform3.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier3))
								{
									enemyIdentifier2 = enemyIdentifierIdentifier3.eid;
								}
								else
								{
									transform3.TryGetComponent<EnemyIdentifier>(out enemyIdentifier2);
								}
								if (enemyIdentifier2 && (!enemyIdentifier2.dead || this.hitEnemy == null))
								{
									this.hitEnemy = enemyIdentifierIdentifier3.eid;
									num2 = num3;
									this.target = transform3;
									this.hitPosition = vector2;
								}
							}
						}
					}
				}
			}
			RaycastHit[] array2 = Physics.SphereCastAll(position + this.direction * 2.5f, 2.5f, this.direction, 3f, LayerMaskDefaults.Get(LMD.Enemies));
			if (array2.Length != 0)
			{
				float num4 = -1f;
				if (this.hitEnemy != null)
				{
					num4 = Vector3.Dot(this.direction, this.hitPosition - position);
				}
				for (int k = 0; k < array2.Length; k++)
				{
					if (!Physics.Raycast(position, array2[k].point - position, out rhit, Vector3.Distance(array2[k].point, position), LayerMaskDefaults.Get(LMD.Environment)))
					{
						float num5 = Vector3.Dot(this.direction, array2[k].point - position);
						if (num5 > num4)
						{
							Transform transform4 = array2[k].transform;
							Vector3 point = array2[k].point;
							ParryHelper parryHelper5;
							if (transform4.TryGetComponent<ParryHelper>(out parryHelper5))
							{
								transform4 = parryHelper5.target.transform;
							}
							EnemyIdentifier enemyIdentifier3 = null;
							EnemyIdentifierIdentifier enemyIdentifierIdentifier4;
							if (transform4.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier4))
							{
								enemyIdentifier3 = enemyIdentifierIdentifier4.eid;
							}
							else
							{
								transform4.TryGetComponent<EnemyIdentifier>(out enemyIdentifier3);
							}
							if (enemyIdentifier3 && (!enemyIdentifier3.dead || this.hitEnemy == null))
							{
								this.hitEnemy = enemyIdentifier3;
								num4 = num5;
								this.target = transform4;
								this.hitPosition = point;
							}
						}
					}
				}
			}
		}
		this.forceWeakHit = true;
		if (this.target != null)
		{
			Breakable breakable;
			Glass glass;
			if (this.hitEnemy != null)
			{
				float num6 = 0.05f;
				this.damage = 3f;
				if (this.tier == 2)
				{
					num6 = 0.5f;
					this.damage = 10f;
				}
				else if (this.tier == 1)
				{
					num6 = 0.25f;
					this.damage = 6f;
				}
				if (this.hitEnemy.dead)
				{
					num6 = 0f;
				}
				if (num6 > 0f)
				{
					this.forceWeakHit = false;
					this.launchPlayer = true;
					Object.Instantiate<AudioSource>(this.hitSound, base.transform.position, Quaternion.identity);
					MonoSingleton<TimeController>.Instance.TrueStop(num6);
					yield return new WaitForSeconds(0.01f);
				}
				else
				{
					this.launchPlayer = false;
				}
				this.DeliverDamage();
			}
			else if (this.target.TryGetComponent<Breakable>(out breakable) && !breakable.precisionOnly && !breakable.specialCaseOnly && !breakable.unbreakable)
			{
				breakable.Break();
			}
			else if (this.target.TryGetComponent<Glass>(out glass) && !glass.broken)
			{
				glass.Shatter();
			}
		}
		if (!this.hitGrenade && ((this.hitEnemy == null && this.target != null && LayerMaskDefaults.IsMatchingLayer(this.target.gameObject.layer, LMD.Environment)) || (this.hitEnemy && this.hitEnemy.dead)))
		{
			MonoSingleton<NewMovement>.Instance.Launch(-this.direction * ((float)(100 * this.tier + 300) / ((float)(MonoSingleton<NewMovement>.Instance.hammerJumps + 3) / 3f)), 8f, false);
			MonoSingleton<NewMovement>.Instance.hammerJumps++;
			MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(position, this.direction, 8f, 10, 2f);
		}
		this.ImpactEffects();
		this.hitEnemy = null;
		this.hitGrenade = null;
		this.impactRoutine = null;
		yield break;
	}

	// Token: 0x060016F1 RID: 5873 RVA: 0x000B9808 File Offset: 0x000B7A08
	private void DeliverDamage()
	{
		this.direction = MonoSingleton<CameraController>.Instance.transform.forward;
		if (this.launchPlayer)
		{
			MonoSingleton<NewMovement>.Instance.Launch(-this.direction * (float)(300 * this.tier + 100), 8f, false);
			if (MonoSingleton<NewMovement>.Instance.rb.velocity.magnitude < this.storedSpeed)
			{
				MonoSingleton<NewMovement>.Instance.rb.velocity = -this.direction * this.storedSpeed;
			}
		}
		if (!this.hitEnemy.hitterWeapons.Contains("hammer" + this.variation.ToString()))
		{
			this.hitEnemy.hitterWeapons.Add("hammer" + this.variation.ToString());
		}
		bool dead = this.hitEnemy.dead;
		if (this.target.gameObject.CompareTag("Body"))
		{
			this.hitEnemy.hitter = "hammerzone";
			this.hitEnemy.DeliverDamage(this.target.gameObject, this.direction * (float)(50000 * this.tier + 50000), this.hitPosition, 4f, true, 0f, base.gameObject, false, false);
		}
		this.hitEnemy.hitter = "hammer";
		this.hitEnemy.DeliverDamage(this.target.gameObject, this.direction * (float)(50000 * this.tier + 50000), this.hitPosition, this.damage, true, 0f, base.gameObject, false, false);
		if (!dead)
		{
			if (this.hitEnemy.dead)
			{
				if (this.tier == 2)
				{
					MonoSingleton<StyleHUD>.Instance.AddPoints(240, "ultrakill.hammerhitred", base.gameObject, this.hitEnemy, -1, "", "");
				}
				else if (this.tier == 1)
				{
					MonoSingleton<StyleHUD>.Instance.AddPoints(120, "ultrakill.hammerhityellow", base.gameObject, this.hitEnemy, -1, "", "");
				}
				else
				{
					MonoSingleton<StyleHUD>.Instance.AddPoints(80, "ultrakill.hammerhitgreen", base.gameObject, this.hitEnemy, -1, "", "");
				}
			}
			else if (this.tier == 2)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(120, "ultrakill.hammerhitheavy", base.gameObject, this.hitEnemy, -1, "", "");
			}
			else
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(40 * (this.tier + 1), "ultrakill.hammerhit", base.gameObject, this.hitEnemy, -1, "", "");
			}
		}
		if (this.tier == 2 && !dead && this.hitEnemy.enemyType != EnemyType.Idol)
		{
			MonoSingleton<WeaponCharges>.Instance.shoaltcooldowns[this.variation] = 7f;
		}
	}

	// Token: 0x060016F2 RID: 5874 RVA: 0x000B9B14 File Offset: 0x000B7D14
	private void HitNade()
	{
		this.direction = MonoSingleton<CameraController>.Instance.transform.forward;
		RaycastHit raycastHit;
		if (Physics.Raycast(MonoSingleton<CameraController>.Instance.GetDefaultPos(), this.direction, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment), QueryTriggerInteraction.Ignore))
		{
			this.hitGrenade.GrenadeBeam(raycastHit.point, base.gameObject);
			return;
		}
		this.hitGrenade.GrenadeBeam(MonoSingleton<CameraController>.Instance.GetDefaultPos() + this.direction * 1000f, base.gameObject);
	}

	// Token: 0x060016F3 RID: 5875 RVA: 0x000B9BAC File Offset: 0x000B7DAC
	private void ImpactEffects()
	{
		Vector3 vector = ((this.hitPosition != Vector3.zero) ? (this.hitPosition - (this.hitPosition - MonoSingleton<CameraController>.Instance.GetDefaultPos()).normalized) : (MonoSingleton<CameraController>.Instance.GetDefaultPos() + this.direction * 2.5f));
		if (this.primaryCharge > 0)
		{
			GameObject gameObject = Object.Instantiate<GameObject>((this.primaryCharge == 3) ? this.overPumpExplosion : this.pumpExplosion, vector, Quaternion.LookRotation(this.direction));
			foreach (Explosion explosion in gameObject.GetComponentsInChildren<Explosion>())
			{
				explosion.sourceWeapon = base.gameObject;
				explosion.hitterWeapon = "hammer";
				if (this.primaryCharge == 2)
				{
					explosion.maxSize *= 2f;
				}
			}
			AudioSource audioSource;
			if (this.primaryCharge == 2 && gameObject.TryGetComponent<AudioSource>(out audioSource))
			{
				audioSource.volume = 1f;
				audioSource.pitch -= 0.4f;
			}
			this.primaryCharge = 0;
		}
		if (this.forceWeakHit || this.tier == 0)
		{
			this.anim.Play("Fire", -1, 0f);
		}
		else if (this.tier == 1)
		{
			this.anim.Play("FireStrong", -1, 0f);
		}
		else
		{
			this.anim.Play("FireStrongest", -1, 0f);
		}
		Object.Instantiate<GameObject>(this.hitImpactParticle[this.forceWeakHit ? 0 : this.tier], vector, MonoSingleton<CameraController>.Instance.transform.rotation);
	}

	// Token: 0x060016F4 RID: 5876 RVA: 0x000B9D68 File Offset: 0x000B7F68
	private void ThrowNade()
	{
		MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge = 0f;
		this.pulledOut = 0.3f;
		this.gunReady = false;
		this.aboutToSecondary = false;
		Vector3 vector = MonoSingleton<CameraController>.Instance.transform.forward;
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			vector = (this.targeter.CurrentTarget.bounds.center - MonoSingleton<CameraController>.Instance.GetDefaultPos()).normalized;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.grenade, MonoSingleton<CameraController>.Instance.GetDefaultPos() + vector * 2f - MonoSingleton<CameraController>.Instance.transform.up * 0.5f, Random.rotation);
		Rigidbody rigidbody;
		if (gameObject.TryGetComponent<Rigidbody>(out rigidbody))
		{
			rigidbody.AddForce(MonoSingleton<CameraController>.Instance.transform.forward * 3f + Vector3.up * 7.5f + (MonoSingleton<NewMovement>.Instance.ridingRocket ? MonoSingleton<NewMovement>.Instance.ridingRocket.rb.velocity : MonoSingleton<NewMovement>.Instance.rb.velocity), ForceMode.VelocityChange);
		}
		Grenade grenade;
		if (gameObject.TryGetComponent<Grenade>(out grenade))
		{
			grenade.sourceWeapon = base.gameObject;
		}
		this.anim.Play("NadeSpawn", -1, 0f);
		Object.Instantiate<AudioSource>(this.nadeSpawnSound);
	}

	// Token: 0x060016F5 RID: 5877 RVA: 0x000B9EFC File Offset: 0x000B80FC
	private void ShootSaw()
	{
		this.gunReady = true;
		base.transform.localPosition = this.wpos.currentDefault;
		Vector3 vector = MonoSingleton<CameraController>.Instance.transform.forward;
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			vector = (this.targeter.CurrentTarget.bounds.center - MonoSingleton<CameraController>.Instance.GetDefaultPos()).normalized;
		}
		Vector3 vector2 = MonoSingleton<CameraController>.Instance.GetDefaultPos() + vector * 0.5f;
		RaycastHit raycastHit;
		if (Physics.Raycast(MonoSingleton<CameraController>.Instance.GetDefaultPos(), vector, out raycastHit, 5f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
		{
			vector2 = raycastHit.point - vector * 5f;
		}
		Chainsaw chainsaw = Object.Instantiate<Chainsaw>(this.chainsaw, vector2, Random.rotation);
		chainsaw.weaponType = "hammer" + this.variation.ToString();
		chainsaw.CheckMultipleRicochets(true);
		chainsaw.sourceWeapon = this.gc.currentWeapon;
		chainsaw.attachedTransform = MonoSingleton<PlayerTracker>.Instance.GetTarget();
		chainsaw.lineStartTransform = this.chainsawAttachPoint;
		chainsaw.GetComponent<Rigidbody>().AddForce(vector * (this.chargeForce + 10f) * 1.5f, ForceMode.VelocityChange);
		this.currentChainsaws.Add(chainsaw);
		this.chainsawBladeRenderer.material = this.chainsawBladeMaterial;
		this.chainsawBladeScroll.scrollSpeedX = 0f;
		this.chainsawAttachPoint.gameObject.SetActive(false);
		Object.Instantiate<AudioSource>(this.nadeSpawnSound);
		this.anim.Play("SawingShot");
		MonoSingleton<CameraController>.Instance.CameraShake(1f);
		this.chargeForce = 0f;
	}

	// Token: 0x060016F6 RID: 5878 RVA: 0x000BA0DC File Offset: 0x000B82DC
	private void Pump()
	{
		if (this.primaryCharge < 3)
		{
			this.primaryCharge++;
		}
		this.pulledOut = 0f;
		this.gunReady = false;
		this.aboutToSecondary = false;
		AudioSource component = Object.Instantiate<GameObject>(this.pumpChargeSound, base.transform.position, Quaternion.identity).GetComponent<AudioSource>();
		float num = (float)this.primaryCharge;
		component.pitch = 1f + num / 5f;
		component.Play();
		Object.Instantiate<AudioSource>(this.pump1Sound);
		this.anim.Play("Pump", -1, 0f);
	}

	// Token: 0x060016F7 RID: 5879 RVA: 0x000BA180 File Offset: 0x000B8380
	private void Pump2Sound()
	{
		Object.Instantiate<AudioSource>(this.pump2Sound);
	}

	// Token: 0x04001FCB RID: 8139
	private WeaponIdentifier wid;

	// Token: 0x04001FCC RID: 8140
	public int variation;

	// Token: 0x04001FCD RID: 8141
	private GunControl gc;

	// Token: 0x04001FCE RID: 8142
	private bool gunReady;

	// Token: 0x04001FCF RID: 8143
	private WeaponPos wpos;

	// Token: 0x04001FD0 RID: 8144
	private CameraFrustumTargeter targeter;

	// Token: 0x04001FD1 RID: 8145
	private Animator anim;

	// Token: 0x04001FD2 RID: 8146
	[HideInInspector]
	public int primaryCharge;

	// Token: 0x04001FD3 RID: 8147
	public GameObject pumpChargeSound;

	// Token: 0x04001FD4 RID: 8148
	public GameObject warningBeep;

	// Token: 0x04001FD5 RID: 8149
	private float timeToBeep;

	// Token: 0x04001FD6 RID: 8150
	private bool chargingSwing;

	// Token: 0x04001FD7 RID: 8151
	private float swingCharge;

	// Token: 0x04001FD8 RID: 8152
	[SerializeField]
	private Transform modelTransform;

	// Token: 0x04001FD9 RID: 8153
	[HideInInspector]
	public Vector3 defaultModelPosition;

	// Token: 0x04001FDA RID: 8154
	[SerializeField]
	private Transform hammerPullable;

	// Token: 0x04001FDB RID: 8155
	[SerializeField]
	private AudioSource hammerPullSound;

	// Token: 0x04001FDC RID: 8156
	[HideInInspector]
	public Vector3 hammerDefaultPosition;

	// Token: 0x04001FDD RID: 8157
	private TimeSince pulledOut;

	// Token: 0x04001FDE RID: 8158
	private bool fireHeldOnPullOut;

	// Token: 0x04001FDF RID: 8159
	private float hammerCooldown;

	// Token: 0x04001FE0 RID: 8160
	[SerializeField]
	private Transform rotatingMotor;

	// Token: 0x04001FE1 RID: 8161
	private Quaternion motorPreviousRotation;

	// Token: 0x04001FE2 RID: 8162
	[SerializeField]
	private SpriteRenderer motorSprite;

	// Token: 0x04001FE3 RID: 8163
	[SerializeField]
	private AudioSource motorSound;

	// Token: 0x04001FE4 RID: 8164
	private bool overheated;

	// Token: 0x04001FE5 RID: 8165
	[SerializeField]
	private ParticleSystem overheatParticle;

	// Token: 0x04001FE6 RID: 8166
	[SerializeField]
	private AudioSource overheatAud;

	// Token: 0x04001FE7 RID: 8167
	private float currentSpeed;

	// Token: 0x04001FE8 RID: 8168
	[SerializeField]
	private Renderer meter;

	// Token: 0x04001FE9 RID: 8169
	[SerializeField]
	private Texture[] meterEmissives;

	// Token: 0x04001FEA RID: 8170
	private int tier;

	// Token: 0x04001FEB RID: 8171
	private MaterialPropertyBlock block;

	// Token: 0x04001FEC RID: 8172
	[SerializeField]
	private Transform meterHand;

	// Token: 0x04001FED RID: 8173
	private TimeSince tierDownTimer;

	// Token: 0x04001FEE RID: 8174
	[SerializeField]
	private Image secondaryMeter;

	// Token: 0x04001FEF RID: 8175
	private float secondaryMeterFill;

	// Token: 0x04001FF0 RID: 8176
	[SerializeField]
	private AudioSource hitSound;

	// Token: 0x04001FF1 RID: 8177
	[SerializeField]
	private GameObject[] hitImpactParticle;

	// Token: 0x04001FF2 RID: 8178
	private Coroutine impactRoutine;

	// Token: 0x04001FF3 RID: 8179
	private float storedSpeed;

	// Token: 0x04001FF4 RID: 8180
	private TimeSince speedStorageTimer;

	// Token: 0x04001FF5 RID: 8181
	[Header("Core Eject")]
	[SerializeField]
	private GameObject grenade;

	// Token: 0x04001FF6 RID: 8182
	[SerializeField]
	private AudioSource nadeSpawnSound;

	// Token: 0x04001FF7 RID: 8183
	[SerializeField]
	private AudioSource nadeReadySound;

	// Token: 0x04001FF8 RID: 8184
	private bool nadeCharging;

	// Token: 0x04001FF9 RID: 8185
	[Header("Pump Charge")]
	[SerializeField]
	private AudioSource pump1Sound;

	// Token: 0x04001FFA RID: 8186
	[SerializeField]
	private AudioSource pump2Sound;

	// Token: 0x04001FFB RID: 8187
	[SerializeField]
	private GameObject pumpExplosion;

	// Token: 0x04001FFC RID: 8188
	[SerializeField]
	private GameObject overPumpExplosion;

	// Token: 0x04001FFD RID: 8189
	private bool aboutToSecondary;

	// Token: 0x04001FFE RID: 8190
	[Header("Chainsaw")]
	public GameObject chargeSoundBubble;

	// Token: 0x04001FFF RID: 8191
	private AudioSource tempChargeSound;

	// Token: 0x04002000 RID: 8192
	private bool charging;

	// Token: 0x04002001 RID: 8193
	private float chargeForce;

	// Token: 0x04002002 RID: 8194
	[SerializeField]
	private Chainsaw chainsaw;

	// Token: 0x04002003 RID: 8195
	private List<Chainsaw> currentChainsaws = new List<Chainsaw>();

	// Token: 0x04002004 RID: 8196
	[SerializeField]
	private Transform chainsawAttachPoint;

	// Token: 0x04002005 RID: 8197
	[SerializeField]
	private ScrollingTexture chainsawBladeScroll;

	// Token: 0x04002006 RID: 8198
	private MeshRenderer chainsawBladeRenderer;

	// Token: 0x04002007 RID: 8199
	private Material chainsawBladeMaterial;

	// Token: 0x04002008 RID: 8200
	[SerializeField]
	private Material chainsawBladeMotionMaterial;

	// Token: 0x04002009 RID: 8201
	[SerializeField]
	private HurtZone sawZone;

	// Token: 0x0400200A RID: 8202
	[SerializeField]
	private ParticleSystem environmentalSawSpark;

	// Token: 0x0400200B RID: 8203
	[SerializeField]
	private AudioSource environmentalSawSound;

	// Token: 0x0400200C RID: 8204
	private TimeSince enviroGibSpawnCooldown;

	// Token: 0x0400200D RID: 8205
	private bool launchPlayer;

	// Token: 0x0400200E RID: 8206
	private EnemyIdentifier hitEnemy;

	// Token: 0x0400200F RID: 8207
	private Vector3 direction;

	// Token: 0x04002010 RID: 8208
	private Transform target;

	// Token: 0x04002011 RID: 8209
	private Vector3 hitPosition;

	// Token: 0x04002012 RID: 8210
	private float damage;

	// Token: 0x04002013 RID: 8211
	private bool forceWeakHit;

	// Token: 0x04002014 RID: 8212
	private Grenade hitGrenade;
}
