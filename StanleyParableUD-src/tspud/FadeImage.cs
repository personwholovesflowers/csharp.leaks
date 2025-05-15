using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000056 RID: 86
[RequireComponent(typeof(Image))]
public class FadeImage : MonoBehaviour
{
	// Token: 0x0600022A RID: 554 RVA: 0x0001008E File Offset: 0x0000E28E
	private void Awake()
	{
		this.img = base.GetComponent<Image>();
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0001009C File Offset: 0x0000E29C
	private void Start()
	{
		if (this.selfStart)
		{
			this.StartFade();
		}
	}

	// Token: 0x0600022C RID: 556 RVA: 0x000100AC File Offset: 0x0000E2AC
	public void StartFade()
	{
		base.StartCoroutine(this.Fade());
	}

	// Token: 0x0600022D RID: 557 RVA: 0x000100BB File Offset: 0x0000E2BB
	private IEnumerator Fade()
	{
		float progress = 0f;
		yield return new WaitForSeconds(this.delay);
		while (progress < 1f)
		{
			progress += Time.deltaTime * (1f / this.duration);
			this.img.color = Color.Lerp(this.fromColor, this.toColor, progress);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400024C RID: 588
	private Image img;

	// Token: 0x0400024D RID: 589
	[SerializeField]
	private Color fromColor;

	// Token: 0x0400024E RID: 590
	[SerializeField]
	private Color toColor;

	// Token: 0x0400024F RID: 591
	[SerializeField]
	private bool selfStart;

	// Token: 0x04000250 RID: 592
	[SerializeField]
	private float duration = 2f;

	// Token: 0x04000251 RID: 593
	[SerializeField]
	private float delay = 1f;
}
