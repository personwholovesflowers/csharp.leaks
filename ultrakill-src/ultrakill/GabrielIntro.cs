using System;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class GabrielIntro : MonoBehaviour
{
	// Token: 0x06000AAF RID: 2735 RVA: 0x0004C599 File Offset: 0x0004A799
	public void Begin()
	{
		this.anim = base.GetComponent<Animator>();
		this.anim.Play("Intro");
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0004C5B8 File Offset: 0x0004A7B8
	private void Update()
	{
		if (this.shaking)
		{
			this.shakeAmount = Mathf.MoveTowards(this.shakeAmount, 0.125f, Time.deltaTime * 0.0125f);
			MonoSingleton<CameraController>.Instance.CameraShake(this.shakeAmount * 2f);
			base.transform.position = new Vector3(this.defaultPos.x + Random.Range(-this.shakeAmount, this.shakeAmount), this.defaultPos.y + Random.Range(-this.shakeAmount, this.shakeAmount), this.defaultPos.z + Random.Range(-this.shakeAmount, this.shakeAmount));
		}
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0004C674 File Offset: 0x0004A874
	private void LateUpdate()
	{
		if (this.tracking)
		{
			this.root.rotation = Quaternion.RotateTowards(this.previousRotation, Quaternion.LookRotation(MonoSingleton<PlayerTracker>.Instance.GetTarget().position - this.root.position), Time.deltaTime * 720f);
			this.previousRotation = this.root.rotation;
		}
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x0004C6DF File Offset: 0x0004A8DF
	private void StartShaking()
	{
		this.shaking = true;
		this.defaultPos = base.transform.position;
		this.rumbling.SetActive(true);
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x0004C705 File Offset: 0x0004A905
	private void StopShaking()
	{
		this.shaking = false;
		base.transform.position = this.defaultPos;
		this.rumbling.SetActive(false);
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x0004C72B File Offset: 0x0004A92B
	private void StartTracking()
	{
		this.tracking = true;
		this.previousRotation = this.root.rotation;
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x0004C748 File Offset: 0x0004A948
	private void SwordPull(int sword)
	{
		if (sword == 1)
		{
			this.sword1.enabled = true;
			this.fakeSword1.SetActive(false);
		}
		else if (sword == 2)
		{
			this.sword2.enabled = true;
			this.fakeSword2.SetActive(false);
		}
		Object.Instantiate<AudioSource>(this.swordUnsheatheSound, this.root.position, Quaternion.identity);
	}

	// Token: 0x04000E38 RID: 3640
	private Animator anim;

	// Token: 0x04000E39 RID: 3641
	private bool shaking;

	// Token: 0x04000E3A RID: 3642
	private bool tracking;

	// Token: 0x04000E3B RID: 3643
	private Quaternion previousRotation;

	// Token: 0x04000E3C RID: 3644
	private Vector3 defaultPos;

	// Token: 0x04000E3D RID: 3645
	private float shakeAmount;

	// Token: 0x04000E3E RID: 3646
	[SerializeField]
	private Transform root;

	// Token: 0x04000E3F RID: 3647
	[SerializeField]
	private SkinnedMeshRenderer sword1;

	// Token: 0x04000E40 RID: 3648
	[SerializeField]
	private GameObject fakeSword1;

	// Token: 0x04000E41 RID: 3649
	[SerializeField]
	private SkinnedMeshRenderer sword2;

	// Token: 0x04000E42 RID: 3650
	[SerializeField]
	private GameObject fakeSword2;

	// Token: 0x04000E43 RID: 3651
	[SerializeField]
	private AudioSource swordUnsheatheSound;

	// Token: 0x04000E44 RID: 3652
	[SerializeField]
	private GameObject rumbling;
}
