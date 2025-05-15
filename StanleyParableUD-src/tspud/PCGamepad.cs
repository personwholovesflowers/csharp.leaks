using System;
using InControl;

// Token: 0x02000155 RID: 341
public class PCGamepad : IPlatformGamepad
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060007FA RID: 2042 RVA: 0x0002790A File Offset: 0x00025B0A
	private InputDevice CurrentGamepad
	{
		get
		{
			return InputManager.ActiveDevice;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060007FB RID: 2043 RVA: 0x00027911 File Offset: 0x00025B11
	public InputControlType ConfirmButton { get; } = InputControlType.Action1;

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060007FC RID: 2044 RVA: 0x00027919 File Offset: 0x00025B19
	public InputControlType BackButton { get; } = InputControlType.Action2;

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060007FD RID: 2045 RVA: 0x00027921 File Offset: 0x00025B21
	public InputControlType JumpButton { get; } = InputControlType.Action1;

	// Token: 0x060007FE RID: 2046 RVA: 0x00027929 File Offset: 0x00025B29
	public void PlayVibration(float strength)
	{
		InputDevice currentGamepad = this.CurrentGamepad;
		if (currentGamepad == null)
		{
			return;
		}
		currentGamepad.Vibrate(strength, strength);
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x0002793D File Offset: 0x00025B3D
	public void StopVibration()
	{
		InputDevice currentGamepad = this.CurrentGamepad;
		if (currentGamepad == null)
		{
			return;
		}
		currentGamepad.Vibrate(0f, 0f);
	}
}
