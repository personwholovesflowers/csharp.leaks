using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class AmbientGlow : MonoBehaviour
{
	// Token: 0x060001CE RID: 462 RVA: 0x0000973B File Offset: 0x0000793B
	private void Start()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
		this.originalAlpha = this.sr.color.a;
		this.target = this.originalAlpha + this.glowVariance;
	}

	// Token: 0x060001CF RID: 463 RVA: 0x00009774 File Offset: 0x00007974
	private void Update()
	{
		this.clr = this.sr.color;
		this.clr.a = Mathf.MoveTowards(this.sr.color.a, this.target, Time.deltaTime * this.glowSpeed);
		this.sr.color = this.clr;
		if (this.clr.a == this.target)
		{
			if (this.target > this.originalAlpha)
			{
				this.target = this.originalAlpha - this.glowVariance;
				return;
			}
			this.target = this.originalAlpha + this.glowVariance;
		}
	}

	// Token: 0x040001EA RID: 490
	private SpriteRenderer sr;

	// Token: 0x040001EB RID: 491
	private float originalAlpha;

	// Token: 0x040001EC RID: 492
	public float glowVariance = 0.2f;

	// Token: 0x040001ED RID: 493
	public float glowSpeed = 0.2f;

	// Token: 0x040001EE RID: 494
	private float target;

	// Token: 0x040001EF RID: 495
	private Color clr;
}
