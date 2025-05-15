using System;
using UnityEngine;

// Token: 0x020003DF RID: 991
public class SecondaryRevolver : MonoBehaviour
{
	// Token: 0x06001663 RID: 5731 RVA: 0x000B4739 File Offset: 0x000B2939
	private void Awake()
	{
		this.defaultGunPos = base.transform.localPosition;
		this.defaultGunRot = base.transform.localRotation;
		Debug.Log("Started!");
	}

	// Token: 0x06001664 RID: 5732 RVA: 0x000B4768 File Offset: 0x000B2968
	private void Start()
	{
		this.targeter = Camera.main.GetComponent<CameraFrustumTargeter>();
		this.screenMR = base.transform.GetChild(1).GetComponent<MeshRenderer>();
		this.gunBarrel = base.transform.GetChild(0).gameObject;
		this.cam = MonoSingleton<CameraController>.Instance.GetComponent<Camera>();
		this.camObj = this.cam.gameObject;
		this.rev = this.camObj.GetComponentInChildren<Revolver>();
		this.gunAud = base.GetComponent<AudioSource>();
		this.PickUp();
		Debug.Log("Awake!");
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x000B4801 File Offset: 0x000B2A01
	private void OnDisable()
	{
		this.PickUp();
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x000B4809 File Offset: 0x000B2A09
	public void PickUp()
	{
		this.gunReady = false;
		this.shootCharge = 0f;
		this.shootReady = false;
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x000B4824 File Offset: 0x000B2A24
	private void Update()
	{
		if (this.gunReady && !MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && this.shootReady)
		{
			this.Shoot();
		}
		if (base.transform.localPosition != this.defaultGunPos && base.transform.localRotation != this.defaultGunRot)
		{
			this.gunReady = false;
		}
		else
		{
			this.gunReady = true;
		}
		if (!this.shootReady)
		{
			if (this.shootCharge + 175f * Time.deltaTime < 100f)
			{
				this.shootCharge += 175f * Time.deltaTime;
			}
			else
			{
				this.shootCharge = 100f;
				this.shootReady = true;
			}
		}
		if (base.transform.localPosition != this.defaultGunPos)
		{
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.defaultGunPos, Time.deltaTime * 3f);
		}
		if (base.transform.localRotation != this.defaultGunRot)
		{
			base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, this.defaultGunRot, Time.deltaTime * 160f);
		}
		if (this.shootCharge < 50f)
		{
			this.screenMR.material.SetTexture("_MainTex", this.rev.batteryLow);
			return;
		}
		if (this.shootCharge < 100f)
		{
			this.screenMR.material.SetTexture("_MainTex", this.rev.batteryMid);
			return;
		}
		this.screenMR.material.SetTexture("_MainTex", this.rev.batteryFull);
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x000B49FC File Offset: 0x000B2BFC
	public void Shoot()
	{
		this.bulletForce = 5000;
		Vector3 vector = this.camObj.transform.forward;
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			vector = this.targeter.CurrentTarget.bounds.center - this.camObj.transform.position;
		}
		Physics.Raycast(this.camObj.transform.position, vector, out this.hit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment));
		this.shotHitPoint = this.hit.point;
		GameObject gameObject = Object.Instantiate<GameObject>(this.secBeamPoint, this.gunBarrel.transform.position, this.gunBarrel.transform.rotation);
		GameObject gameObject2 = Object.Instantiate<GameObject>(this.secHitParticle, this.hit.point, base.transform.rotation);
		if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
		{
			gameObject.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
			gameObject2.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
		}
		this.shootReady = false;
		this.shootCharge = 0f;
		this.currentGunShot = Random.Range(0, this.rev.gunShots.Length);
		this.gunAud.clip = this.rev.gunShots[this.currentGunShot];
		this.gunAud.volume = 0.5f;
		this.gunAud.pitch = Random.Range(0.95f, 1.05f);
		this.gunAud.Play();
		this.cam.fieldOfView = (this.rev.recoilFOV - this.cam.fieldOfView) / 2f + this.cam.fieldOfView;
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x000B4C14 File Offset: 0x000B2E14
	private void ReadyToShoot()
	{
		this.shootReady = true;
	}

	// Token: 0x04001EE7 RID: 7911
	private int bulletForce;

	// Token: 0x04001EE8 RID: 7912
	private GameObject camObj;

	// Token: 0x04001EE9 RID: 7913
	private Camera cam;

	// Token: 0x04001EEA RID: 7914
	public RaycastHit hit;

	// Token: 0x04001EEB RID: 7915
	private bool gunReady;

	// Token: 0x04001EEC RID: 7916
	public Vector3 shotHitPoint;

	// Token: 0x04001EED RID: 7917
	private bool shootReady;

	// Token: 0x04001EEE RID: 7918
	private float shootCharge;

	// Token: 0x04001EEF RID: 7919
	private int currentGunShot;

	// Token: 0x04001EF0 RID: 7920
	private AudioSource gunAud;

	// Token: 0x04001EF1 RID: 7921
	public Revolver rev;

	// Token: 0x04001EF2 RID: 7922
	public GameObject secBeamPoint;

	// Token: 0x04001EF3 RID: 7923
	public GameObject secHitParticle;

	// Token: 0x04001EF4 RID: 7924
	private GameObject gunBarrel;

	// Token: 0x04001EF5 RID: 7925
	private Vector3 defaultGunPos;

	// Token: 0x04001EF6 RID: 7926
	private Quaternion defaultGunRot;

	// Token: 0x04001EF7 RID: 7927
	private MeshRenderer screenMR;

	// Token: 0x04001EF8 RID: 7928
	private CameraFrustumTargeter targeter;
}
