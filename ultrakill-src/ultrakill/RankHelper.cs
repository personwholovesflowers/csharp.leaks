using System;
using UnityEngine;

// Token: 0x02000380 RID: 896
public static class RankHelper
{
	// Token: 0x060014B1 RID: 5297 RVA: 0x000A7524 File Offset: 0x000A5724
	public static string GetRankLetter(int rank)
	{
		if (rank < 0)
		{
			return "";
		}
		if (rank == 12)
		{
			return "P";
		}
		switch (rank)
		{
		case 1:
			return "C";
		case 2:
			return "B";
		case 3:
			return "A";
		case 4:
		case 5:
		case 6:
			return "S";
		default:
			return "D";
		}
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x000A7585 File Offset: 0x000A5785
	public static Color GetRankBackgroundColor(int rank)
	{
		if (rank != 12)
		{
			return Color.white;
		}
		return new Color(1f, 0.686f, 0f, 1f);
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x000A75AC File Offset: 0x000A57AC
	public static string GetRankForegroundColor(int rank)
	{
		if (rank < 0 || rank == 12)
		{
			return "#FFFFFF";
		}
		switch (rank)
		{
		case 1:
			return "#4CFF00";
		case 2:
			return "#FFD800";
		case 3:
			return "#FF6A00";
		case 4:
		case 5:
		case 6:
			return "#FF0000";
		default:
			return "#0094FF";
		}
	}
}
