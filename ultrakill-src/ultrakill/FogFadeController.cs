using System;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class FogFadeController : MonoSingleton<FogFadeController>
{
	// Token: 0x06000A55 RID: 2645 RVA: 0x00048D2C File Offset: 0x00046F2C
	private void Update()
	{
		if (!this.fading)
		{
			return;
		}
		if (this.autoDetect && (RenderSettings.fogColor != this.previousFogColor || this.previousFogMin != RenderSettings.fogStartDistance || this.previousFogMax != RenderSettings.fogEndDistance))
		{
			this.fading = false;
			return;
		}
		this.previousFogMin = Mathf.MoveTowards(this.previousFogMin, this.minTarget, Time.deltaTime * this.speed);
		this.previousFogMax = Mathf.MoveTowards(this.previousFogMax, this.maxTarget, Time.deltaTime * this.speed);
		RenderSettings.fogStartDistance = this.previousFogMin;
		RenderSettings.fogEndDistance = this.previousFogMax;
		if (this.previousFogMin == this.minTarget && this.previousFogMax == this.maxTarget)
		{
			if (this.toDisable)
			{
				RenderSettings.fog = false;
			}
			this.fading = false;
			return;
		}
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x00048E0C File Offset: 0x0004700C
	public void FadeOut(bool autoDetectFogChange = true, float fadeSpeed = 10f)
	{
		if (!RenderSettings.fog)
		{
			return;
		}
		this.autoDetect = autoDetectFogChange;
		this.speed = fadeSpeed;
		this.previousFogColor = RenderSettings.fogColor;
		this.previousFogMin = RenderSettings.fogStartDistance;
		this.previousFogMax = RenderSettings.fogEndDistance;
		this.minTarget = this.previousFogMax;
		this.maxTarget = this.previousFogMax;
		this.toDisable = true;
		this.fading = true;
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x00048E78 File Offset: 0x00047078
	public void FadeIn(float newMin, float newMax, bool autoDetectFogChange = true, float fadeSpeed = 10f)
	{
		this.autoDetect = autoDetectFogChange;
		this.speed = fadeSpeed;
		if (!RenderSettings.fog)
		{
			RenderSettings.fogStartDistance = newMax;
			RenderSettings.fogEndDistance = newMax;
			RenderSettings.fog = true;
		}
		this.previousFogColor = RenderSettings.fogColor;
		this.previousFogMin = RenderSettings.fogStartDistance;
		this.previousFogMax = RenderSettings.fogEndDistance;
		this.minTarget = newMin;
		this.maxTarget = newMax;
		this.toDisable = false;
		this.fading = true;
	}

	// Token: 0x04000DB2 RID: 3506
	private bool fading;

	// Token: 0x04000DB3 RID: 3507
	private bool toDisable;

	// Token: 0x04000DB4 RID: 3508
	private bool autoDetect;

	// Token: 0x04000DB5 RID: 3509
	private float speed;

	// Token: 0x04000DB6 RID: 3510
	private Color previousFogColor;

	// Token: 0x04000DB7 RID: 3511
	private float previousFogMin;

	// Token: 0x04000DB8 RID: 3512
	private float previousFogMax;

	// Token: 0x04000DB9 RID: 3513
	private float minTarget;

	// Token: 0x04000DBA RID: 3514
	private float maxTarget;
}
