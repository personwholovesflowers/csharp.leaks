using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020003EF RID: 1007
public static class ShaderProperties
{
	// Token: 0x060016A1 RID: 5793 RVA: 0x000B59E3 File Offset: 0x000B3BE3
	public static int GetMainTexID(Material material)
	{
		if (!material.HasProperty(ShaderProperties.BaseMap))
		{
			return ShaderProperties.MainTex;
		}
		return ShaderProperties.BaseMap;
	}

	// Token: 0x060016A2 RID: 5794 RVA: 0x000B59FD File Offset: 0x000B3BFD
	public static int PrefixedPropertyToID(string prefix = "", [CallerMemberName] string name = null)
	{
		if (!name.StartsWith("_"))
		{
			prefix += "_";
		}
		return Shader.PropertyToID(prefix + name);
	}

	// Token: 0x04001F3C RID: 7996
	public static readonly int BaseMap = ShaderProperties.PrefixedPropertyToID("", "BaseMap");

	// Token: 0x04001F3D RID: 7997
	public static readonly int Color = ShaderProperties.PrefixedPropertyToID("", "Color");

	// Token: 0x04001F3E RID: 7998
	public static readonly int MainTex = ShaderProperties.PrefixedPropertyToID("", "MainTex");

	// Token: 0x04001F3F RID: 7999
	public static readonly int Cutoff = ShaderProperties.PrefixedPropertyToID("", "Cutoff");

	// Token: 0x04001F40 RID: 8000
	public static readonly int Glossiness = ShaderProperties.PrefixedPropertyToID("", "Glossiness");

	// Token: 0x04001F41 RID: 8001
	public static readonly int GlossMapScale = ShaderProperties.PrefixedPropertyToID("", "GlossMapScale");

	// Token: 0x04001F42 RID: 8002
	public static readonly int SmoothnessTextureChannel = ShaderProperties.PrefixedPropertyToID("", "SmoothnessTextureChannel");

	// Token: 0x04001F43 RID: 8003
	public static readonly int Metallic = ShaderProperties.PrefixedPropertyToID("", "Metallic");

	// Token: 0x04001F44 RID: 8004
	public static readonly int MetallicGlossMap = ShaderProperties.PrefixedPropertyToID("", "MetallicGlossMap");

	// Token: 0x04001F45 RID: 8005
	public static readonly int SpecularHighlights = ShaderProperties.PrefixedPropertyToID("", "SpecularHighlights");

	// Token: 0x04001F46 RID: 8006
	public static readonly int GlossyReflections = ShaderProperties.PrefixedPropertyToID("", "GlossyReflections");

	// Token: 0x04001F47 RID: 8007
	public static readonly int BumpScale = ShaderProperties.PrefixedPropertyToID("", "BumpScale");

	// Token: 0x04001F48 RID: 8008
	public static readonly int BumpMap = ShaderProperties.PrefixedPropertyToID("", "BumpMap");

	// Token: 0x04001F49 RID: 8009
	public static readonly int Parallax = ShaderProperties.PrefixedPropertyToID("", "Parallax");

	// Token: 0x04001F4A RID: 8010
	public static readonly int ParallaxMap = ShaderProperties.PrefixedPropertyToID("", "ParallaxMap");

	// Token: 0x04001F4B RID: 8011
	public static readonly int OcclusionStrength = ShaderProperties.PrefixedPropertyToID("", "OcclusionStrength");

	// Token: 0x04001F4C RID: 8012
	public static readonly int OcclusionMap = ShaderProperties.PrefixedPropertyToID("", "OcclusionMap");

	// Token: 0x04001F4D RID: 8013
	public static readonly int EmissionColor = ShaderProperties.PrefixedPropertyToID("", "EmissionColor");

	// Token: 0x04001F4E RID: 8014
	public static readonly int EmissionMap = ShaderProperties.PrefixedPropertyToID("", "EmissionMap");

	// Token: 0x04001F4F RID: 8015
	public static readonly int DetailMask = ShaderProperties.PrefixedPropertyToID("", "DetailMask");

	// Token: 0x04001F50 RID: 8016
	public static readonly int DetailAlbedoMap = ShaderProperties.PrefixedPropertyToID("", "DetailAlbedoMap");

	// Token: 0x04001F51 RID: 8017
	public static readonly int DetailNormalMapScale = ShaderProperties.PrefixedPropertyToID("", "DetailNormalMapScale");

	// Token: 0x04001F52 RID: 8018
	public static readonly int DetailNormalMap = ShaderProperties.PrefixedPropertyToID("", "DetailNormalMap");

	// Token: 0x04001F53 RID: 8019
	public static readonly int UVSec = ShaderProperties.PrefixedPropertyToID("", "UVSec");

	// Token: 0x04001F54 RID: 8020
	public static readonly int Mode = ShaderProperties.PrefixedPropertyToID("", "Mode");

	// Token: 0x04001F55 RID: 8021
	public static readonly int SrcBlend = ShaderProperties.PrefixedPropertyToID("", "SrcBlend");

	// Token: 0x04001F56 RID: 8022
	public static readonly int DstBlend = ShaderProperties.PrefixedPropertyToID("", "DstBlend");

	// Token: 0x04001F57 RID: 8023
	public static readonly int ZWrite = ShaderProperties.PrefixedPropertyToID("", "ZWrite");

	// Token: 0x04001F58 RID: 8024
	public static readonly int WorldSpaceCameraPos = ShaderProperties.PrefixedPropertyToID("", "WorldSpaceCameraPos");

	// Token: 0x04001F59 RID: 8025
	public static readonly int ProjectionParams = ShaderProperties.PrefixedPropertyToID("", "ProjectionParams");

	// Token: 0x04001F5A RID: 8026
	public static readonly int ScreenParams = ShaderProperties.PrefixedPropertyToID("", "ScreenParams");

	// Token: 0x04001F5B RID: 8027
	public static readonly int ZBufferParams = ShaderProperties.PrefixedPropertyToID("", "ZBufferParams");

	// Token: 0x04001F5C RID: 8028
	public static readonly int Time = ShaderProperties.PrefixedPropertyToID("", "Time");

	// Token: 0x04001F5D RID: 8029
	public static readonly int SinTime = ShaderProperties.PrefixedPropertyToID("", "SinTime");

	// Token: 0x04001F5E RID: 8030
	public static readonly int CosTime = ShaderProperties.PrefixedPropertyToID("", "CosTime");

	// Token: 0x04001F5F RID: 8031
	public static readonly int LightColor0 = ShaderProperties.PrefixedPropertyToID("", "LightColor0");

	// Token: 0x04001F60 RID: 8032
	public static readonly int WorldSpaceLightPos0 = ShaderProperties.PrefixedPropertyToID("", "WorldSpaceLightPos0");

	// Token: 0x04001F61 RID: 8033
	public static readonly int LightMatrix0 = ShaderProperties.PrefixedPropertyToID("", "LightMatrix0");

	// Token: 0x04001F62 RID: 8034
	public static readonly int TextureSampleAdd = ShaderProperties.PrefixedPropertyToID("", "TextureSampleAdd");

	// Token: 0x04001F63 RID: 8035
	public static readonly int OpacScale = ShaderProperties.PrefixedPropertyToID("", "OpacScale");

	// Token: 0x04001F64 RID: 8036
	public static readonly int SurfaceType = ShaderProperties.PrefixedPropertyToID("", "SurfaceType");

	// Token: 0x04001F65 RID: 8037
	public static readonly int SecondarySurfaceType = ShaderProperties.PrefixedPropertyToID("", "SecondarySurfaceType");

	// Token: 0x04001F66 RID: 8038
	public static readonly int EnviroParticleColor = ShaderProperties.PrefixedPropertyToID("", "EnviroParticleColor");

	// Token: 0x04001F67 RID: 8039
	public static readonly int SecondaryEnviroParticleColor = ShaderProperties.PrefixedPropertyToID("", "SecondaryEnviroParticleColor");
}
