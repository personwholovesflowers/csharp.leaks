using System;
using UnityEngine;

// Token: 0x020002AD RID: 685
public class LerpColor : MonoBehaviour
{
	// Token: 0x06000EF2 RID: 3826 RVA: 0x0006F155 File Offset: 0x0006D355
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.Activate();
		}
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x0006F168 File Offset: 0x0006D368
	private void Update()
	{
		if (this.activated)
		{
			if (this.rainbow)
			{
				this.currentColor = LerpColor.RainbowShift(this.currentColor, Time.deltaTime / this.time);
			}
			else
			{
				this.currentTime = Mathf.MoveTowards(this.currentTime, 1f, Time.deltaTime / this.time);
				this.currentColor = Color.Lerp(this.originalColor, this.targetColor, this.currentTime);
			}
			switch (this.type)
			{
			case LerpColorType.Light:
				this.lit.color = new Color(this.currentColor.r, this.currentColor.g, this.currentColor.b, this.dontOverrideAlpha ? this.lit.color.a : this.currentColor.a);
				break;
			case LerpColorType.MaterialColor:
				this.mat.color = new Color(this.currentColor.r, this.currentColor.g, this.currentColor.b, this.dontOverrideAlpha ? this.mat.color.a : this.currentColor.a);
				break;
			case LerpColorType.MaterialEmissive:
				this.mat.SetColor(UKShaderProperties.EmissiveColor, new Color(this.currentColor.r, this.currentColor.g, this.currentColor.b, this.dontOverrideAlpha ? this.mat.color.a : this.currentColor.a));
				break;
			case LerpColorType.MaterialSecondary:
				this.mat.SetColor(UKShaderProperties.BlendColor, new Color(this.currentColor.r, this.currentColor.g, this.currentColor.b, this.dontOverrideAlpha ? this.mat.color.a : this.currentColor.a));
				break;
			case LerpColorType.Fog:
				RenderSettings.fogColor = new Color(this.currentColor.r, this.currentColor.g, this.currentColor.b, this.dontOverrideAlpha ? RenderSettings.fogColor.a : this.currentColor.a);
				break;
			case LerpColorType.Water:
				this.wtr.UpdateColor(new Color(this.currentColor.r, this.currentColor.g, this.currentColor.b, this.dontOverrideAlpha ? this.wtr.clr.a : this.currentColor.a));
				break;
			case LerpColorType.SpriteRenderer:
				this.spr.color = new Color(this.currentColor.r, this.currentColor.g, this.currentColor.b, this.dontOverrideAlpha ? this.spr.color.a : this.currentColor.a);
				break;
			}
			if (this.currentTime >= 1f)
			{
				this.activated = false;
			}
		}
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x0006F49B File Offset: 0x0006D69B
	public void Activate()
	{
		if (this.beenActivated && this.oneTime)
		{
			return;
		}
		this.beenActivated = true;
		this.GetValues();
		this.currentColor = this.originalColor;
		this.activated = true;
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x0006F4D0 File Offset: 0x0006D6D0
	public void Revert()
	{
		if (!this.beenActivated)
		{
			return;
		}
		Color color = this.originalColor;
		this.originalColor = this.targetColor;
		this.targetColor = color;
		this.currentTime = 1f - this.currentTime;
		this.currentColor = Color.Lerp(this.originalColor, this.targetColor, this.currentTime);
		this.activated = true;
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x0006F536 File Offset: 0x0006D736
	public void Skip()
	{
		if (this.beenActivated && this.oneTime)
		{
			return;
		}
		this.beenActivated = true;
		this.GetValues();
		this.currentColor = this.targetColor;
		this.activated = true;
		this.currentTime = 1f;
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0006F574 File Offset: 0x0006D774
	private void GetValues()
	{
		switch (this.type)
		{
		case LerpColorType.Light:
			this.lit = base.GetComponent<Light>();
			this.originalColor = this.lit.color;
			return;
		case LerpColorType.MaterialColor:
		{
			Renderer renderer = base.GetComponent<Renderer>();
			for (int i = 0; i < renderer.materials.Length; i++)
			{
				if (renderer.materials[i].HasColor(UKShaderProperties.Color))
				{
					this.mat = renderer.materials[i];
					this.originalColor = this.mat.color;
					return;
				}
			}
			return;
		}
		case LerpColorType.MaterialEmissive:
		{
			Renderer renderer = base.GetComponent<Renderer>();
			for (int j = 0; j < renderer.materials.Length; j++)
			{
				if (renderer.materials[j].IsKeywordEnabled("EMISSIVE"))
				{
					this.mat = renderer.materials[j];
					this.originalColor = this.mat.GetColor(UKShaderProperties.EmissiveColor);
					return;
				}
			}
			return;
		}
		case LerpColorType.MaterialSecondary:
		{
			Renderer renderer = base.GetComponent<Renderer>();
			for (int k = 0; k < renderer.materials.Length; k++)
			{
				if (renderer.materials[k].IsKeywordEnabled("VERTEX_BLENDING"))
				{
					this.mat = renderer.materials[k];
					this.originalColor = this.mat.GetColor(UKShaderProperties.BlendColor);
					return;
				}
			}
			return;
		}
		case LerpColorType.Fog:
			this.originalColor = RenderSettings.fogColor;
			return;
		case LerpColorType.Water:
			this.wtr = base.GetComponent<Water>();
			this.originalColor = this.wtr.clr;
			return;
		case LerpColorType.SpriteRenderer:
			this.spr = base.GetComponent<SpriteRenderer>();
			this.originalColor = this.spr.color;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0006F710 File Offset: 0x0006D910
	public static Color RainbowShift(Color color, float amount)
	{
		float num;
		float num2;
		float num3;
		Color.RGBToHSV(color, out num, out num2, out num3);
		num += amount;
		num2 = 1f;
		num3 = 1f;
		return Color.HSVToRGB(num, num2, num3);
	}

	// Token: 0x04001413 RID: 5139
	public bool onEnable = true;

	// Token: 0x04001414 RID: 5140
	public bool oneTime;

	// Token: 0x04001415 RID: 5141
	[HideInInspector]
	public bool beenActivated;

	// Token: 0x04001416 RID: 5142
	public LerpColorType type;

	// Token: 0x04001417 RID: 5143
	private Light lit;

	// Token: 0x04001418 RID: 5144
	private Material mat;

	// Token: 0x04001419 RID: 5145
	private Water wtr;

	// Token: 0x0400141A RID: 5146
	private SpriteRenderer spr;

	// Token: 0x0400141B RID: 5147
	private bool activated;

	// Token: 0x0400141C RID: 5148
	public bool rainbow;

	// Token: 0x0400141D RID: 5149
	private Color originalColor;

	// Token: 0x0400141E RID: 5150
	public Color targetColor;

	// Token: 0x0400141F RID: 5151
	private Color currentColor;

	// Token: 0x04001420 RID: 5152
	public float time;

	// Token: 0x04001421 RID: 5153
	private float currentTime;

	// Token: 0x04001422 RID: 5154
	public bool dontOverrideAlpha;
}
