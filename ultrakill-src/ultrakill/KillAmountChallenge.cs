using System;
using UnityEngine;

// Token: 0x020002B6 RID: 694
public class KillAmountChallenge : MonoBehaviour
{
	// Token: 0x06000F12 RID: 3858 RVA: 0x0006FBFB File Offset: 0x0006DDFB
	private void Update()
	{
		if (MonoSingleton<StatsManager>.Instance.kills == this.kills)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
			return;
		}
		MonoSingleton<ChallengeManager>.Instance.challengeDone = false;
	}

	// Token: 0x0400143C RID: 5180
	public int kills;
}
