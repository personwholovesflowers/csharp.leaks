using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002EF RID: 751
public class MenuAlphaPop : MonoBehaviour
{
	// Token: 0x0600108A RID: 4234 RVA: 0x0007EE40 File Offset: 0x0007D040
	private void Awake()
	{
		this.targetImage = base.GetComponent<Image>();
		this.targetGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x0007EE5A File Offset: 0x0007D05A
	private void OnEnable()
	{
		if (this.animateOnEnable)
		{
			this.Animate();
		}
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x0007EE6C File Offset: 0x0007D06C
	private void Update()
	{
		if (!this.isAnimating)
		{
			return;
		}
		this.progress += Time.deltaTime / this.animationDuration;
		if (this.progress >= 1f)
		{
			this.isAnimating = false;
		}
		if (this.canvasGroupInsteadOfImage)
		{
			this.targetGroup.alpha = Mathf.Lerp(this.initialAlpha, this.finalAlpha, this.progress);
			return;
		}
		Color color = this.targetImage.color;
		color.a = Mathf.Lerp(this.initialAlpha, this.finalAlpha, this.progress);
		this.targetImage.color = color;
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x0007EF10 File Offset: 0x0007D110
	public void Animate()
	{
		this.isAnimating = true;
		this.progress = 0f;
	}

	// Token: 0x0400167E RID: 5758
	[SerializeField]
	private bool animateOnEnable = true;

	// Token: 0x0400167F RID: 5759
	[SerializeField]
	private bool canvasGroupInsteadOfImage;

	// Token: 0x04001680 RID: 5760
	[SerializeField]
	private float initialAlpha = 1f;

	// Token: 0x04001681 RID: 5761
	[SerializeField]
	private float finalAlpha;

	// Token: 0x04001682 RID: 5762
	[SerializeField]
	private float animationDuration = 1f;

	// Token: 0x04001683 RID: 5763
	private Image targetImage;

	// Token: 0x04001684 RID: 5764
	private CanvasGroup targetGroup;

	// Token: 0x04001685 RID: 5765
	private bool isAnimating;

	// Token: 0x04001686 RID: 5766
	private float progress;
}
