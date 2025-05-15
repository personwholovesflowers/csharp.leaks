using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002C0 RID: 704
public class LevelNameFinder : MonoBehaviour
{
	// Token: 0x06000F2F RID: 3887 RVA: 0x0007007C File Offset: 0x0006E27C
	private void OnEnable()
	{
		if (this.lookForPreviousMission || this.lookForLatestMission)
		{
			bool flag = false;
			if (this.lookForPreviousMission)
			{
				PreviousMissionSaver instance = MonoSingleton<PreviousMissionSaver>.Instance;
				if (instance != null)
				{
					flag = true;
					this.otherLevelNumber = instance.previousMission;
				}
			}
			if (!flag && this.lookForLatestMission)
			{
				this.otherLevelNumber = GameProgressSaver.GetProgress(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0));
			}
		}
		string text;
		if (this.otherLevelNumber != 0)
		{
			text = this.textBeforeName + (this.breakLine ? "\n" : "") + GetMissionName.GetMission(this.otherLevelNumber);
		}
		else
		{
			if (this.thisLevelNumber == 0)
			{
				this.thisLevelNumber = (MonoSingleton<StatsManager>.Instance ? MonoSingleton<StatsManager>.Instance.levelNumber : (-1));
			}
			text = this.textBeforeName + (this.breakLine ? "\n" : "") + GetMissionName.GetMission(this.thisLevelNumber);
		}
		if (!this.txt2)
		{
			this.txt2 = base.GetComponent<TMP_Text>();
		}
		if (this.txt2)
		{
			this.txt2.text = text;
			return;
		}
		if (!this.txt)
		{
			this.txt = base.GetComponent<Text>();
		}
		if (this.txt)
		{
			this.txt.text = text;
		}
	}

	// Token: 0x04001456 RID: 5206
	public string textBeforeName;

	// Token: 0x04001457 RID: 5207
	public bool breakLine;

	// Token: 0x04001458 RID: 5208
	private int thisLevelNumber;

	// Token: 0x04001459 RID: 5209
	public int otherLevelNumber;

	// Token: 0x0400145A RID: 5210
	private Text txt;

	// Token: 0x0400145B RID: 5211
	private TMP_Text txt2;

	// Token: 0x0400145C RID: 5212
	public bool lookForPreviousMission;

	// Token: 0x0400145D RID: 5213
	public bool lookForLatestMission;
}
