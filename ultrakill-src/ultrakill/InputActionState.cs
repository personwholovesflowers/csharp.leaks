using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200026D RID: 621
public class InputActionState
{
	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x00067056 File Offset: 0x00065256
	public InputAction Action { get; }

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0006705E File Offset: 0x0006525E
	// (set) Token: 0x06000DA2 RID: 3490 RVA: 0x00067066 File Offset: 0x00065266
	public float PerformedTime { get; private set; }

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0006706F File Offset: 0x0006526F
	// (set) Token: 0x06000DA4 RID: 3492 RVA: 0x00067077 File Offset: 0x00065277
	public int PerformedFrame { get; private set; }

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x00067080 File Offset: 0x00065280
	// (set) Token: 0x06000DA6 RID: 3494 RVA: 0x00067088 File Offset: 0x00065288
	public int CanceledFrame { get; private set; }

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x00067091 File Offset: 0x00065291
	// (set) Token: 0x06000DA8 RID: 3496 RVA: 0x00067099 File Offset: 0x00065299
	public bool IsPressed { get; private set; }

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x000670A2 File Offset: 0x000652A2
	// (set) Token: 0x06000DAA RID: 3498 RVA: 0x000670AA File Offset: 0x000652AA
	public string LastUsedBinding { get; private set; }

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000DAB RID: 3499 RVA: 0x000670B3 File Offset: 0x000652B3
	public float HoldTime
	{
		get
		{
			if (!this.IsPressed && !this.WasCanceledThisFrame)
			{
				return 0f;
			}
			return Time.time - this.PerformedTime;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000DAC RID: 3500 RVA: 0x000670D7 File Offset: 0x000652D7
	public bool WasPerformedThisFrame
	{
		get
		{
			return this.PerformedFrame == Time.frameCount;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000DAD RID: 3501 RVA: 0x000670E6 File Offset: 0x000652E6
	public bool WasCanceledThisFrame
	{
		get
		{
			return this.CanceledFrame == Time.frameCount;
		}
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x000670F5 File Offset: 0x000652F5
	public InputActionState(InputAction action)
	{
		this.Action = action;
		action.started += this.OnTriggered;
		action.canceled += this.OnTriggered;
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00067128 File Offset: 0x00065328
	private void OnTriggered(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.IsPressed = true;
			this.PerformedFrame = Time.frameCount;
			this.PerformedTime = Time.time;
			this.LastUsedBinding = context.control.path;
			return;
		}
		if (context.canceled)
		{
			this.IsPressed = false;
			this.CanceledFrame = Time.frameCount;
		}
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x00067189 File Offset: 0x00065389
	public TValue ReadValue<TValue>() where TValue : struct
	{
		return this.Action.ReadValue<TValue>();
	}
}
