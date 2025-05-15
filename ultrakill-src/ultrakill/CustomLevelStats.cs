using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class CustomLevelStats : MonoBehaviour
{
	// Token: 0x060001A4 RID: 420 RVA: 0x000084A8 File Offset: 0x000066A8
	public void LoadStats(string uuid)
	{
		if (uuid == null || string.IsNullOrEmpty(uuid))
		{
			this.statsText.text = "Unsupported";
			this.secretsText.text = string.Empty;
			this.mainRankIcon.gameObject.SetActive(false);
			return;
		}
		this.statsText.text = "No stats yet";
		Debug.Log("Loading stats for " + uuid);
		RankData customRankData = GameProgressSaver.GetCustomRankData(uuid);
		if (customRankData == null)
		{
			this.mainRankIcon.SetEmpty();
			this.secretsText.text = string.Empty;
			return;
		}
		int[] ranks = customRankData.ranks;
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		int num = ranks[@int];
		this.mainRankIcon.SetRank(num);
		int secretsAmount = customRankData.secretsAmount;
		int num2 = customRankData.secretsFound.Count((bool x) => x);
		this.secretsText.text = string.Format("Secrets\n{0}/{1}", num2, secretsAmount);
		RankScoreData[] stats = customRankData.stats;
		if (stats != null && stats.Length >= @int && stats[@int] != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			RankScoreData rankScoreData = stats[@int];
			stringBuilder.AppendLine("Time: <color=orange>" + TimeHelper.ConvertSecondsToString(rankScoreData.time) + "</color>");
			stringBuilder.AppendLine(string.Format("Kills: <color={0}>{1}</color>", "orange", rankScoreData.kills));
			stringBuilder.AppendLine(string.Format("Style: <color={0}>{1}</color>", "orange", rankScoreData.style));
			this.statsText.text = stringBuilder.ToString();
		}
	}

	// Token: 0x040001B1 RID: 433
	[SerializeField]
	private RankIcon mainRankIcon;

	// Token: 0x040001B2 RID: 434
	[SerializeField]
	private TMP_Text secretsText;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	private TMP_Text statsText;

	// Token: 0x040001B4 RID: 436
	private const string AccentColor = "orange";
}
