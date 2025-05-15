using System;
using UnityEngine;

// Token: 0x020002B4 RID: 692
public class Envirokills : MonoBehaviour
{
	// Token: 0x06000F0D RID: 3853 RVA: 0x0006FB6C File Offset: 0x0006DD6C
	private void Update()
	{
		if (this.ekt == enviroKillType.Glass && this.killAmount <= MonoSingleton<StatsManager>.Instance.maxGlassKills)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
		}
	}

	// Token: 0x04001438 RID: 5176
	public enviroKillType ekt;

	// Token: 0x04001439 RID: 5177
	public int killAmount;
}
