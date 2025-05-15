using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class AnimationSpeedRandomizer : MonoBehaviour
{
	// Token: 0x060001DE RID: 478 RVA: 0x00009BDC File Offset: 0x00007DDC
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
		this.anim.speed = this.speed + Random.Range(-this.maxRandomness, this.maxRandomness);
		if (this.randomizePlaybackPosition)
		{
			this.anim.Play(0, -1, Random.Range(0f, 1f));
		}
	}

	// Token: 0x04000204 RID: 516
	private Animator anim;

	// Token: 0x04000205 RID: 517
	public float speed = 1f;

	// Token: 0x04000206 RID: 518
	public float maxRandomness = 0.1f;

	// Token: 0x04000207 RID: 519
	public bool randomizePlaybackPosition;
}
