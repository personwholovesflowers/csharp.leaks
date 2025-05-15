using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000410 RID: 1040
public class SliderValueToText : MonoBehaviour
{
	// Token: 0x060017B4 RID: 6068 RVA: 0x000C2734 File Offset: 0x000C0934
	private void Start()
	{
		switch (this.decimalType)
		{
		case DecimalType.Three:
			this.decString = "F3";
			break;
		case DecimalType.Two:
			this.decString = "F2";
			break;
		case DecimalType.One:
			this.decString = "F1";
			break;
		case DecimalType.NoDecimals:
			this.decString = "F0";
			break;
		}
		if (this.targetSlider == null)
		{
			this.targetSlider = base.GetComponentInParent<Slider>();
		}
		if (this.targetTextTMP == null)
		{
			this.targetTextTMP = base.GetComponent<TMP_Text>();
		}
		if (this.targetTextTMP == null && this.targetText == null)
		{
			this.targetText = base.GetComponent<Text>();
		}
		Color color = this.origColor.GetValueOrDefault();
		if (this.origColor == null)
		{
			color = (this.targetTextTMP ? this.targetTextTMP.color : this.targetText.color);
			this.origColor = new Color?(color);
		}
		this.nullColor = new Color(0f, 0f, 0f, 0f);
	}

	// Token: 0x060017B5 RID: 6069 RVA: 0x000C285C File Offset: 0x000C0A5C
	private void Update()
	{
		Color color = this.origColor ?? Color.black;
		string text;
		if (this.ifMax != "" && this.targetSlider.value == this.targetSlider.maxValue)
		{
			text = this.ifMax;
		}
		else if (this.ifMin != "" && this.targetSlider.value == this.targetSlider.minValue)
		{
			text = this.ifMin;
		}
		else
		{
			text = (this.targetSlider.value * this.multiplier).ToString(this.decString) + this.suffix;
		}
		if (this.maxColor != this.nullColor && this.targetSlider.value == this.targetSlider.maxValue)
		{
			color = this.maxColor;
		}
		else if (this.minColor != this.nullColor && this.targetSlider.value == this.targetSlider.minValue)
		{
			color = this.minColor;
		}
		if (this.targetTextTMP)
		{
			this.targetTextTMP.text = text;
			this.targetTextTMP.color = color;
			return;
		}
		this.targetText.text = text;
		this.targetText.color = color;
	}

	// Token: 0x060017B6 RID: 6070 RVA: 0x000C29C4 File Offset: 0x000C0BC4
	public void ConfigureFrom(SliderValueToTextConfig config)
	{
		this.decimalType = config.decimalType;
		this.multiplier = config.multiplier;
		this.suffix = config.suffix;
		this.ifMax = config.ifMax;
		this.ifMin = config.ifMin;
		this.minColor = config.minColor;
		this.maxColor = config.maxColor;
		this.Start();
	}

	// Token: 0x04002119 RID: 8473
	public DecimalType decimalType;

	// Token: 0x0400211A RID: 8474
	[FormerlySerializedAs("modifier")]
	public float multiplier = 1f;

	// Token: 0x0400211B RID: 8475
	private string decString;

	// Token: 0x0400211C RID: 8476
	private Slider targetSlider;

	// Token: 0x0400211D RID: 8477
	private Text targetText;

	// Token: 0x0400211E RID: 8478
	private TMP_Text targetTextTMP;

	// Token: 0x0400211F RID: 8479
	public string suffix;

	// Token: 0x04002120 RID: 8480
	public string ifMax;

	// Token: 0x04002121 RID: 8481
	public string ifMin;

	// Token: 0x04002122 RID: 8482
	public Color minColor;

	// Token: 0x04002123 RID: 8483
	public Color maxColor;

	// Token: 0x04002124 RID: 8484
	private Color? origColor;

	// Token: 0x04002125 RID: 8485
	private Color nullColor;
}
