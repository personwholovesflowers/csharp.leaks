using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Token: 0x02000211 RID: 529
internal sealed class GamepadEnableWhileSelected : MonoBehaviour
{
	// Token: 0x06000B25 RID: 2853 RVA: 0x00050050 File Offset: 0x0004E250
	private void Update()
	{
		if (!(MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad))
		{
			return;
		}
		if (EventSystem.current.currentSelectedGameObject == base.gameObject)
		{
			if (this.GameObjects != null)
			{
				GameObject[] array = this.GameObjects;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(true);
				}
			}
			if (this.Disable != null)
			{
				foreach (GameObject gameObject in this.Disable)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(false);
					}
				}
				return;
			}
		}
		else if (this.DisableWhenDeselected && this.GameObjects != null)
		{
			GameObject[] array = this.GameObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}
	}

	// Token: 0x04000ECB RID: 3787
	public GameObject[] GameObjects;

	// Token: 0x04000ECC RID: 3788
	public GameObject[] Disable;

	// Token: 0x04000ECD RID: 3789
	public bool DisableWhenDeselected;
}
