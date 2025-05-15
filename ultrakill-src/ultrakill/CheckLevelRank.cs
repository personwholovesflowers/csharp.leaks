using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class CheckLevelRank : MonoBehaviour
{
	// Token: 0x060003B9 RID: 953 RVA: 0x00017115 File Offset: 0x00015315
	private void Start()
	{
		if (CheckLevelRank.CheckLevelStatus())
		{
			UltrakillEvent ultrakillEvent = this.onSuccess;
			if (ultrakillEvent == null)
			{
				return;
			}
			ultrakillEvent.Invoke("");
			return;
		}
		else
		{
			UltrakillEvent ultrakillEvent2 = this.onFail;
			if (ultrakillEvent2 == null)
			{
				return;
			}
			ultrakillEvent2.Invoke("");
			return;
		}
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0001714C File Offset: 0x0001534C
	public static bool CheckLevelStatus()
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return true;
		}
		if (MonoSingleton<StatsManager>.Instance.levelNumber == 0)
		{
			return true;
		}
		RankData rank = GameProgressSaver.GetRank(true, -1);
		if (rank != null && rank.levelNumber == MonoSingleton<StatsManager>.Instance.levelNumber && rank.ranks != null && rank.ranks.Length != 0)
		{
			int[] ranks = rank.ranks;
			for (int i = 0; i < ranks.Length; i++)
			{
				if (ranks[i] != -1)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x0400048F RID: 1167
	public UltrakillEvent onSuccess;

	// Token: 0x04000490 RID: 1168
	public UltrakillEvent onFail;
}
