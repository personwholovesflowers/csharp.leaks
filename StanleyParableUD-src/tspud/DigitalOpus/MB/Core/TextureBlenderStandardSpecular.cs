using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000269 RID: 617
	public class TextureBlenderStandardSpecular : TextureBlender
	{
		// Token: 0x06000EA5 RID: 3749 RVA: 0x00047C28 File Offset: 0x00045E28
		public bool DoesShaderNameMatch(string shaderName)
		{
			return shaderName.Equals("Standard (Specular setup)");
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00047C38 File Offset: 0x00045E38
		public void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName)
		{
			if (shaderTexturePropertyName.Equals("_MainTex"))
			{
				this.propertyToDo = TextureBlenderStandardSpecular.Prop.doColor;
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
					this.propertyToDo = TextureBlenderStandardSpecular.Prop.doSpecular;
					return;
				}
				if (!shaderTexturePropertyName.Equals("_EmissionMap"))
				{
					this.propertyToDo = TextureBlenderStandardSpecular.Prop.doNone;
					return;
				}
				this.propertyToDo = TextureBlenderStandardSpecular.Prop.doEmission;
				if (sourceMat.HasProperty(shaderTexturePropertyName))
				{
					this.m_emission = sourceMat.GetColor("_EmissionColor");
					return;
				}
				this.m_emission = this.m_defaultEmission;
				return;
			}
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00047CDC File Offset: 0x00045EDC
		public Color OnBlendTexturePixel(string propertyToDoshaderPropertyName, Color pixelColor)
		{
			if (this.propertyToDo == TextureBlenderStandardSpecular.Prop.doColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			if (this.propertyToDo == TextureBlenderStandardSpecular.Prop.doSpecular)
			{
				return pixelColor;
			}
			if (this.propertyToDo == TextureBlenderStandardSpecular.Prop.doEmission)
			{
				return new Color(pixelColor.r * this.m_emission.r, pixelColor.g * this.m_emission.g, pixelColor.b * this.m_emission.b, pixelColor.a * this.m_emission.a);
			}
			return pixelColor;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00047DA4 File Offset: 0x00045FA4
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultColor, "_Color") && TextureBlenderFallback._compareColor(a, b, this.m_defaultSpecular, "_SpecColor") && TextureBlenderFallback._compareFloat(a, b, this.m_defaultGlossiness, "_Glossiness") && TextureBlenderFallback._compareColor(a, b, this.m_defaultEmission, "_EmissionColor");
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00047E0C File Offset: 0x0004600C
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			resultMaterial.SetColor("_Color", this.m_defaultColor);
			resultMaterial.SetColor("_SpecColor", this.m_defaultSpecular);
			resultMaterial.SetFloat("_Glossiness", this.m_defaultGlossiness);
			if (resultMaterial.GetTexture("_EmissionMap") == null)
			{
				resultMaterial.SetColor("_EmissionColor", Color.black);
				return;
			}
			resultMaterial.SetColor("_EmissionColor", Color.white);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00047E80 File Offset: 0x00046080
		public Color GetColorIfNoTexture(Material mat, ShaderTextureProperty texPropertyName)
		{
			if (texPropertyName.name.Equals("_BumpMap"))
			{
				return new Color(0.5f, 0.5f, 1f);
			}
			if (texPropertyName.name.Equals("_MainTex"))
			{
				if (!(mat != null) || !mat.HasProperty("_Color"))
				{
					goto IL_01AD;
				}
				try
				{
					return mat.GetColor("_Color");
				}
				catch (Exception)
				{
					goto IL_01AD;
				}
			}
			if (texPropertyName.name.Equals("_SpecGlossMap"))
			{
				bool flag = false;
				if (mat != null && mat.HasProperty("_SpecColor"))
				{
					try
					{
						Color color = mat.GetColor("_SpecColor");
						if (mat.HasProperty("_Glossiness"))
						{
							try
							{
								flag = true;
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
					}
				}
				if (!flag)
				{
					return this.m_defaultSpecular;
				}
			}
			else
			{
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
								goto IL_01AD;
							}
						}
						return Color.black;
					}
				}
				else if (texPropertyName.name.Equals("_DetailMask"))
				{
					return new Color(0f, 0f, 0f, 0f);
				}
			}
			IL_01AD:
			return new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x04000D3B RID: 3387
		private Color m_tintColor;

		// Token: 0x04000D3C RID: 3388
		private Color m_emission;

		// Token: 0x04000D3D RID: 3389
		private TextureBlenderStandardSpecular.Prop propertyToDo = TextureBlenderStandardSpecular.Prop.doNone;

		// Token: 0x04000D3E RID: 3390
		private Color m_defaultColor = Color.white;

		// Token: 0x04000D3F RID: 3391
		private Color m_defaultSpecular = Color.black;

		// Token: 0x04000D40 RID: 3392
		private float m_defaultGlossiness = 0.5f;

		// Token: 0x04000D41 RID: 3393
		private Color m_defaultEmission = Color.black;

		// Token: 0x02000450 RID: 1104
		private enum Prop
		{
			// Token: 0x0400160A RID: 5642
			doColor,
			// Token: 0x0400160B RID: 5643
			doSpecular,
			// Token: 0x0400160C RID: 5644
			doEmission,
			// Token: 0x0400160D RID: 5645
			doNone
		}
	}
}
