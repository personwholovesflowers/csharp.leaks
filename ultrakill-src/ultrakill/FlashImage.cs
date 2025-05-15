using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F4 RID: 500
public class FlashImage : MonoBehaviour
{
	// Token: 0x06000A32 RID: 2610 RVA: 0x00046BA7 File Offset: 0x00044DA7
	private void OnEnable()
	{
		if (!this.dontFlashOnEnable && (!this.oneTime || !this.flashed))
		{
			this.Flash(this.flashAlpha);
		}
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x00046BD0 File Offset: 0x00044DD0
	public void Flash(float amount)
	{
		if (this.oneTime)
		{
			if (this.flashed)
			{
				return;
			}
			this.flashed = true;
		}
		if (!this.img)
		{
			this.img = base.GetComponent<Image>();
		}
		this.img.color = new Color(this.img.color.r, this.img.color.g, this.img.color.b, amount);
		this.flashing = true;
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x00046C58 File Offset: 0x00044E58
	private void Update()
	{
		if (this.flashing && this.img)
		{
			this.img.color = new Color(this.img.color.r, this.img.color.g, this.img.color.b, Mathf.MoveTowards(this.img.color.a, 0f, Time.deltaTime * this.speed));
			if (this.img.color.a <= 0f)
			{
				this.flashing = false;
			}
		}
	}

	// Token: 0x04000D52 RID: 3410
	private Image img;

	// Token: 0x04000D53 RID: 3411
	private bool flashing;

	// Token: 0x04000D54 RID: 3412
	public float speed;

	// Token: 0x04000D55 RID: 3413
	public float flashAlpha;

	// Token: 0x04000D56 RID: 3414
	public bool dontFlashOnEnable;

	// Token: 0x04000D57 RID: 3415
	public bool oneTime;

	// Token: 0x04000D58 RID: 3416
	private bool flashed;
}
