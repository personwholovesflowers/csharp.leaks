using System;
using UnityEngine;

// Token: 0x020002BD RID: 701
public class TimeChallenge : MonoBehaviour
{
	// Token: 0x06000F23 RID: 3875 RVA: 0x0006FE80 File Offset: 0x0006E080
	private void Update()
	{
		if (MonoSingleton<StatsManager>.Instance.seconds >= this.time && !this.reachedGoal)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeFailed = true;
			MonoSingleton<ChallengeManager>.Instance.challengeFailedPermanently = true;
			base.enabled = false;
			return;
		}
		MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x0006FED0 File Offset: 0x0006E0D0
	public void ReachedGoal()
	{
		this.reachedGoal = true;
	}

	// Token: 0x0400144A RID: 5194
	public float time;

	// Token: 0x0400144B RID: 5195
	public bool reachedGoal;
}
