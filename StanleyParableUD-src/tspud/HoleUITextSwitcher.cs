using System;
using I2.Loc;
using TMPro;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class HoleUITextSwitcher : MonoBehaviour
{
	// Token: 0x06000591 RID: 1425 RVA: 0x0001F44C File Offset: 0x0001D64C
	private void Start()
	{
		this.UpdateUI();
		Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged += this.UpdateUI;
		BooleanConfigurable simplifiedControlsConfigurable = Singleton<GameMaster>.Instance.simplifiedControlsConfigurable;
		simplifiedControlsConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(simplifiedControlsConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateUI));
		IntConfigurable languageProfile = Singleton<GameMaster>.Instance.languageProfile;
		languageProfile.OnValueChanged = (Action<LiveData>)Delegate.Combine(languageProfile.OnValueChanged, new Action<LiveData>(this.UpdateUI));
		Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsLoaded += this.UpdateUI;
		Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsSaved += this.UpdateUI;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0001F504 File Offset: 0x0001D704
	private void OnDestroy()
	{
		Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged -= this.UpdateUI;
		BooleanConfigurable simplifiedControlsConfigurable = Singleton<GameMaster>.Instance.simplifiedControlsConfigurable;
		simplifiedControlsConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(simplifiedControlsConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateUI));
		IntConfigurable languageProfile = Singleton<GameMaster>.Instance.languageProfile;
		languageProfile.OnValueChanged = (Action<LiveData>)Delegate.Remove(languageProfile.OnValueChanged, new Action<LiveData>(this.UpdateUI));
		Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsLoaded -= this.UpdateUI;
		Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsSaved -= this.UpdateUI;
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0001F5B3 File Offset: 0x0001D7B3
	private void UpdateUI(GameMaster.InputDevice i)
	{
		this.UpdateUI();
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0001F5B3 File Offset: 0x0001D7B3
	private void UpdateUI(LiveData d)
	{
		this.UpdateUI();
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0001F5BC File Offset: 0x0001D7BC
	private void UpdateUI()
	{
		HoleUITextSwitcher.ChangeTexts[] array = new HoleUITextSwitcher.ChangeTexts[] { this.changeTexts1, this.changeTexts2, this.changeTexts3, this.changeTexts4 };
		for (int i = 0; i < 4; i++)
		{
			string localizedSpritedText = HoleUITextSwitcher.GetLocalizedSpritedText(array[i].changeTextKey, i);
			array[i].changeText.text = localizedSpritedText;
			if (array[i].changeTextShadow)
			{
				array[i].changeTextShadow.text = localizedSpritedText;
			}
		}
		this.teleportText.text = HoleUITextSwitcher.GetLocalizedSpritedText(this.teleportKey, StanleyActions.HoleTeleportIndex);
		this.teleportTextCopy.text = HoleUITextSwitcher.GetLocalizedSpritedText(this.teleportKey, StanleyActions.HoleTeleportIndex);
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0001F690 File Offset: 0x0001D890
	public static string GetLocalizedSpritedText(string key, int inputIndex)
	{
		string text = LocalizationManager.GetTranslation(key, true, 0, true, false, null, null);
		if (Singleton<GameMaster>.Instance.UsingSimplifiedControls)
		{
			if (Singleton<GameMaster>.Instance.InputDeviceType == GameMaster.InputDevice.KeyboardAndMouse)
			{
				text = text.Replace("%!K!%", Singleton<GameMaster>.Instance.stanleyActions.GetUseBindingDescription().KeyboardSpriteTag);
			}
			else
			{
				text = text.Replace("%!K!%", Singleton<GameMaster>.Instance.stanleyActions.GetUseBindingDescription().GamepadSpriteTag);
			}
		}
		else if (Singleton<GameMaster>.Instance.InputDeviceType == GameMaster.InputDevice.KeyboardAndMouse)
		{
			text = text.Replace("%!K!%", Singleton<GameMaster>.Instance.stanleyActions.GetExtraActionBindingDescription(inputIndex).KeyboardSpriteTag);
		}
		else
		{
			text = text.Replace("%!K!%", Singleton<GameMaster>.Instance.stanleyActions.GetExtraActionBindingDescription(inputIndex).GamepadSpriteTag);
		}
		return text;
	}

	// Token: 0x040005D4 RID: 1492
	[SerializeField]
	private string teleportKey;

	// Token: 0x040005D5 RID: 1493
	[SerializeField]
	private TextMeshPro teleportText;

	// Token: 0x040005D6 RID: 1494
	[SerializeField]
	private TextMeshPro teleportTextCopy;

	// Token: 0x040005D7 RID: 1495
	[SerializeField]
	private HoleUITextSwitcher.ChangeTexts changeTexts1;

	// Token: 0x040005D8 RID: 1496
	[SerializeField]
	private HoleUITextSwitcher.ChangeTexts changeTexts2;

	// Token: 0x040005D9 RID: 1497
	[SerializeField]
	private HoleUITextSwitcher.ChangeTexts changeTexts3;

	// Token: 0x040005DA RID: 1498
	[SerializeField]
	private HoleUITextSwitcher.ChangeTexts changeTexts4;

	// Token: 0x020003BE RID: 958
	[Serializable]
	private struct ChangeTexts
	{
		// Token: 0x040013D6 RID: 5078
		public string changeTextKey;

		// Token: 0x040013D7 RID: 5079
		public TextMeshProUGUI changeText;

		// Token: 0x040013D8 RID: 5080
		public TextMeshProUGUI changeTextShadow;
	}
}
