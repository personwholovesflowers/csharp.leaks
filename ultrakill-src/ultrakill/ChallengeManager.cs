using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002AF RID: 687
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class ChallengeManager : MonoSingleton<ChallengeManager>
{
	// Token: 0x06000EFD RID: 3837 RVA: 0x0006F784 File Offset: 0x0006D984
	protected override void Awake()
	{
		base.Awake();
		if (this.fr == null)
		{
			this.fr = base.GetComponentInParent<FinalRank>();
		}
		base.SendMessage("OnDisable", base.gameObject);
		base.gameObject.SetActive(false);
		this.startCheckingForChallenge = true;
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0006F7D8 File Offset: 0x0006D9D8
	private new void OnEnable()
	{
		if (this.startCheckingForChallenge)
		{
			StatsManager statsManager = (MonoSingleton<StatsManager>.Instance ? MonoSingleton<StatsManager>.Instance : Object.FindObjectOfType<StatsManager>());
			if (this.challengeDone && !this.completedCheck && !this.challengeFailed)
			{
				this.ChallengeDone();
			}
			if (statsManager.challengeComplete && (!this.challengeDone || this.challengeFailed))
			{
				this.challengePanel.GetComponent<Image>().color = new Color(1f, 0.696f, 0f, 0.5f);
				this.challengePanel.GetComponent<AudioSource>().volume = 0f;
				this.challengePanel.SetActive(true);
			}
		}
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0006F888 File Offset: 0x0006DA88
	public void ChallengeDone()
	{
		if (this.challengeFailed)
		{
			return;
		}
		if (this.fr == null)
		{
			this.fr = base.GetComponentInParent<FinalRank>();
		}
		this.challengeDone = true;
		Debug.Log("! CHALLENGE COMPLETE !");
		this.challengePanel.GetComponent<Image>().color = new Color(1f, 0.696f, 0f, 1f);
		this.challengePanel.SetActive(true);
		GameProgressSaver.ChallengeComplete();
		this.completedCheck = true;
		if (this.fr != null)
		{
			this.fr.FlashPanel(this.challengePanel.transform.parent.GetChild(2).gameObject);
		}
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x0006F93E File Offset: 0x0006DB3E
	public void ChallengeFailed()
	{
		this.challengeFailed = true;
	}

	// Token: 0x04001424 RID: 5156
	public GameObject challengePanel;

	// Token: 0x04001425 RID: 5157
	public FinalRank fr;

	// Token: 0x04001426 RID: 5158
	private bool completedCheck;

	// Token: 0x04001427 RID: 5159
	private bool startCheckingForChallenge;

	// Token: 0x04001428 RID: 5160
	public bool challengeDone;

	// Token: 0x04001429 RID: 5161
	public bool challengeFailed;

	// Token: 0x0400142A RID: 5162
	public bool challengeFailedPermanently;
}
