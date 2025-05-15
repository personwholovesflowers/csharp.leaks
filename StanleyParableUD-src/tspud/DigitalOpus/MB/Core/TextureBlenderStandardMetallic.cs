using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000268 RID: 616
	public class TextureBlenderStandardMetallic : TextureBlender
	{
		// Token: 0x06000E9E RID: 3742 RVA: 0x00047793 File Offset: 0x00045993
		public bool DoesShaderNameMatch(string shaderName)
		{
			return shaderName.Equals("Standard");
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x000477A0 File Offset: 0x000459A0
		public void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName)
		{
			if (shaderTexturePropertyName.Equals("_MainTex"))
			{
				this.propertyToDo = TextureBlenderStandardMetallic.Prop.doColor;
				if (sourceMat.HasProperty(shaderTexturePropertyName))
				{
					this.m_tintColor = sourceMat.GetColor("_Color");
					return;
				}
				this.m_tintColor = this.m_defaultColor;
				return;
			}
			else
			{
				if (shaderTexturePropertyName.Equals("_MetallicGlossMap"))
				{
					this.propertyToDo = TextureBlenderStandardMetallic.Prop.doMetallic;
					return;
				}
				if (!shaderTexturePropertyName.Equals("_EmissionMap"))
				{
					this.propertyToDo = TextureBlenderStandardMetallic.Prop.doNone;
					return;
				}
				this.propertyToDo = TextureBlenderStandardMetallic.Prop.doEmission;
				if (sourceMat.HasProperty(shaderTexturePropertyName))
				{
					this.m_emission = sourceMat.GetColor("_EmissionColor");
					return;
				}
				this.m_emission = this.m_defaultEmission;
				return;
			}
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00047844 File Offset: 0x00045A44
		public Color OnBlendTexturePixel(string propertyToDoshaderPropertyName, Color pixelColor)
		{
			if (this.propertyToDo == TextureBlenderStandardMetallic.Prop.doColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			if (this.propertyToDo == TextureBlenderStandardMetallic.Prop.doMetallic)
			{
				return pixelColor;
			}
			if (this.propertyToDo == TextureBlenderStandardMetallic.Prop.doEmission)
			{
				return new Color(pixelColor.r * this.m_emission.r, pixelColor.g * this.m_emission.g, pixelColor.b * this.m_emission.b, pixelColor.a * this.m_emission.a);
			}
			return pixelColor;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0004790C File Offset: 0x00045B0C
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultColor, "_Color") && TextureBlenderFallback._compareFloat(a, b, this.m_defaultMetallic, "_Metallic") && TextureBlenderFallback._compareFloat(a, b, this.m_defaultGlossiness, "_Glossiness") && TextureBlenderFallback._compareColor(a, b, this.m_defaultEmission, "_EmissionColor");
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00047974 File Offset: 0x00045B74
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			resultMaterial.SetColor("_Color", this.m_defaultColor);
			resultMaterial.SetFloat("_Metallic", this.m_defaultMetallic);
			resultMaterial.SetFloat("_Glossiness", this.m_defaultGlossiness);
			if (resultMaterial.GetTexture("_EmissionMap") == null)
			{
				resultMaterial.SetColor("_EmissionColor", Color.black);
				return;
			}
			resultMaterial.SetColor("_EmissionColor", Color.white);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x000479E8 File Offset: 0x00045BE8
		public Color GetColorIfNoTexture(Material mat, ShaderTextureProperty texPropertyName)
		{
			if (texPropertyName.name.Equals("_BumpMap"))
			{
				return new Color(0.5f, 0.5f, 1f);
			}
			if (texPropertyName.Equals("_MainTex"))
			{
				if (!(mat != null) || !mat.HasProperty("_Color"))
				{
					goto IL_01B3;
				}
				try
				{
					return mat.GetColor("_Color");
				}
				catch (Exception)
				{
					goto IL_01B3;
				}
			}
			if (texPropertyName.name.Equals("_MetallicGlossMap"))
			{
				if (mat != null && mat.HasProperty("_Metallic"))
				{
					try
					{
						float @float = mat.GetFloat("_Metallic");
						Color color = new Color(@float, @float, @float);
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
						return color;
					}
					catch (Exception)
					{
						goto IL_01B3;
					}
				}
				return new Color(0f, 0f, 0f, 0.5f);
			}
			if (texPropertyName.name.Equals("_ParallaxMap"))
			{
				return new Color(0f, 0f, 0f, 0f);
			}
			if (texPropertyName.name.Equals("_OcclusionMap"))
			{
				return new Color(1f, 1f, 1f, 1f);
			}
			if (texPropertyName.name.Equals("_EmissionMap"))
			{
				if (mat != null)
				{
					if (mat.HasProperty("_EmissionColor"))
					{
						try
						{
							return mat.GetColor("_EmissionColor");
						}
						catch (Exception)
						{
							goto IL_01B3;
						}
					}
					return Color.black;
				}
			}
			else if (texPropertyName.name.Equals("_DetailMask"))
			{
				return new Color(0f, 0f, 0f, 0f);
			}
			IL_01B3:
			return new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x04000D34 RID: 3380
		private Color m_tintColor;

		// Token: 0x04000D35 RID: 3381
		private Color m_emission;

		// Token: 0x04000D36 RID: 3382
		private TextureBlenderStandardMetallic.Prop propertyToDo = TextureBlenderStandardMetallic.Prop.doNone;

		// Token: 0x04000D37 RID: 3383
		private Color m_defaultColor = Color.white;

		// Token: 0x04000D38 RID: 3384
		private float m_defaultMetallic;

		// Token: 0x04000D39 RID: 3385
		private float m_defaultGlossiness = 0.5f;

		// Token: 0x04000D3A RID: 3386
		private Color m_defaultEmission = Color.black;

		// Token: 0x0200044F RID: 1103
		private enum Prop
		{
			// Token: 0x04001605 RID: 5637
			doColor,
			// Token: 0x04001606 RID: 5638
			doMetallic,
			// Token: 0x04001607 RID: 5639
			doEmission,
			// Token: 0x04001608 RID: 5640
			doNone
		}
	}
}
