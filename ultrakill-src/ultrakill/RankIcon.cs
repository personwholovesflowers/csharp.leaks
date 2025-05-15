using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000381 RID: 897
public class RankIcon : MonoBehaviour
{
	// Token: 0x060014B4 RID: 5300 RVA: 0x000A7607 File Offset: 0x000A5807
	private void Start()
	{
		if (this.useDefaultRank)
		{
			this.SetRank(this.defaultRank);
		}
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x000A7620 File Offset: 0x000A5820
	public void SetRank(int rank)
	{
		base.gameObject.SetActive(true);
		this.mainRankLetter.text = string.Concat(new string[]
		{
			"<color=",
			RankHelper.GetRankForegroundColor(rank),
			">",
			RankHelper.GetRankLetter(rank),
			"</color>"
		});
		this.mainRankBackground.fillCenter = rank == 12;
		this.mainRankBackground.color = RankHelper.GetRankBackgroundColor(rank);
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x000A769A File Offset: 0x000A589A
	public void SetEmpty()
	{
		base.gameObject.SetActive(true);
		this.mainRankLetter.text = string.Empty;
		this.mainRankBackground.fillCenter = false;
		this.mainRankBackground.color = Color.white;
	}

	// Token: 0x04001C86 RID: 7302
	[SerializeField]
	private bool useDefaultRank;

	// Token: 0x04001C87 RID: 7303
	[SerializeField]
	[Range(0f, 12f)]
	private int defaultRank;

	// Token: 0x04001C88 RID: 7304
	[SerializeField]
	private TMP_Text mainRankLetter;

	// Token: 0x04001C89 RID: 7305
	[SerializeField]
	private Image mainRankBackground;
}
