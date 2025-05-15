using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000264 RID: 612
	public interface TextureBlender
	{
		// Token: 0x06000E81 RID: 3713
		bool DoesShaderNameMatch(string shaderName);

		// Token: 0x06000E82 RID: 3714
		void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName);

		// Token: 0x06000E83 RID: 3715
		Color OnBlendTexturePixel(string shaderPropertyName, Color pixelColor);

		// Token: 0x06000E84 RID: 3716
		bool NonTexturePropertiesAreEqual(Material a, Material b);

		// Token: 0x06000E85 RID: 3717
		void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial);

		// Token: 0x06000E86 RID: 3718
		Color GetColorIfNoTexture(Material m, ShaderTextureProperty texPropertyName);
	}
}
