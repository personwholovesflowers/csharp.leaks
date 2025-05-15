using System;
using UnityEngine;

// Token: 0x0200018D RID: 397
[ExecuteInEditMode]
public class ShaderValues : MonoBehaviour
{
	// Token: 0x06000930 RID: 2352 RVA: 0x0002B482 File Offset: 0x00029682
	private void Awake()
	{
		this.SetVariables();
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0002B48C File Offset: 0x0002968C
	private void SetVariables()
	{
		Shader.SetGlobalFloat("Stanley_LightmapSimpleContrast", this.Stanley_LightmapSimpleContrast);
		Shader.SetGlobalFloat("Stanley_LightmapPower", this.Stanley_LightmapPower);
		Shader.SetGlobalInt("Stanley_AverageRoughnessMip", this.Stanley_AverageRoughnessMip);
		Shader.SetGlobalFloat("Stanley_AverageRoughnessContrast", this.Stanley_AverageRoughnessContrast);
		Shader.SetGlobalFloat("Stanley_AverageRoughnessMultiply", this.Stanley_AverageRoughnessMultiply);
		Shader.SetGlobalFloat("Stanley_ViewAngleContrast", this.Stanley_ViewAngleContrast);
		Shader.SetGlobalFloat("Stanley_ViewAngleMultiplier", this.Stanley_ViewAngleMultiplier);
		Shader.SetGlobalFloat("Stanley_ReflectionMasterContrast", this.Stanley_ReflectionMasterContrast);
		Shader.SetGlobalFloat("Stanley_ReflectionMasterMultiplier", this.Stanley_ReflectionMasterMultiplier);
		Shader.SetGlobalFloat("Stanley_ReflectionMaskMultiplier", this.Stanley_ReflectionMaskMultiplier);
		Shader.SetGlobalFloat("Stanley_ReflectionDesaturation", this.Stanley_ReflectionDesaturation);
		Shader.SetGlobalFloat("Stanley_ReflectionLightingContrast", this.Stanley_ReflectionLightingContrast);
		Shader.SetGlobalFloat("StylizedFresnelBias", this.StylizedFresnelBias);
		Shader.SetGlobalFloat("StylizedFresnelScale", this.StylizedFresnelScale);
		Shader.SetGlobalFloat("StylizedFresnelPower", this.StylizedFresnelPower);
	}

	// Token: 0x04000900 RID: 2304
	[Header("Lightmap/Lighting")]
	[SerializeField]
	private float Stanley_LightmapSimpleContrast = 1f;

	// Token: 0x04000901 RID: 2305
	[SerializeField]
	private float Stanley_ReflectionLightingContrast = 2.75f;

	// Token: 0x04000902 RID: 2306
	[SerializeField]
	private float Stanley_LightmapPower = 1f;

	// Token: 0x04000903 RID: 2307
	[Header("Roughness")]
	[SerializeField]
	private int Stanley_AverageRoughnessMip = 3;

	// Token: 0x04000904 RID: 2308
	[SerializeField]
	private float Stanley_AverageRoughnessContrast = 1.05f;

	// Token: 0x04000905 RID: 2309
	[SerializeField]
	private float Stanley_AverageRoughnessMultiply = 1.2f;

	// Token: 0x04000906 RID: 2310
	[Header("View Angle")]
	[SerializeField]
	private float Stanley_ViewAngleContrast = 1f;

	// Token: 0x04000907 RID: 2311
	[SerializeField]
	private float Stanley_ViewAngleMultiplier = 1.2f;

	// Token: 0x04000908 RID: 2312
	[Header("Reflection Master")]
	[SerializeField]
	private float Stanley_ReflectionMasterContrast = 2f;

	// Token: 0x04000909 RID: 2313
	[SerializeField]
	private float Stanley_ReflectionMasterMultiplier = 20f;

	// Token: 0x0400090A RID: 2314
	[Header("Reflection Mask")]
	[SerializeField]
	[HideInInspector]
	private float Stanley_ReflectionMaskMultiplier = 10f;

	// Token: 0x0400090B RID: 2315
	[Header("Desaturation")]
	[SerializeField]
	private float Stanley_ReflectionDesaturation = 0.6f;

	// Token: 0x0400090C RID: 2316
	[Header("Fresnel")]
	[SerializeField]
	private float StylizedFresnelBias = 0.2f;

	// Token: 0x0400090D RID: 2317
	[SerializeField]
	private float StylizedFresnelScale = 1.2f;

	// Token: 0x0400090E RID: 2318
	[SerializeField]
	private float StylizedFresnelPower = 1.2f;
}
