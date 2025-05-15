using System;

// Token: 0x02000151 RID: 337
public static class PlatformRichPresence
{
	// Token: 0x060007E5 RID: 2021 RVA: 0x000276E3 File Offset: 0x000258E3
	public static void InitPlatformRichPresence(IPlatformRichPresence presences)
	{
		PlatformRichPresence.platformRP = presences;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x000276EB File Offset: 0x000258EB
	public static void SetPresence(PresenceID presence)
	{
		IPlatformRichPresence platformRichPresence = PlatformRichPresence.platformRP;
		if (platformRichPresence == null)
		{
			return;
		}
		platformRichPresence.SetPresence(presence);
	}

	// Token: 0x040007EE RID: 2030
	private static IPlatformRichPresence platformRP;
}
