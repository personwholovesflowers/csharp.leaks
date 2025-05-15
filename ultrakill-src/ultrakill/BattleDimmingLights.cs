using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class BattleDimmingLights : MonoBehaviour
{
	// Token: 0x06000216 RID: 534 RVA: 0x0000ACC0 File Offset: 0x00008EC0
	private void Start()
	{
		this.lights = base.GetComponentsInChildren<Light>();
		this.intensities = new float[this.lights.Length];
		for (int i = 0; i < this.lights.Length; i++)
		{
			this.intensities[i] = this.lights[i].intensity;
		}
		this.originalAmbientLightColor = RenderSettings.ambientLight;
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000AD20 File Offset: 0x00008F20
	private void Update()
	{
		if (MonoSingleton<PrefsManager>.Instance.GetBool("level_7-1.alwaysDark", false))
		{
			for (int i = 0; i < this.lights.Length; i++)
			{
				this.lights[i].intensity = 0f;
			}
			if (this.dimAmbientLight)
			{
				RenderSettings.ambientLight = this.dimmedAmbientLightColor;
				return;
			}
		}
		else if (!this.disabledUnlessAlwaysDark && (MonoSingleton<MusicManager>.Instance.IsInBattle() || this.lerp < 1f))
		{
			this.lerp = Mathf.MoveTowards(this.lerp, (float)(MonoSingleton<MusicManager>.Instance.IsInBattle() ? 0 : 1), Time.deltaTime * this.speedMultiplier);
			for (int j = 0; j < this.lights.Length; j++)
			{
				this.lights[j].intensity = Mathf.Lerp(0f, this.intensities[j], this.lerp);
			}
			if (this.dimAmbientLight)
			{
				RenderSettings.ambientLight = Color.Lerp(this.dimmedAmbientLightColor, this.originalAmbientLightColor, this.lerp);
			}
		}
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000AE2C File Offset: 0x0000902C
	public void Active(bool stuff)
	{
		this.disabledUnlessAlwaysDark = !stuff;
	}

	// Token: 0x04000248 RID: 584
	private Light[] lights;

	// Token: 0x04000249 RID: 585
	private float[] intensities;

	// Token: 0x0400024A RID: 586
	private float lerp = 1f;

	// Token: 0x0400024B RID: 587
	public float speedMultiplier = 1f;

	// Token: 0x0400024C RID: 588
	public bool disabledUnlessAlwaysDark;

	// Token: 0x0400024D RID: 589
	[Header("Ambient Color")]
	public bool dimAmbientLight;

	// Token: 0x0400024E RID: 590
	private Color originalAmbientLightColor;

	// Token: 0x0400024F RID: 591
	public Color dimmedAmbientLightColor;
}
