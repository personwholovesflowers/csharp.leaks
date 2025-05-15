using System;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class PCManager : IPlatformManager
{
	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000801 RID: 2049 RVA: 0x00027979 File Offset: 0x00025B79
	// (set) Token: 0x06000802 RID: 2050 RVA: 0x00027981 File Offset: 0x00025B81
	public string SystemLanguage { get; private set; }

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06000803 RID: 2051 RVA: 0x0002798A File Offset: 0x00025B8A
	// (set) Token: 0x06000804 RID: 2052 RVA: 0x00027992 File Offset: 0x00025B92
	public bool UseLowerFPSVideos { get; private set; }

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000805 RID: 2053 RVA: 0x0002799B File Offset: 0x00025B9B
	// (set) Token: 0x06000806 RID: 2054 RVA: 0x000279A3 File Offset: 0x00025BA3
	public IPlatformAchievements Achievements { get; private set; }

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000807 RID: 2055 RVA: 0x000279AC File Offset: 0x00025BAC
	// (set) Token: 0x06000808 RID: 2056 RVA: 0x000279B4 File Offset: 0x00025BB4
	public IPlatformGamepad Gamepad { get; private set; }

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000809 RID: 2057 RVA: 0x000279BD File Offset: 0x00025BBD
	// (set) Token: 0x0600080A RID: 2058 RVA: 0x000279C5 File Offset: 0x00025BC5
	public IPlatformPlayerPrefs PlayerPrefs { get; private set; }

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x0600080B RID: 2059 RVA: 0x000279CE File Offset: 0x00025BCE
	// (set) Token: 0x0600080C RID: 2060 RVA: 0x000279D6 File Offset: 0x00025BD6
	public IPlatformRichPresence RichPresence { get; private set; }

	// Token: 0x0600080D RID: 2061 RVA: 0x000279E0 File Offset: 0x00025BE0
	public void Init()
	{
		if (!this.isInitialized)
		{
			this.Achievements = new PCAchievements();
			this.Gamepad = new PCGamepad();
			this.PlayerPrefs = new PCFileBasePlayerPrefs();
			this.RichPresence = new PCRichPresence();
			this.SystemLanguage = Application.systemLanguage.ToString();
			this.UseLowerFPSVideos = false;
			this.isInitialized = true;
		}
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00005444 File Offset: 0x00003644
	public void PlatformManagerUpdate()
	{
	}

	// Token: 0x0400081B RID: 2075
	private bool isInitialized;
}
