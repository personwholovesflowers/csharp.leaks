using System;
using plog.unity.Models;

namespace GameConsole
{
	// Token: 0x020005B8 RID: 1464
	public static class PLogConfigHelper
	{
		// Token: 0x060020D6 RID: 8406 RVA: 0x00107C4D File Offset: 0x00105E4D
		public static UnityConfiguration GetCurrentConfiguration()
		{
			return UnityConfiguration.RuntimeDefault;
		}
	}
}
