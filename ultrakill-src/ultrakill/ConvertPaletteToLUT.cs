using System;
using SettingsMenu.Components.Pages;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000E5 RID: 229
[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
public class ConvertPaletteToLUT : MonoSingleton<ConvertPaletteToLUT>
{
	// Token: 0x06000487 RID: 1159 RVA: 0x0001F776 File Offset: 0x0001D976
	public void ApplyLastPalette()
	{
		Shader.EnableKeyword("PALETTIZE");
		Shader.SetGlobalInt("_ColorPrecision", 2048);
		Shader.SetGlobalTexture("_LUT", this.processedLUT);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0001F7A4 File Offset: 0x0001D9A4
	public void ConvertPalette(Texture2D inputPalette, ComputeShader paletteCompute, Shader paletteCalc)
	{
		if (!this.processedLUT)
		{
			this.processedLUT = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32, 0);
			this.processedLUT.dimension = TextureDimension.Tex3D;
			this.processedLUT.volumeDepth = 256;
			this.processedLUT.enableRandomWrite = true;
		}
		Shader.SetGlobalTexture("_LUT", this.processedLUT);
		bool flag = true;
		if (this.lastPalette)
		{
			flag = this.lastPalette.name != inputPalette.name;
		}
		if (flag)
		{
			this.lastPalette = inputPalette;
			if (SettingsMenu.Components.Pages.GraphicsSettings.disabledComputeShaders)
			{
				if (!this.fallbackMaterial)
				{
					this.fallbackMaterial = new Material(paletteCalc);
				}
				this.fallbackMaterial.SetTexture("_PaletteTex", inputPalette);
				for (int i = 0; i < this.processedLUT.volumeDepth; i++)
				{
					float num = (float)i / 255f;
					this.fallbackMaterial.SetFloat("_Slice", num);
					Graphics.SetRenderTarget(this.processedLUT, 0, CubemapFace.PositiveX, i);
					Graphics.Blit(null, this.fallbackMaterial);
				}
			}
			else
			{
				paletteCompute.SetTexture(0, "_PaletteTex", inputPalette);
				paletteCompute.SetTexture(0, "Result", this.processedLUT);
				paletteCompute.Dispatch(0, 32, 32, 32);
			}
			this.lastLUT = new RenderTexture(this.processedLUT);
			this.processedLUT.antiAliasing = 1;
			this.processedLUT.filterMode = FilterMode.Point;
			this.lastLUT.antiAliasing = 1;
			this.lastLUT.filterMode = FilterMode.Point;
		}
	}

	// Token: 0x0400062B RID: 1579
	public RenderTexture processedLUT;

	// Token: 0x0400062C RID: 1580
	public RenderTexture lastLUT;

	// Token: 0x0400062D RID: 1581
	public Texture2D lastPalette;

	// Token: 0x0400062E RID: 1582
	private Material fallbackMaterial;
}
