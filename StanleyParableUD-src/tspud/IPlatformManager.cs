using System;

// Token: 0x0200015E RID: 350
public interface IPlatformManager
{
	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000838 RID: 2104
	IPlatformAchievements Achievements { get; }

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000839 RID: 2105
	IPlatformGamepad Gamepad { get; }

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x0600083A RID: 2106
	IPlatformRichPresence RichPresence { get; }

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600083B RID: 2107
	IPlatformPlayerPrefs PlayerPrefs { get; }

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600083C RID: 2108
	string SystemLanguage { get; }

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x0600083D RID: 2109
	bool UseLowerFPSVideos { get; }

	// Token: 0x0600083E RID: 2110
	void Init();

	// Token: 0x0600083F RID: 2111
	void PlatformManagerUpdate();
}
