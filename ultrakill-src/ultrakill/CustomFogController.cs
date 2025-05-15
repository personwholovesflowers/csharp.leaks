using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000139 RID: 313
public class CustomFogController : MonoBehaviour
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x00029666 File Offset: 0x00027866
	private bool fogDisabled
	{
		get
		{
			return this.fogState == CustomFogController.FogState.Disabled;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000608 RID: 1544 RVA: 0x00029671 File Offset: 0x00027871
	private bool fogStatic
	{
		get
		{
			return this.fogState == CustomFogController.FogState.Static;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000609 RID: 1545 RVA: 0x0002967C File Offset: 0x0002787C
	private bool fogDynamic
	{
		get
		{
			return this.fogState == CustomFogController.FogState.Dynamic;
		}
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x00029688 File Offset: 0x00027888
	private void Start()
	{
		this.disabledShopButton = this.disabledButton.GetComponent<ShopButton>();
		this.staticShopButton = this.staticButton.GetComponent<ShopButton>();
		this.dynamicShopButton = this.dynamicButton.GetComponent<ShopButton>();
		this.disabledShopButton.PointerClickSuccess += delegate
		{
			this.SetState(CustomFogController.FogState.Disabled);
		};
		this.staticShopButton.PointerClickSuccess += delegate
		{
			this.SetState(CustomFogController.FogState.Static);
		};
		this.dynamicShopButton.PointerClickSuccess += delegate
		{
			this.SetState(CustomFogController.FogState.Dynamic);
		};
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("cyberGrind.theme", 0);
		this.fogState = (CustomFogController.FogState)MonoSingleton<PrefsManager>.Instance.GetIntLocal("cyberGrind.fogState", (int)this.presets[@int].fogState);
		this.disabledButton.interactable = !this.fogDisabled;
		this.disabledShopButton.deactivated = this.fogDisabled;
		this.staticButton.interactable = !this.fogStatic;
		this.staticShopButton.deactivated = this.fogStatic;
		this.dynamicButton.interactable = !this.fogDynamic;
		this.dynamicShopButton.deactivated = this.fogDynamic;
		this.redAmount = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("cyberGrind.fogColor.r", this.presets[@int].redAmount);
		this.redSlider.SetValueWithoutNotify(this.redAmount);
		this.greenAmount = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("cyberGrind.fogColor.g", this.presets[@int].greenAmount);
		this.greenSlider.SetValueWithoutNotify(this.greenAmount);
		this.blueAmount = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("cyberGrind.fogColor.b", this.presets[@int].blueAmount);
		this.blueSlider.SetValueWithoutNotify(this.blueAmount);
		this.colorImage.color = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		RenderSettings.fogColor = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		this.startDistance = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("cyberGrind.fogStartDistance", this.presets[@int].startDistance);
		this.endDistance = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("cyberGrind.fogEndDistance", this.presets[@int].endDistance);
		if (this.startDistance == this.endDistance)
		{
			if (this.startDistance == this.startDistanceSlider.minValue)
			{
				this.endDistance = this.startDistance + 1f;
			}
			else
			{
				this.startDistance = this.endDistance - 1f;
			}
		}
		this.startDistanceSlider.SetValueWithoutNotify(this.startDistance);
		this.endDistanceSlider.SetValueWithoutNotify(this.endDistance);
		this.disabledText.SetActive(this.fogDisabled);
		this.startDistanceSliderGameObject.SetActive(this.fogStatic);
		this.endDistanceSliderGameObject.SetActive(!this.fogDisabled);
		this.fogSetterBounds.enabled = this.fogDynamic;
		if (!this.fogDynamic)
		{
			RenderSettings.fogStartDistance = this.startDistance;
		}
		RenderSettings.fogEndDistance = this.endDistance;
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x000299B0 File Offset: 0x00027BB0
	public void SetState(CustomFogController.FogState state)
	{
		MonoSingleton<PrefsManager>.Instance.SetIntLocal("cyberGrind.fogState", (int)state);
		this.fogState = state;
		RenderSettings.fog = !this.fogDisabled && this.levelStarted;
		this.fogSetterBounds.enabled = this.fogDynamic;
		if (!this.fogDynamic)
		{
			RenderSettings.fogStartDistance = this.startDistance;
		}
		RenderSettings.fogEndDistance = this.endDistance;
		this.disabledButton.interactable = !this.fogDisabled;
		this.disabledShopButton.deactivated = this.fogDisabled;
		this.staticButton.interactable = !this.fogStatic;
		this.staticShopButton.deactivated = this.fogStatic;
		this.dynamicButton.interactable = !this.fogDynamic;
		this.dynamicShopButton.deactivated = this.fogDynamic;
		this.disabledText.SetActive(this.fogDisabled);
		this.startDistanceSliderGameObject.SetActive(this.fogStatic);
		this.endDistanceSliderGameObject.SetActive(!this.fogDisabled);
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00029ABE File Offset: 0x00027CBE
	public void SetRed(float amount)
	{
		this.redAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00029ACD File Offset: 0x00027CCD
	public void SetGreen(float amount)
	{
		this.greenAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00029ADC File Offset: 0x00027CDC
	public void SetBlue(float amount)
	{
		this.blueAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00029AEC File Offset: 0x00027CEC
	private void UpdateColor()
	{
		MonoSingleton<PrefsManager>.Instance.SetFloatLocal("cyberGrind.fogColor.r", this.redAmount);
		MonoSingleton<PrefsManager>.Instance.SetFloatLocal("cyberGrind.fogColor.g", this.greenAmount);
		MonoSingleton<PrefsManager>.Instance.SetFloatLocal("cyberGrind.fogColor.b", this.blueAmount);
		this.colorImage.color = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		RenderSettings.fogColor = new Color(this.redAmount, this.greenAmount, this.blueAmount);
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x00029B78 File Offset: 0x00027D78
	public void SetFogStartDistance(float distance)
	{
		if (distance >= this.endDistance)
		{
			if (distance + 1f > this.endDistanceSlider.maxValue)
			{
				distance = this.endDistanceSlider.maxValue - 1f;
				this.endDistance = this.endDistanceSlider.maxValue;
				this.startDistanceSlider.SetValueWithoutNotify(distance);
			}
			else
			{
				this.endDistance = distance + 1f;
			}
			this.endDistanceSlider.SetValueWithoutNotify(this.endDistance);
			MonoSingleton<PrefsManager>.Instance.SetFloatLocal("cyberGrind.fogEndDistance", this.endDistance);
			this.fogSetterBounds.ChangeDistance(this.endDistance);
			if (!this.fogDynamic)
			{
				RenderSettings.fogEndDistance = this.endDistance;
			}
		}
		this.startDistance = distance;
		MonoSingleton<PrefsManager>.Instance.SetFloatLocal("cyberGrind.fogStartDistance", this.startDistance);
		RenderSettings.fogStartDistance = this.startDistance;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00029C58 File Offset: 0x00027E58
	public void SetFogEndDistance(float distance)
	{
		if (distance <= this.startDistance)
		{
			if (distance - 1f < this.startDistanceSlider.minValue)
			{
				distance = this.startDistanceSlider.minValue + 1f;
				this.startDistance = this.startDistanceSlider.minValue;
				this.endDistanceSlider.SetValueWithoutNotify(distance);
			}
			else
			{
				this.startDistance = distance - 1f;
			}
			this.startDistanceSlider.SetValueWithoutNotify(this.startDistance);
			MonoSingleton<PrefsManager>.Instance.SetFloatLocal("cyberGrind.fogStartDistance", this.startDistance);
			if (!this.fogDynamic)
			{
				RenderSettings.fogStartDistance = this.startDistance;
			}
		}
		this.endDistance = distance;
		MonoSingleton<PrefsManager>.Instance.SetFloatLocal("cyberGrind.fogEndDistance", this.endDistance);
		this.fogSetterBounds.ChangeDistance(this.endDistance);
		if (!this.fogDynamic)
		{
			RenderSettings.fogEndDistance = this.endDistance;
		}
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x00029D40 File Offset: 0x00027F40
	public void ResetValues()
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("cyberGrind.theme", 0);
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogState");
		this.fogState = this.presets[@int].fogState;
		RenderSettings.fog = !this.fogDisabled && this.levelStarted;
		this.disabledButton.interactable = !this.fogDisabled;
		this.disabledShopButton.deactivated = this.fogDisabled;
		this.staticButton.interactable = !this.fogStatic;
		this.staticShopButton.deactivated = this.fogStatic;
		this.dynamicButton.interactable = !this.fogDynamic;
		this.dynamicShopButton.deactivated = this.fogDynamic;
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogColor.r");
		this.redAmount = this.presets[@int].redAmount;
		this.redSlider.SetValueWithoutNotify(this.redAmount);
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogColor.g");
		this.greenAmount = this.presets[@int].greenAmount;
		this.greenSlider.SetValueWithoutNotify(this.greenAmount);
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogColor.b");
		this.blueAmount = this.presets[@int].blueAmount;
		this.blueSlider.SetValueWithoutNotify(this.blueAmount);
		this.colorImage.color = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		RenderSettings.fogColor = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogStartDistance");
		this.startDistance = this.presets[@int].startDistance;
		this.startDistanceSlider.SetValueWithoutNotify(this.startDistance);
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogEndDistance");
		this.endDistance = this.presets[@int].endDistance;
		this.endDistanceSlider.SetValueWithoutNotify(this.endDistance);
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogDynamicDistance");
		this.disabledText.SetActive(this.fogDisabled);
		this.startDistanceSliderGameObject.SetActive(this.fogStatic);
		this.endDistanceSliderGameObject.SetActive(!this.fogDisabled);
		this.fogSetterBounds.enabled = this.fogDynamic;
		this.fogSetterBounds.ChangeDistance(this.endDistance);
		if (!this.fogDynamic)
		{
			RenderSettings.fogStartDistance = this.startDistance;
			RenderSettings.fogEndDistance = this.endDistance;
		}
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x00029FE0 File Offset: 0x000281E0
	public void SetPreset(int index)
	{
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogState");
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogColor.r");
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogColor.g");
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogColor.b");
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogStartDistance");
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogEndDistance");
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.fogDynamicDistance");
		this.fogState = (CustomFogController.FogState)MonoSingleton<PrefsManager>.Instance.GetIntLocal("cyberGrind.fogState", (int)this.presets[index].fogState);
		RenderSettings.fog = !this.fogDisabled && this.levelStarted;
		this.redAmount = this.presets[index].redAmount;
		this.redSlider.SetValueWithoutNotify(this.redAmount);
		this.greenAmount = this.presets[index].greenAmount;
		this.greenSlider.SetValueWithoutNotify(this.greenAmount);
		this.blueAmount = this.presets[index].blueAmount;
		this.blueSlider.SetValueWithoutNotify(this.blueAmount);
		this.colorImage.color = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		RenderSettings.fogColor = new Color(this.redAmount, this.greenAmount, this.blueAmount);
		this.startDistance = this.presets[index].startDistance;
		this.startDistanceSlider.SetValueWithoutNotify(this.startDistance);
		this.endDistance = this.presets[index].endDistance;
		this.endDistanceSlider.SetValueWithoutNotify(this.endDistance);
		this.disabledText.SetActive(this.fogDisabled);
		this.startDistanceSliderGameObject.SetActive(this.fogStatic);
		this.endDistanceSliderGameObject.SetActive(!this.fogDisabled);
		this.fogSetterBounds.enabled = this.fogDynamic;
		this.fogSetterBounds.ChangeDistance(this.endDistance);
		if (!this.fogDynamic)
		{
			RenderSettings.fogStartDistance = this.startDistance;
			RenderSettings.fogEndDistance = this.endDistance;
		}
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0002A20E File Offset: 0x0002840E
	public void LevelStart()
	{
		this.levelStarted = true;
		RenderSettings.fog = !this.fogDisabled && this.levelStarted;
	}

	// Token: 0x04000825 RID: 2085
	[SerializeField]
	private CustomFogController.FogState fogState;

	// Token: 0x04000826 RID: 2086
	[SerializeField]
	private Button disabledButton;

	// Token: 0x04000827 RID: 2087
	private ShopButton disabledShopButton;

	// Token: 0x04000828 RID: 2088
	[SerializeField]
	private Button staticButton;

	// Token: 0x04000829 RID: 2089
	private ShopButton staticShopButton;

	// Token: 0x0400082A RID: 2090
	[SerializeField]
	private Button dynamicButton;

	// Token: 0x0400082B RID: 2091
	private ShopButton dynamicShopButton;

	// Token: 0x0400082C RID: 2092
	private float redAmount;

	// Token: 0x0400082D RID: 2093
	private float greenAmount;

	// Token: 0x0400082E RID: 2094
	private float blueAmount;

	// Token: 0x0400082F RID: 2095
	[Space]
	[SerializeField]
	private Slider redSlider;

	// Token: 0x04000830 RID: 2096
	[SerializeField]
	private Slider greenSlider;

	// Token: 0x04000831 RID: 2097
	[SerializeField]
	private Slider blueSlider;

	// Token: 0x04000832 RID: 2098
	[Space]
	[SerializeField]
	private Image colorImage;

	// Token: 0x04000833 RID: 2099
	private float startDistance;

	// Token: 0x04000834 RID: 2100
	private float endDistance;

	// Token: 0x04000835 RID: 2101
	[Space]
	[SerializeField]
	private Slider startDistanceSlider;

	// Token: 0x04000836 RID: 2102
	[SerializeField]
	private Slider endDistanceSlider;

	// Token: 0x04000837 RID: 2103
	[Space]
	[SerializeField]
	private GameObject disabledText;

	// Token: 0x04000838 RID: 2104
	[SerializeField]
	private GameObject startDistanceSliderGameObject;

	// Token: 0x04000839 RID: 2105
	[SerializeField]
	private GameObject endDistanceSliderGameObject;

	// Token: 0x0400083A RID: 2106
	[Space]
	[SerializeField]
	private FogSetterBounds fogSetterBounds;

	// Token: 0x0400083B RID: 2107
	private bool levelStarted;

	// Token: 0x0400083C RID: 2108
	[Header("Preset Values")]
	[SerializeField]
	private CustomFogController.ValuePreset[] presets = new CustomFogController.ValuePreset[2];

	// Token: 0x0200013A RID: 314
	[Serializable]
	public enum FogState
	{
		// Token: 0x0400083E RID: 2110
		Disabled,
		// Token: 0x0400083F RID: 2111
		Static,
		// Token: 0x04000840 RID: 2112
		Dynamic
	}

	// Token: 0x0200013B RID: 315
	[Serializable]
	private struct ValuePreset
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x0002A25C File Offset: 0x0002845C
		public ValuePreset(CustomFogController.FogState fogState, float redAmount, float greenAmount, float blueAmount, float startDistance, float endDistance)
		{
			this.fogState = fogState;
			this.redAmount = redAmount;
			this.greenAmount = greenAmount;
			this.blueAmount = blueAmount;
			this.startDistance = startDistance;
			this.endDistance = endDistance;
		}

		// Token: 0x04000841 RID: 2113
		public CustomFogController.FogState fogState;

		// Token: 0x04000842 RID: 2114
		public float redAmount;

		// Token: 0x04000843 RID: 2115
		public float greenAmount;

		// Token: 0x04000844 RID: 2116
		public float blueAmount;

		// Token: 0x04000845 RID: 2117
		public float startDistance;

		// Token: 0x04000846 RID: 2118
		public float endDistance;
	}
}
