using System;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200038B RID: 907
public class Revolver : MonoBehaviour
{
	// Token: 0x060014D7 RID: 5335 RVA: 0x000A7B50 File Offset: 0x000A5D50
	private void Start()
	{
		this.targeter = MonoSingleton<CameraFrustumTargeter>.Instance;
		this.inman = MonoSingleton<InputManager>.Instance;
		this.wid = base.GetComponent<WeaponIdentifier>();
		this.gunReady = false;
		this.cam = MonoSingleton<CameraController>.Instance.GetComponent<Camera>();
		this.camObj = this.cam.gameObject;
		this.cc = MonoSingleton<CameraController>.Instance;
		this.nmov = MonoSingleton<NewMovement>.Instance;
		this.shootCharge = 0f;
		this.pierceShotCharge = 0f;
		this.pierceCharge = 100f;
		this.pierceReady = false;
		this.gunAud = base.GetComponent<AudioSource>();
		if (this.gunVariation == 0)
		{
			this.screenAud = this.screenMR.gameObject.GetComponent<AudioSource>();
		}
		else
		{
			this.screenAud = base.GetComponentInChildren<Canvas>().GetComponent<AudioSource>();
		}
		if (this.chargeEffect)
		{
			this.ceaud = this.chargeEffect.GetComponent<AudioSource>();
			this.celight = this.chargeEffect.GetComponent<Light>();
		}
		if (this.gunVariation == 0)
		{
			this.screenAud.clip = this.chargingSound;
			this.screenAud.loop = true;
			this.screenAud.pitch = 1f;
			this.screenAud.volume = 0.25f;
			this.screenAud.Play();
		}
		this.cylinder = base.GetComponentInChildren<RevolverCylinder>();
		this.gc = base.GetComponentInParent<GunControl>();
		this.beamDirectionSetter = new GameObject();
		this.anim = base.GetComponentInChildren<Animator>();
		this.wc = MonoSingleton<WeaponCharges>.Instance;
		this.wpos = base.GetComponent<WeaponPos>();
		if (this.wid.delay != 0f && this.gunVariation == 0)
		{
			this.pierceCharge = this.wc.rev0charge;
		}
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x000A7D14 File Offset: 0x000A5F14
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		if (this.wc == null)
		{
			this.wc = MonoSingleton<WeaponCharges>.Instance;
		}
		if (this.gunVariation == 0)
		{
			this.wc.rev0alt = this.altVersion;
			this.wc.rev0charge = this.pierceCharge;
		}
		this.pierceShotCharge = 0f;
		this.gunReady = false;
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x000A7D8C File Offset: 0x000A5F8C
	private void OnEnable()
	{
		if (this.wc == null)
		{
			this.wc = MonoSingleton<WeaponCharges>.Instance;
		}
		this.shootCharge = 100f;
		if (this.gunVariation == 0)
		{
			this.pierceCharge = this.wc.rev0charge;
		}
		else
		{
			this.pierceCharge = 100f;
			this.pierceReady = true;
			this.CheckCoinCharges();
		}
		if (this.gunVariation == 2)
		{
			this.wc.rev2alt = this.altVersion;
		}
		if (this.altVersion)
		{
			if (!this.anim)
			{
				this.anim = base.GetComponentInChildren<Animator>();
			}
			if (this.wc.revaltpickupcharges[this.gunVariation] > 0f)
			{
				this.anim.SetBool("SlowPickup", true);
			}
			else
			{
				this.anim.SetBool("SlowPickup", false);
			}
		}
		if (this.screenProps == null)
		{
			this.screenProps = new MaterialPropertyBlock();
		}
		if (this.screenMR)
		{
			this.screenMR.GetPropertyBlock(this.screenProps);
		}
		this.gunReady = false;
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x000A7EA0 File Offset: 0x000A60A0
	private void Update()
	{
		if (!this.shootReady)
		{
			if (this.shootCharge + 200f * Time.deltaTime < 100f)
			{
				this.shootCharge += 200f * Time.deltaTime;
			}
			else
			{
				this.shootCharge = 100f;
				this.shootReady = true;
			}
		}
		if (!this.pierceReady)
		{
			if (this.gunVariation == 0)
			{
				if (NoWeaponCooldown.NoCooldown)
				{
					this.pierceCharge = 100f;
				}
				float num = 1f;
				if (this.altVersion)
				{
					num = 0.5f;
				}
				if (this.pierceCharge + 40f * Time.deltaTime < 100f)
				{
					this.pierceCharge += 40f * Time.deltaTime * num;
				}
				else
				{
					this.pierceCharge = 100f;
					this.pierceReady = true;
					this.screenAud.clip = this.chargedSound;
					this.screenAud.loop = false;
					this.screenAud.volume = 0.35f;
					this.screenAud.pitch = Random.Range(1f, 1.1f);
					this.screenAud.Play();
				}
				if (this.cylinder.spinSpeed > 0f)
				{
					this.cylinder.spinSpeed = Mathf.MoveTowards(this.cylinder.spinSpeed, 0f, Time.deltaTime * 50f);
				}
				if (this.pierceCharge < 50f)
				{
					this.screenProps.SetTexture("_MainTex", this.batteryLow);
					this.screenProps.SetColor("_Color", Color.red);
				}
				else if (this.pierceCharge < 100f)
				{
					this.screenProps.SetTexture("_MainTex", this.batteryMid);
					this.screenProps.SetColor("_Color", Color.yellow);
				}
				else
				{
					this.screenProps.SetTexture("_MainTex", this.batteryFull);
				}
			}
			else if (this.pierceCharge + 480f * Time.deltaTime < 100f)
			{
				this.pierceCharge += 480f * Time.deltaTime;
			}
			else
			{
				this.pierceCharge = 100f;
				this.pierceReady = true;
			}
		}
		else if (this.gunVariation == 0)
		{
			if (this.pierceShotCharge != 0f)
			{
				if (this.pierceShotCharge < 50f)
				{
					this.screenProps.SetTexture("_MainTex", this.batteryCharges[0]);
				}
				else if (this.pierceShotCharge < 100f)
				{
					this.screenProps.SetTexture("_MainTex", this.batteryCharges[1]);
				}
				else
				{
					this.screenProps.SetTexture("_MainTex", this.batteryCharges[2]);
				}
				base.transform.localPosition = new Vector3(this.wpos.currentDefault.x + this.pierceShotCharge / 250f * Random.Range(-0.05f, 0.05f), this.wpos.currentDefault.y + this.pierceShotCharge / 250f * Random.Range(-0.05f, 0.05f), this.wpos.currentDefault.z + this.pierceShotCharge / 250f * Random.Range(-0.05f, 0.05f));
				this.cylinder.spinSpeed = this.pierceShotCharge;
			}
			else
			{
				this.screenProps.SetTexture("_MainTex", this.batteryFull);
				if (this.cylinder.spinSpeed != 0f)
				{
					this.cylinder.spinSpeed = 0f;
				}
			}
		}
		if (this.gc.activated)
		{
			if (this.gunVariation != 1 && this.gunReady)
			{
				float num2 = (float)((this.gunVariation == 0) ? 175 : (this.altVersion ? 750 : 75));
				if ((this.inman.InputSource.Fire2.WasCanceledThisFrame || (!this.inman.PerformingCheatMenuCombo() && !GameStateManager.Instance.PlayerInputLocked && this.inman.InputSource.Fire1.IsPressed)) && this.shootReady && ((this.gunVariation == 0) ? (this.pierceShotCharge == 100f) : (this.pierceShotCharge >= 25f)))
				{
					if (!this.wid || this.wid.delay == 0f)
					{
						this.Shoot(2);
					}
					else
					{
						this.shootReady = false;
						this.shootCharge = 0f;
						base.Invoke("DelayedShoot2", this.wid.delay);
					}
				}
				else if (!this.inman.PerformingCheatMenuCombo() && this.inman.InputSource.Fire1.IsPressed && this.shootReady && !this.chargingPierce)
				{
					if (!this.wid || this.wid.delay == 0f)
					{
						this.Shoot(1);
					}
					else
					{
						this.shootReady = false;
						this.shootCharge = 0f;
						base.Invoke("DelayedShoot", this.wid.delay);
					}
				}
				else if (this.inman.InputSource.Fire2.IsPressed && (this.gunVariation == 2 || this.shootReady) && ((this.gunVariation == 0) ? this.pierceReady : (this.coinCharge >= (float)(this.altVersion ? 300 : 100))))
				{
					if (!this.chargingPierce && !this.twirlRecovery)
					{
						this.latestTwirlRotation = 0f;
					}
					this.chargingPierce = true;
					if (this.pierceShotCharge + num2 * Time.deltaTime < 100f)
					{
						this.pierceShotCharge += num2 * Time.deltaTime;
					}
					else
					{
						this.pierceShotCharge = 100f;
					}
				}
				else
				{
					if (this.chargingPierce)
					{
						this.twirlRecovery = true;
					}
					this.chargingPierce = false;
					if (this.pierceShotCharge - num2 * Time.deltaTime > 0f)
					{
						this.pierceShotCharge -= num2 * Time.deltaTime;
					}
					else
					{
						this.pierceShotCharge = 0f;
					}
				}
			}
			else if (this.gunVariation == 1)
			{
				if (this.inman.InputSource.Fire2.WasPerformedThisFrame && this.pierceReady && this.coinCharge >= 100f)
				{
					this.cc.StopShake();
					if (!this.wid || this.wid.delay == 0f)
					{
						this.wc.rev1charge -= 100f;
					}
					if (!this.wid || this.wid.delay == 0f)
					{
						this.ThrowCoin();
					}
					else
					{
						base.Invoke("ThrowCoin", this.wid.delay);
						this.pierceReady = false;
						this.pierceCharge = 0f;
					}
				}
				else if (this.gunReady && !this.inman.PerformingCheatMenuCombo() && this.inman.InputSource.Fire1.IsPressed && this.shootReady)
				{
					if (!this.wid || this.wid.delay == 0f)
					{
						this.Shoot(1);
					}
					else
					{
						this.shootReady = false;
						this.shootCharge = 0f;
						base.Invoke("DelayedShoot", this.wid.delay);
					}
					if (this.ceaud && this.ceaud.volume != 0f)
					{
						this.ceaud.volume = 0f;
					}
				}
			}
		}
		if (this.celight)
		{
			if (this.pierceShotCharge == 0f && this.celight.enabled)
			{
				this.celight.enabled = false;
			}
			else if (this.pierceShotCharge != 0f)
			{
				this.celight.enabled = true;
				this.celight.range = this.pierceShotCharge * 0.01f;
			}
		}
		if (this.gunVariation != 1)
		{
			if (this.gunVariation == 0)
			{
				this.chargeEffect.transform.localScale = Vector3.one * this.pierceShotCharge * 0.02f;
				this.ceaud.pitch = this.pierceShotCharge * 0.005f;
			}
			this.ceaud.volume = 0.25f + this.pierceShotCharge * 0.005f;
			MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.RevolverCharge, this.ceaud.gameObject).intensityMultiplier = this.pierceShotCharge / 250f;
		}
		if (this.gunVariation != 0)
		{
			this.CheckCoinCharges();
		}
		else if (this.pierceCharge == 100f && MonoSingleton<ColorBlindSettings>.Instance)
		{
			this.screenProps.SetColor("_Color", MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.gunVariation]);
		}
		if (this.gunVariation == 0)
		{
			this.screenMR.SetPropertyBlock(this.screenProps);
		}
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x000A8814 File Offset: 0x000A6A14
	private void LateUpdate()
	{
		if (this.gunVariation != 2)
		{
			return;
		}
		if (this.chargingPierce || this.twirlRecovery)
		{
			this.anim.SetBool("Spinning", true);
			bool flag = this.latestTwirlRotation < 0f;
			if (this.chargingPierce)
			{
				this.twirlLevel = Mathf.Min(3f, Mathf.Floor(this.pierceShotCharge / 25f)) + 1f;
			}
			else
			{
				this.twirlLevel = Mathf.MoveTowards(this.twirlLevel, 0.1f, Time.deltaTime * 100f * this.twirlLevel);
			}
			this.latestTwirlRotation += 1200f * (this.twirlLevel / 3f + 0.5f) * Time.deltaTime;
			if (this.twirlSprite)
			{
				this.twirlSprite.color = new Color(1f, 1f, 1f, Mathf.Min(2f, Mathf.Floor(this.pierceShotCharge / 25f)) / 3f);
			}
			if (!this.ceaud.isPlaying)
			{
				this.ceaud.Play();
			}
			this.ceaud.pitch = 0.5f + this.twirlLevel / 2f;
			if (this.twirlRecovery && flag && this.latestTwirlRotation >= 0f)
			{
				this.latestTwirlRotation = 0f;
				this.twirlRecovery = false;
				if (this.twirlSprite)
				{
					this.twirlSprite.color = new Color(1f, 1f, 1f, 0f);
				}
			}
			else
			{
				while (this.latestTwirlRotation > 180f)
				{
					this.latestTwirlRotation -= 360f;
				}
				this.twirlBone.localRotation = Quaternion.Euler(this.twirlBone.localRotation.eulerAngles + (this.altVersion ? Vector3.left : Vector3.forward) * this.latestTwirlRotation);
			}
			this.anim.SetFloat("TwirlSpeed", this.twirlLevel / 3f);
			if (this.wid && this.wid.delay != 0f && !MonoSingleton<NewMovement>.Instance.gc.onGround)
			{
				MonoSingleton<NewMovement>.Instance.rb.AddForce(MonoSingleton<CameraController>.Instance.transform.up * 400f * this.twirlLevel * Time.deltaTime, ForceMode.Acceleration);
				return;
			}
		}
		else
		{
			this.anim.SetBool("Spinning", false);
			if (this.twirlSprite)
			{
				this.twirlSprite.color = new Color(1f, 1f, 1f, 0f);
			}
			this.ceaud.Stop();
		}
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x000A8B0C File Offset: 0x000A6D0C
	private void Shoot(int shotType = 1)
	{
		this.cc.StopShake();
		this.shootReady = false;
		this.shootCharge = 0f;
		if (this.altVersion)
		{
			MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges[this.gunVariation] = 2f;
		}
		if (shotType == 1)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.revolverBeam, this.cc.transform.position, this.cc.transform.rotation);
			if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
			{
				gameObject.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
			}
			RevolverBeam component = gameObject.GetComponent<RevolverBeam>();
			component.sourceWeapon = this.gc.currentWeapon;
			component.alternateStartPoint = this.gunBarrel.transform.position;
			component.gunVariation = this.gunVariation;
			if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
			{
				component.quickDraw = true;
			}
			this.currentGunShot = Random.Range(0, this.gunShots.Length);
			this.gunAud.clip = this.gunShots[this.currentGunShot];
			this.gunAud.volume = 0.55f;
			this.gunAud.pitch = Random.Range(0.9f, 1.1f);
			this.gunAud.Play();
			this.cam.fieldOfView = this.cam.fieldOfView + this.cc.defaultFov / 40f;
			MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFire, base.gameObject);
		}
		else if (shotType == 2)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.revolverBeamSuper, this.cc.transform.position, this.cc.transform.rotation);
			if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
			{
				gameObject2.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
			}
			RevolverBeam component2 = gameObject2.GetComponent<RevolverBeam>();
			component2.sourceWeapon = this.gc.currentWeapon;
			component2.alternateStartPoint = this.gunBarrel.transform.position;
			component2.gunVariation = this.gunVariation;
			if (this.gunVariation == 2)
			{
				component2.ricochetAmount = Mathf.Min(3, Mathf.FloorToInt(this.pierceShotCharge / 25f));
			}
			this.pierceShotCharge = 0f;
			if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
			{
				component2.quickDraw = true;
			}
			this.pierceReady = false;
			this.pierceCharge = 0f;
			if (this.gunVariation == 0)
			{
				this.screenAud.clip = this.chargingSound;
				this.screenAud.loop = true;
				if (this.altVersion)
				{
					this.screenAud.pitch = 0.5f;
				}
				else
				{
					this.screenAud.pitch = 1f;
				}
				this.screenAud.volume = 0.55f;
				this.screenAud.Play();
			}
			else if (!this.wid || this.wid.delay == 0f)
			{
				this.wc.rev2charge -= (float)(this.altVersion ? 300 : 100);
			}
			if (this.superGunSound)
			{
				Object.Instantiate<AudioSource>(this.superGunSound);
			}
			if (this.gunVariation == 2 && this.twirlShotSound)
			{
				Object.Instantiate<GameObject>(this.twirlShotSound, base.transform.position, Quaternion.identity);
			}
			this.cam.fieldOfView = this.cam.fieldOfView + this.cc.defaultFov / 20f;
			MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFireStrong, base.gameObject);
		}
		if (!this.altVersion)
		{
			this.cylinder.DoTurn();
		}
		this.anim.SetFloat("RandomChance", Random.Range(0f, 1f));
		if (shotType == 1)
		{
			this.anim.SetTrigger("Shoot");
		}
		else
		{
			this.anim.SetTrigger("ChargeShoot");
		}
		this.gunReady = false;
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x000A8F80 File Offset: 0x000A7180
	private void ThrowCoin()
	{
		if (this.punch == null || !this.punch.gameObject.activeInHierarchy)
		{
			this.punch = MonoSingleton<FistControl>.Instance.currentPunch;
		}
		if (this.punch)
		{
			this.punch.CoinFlip();
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.coin, this.camObj.transform.position + this.camObj.transform.up * -0.5f, this.camObj.transform.rotation);
		gameObject.GetComponent<Coin>().sourceWeapon = this.gc.currentWeapon;
		MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.CoinToss);
		gameObject.GetComponent<Rigidbody>().AddForce(this.camObj.transform.forward * 20f + Vector3.up * 15f + MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(true), ForceMode.VelocityChange);
		this.pierceCharge = 0f;
		this.pierceReady = false;
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x000A90A0 File Offset: 0x000A72A0
	private void ReadyToShoot()
	{
		this.shootReady = true;
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x000A90A9 File Offset: 0x000A72A9
	public void Punch()
	{
		this.gunReady = false;
		this.anim.SetTrigger("ChargeShoot");
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x000A90C2 File Offset: 0x000A72C2
	public void ReadyGun()
	{
		this.gunReady = true;
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x000A90CB File Offset: 0x000A72CB
	public void Click()
	{
		if (this.altVersion)
		{
			MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges[this.gunVariation] = 0f;
		}
		if (this.gunVariation == 2)
		{
			this.chargingPierce = false;
			this.twirlRecovery = false;
		}
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x000A9104 File Offset: 0x000A7304
	public void InstaClick()
	{
		if (!this.altVersion)
		{
			return;
		}
		for (int i = 0; i < MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges.Length; i++)
		{
			MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges[i] = 0f;
		}
		Object.Instantiate<GameObject>(this.quickTwirlEffect, this.cylinder.transform.position, base.transform.rotation).transform.SetParent(this.cylinder.transform);
		this.anim.Rebind();
		this.anim.Play("ShootTwirl", -1, 0f);
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x000A919E File Offset: 0x000A739E
	public void MaxCharge()
	{
		if (this.gunVariation == 0)
		{
			this.pierceCharge = 100f;
			return;
		}
		this.CheckCoinCharges();
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x000A91BA File Offset: 0x000A73BA
	private void DelayedShoot()
	{
		this.Shoot(1);
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x000A91C3 File Offset: 0x000A73C3
	private void DelayedShoot2()
	{
		this.Shoot(2);
	}

	// Token: 0x060014E6 RID: 5350 RVA: 0x000A91CC File Offset: 0x000A73CC
	private void CheckCoinCharges()
	{
		if (this.coinPanelsCharged == null || this.coinPanelsCharged.Length == 0)
		{
			this.coinPanelsCharged = new bool[this.coinPanels.Length];
		}
		this.coinCharge = ((this.gunVariation == 1) ? this.wc.rev1charge : this.wc.rev2charge);
		for (int i = 0; i < this.coinPanels.Length; i++)
		{
			if (this.altVersion && this.gunVariation == 2)
			{
				this.coinPanels[i].fillAmount = this.coinCharge / 300f;
			}
			else
			{
				this.coinPanels[i].fillAmount = this.coinCharge / 100f - (float)i;
			}
			if (this.coinPanels[i].fillAmount < 1f)
			{
				this.coinPanels[i].color = ((this.gunVariation == 1) ? Color.red : Color.gray);
				this.coinPanelsCharged[i] = false;
			}
			else
			{
				if (MonoSingleton<ColorBlindSettings>.Instance && this.coinPanels[i].color != MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.gunVariation])
				{
					this.coinPanels[i].color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.gunVariation];
				}
				if (!this.coinPanelsCharged[i] && (!this.wid || this.wid.delay == 0f))
				{
					if (!this.screenAud)
					{
						this.screenAud = base.GetComponentInChildren<Canvas>().GetComponent<AudioSource>();
					}
					this.screenAud.pitch = 1f + (float)i / 2f;
					this.screenAud.Play();
					this.coinPanelsCharged[i] = true;
				}
			}
		}
	}

	// Token: 0x04001C9C RID: 7324
	private InputManager inman;

	// Token: 0x04001C9D RID: 7325
	private WeaponIdentifier wid;

	// Token: 0x04001C9E RID: 7326
	public int gunVariation;

	// Token: 0x04001C9F RID: 7327
	public bool altVersion;

	// Token: 0x04001CA0 RID: 7328
	private AudioSource gunAud;

	// Token: 0x04001CA1 RID: 7329
	public AudioClip[] gunShots;

	// Token: 0x04001CA2 RID: 7330
	private int currentGunShot;

	// Token: 0x04001CA3 RID: 7331
	public GameObject gunBarrel;

	// Token: 0x04001CA4 RID: 7332
	private bool gunReady;

	// Token: 0x04001CA5 RID: 7333
	private bool shootReady = true;

	// Token: 0x04001CA6 RID: 7334
	private bool pierceReady = true;

	// Token: 0x04001CA7 RID: 7335
	public float shootCharge;

	// Token: 0x04001CA8 RID: 7336
	public float pierceCharge;

	// Token: 0x04001CA9 RID: 7337
	private bool chargingPierce;

	// Token: 0x04001CAA RID: 7338
	public float pierceShotCharge;

	// Token: 0x04001CAB RID: 7339
	public Vector3 shotHitPoint;

	// Token: 0x04001CAC RID: 7340
	public GameObject revolverBeam;

	// Token: 0x04001CAD RID: 7341
	public GameObject revolverBeamSuper;

	// Token: 0x04001CAE RID: 7342
	public AudioSource superGunSound;

	// Token: 0x04001CAF RID: 7343
	public RaycastHit hit;

	// Token: 0x04001CB0 RID: 7344
	public RaycastHit[] allHits;

	// Token: 0x04001CB1 RID: 7345
	private int currentHit;

	// Token: 0x04001CB2 RID: 7346
	private int currentHitMultiplier;

	// Token: 0x04001CB3 RID: 7347
	public float recoilFOV;

	// Token: 0x04001CB4 RID: 7348
	public GameObject chargeEffect;

	// Token: 0x04001CB5 RID: 7349
	private AudioSource ceaud;

	// Token: 0x04001CB6 RID: 7350
	private Light celight;

	// Token: 0x04001CB7 RID: 7351
	private GameObject camObj;

	// Token: 0x04001CB8 RID: 7352
	private Camera cam;

	// Token: 0x04001CB9 RID: 7353
	private CameraController cc;

	// Token: 0x04001CBA RID: 7354
	private Vector3 tempCamPos;

	// Token: 0x04001CBB RID: 7355
	public Vector3 beamReflectPos;

	// Token: 0x04001CBC RID: 7356
	private GameObject beamDirectionSetter;

	// Token: 0x04001CBD RID: 7357
	public MeshRenderer screenMR;

	// Token: 0x04001CBE RID: 7358
	private MaterialPropertyBlock screenProps;

	// Token: 0x04001CBF RID: 7359
	public Material batteryMat;

	// Token: 0x04001CC0 RID: 7360
	public Texture2D batteryFull;

	// Token: 0x04001CC1 RID: 7361
	public Texture2D batteryMid;

	// Token: 0x04001CC2 RID: 7362
	public Texture2D batteryLow;

	// Token: 0x04001CC3 RID: 7363
	public Texture2D[] batteryCharges;

	// Token: 0x04001CC4 RID: 7364
	private AudioSource screenAud;

	// Token: 0x04001CC5 RID: 7365
	public AudioClip chargedSound;

	// Token: 0x04001CC6 RID: 7366
	public AudioClip chargingSound;

	// Token: 0x04001CC7 RID: 7367
	public Transform twirlBone;

	// Token: 0x04001CC8 RID: 7368
	private float latestTwirlRotation;

	// Token: 0x04001CC9 RID: 7369
	private float twirlLevel;

	// Token: 0x04001CCA RID: 7370
	public bool twirlRecovery;

	// Token: 0x04001CCB RID: 7371
	public SpriteRenderer twirlSprite;

	// Token: 0x04001CCC RID: 7372
	public GameObject twirlShotSound;

	// Token: 0x04001CCD RID: 7373
	private GameObject currentDrip;

	// Token: 0x04001CCE RID: 7374
	public GameObject coin;

	// Token: 0x04001CCF RID: 7375
	public GameObject quickTwirlEffect;

	// Token: 0x04001CD0 RID: 7376
	[HideInInspector]
	public RevolverCylinder cylinder;

	// Token: 0x04001CD1 RID: 7377
	private SwitchMaterial rimLight;

	// Token: 0x04001CD2 RID: 7378
	public GunControl gc;

	// Token: 0x04001CD3 RID: 7379
	private Animator anim;

	// Token: 0x04001CD4 RID: 7380
	private Punch punch;

	// Token: 0x04001CD5 RID: 7381
	private NewMovement nmov;

	// Token: 0x04001CD6 RID: 7382
	private WeaponPos wpos;

	// Token: 0x04001CD7 RID: 7383
	public Image[] coinPanels;

	// Token: 0x04001CD8 RID: 7384
	public bool[] coinPanelsCharged;

	// Token: 0x04001CD9 RID: 7385
	private WeaponCharges wc;

	// Token: 0x04001CDA RID: 7386
	private CameraFrustumTargeter targeter;

	// Token: 0x04001CDB RID: 7387
	private float coinCharge = 400f;
}
