using System;
using I2.Loc;
using TMPro;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class UITextSwitcher : MonoBehaviour
{
	// Token: 0x06000A5F RID: 2655 RVA: 0x00030BFC File Offset: 0x0002EDFC
	private void Start()
	{
		this.UpdateUI();
		Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged += this.UpdateUI;
		IntConfigurable languageProfile = Singleton<GameMaster>.Instance.languageProfile;
		languageProfile.OnValueChanged = (Action<LiveData>)Delegate.Combine(languageProfile.OnValueChanged, new Action<LiveData>(this.UpdateUI));
		Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsSaved += this.UpdateUI;
		Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsLoaded += this.UpdateUI;
		BooleanConfigurable simplifiedControlsConfigurable = Singleton<GameMaster>.Instance.simplifiedControlsConfigurable;
		simplifiedControlsConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(simplifiedControlsConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateUI));
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00030CB4 File Offset: 0x0002EEB4
	private void OnDestroy()
	{
		if (Singleton<GameMaster>.Instance != null)
		{
			Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged -= this.UpdateUI;
			IntConfigurable languageProfile = Singleton<GameMaster>.Instance.languageProfile;
			languageProfile.OnValueChanged = (Action<LiveData>)Delegate.Remove(languageProfile.OnValueChanged, new Action<LiveData>(this.UpdateUI));
			Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsSaved -= this.UpdateUI;
			Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsLoaded -= this.UpdateUI;
			BooleanConfigurable simplifiedControlsConfigurable = Singleton<GameMaster>.Instance.simplifiedControlsConfigurable;
			simplifiedControlsConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(simplifiedControlsConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateUI));
		}
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x00030D73 File Offset: 0x0002EF73
	private void UpdateUI(LiveData d)
	{
		this.UpdateUI();
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x00030D73 File Offset: 0x0002EF73
	private void UpdateUI(GameMaster.InputDevice i)
	{
		this.UpdateUI();
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x00030D7C File Offset: 0x0002EF7C
	[ContextMenu("Update UI")]
	private void UpdateUI()
	{
		string text = ((this.textKey != "") ? UITextSwitcher.GetLocalizedSpritedText(this.textKey, this.inputAction) : "");
		this.tmPro.text = text;
		if (this.tmProCopyOrShadow)
		{
			this.tmProCopyOrShadow.text = text;
		}
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00030DD9 File Offset: 0x0002EFD9
	public void SetTerm(string newTerm)
	{
		this.textKey = newTerm;
		this.UpdateUI();
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00030DE8 File Offset: 0x0002EFE8
	public static string GetLocalizedSpritedText(string key, int extraInputActionIndex)
	{
		string text = LocalizationManager.GetTranslation(key, true, 0, true, false, null, null);
		if (text == null)
		{
			return "";
		}
		StanleyActions.KeyControllerPairSpriteTags extraActionBindingDescription = Singleton<GameMaster>.Instance.stanleyActions.GetExtraActionBindingDescription(extraInputActionIndex);
		if (Singleton<GameMaster>.Instance.InputDeviceType == GameMaster.InputDevice.KeyboardAndMouse)
		{
			text = text.Replace("%!K!%", extraActionBindingDescription.KeyboardSpriteTag);
			text = text.Replace("%! K!%", extraActionBindingDescription.KeyboardSpriteTag);
		}
		else
		{
			text = text.Replace("%!K!%", extraActionBindingDescription.GamepadSpriteTag);
			text = text.Replace("%! K!%", extraActionBindingDescription.GamepadSpriteTag);
		}
		return text;
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00030E74 File Offset: 0x0002F074
	public static string GetLocalizedSpritedText(string key, UITextSwitcher.InputAction inputAction)
	{
		string text = LocalizationManager.GetTranslation(key, true, 0, true, false, null, null);
		if (text == null)
		{
			return "";
		}
		StanleyActions.KeyControllerPairSpriteTags keyControllerPairSpriteTags;
		if (inputAction != UITextSwitcher.InputAction.Jump)
		{
			if (inputAction != UITextSwitcher.InputAction.Use)
			{
				keyControllerPairSpriteTags = Singleton<GameMaster>.Instance.stanleyActions.GetExtraActionBindingDescription((int)inputAction);
			}
			else
			{
				keyControllerPairSpriteTags = Singleton<GameMaster>.Instance.stanleyActions.GetUseBindingDescription();
			}
		}
		else
		{
			keyControllerPairSpriteTags = Singleton<GameMaster>.Instance.stanleyActions.GetJumpBindingDescription();
		}
		if (Singleton<GameMaster>.Instance.InputDeviceType == GameMaster.InputDevice.KeyboardAndMouse)
		{
			text = text.Replace("%!K!%", keyControllerPairSpriteTags.KeyboardSpriteTag);
		}
		else
		{
			text = text.Replace("%!K!%", keyControllerPairSpriteTags.GamepadSpriteTag);
		}
		return text;
	}

	// Token: 0x04000A53 RID: 2643
	[SerializeField]
	private string textKey;

	// Token: 0x04000A54 RID: 2644
	[SerializeField]
	private TMP_Text tmPro;

	// Token: 0x04000A55 RID: 2645
	[SerializeField]
	private TMP_Text tmProCopyOrShadow;

	// Token: 0x04000A56 RID: 2646
	[SerializeField]
	private UITextSwitcher.InputAction inputAction;

	// Token: 0x02000401 RID: 1025
	public enum InputAction
	{
		// Token: 0x040014E6 RID: 5350
		ExtraAction1,
		// Token: 0x040014E7 RID: 5351
		ExtraAction2,
		// Token: 0x040014E8 RID: 5352
		ExtraAction3,
		// Token: 0x040014E9 RID: 5353
		ExtraAction4,
		// Token: 0x040014EA RID: 5354
		Teleport,
		// Token: 0x040014EB RID: 5355
		Jump,
		// Token: 0x040014EC RID: 5356
		Use
	}
}
