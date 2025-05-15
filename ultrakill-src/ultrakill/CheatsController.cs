using System;
using plog;
using TMPro;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

// Token: 0x020000B0 RID: 176
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class CheatsController : MonoSingleton<CheatsController>
{
	// Token: 0x06000368 RID: 872 RVA: 0x000159B0 File Offset: 0x00013BB0
	private static bool TryGetKeyboardButton(int sequenceIndex, out ButtonControl button)
	{
		button = null;
		if (Keyboard.current == null)
		{
			return false;
		}
		KeyCode keyCode = CheatsController.Sequence[sequenceIndex];
		if (keyCode != KeyCode.A)
		{
			if (keyCode != KeyCode.B)
			{
				switch (keyCode)
				{
				case KeyCode.UpArrow:
					button = Keyboard.current.upArrowKey;
					break;
				case KeyCode.DownArrow:
					button = Keyboard.current.downArrowKey;
					break;
				case KeyCode.RightArrow:
					button = Keyboard.current.rightArrowKey;
					break;
				case KeyCode.LeftArrow:
					button = Keyboard.current.leftArrowKey;
					break;
				}
			}
			else
			{
				button = Keyboard.current.bKey;
			}
		}
		else
		{
			button = Keyboard.current.aKey;
		}
		return button != null;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00015A50 File Offset: 0x00013C50
	private static bool TryGetGamepadButton(int sequenceIndex, out ButtonControl button)
	{
		button = null;
		if (Gamepad.current == null)
		{
			return false;
		}
		KeyCode keyCode = CheatsController.Sequence[sequenceIndex];
		if (keyCode != KeyCode.A)
		{
			if (keyCode != KeyCode.B)
			{
				switch (keyCode)
				{
				case KeyCode.UpArrow:
					button = Gamepad.current.dpad.up;
					break;
				case KeyCode.DownArrow:
					button = Gamepad.current.dpad.down;
					break;
				case KeyCode.RightArrow:
					button = Gamepad.current.dpad.right;
					break;
				case KeyCode.LeftArrow:
					button = Gamepad.current.dpad.left;
					break;
				}
			}
			else
			{
				button = Gamepad.current.buttonEast;
			}
		}
		else
		{
			button = Gamepad.current.buttonSouth;
		}
		return button != null;
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00015B04 File Offset: 0x00013D04
	public void ShowTeleportPanel()
	{
		this.cheatsTeleportPanel.SetActive(true);
		GameStateManager.Instance.RegisterState(new GameState("teleport-menu", this.cheatsTeleportPanel)
		{
			cursorLock = LockMode.Unlock
		});
		MonoSingleton<OptionsManager>.Instance.Freeze();
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00015B3D File Offset: 0x00013D3D
	private void Start()
	{
		if (!CheatsManager.KeepCheatsEnabled)
		{
			return;
		}
		MonoSingleton<AssistController>.Instance.cheatsEnabled = true;
		this.consentScreen.SetActive(false);
		this.cheatsEnabled = true;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00015B65 File Offset: 0x00013D65
	public void PlayToggleSound(bool newState)
	{
		if (newState)
		{
			this.cheatEnabledSound.Play();
			return;
		}
		this.cheatDisabledSound.Play();
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00015B84 File Offset: 0x00013D84
	private void ProcessInput()
	{
		ButtonControl buttonControl;
		CheatsController.TryGetGamepadButton(this.sequenceIndex, out buttonControl);
		ButtonControl buttonControl2;
		CheatsController.TryGetKeyboardButton(this.sequenceIndex, out buttonControl2);
		if ((buttonControl2 != null && buttonControl2.wasPressedThisFrame) || (buttonControl != null && buttonControl.wasPressedThisFrame))
		{
			this.sequenceIndex++;
			if (this.sequenceIndex == CheatsController.Sequence.Length)
			{
				MonoSingleton<OptionsManager>.Instance.Pause();
				this.consentScreen.SetActive(true);
				this.sequenceIndex = 0;
				return;
			}
		}
		else
		{
			Keyboard current = Keyboard.current;
			if ((current != null && current.anyKey.wasPressedThisFrame) || this.AnyGamepadButtonPressed())
			{
				this.sequenceIndex = 0;
			}
		}
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00015C28 File Offset: 0x00013E28
	private bool AnyGamepadButtonPressed()
	{
		if (Gamepad.current == null)
		{
			return false;
		}
		foreach (InputControl inputControl in Gamepad.current.allControls)
		{
			ButtonControl buttonControl = inputControl as ButtonControl;
			if (buttonControl != null && buttonControl.wasPressedThisFrame)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00015C9C File Offset: 0x00013E9C
	private bool GamepadCombo()
	{
		return Gamepad.current != null && Gamepad.current.selectButton.isPressed && Gamepad.current.rightTrigger.wasPressedThisFrame;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00015CCC File Offset: 0x00013ECC
	public void Update()
	{
		if (!this.cheatsEnabled)
		{
			this.ProcessInput();
		}
		bool flag = this.cheatsEnabled && !HideCheatsStatus.HideStatus;
		if (this.cheatsEnabled && MonoSingleton<CheatsManager>.Instance && MonoSingleton<CheatsManager>.Instance.IsMenuOpen())
		{
			flag = true;
		}
		this.cheatsEnabledPanel.SetActive(flag);
		this.cheatsInfoPanel.SetActive(flag);
		if (MonoSingleton<CheatBinds>.Instance.isRebinding || !this.cheatsEnabled)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.BackQuote) || this.GamepadCombo())
		{
			if (MonoSingleton<OptionsManager>.Instance.paused)
			{
				CheatsController.Log.Info("Un-Paused", null, null, null);
				if (SandboxHud.SavesMenuOpen)
				{
					MonoSingleton<SandboxHud>.Instance.HideSavesMenu();
				}
				MonoSingleton<CheatsManager>.Instance.HideMenu();
				return;
			}
			CheatsController.Log.Info("Paused", null, null, null);
			MonoSingleton<OptionsManager>.Instance.Pause();
			MonoSingleton<CheatsManager>.Instance.ShowMenu();
		}
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00015DD0 File Offset: 0x00013FD0
	public void ActivateCheats()
	{
		MonoSingleton<AssistController>.Instance.cheatsEnabled = true;
		this.consentScreen.SetActive(false);
		this.cheatsEnabled = true;
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00015DF0 File Offset: 0x00013FF0
	public void Cancel()
	{
		this.consentScreen.SetActive(false);
	}

	// Token: 0x0400045A RID: 1114
	private static readonly global::plog.Logger Log = new global::plog.Logger("CheatsController");

	// Token: 0x0400045B RID: 1115
	public GameObject spawnerArm;

	// Token: 0x0400045C RID: 1116
	public GameObject fullBrightLight;

	// Token: 0x0400045D RID: 1117
	private static readonly KeyCode[] Sequence = new KeyCode[]
	{
		KeyCode.UpArrow,
		KeyCode.UpArrow,
		KeyCode.DownArrow,
		KeyCode.DownArrow,
		KeyCode.LeftArrow,
		KeyCode.RightArrow,
		KeyCode.LeftArrow,
		KeyCode.RightArrow,
		KeyCode.B,
		KeyCode.A
	};

	// Token: 0x0400045E RID: 1118
	[Space]
	[SerializeField]
	private GameObject consentScreen;

	// Token: 0x0400045F RID: 1119
	[SerializeField]
	private GameObject cheatsEnabledPanel;

	// Token: 0x04000460 RID: 1120
	[SerializeField]
	private GameObject cheatsInfoPanel;

	// Token: 0x04000461 RID: 1121
	[SerializeField]
	public GameObject cheatsTeleportPanel;

	// Token: 0x04000462 RID: 1122
	public TMP_Text cheatsInfo;

	// Token: 0x04000463 RID: 1123
	[Space]
	[SerializeField]
	private AudioSource cheatEnabledSound;

	// Token: 0x04000464 RID: 1124
	[SerializeField]
	private AudioSource cheatDisabledSound;

	// Token: 0x04000465 RID: 1125
	private int sequenceIndex;

	// Token: 0x04000466 RID: 1126
	public bool cheatsEnabled;

	// Token: 0x04000467 RID: 1127
	private bool noclip;

	// Token: 0x04000468 RID: 1128
	private bool flight;

	// Token: 0x04000469 RID: 1129
	private bool infiniteJumps;

	// Token: 0x0400046A RID: 1130
	private bool stayEnabled;
}
