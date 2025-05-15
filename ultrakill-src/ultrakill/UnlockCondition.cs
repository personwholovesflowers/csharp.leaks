using System;
using System.Linq;

// Token: 0x02000159 RID: 345
[Serializable]
public abstract class UnlockCondition
{
	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060006C2 RID: 1730
	public abstract bool conditionMet { get; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060006C3 RID: 1731
	public abstract string description { get; }

	// Token: 0x060006C4 RID: 1732 RVA: 0x00004ADB File Offset: 0x00002CDB
	public UnlockCondition()
	{
	}

	// Token: 0x0200015A RID: 346
	[Serializable]
	public class HasCompletedLevelChallenge : UnlockCondition
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0002D206 File Offset: 0x0002B406
		public override bool conditionMet
		{
			get
			{
				RankData rank = GameProgressSaver.GetRank(this.levelIndex, true);
				return rank != null && rank.challenge;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0002D21F File Offset: 0x0002B41F
		public override string description
		{
			get
			{
				return "COMPLETE CHALLENGE FOR " + GetMissionName.GetMissionNumberOnly(this.levelIndex);
			}
		}

		// Token: 0x040008DD RID: 2269
		public int levelIndex = 1;
	}

	// Token: 0x0200015B RID: 347
	[Serializable]
	public class HasSeenEnemy : UnlockCondition
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0002D245 File Offset: 0x0002B445
		public override bool conditionMet
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x0002D248 File Offset: 0x0002B448
		public override string description
		{
			get
			{
				return "ENCOUNTER AN UNKNOWN FOE";
			}
		}

		// Token: 0x040008DE RID: 2270
		public EnemyType enemy;
	}

	// Token: 0x0200015C RID: 348
	[Serializable]
	public class HasReachedLevel : UnlockCondition
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x0002D257 File Offset: 0x0002B457
		public override bool conditionMet
		{
			get
			{
				return GameProgressSaver.GetRank(this.levelIndex, true) != null;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0002D268 File Offset: 0x0002B468
		public override string description
		{
			get
			{
				return "REACH " + GetMissionName.GetMissionNumberOnly(this.levelIndex);
			}
		}

		// Token: 0x040008DF RID: 2271
		public int levelIndex = 1;
	}

	// Token: 0x0200015D RID: 349
	[Serializable]
	public class HasCompletedLevel : UnlockCondition
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x0002D290 File Offset: 0x0002B490
		public override bool conditionMet
		{
			get
			{
				int num = this.levelIndex;
				bool flag;
				if (num >= 100)
				{
					if (num >= 666)
					{
						flag = GameProgressSaver.GetPrime(0, this.levelIndex - 665) > 0;
					}
					else
					{
						flag = GameProgressSaver.GetEncoreProgress(0) > this.levelIndex - 100;
					}
				}
				else
				{
					flag = GameProgressSaver.GetRank(this.levelIndex, true).ranks.Aggregate(false, (bool acc, int rank) => acc || rank > -1);
				}
				return flag;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0002D317 File Offset: 0x0002B517
		public override string description
		{
			get
			{
				return "COMPLETE " + GetMissionName.GetMissionNumberOnly(this.levelIndex);
			}
		}

		// Token: 0x040008E0 RID: 2272
		public int levelIndex = 1;
	}

	// Token: 0x0200015F RID: 351
	[Serializable]
	public class HasCompletedSecretLevel : UnlockCondition
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0002D354 File Offset: 0x0002B554
		public override bool conditionMet
		{
			get
			{
				return GameProgressSaver.GetSecretMission(this.secretLevelIndex) == 2;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0002D364 File Offset: 0x0002B564
		public override string description
		{
			get
			{
				return "COMPLETE " + this.secretLevelIndex.ToString() + "-S";
			}
		}

		// Token: 0x040008E3 RID: 2275
		public int secretLevelIndex = 1;
	}

	// Token: 0x02000160 RID: 352
	[Serializable]
	public class HasObtainedWeapon : UnlockCondition
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0002D38F File Offset: 0x0002B58F
		public override bool conditionMet
		{
			get
			{
				return GameProgressSaver.CheckGear(this.gearName) > 0;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0002D39F File Offset: 0x0002B59F
		public override string description
		{
			get
			{
				return "UNLOCK A WEAPON YET UNDISCOVERED";
			}
		}

		// Token: 0x040008E4 RID: 2276
		public string gearName;
	}
}
