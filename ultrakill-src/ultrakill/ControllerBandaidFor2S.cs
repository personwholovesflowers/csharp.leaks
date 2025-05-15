using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020000DB RID: 219
public class ControllerBandaidFor2S : MonoBehaviour
{
	// Token: 0x06000449 RID: 1097 RVA: 0x0001DC08 File Offset: 0x0001BE08
	private void Update()
	{
		if (Gamepad.current == null || MonoSingleton<OptionsManager>.Instance == null || MonoSingleton<OptionsManager>.Instance.paused)
		{
			return;
		}
		if (this.a && Gamepad.current.aButton.wasPressedThisFrame)
		{
			this.Activate();
			return;
		}
		if (this.b && Gamepad.current.bButton.wasPressedThisFrame)
		{
			this.Activate();
			return;
		}
		if (this.y && Gamepad.current.yButton.wasPressedThisFrame)
		{
			this.Activate();
			return;
		}
		if (this.x && Gamepad.current.xButton.wasPressedThisFrame)
		{
			this.Activate();
		}
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0001DCB8 File Offset: 0x0001BEB8
	private void Activate()
	{
		Button componentInParent = base.GetComponentInParent<Button>();
		if (componentInParent && componentInParent.interactable && componentInParent.gameObject.activeInHierarchy)
		{
			componentInParent.onClick.Invoke();
		}
	}

	// Token: 0x040005E4 RID: 1508
	public bool a;

	// Token: 0x040005E5 RID: 1509
	public bool b;

	// Token: 0x040005E6 RID: 1510
	public bool x;

	// Token: 0x040005E7 RID: 1511
	public bool y;
}
