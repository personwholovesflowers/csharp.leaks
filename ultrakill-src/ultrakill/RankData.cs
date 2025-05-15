using System;

// Token: 0x0200037E RID: 894
[Serializable]
public class RankData
{
	// Token: 0x060014AF RID: 5295 RVA: 0x000A727C File Offset: 0x000A547C
	public RankData(StatsManager sman)
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		this.levelNumber = sman.levelNumber;
		RankData rank = GameProgressSaver.GetRank(true, -1);
		if (rank != null)
		{
			this.ranks = rank.ranks;
			if (rank.majorAssists != null)
			{
				this.majorAssists = rank.majorAssists;
			}
			else
			{
				this.majorAssists = new bool[6];
			}
			if (rank.stats != null)
			{
				this.stats = rank.stats;
			}
			else
			{
				this.stats = new RankScoreData[6];
			}
			if ((sman.rankScore >= rank.ranks[@int] && (rank.majorAssists == null || (!sman.majorUsed && rank.majorAssists[@int]))) || sman.rankScore > rank.ranks[@int] || rank.levelNumber != this.levelNumber)
			{
				this.majorAssists[@int] = sman.majorUsed;
				this.ranks[@int] = sman.rankScore;
				if (this.stats[@int] == null)
				{
					this.stats[@int] = new RankScoreData();
				}
				this.stats[@int].kills = sman.kills;
				this.stats[@int].style = sman.stylePoints;
				this.stats[@int].time = sman.seconds;
			}
			this.secretsAmount = sman.secretObjects.Length;
			this.secretsFound = new bool[this.secretsAmount];
			int num = 0;
			while (num < this.secretsAmount && num < rank.secretsFound.Length)
			{
				if (sman.secretObjects[num] == null || rank.secretsFound[num])
				{
					this.secretsFound[num] = true;
				}
				num++;
			}
			this.challenge = rank.challenge;
			return;
		}
		this.ranks = new int[6];
		this.stats = new RankScoreData[6];
		if (this.stats[@int] == null)
		{
			this.stats[@int] = new RankScoreData();
		}
		this.majorAssists = new bool[6];
		for (int i = 0; i < this.ranks.Length; i++)
		{
			this.ranks[i] = -1;
		}
		this.ranks[@int] = sman.rankScore;
		this.majorAssists[@int] = sman.majorUsed;
		this.stats[@int].kills = sman.kills;
		this.stats[@int].style = sman.stylePoints;
		this.stats[@int].time = sman.seconds;
		this.secretsAmount = sman.secretObjects.Length;
		this.secretsFound = new bool[this.secretsAmount];
		for (int j = 0; j < this.secretsAmount; j++)
		{
			if (sman.secretObjects[j] == null)
			{
				this.secretsFound[j] = true;
			}
		}
	}

	// Token: 0x04001C7C RID: 7292
	public int[] ranks;

	// Token: 0x04001C7D RID: 7293
	public int secretsAmount;

	// Token: 0x04001C7E RID: 7294
	public bool[] secretsFound;

	// Token: 0x04001C7F RID: 7295
	public bool challenge;

	// Token: 0x04001C80 RID: 7296
	public int levelNumber;

	// Token: 0x04001C81 RID: 7297
	public bool[] majorAssists;

	// Token: 0x04001C82 RID: 7298
	public RankScoreData[] stats;
}
