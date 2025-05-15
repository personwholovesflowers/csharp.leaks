using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000371 RID: 881
public class Railcannon : MonoBehaviour
{
	// Token: 0x06001477 RID: 5239 RVA: 0x000A5B6D File Offset: 0x000A3D6D
	private void Awake()
	{
		if (!this.gotStuff)
		{
			this.wid = base.GetComponent<WeaponIdentifier>();
		}
	}

	// Token: 0x06001478 RID: 5240 RVA: 0x000A5B83 File Offset: 0x000A3D83
	private void Start()
	{
		if (!this.gotStuff)
		{
			this.gotStuff = true;
			this.GetStuff();
		}
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x000A5B9C File Offset: 0x000A3D9C
	private void OnEnable()
	{
		if (!this.gotStuff)
		{
			this.gotStuff = true;
			this.GetStuff();
		}
		if (this.wc.raicharge != 5f)
		{
			this.fullCharge.SetActive(false);
			base.transform.localPosition = this.wpos.currentDefault;
			return;
		}
		if (this.variation == 2)
		{
			if (this.fullAud == null)
			{
				this.fullAud = this.fullCharge.GetComponent<AudioSource>();
			}
			this.pitchRise = true;
			this.fullAud.pitch = 0f;
		}
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x000A5C34 File Offset: 0x000A3E34
	private void OnDisable()
	{
		if (this.wc == null)
		{
			this.wc = base.GetComponentInParent<WeaponCharges>();
		}
		if (this.wpos != null)
		{
			base.transform.localPosition = this.wpos.currentDefault;
		}
		if (this.zooming)
		{
			this.zooming = false;
			this.cc.StopZoom();
		}
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x000A5C9C File Offset: 0x000A3E9C
	private void Update()
	{
		if (this.wid.delay > 0f && this.altCharge < this.wc.raicharge)
		{
			this.altCharge = this.wc.raicharge;
		}
		float raicharge = this.wc.raicharge;
		if (this.wid.delay > 0f)
		{
			raicharge = this.altCharge;
		}
		if (raicharge < 5f && !NoWeaponCooldown.NoCooldown)
		{
			this.SetMaterialIntensity(raicharge, true);
		}
		else
		{
			MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.RailcannonIdle, this.fullCharge);
			if (!this.fullCharge.activeSelf)
			{
				this.fullCharge.SetActive(true);
				if (this.variation == 2)
				{
					this.pitchRise = true;
					this.fullAud.pitch = 0f;
				}
			}
			if (!this.wc.railChargePlayed)
			{
				this.wc.PlayRailCharge();
			}
			base.transform.localPosition = new Vector3(this.wpos.currentDefault.x + Random.Range(-0.005f, 0.005f), this.wpos.currentDefault.y + Random.Range(-0.005f, 0.005f), this.wpos.currentDefault.z + Random.Range(-0.005f, 0.005f));
			this.SetMaterialIntensity(1f, false);
			Color color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation];
			this.fullChargeLight.color = color;
			this.fullChargeParticles.main.startColor = color;
			if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
			{
				this.fullCharge.SetActive(false);
				base.transform.localPosition = this.wpos.currentDefault;
				this.wc.raicharge = 0f;
				this.wc.railChargePlayed = false;
				this.altCharge = 0f;
				if (!this.wid || this.wid.delay == 0f)
				{
					this.Shoot();
				}
				else
				{
					base.Invoke("Shoot", this.wid.delay);
				}
			}
		}
		if (!this.wid || this.wid.delay == 0f)
		{
			if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && this.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
			{
				this.zooming = true;
				this.cc.Zoom(this.cc.defaultFov / 2f);
			}
			else if (this.zooming)
			{
				this.zooming = false;
				this.cc.StopZoom();
			}
		}
		if (this.wid.delay != this.gotWidDelay)
		{
			this.gotWidDelay = this.wid.delay;
			if (this.wid && this.wid.delay != 0f)
			{
				this.fullAud.volume -= this.wid.delay * 2f;
				if (this.fullAud.volume < 0f)
				{
					this.fullAud.volume = 0f;
				}
			}
		}
		if (this.pitchRise)
		{
			this.fullAud.pitch = Mathf.MoveTowards(this.fullAud.pitch, 2f, Time.deltaTime * 4f);
			if (this.fullAud.pitch == 2f)
			{
				this.pitchRise = false;
			}
		}
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x000A606C File Offset: 0x000A426C
	private void SetMaterialIntensity(float newIntensity, bool isRecharging)
	{
		Color color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation];
		this.body.GetPropertyBlock(this.propBlock);
		this.propBlock.SetFloat(Railcannon.EmissiveIntensityID, isRecharging ? (newIntensity / 5f) : newIntensity);
		this.propBlock.SetColor("_EmissiveColor", color);
		this.body.SetPropertyBlock(this.propBlock);
		for (int i = 0; i < this.pips.Length; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = this.pips[i];
			skinnedMeshRenderer.GetPropertyBlock(this.propBlock);
			this.propBlock.SetColor("_EmissiveColor", color);
			if (isRecharging)
			{
				if (newIntensity > (float)i + 1f)
				{
					this.propBlock.SetFloat(Railcannon.EmissiveIntensityID, 1f);
				}
				else
				{
					float num = (newIntensity - (float)i) % 1f;
					this.propBlock.SetFloat(Railcannon.EmissiveIntensityID, num);
				}
			}
			else
			{
				this.propBlock.SetFloat(Railcannon.EmissiveIntensityID, newIntensity);
			}
			skinnedMeshRenderer.SetPropertyBlock(this.propBlock);
		}
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x000A617C File Offset: 0x000A437C
	private void Shoot()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.beam, this.cc.GetDefaultPos(), this.cc.transform.rotation);
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			gameObject.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
		}
		if (this.variation != 1)
		{
			RevolverBeam revolverBeam;
			if (gameObject.TryGetComponent<RevolverBeam>(out revolverBeam))
			{
				revolverBeam.sourceWeapon = this.gc.currentWeapon;
				revolverBeam.alternateStartPoint = this.shootPoint.position;
			}
		}
		else
		{
			gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 250f, ForceMode.VelocityChange);
			Harpoon harpoon;
			if (gameObject.TryGetComponent<Harpoon>(out harpoon))
			{
				harpoon.sourceWeapon = base.gameObject;
			}
		}
		Object.Instantiate<GameObject>(this.fireSound);
		this.anim.SetTrigger("Shoot");
		this.cc.CameraShake(2f);
		MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.GunFireStrong);
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x000A629C File Offset: 0x000A449C
	private void GetStuff()
	{
		this.targeter = Camera.main.GetComponent<CameraFrustumTargeter>();
		this.inman = MonoSingleton<InputManager>.Instance;
		this.wid = base.GetComponent<WeaponIdentifier>();
		this.aud = base.GetComponent<AudioSource>();
		this.cc = MonoSingleton<CameraController>.Instance;
		this.cam = this.cc.GetComponent<Camera>();
		this.gc = base.GetComponentInParent<GunControl>();
		this.anim = base.GetComponentInChildren<Animator>();
		this.wpos = base.GetComponent<WeaponPos>();
		this.fullAud = this.fullCharge.GetComponent<AudioSource>();
		this.wc = MonoSingleton<WeaponCharges>.Instance;
		this.propBlock = new MaterialPropertyBlock();
	}

	// Token: 0x04001C1F RID: 7199
	public int variation;

	// Token: 0x04001C20 RID: 7200
	public GameObject beam;

	// Token: 0x04001C21 RID: 7201
	public Transform shootPoint;

	// Token: 0x04001C22 RID: 7202
	public GameObject fullCharge;

	// Token: 0x04001C23 RID: 7203
	public GameObject fireSound;

	// Token: 0x04001C24 RID: 7204
	private AudioSource fullAud;

	// Token: 0x04001C25 RID: 7205
	private bool pitchRise;

	// Token: 0x04001C26 RID: 7206
	private InputManager inman;

	// Token: 0x04001C27 RID: 7207
	public WeaponIdentifier wid;

	// Token: 0x04001C28 RID: 7208
	private float gotWidDelay;

	// Token: 0x04001C29 RID: 7209
	private AudioSource aud;

	// Token: 0x04001C2A RID: 7210
	private CameraController cc;

	// Token: 0x04001C2B RID: 7211
	private Camera cam;

	// Token: 0x04001C2C RID: 7212
	private GunControl gc;

	// Token: 0x04001C2D RID: 7213
	private Animator anim;

	// Token: 0x04001C2E RID: 7214
	public SkinnedMeshRenderer body;

	// Token: 0x04001C2F RID: 7215
	public SkinnedMeshRenderer[] pips;

	// Token: 0x04001C30 RID: 7216
	private WeaponCharges wc;

	// Token: 0x04001C31 RID: 7217
	private WeaponPos wpos;

	// Token: 0x04001C32 RID: 7218
	private bool zooming;

	// Token: 0x04001C33 RID: 7219
	private bool gotStuff;

	// Token: 0x04001C34 RID: 7220
	private MaterialPropertyBlock propBlock;

	// Token: 0x04001C35 RID: 7221
	private static readonly int EmissiveIntensityID = Shader.PropertyToID("_EmissiveIntensity");

	// Token: 0x04001C36 RID: 7222
	private CameraFrustumTargeter targeter;

	// Token: 0x04001C37 RID: 7223
	private float altCharge;

	// Token: 0x04001C38 RID: 7224
	[SerializeField]
	private Light fullChargeLight;

	// Token: 0x04001C39 RID: 7225
	[SerializeField]
	private ParticleSystem fullChargeParticles;
}
