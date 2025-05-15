using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000090 RID: 144
public class BossHealthBarTemplate : MonoBehaviour
{
	// Token: 0x060002C5 RID: 709 RVA: 0x0000FF7C File Offset: 0x0000E17C
	public void Initialize(BossHealthBar bossBar, SliderLayer[] colorLayers)
	{
		this.source = bossBar.source;
		List<Slider> list = new List<Slider>();
		List<Slider> list2 = new List<Slider>();
		HealthLayer[] healthLayers = bossBar.healthLayers;
		this.bossNameText.text = bossBar.bossName.ToUpper();
		float num = 0f;
		for (int i = 0; i < healthLayers.Length; i++)
		{
			BossHealthSliderTemplate bossHealthSliderTemplate = Object.Instantiate<BossHealthSliderTemplate>(this.sliderTemplate, this.sliderTemplate.transform.parent);
			bossHealthSliderTemplate.name = "Health After Image " + bossBar.bossName;
			list2.Add(bossHealthSliderTemplate.slider);
			bossHealthSliderTemplate.slider.minValue = num;
			bossHealthSliderTemplate.slider.maxValue = num + healthLayers[i].health;
			bossHealthSliderTemplate.gameObject.SetActive(true);
			bossHealthSliderTemplate.background.SetActive(i == 0);
			bossHealthSliderTemplate.fill.color = colorLayers[i].afterImageColor;
			BossHealthSliderTemplate bossHealthSliderTemplate2 = Object.Instantiate<BossHealthSliderTemplate>(this.sliderTemplate, this.sliderTemplate.transform.parent);
			bossHealthSliderTemplate2.name = "Health Slider " + bossBar.bossName;
			list.Add(bossHealthSliderTemplate2.slider);
			bossHealthSliderTemplate2.slider.minValue = num;
			bossHealthSliderTemplate2.slider.maxValue = num + healthLayers[i].health;
			bossHealthSliderTemplate2.gameObject.SetActive(true);
			bossHealthSliderTemplate2.background.SetActive(false);
			bossHealthSliderTemplate2.fill.color = colorLayers[i].color;
			num += healthLayers[i].health;
		}
		this.hpSlider = list.ToArray();
		this.hpAfterImage = list2.ToArray();
		this.textInstances = base.GetComponentsInChildren<TMP_Text>(true);
		this.filler = this.sliderTemplate.filler;
		this.originalPosition = this.filler.transform.localPosition;
		for (int j = this.hpSlider.Length - 1; j >= 0; j--)
		{
			if (bossBar.source.Health > this.hpSlider[j].minValue)
			{
				this.currentHpSlider = j;
				this.currentAfterImageSlider = this.currentHpSlider;
				break;
			}
		}
		Slider[] array = this.hpSlider;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].value = 0f;
		}
		array = this.hpAfterImage;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].value = 0f;
		}
		this.hpColors = new Color[this.hpSlider.Length];
		this.healFadeLerps = new float[this.hpSlider.Length];
		for (int l = 0; l < this.hpColors.Length; l++)
		{
			this.hpColors[l] = this.hpSlider[l].targetGraphic.color;
			this.hpSlider[l].targetGraphic.color = this.GetHPColor(l);
		}
		for (int m = 0; m < this.healFadeLerps.Length; m++)
		{
			this.healFadeLerps[m] = 1f;
		}
		if (bossBar.secondaryBar)
		{
			this.CreateSecondaryBar(bossBar);
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x000102B6 File Offset: 0x0000E4B6
	public void SetVisible(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x000102C4 File Offset: 0x0000E4C4
	private void CreateSecondaryBar(BossHealthBar bossBar)
	{
		if (this.secondaryObject)
		{
			return;
		}
		BossHealthSliderTemplate bossHealthSliderTemplate = Object.Instantiate<BossHealthSliderTemplate>(this.thinSliderTemplate, this.thinSliderTemplate.transform.parent);
		this.secondarySlider = bossHealthSliderTemplate.slider;
		this.secondaryObject = bossHealthSliderTemplate.gameObject;
		this.secondarySlider.targetGraphic.color = bossBar.secondaryBarColor;
		this.secondarySlider.value = bossBar.secondaryBarValue;
		this.secondaryObject.SetActive(true);
		MonoSingleton<BossBarManager>.Instance.ForceLayoutRebuild();
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00010350 File Offset: 0x0000E550
	public void UpdateSecondaryBar(BossHealthBar bossBar)
	{
		if (!this.secondaryObject || !this.secondarySlider)
		{
			this.CreateSecondaryBar(bossBar);
		}
		this.secondarySlider.value = bossBar.secondaryBarValue;
		this.secondarySlider.targetGraphic.color = bossBar.secondaryBarColor;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x000103A8 File Offset: 0x0000E5A8
	public void ResetSecondaryBar()
	{
		if (!this.secondaryObject && !this.secondarySlider)
		{
			return;
		}
		if (this.secondaryObject)
		{
			Object.Destroy(this.secondaryObject);
		}
		this.secondaryObject = null;
		this.secondarySlider = null;
		MonoSingleton<BossBarManager>.Instance.ForceLayoutRebuild();
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00010400 File Offset: 0x0000E600
	public void ScaleChanged(float scale)
	{
		TMP_Text[] array = this.textInstances;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.localScale = new Vector3(scale, 1f, 1f);
		}
	}

	// Token: 0x060002CB RID: 715 RVA: 0x0001043F File Offset: 0x0000E63F
	public void UpdateState(IEnemyHealthDetails details)
	{
		if (this.source == null || this.source != details)
		{
			this.source = details;
		}
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00010459 File Offset: 0x0000E659
	private Color GetHPColor(int index)
	{
		if (this.source != null && this.source.Blessed)
		{
			return BossHealthBarTemplate.IdolProtectedColor;
		}
		return this.hpColors[index];
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00010484 File Offset: 0x0000E684
	private void Update()
	{
		if (this.hpSlider[this.currentHpSlider].value != this.source.Health)
		{
			if (this.introCharge < this.source.Health)
			{
				this.introCharge = Mathf.MoveTowards(this.introCharge, this.source.Health, (this.source.Health - this.introCharge) * Time.deltaTime * 3f);
				Slider[] array = this.hpSlider;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].value = this.introCharge;
				}
			}
			else
			{
				if (this.hpSlider[this.currentHpSlider].value < this.source.Health)
				{
					this.hpSlider[this.currentHpSlider].targetGraphic.color = Color.green;
					this.healFadeLerps[this.currentHpSlider] = 0f;
				}
				this.shakeTime = 5f * (this.hpSlider[this.currentHpSlider].value - this.source.Health);
				this.hpSlider[this.currentHpSlider].value = this.source.Health;
				this.waitForDamage = 0.15f;
				if (this.hpSlider[this.currentHpSlider].minValue > this.source.Health && this.currentHpSlider > 0)
				{
					this.currentHpSlider--;
					this.hpSlider[this.currentHpSlider].value = this.source.Health;
				}
				else if (this.hpSlider[this.currentHpSlider].maxValue < this.source.Health && this.currentHpSlider < this.hpSlider.Length - 1)
				{
					this.hpSlider[this.currentHpSlider].value = this.hpSlider[this.currentHpSlider].value;
					this.currentHpSlider++;
				}
			}
		}
		if (this.hpAfterImage[this.currentAfterImageSlider].value != this.hpSlider[this.currentHpSlider].value)
		{
			if (this.waitForDamage > 0f && this.hpSlider[0].value > 0f)
			{
				this.waitForDamage = Mathf.MoveTowards(this.waitForDamage, 0f, Time.deltaTime);
			}
			else if (this.hpAfterImage[this.currentAfterImageSlider].value > this.hpSlider[this.currentHpSlider].value)
			{
				this.hpAfterImage[this.currentAfterImageSlider].value = Mathf.MoveTowards(this.hpAfterImage[this.currentAfterImageSlider].value, this.hpSlider[this.currentHpSlider].value, Time.deltaTime * (Mathf.Abs((this.hpAfterImage[this.currentAfterImageSlider].value - this.hpSlider[this.currentHpSlider].value) * 5f) + 0.5f));
			}
			else
			{
				this.hpAfterImage[this.currentAfterImageSlider].value = this.hpSlider[this.currentHpSlider].value;
			}
			if (this.hpAfterImage[this.currentAfterImageSlider].value <= this.hpAfterImage[this.currentAfterImageSlider].minValue && this.currentAfterImageSlider > 0)
			{
				this.currentAfterImageSlider--;
			}
		}
		for (int j = 0; j < this.hpColors.Length; j++)
		{
			if (this.hpSlider[j].targetGraphic.color != this.GetHPColor(j))
			{
				this.healFadeLerps[j] = Mathf.MoveTowards(this.healFadeLerps[j], 1f, Time.deltaTime * 2f);
				this.hpSlider[j].targetGraphic.color = Color.Lerp(Color.green, this.GetHPColor(j), this.healFadeLerps[j]);
			}
		}
		if (this.shakeTime != 0f)
		{
			if (this.shakeTime > 10f)
			{
				this.shakeTime = 10f;
			}
			this.shakeTime = Mathf.MoveTowards(this.shakeTime, 0f, Time.deltaTime * 10f);
			if (this.shakeTime <= 0f)
			{
				this.shakeTime = 0f;
				this.filler.transform.localPosition = this.originalPosition;
				return;
			}
			this.filler.transform.localPosition = new Vector3(this.originalPosition.x + Random.Range(-this.shakeTime, this.shakeTime), this.originalPosition.y + Random.Range(-this.shakeTime, this.shakeTime), this.originalPosition.z);
		}
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00010940 File Offset: 0x0000EB40
	public void ChangeName(string text)
	{
		this.bossNameText.text = text;
		TMP_Text[] array = this.textInstances;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = text;
		}
	}

	// Token: 0x0400034A RID: 842
	private static readonly Color IdolProtectedColor = new Color(0.25f, 0.75f, 1f);

	// Token: 0x0400034B RID: 843
	public BossHealthSliderTemplate sliderTemplate;

	// Token: 0x0400034C RID: 844
	public TMP_Text bossNameText;

	// Token: 0x0400034D RID: 845
	public BossHealthSliderTemplate thinSliderTemplate;

	// Token: 0x0400034E RID: 846
	private TMP_Text[] textInstances;

	// Token: 0x0400034F RID: 847
	private Slider[] hpSlider;

	// Token: 0x04000350 RID: 848
	private Slider[] hpAfterImage;

	// Token: 0x04000351 RID: 849
	private Color[] hpColors;

	// Token: 0x04000352 RID: 850
	private float[] healFadeLerps;

	// Token: 0x04000353 RID: 851
	private float introCharge;

	// Token: 0x04000354 RID: 852
	private float waitForDamage;

	// Token: 0x04000355 RID: 853
	private GameObject filler;

	// Token: 0x04000356 RID: 854
	private float shakeTime;

	// Token: 0x04000357 RID: 855
	private Vector3 originalPosition;

	// Token: 0x04000358 RID: 856
	private bool done;

	// Token: 0x04000359 RID: 857
	private Slider secondarySlider;

	// Token: 0x0400035A RID: 858
	private GameObject secondaryObject;

	// Token: 0x0400035B RID: 859
	private int currentHpSlider;

	// Token: 0x0400035C RID: 860
	private int currentAfterImageSlider;

	// Token: 0x0400035D RID: 861
	private IEnemyHealthDetails source;
}
