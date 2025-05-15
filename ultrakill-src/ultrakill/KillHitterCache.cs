using System;
using System.Collections.Generic;

// Token: 0x020002B7 RID: 695
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class KillHitterCache : MonoSingleton<KillHitterCache>
{
	// Token: 0x06000F14 RID: 3860 RVA: 0x0006FC28 File Offset: 0x0006DE28
	public void OneDone(int enemyId)
	{
		if (this.eids.Count == 0 || !this.eids.Contains(enemyId))
		{
			this.currentScore++;
			this.eids.Add(enemyId);
			if (this.currentScore >= this.neededScore)
			{
				MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
			}
		}
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0006FC84 File Offset: 0x0006DE84
	public void RemoveId(int enemyId)
	{
		if (this.eids.Contains(enemyId))
		{
			this.currentScore--;
			this.eids.Remove(enemyId);
			if (this.currentScore < this.neededScore)
			{
				MonoSingleton<ChallengeManager>.Instance.challengeDone = false;
			}
		}
	}

	// Token: 0x0400143D RID: 5181
	public int neededScore;

	// Token: 0x0400143E RID: 5182
	public int currentScore;

	// Token: 0x0400143F RID: 5183
	private List<int> eids = new List<int>();

	// Token: 0x04001440 RID: 5184
	public bool ignoreRestarts;
}
