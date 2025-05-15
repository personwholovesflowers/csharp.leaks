using System;
using System.Diagnostics;

// Token: 0x02000164 RID: 356
public static class PlatformLog
{
	// Token: 0x06000855 RID: 2133 RVA: 0x00005444 File Offset: 0x00003644
	[Conditional("PLATFORMDEBUG")]
	public static void Log(string msg)
	{
	}
}
