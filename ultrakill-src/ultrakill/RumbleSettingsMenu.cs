using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200039E RID: 926
public class RumbleSettingsMenu : MonoBehaviour
{
	// Token: 0x06001546 RID: 5446 RVA: 0x000ADCB4 File Offset: 0x000ABEB4
	private void Start()
	{
		this.optionTemplate.gameObject.SetActive(false);
		Button button = this.totalWrapper;
		RumbleKey[] all = RumbleProperties.All;
		for (int i = 0; i < all.Length; i++)
		{
			RumbleKey rumbleKey = all[i];
			RumbleKeyOptionEntry option = Object.Instantiate<RumbleKeyOptionEntry>(this.optionTemplate, this.container);
			option.gameObject.SetActive(true);
			option.key = rumbleKey;
			option.keyName.text = RumbleManager.ResolveFullName(rumbleKey);
			float num = MonoSingleton<RumbleManager>.Instance.ResolveDuration(rumbleKey);
			if (num >= float.PositiveInfinity)
			{
				option.durationContainer.gameObject.SetActive(false);
			}
			else
			{
				option.durationSlider.SetValueWithoutNotify(num);
				option.durationSlider.onValueChanged.AddListener(delegate(float value)
				{
					option.SetDuration(value);
				});
			}
			option.intensitySlider.SetValueWithoutNotify(MonoSingleton<RumbleManager>.Instance.ResolveIntensity(rumbleKey));
			option.intensitySlider.onValueChanged.AddListener(delegate(float value)
			{
				option.SetIntensity(value);
			});
			Navigation navigation = option.intensityWrapper.navigation;
			navigation.selectOnUp = button;
			option.intensityWrapper.navigation = navigation;
			navigation = button.navigation;
			navigation.selectOnDown = option.intensityWrapper;
			button.navigation = navigation;
			if (!option.durationContainer.gameObject.activeSelf)
			{
				button = option.intensityWrapper;
			}
			else
			{
				button = option.durationWrapper;
			}
			if (button == null)
			{
				Debug.LogError("Previous button is null");
			}
		}
		Navigation navigation2 = button.navigation;
		navigation2.selectOnDown = this.quitButton;
		button.navigation = navigation2;
		Navigation navigation3 = this.quitButton.navigation;
		navigation3.selectOnUp = button;
		this.quitButton.navigation = navigation3;
		this.totalSlider.SetValueWithoutNotify(MonoSingleton<PrefsManager>.Instance.GetFloat("totalRumbleIntensity", 0f));
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x000ADEEC File Offset: 0x000AC0EC
	public void ChangeMasterMulti(float value)
	{
		MonoSingleton<PrefsManager>.Instance.SetFloat("totalRumbleIntensity", value);
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x000ADEFE File Offset: 0x000AC0FE
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x04001D96 RID: 7574
	[SerializeField]
	private RumbleKeyOptionEntry optionTemplate;

	// Token: 0x04001D97 RID: 7575
	[SerializeField]
	private Transform container;

	// Token: 0x04001D98 RID: 7576
	[SerializeField]
	private Button totalWrapper;

	// Token: 0x04001D99 RID: 7577
	[SerializeField]
	private Button quitButton;

	// Token: 0x04001D9A RID: 7578
	[SerializeField]
	private Slider totalSlider;
}
