using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class BlackHoleCannon : MonoBehaviour
{
	// Token: 0x0600023E RID: 574 RVA: 0x0000BFAD File Offset: 0x0000A1AD
	private void Start()
	{
		this.cam = Camera.main.gameObject;
		this.cc = this.cam.GetComponent<CameraController>();
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000BFDC File Offset: 0x0000A1DC
	private void Update()
	{
		if (!MonoSingleton<GunControl>.Instance.activated)
		{
			return;
		}
		if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame)
		{
			if (!this.currentbh)
			{
				this.Shoot();
			}
			else
			{
				this.aud.PlayOneShot(this.emptyClick, 1f);
			}
		}
		BlackHoleProjectile blackHoleProjectile;
		if (this.currentbh != null && MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame && this.currentbh.TryGetComponent<BlackHoleProjectile>(out blackHoleProjectile))
		{
			blackHoleProjectile.Activate();
		}
	}

	// Token: 0x06000240 RID: 576 RVA: 0x0000C07C File Offset: 0x0000A27C
	private void Shoot()
	{
		Vector3 vector = this.cam.transform.position + this.cam.transform.forward;
		this.currentbh = Object.Instantiate<GameObject>(this.bh, vector, this.cam.transform.rotation);
		if (Physics.Raycast(this.cam.transform.position, this.cam.transform.forward, out this.rhit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
		{
			this.currentbh.transform.LookAt(this.rhit.point);
		}
		else
		{
			this.currentbh.transform.rotation = this.cam.transform.rotation;
		}
		this.aud.Play();
		this.cc.CameraShake(0.5f);
	}

	// Token: 0x04000290 RID: 656
	public Transform shootPoint;

	// Token: 0x04000291 RID: 657
	public GameObject bh;

	// Token: 0x04000292 RID: 658
	private GameObject currentbh;

	// Token: 0x04000293 RID: 659
	private RaycastHit rhit;

	// Token: 0x04000294 RID: 660
	private GameObject cam;

	// Token: 0x04000295 RID: 661
	private CameraController cc;

	// Token: 0x04000296 RID: 662
	private AudioSource aud;

	// Token: 0x04000297 RID: 663
	private WeaponHUD whud;

	// Token: 0x04000298 RID: 664
	public AudioClip emptyClick;
}
