using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200018B RID: 395
public class SettingsCharacterInputController : MonoBehaviour
{
	// Token: 0x06000925 RID: 2341 RVA: 0x0002B36F File Offset: 0x0002956F
	private void Start()
	{
		Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged += this.Instance_OnInputDeviceTypeChanged;
		this.Instance_OnInputDeviceTypeChanged(Singleton<GameMaster>.Instance.InputDeviceType, true);
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0002B398 File Offset: 0x00029598
	private void Instance_OnInputDeviceTypeChanged(GameMaster.InputDevice inputDevice)
	{
		this.Instance_OnInputDeviceTypeChanged(inputDevice, false);
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0002B3A2 File Offset: 0x000295A2
	private void Instance_OnInputDeviceTypeChanged(GameMaster.InputDevice inputDevice, bool ignoreMouseMove)
	{
		if (inputDevice == GameMaster.InputDevice.KeyboardAndMouse)
		{
			EventSystem.current.SetSelectedGameObject(null);
			GameMaster.CursorVisible = ignoreMouseMove || Singleton<GameMaster>.Instance.MouseMoved;
			GameMaster.CursorLockState = CursorLockMode.None;
			return;
		}
		GameMaster.CursorVisible = false;
		GameMaster.CursorLockState = CursorLockMode.Locked;
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0002B3DA File Offset: 0x000295DA
	private void Update()
	{
		if (this.forceMouseVisibleOnMove && Singleton<GameMaster>.Instance.MouseMoved)
		{
			GameMaster.CursorVisible = true;
			GameMaster.CursorLockState = CursorLockMode.None;
		}
		this.visible_DEBUG = GameMaster.CursorVisible;
		this.cursorLockState_DEBUG = GameMaster.CursorLockState;
	}

	// Token: 0x040008F8 RID: 2296
	public bool forceMouseVisibleOnMove;

	// Token: 0x040008F9 RID: 2297
	public bool visible_DEBUG;

	// Token: 0x040008FA RID: 2298
	public CursorLockMode cursorLockState_DEBUG;
}
