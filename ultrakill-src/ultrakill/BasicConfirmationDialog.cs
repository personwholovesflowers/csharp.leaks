using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Token: 0x0200006F RID: 111
public class BasicConfirmationDialog : MonoBehaviour
{
	// Token: 0x06000211 RID: 529 RVA: 0x0000AC2E File Offset: 0x00008E2E
	public void ShowDialog()
	{
		base.gameObject.SetActive(true);
		this.blocker.SetActive(true);
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000AC48 File Offset: 0x00008E48
	private void Update()
	{
		if (MonoSingleton<InputManager>.Instance.InputSource.Pause.WasPerformedThisFrame || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
		{
			this.Cancel();
		}
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000AC7E File Offset: 0x00008E7E
	public void Confirm()
	{
		base.gameObject.SetActive(false);
		this.blocker.SetActive(false);
		this.onConfirm.Invoke();
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000ACA3 File Offset: 0x00008EA3
	public void Cancel()
	{
		base.gameObject.SetActive(false);
		this.blocker.SetActive(false);
	}

	// Token: 0x04000246 RID: 582
	[SerializeField]
	private GameObject blocker;

	// Token: 0x04000247 RID: 583
	[SerializeField]
	private UnityEvent onConfirm;
}
