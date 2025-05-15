using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000267 RID: 615
	public class TextureBlenderLegacyDiffuse : TextureBlender
	{
		// Token: 0x06000E97 RID: 3735 RVA: 0x0004762F File Offset: 0x0004582F
		public bool DoesShaderNameMatch(string shaderName)
		{
			return shaderName.Equals("Legacy Shaders/Diffuse") || shaderName.Equals("Diffuse");
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00047650 File Offset: 0x00045850
		public void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName)
		{
			if (shaderTexturePropertyName.EndsWith("_MainTex"))
			{
				this.doColor = true;
				this.m_tintColor = sourceMat.GetColor("_Color");
				return;
			}
			this.doColor = false;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00047680 File Offset: 0x00045880
		public Color OnBlendTexturePixel(string propertyToDoshaderPropertyName, Color pixelColor)
		{
			if (this.doColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			return pixelColor;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x000476E4 File Offset: 0x000458E4
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultTintColor, "_Color");
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x000476F8 File Offset: 0x000458F8
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			resultMaterial.SetColor("_Color", Color.white);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0004770C File Offset: 0x0004590C
		public Color GetColorIfNoTexture(Material m, ShaderTextureProperty texPropertyName)
		{
			if (texPropertyName.name.Equals("_MainTex") && m != null && m.HasProperty("_Color"))
			{
				try
				{
					return m.GetColor("_Color");
				}
				catch (Exception)
				{
				}
			}
			return new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x04000D31 RID: 3377
		private bool doColor;

		// Token: 0x04000D32 RID: 3378
		private Color m_tintColor;

		// Token: 0x04000D33 RID: 3379
		private Color m_defaultTintColor = Color.white;
	}
}
