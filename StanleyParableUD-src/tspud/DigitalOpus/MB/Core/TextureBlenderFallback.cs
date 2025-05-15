using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000265 RID: 613
	public class TextureBlenderFallback : TextureBlender
	{
		// Token: 0x06000E87 RID: 3719 RVA: 0x0001C409 File Offset: 0x0001A609
		public bool DoesShaderNameMatch(string shaderName)
		{
			return true;
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00046FEC File Offset: 0x000451EC
		public void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName)
		{
			if (shaderTexturePropertyName.Equals("_MainTex"))
			{
				this.m_doTintColor = true;
				this.m_tintColor = Color.white;
				if (sourceMat.HasProperty("_Color"))
				{
					this.m_tintColor = sourceMat.GetColor("_Color");
					return;
				}
			}
			else
			{
				this.m_doTintColor = false;
			}
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00047040 File Offset: 0x00045240
		public Color OnBlendTexturePixel(string shaderPropertyName, Color pixelColor)
		{
			if (this.m_doTintColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			return pixelColor;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x000470A4 File Offset: 0x000452A4
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultColor, "_Color");
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x000470BD File Offset: 0x000452BD
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			if (resultMaterial.HasProperty("_Color"))
			{
				resultMaterial.SetColor("_Color", this.m_defaultColor);
			}
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x000470E0 File Offset: 0x000452E0
		public Color GetColorIfNoTexture(Material mat, ShaderTextureProperty texProperty)
		{
			if (texProperty.isNormalMap)
			{
				return new Color(0.5f, 0.5f, 1f);
			}
			if (texProperty.name.Equals("_MainTex"))
			{
				if (!(mat != null) || !mat.HasProperty("_Color"))
				{
					goto IL_02B8;
				}
				try
				{
					return mat.GetColor("_Color");
				}
				catch (Exception)
				{
					goto IL_02B8;
				}
			}
			if (texProperty.name.Equals("_SpecGlossMap"))
			{
				if (!(mat != null) || !mat.HasProperty("_SpecColor"))
				{
					goto IL_02B8;
				}
				try
				{
					Color color = mat.GetColor("_SpecColor");
					if (mat.HasProperty("_Glossiness"))
					{
						try
						{
							color.a = mat.GetFloat("_Glossiness");
						}
						catch (Exception)
						{
						}
					}
					Debug.LogWarning(color);
					return color;
				}
				catch (Exception)
				{
					goto IL_02B8;
				}
			}
			if (texProperty.name.Equals("_MetallicGlossMap"))
			{
				if (!(mat != null) || !mat.HasProperty("_Metallic"))
				{
					goto IL_02B8;
				}
				try
				{
					float @float = mat.GetFloat("_Metallic");
					Color color2 = new Color(@float, @float, @float);
					if (mat.HasProperty("_Glossiness"))
					{
						try
						{
							color2.a = mat.GetFloat("_Glossiness");
						}
						catch (Exception)
						{
						}
					}
					return color2;
				}
				catch (Exception)
				{
					goto IL_02B8;
				}
			}
			if (texProperty.name.Equals("_ParallaxMap"))
			{
				return new Color(0f, 0f, 0f, 0f);
			}
			if (texProperty.name.Equals("_OcclusionMap"))
			{
				return new Color(1f, 1f, 1f, 1f);
			}
			if (texProperty.name.Equals("_EmissionMap"))
			{
				if (!(mat != null) || !mat.HasProperty("_EmissionScaleUI"))
				{
					goto IL_02B8;
				}
				if (mat.HasProperty("_EmissionColor") && mat.HasProperty("_EmissionColorUI"))
				{
					try
					{
						Color color3 = mat.GetColor("_EmissionColor");
						Color color4 = mat.GetColor("_EmissionColorUI");
						float float2 = mat.GetFloat("_EmissionScaleUI");
						if (color3 == new Color(0f, 0f, 0f, 0f) && color4 == new Color(1f, 1f, 1f, 1f))
						{
							return new Color(float2, float2, float2, float2);
						}
						return color4;
					}
					catch (Exception)
					{
						goto IL_02B8;
					}
				}
				try
				{
					float float3 = mat.GetFloat("_EmissionScaleUI");
					return new Color(float3, float3, float3, float3);
				}
				catch (Exception)
				{
					goto IL_02B8;
				}
			}
			if (texProperty.name.Equals("_DetailMask"))
			{
				return new Color(0f, 0f, 0f, 0f);
			}
			IL_02B8:
			return new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00047418 File Offset: 0x00045618
		public static bool _compareColor(Material a, Material b, Color defaultVal, string propertyName)
		{
			Color color = defaultVal;
			Color color2 = defaultVal;
			if (a.HasProperty(propertyName))
			{
				color = a.GetColor(propertyName);
			}
			if (b.HasProperty(propertyName))
			{
				color2 = b.GetColor(propertyName);
			}
			return !(color != color2);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00047458 File Offset: 0x00045658
		public static bool _compareFloat(Material a, Material b, float defaultVal, string propertyName)
		{
			float num = defaultVal;
			float num2 = defaultVal;
			if (a.HasProperty(propertyName))
			{
				num = a.GetFloat(propertyName);
			}
			if (b.HasProperty(propertyName))
			{
				num2 = b.GetFloat(propertyName);
			}
			return num == num2;
		}

		// Token: 0x04000D2B RID: 3371
		private bool m_doTintColor;

		// Token: 0x04000D2C RID: 3372
		private Color m_tintColor;

		// Token: 0x04000D2D RID: 3373
		private Color m_defaultColor = Color.white;
	}
}
