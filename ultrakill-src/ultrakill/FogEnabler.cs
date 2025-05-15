using System;
using UnityEngine;

// Token: 0x020001F9 RID: 505
public class FogEnabler : MonoBehaviour
{
	// Token: 0x06000A50 RID: 2640 RVA: 0x00048BA8 File Offset: 0x00046DA8
	private void Awake()
	{
		Collider collider;
		Rigidbody rigidbody;
		if (!base.TryGetComponent<Collider>(out collider) && !base.TryGetComponent<Rigidbody>(out rigidbody))
		{
			this.colliderless = true;
		}
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x00048BD0 File Offset: 0x00046DD0
	private void OnEnable()
	{
		if (this.colliderless)
		{
			this.Activate();
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00048BE0 File Offset: 0x00046DE0
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform == MonoSingleton<NewMovement>.Instance.transform)
		{
			this.Activate();
		}
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00048C00 File Offset: 0x00046E00
	private void Activate()
	{
		if (this.oneTime && this.activated)
		{
			return;
		}
		this.activated = true;
		if (this.changeFogSettings)
		{
			RenderSettings.fogColor = this.fogColor;
			if (!this.gradual)
			{
				RenderSettings.fogStartDistance = this.fogMinimum;
				RenderSettings.fogEndDistance = this.fogMaximum;
			}
		}
		if (this.gradual)
		{
			if (this.disable)
			{
				MonoSingleton<FogFadeController>.Instance.FadeOut(true, this.gradualFadeSpeed);
			}
			else if (this.changeFogSettings)
			{
				MonoSingleton<FogFadeController>.Instance.FadeIn(this.fogMinimum, this.fogMaximum, true, this.gradualFadeSpeed);
			}
			else
			{
				MonoSingleton<FogFadeController>.Instance.FadeIn(RenderSettings.fogStartDistance, RenderSettings.fogEndDistance, true, this.gradualFadeSpeed);
			}
		}
		else
		{
			RenderSettings.fog = !this.disable;
		}
		if (this.changeLimboSkyboxFogSettings)
		{
			LimboSkybox[] array = Object.FindObjectsOfType<LimboSkybox>(true);
			if (array == null || array.Length == 0)
			{
				return;
			}
			foreach (LimboSkybox limboSkybox in array)
			{
				limboSkybox.fogColor = this.limboSkyboxFogColor;
				limboSkybox.fogStart = this.limboSkyboxFogMinimum;
				limboSkybox.fogEnd = this.limboSkyboxFogMaximum;
			}
		}
	}

	// Token: 0x04000DA4 RID: 3492
	public bool disable;

	// Token: 0x04000DA5 RID: 3493
	public bool oneTime;

	// Token: 0x04000DA6 RID: 3494
	private bool activated;

	// Token: 0x04000DA7 RID: 3495
	private bool colliderless;

	// Token: 0x04000DA8 RID: 3496
	public bool gradual;

	// Token: 0x04000DA9 RID: 3497
	public float gradualFadeSpeed = 10f;

	// Token: 0x04000DAA RID: 3498
	[Space]
	public bool changeFogSettings;

	// Token: 0x04000DAB RID: 3499
	public Color fogColor;

	// Token: 0x04000DAC RID: 3500
	public float fogMinimum;

	// Token: 0x04000DAD RID: 3501
	public float fogMaximum;

	// Token: 0x04000DAE RID: 3502
	[Space]
	public bool changeLimboSkyboxFogSettings;

	// Token: 0x04000DAF RID: 3503
	public Color limboSkyboxFogColor;

	// Token: 0x04000DB0 RID: 3504
	public float limboSkyboxFogMinimum;

	// Token: 0x04000DB1 RID: 3505
	public float limboSkyboxFogMaximum;
}
