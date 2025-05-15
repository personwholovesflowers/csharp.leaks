using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000157 RID: 343
public static class UnlockConditionExtensions
{
	// Token: 0x060006BC RID: 1724 RVA: 0x0002D14A File Offset: 0x0002B34A
	public static bool AllMet(this List<UnlockCondition> list)
	{
		return list.Aggregate(true, (bool acc, UnlockCondition cond) => acc && cond.conditionMet);
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0002D174 File Offset: 0x0002B374
	public static string DescribeAll(this List<UnlockCondition> list)
	{
		if (list.Count == 0)
		{
			return "";
		}
		if (list.Count == 1)
		{
			return list.First<UnlockCondition>().description;
		}
		return list.Skip(1).Aggregate(list.First<UnlockCondition>().description, (string desc, UnlockCondition cond) => desc + ", " + cond.description);
	}
}
