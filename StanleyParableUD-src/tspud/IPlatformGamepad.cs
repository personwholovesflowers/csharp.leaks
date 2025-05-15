using System;
using InControl;

// Token: 0x0200015D RID: 349
public interface IPlatformGamepad
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000833 RID: 2099
	InputControlType ConfirmButton { get; }

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000834 RID: 2100
	InputControlType BackButton { get; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000835 RID: 2101
	InputControlType JumpButton { get; }

	// Token: 0x06000836 RID: 2102
	void PlayVibration(float strength);

	// Token: 0x06000837 RID: 2103
	void StopVibration();
}
