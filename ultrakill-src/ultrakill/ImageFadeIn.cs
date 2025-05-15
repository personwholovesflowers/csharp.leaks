using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000267 RID: 615
public class ImageFadeIn : MonoBehaviour
{
	// Token: 0x06000D85 RID: 3461 RVA: 0x0006657C File Offset: 0x0006477C
	private void Start()
	{
		this.img = base.GetComponent<Image>();
		this.text = base.GetComponent<TMP_Text>();
		if (this.maxAlpha == 0f)
		{
			this.maxAlpha = 1f;
		}
		if (this.startAt0)
		{
			if (this.img)
			{
				this.img.color = new Color(this.img.color.r, this.img.color.g, this.img.color.b, 0f);
			}
			if (this.text)
			{
				this.text.color = new Color(this.text.color.r, this.text.color.g, this.text.color.b, 0f);
			}
		}
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x00066668 File Offset: 0x00064868
	private void Update()
	{
		if (this.img && this.img.color.a != this.maxAlpha)
		{
			Color color = this.img.color;
			color.a = Mathf.MoveTowards(color.a, this.maxAlpha, Time.deltaTime * this.speed);
			this.img.color = color;
			if (this.img.color.a == this.maxAlpha)
			{
				UnityEvent unityEvent = this.onFull;
				if (unityEvent != null)
				{
					unityEvent.Invoke();
				}
			}
		}
		if (this.text && this.text.color.a != this.maxAlpha)
		{
			Color color2 = this.text.color;
			color2.a = Mathf.MoveTowards(color2.a, this.maxAlpha, Time.deltaTime * this.speed);
			this.text.color = color2;
			if (this.text.color.a == this.maxAlpha)
			{
				UnityEvent unityEvent2 = this.onFull;
				if (unityEvent2 == null)
				{
					return;
				}
				unityEvent2.Invoke();
			}
		}
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x00066788 File Offset: 0x00064988
	public void ResetFade()
	{
		if (this.img)
		{
			this.img.color = new Color(this.img.color.r, this.img.color.g, this.img.color.b, 0f);
		}
		if (this.text)
		{
			this.text.color = new Color(this.text.color.r, this.text.color.g, this.text.color.b, 0f);
		}
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x00066839 File Offset: 0x00064A39
	public void CancelFade()
	{
		this.ResetFade();
		base.enabled = false;
	}

	// Token: 0x0400120A RID: 4618
	private Image img;

	// Token: 0x0400120B RID: 4619
	private TMP_Text text;

	// Token: 0x0400120C RID: 4620
	public float speed;

	// Token: 0x0400120D RID: 4621
	public float maxAlpha = 1f;

	// Token: 0x0400120E RID: 4622
	public bool startAt0;

	// Token: 0x0400120F RID: 4623
	public UnityEvent onFull;
}
