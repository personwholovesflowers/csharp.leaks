using System;
using System.Text;
using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class FishLeaderboard : MonoBehaviour
{
	// Token: 0x060009FB RID: 2555 RVA: 0x00045349 File Offset: 0x00043549
	private void OnEnable()
	{
		this.Fetch();
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00045354 File Offset: 0x00043554
	private async void Fetch()
	{
		LeaderboardEntry[] array = await MonoSingleton<LeaderboardController>.Instance.GetFishScores(LeaderboardType.Global);
		StringBuilder strBlrd = new StringBuilder();
		if (array != null)
		{
			strBlrd.AppendLine("<b>GLOBAL</b>");
			int num = 1;
			foreach (LeaderboardEntry leaderboardEntry in array)
			{
				Friend friend = leaderboardEntry.User;
				string name = friend.Name;
				friend = leaderboardEntry.User;
				if (friend.IsMe)
				{
					strBlrd.Append("<color=orange>");
				}
				strBlrd.Append("<noparse>");
				string text = string.Format("[{0}] {1} - {2}", num, leaderboardEntry.Score, name);
				if (text.Length > 25)
				{
					text = text.Substring(0, 25);
				}
				strBlrd.AppendLine(text);
				strBlrd.Append("</noparse>");
				friend = leaderboardEntry.User;
				if (friend.IsMe)
				{
					strBlrd.Append("</color>");
				}
				num++;
			}
		}
		else
		{
			strBlrd.Append("Error fetching leaderboard data.");
		}
		this.globalText.text = strBlrd.ToString();
		LeaderboardEntry[] array3 = await MonoSingleton<LeaderboardController>.Instance.GetFishScores(LeaderboardType.Friends);
		strBlrd.Clear();
		if (array3 != null)
		{
			strBlrd.AppendLine("<b>FRIENDS</b>");
			foreach (LeaderboardEntry leaderboardEntry2 in array3)
			{
				Friend friend = leaderboardEntry2.User;
				string name2 = friend.Name;
				friend = leaderboardEntry2.User;
				if (friend.IsMe)
				{
					strBlrd.Append("<color=orange>");
				}
				strBlrd.Append("<noparse>");
				string text2 = string.Format("[{0}] {1} - {2}", leaderboardEntry2.GlobalRank, leaderboardEntry2.Score, name2);
				if (text2.Length > 25)
				{
					text2 = text2.Substring(0, 25);
				}
				strBlrd.AppendLine(text2);
				strBlrd.Append("</noparse>");
				friend = leaderboardEntry2.User;
				if (friend.IsMe)
				{
					strBlrd.Append("</color>");
				}
			}
		}
		else
		{
			strBlrd.Append("Error fetching leaderboard data.");
		}
		this.friendsText.text = strBlrd.ToString();
	}

	// Token: 0x04000D0A RID: 3338
	[SerializeField]
	private TMP_Text globalText;

	// Token: 0x04000D0B RID: 3339
	[SerializeField]
	private TMP_Text friendsText;
}
