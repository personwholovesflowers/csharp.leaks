using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000345 RID: 837
public class PlayerInputHooks : MonoBehaviour
{
	// Token: 0x06001346 RID: 4934 RVA: 0x0009B5DC File Offset: 0x000997DC
	private void Update()
	{
		if (MonoSingleton<OptionsManager>.Instance && MonoSingleton<OptionsManager>.Instance.paused)
		{
			return;
		}
		if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame)
		{
			this.onFire1Pressed.Invoke();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasCanceledThisFrame)
		{
			this.onFire1Released.Invoke();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame)
		{
			this.onFire2Pressed.Invoke();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasCanceledThisFrame)
		{
			this.onFire2Released.Invoke();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame)
		{
			this.onSlideInputStart.Invoke();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame)
		{
			this.onSlideInputEnd.Invoke();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame)
		{
			this.onJumpPressed.Invoke();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Jump.WasCanceledThisFrame)
		{
			this.onJumpReleased.Invoke();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame)
		{
			this.onDashPressed.Invoke();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasCanceledThisFrame)
		{
			this.onDashReleased.Invoke();
		}
	}

	// Token: 0x04001A9C RID: 6812
	[Header("Fire1")]
	[SerializeField]
	private UnityEvent onFire1Pressed;

	// Token: 0x04001A9D RID: 6813
	[SerializeField]
	private UnityEvent onFire1Released;

	// Token: 0x04001A9E RID: 6814
	[Space]
	[Header("Fire2")]
	[SerializeField]
	private UnityEvent onFire2Pressed;

	// Token: 0x04001A9F RID: 6815
	[SerializeField]
	private UnityEvent onFire2Released;

	// Token: 0x04001AA0 RID: 6816
	[Space]
	[Header("Slide")]
	[SerializeField]
	private UnityEvent onSlideInputStart;

	// Token: 0x04001AA1 RID: 6817
	[SerializeField]
	private UnityEvent onSlideInputEnd;

	// Token: 0x04001AA2 RID: 6818
	[Space]
	[Header("Jump")]
	[SerializeField]
	private UnityEvent onJumpPressed;

	// Token: 0x04001AA3 RID: 6819
	[SerializeField]
	private UnityEvent onJumpReleased;

	// Token: 0x04001AA4 RID: 6820
	[Header("Dash")]
	[SerializeField]
	private UnityEvent onDashPressed;

	// Token: 0x04001AA5 RID: 6821
	[SerializeField]
	private UnityEvent onDashReleased;
}
