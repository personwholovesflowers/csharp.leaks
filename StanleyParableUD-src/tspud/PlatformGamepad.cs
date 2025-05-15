using System;
using InControl;

// Token: 0x0200014E RID: 334
public static class PlatformGamepad
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060007CA RID: 1994 RVA: 0x000273F4 File Offset: 0x000255F4
	public static InputControlType ConfirmButton
	{
		get
		{
			return PlatformGamepad.platformGamepad.ConfirmButton;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060007CB RID: 1995 RVA: 0x00027400 File Offset: 0x00025600
	public static InputControlType BackButton
	{
		get
		{
			return PlatformGamepad.platformGamepad.BackButton;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060007CC RID: 1996 RVA: 0x0002740C File Offset: 0x0002560C
	public static InputControlType JumpButton
	{
		get
		{
			return PlatformGamepad.platformGamepad.JumpButton;
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00027418 File Offset: 0x00025618
	public static void InitPlatformGamepad(IPlatformGamepad gamepad)
	{
		PlatformGamepad.platformGamepad = gamepad;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00027420 File Offset: 0x00025620
	public static void PlayVibration(float strength)
	{
		IPlatformGamepad platformGamepad = PlatformGamepad.platformGamepad;
		if (platformGamepad == null)
		{
			return;
		}
		platformGamepad.PlayVibration(strength);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00027432 File Offset: 0x00025632
	public static void StopVibration()
	{
		IPlatformGamepad platformGamepad = PlatformGamepad.platformGamepad;
		if (platformGamepad == null)
		{
			return;
		}
		platformGamepad.StopVibration();
	}

	// Token: 0x040007E7 RID: 2023
	private static IPlatformGamepad platformGamepad;
}
