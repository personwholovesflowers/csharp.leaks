using System;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class WaitForGameSeconds : CustomYieldInstruction
{
	// Token: 0x06000459 RID: 1113 RVA: 0x0001A03B File Offset: 0x0001823B
	public WaitForGameSeconds(float duration)
	{
		this.endTime = Singleton<GameMaster>.Instance.GameTime + duration;
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x0600045A RID: 1114 RVA: 0x0001A059 File Offset: 0x00018259
	public override bool keepWaiting
	{
		get
		{
			return Singleton<GameMaster>.Instance.GameTime < this.endTime;
		}
	}

	// Token: 0x04000440 RID: 1088
	private fint32 endTime;
}
