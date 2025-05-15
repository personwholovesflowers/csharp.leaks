using System;
using UnityEngine;

// Token: 0x020002E8 RID: 744
public class MannequinPoses : MonoBehaviour
{
	// Token: 0x06001041 RID: 4161 RVA: 0x0007CA34 File Offset: 0x0007AC34
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
		if (this.altar)
		{
			this.anim.Play("Altar");
			base.enabled = false;
			return;
		}
		if (!this.beenActivated)
		{
			this.beenActivated = true;
			this.RandomPose();
		}
		else
		{
			this.ChangePose(this.currentPose);
		}
		this.SlowUpdate();
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0007CA98 File Offset: 0x0007AC98
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", Random.Range(1f, 3f));
		if (Vector3.Dot(MonoSingleton<CameraController>.Instance.transform.forward, base.transform.position - MonoSingleton<CameraController>.Instance.transform.position) < -0.33f)
		{
			this.RandomPose();
		}
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x0007CAFF File Offset: 0x0007ACFF
	private void RandomPose()
	{
		this.ChangePose(Random.Range(1, 10));
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0007CB0F File Offset: 0x0007AD0F
	private void ChangePose(int num)
	{
		this.currentPose = num;
		this.anim.SetInteger("TargetPose", this.currentPose);
	}

	// Token: 0x04001622 RID: 5666
	private Animator anim;

	// Token: 0x04001623 RID: 5667
	[SerializeField]
	private bool altar;

	// Token: 0x04001624 RID: 5668
	[HideInInspector]
	public bool beenActivated;

	// Token: 0x04001625 RID: 5669
	[HideInInspector]
	public int currentPose;
}
