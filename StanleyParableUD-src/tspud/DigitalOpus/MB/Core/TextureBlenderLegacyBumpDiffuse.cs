using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000266 RID: 614
	public class TextureBlenderLegacyBumpDiffuse : TextureBlender
	{
		// Token: 0x06000E90 RID: 3728 RVA: 0x000474A5 File Offset: 0x000456A5
		public bool DoesShaderNameMatch(string shaderName)
		{
			return shaderName.Equals("Legacy Shaders/Bumped Diffuse") || shaderName.Equals("Bumped Diffuse");
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000474C6 File Offset: 0x000456C6
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

		// Token: 0x06000E92 RID: 3730 RVA: 0x000474F8 File Offset: 0x000456F8
		public Color OnBlendTexturePixel(string propertyToDoshaderPropertyName, Color pixelColor)
		{
			if (this.doColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			return pixelColor;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0004755C File Offset: 0x0004575C
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultTintColor, "_Color");
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00047570 File Offset: 0x00045770
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			resultMaterial.SetColor("_Color", Color.white);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00047584 File Offset: 0x00045784
		public Color GetColorIfNoTexture(Material m, ShaderTextureProperty texPropertyName)
		{
			if (texPropertyName.name.Equals("_BumpMap"))
			{
				return new Color(0.5f, 0.5f, 1f);
			}
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

		// Token: 0x04000D2E RID: 3374
		private bool doColor;

		// Token: 0x04000D2F RID: 3375
		private Color m_tintColor;

		// Token: 0x04000D30 RID: 3376
		private Color m_defaultTintColor = Color.white;
	}
}
