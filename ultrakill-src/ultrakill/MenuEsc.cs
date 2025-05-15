using System;
using SettingsMenu.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020002F1 RID: 753
public class MenuEsc : MonoBehaviour
{
	// Token: 0x06001092 RID: 4242 RVA: 0x0007EF65 File Offset: 0x0007D165
	private void OnEnable()
	{
		MenuEsc.current = this;
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x0007EF6D File Offset: 0x0007D16D
	private void OnDisable()
	{
		if (MenuEsc.current == this)
		{
			MenuEsc.current = null;
		}
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x0007EF84 File Offset: 0x0007D184
	private void Update()
	{
		if (MenuEsc.current != null && MenuEsc.current != this)
		{
			return;
		}
		MenuEsc.current = this;
		if (SettingsMenu.selectedSomethingThisFrame)
		{
			return;
		}
		Slider slider;
		if (MonoSingleton<InputManager>.Instance.InputSource.Pause.WasPerformedThisFrame || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame && EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.TryGetComponent<Slider>(out slider)))
		{
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				BackSelectEvent componentInParent = EventSystem.current.currentSelectedGameObject.GetComponentInParent<BackSelectEvent>();
				if (componentInParent != null)
				{
					componentInParent.InvokeOnBack();
				}
				BackSelectOverride backSelectOverride;
				if (EventSystem.current.currentSelectedGameObject.TryGetComponent<BackSelectOverride>(out backSelectOverride))
				{
					backSelectOverride.Selectable.Select();
					return;
				}
			}
			if (this.DontClose)
			{
				return;
			}
			if (this.HardClose)
			{
				if (SandboxHud.SavesMenuOpen)
				{
					MonoSingleton<SandboxHud>.Instance.HideSavesMenu();
				}
				MonoSingleton<OptionsManager>.Instance.CloseOptions();
				MonoSingleton<OptionsManager>.Instance.UnPause();
				MonoSingleton<OptionsManager>.Instance.UnFreeze();
			}
			base.gameObject.SetActive(false);
			if (this.previousPage != null)
			{
				this.previousPage.SetActive(true);
				Selectable selectable;
				if (this.previousPage.TryGetComponent<Selectable>(out selectable))
				{
					selectable.Select();
				}
			}
			UltrakillEvent onClose = this.OnClose;
			if (onClose == null)
			{
				return;
			}
			onClose.Invoke("");
		}
	}

	// Token: 0x04001688 RID: 5768
	private static MenuEsc current;

	// Token: 0x04001689 RID: 5769
	public GameObject previousPage;

	// Token: 0x0400168A RID: 5770
	public bool HardClose;

	// Token: 0x0400168B RID: 5771
	public bool DontClose;

	// Token: 0x0400168C RID: 5772
	public UltrakillEvent OnClose;
}
