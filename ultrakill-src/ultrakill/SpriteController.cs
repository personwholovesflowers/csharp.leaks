using System;
using UnityEngine;

// Token: 0x02000439 RID: 1081
public class SpriteController : MonoBehaviour
{
	// Token: 0x06001858 RID: 6232 RVA: 0x000C6B2C File Offset: 0x000C4D2C
	private void Awake()
	{
		if (this.randomRotation != Vector3.zero)
		{
			base.transform.Rotate(this.randomRotation * (float)Random.Range(0, 360));
		}
		this.spr = base.GetComponent<SpriteRenderer>();
		this.originalAlpha = this.spr.color.a;
		this.originalScale = base.transform.localScale;
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x000C6BA0 File Offset: 0x000C4DA0
	private void Update()
	{
		if (this.fadeSpeed != 0f)
		{
			if (this.blinking)
			{
				if (this.spr.color.a >= this.originalAlpha && this.fadeSpeed < 0f)
				{
					this.fadeSpeed = Mathf.Abs(this.fadeSpeed);
				}
				else if (this.spr.color.a <= 0f && this.fadeSpeed > 0f)
				{
					this.fadeSpeed = Mathf.Abs(this.fadeSpeed) * -1f;
				}
			}
			Color color = this.spr.color;
			color.a -= this.fadeSpeed * Time.deltaTime;
			this.spr.color = color;
		}
		if (this.shrinkSpeed > 0f)
		{
			base.transform.localScale = base.transform.localScale - Vector3.one * this.shrinkSpeed * Time.deltaTime;
		}
	}

	// Token: 0x04002229 RID: 8745
	public Vector3 randomRotation;

	// Token: 0x0400222A RID: 8746
	public bool blinking;

	// Token: 0x0400222B RID: 8747
	public float fadeSpeed;

	// Token: 0x0400222C RID: 8748
	public float shrinkSpeed;

	// Token: 0x0400222D RID: 8749
	private SpriteRenderer spr;

	// Token: 0x0400222E RID: 8750
	private float originalAlpha;

	// Token: 0x0400222F RID: 8751
	private Vector3 originalScale;
}
