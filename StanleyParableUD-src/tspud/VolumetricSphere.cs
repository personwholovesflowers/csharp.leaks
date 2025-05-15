using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
[ExecuteInEditMode]
public class VolumetricSphere : MonoBehaviour
{
	// Token: 0x0600004F RID: 79 RVA: 0x000046AC File Offset: 0x000028AC
	private void Update()
	{
		Shader.SetGlobalVector("_SpherePosition", base.transform.position);
		Shader.SetGlobalFloat("_SphereRadius", this.radius);
		Shader.SetGlobalFloat("_MaskDensity", this.density);
		Shader.SetGlobalFloat("_MaskExponent", this.exponent);
		Shader.SetGlobalInt("_MaxPixelizationLevel", this.maxPixelizationLevel);
		if (this.enableLayersInterpolation)
		{
			Shader.EnableKeyword("_INTERPOLATE_LAYERS_ON");
		}
		else
		{
			Shader.DisableKeyword("_INTERPOLATE_LAYERS_ON");
		}
		if (this.debugSphere)
		{
			Shader.EnableKeyword("_DEBUG_MASK_ON");
			return;
		}
		Shader.DisableKeyword("_DEBUG_MASK_ON");
	}

	// Token: 0x06000050 RID: 80 RVA: 0x0000474E File Offset: 0x0000294E
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
	}

	// Token: 0x0400006C RID: 108
	[Header("Parameters")]
	[Tooltip("The radius of the sphere")]
	[Range(0f, 50f)]
	public float radius = 3f;

	// Token: 0x0400006D RID: 109
	[Tooltip("The density of the sphere")]
	[Range(0f, 10f)]
	public float density = 1f;

	// Token: 0x0400006E RID: 110
	[Tooltip("The curve of the fade-out")]
	[Range(0.2f, 5f)]
	public float exponent = 0.33333334f;

	// Token: 0x0400006F RID: 111
	[Tooltip("The maximum pixelization size")]
	[Range(1f, 10f)]
	public int maxPixelizationLevel = 5;

	// Token: 0x04000070 RID: 112
	[Tooltip("Enabled the interpolation between the layers of different pixels size")]
	public bool enableLayersInterpolation = true;

	// Token: 0x04000071 RID: 113
	[Header("Debug")]
	[Tooltip("Outputs the sphere mask")]
	public bool debugSphere;
}
