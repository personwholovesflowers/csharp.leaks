using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003F9 RID: 1017
public class Shotgun : MonoBehaviour
{
	// Token: 0x060016CE RID: 5838 RVA: 0x000B6971 File Offset: 0x000B4B71
	private void Awake()
	{
		this.chargeSlider = base.GetComponentInChildren<Slider>();
		this.sliderFill = this.chargeSlider.GetComponentInChildren<Image>();
	}

	// Token: 0x060016CF RID: 5839 RVA: 0x000B6990 File Offset: 0x000B4B90
	private void Start()
	{
		this.targeter = MonoSingleton<CameraController>.Instance.GetComponent<CameraFrustumTargeter>();
		this.inman = MonoSingleton<InputManager>.Instance;
		this.wid = base.GetComponent<WeaponIdentifier>();
		this.gunAud = base.GetComponent<AudioSource>();
		this.anim = base.GetComponentInChildren<Animator>();
		this.cam = MonoSingleton<CameraController>.Instance.gameObject;
		this.cc = MonoSingleton<CameraController>.Instance;
		this.gc = base.GetComponentInParent<GunControl>();
		this.tempColor = this.heatSinkSMR.materials[3].GetColor("_TintColor");
		this.heatSinkAud = this.heatSinkSMR.GetComponent<AudioSource>();
		if (this.variation == 0)
		{
			this.chargeSlider.value = this.chargeSlider.maxValue;
		}
		else if (this.variation == 1)
		{
			this.chargeSlider.value = 0f;
		}
		this.wpos = base.GetComponent<WeaponPos>();
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

	// Token: 0x060016D0 RID: 5840 RVA: 0x000B6AC4 File Offset: 0x000B4CC4
	private void OnEnable()
	{
		if (this.variation == 2)
		{
			foreach (Chainsaw chainsaw in this.currentChainsaws)
			{
				chainsaw.lineStartTransform = this.chainsawAttachPoint;
			}
			this.chainsawAttachPoint.gameObject.SetActive(MonoSingleton<WeaponCharges>.Instance.shoSawCharge == 1f);
		}
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x000B6B44 File Offset: 0x000B4D44
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		if (this.anim == null)
		{
			this.anim = base.GetComponentInChildren<Animator>();
		}
		this.anim.StopPlayback();
		this.gunReady = false;
		if (this.sliderFill != null && MonoSingleton<ColorBlindSettings>.Instance)
		{
			this.sliderFill.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation];
		}
		if (this.chargeSlider == null)
		{
			this.chargeSlider = base.GetComponentInChildren<Slider>();
		}
		if (this.variation == 0)
		{
			this.chargeSlider.value = this.chargeSlider.maxValue;
		}
		else if (this.variation == 1)
		{
			this.chargeSlider.value = 0f;
		}
		if (this.sliderFill == null)
		{
			this.sliderFill = this.chargeSlider.GetComponentInChildren<Image>();
		}
		this.primaryCharge = 0;
		this.charging = false;
		this.grenadeForce = 0f;
		this.meterOverride = false;
		if (this.tempChargeSound != null)
		{
			Object.Destroy(this.tempChargeSound.gameObject);
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

	// Token: 0x060016D2 RID: 5842 RVA: 0x000B6D14 File Offset: 0x000B4F14
	private void Update()
	{
		if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && this.gunReady && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked && !this.charging)
		{
			if (!this.wid || this.wid.delay == 0f)
			{
				this.Shoot();
			}
			else
			{
				this.gunReady = false;
				base.Invoke("Shoot", this.wid.delay);
			}
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && this.variation == 1 && this.gunReady && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			this.gunReady = false;
			if (!this.wid || this.wid.delay == 0f)
			{
				this.Pump();
			}
			else
			{
				base.Invoke("Pump", this.wid.delay);
			}
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && this.variation != 1 && this.gunReady && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked && (this.variation != 2 || MonoSingleton<WeaponCharges>.Instance.shoSawCharge >= 1f))
		{
			this.charging = true;
			if (this.grenadeForce < 60f)
			{
				this.grenadeForce = Mathf.MoveTowards(this.grenadeForce, 60f, Time.deltaTime * 60f);
			}
			this.grenadeVector = new Vector3(this.cam.transform.forward.x, this.cam.transform.forward.y, this.cam.transform.forward.z);
			if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
			{
				this.grenadeVector = Vector3.Normalize(this.targeter.CurrentTarget.bounds.center - this.cam.transform.position);
			}
			this.grenadeVector += new Vector3(0f, this.grenadeForce * 0.002f, 0f);
			float num = 3000f;
			if (this.variation == 2)
			{
				num = 12000f;
			}
			base.transform.localPosition = new Vector3(this.wpos.currentDefault.x + Random.Range(this.grenadeForce / num * -1f, this.grenadeForce / num), this.wpos.currentDefault.y + Random.Range(this.grenadeForce / num * -1f, this.grenadeForce / num), this.wpos.currentDefault.z + Random.Range(this.grenadeForce / num * -1f, this.grenadeForce / num));
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
			MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.ShotgunCharge, this.tempChargeSound.gameObject).intensityMultiplier = this.grenadeForce / 60f;
			if (this.variation == 0)
			{
				this.tempChargeSound.pitch = this.grenadeForce / 60f;
			}
			else
			{
				this.tempChargeSound.pitch = (this.grenadeForce / 2f + 30f) / 60f;
			}
		}
		if ((MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasCanceledThisFrame || (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && !GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame)) && this.variation != 1 && this.gunReady && this.gc.activated && this.charging)
		{
			this.charging = false;
			if (this.variation == 2)
			{
				MonoSingleton<WeaponCharges>.Instance.shoSawCharge = 0f;
			}
			if (!this.wid || this.wid.delay == 0f)
			{
				if (this.variation == 0)
				{
					this.ShootSinks();
				}
				else
				{
					this.ShootSaw();
				}
			}
			else
			{
				this.gunReady = false;
				base.Invoke((this.variation == 0) ? "ShootSinks" : "ShootSaw", this.wid.delay);
			}
			Object.Destroy(this.tempChargeSound.gameObject);
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
			this.chainsawBladeScroll.scrollSpeedX = this.grenadeForce / 6f;
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
		if (this.releasingHeat)
		{
			this.tempColor.a = this.tempColor.a - Time.deltaTime * 2.5f;
			this.heatSinkSMR.sharedMaterials[3].SetColor("_TintColor", this.tempColor);
		}
		this.UpdateMeter();
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x000B7444 File Offset: 0x000B5644
	private void UpdateMeter()
	{
		if (this.variation == 1)
		{
			if (this.timeToBeep != 0f)
			{
				this.timeToBeep = Mathf.MoveTowards(this.timeToBeep, 0f, Time.deltaTime * 5f);
			}
			if (this.primaryCharge != 3)
			{
				this.chargeSlider.value = (float)(this.primaryCharge * 20);
				this.sliderFill.color = Color.Lerp(MonoSingleton<ColorBlindSettings>.Instance.variationColors[1], new Color(1f, 0.25f, 0.25f), (float)this.primaryCharge / 2f);
				return;
			}
			this.chargeSlider.value = this.chargeSlider.maxValue;
			if (this.timeToBeep == 0f)
			{
				this.timeToBeep = 1f;
				Object.Instantiate<GameObject>(this.warningBeep);
				this.sliderFill.color = Color.red;
				return;
			}
			if (this.timeToBeep < 0.5f)
			{
				this.sliderFill.color = Color.black;
				return;
			}
		}
		else if (!this.meterOverride)
		{
			if (this.variation == 2 && MonoSingleton<WeaponCharges>.Instance.shoSawCharge == 1f && !this.chainsawAttachPoint.gameObject.activeSelf)
			{
				this.chainsawAttachPoint.gameObject.SetActive(true);
			}
			if (this.grenadeForce > 0f)
			{
				this.chargeSlider.value = this.grenadeForce;
				this.sliderFill.color = Color.Lerp(MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation], new Color(1f, 0.25f, 0.25f), this.grenadeForce / 60f);
				return;
			}
			if (this.variation == 0)
			{
				this.chargeSlider.value = this.chargeSlider.maxValue;
				this.sliderFill.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[0];
				return;
			}
			this.chargeSlider.value = MonoSingleton<WeaponCharges>.Instance.shoSawCharge * this.chargeSlider.maxValue;
			this.sliderFill.color = ((MonoSingleton<WeaponCharges>.Instance.shoSawCharge == 1f) ? MonoSingleton<ColorBlindSettings>.Instance.variationColors[2] : Color.gray);
		}
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x000B7694 File Offset: 0x000B5894
	private void Shoot()
	{
		this.gunReady = false;
		int num = 12;
		if (this.variation == 1)
		{
			switch (this.primaryCharge)
			{
			case 0:
				num = 10;
				this.gunAud.pitch = Random.Range(1.15f, 1.25f);
				break;
			case 1:
				num = 16;
				this.gunAud.pitch = Random.Range(0.95f, 1.05f);
				break;
			case 2:
				num = 24;
				this.gunAud.pitch = Random.Range(0.75f, 0.85f);
				break;
			case 3:
				num = 0;
				this.gunAud.pitch = Random.Range(0.75f, 0.85f);
				break;
			}
		}
		MonoSingleton<CameraController>.Instance.StopShake();
		Vector3 vector = this.cam.transform.forward;
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			vector = (this.targeter.CurrentTarget.bounds.center - MonoSingleton<CameraController>.Instance.GetDefaultPos()).normalized;
		}
		this.rhits = Physics.RaycastAll(this.cam.transform.position, vector, 4f, LayerMaskDefaults.Get(LMD.Enemies));
		if (this.rhits.Length != 0)
		{
			foreach (RaycastHit raycastHit in this.rhits)
			{
				if (raycastHit.collider.gameObject.CompareTag("Body"))
				{
					EnemyIdentifierIdentifier componentInParent = raycastHit.collider.GetComponentInParent<EnemyIdentifierIdentifier>();
					if (componentInParent && componentInParent.eid)
					{
						EnemyIdentifier eid = componentInParent.eid;
						if (!eid.dead && !eid.blessed && this.anim.GetCurrentAnimatorStateInfo(0).IsName("Equip"))
						{
							MonoSingleton<StyleHUD>.Instance.AddPoints(50, "ultrakill.quickdraw", this.gc.currentWeapon, eid, -1, "", "");
						}
						eid.hitter = "shotgunzone";
						if (!eid.hitterWeapons.Contains("shotgun" + this.variation.ToString()))
						{
							eid.hitterWeapons.Add("shotgun" + this.variation.ToString());
						}
						eid.DeliverDamage(raycastHit.collider.gameObject, (eid.transform.position - base.transform.position).normalized * 10000f, raycastHit.point, 4f, false, 0f, base.gameObject, false, false);
					}
				}
			}
		}
		MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFireProjectiles, base.gameObject);
		if (this.variation != 1 || this.primaryCharge != 3)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.bullet, this.cam.transform.position, this.cam.transform.rotation);
				Projectile component = gameObject.GetComponent<Projectile>();
				component.weaponType = "shotgun" + this.variation.ToString();
				component.sourceWeapon = this.gc.currentWeapon;
				if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
				{
					gameObject.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
				}
				if (this.variation == 1)
				{
					switch (this.primaryCharge)
					{
					case 0:
						gameObject.transform.Rotate(Random.Range(-this.spread / 1.5f, this.spread / 1.5f), Random.Range(-this.spread / 1.5f, this.spread / 1.5f), Random.Range(-this.spread / 1.5f, this.spread / 1.5f));
						break;
					case 1:
						gameObject.transform.Rotate(Random.Range(-this.spread, this.spread), Random.Range(-this.spread, this.spread), Random.Range(-this.spread, this.spread));
						break;
					case 2:
						gameObject.transform.Rotate(Random.Range(-this.spread * 2f, this.spread * 2f), Random.Range(-this.spread * 2f, this.spread * 2f), Random.Range(-this.spread * 2f, this.spread * 2f));
						break;
					}
				}
				else
				{
					gameObject.transform.Rotate(Random.Range(-this.spread, this.spread), Random.Range(-this.spread, this.spread), Random.Range(-this.spread, this.spread));
				}
			}
		}
		else
		{
			Vector3 vector2 = this.cam.transform.position + this.cam.transform.forward;
			RaycastHit raycastHit2;
			if (Physics.Raycast(this.cam.transform.position, this.cam.transform.forward, out raycastHit2, 1f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				vector2 = raycastHit2.point - this.cam.transform.forward * 0.1f;
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.explosion, vector2, this.cam.transform.rotation);
			if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
			{
				gameObject2.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
			}
			foreach (Explosion explosion in gameObject2.GetComponentsInChildren<Explosion>())
			{
				explosion.sourceWeapon = this.gc.currentWeapon;
				explosion.enemyDamageMultiplier = 1f;
				explosion.maxSize *= 1.5f;
				explosion.damage = 50;
			}
		}
		if (this.variation != 1)
		{
			this.gunAud.pitch = Random.Range(0.95f, 1.05f);
		}
		this.gunAud.clip = this.shootSound;
		this.gunAud.volume = 0.45f;
		this.gunAud.panStereo = 0f;
		this.gunAud.Play();
		this.cc.CameraShake(1f);
		if (this.variation == 1)
		{
			this.anim.SetTrigger("PumpFire");
		}
		else
		{
			this.anim.SetTrigger("Fire");
		}
		foreach (Transform transform in this.shootPoints)
		{
			Object.Instantiate<GameObject>(this.muzzleFlash, transform.transform.position, transform.transform.rotation);
		}
		this.releasingHeat = false;
		this.tempColor.a = 1f;
		this.heatSinkSMR.sharedMaterials[3].SetColor("_TintColor", this.tempColor);
		if (this.variation == 1)
		{
			this.primaryCharge = 0;
		}
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x000B7E34 File Offset: 0x000B6034
	private void ShootSinks()
	{
		this.gunReady = false;
		base.transform.localPosition = this.wpos.currentDefault;
		foreach (Transform transform in this.shootPoints)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.grenade, this.cam.transform.position + this.cam.transform.forward * 0.5f, Random.rotation);
			gameObject.GetComponentInChildren<Grenade>().sourceWeapon = this.gc.currentWeapon;
			gameObject.GetComponent<Rigidbody>().AddForce(this.grenadeVector * (this.grenadeForce + 10f), ForceMode.VelocityChange);
		}
		Object.Instantiate<GameObject>(this.grenadeSoundBubble).GetComponent<AudioSource>().volume = 0.45f * Mathf.Sqrt(Mathf.Pow(1f, 2f) - Mathf.Pow(this.grenadeForce, 2f) / Mathf.Pow(60f, 2f));
		this.anim.SetTrigger("Secondary Fire");
		this.gunAud.clip = this.shootSound;
		this.gunAud.volume = 0.45f * (this.grenadeForce / 60f);
		this.gunAud.panStereo = 0f;
		this.gunAud.pitch = Random.Range(0.75f, 0.85f);
		this.gunAud.Play();
		this.cc.CameraShake(1f);
		this.meterOverride = true;
		this.chargeSlider.value = 0f;
		this.sliderFill.color = Color.black;
		foreach (Transform transform2 in this.shootPoints)
		{
			Object.Instantiate<GameObject>(this.muzzleFlash, transform2.transform.position, transform2.transform.rotation);
		}
		this.releasingHeat = false;
		this.tempColor.a = 0f;
		this.heatSinkSMR.sharedMaterials[3].SetColor("_TintColor", this.tempColor);
		this.grenadeForce = 0f;
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x000B8068 File Offset: 0x000B6268
	private void ShootSaw()
	{
		this.gunReady = true;
		base.transform.localPosition = this.wpos.currentDefault;
		Vector3 vector = this.cam.transform.forward;
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			vector = (this.targeter.CurrentTarget.bounds.center - MonoSingleton<CameraController>.Instance.GetDefaultPos()).normalized;
		}
		foreach (Transform transform in this.shootPoints)
		{
			Vector3 vector2 = MonoSingleton<CameraController>.Instance.GetDefaultPos() + vector * 0.5f;
			RaycastHit raycastHit;
			if (Physics.Raycast(MonoSingleton<CameraController>.Instance.GetDefaultPos(), vector, out raycastHit, 5f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
			{
				vector2 = raycastHit.point - vector * 5f;
			}
			Chainsaw chainsaw = Object.Instantiate<Chainsaw>(this.chainsaw, vector2, Random.rotation);
			chainsaw.weaponType = "shotgun" + this.variation.ToString();
			chainsaw.CheckMultipleRicochets(true);
			chainsaw.sourceWeapon = this.gc.currentWeapon;
			chainsaw.attachedTransform = MonoSingleton<PlayerTracker>.Instance.GetTarget();
			chainsaw.lineStartTransform = this.chainsawAttachPoint;
			chainsaw.GetComponent<Rigidbody>().AddForce(vector * (this.grenadeForce + 10f) * 1.5f, ForceMode.VelocityChange);
			this.currentChainsaws.Add(chainsaw);
		}
		this.chainsawBladeRenderer.material = this.chainsawBladeMaterial;
		this.chainsawBladeScroll.scrollSpeedX = 0f;
		this.chainsawAttachPoint.gameObject.SetActive(false);
		Object.Instantiate<GameObject>(this.grenadeSoundBubble).GetComponent<AudioSource>().volume = 0.45f * Mathf.Sqrt(Mathf.Pow(1f, 2f) - Mathf.Pow(this.grenadeForce, 2f) / Mathf.Pow(60f, 2f));
		this.anim.Play("FireNoReload");
		this.gunAud.clip = this.shootSound;
		this.gunAud.volume = 0.45f * Mathf.Max(0.5f, this.grenadeForce / 60f);
		this.gunAud.panStereo = 0f;
		this.gunAud.pitch = Random.Range(0.75f, 0.85f);
		this.gunAud.Play();
		this.cc.CameraShake(1f);
		this.releasingHeat = false;
		this.grenadeForce = 0f;
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x000B832E File Offset: 0x000B652E
	private void Pump()
	{
		this.anim.SetTrigger("Pump");
		if (this.primaryCharge < 3)
		{
			this.primaryCharge++;
		}
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x000B8358 File Offset: 0x000B6558
	public void ReleaseHeat()
	{
		this.releasingHeat = true;
		ParticleSystem[] array = this.heatReleaseParticles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Play();
		}
		this.heatSinkAud.Play();
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x000B8394 File Offset: 0x000B6594
	public void ClickSound()
	{
		if (this.sliderFill.color != MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation])
		{
			this.gunAud.clip = this.clickChargeSound;
		}
		else
		{
			this.gunAud.clip = this.clickSound;
		}
		this.gunAud.volume = 0.5f;
		this.gunAud.pitch = Random.Range(0.95f, 1.05f);
		this.gunAud.panStereo = 0.1f;
		this.gunAud.Play();
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x000B8431 File Offset: 0x000B6631
	public void ReadyGun()
	{
		this.gunReady = true;
		this.meterOverride = false;
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x000B8444 File Offset: 0x000B6644
	public void Smack()
	{
		this.gunAud.clip = this.smackSound;
		this.gunAud.volume = 0.75f;
		this.gunAud.pitch = Random.Range(2f, 2.2f);
		this.gunAud.panStereo = 0.1f;
		this.gunAud.Play();
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x000B84A7 File Offset: 0x000B66A7
	public void SkipShoot()
	{
		this.anim.ResetTrigger("Fire");
		this.anim.Play("FireWithReload", -1, 0.05f);
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x000B84D0 File Offset: 0x000B66D0
	public void Pump1Sound()
	{
		AudioSource component = Object.Instantiate<GameObject>(this.grenadeSoundBubble).GetComponent<AudioSource>();
		component.pitch = Random.Range(0.95f, 1.05f);
		component.clip = this.pump1sound;
		component.volume = 1f;
		component.panStereo = 0.1f;
		component.Play();
		AudioSource component2 = Object.Instantiate<GameObject>(this.pumpChargeSound).GetComponent<AudioSource>();
		float num = (float)this.primaryCharge;
		component2.pitch = 1f + num / 5f;
		component2.Play();
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x000B855C File Offset: 0x000B675C
	public void Pump2Sound()
	{
		AudioSource component = Object.Instantiate<GameObject>(this.grenadeSoundBubble).GetComponent<AudioSource>();
		component.pitch = Random.Range(0.95f, 1.05f);
		component.clip = this.pump2sound;
		component.volume = 1f;
		component.panStereo = 0.1f;
		component.Play();
	}

	// Token: 0x04001F92 RID: 8082
	private InputManager inman;

	// Token: 0x04001F93 RID: 8083
	private WeaponIdentifier wid;

	// Token: 0x04001F94 RID: 8084
	private AudioSource gunAud;

	// Token: 0x04001F95 RID: 8085
	public AudioClip shootSound;

	// Token: 0x04001F96 RID: 8086
	public AudioClip shootSound2;

	// Token: 0x04001F97 RID: 8087
	public AudioClip clickSound;

	// Token: 0x04001F98 RID: 8088
	public AudioClip clickChargeSound;

	// Token: 0x04001F99 RID: 8089
	public AudioClip smackSound;

	// Token: 0x04001F9A RID: 8090
	public AudioClip pump1sound;

	// Token: 0x04001F9B RID: 8091
	public AudioClip pump2sound;

	// Token: 0x04001F9C RID: 8092
	public int variation;

	// Token: 0x04001F9D RID: 8093
	public GameObject bullet;

	// Token: 0x04001F9E RID: 8094
	public GameObject grenade;

	// Token: 0x04001F9F RID: 8095
	public float spread;

	// Token: 0x04001FA0 RID: 8096
	private bool smallSpread;

	// Token: 0x04001FA1 RID: 8097
	private Animator anim;

	// Token: 0x04001FA2 RID: 8098
	private GameObject cam;

	// Token: 0x04001FA3 RID: 8099
	private CameraController cc;

	// Token: 0x04001FA4 RID: 8100
	private GunControl gc;

	// Token: 0x04001FA5 RID: 8101
	private bool gunReady;

	// Token: 0x04001FA6 RID: 8102
	public Transform[] shootPoints;

	// Token: 0x04001FA7 RID: 8103
	public GameObject muzzleFlash;

	// Token: 0x04001FA8 RID: 8104
	public SkinnedMeshRenderer heatSinkSMR;

	// Token: 0x04001FA9 RID: 8105
	private Color tempColor;

	// Token: 0x04001FAA RID: 8106
	private bool releasingHeat;

	// Token: 0x04001FAB RID: 8107
	[SerializeField]
	private ParticleSystem[] heatReleaseParticles;

	// Token: 0x04001FAC RID: 8108
	private AudioSource heatSinkAud;

	// Token: 0x04001FAD RID: 8109
	private RaycastHit[] rhits;

	// Token: 0x04001FAE RID: 8110
	private bool charging;

	// Token: 0x04001FAF RID: 8111
	private float grenadeForce;

	// Token: 0x04001FB0 RID: 8112
	private Vector3 grenadeVector;

	// Token: 0x04001FB1 RID: 8113
	private Slider chargeSlider;

	// Token: 0x04001FB2 RID: 8114
	public Image sliderFill;

	// Token: 0x04001FB3 RID: 8115
	public GameObject grenadeSoundBubble;

	// Token: 0x04001FB4 RID: 8116
	public GameObject chargeSoundBubble;

	// Token: 0x04001FB5 RID: 8117
	private AudioSource tempChargeSound;

	// Token: 0x04001FB6 RID: 8118
	[HideInInspector]
	public int primaryCharge;

	// Token: 0x04001FB7 RID: 8119
	private bool cockedBack;

	// Token: 0x04001FB8 RID: 8120
	public GameObject explosion;

	// Token: 0x04001FB9 RID: 8121
	public GameObject pumpChargeSound;

	// Token: 0x04001FBA RID: 8122
	public GameObject warningBeep;

	// Token: 0x04001FBB RID: 8123
	private float timeToBeep;

	// Token: 0x04001FBC RID: 8124
	[SerializeField]
	private Chainsaw chainsaw;

	// Token: 0x04001FBD RID: 8125
	private List<Chainsaw> currentChainsaws = new List<Chainsaw>();

	// Token: 0x04001FBE RID: 8126
	[SerializeField]
	private Transform chainsawAttachPoint;

	// Token: 0x04001FBF RID: 8127
	[SerializeField]
	private ScrollingTexture chainsawBladeScroll;

	// Token: 0x04001FC0 RID: 8128
	private MeshRenderer chainsawBladeRenderer;

	// Token: 0x04001FC1 RID: 8129
	private Material chainsawBladeMaterial;

	// Token: 0x04001FC2 RID: 8130
	[SerializeField]
	private Material chainsawBladeMotionMaterial;

	// Token: 0x04001FC3 RID: 8131
	[SerializeField]
	private HurtZone sawZone;

	// Token: 0x04001FC4 RID: 8132
	[SerializeField]
	private ParticleSystem environmentalSawSpark;

	// Token: 0x04001FC5 RID: 8133
	[SerializeField]
	private AudioSource environmentalSawSound;

	// Token: 0x04001FC6 RID: 8134
	private TimeSince enviroGibSpawnCooldown;

	// Token: 0x04001FC7 RID: 8135
	private WeaponPos wpos;

	// Token: 0x04001FC8 RID: 8136
	private CameraFrustumTargeter targeter;

	// Token: 0x04001FC9 RID: 8137
	private bool meterOverride;
}
