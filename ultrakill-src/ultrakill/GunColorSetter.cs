using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023D RID: 573
public class GunColorSetter : MonoBehaviour
{
	// Token: 0x06000C4F RID: 3151 RVA: 0x00057B00 File Offset: 0x00055D00
	private void OnEnable()
	{
		this.weaponNumber = this.gctg.weaponNumber;
		this.altVersion = this.gctg.altVersion;
		this.redAmount = MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"r"
		}), 1f);
		this.greenAmount = MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"g"
		}), 1f);
		this.blueAmount = MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"b"
		}), 1f);
		this.metalAmount = MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"a"
		}), 0f);
		this.redSlider.value = this.redAmount;
		this.greenSlider.value = this.greenAmount;
		this.blueSlider.value = this.blueAmount;
		this.metalSlider.value = this.metalAmount;
		this.colorExample.color = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		float num = ((Vector3.Dot(Vector3.one, new Vector3(this.redAmount, this.greenAmount, this.blueAmount)) / 3f < 0.9f) ? 1f : 0.7f);
		this.metalExample.color = new Color(num, num, num, this.metalAmount);
		this.gctg.UpdatePreview();
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00057DA0 File Offset: 0x00055FA0
	public void SetRed(float amount)
	{
		this.redAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00057DAF File Offset: 0x00055FAF
	public void SetGreen(float amount)
	{
		this.greenAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00057DBE File Offset: 0x00055FBE
	public void SetBlue(float amount)
	{
		this.blueAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00057DCD File Offset: 0x00055FCD
	public void SetMetal(float amount)
	{
		this.metalAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00057DDC File Offset: 0x00055FDC
	public void UpdateColor()
	{
		this.weaponNumber = this.gctg.weaponNumber;
		this.altVersion = this.gctg.altVersion;
		MonoSingleton<PrefsManager>.Instance.SetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"r"
		}), this.redAmount);
		MonoSingleton<PrefsManager>.Instance.SetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"g"
		}), this.greenAmount);
		MonoSingleton<PrefsManager>.Instance.SetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"b"
		}), this.blueAmount);
		MonoSingleton<PrefsManager>.Instance.SetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"a"
		}), this.metalAmount);
		this.colorExample.color = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		float num = ((Vector3.Dot(Vector3.one, new Vector3(this.redAmount, this.greenAmount, this.blueAmount)) / 3f < 0.9f) ? 1f : 0.7f);
		this.metalExample.color = new Color(num, num, num, this.metalAmount);
		MonoSingleton<GunColorController>.Instance.UpdateGunColors();
		this.gctg.UpdatePreview();
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00058030 File Offset: 0x00056230
	public void UpdateSliders()
	{
		this.weaponNumber = this.gctg.weaponNumber;
		this.altVersion = this.gctg.altVersion;
		this.redAmount = MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"r"
		}), 1f);
		this.greenAmount = MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"g"
		}), 1f);
		this.blueAmount = MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"b"
		}), 1f);
		this.metalAmount = MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			this.weaponNumber.ToString(),
			".",
			this.colorNumber.ToString(),
			this.altVersion ? ".a" : ".",
			"a"
		}), 0f);
		this.redSlider.value = this.redAmount;
		this.greenSlider.value = this.greenAmount;
		this.blueSlider.value = this.blueAmount;
		this.metalSlider.value = this.metalAmount;
		this.colorExample.color = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		float num = ((Vector3.Dot(Vector3.one, new Vector3(this.redAmount, this.greenAmount, this.blueAmount)) / 3f < 0.9f) ? 1f : 0.7f);
		this.metalExample.color = new Color(num, num, num, this.metalAmount);
	}

	// Token: 0x0400102D RID: 4141
	private int weaponNumber;

	// Token: 0x0400102E RID: 4142
	public int colorNumber;

	// Token: 0x0400102F RID: 4143
	private bool altVersion;

	// Token: 0x04001030 RID: 4144
	private float redAmount;

	// Token: 0x04001031 RID: 4145
	private float greenAmount;

	// Token: 0x04001032 RID: 4146
	private float blueAmount;

	// Token: 0x04001033 RID: 4147
	private float metalAmount;

	// Token: 0x04001034 RID: 4148
	public Slider redSlider;

	// Token: 0x04001035 RID: 4149
	public Slider greenSlider;

	// Token: 0x04001036 RID: 4150
	public Slider blueSlider;

	// Token: 0x04001037 RID: 4151
	public Slider metalSlider;

	// Token: 0x04001038 RID: 4152
	public Image colorExample;

	// Token: 0x04001039 RID: 4153
	public Image metalExample;

	// Token: 0x0400103A RID: 4154
	[SerializeField]
	private GunColorTypeGetter gctg;
}
