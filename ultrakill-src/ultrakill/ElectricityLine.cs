using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class ElectricityLine : MonoBehaviour
{
	// Token: 0x060005FE RID: 1534 RVA: 0x00029254 File Offset: 0x00027454
	private void Awake()
	{
		this.animatedTexture = base.GetComponent<AnimatedTexture>();
		if (this.animatedTexture == null)
		{
			Debug.LogError("This asset needs to be updated to the new electricity setup", base.gameObject);
		}
		this.animatedTexture.arrayTex = this.electricityArray;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x00029294 File Offset: 0x00027494
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
		this.animatedTexture.SetArraySlice(Random.Range(0, this.electricityArray.depth));
		this.lr.widthMultiplier = Random.Range(this.minWidth, this.maxWidth);
		this.lr.startColor = this.colors.Evaluate(Random.Range(0f, 1f));
		this.lr.endColor = this.colors.Evaluate(Random.Range(0f, 1f));
		this.lr.startColor = new Color(this.lr.startColor.r, this.lr.startColor.g, this.lr.startColor.b, this.lr.startColor.a * this.fadeLerp);
		this.lr.endColor = new Color(this.lr.endColor.r, this.lr.endColor.g, this.lr.endColor.b, this.lr.endColor.a * this.fadeLerp);
	}

	// Token: 0x04000804 RID: 2052
	private LineRenderer lr;

	// Token: 0x04000805 RID: 2053
	public Texture2DArray electricityArray;

	// Token: 0x04000806 RID: 2054
	public float minWidth;

	// Token: 0x04000807 RID: 2055
	public float maxWidth;

	// Token: 0x04000808 RID: 2056
	public Gradient colors;

	// Token: 0x04000809 RID: 2057
	private float cooldown;

	// Token: 0x0400080A RID: 2058
	public float fadeSpeed;

	// Token: 0x0400080B RID: 2059
	private float fadeLerp = 1f;

	// Token: 0x0400080C RID: 2060
	private AnimatedTexture animatedTexture;
}
