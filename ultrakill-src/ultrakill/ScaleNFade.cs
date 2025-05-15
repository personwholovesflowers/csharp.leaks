using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003C5 RID: 965
public class ScaleNFade : MonoBehaviour
{
	// Token: 0x060015ED RID: 5613 RVA: 0x000B1BCC File Offset: 0x000AFDCC
	private void Start()
	{
		if (this.fade)
		{
			switch (this.ft)
			{
			case FadeType.Sprite:
				this.sr = base.GetComponent<SpriteRenderer>();
				break;
			case FadeType.Line:
				this.lr = base.GetComponent<LineRenderer>();
				break;
			case FadeType.Light:
				this.lght = base.GetComponent<Light>();
				break;
			case FadeType.Renderer:
				this.rend = base.GetComponent<Renderer>();
				if (this.rend == null)
				{
					this.rend = base.GetComponentInChildren<Renderer>();
				}
				break;
			case FadeType.UiImage:
				this.img = base.GetComponent<Image>();
				break;
			}
		}
		if (this.rend != null)
		{
			this.hasOpacScale = this.rend.material.HasProperty("_OpacScale");
			this.hasTint = this.rend.material.HasProperty("_Tint");
			this.hasColor = this.rend.material.HasProperty("_Color");
		}
		this.scaleAmt = base.transform.localScale;
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x000B1CD8 File Offset: 0x000AFED8
	private void Update()
	{
		if (this.scale)
		{
			this.scaleAmt += Vector3.one * Time.deltaTime * this.scaleSpeed;
			base.transform.localScale = this.scaleAmt;
		}
		if (!this.fade)
		{
			return;
		}
		switch (this.ft)
		{
		case FadeType.Sprite:
			this.sr.color = this.UpdateColor(this.sr.color);
			return;
		case FadeType.Line:
			break;
		case FadeType.Light:
			this.UpdateLightFade();
			return;
		case FadeType.Renderer:
			this.UpdateRendererFade();
			break;
		case FadeType.UiImage:
			this.img.color = this.UpdateColor(this.img.color);
			return;
		default:
			return;
		}
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x000B1D9C File Offset: 0x000AFF9C
	private Color UpdateColor(Color newColor)
	{
		if (newColor.a <= 0f && this.fadeSpeed > 0f)
		{
			if (!this.dontDestroyOnZero)
			{
				Object.Destroy(base.gameObject);
			}
			return newColor;
		}
		newColor.a -= this.fadeSpeed * Time.deltaTime;
		if (this.clampFade)
		{
			newColor.a = Mathf.Clamp(newColor.a, this.clampMinimum, this.clampMaximum);
		}
		return newColor;
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x000B1E18 File Offset: 0x000B0018
	private void UpdateLightFade()
	{
		float num = (this.lightUseIntensityInsteadOfRange ? this.lght.intensity : this.lght.range);
		if (num <= 0f && this.fadeSpeed > 0f)
		{
			if (!this.dontDestroyOnZero)
			{
				Object.Destroy(base.gameObject);
			}
			return;
		}
		num -= this.fadeSpeed * Time.deltaTime;
		if (this.clampFade)
		{
			num = Mathf.Clamp(num, this.clampMinimum, this.clampMaximum);
		}
		if (this.lightUseIntensityInsteadOfRange)
		{
			this.lght.intensity = num;
			return;
		}
		this.lght.range = num;
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x000B1EBA File Offset: 0x000B00BA
	private void UpdateRendererFade()
	{
		if (this.hasOpacScale)
		{
			this.UpdateOpacityScale();
			return;
		}
		if (this.hasTint || this.hasColor)
		{
			this.UpdateColorFade();
		}
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x000B1EE4 File Offset: 0x000B00E4
	private void UpdateOpacityScale()
	{
		float num = this.rend.material.GetFloat("_OpacScale");
		if (num <= 0f && this.fadeSpeed > 0f && !this.dontDestroyOnZero)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		num = Mathf.Max(num - this.fadeSpeed * Time.deltaTime, 0f);
		if (this.clampFade)
		{
			num = Mathf.Clamp(num, this.clampMinimum, this.clampMaximum);
		}
		this.rend.material.SetFloat("_OpacScale", num);
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x000B1F7C File Offset: 0x000B017C
	private void UpdateColorFade()
	{
		string text = (this.hasTint ? "_Tint" : "_Color");
		Color color = this.rend.material.GetColor(text);
		if (this.fadeToBlack)
		{
			color = Color.Lerp(color, Color.black, this.fadeSpeed * Time.deltaTime);
		}
		else
		{
			color.a = Mathf.Max(color.a - this.fadeSpeed * Time.deltaTime, 0f);
			if (this.clampFade)
			{
				color.a = Mathf.Clamp(color.a, this.clampMinimum, this.clampMaximum);
			}
		}
		if (color.a <= 0f && this.fadeSpeed > 0f && !this.dontDestroyOnZero)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.rend.material.SetColor(text, color);
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x000B205C File Offset: 0x000B025C
	private void FixedUpdate()
	{
		if (this.fade && this.ft == FadeType.Line)
		{
			Color color = this.lr.startColor;
			color.a -= this.fadeSpeed * Time.deltaTime;
			if (this.clampFade)
			{
				color.a = Mathf.Clamp(color.a, this.clampMinimum, this.clampMaximum);
			}
			this.lr.startColor = color;
			color = this.lr.endColor;
			color.a -= this.fadeSpeed * Time.deltaTime;
			if (this.clampFade)
			{
				color.a = Mathf.Clamp(color.a, this.clampMinimum, this.clampMaximum);
			}
			this.lr.endColor = color;
			if (this.lr.startColor.a <= 0f && this.lr.endColor.a <= 0f && this.fadeSpeed > 0f && !this.dontDestroyOnZero)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x000B2178 File Offset: 0x000B0378
	public void ChangeFadeSpeed(float newSpeed)
	{
		this.fadeSpeed = newSpeed;
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x000B2181 File Offset: 0x000B0381
	public void ChangeScaleSpeed(float newSpeed)
	{
		this.scaleSpeed = newSpeed;
	}

	// Token: 0x04001E33 RID: 7731
	public bool scale;

	// Token: 0x04001E34 RID: 7732
	public bool fade;

	// Token: 0x04001E35 RID: 7733
	public FadeType ft;

	// Token: 0x04001E36 RID: 7734
	public float scaleSpeed;

	// Token: 0x04001E37 RID: 7735
	public float fadeSpeed;

	// Token: 0x04001E38 RID: 7736
	private SpriteRenderer sr;

	// Token: 0x04001E39 RID: 7737
	private LineRenderer lr;

	// Token: 0x04001E3A RID: 7738
	private Light lght;

	// Token: 0x04001E3B RID: 7739
	private Renderer rend;

	// Token: 0x04001E3C RID: 7740
	private Image img;

	// Token: 0x04001E3D RID: 7741
	public bool dontDestroyOnZero;

	// Token: 0x04001E3E RID: 7742
	public bool lightUseIntensityInsteadOfRange;

	// Token: 0x04001E3F RID: 7743
	public bool fadeToBlack;

	// Token: 0x04001E40 RID: 7744
	private Vector3 scaleAmt = Vector3.one;

	// Token: 0x04001E41 RID: 7745
	private bool hasOpacScale;

	// Token: 0x04001E42 RID: 7746
	private bool hasTint;

	// Token: 0x04001E43 RID: 7747
	private bool hasColor;

	// Token: 0x04001E44 RID: 7748
	public bool clampFade;

	// Token: 0x04001E45 RID: 7749
	public float clampMinimum;

	// Token: 0x04001E46 RID: 7750
	public float clampMaximum;
}
