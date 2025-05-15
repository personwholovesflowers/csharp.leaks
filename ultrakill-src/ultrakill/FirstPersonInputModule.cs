using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001D4 RID: 468
public class FirstPersonInputModule : StandaloneInputModule
{
	// Token: 0x0600098F RID: 2447 RVA: 0x0004277C File Offset: 0x0004097C
	protected override PointerInputModule.MouseState GetMousePointerEventData(int id)
	{
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		PointerInputModule.MouseState mousePointerEventData = base.GetMousePointerEventData(id);
		Cursor.lockState = lockState;
		return mousePointerEventData;
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x000427A2 File Offset: 0x000409A2
	protected override void ProcessMove(PointerEventData pointerEvent)
	{
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		base.ProcessMove(pointerEvent);
		Cursor.lockState = lockState;
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x000427BB File Offset: 0x000409BB
	protected override void ProcessDrag(PointerEventData pointerEvent)
	{
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		base.ProcessDrag(pointerEvent);
		Cursor.lockState = lockState;
	}
}
