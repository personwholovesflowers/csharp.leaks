using System;
using TMPro;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class FlashingText : MonoBehaviour
{
	// Token: 0x06000A36 RID: 2614 RVA: 0x00046D04 File Offset: 0x00044F04
	private void Start()
	{
		this.text = base.GetComponent<TextMeshProUGUI>();
		this.originalColor = this.text.color;
		this.text.color = this.flashColor;
		if (this.forcePreciseTiming)
		{
			base.Invoke("Flash", this.fadeTime + this.delay);
			return;
		}
		this.Flash();
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x00046D68 File Offset: 0x00044F68
	private void Update()
	{
		if (this.matchToMusic.Length != 0)
		{
			for (int i = this.matchToMusic.Length - 1; i >= 0; i--)
			{
				if (this.matchToMusic[i].isPlaying)
				{
					float num = this.matchToMusic[i].time % (this.fadeTime + this.delay);
					this.text.color = Color.Lerp(this.flashColor, this.originalColor, num);
					if (this.previousLerp > num)
					{
						UltrakillEvent ultrakillEvent = this.onFlash;
						if (ultrakillEvent != null)
						{
							ultrakillEvent.Invoke("");
						}
					}
					this.previousLerp = num;
					return;
				}
			}
			return;
		}
		this.fading = Mathf.MoveTowards(this.fading, 0f, Time.deltaTime / this.fadeTime);
		this.text.color = Color.Lerp(this.originalColor, this.flashColor, this.fading);
		if (this.fading == 0f)
		{
			if (this.cooldown != 0f)
			{
				this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
			}
			if (this.cooldown == 0f)
			{
				this.Flash();
			}
		}
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x00046E94 File Offset: 0x00045094
	private void Flash()
	{
		this.fading = 1f;
		this.cooldown = this.delay;
		this.text.color = Color.Lerp(this.originalColor, this.flashColor, 1f);
		if (this.forcePreciseTiming)
		{
			base.Invoke("Flash", this.fadeTime + this.delay);
		}
		UltrakillEvent ultrakillEvent = this.onFlash;
		if (ultrakillEvent == null)
		{
			return;
		}
		ultrakillEvent.Invoke("");
	}

	// Token: 0x04000D59 RID: 3417
	private TextMeshProUGUI text;

	// Token: 0x04000D5A RID: 3418
	private Color originalColor;

	// Token: 0x04000D5B RID: 3419
	public Color flashColor;

	// Token: 0x04000D5C RID: 3420
	public float fadeTime;

	// Token: 0x04000D5D RID: 3421
	private float fading = 1f;

	// Token: 0x04000D5E RID: 3422
	public float delay;

	// Token: 0x04000D5F RID: 3423
	private float cooldown;

	// Token: 0x04000D60 RID: 3424
	public bool forcePreciseTiming;

	// Token: 0x04000D61 RID: 3425
	private float previousLerp;

	// Token: 0x04000D62 RID: 3426
	public AudioSource[] matchToMusic;

	// Token: 0x04000D63 RID: 3427
	public UltrakillEvent onFlash;
}
