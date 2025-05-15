using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000391 RID: 913
public class RocketLauncher : MonoBehaviour
{
	// Token: 0x060014FF RID: 5375 RVA: 0x000AB774 File Offset: 0x000A9974
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.wid = base.GetComponent<WeaponIdentifier>();
		this.anim = base.GetComponent<Animator>();
		this.wpos = base.GetComponent<WeaponPos>();
		this.colorablesTransparencies = new float[this.variationColorables.Length];
		for (int i = 0; i < this.variationColorables.Length; i++)
		{
			this.colorablesTransparencies[i] = this.variationColorables[i].color.a;
		}
		if (this.variation == 0 && (!this.wid || this.wid.delay == 0f))
		{
			MonoSingleton<WeaponCharges>.Instance.rocketLauncher = this;
		}
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x000AB824 File Offset: 0x000A9A24
	private void OnEnable()
	{
		if (!MonoSingleton<WeaponCharges>.Instance.rocketset)
		{
			this.lookingForValue = true;
			return;
		}
		MonoSingleton<WeaponCharges>.Instance.rocketset = false;
		if (MonoSingleton<WeaponCharges>.Instance.rocketcharge < 0.25f)
		{
			this.cooldown = 0.25f;
			return;
		}
		this.cooldown = MonoSingleton<WeaponCharges>.Instance.rocketcharge;
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x000AB880 File Offset: 0x000A9A80
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		MonoSingleton<WeaponCharges>.Instance.rocketcharge = this.cooldown;
		MonoSingleton<WeaponCharges>.Instance.rocketset = true;
		this.cbCharge = 0f;
		this.firingCannonball = false;
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x000AB8D0 File Offset: 0x000A9AD0
	private void OnDestroy()
	{
		if (this.currentTimerTickSound)
		{
			Object.Destroy(this.currentTimerTickSound.gameObject);
		}
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x000AB8F0 File Offset: 0x000A9AF0
	private void Update()
	{
		if (!MonoSingleton<ColorBlindSettings>.Instance)
		{
			return;
		}
		Color color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation];
		float num = 1f;
		if (MonoSingleton<WeaponCharges>.Instance.rocketset && this.lookingForValue)
		{
			MonoSingleton<WeaponCharges>.Instance.rocketset = false;
			this.lookingForValue = false;
			if (MonoSingleton<WeaponCharges>.Instance.rocketcharge < 0.25f)
			{
				this.cooldown = 0.25f;
			}
			else
			{
				this.cooldown = MonoSingleton<WeaponCharges>.Instance.rocketcharge;
			}
		}
		if (this.variation == 1)
		{
			if (MonoSingleton<GunControl>.Instance.activated && !GameStateManager.Instance.PlayerInputLocked)
			{
				if (this.timerArm)
				{
					this.timerArm.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(360f, 0f, MonoSingleton<WeaponCharges>.Instance.rocketCannonballCharge));
				}
				if (this.timerMeter)
				{
					this.timerMeter.fillAmount = MonoSingleton<WeaponCharges>.Instance.rocketCannonballCharge;
				}
				if (this.lastKnownTimerAmount != MonoSingleton<WeaponCharges>.Instance.rocketCannonballCharge && (!this.wid || this.wid.delay == 0f))
				{
					float num2 = 4f;
					while (num2 > 0f)
					{
						if (MonoSingleton<WeaponCharges>.Instance.rocketCannonballCharge >= num2 / 4f && this.lastKnownTimerAmount < num2 / 4f)
						{
							AudioSource audioSource = Object.Instantiate<AudioSource>(this.timerWindupSound);
							audioSource.pitch = 1.6f + num2 * 0.1f;
							if (MonoSingleton<WeaponCharges>.Instance.rocketCannonballCharge < 1f)
							{
								audioSource.volume /= 2f;
								break;
							}
							break;
						}
						else
						{
							num2 -= 1f;
						}
					}
					this.lastKnownTimerAmount = MonoSingleton<WeaponCharges>.Instance.rocketCannonballCharge;
				}
				if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && !MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && MonoSingleton<WeaponCharges>.Instance.rocketCannonballCharge >= 1f)
				{
					if (!this.wid || this.wid.delay == 0f)
					{
						if (!this.chargeSound.isPlaying)
						{
							this.chargeSound.Play();
						}
						this.chargeSound.pitch = this.cbCharge + 0.5f;
					}
					this.cbCharge = Mathf.MoveTowards(this.cbCharge, 1f, Time.deltaTime);
					base.transform.localPosition = new Vector3(this.wpos.currentDefault.x + Random.Range(this.cbCharge / 100f * -1f, this.cbCharge / 100f), this.wpos.currentDefault.y + Random.Range(this.cbCharge / 100f * -1f, this.cbCharge / 100f), this.wpos.currentDefault.z + Random.Range(this.cbCharge / 100f * -1f, this.cbCharge / 100f));
					if (this.timerArm)
					{
						this.timerArm.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(360f, 0f, this.cbCharge));
					}
					if (this.timerMeter)
					{
						this.timerMeter.fillAmount = this.cbCharge;
					}
				}
				else if (this.cbCharge > 0f && !this.firingCannonball)
				{
					this.chargeSound.Stop();
					if (!this.wid || this.wid.delay == 0f)
					{
						this.ShootCannonball();
					}
					else
					{
						base.Invoke("ShootCannonball", this.wid.delay);
						this.firingCannonball = true;
					}
					MonoSingleton<WeaponCharges>.Instance.rocketCannonballCharge = 0f;
				}
			}
			if (this.cbCharge > 0f)
			{
				color = Color.Lerp(MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation], Color.red, this.cbCharge);
			}
			else if (MonoSingleton<WeaponCharges>.Instance.rocketCannonballCharge < 1f)
			{
				num = 0.5f;
			}
		}
		else if (this.variation == 0)
		{
			if (MonoSingleton<WeaponCharges>.Instance.rocketFreezeTime > 0f && MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame && !GameStateManager.Instance.PlayerInputLocked && (!this.wid || !this.wid.duplicate))
			{
				if (MonoSingleton<WeaponCharges>.Instance.rocketFrozen)
				{
					this.UnfreezeRockets();
				}
				else
				{
					this.FreezeRockets();
				}
			}
			if (this.timerArm)
			{
				this.timerArm.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(360f, 0f, MonoSingleton<WeaponCharges>.Instance.rocketFreezeTime / 5f));
			}
			if (this.timerMeter)
			{
				this.timerMeter.fillAmount = MonoSingleton<WeaponCharges>.Instance.rocketFreezeTime / 5f;
			}
			if (this.lastKnownTimerAmount != MonoSingleton<WeaponCharges>.Instance.rocketFreezeTime && (!this.wid || this.wid.delay == 0f))
			{
				for (float num3 = 4f; num3 > 0f; num3 -= 1f)
				{
					if (MonoSingleton<WeaponCharges>.Instance.rocketFreezeTime / 5f >= num3 / 4f && this.lastKnownTimerAmount / 5f < num3 / 4f)
					{
						Object.Instantiate<AudioSource>(this.timerWindupSound).pitch = 0.6f + num3 * 0.1f;
						break;
					}
				}
				this.lastKnownTimerAmount = MonoSingleton<WeaponCharges>.Instance.rocketFreezeTime;
			}
		}
		else if (this.variation == 2)
		{
			if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && !GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && !MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel > 0f && (this.firingNapalm || MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel >= 0.25f))
			{
				if (this.cooldown < 0.5f)
				{
					if (!this.firingNapalm)
					{
						this.napalmMuzzleFlashTransform.localScale = Vector3.one * 3f;
						this.napalmMuzzleFlashParticles.Play();
						foreach (AudioSource audioSource2 in this.napalmMuzzleFlashSounds)
						{
							audioSource2.pitch = Random.Range(0.9f, 1.1f);
							audioSource2.Play();
						}
					}
					this.firingNapalm = true;
				}
			}
			else if (this.firingNapalm)
			{
				this.firingNapalm = false;
				this.napalmMuzzleFlashParticles.Stop();
				foreach (AudioSource audioSource3 in this.napalmMuzzleFlashSounds)
				{
					if (audioSource3.loop)
					{
						audioSource3.Stop();
					}
				}
				this.napalmStopSound.pitch = Random.Range(0.9f, 1.1f);
				this.napalmStopSound.Play();
			}
			else if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame && MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel < 0.25f)
			{
				this.napalmNoAmmoSound.Play();
			}
			if (!this.firingNapalm && this.napalmMuzzleFlashTransform.localScale != Vector3.zero)
			{
				this.napalmMuzzleFlashTransform.localScale = Vector3.MoveTowards(this.napalmMuzzleFlashTransform.localScale, Vector3.zero, Time.deltaTime * 9f);
			}
			if (this.timerArm)
			{
				this.timerArm.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(360f, 0f, MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel));
			}
			if (this.timerMeter)
			{
				this.timerMeter.fillAmount = MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel;
			}
			if (this.lastKnownTimerAmount != MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel && (!this.wid || this.wid.delay == 0f))
			{
				if (MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel >= 0.25f && this.lastKnownTimerAmount < 0.25f)
				{
					Object.Instantiate<AudioSource>(this.timerWindupSound);
				}
				this.lastKnownTimerAmount = MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel;
			}
			if (!this.firingNapalm && MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel < 0.25f)
			{
				color = Color.grey;
			}
		}
		if (this.cooldown > 0f)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
		}
		else if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && MonoSingleton<GunControl>.Instance.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			if (!this.wid || this.wid.delay == 0f)
			{
				this.Shoot();
			}
			else
			{
				base.Invoke("Shoot", this.wid.delay);
				this.cooldown = 1f;
			}
		}
		for (int j = 0; j < this.variationColorables.Length; j++)
		{
			this.variationColorables[j].color = new Color(color.r, color.g, color.b, this.colorablesTransparencies[j] * num);
		}
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x000AC2B8 File Offset: 0x000AA4B8
	private void FixedUpdate()
	{
		if (this.napalmProjectileCooldown > 0f)
		{
			this.napalmProjectileCooldown = Mathf.MoveTowards(this.napalmProjectileCooldown, 0f, Time.fixedDeltaTime);
		}
		if (this.firingNapalm && this.napalmProjectileCooldown == 0f)
		{
			this.ShootNapalm();
		}
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x000AC308 File Offset: 0x000AA508
	public void Shoot()
	{
		if (this.aud)
		{
			this.aud.pitch = Random.Range(0.9f, 1.1f);
			this.aud.Play();
		}
		if (this.variation == 1 && this.cbCharge > 0f)
		{
			this.chargeSound.Stop();
			this.cbCharge = 0f;
		}
		Object.Instantiate<GameObject>(this.muzzleFlash, this.shootPoint.position, MonoSingleton<CameraController>.Instance.transform.rotation);
		this.anim.SetTrigger("Fire");
		this.cooldown = this.rateOfFire;
		GameObject gameObject = Object.Instantiate<GameObject>(this.rocket, MonoSingleton<CameraController>.Instance.transform.position, MonoSingleton<CameraController>.Instance.transform.rotation);
		if (MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
		{
			gameObject.transform.LookAt(MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center);
		}
		Grenade component = gameObject.GetComponent<Grenade>();
		if (component)
		{
			component.sourceWeapon = MonoSingleton<GunControl>.Instance.currentWeapon;
		}
		MonoSingleton<CameraController>.Instance.CameraShake(0.75f);
		MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFire, base.gameObject);
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x000AC464 File Offset: 0x000AA664
	public void ShootCannonball()
	{
		if (this.aud)
		{
			this.aud.pitch = Random.Range(0.6f, 0.8f);
			this.aud.Play();
		}
		base.transform.localPosition = this.wpos.currentDefault;
		Object.Instantiate<GameObject>(this.muzzleFlash, this.shootPoint.position, MonoSingleton<CameraController>.Instance.transform.rotation);
		this.anim.SetTrigger("Fire");
		this.cooldown = this.rateOfFire;
		Rigidbody rigidbody = Object.Instantiate<Rigidbody>(this.cannonBall, MonoSingleton<CameraController>.Instance.transform.position + MonoSingleton<CameraController>.Instance.transform.forward, MonoSingleton<CameraController>.Instance.transform.rotation);
		if (MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
		{
			rigidbody.transform.LookAt(MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center);
		}
		rigidbody.velocity = rigidbody.transform.forward * Mathf.Max(15f, this.cbCharge * 150f);
		Cannonball cannonball;
		if (rigidbody.TryGetComponent<Cannonball>(out cannonball))
		{
			cannonball.sourceWeapon = MonoSingleton<GunControl>.Instance.currentWeapon;
		}
		MonoSingleton<CameraController>.Instance.CameraShake(0.75f);
		this.cbCharge = 0f;
		this.firingCannonball = false;
	}

	// Token: 0x06001507 RID: 5383 RVA: 0x000AC5E0 File Offset: 0x000AA7E0
	public void ShootNapalm()
	{
		this.anim.SetTrigger("Spray");
		this.napalmProjectileCooldown = 0.02f;
		MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel -= 0.015f;
		Rigidbody rigidbody = Object.Instantiate<Rigidbody>(this.napalmProjectile, MonoSingleton<CameraController>.Instance.transform.position + MonoSingleton<CameraController>.Instance.transform.forward, MonoSingleton<CameraController>.Instance.transform.rotation);
		if (MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
		{
			rigidbody.transform.LookAt(MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center);
		}
		rigidbody.transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
		rigidbody.velocity = rigidbody.transform.forward * 150f;
		MonoSingleton<CameraController>.Instance.CameraShake(0.15f);
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x000AC704 File Offset: 0x000AA904
	public void FreezeRockets()
	{
		MonoSingleton<WeaponCharges>.Instance.rocketFrozen = true;
		if (this.wid && this.wid.delay != 0f)
		{
			return;
		}
		MonoSingleton<WeaponCharges>.Instance.timeSinceIdleFrozen = 0f;
		Object.Instantiate<AudioSource>(this.timerFreezeSound);
		if (!this.currentTimerTickSound)
		{
			this.currentTimerTickSound = Object.Instantiate<AudioSource>(this.timerTickSound);
		}
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x000AC77C File Offset: 0x000AA97C
	public void UnfreezeRockets()
	{
		MonoSingleton<WeaponCharges>.Instance.rocketFrozen = false;
		if (this.wid && this.wid.delay != 0f)
		{
			return;
		}
		MonoSingleton<WeaponCharges>.Instance.canAutoUnfreeze = false;
		Object.Instantiate<AudioSource>(this.timerUnfreezeSound);
		if (this.currentTimerTickSound)
		{
			Object.Destroy(this.currentTimerTickSound.gameObject);
		}
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x000AC7E8 File Offset: 0x000AA9E8
	public void Clunk(float pitch)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.clunkSound, base.transform.position, Quaternion.identity);
		MonoSingleton<CameraController>.Instance.CameraShake(0.25f);
		AudioSource audioSource;
		if (gameObject.TryGetComponent<AudioSource>(out audioSource))
		{
			audioSource.pitch = Random.Range(pitch - 0.1f, pitch + 0.1f);
		}
	}

	// Token: 0x04001D23 RID: 7459
	public int variation;

	// Token: 0x04001D24 RID: 7460
	public GameObject rocket;

	// Token: 0x04001D25 RID: 7461
	public GameObject clunkSound;

	// Token: 0x04001D26 RID: 7462
	public float rateOfFire;

	// Token: 0x04001D27 RID: 7463
	private float cooldown = 0.25f;

	// Token: 0x04001D28 RID: 7464
	private bool lookingForValue;

	// Token: 0x04001D29 RID: 7465
	private AudioSource aud;

	// Token: 0x04001D2A RID: 7466
	private Animator anim;

	// Token: 0x04001D2B RID: 7467
	private WeaponIdentifier wid;

	// Token: 0x04001D2C RID: 7468
	public Transform shootPoint;

	// Token: 0x04001D2D RID: 7469
	public GameObject muzzleFlash;

	// Token: 0x04001D2E RID: 7470
	[SerializeField]
	private Image timerMeter;

	// Token: 0x04001D2F RID: 7471
	[SerializeField]
	private RectTransform timerArm;

	// Token: 0x04001D30 RID: 7472
	[SerializeField]
	private Image[] variationColorables;

	// Token: 0x04001D31 RID: 7473
	private float[] colorablesTransparencies;

	// Token: 0x04001D32 RID: 7474
	private WeaponPos wpos;

	// Token: 0x04001D33 RID: 7475
	[Header("Freeze variation")]
	[SerializeField]
	private AudioSource timerFreezeSound;

	// Token: 0x04001D34 RID: 7476
	[SerializeField]
	private AudioSource timerUnfreezeSound;

	// Token: 0x04001D35 RID: 7477
	[SerializeField]
	private AudioSource timerTickSound;

	// Token: 0x04001D36 RID: 7478
	[HideInInspector]
	public AudioSource currentTimerTickSound;

	// Token: 0x04001D37 RID: 7479
	[SerializeField]
	private AudioSource timerWindupSound;

	// Token: 0x04001D38 RID: 7480
	private float lastKnownTimerAmount;

	// Token: 0x04001D39 RID: 7481
	[Header("Cannonball variation")]
	public Rigidbody cannonBall;

	// Token: 0x04001D3A RID: 7482
	[SerializeField]
	private AudioSource chargeSound;

	// Token: 0x04001D3B RID: 7483
	private float cbCharge;

	// Token: 0x04001D3C RID: 7484
	private bool firingCannonball;

	// Token: 0x04001D3D RID: 7485
	[Header("Napalm variation")]
	[SerializeField]
	private Rigidbody napalmProjectile;

	// Token: 0x04001D3E RID: 7486
	private float napalmProjectileCooldown;

	// Token: 0x04001D3F RID: 7487
	[SerializeField]
	private Transform napalmMuzzleFlashTransform;

	// Token: 0x04001D40 RID: 7488
	[SerializeField]
	private ParticleSystem napalmMuzzleFlashParticles;

	// Token: 0x04001D41 RID: 7489
	[SerializeField]
	private AudioSource[] napalmMuzzleFlashSounds;

	// Token: 0x04001D42 RID: 7490
	[SerializeField]
	private AudioSource napalmStopSound;

	// Token: 0x04001D43 RID: 7491
	[SerializeField]
	private AudioSource napalmNoAmmoSound;

	// Token: 0x04001D44 RID: 7492
	private bool firingNapalm;
}
