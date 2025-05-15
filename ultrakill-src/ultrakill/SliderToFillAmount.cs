using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200040E RID: 1038
public class SliderToFillAmount : MonoBehaviour
{
	// Token: 0x060017B0 RID: 6064 RVA: 0x000C259B File Offset: 0x000C079B
	private void OnEnable()
	{
		if (this.img == null)
		{
			this.img = base.GetComponent<Image>();
		}
		this.lastInvisible = !this.isInvisible;
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x000C25C8 File Offset: 0x000C07C8
	private void Update()
	{
		this.isInvisible = Mathf.Approximately(this.img.color.a, 0f);
		if (this.isInvisible != this.lastInvisible)
		{
			this.img.enabled = !this.isInvisible;
			this.lastInvisible = this.isInvisible;
		}
		float fillAmount = this.img.fillAmount;
		float num = (this.targetSlider.value - this.targetSlider.minValue) / (this.targetSlider.maxValue - this.targetSlider.minValue) * this.maxFill;
		if (num != fillAmount)
		{
			this.img.fillAmount = num;
			this.ResetFadeTimer();
		}
		if (this.copyColor)
		{
			Color color = this.img.color;
			Color color2 = this.targetSlider.targetGraphic.color;
			if (color2 != color)
			{
				this.img.color = color2;
			}
		}
		if (this.mama != null)
		{
			Color color3 = this.img.color;
			float num2 = ((this.mama.fadeOutTime < 1f) ? this.mama.fadeOutTime : 1f);
			if (num2 != color3.a)
			{
				color3.a = num2;
				this.img.color = color3;
			}
		}
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x000C2718 File Offset: 0x000C0918
	private void ResetFadeTimer()
	{
		if (this.mama)
		{
			this.mama.ResetTimer();
		}
	}

	// Token: 0x0400210B RID: 8459
	public Slider targetSlider;

	// Token: 0x0400210C RID: 8460
	public float maxFill;

	// Token: 0x0400210D RID: 8461
	public bool copyColor;

	// Token: 0x0400210E RID: 8462
	private Image img;

	// Token: 0x0400210F RID: 8463
	public FadeOutBars mama;

	// Token: 0x04002110 RID: 8464
	public bool dontFadeUntilEmpty;

	// Token: 0x04002111 RID: 8465
	private bool isInvisible;

	// Token: 0x04002112 RID: 8466
	private bool lastInvisible;
}
