using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CD RID: 205
public class ColorBlindGet : MonoBehaviour
{
	// Token: 0x06000413 RID: 1043 RVA: 0x0001C34C File Offset: 0x0001A54C
	private void Start()
	{
		this.UpdateColor();
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0001C34C File Offset: 0x0001A54C
	private void OnEnable()
	{
		this.UpdateColor();
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0001C354 File Offset: 0x0001A554
	public void UpdateColor()
	{
		if (!this.gotTarget)
		{
			this.GetTarget();
		}
		Color color = (this.variationColor ? MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variationNumber] : MonoSingleton<ColorBlindSettings>.Instance.GetHudColor(this.hct));
		if (this.rend)
		{
			this.rend.GetPropertyBlock(this.block);
			this.block.SetColor("_CustomColor1", color);
			this.rend.SetPropertyBlock(this.block);
			return;
		}
		if (this.img)
		{
			this.img.color = color;
		}
		if (this.txt)
		{
			this.txt.color = color;
		}
		if (this.txt2)
		{
			this.txt2.color = color;
		}
		if (this.lit)
		{
			this.lit.color = color;
		}
		if (this.sr)
		{
			this.sr.color = color;
		}
		if (this.ps)
		{
			this.ps.main.startColor = color;
		}
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0001C484 File Offset: 0x0001A684
	private void GetTarget()
	{
		this.gotTarget = true;
		if (this.customColorRenderer)
		{
			this.rend = base.GetComponent<Renderer>();
			this.block = new MaterialPropertyBlock();
		}
		this.img = base.GetComponent<Image>();
		this.txt = base.GetComponent<Text>();
		this.txt2 = base.GetComponent<TMP_Text>();
		this.lit = base.GetComponent<Light>();
		this.sr = base.GetComponent<SpriteRenderer>();
		this.ps = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x040004FC RID: 1276
	public HudColorType hct;

	// Token: 0x040004FD RID: 1277
	private Image img;

	// Token: 0x040004FE RID: 1278
	private Text txt;

	// Token: 0x040004FF RID: 1279
	private Light lit;

	// Token: 0x04000500 RID: 1280
	private SpriteRenderer sr;

	// Token: 0x04000501 RID: 1281
	private TMP_Text txt2;

	// Token: 0x04000502 RID: 1282
	private ParticleSystem ps;

	// Token: 0x04000503 RID: 1283
	private bool gotTarget;

	// Token: 0x04000504 RID: 1284
	public bool variationColor;

	// Token: 0x04000505 RID: 1285
	public int variationNumber;

	// Token: 0x04000506 RID: 1286
	public bool customColorRenderer;

	// Token: 0x04000507 RID: 1287
	private Renderer rend;

	// Token: 0x04000508 RID: 1288
	private MaterialPropertyBlock block;
}
