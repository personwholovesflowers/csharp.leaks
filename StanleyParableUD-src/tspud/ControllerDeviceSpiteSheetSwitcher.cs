using System;
using TMPro;
using UnityEngine;

// Token: 0x020000A3 RID: 163
[RequireComponent(typeof(TMP_Text))]
public class ControllerDeviceSpiteSheetSwitcher : MonoBehaviour
{
	// Token: 0x060003EA RID: 1002 RVA: 0x00018C08 File Offset: 0x00016E08
	private void Start()
	{
		Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged += this.OnInputDeviceTypeChanged;
		this.OnInputDeviceTypeChanged(Singleton<GameMaster>.Instance.InputDeviceType);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x00018C30 File Offset: 0x00016E30
	private void OnDestroy()
	{
		if (Singleton<GameMaster>.Instance != null)
		{
			Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged -= this.OnInputDeviceTypeChanged;
		}
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x00018C55 File Offset: 0x00016E55
	[ContextMenu("Set to XBOX")]
	private void SetToXBOX()
	{
		this.OnInputDeviceTypeChanged(GameMaster.InputDevice.GamepadXBOXOneOrGeneric);
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00018C5E File Offset: 0x00016E5E
	private void OnInputDeviceTypeChanged(GameMaster.InputDevice inputDevice)
	{
		base.GetComponent<TMP_Text>().spriteAsset = PlatformSettings.Instance.GetSpriteSheetForInputDevice(inputDevice);
		this.lastInputDevice = inputDevice;
	}

	// Token: 0x040003E6 RID: 998
	public GameMaster.InputDevice lastInputDevice;
}
