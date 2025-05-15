using System;
using TMPro;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000313 RID: 787
public class Nailgun : MonoBehaviour
{
	// Token: 0x060011F0 RID: 4592 RVA: 0x0008C464 File Offset: 0x0008A664
	private void Awake()
	{
		this.wid = base.GetComponent<WeaponIdentifier>();
		this.barrels = base.GetComponentsInChildren<Spin>(true);
		this.barrelLights = this.barrels[0].transform.parent.GetComponentsInChildren<Light>();
		this.barrelAud = this.barrels[0].GetComponent<AudioSource>();
		this.aud = base.GetComponent<AudioSource>();
		this.anim = base.GetComponentInChildren<Animator>();
		if (this.variation != 2)
		{
			this.heatSlider = base.GetComponentInChildren<Slider>();
			if (this.heatSlider)
			{
				this.sliderBg = this.heatSlider.GetComponentInParent<Image>();
			}
		}
		if (!this.altVersion)
		{
			this.heatSteam = base.GetComponentInChildren<ParticleSystem>();
			this.heatSteamAud = this.heatSteam.GetComponent<AudioSource>();
		}
		this.wpos = base.GetComponent<WeaponPos>();
		this.gc = base.GetComponentInParent<GunControl>();
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x0008C544 File Offset: 0x0008A744
	private void Start()
	{
		this.targeter = MonoSingleton<CameraFrustumTargeter>.Instance;
		this.inman = MonoSingleton<InputManager>.Instance;
		this.heatProps = new MaterialPropertyBlock();
		if (this.barrelHeats.Length != 0)
		{
			this.barrelHeats[0].GetPropertyBlock(this.heatProps);
			this.heatColor = Color.white;
			this.SetHeat(0f);
		}
		this.cc = MonoSingleton<CameraController>.Instance;
		this.nm = MonoSingleton<NewMovement>.Instance;
		this.currentFireRate = this.fireRate;
		if (this.aud)
		{
			this.aud.volume -= this.wid.delay * 2f;
			if (this.aud.volume < 0f)
			{
				this.aud.volume = 0f;
			}
		}
		if (this.barrelAud)
		{
			this.barrelAud.volume -= this.wid.delay * 2f;
			if (this.barrelAud.volume < 0f)
			{
				this.barrelAud.volume = 0f;
			}
		}
		if (this.heatSteamAud)
		{
			this.heatSteamAud.volume -= this.wid.delay * 2f;
			if (this.heatSteamAud.volume < 0f)
			{
				this.heatSteamAud.volume = 0f;
			}
		}
		if (this.wc == null)
		{
			this.wc = MonoSingleton<WeaponCharges>.Instance;
		}
		this.projectileVariationTypes = new string[4];
		for (int i = 0; i < this.projectileVariationTypes.Length; i++)
		{
			this.projectileVariationTypes[i] = "nailgun" + i.ToString();
		}
		if (this.altVersion && this.variation == 0 && this.heatSinks == 2f)
		{
			this.heatSinks = 1f;
		}
		this.anim.SetLayerWeight(1, 0f);
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x0008C748 File Offset: 0x0008A948
	private void OnDisable()
	{
		this.canShoot = false;
		this.harpoonCharge = 1f;
		this.wc.naiheatUp = this.heatUp;
		this.wc.nai0set = true;
		if (this.variation == 0)
		{
			if (!this.altVersion)
			{
				this.wc.naiHeatsinks = this.heatSinks;
			}
			else
			{
				this.wc.naiSawHeatsinks = this.heatSinks;
			}
		}
		if (this.currentZapper != null && MonoSingleton<NewMovement>.Instance)
		{
			this.currentZapper.lineStartTransform = MonoSingleton<NewMovement>.Instance.transform;
		}
		if (MonoSingleton<WeaponCharges>.Instance)
		{
			MonoSingleton<WeaponCharges>.Instance.naiAmmoDontCharge = false;
		}
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x0008C800 File Offset: 0x0008AA00
	private void OnEnable()
	{
		if (this.wc == null)
		{
			this.wc = MonoSingleton<WeaponCharges>.Instance;
		}
		if (this.variation == 0)
		{
			if (!this.altVersion)
			{
				this.heatSinks = this.wc.naiHeatsinks;
			}
			else
			{
				this.heatSinks = this.wc.naiSawHeatsinks;
			}
		}
		if (this.variation == 1 && this.aud)
		{
			this.RefreshHeatSinkFill(this.wc.naiMagnetCharge, true);
		}
		if (this.wc.nai0set)
		{
			this.wc.nai0set = false;
			if (this.heatSinks >= 1f)
			{
				this.heatUp = this.wc.naiheatUp;
			}
		}
		else
		{
			this.lookingForValue = true;
		}
		this.spinSpeed = 250f + this.heatUp * 2250f;
		Animator animator = this.anim;
		if (animator != null)
		{
			animator.SetLayerWeight(1, 0f);
		}
		if (this.currentZapper != null)
		{
			this.currentZapper.lineStartTransform = this.zapperAttachTransform;
		}
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x0008C910 File Offset: 0x0008AB10
	private void Update()
	{
		if (NoWeaponCooldown.NoCooldown)
		{
			if (this.altVersion)
			{
				this.heatSinks = 1f;
			}
			else
			{
				this.heatSinks = 2f;
			}
			this.wc.naiAmmo = 100f;
			this.wc.naiSaws = 10f;
			this.wc.naiMagnetCharge = 3f;
		}
		if (this.lookingForValue && this.wc && this.wc.nai0set)
		{
			this.wc.nai0set = false;
			this.heatUp = this.wc.naiheatUp;
			this.spinSpeed = 250f + this.heatUp * 2250f;
		}
		if (this.burnOut || this.heatSinks < 1f)
		{
			this.heatUp = Mathf.MoveTowards(this.heatUp, 0f, Time.deltaTime);
			if (this.burnOut && this.heatUp <= 0f)
			{
				this.burnOut = false;
				ParticleSystem particleSystem = this.heatSteam;
				if (particleSystem != null)
				{
					particleSystem.Stop();
				}
				AudioSource audioSource = this.heatSteamAud;
				if (audioSource != null)
				{
					audioSource.Stop();
				}
			}
		}
		else if (this.canShoot && !MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && (this.heatUp < 1f || this.variation == 2) && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			if (this.variation == 2)
			{
				this.heatUp = ((this.currentZapper && this.currentZapper.attached) ? 1f : 0.33f);
			}
			else if (this.variation == 1)
			{
				this.heatUp = 1f;
			}
			else if (!this.altVersion)
			{
				this.heatUp = Mathf.MoveTowards(this.heatUp, 1f, Time.deltaTime * 0.55f);
			}
		}
		else if (this.heatUp > 0f && (!this.canShoot || !MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed))
		{
			if (!this.altVersion)
			{
				this.heatUp = Mathf.MoveTowards(this.heatUp, 0f, Time.deltaTime * 0.3f);
			}
			else if (this.fireCooldown <= 0f)
			{
				this.heatUp = Mathf.MoveTowards(this.heatUp, 0f, Time.deltaTime * 0.2f);
			}
			else
			{
				this.heatUp = Mathf.MoveTowards(this.heatUp, 0f, Time.deltaTime * 0.03f);
			}
		}
		if (this.heatSlider)
		{
			if (this.heatSlider.value != this.heatUp)
			{
				this.heatSlider.value = this.heatUp;
				this.sliderBg.color = Color.Lerp(this.emptyColor, this.fullColor, this.heatUp);
				if (this.heatUp <= 0f && this.heatSinks < 1f)
				{
					this.sliderBg.color = new Color(0f, 0f, 0f, 0f);
				}
			}
			else if (this.heatUp == 0f && this.heatSinks >= 1f && this.sliderBg.color.a == 0f)
			{
				this.sliderBg.color = this.emptyColor;
			}
		}
		if (this.canShoot && !MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			this.spinSpeed = 250f + this.heatUp * 2250f;
		}
		else
		{
			this.spinSpeed = Mathf.MoveTowards(this.spinSpeed, 0f, Time.deltaTime * 1000f);
		}
		if (this.variation != 0)
		{
			if (this.variation == 2 && this.altVersion)
			{
				if (this.currentZapper != null)
				{
					this.currentFireRate = this.fireRate + 3.5f - this.heatUp * 3.5f;
				}
				else
				{
					this.currentFireRate = this.fireRate * 2f;
				}
			}
			else
			{
				this.currentFireRate = this.fireRate + 3.5f - this.heatUp * 3.5f;
			}
		}
		else if (this.burnOut)
		{
			if (this.altVersion)
			{
				this.currentFireRate = 3f;
			}
			else
			{
				this.currentFireRate = this.fireRate - 2.5f;
			}
		}
		else if (this.heatSinks >= 1f)
		{
			if (this.heatUp < 0.5f)
			{
				this.currentFireRate = this.fireRate;
			}
			else if (this.altVersion)
			{
				this.currentFireRate = this.fireRate + (this.heatUp - 0.5f) * 65f;
			}
			else
			{
				this.currentFireRate = this.fireRate + (this.heatUp - 0.5f) * 7f;
			}
		}
		else if (this.altVersion)
		{
			this.currentFireRate = this.fireRate + 50f;
		}
		else
		{
			this.currentFireRate = this.fireRate + 10f;
		}
		this.barrelAud.pitch = this.spinSpeed / 1500f * 2f;
		Spin[] array = this.barrels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].speed = this.spinSpeed;
		}
		if (this.heatUp > 0f)
		{
			Light[] array2 = this.barrelLights;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].intensity = this.heatUp * 10f;
			}
			this.SetHeat(this.heatUp);
		}
		else
		{
			Light[] array2 = this.barrelLights;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].intensity = 0f;
			}
			this.SetHeat(0f);
		}
		if (this.variation == 1)
		{
			if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
			{
				MonoSingleton<WeaponCharges>.Instance.naiAmmoDontCharge = true;
			}
			else
			{
				MonoSingleton<WeaponCharges>.Instance.naiAmmoDontCharge = false;
			}
			if (this.altVersion)
			{
				this.ammoText.text = Mathf.RoundToInt(this.wc.naiSaws).ToString();
			}
			else
			{
				this.ammoText.text = Mathf.RoundToInt(this.wc.naiAmmo).ToString();
			}
		}
		if (this.canShoot && !this.burnOut && this.heatSinks >= 1f && this.heatUp >= 0.1f && this.variation == 0 && MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			if (this.altVersion)
			{
				this.SuperSaw();
			}
			else
			{
				this.burnOut = true;
				this.fireCooldown = 0f;
				this.heatSinks -= 1f;
				ParticleSystem particleSystem2 = this.heatSteam;
				if (particleSystem2 != null)
				{
					particleSystem2.Play();
				}
				AudioSource audioSource2 = this.heatSteamAud;
				if (audioSource2 != null)
				{
					audioSource2.Play();
				}
				this.currentFireRate = this.fireRate - 2.5f;
			}
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame && this.variation != 0)
		{
			if (this.variation == 1 && (!this.wid || this.wid.delay == 0f))
			{
				if (this.wc.naiMagnetCharge >= 1f)
				{
					this.ShootMagnet();
				}
				else
				{
					Object.Instantiate<GameObject>(this.noAmmoSound);
				}
			}
			else if (this.variation == 2)
			{
				if (MonoSingleton<WeaponCharges>.Instance.naiZapperRecharge >= 5f)
				{
					if (!this.wid || this.wid.delay == 0f)
					{
						this.ShootZapper();
					}
					else
					{
						base.Invoke("ShootZapper", this.wid.delay);
					}
				}
				else
				{
					Object.Instantiate<GameObject>(this.noAmmoSound);
				}
			}
		}
		if (this.variation == 0 && !MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
		{
			if (this.heatSinks < 2f && !this.altVersion)
			{
				this.heatSinks = Mathf.MoveTowards(this.heatSinks, 2f, Time.deltaTime * 0.125f);
			}
			else if (this.altVersion && this.heatSinks < 1f)
			{
				this.heatSinks = Mathf.MoveTowards(this.heatSinks, 1f, Time.deltaTime * 0.125f);
			}
		}
		if (this.variation == 2)
		{
			this.UpdateZapHud();
		}
		if (this.heatSinkImages == null || this.heatSinkImages.Length == 0)
		{
			return;
		}
		float naiMagnetCharge = this.heatSinks;
		if (this.variation == 1)
		{
			naiMagnetCharge = this.wc.naiMagnetCharge;
		}
		this.RefreshHeatSinkFill(naiMagnetCharge, this.heatSinkFill != naiMagnetCharge);
	}

	// Token: 0x060011F5 RID: 4597 RVA: 0x0008D270 File Offset: 0x0008B470
	private void UpdateZapHud()
	{
		if (this.wc.naiZapperRecharge < 5f)
		{
			this.rechargingOverlay.SetActive(true);
			this.rechargingMeter.fillAmount = this.wc.naiZapperRecharge / 5f;
			this.statusText.text = "";
			this.zapMeter.gameObject.SetActive(false);
			this.warningX.enabled = false;
			this.distanceMeter.value = 0f;
			return;
		}
		if (this.rechargingOverlay.activeSelf)
		{
			AudioSource audioSource = this.aud;
			if (audioSource != null)
			{
				audioSource.Play();
			}
			this.rechargingOverlay.SetActive(false);
		}
		if (this.currentZapper == null)
		{
			this.zapMeter.gameObject.SetActive(false);
			this.warningX.enabled = false;
			RaycastHit raycastHit;
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			float num;
			if (Physics.Raycast(this.cc.GetDefaultPos(), this.cc.transform.forward, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment)) && !LayerMaskDefaults.IsMatchingLayer(raycastHit.collider.gameObject.layer, LMD.Environment) && raycastHit.collider.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && !enemyIdentifierIdentifier.eid.dead)
			{
				if (raycastHit.distance < this.zapper.maxDistance - 5f)
				{
					this.statusText.text = "READY";
					num = 1f - (this.zapper.maxDistance - 5f - raycastHit.distance) / (this.zapper.maxDistance - 5f);
					this.statusText.color = Color.white;
				}
				else
				{
					this.statusText.text = (this.altVersion ? "TOO FAR" : "OUT OF RANGE");
					num = 0f;
					this.statusText.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[2];
				}
			}
			else
			{
				this.statusText.text = (this.altVersion ? "NULL" : "NO TARGET");
				num = 0f;
				this.statusText.color = Color.gray;
			}
			this.distanceMeter.value = Mathf.MoveTowards(this.distanceMeter.value, num, Time.deltaTime * 10f);
			return;
		}
		if (this.currentZapper.charge > 0f)
		{
			this.zapMeter.gameObject.SetActive(true);
			this.zapMeter.value = this.currentZapper.charge / 5f;
		}
		else
		{
			this.zapMeter.gameObject.SetActive(false);
		}
		if (this.currentZapper.distance > this.currentZapper.maxDistance || this.currentZapper.raycastBlocked)
		{
			this.warningX.enabled = true;
			this.warningX.color = ((this.currentZapper.breakTimer % 0.1f > 0.05f) ? Color.red : Color.white);
			this.distanceMeter.value = 1f;
			this.statusText.text = (this.currentZapper.raycastBlocked ? "BLOCKED" : (this.altVersion ? "TOO FAR" : "OUT OF RANGE"));
			this.statusText.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[2];
			return;
		}
		this.warningX.enabled = false;
		this.distanceMeter.value = 1f - (this.currentZapper.maxDistance - this.currentZapper.distance) / this.currentZapper.maxDistance;
		this.statusText.text = (this.altVersion ? "" : "DISTANCE: ") + this.currentZapper.distance.ToString("f1");
		this.statusText.color = Color.Lerp(Color.red, Color.white, (this.currentZapper.maxDistance - this.currentZapper.distance) / this.currentZapper.maxDistance);
	}

	// Token: 0x060011F6 RID: 4598 RVA: 0x0008D6B0 File Offset: 0x0008B8B0
	private void FixedUpdate()
	{
		if (this.fireCooldown > 0f)
		{
			this.fireCooldown = Mathf.MoveTowards(this.fireCooldown, 0f, Time.deltaTime * 100f);
			if (this.fireCooldown < 0.01f)
			{
				this.fireCooldown = 0f;
			}
		}
		if (this.canShoot && ((!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed) || this.burnOut) && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			if (this.fireCooldown == 0f)
			{
				if (this.variation == 1 && ((!this.altVersion && Mathf.RoundToInt(this.wc.naiAmmo) <= 0) || (this.altVersion && Mathf.RoundToInt(this.wc.naiSaws) <= 0)))
				{
					this.fireCooldown = this.currentFireRate * 2f;
					if (this.shotSuccesfully)
					{
						Object.Instantiate<GameObject>(this.lastShotSound);
					}
					else
					{
						Object.Instantiate<GameObject>(this.noAmmoSound);
					}
					this.shotSuccesfully = false;
					return;
				}
				if (!this.wid || this.wid.delay == 0f)
				{
					this.Shoot();
					return;
				}
				this.fireCooldown = this.currentFireRate;
				base.Invoke("Shoot", this.wid.delay / 10f);
				return;
			}
		}
		else if (!MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
		{
			this.shotSuccesfully = false;
		}
	}

	// Token: 0x060011F7 RID: 4599 RVA: 0x0008D854 File Offset: 0x0008BA54
	private void UpdateAnimationWeight()
	{
		if (!this.burnOut && this.variation == 0)
		{
			if (this.heatSinks < 1f)
			{
				this.anim.SetLayerWeight(1, 0.9f);
				return;
			}
			if (this.wpos.currentDefault == this.wpos.middlePos)
			{
				this.anim.SetLayerWeight(1, 0.75f);
				return;
			}
			this.anim.SetLayerWeight(1, this.heatUp * 0.6f);
			return;
		}
		else
		{
			if (this.wpos.currentDefault == this.wpos.middlePos || (MonoSingleton<PowerUpMeter>.Instance && MonoSingleton<PowerUpMeter>.Instance.juice > 0f))
			{
				this.anim.SetLayerWeight(1, 0.75f);
				return;
			}
			this.anim.SetLayerWeight(1, 0f);
			return;
		}
	}

	// Token: 0x060011F8 RID: 4600 RVA: 0x0008D938 File Offset: 0x0008BB38
	private void SetHeat(float heat)
	{
		this.heatColor.a = heat;
		this.heatProps.SetColor("_Color", this.heatColor);
		for (int i = 0; i < this.barrelHeats.Length; i++)
		{
			this.barrelHeats[i].SetPropertyBlock(this.heatProps);
		}
		if (heat == 0f)
		{
			MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.NailgunFire);
			return;
		}
		if (this.barrelHeats != null && this.barrelHeats.Length != 0 && !this.altVersion)
		{
			bool flag = this.variation == 0 && this.burnOut;
			MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.NailgunFire, this.barrelHeats[0].gameObject).intensityMultiplier = (flag ? 3f : heat);
		}
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x0008D9FC File Offset: 0x0008BBFC
	private void Shoot()
	{
		this.UpdateAnimationWeight();
		this.fireCooldown = this.currentFireRate;
		this.shotSuccesfully = true;
		if (this.variation == 1 && (!this.wid || this.wid.delay == 0f))
		{
			if (this.altVersion)
			{
				this.wc.naiSaws -= 1f;
			}
			else
			{
				this.wc.naiAmmo -= 1f;
			}
		}
		this.anim.SetTrigger("Shoot");
		this.barrelNum++;
		if (this.barrelNum >= this.shootPoints.Length)
		{
			this.barrelNum = 0;
		}
		GameObject gameObject;
		if (this.burnOut)
		{
			gameObject = Object.Instantiate<GameObject>(this.muzzleFlash2, this.shootPoints[this.barrelNum].transform);
		}
		else
		{
			gameObject = Object.Instantiate<GameObject>(this.muzzleFlash, this.shootPoints[this.barrelNum].transform);
		}
		if (!this.altVersion)
		{
			AudioSource component = gameObject.GetComponent<AudioSource>();
			if (this.burnOut)
			{
				component.volume = 0.65f - this.wid.delay * 2f;
				if (component.volume < 0f)
				{
					component.volume = 0f;
				}
				component.pitch = 2f;
				this.currentSpread = this.spread * 2f;
			}
			else
			{
				if (this.heatSinks < 1f)
				{
					component.pitch = 0.75f;
					component.volume = 0.25f - this.wid.delay * 2f;
					if (component.volume < 0f)
					{
						component.volume = 0f;
					}
				}
				else
				{
					component.volume = 0.65f - this.wid.delay * 2f;
					if (component.volume < 0f)
					{
						component.volume = 0f;
					}
				}
				this.currentSpread = this.spread;
			}
		}
		else if (this.burnOut)
		{
			this.currentSpread = 45f;
		}
		else if (this.altVersion && this.variation == 0)
		{
			if (this.heatSinks < 1f)
			{
				this.currentSpread = 45f;
			}
			else
			{
				this.currentSpread = Mathf.Lerp(0f, 45f, Mathf.Max(0f, this.heatUp - 0.25f));
			}
		}
		else
		{
			this.currentSpread = 0f;
		}
		GameObject gameObject2;
		if (this.burnOut)
		{
			gameObject2 = Object.Instantiate<GameObject>(this.heatedNail, this.cc.transform.position + this.cc.transform.forward, base.transform.rotation);
		}
		else
		{
			gameObject2 = Object.Instantiate<GameObject>(this.nail, this.cc.transform.position + this.cc.transform.forward, base.transform.rotation);
		}
		if (this.altVersion && this.variation == 0 && this.heatSinks >= 1f)
		{
			this.heatUp = Mathf.MoveTowards(this.heatUp, 1f, 0.125f);
		}
		gameObject2.transform.forward = this.cc.transform.forward;
		if (Physics.Raycast(this.cc.transform.position, this.cc.transform.forward, 1f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			gameObject2.transform.position = this.cc.transform.position;
		}
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			gameObject2.transform.position = this.cc.transform.position + (this.targeter.CurrentTarget.bounds.center - this.cc.transform.position).normalized;
			gameObject2.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
		}
		gameObject2.transform.Rotate(Random.Range(-this.currentSpread / 3f, this.currentSpread / 3f), Random.Range(-this.currentSpread / 3f, this.currentSpread / 3f), Random.Range(-this.currentSpread / 3f, this.currentSpread / 3f));
		Rigidbody rigidbody;
		if (gameObject2.TryGetComponent<Rigidbody>(out rigidbody))
		{
			rigidbody.velocity = gameObject2.transform.forward * 200f;
		}
		Nail nail;
		if (gameObject2.TryGetComponent<Nail>(out nail))
		{
			nail.sourceWeapon = this.gc.currentWeapon;
			nail.weaponType = this.projectileVariationTypes[this.variation];
			if (this.altVersion && this.variation != 1)
			{
				if (this.heatSinks >= 1f && this.variation != 2)
				{
					nail.hitAmount = Mathf.Lerp(3f, 1f, this.heatUp);
				}
				else
				{
					nail.hitAmount = 1f;
				}
			}
			if (nail.sawblade)
			{
				nail.ForceCheckSawbladeRicochet();
			}
		}
		if (!this.burnOut)
		{
			this.cc.CameraShake(0.1f);
		}
		else
		{
			this.cc.CameraShake(0.35f);
		}
		if (this.altVersion)
		{
			MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Sawblade);
		}
	}

	// Token: 0x060011FA RID: 4602 RVA: 0x0008DFA4 File Offset: 0x0008C1A4
	public void BurstFire()
	{
		this.UpdateAnimationWeight();
		this.burstAmount--;
		this.barrelNum++;
		if (this.barrelNum >= this.shootPoints.Length)
		{
			this.barrelNum = 0;
		}
		Object.Instantiate<GameObject>(this.muzzleFlash2, this.shootPoints[this.barrelNum].transform);
		this.currentSpread = this.spread;
		GameObject gameObject = Object.Instantiate<GameObject>(this.heatedNail, base.transform.position + base.transform.forward, base.transform.rotation);
		gameObject.transform.forward = base.transform.forward * -1f;
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			gameObject.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
			gameObject.transform.forward *= -1f;
		}
		gameObject.transform.Rotate(Random.Range(-this.currentSpread / 3f, this.currentSpread / 3f), Random.Range(-this.currentSpread / 3f, this.currentSpread / 3f), Random.Range(-this.currentSpread / 3f, this.currentSpread / 3f));
		gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * -100f, ForceMode.VelocityChange);
		Nail component = gameObject.GetComponent<Nail>();
		component.weaponType = this.projectileVariationTypes[this.variation];
		component.sourceWeapon = this.gc.currentWeapon;
		this.cc.CameraShake(0.5f);
		if (this.burstAmount > 0)
		{
			base.Invoke("BurstFire", 0.03f);
		}
	}

	// Token: 0x060011FB RID: 4603 RVA: 0x0008E19C File Offset: 0x0008C39C
	public void SuperSaw()
	{
		this.fireCooldown = this.currentFireRate;
		this.shotSuccesfully = true;
		this.anim.SetLayerWeight(1, 0f);
		this.anim.SetTrigger("SuperShoot");
		MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.SuperSaw);
		this.barrelNum++;
		if (this.barrelNum >= this.shootPoints.Length)
		{
			this.barrelNum = 0;
		}
		Object.Instantiate<GameObject>(this.muzzleFlash2, this.shootPoints[this.barrelNum].transform);
		this.currentSpread = 0f;
		GameObject gameObject = Object.Instantiate<GameObject>(this.heatedNail, this.cc.transform.position + this.cc.transform.forward, base.transform.rotation);
		gameObject.transform.forward = this.cc.transform.forward;
		if (Physics.Raycast(this.cc.transform.position, this.cc.transform.forward, 1f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			gameObject.transform.position = this.cc.transform.position;
		}
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			gameObject.transform.position = this.cc.transform.position + (this.targeter.CurrentTarget.bounds.center - this.cc.transform.position).normalized;
			gameObject.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
		}
		Rigidbody rigidbody;
		if (gameObject.TryGetComponent<Rigidbody>(out rigidbody))
		{
			rigidbody.velocity = gameObject.transform.forward * 200f;
		}
		Nail nail;
		if (gameObject.TryGetComponent<Nail>(out nail))
		{
			nail.weaponType = this.projectileVariationTypes[this.variation];
			nail.multiHitAmount = Mathf.RoundToInt(this.heatUp * 3f);
			nail.ForceCheckSawbladeRicochet();
			nail.sourceWeapon = this.gc.currentWeapon;
		}
		this.heatSinks -= 1f;
		this.heatUp = 0f;
		this.cc.CameraShake(0.5f);
	}

	// Token: 0x060011FC RID: 4604 RVA: 0x0008E41C File Offset: 0x0008C61C
	public void ShootMagnet()
	{
		this.UpdateAnimationWeight();
		GameObject gameObject = Object.Instantiate<GameObject>(this.magnetNail, this.cc.transform.position, base.transform.rotation);
		gameObject.transform.forward = base.transform.forward;
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			gameObject.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
		}
		gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 100f, ForceMode.VelocityChange);
		if (this.canShoot)
		{
			this.anim.SetTrigger("Shoot");
		}
		Object.Instantiate<AudioSource>(this.magnetShotSound);
		Magnet componentInChildren = gameObject.GetComponentInChildren<Magnet>();
		if (componentInChildren)
		{
			this.wc.magnets.Add(componentInChildren);
		}
		this.wc.naiMagnetCharge -= 1f;
		MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Magnet);
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x0008E53C File Offset: 0x0008C73C
	public void ShootZapper()
	{
		this.UpdateAnimationWeight();
		if (this.currentZapper)
		{
			this.currentZapper.Break(false);
		}
		this.currentZapper = Object.Instantiate<Zapper>(this.zapper, this.cc.GetDefaultPos(), base.transform.rotation);
		this.currentZapper.transform.forward = base.transform.forward;
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			this.currentZapper.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
		}
		this.currentZapper.GetComponent<Rigidbody>().AddForce(this.currentZapper.transform.forward * 100f, ForceMode.VelocityChange);
		if (this.canShoot)
		{
			this.anim.SetTrigger("Shoot");
		}
		Object.Instantiate<AudioSource>(this.magnetShotSound);
		this.currentZapper.lineStartTransform = this.zapperAttachTransform;
		this.currentZapper.connectedRB = MonoSingleton<NewMovement>.Instance.rb;
		this.currentZapper.sourceWeapon = base.gameObject;
		MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Magnet);
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x0008E687 File Offset: 0x0008C887
	public void CanShoot()
	{
		this.canShoot = true;
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x0008E690 File Offset: 0x0008C890
	private void MaxCharge()
	{
		if (this.variation == 0 && (this.heatSinks < 1f || (this.heatSinks < 2f && !this.altVersion)))
		{
			if (this.altVersion)
			{
				this.heatSinks = 1f;
				return;
			}
			this.heatSinks = 2f;
		}
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x0008E6E8 File Offset: 0x0008C8E8
	private void RefreshHeatSinkFill(float charge, bool playSound = false)
	{
		this.heatSinkFill = Mathf.MoveTowards(this.heatSinkFill, charge, Time.deltaTime * (Mathf.Abs((charge - this.heatSinkFill) * 20f) + 1f));
		for (int i = 0; i < this.heatSinkImages.Length; i++)
		{
			if (this.heatSinkFill > (float)i)
			{
				this.heatSinkImages[i].fillAmount = this.heatSinkFill - (float)i;
				int num = this.CorrectVariation();
				if (this.heatSinkFill >= (float)(i + 1) && this.heatSinkImages[i].color != MonoSingleton<ColorBlindSettings>.Instance.variationColors[num])
				{
					if (playSound)
					{
						this.aud.pitch = (float)i * 0.5f + 1f;
						this.aud.Play();
					}
					this.heatSinkImages[i].color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[num];
				}
				else if (this.heatSinkFill < (float)(i + 1))
				{
					this.heatSinkImages[i].color = this.emptyColor;
				}
			}
			else
			{
				this.heatSinkImages[i].fillAmount = 0f;
			}
		}
		if (this.ammoText)
		{
			this.ammoText.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.CorrectVariation()];
		}
	}

	// Token: 0x06001201 RID: 4609 RVA: 0x0008E83F File Offset: 0x0008CA3F
	public void SnapSound()
	{
		Object.Instantiate<AudioSource>(this.snapSound);
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x0008E84D File Offset: 0x0008CA4D
	private int CorrectVariation()
	{
		if (this.variation == 0)
		{
			return 1;
		}
		if (this.variation == 1)
		{
			return 0;
		}
		return this.variation;
	}

	// Token: 0x04001870 RID: 6256
	private InputManager inman;

	// Token: 0x04001871 RID: 6257
	private WeaponIdentifier wid;

	// Token: 0x04001872 RID: 6258
	public int variation;

	// Token: 0x04001873 RID: 6259
	public bool altVersion;

	// Token: 0x04001874 RID: 6260
	public GameObject[] shootPoints;

	// Token: 0x04001875 RID: 6261
	private Spin[] barrels;

	// Token: 0x04001876 RID: 6262
	private float spinSpeed;

	// Token: 0x04001877 RID: 6263
	private int barrelNum;

	// Token: 0x04001878 RID: 6264
	private Light[] barrelLights;

	// Token: 0x04001879 RID: 6265
	[SerializeField]
	private Renderer[] barrelHeats;

	// Token: 0x0400187A RID: 6266
	private float heatUp;

	// Token: 0x0400187B RID: 6267
	private bool burnOut;

	// Token: 0x0400187C RID: 6268
	public GameObject muzzleFlash;

	// Token: 0x0400187D RID: 6269
	public GameObject muzzleFlash2;

	// Token: 0x0400187E RID: 6270
	public AudioSource snapSound;

	// Token: 0x0400187F RID: 6271
	public float fireRate;

	// Token: 0x04001880 RID: 6272
	private float currentFireRate;

	// Token: 0x04001881 RID: 6273
	private float fireCooldown;

	// Token: 0x04001882 RID: 6274
	private AudioSource aud;

	// Token: 0x04001883 RID: 6275
	private AudioSource barrelAud;

	// Token: 0x04001884 RID: 6276
	public GameObject nail;

	// Token: 0x04001885 RID: 6277
	public GameObject heatedNail;

	// Token: 0x04001886 RID: 6278
	public GameObject magnetNail;

	// Token: 0x04001887 RID: 6279
	public AudioSource magnetShotSound;

	// Token: 0x04001888 RID: 6280
	private CameraController cc;

	// Token: 0x04001889 RID: 6281
	public float spread;

	// Token: 0x0400188A RID: 6282
	private float currentSpread;

	// Token: 0x0400188B RID: 6283
	private int burstAmount;

	// Token: 0x0400188C RID: 6284
	private Animator anim;

	// Token: 0x0400188D RID: 6285
	private bool canShoot;

	// Token: 0x0400188E RID: 6286
	private NewMovement nm;

	// Token: 0x0400188F RID: 6287
	[Header("Magnet")]
	public Text ammoText;

	// Token: 0x04001890 RID: 6288
	public GameObject noAmmoSound;

	// Token: 0x04001891 RID: 6289
	public GameObject lastShotSound;

	// Token: 0x04001892 RID: 6290
	private float harpoonCharge = 1f;

	// Token: 0x04001893 RID: 6291
	private bool shotSuccesfully;

	// Token: 0x04001894 RID: 6292
	[Header("Overheat")]
	public Color emptyColor;

	// Token: 0x04001895 RID: 6293
	public Color fullColor;

	// Token: 0x04001896 RID: 6294
	private Slider heatSlider;

	// Token: 0x04001897 RID: 6295
	private Image sliderBg;

	// Token: 0x04001898 RID: 6296
	private float heatSinks = 2f;

	// Token: 0x04001899 RID: 6297
	private float heatSinkFill = 2f;

	// Token: 0x0400189A RID: 6298
	public Image[] heatSinkImages;

	// Token: 0x0400189B RID: 6299
	private ParticleSystem heatSteam;

	// Token: 0x0400189C RID: 6300
	private AudioSource heatSteamAud;

	// Token: 0x0400189D RID: 6301
	private float heatCharge = 1f;

	// Token: 0x0400189E RID: 6302
	[Header("Zapper")]
	public Zapper zapper;

	// Token: 0x0400189F RID: 6303
	private Zapper currentZapper;

	// Token: 0x040018A0 RID: 6304
	public Transform zapperAttachTransform;

	// Token: 0x040018A1 RID: 6305
	[SerializeField]
	private TMP_Text statusText;

	// Token: 0x040018A2 RID: 6306
	[SerializeField]
	private Slider distanceMeter;

	// Token: 0x040018A3 RID: 6307
	[SerializeField]
	private Slider zapMeter;

	// Token: 0x040018A4 RID: 6308
	[SerializeField]
	private Image warningX;

	// Token: 0x040018A5 RID: 6309
	[SerializeField]
	private GameObject rechargingOverlay;

	// Token: 0x040018A6 RID: 6310
	[SerializeField]
	private Image rechargingMeter;

	// Token: 0x040018A7 RID: 6311
	private WeaponCharges wc;

	// Token: 0x040018A8 RID: 6312
	private WeaponPos wpos;

	// Token: 0x040018A9 RID: 6313
	private GunControl gc;

	// Token: 0x040018AA RID: 6314
	private bool lookingForValue;

	// Token: 0x040018AB RID: 6315
	private CameraFrustumTargeter targeter;

	// Token: 0x040018AC RID: 6316
	private MaterialPropertyBlock heatProps;

	// Token: 0x040018AD RID: 6317
	private Color heatColor;

	// Token: 0x040018AE RID: 6318
	private string[] projectileVariationTypes;
}
