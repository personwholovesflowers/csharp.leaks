using System;
using UnityEngine;

// Token: 0x02000500 RID: 1280
public static class SystemInfoEx
{
	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06001D3D RID: 7485 RVA: 0x000F500F File Offset: 0x000F320F
	// (set) Token: 0x06001D3E RID: 7486 RVA: 0x000F5016 File Offset: 0x000F3216
	public static bool supportsComputeShaders { get; private set; }

	// Token: 0x06001D3F RID: 7487 RVA: 0x000F501E File Offset: 0x000F321E
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void Initialize()
	{
		SystemInfoEx.supportsComputeShaders = SystemInfo.supportsComputeShaders;
	}
}
