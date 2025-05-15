using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class ElectricityLine_Old : MonoBehaviour
{
	// Token: 0x06000601 RID: 1537 RVA: 0x0002946C File Offset: 0x0002766C
	private void Update()
	{
		this.fadeLerp = Mathf.MoveTowards(this.fadeLerp, 0f, Time.deltaTime * this.fadeSpeed);
		if (this.fadeLerp <= 0f)
		{
			base.gameObject.SetActive(false);
		}
		if (this.cooldown > 0f)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
			return;
		}
		this.cooldown = 0.05f;
		if (!this.lr)
		{
			this.lr = base.GetComponent<LineRenderer>();
		}
		this.lr.material = this.lightningMats[Random.Range(0, this.lightningMats.Length)];
		this.lr.widthMultiplier = Random.Range(this.minWidth, this.maxWidth);
		this.lr.startColor = this.colors.Evaluate(Random.Range(0f, 1f));
		this.lr.endColor = this.colors.Evaluate(Random.Range(0f, 1f));
		this.lr.startColor = new Color(this.lr.startColor.r, this.lr.startColor.g, this.lr.startColor.b, this.lr.startColor.a * this.fadeLerp);
		this.lr.endColor = new Color(this.lr.endColor.r, this.lr.endColor.g, this.lr.endColor.b, this.lr.endColor.a * this.fadeLerp);
	}

	// Token: 0x0400080D RID: 2061
	private LineRenderer lr;

	// Token: 0x0400080E RID: 2062
	public Material[] lightningMats;

	// Token: 0x0400080F RID: 2063
	public float minWidth;

	// Token: 0x04000810 RID: 2064
	public float maxWidth;

	// Token: 0x04000811 RID: 2065
	public Gradient colors;

	// Token: 0x04000812 RID: 2066
	private float cooldown;

	// Token: 0x04000813 RID: 2067
	public float fadeSpeed;

	// Token: 0x04000814 RID: 2068
	private float fadeLerp = 1f;
}
