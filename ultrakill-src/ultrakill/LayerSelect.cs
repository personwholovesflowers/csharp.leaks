using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200029A RID: 666
public class LayerSelect : MonoBehaviour
{
	// Token: 0x06000EA7 RID: 3751 RVA: 0x0006CA6A File Offset: 0x0006AC6A
	private void Awake()
	{
		this.childLeaderboards = base.GetComponentsInChildren<LevelSelectLeaderboard>(true);
		this.Setup();
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x0006CA80 File Offset: 0x0006AC80
	private void Setup()
	{
		if (this.rankText == null)
		{
			this.rankText = base.transform.Find("Header").Find("RankPanel").GetComponentInChildren<TMP_Text>();
		}
		if (this.rankImage == null && this.rankText)
		{
			this.rankImage = this.rankText.transform.parent.GetComponent<Image>();
		}
		if (this.rankSpriteOriginal == null && this.rankImage)
		{
			this.rankSpriteOriginal = this.rankImage.sprite;
		}
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x0006CB24 File Offset: 0x0006AD24
	private void OnDisable()
	{
		this.totalScore = 0f;
		this.scoresChecked = 0f;
		this.perfects = 0;
		this.golds = 0;
		this.rankText.text = "";
		this.rankImage.color = Color.white;
		this.rankImage.sprite = this.rankSpriteOriginal;
		this.secretMission = false;
		base.GetComponent<Image>().color = this.defaultColor;
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x0006CBA0 File Offset: 0x0006ADA0
	public void CheckScore()
	{
		this.Setup();
		this.totalScore = 0f;
		this.trueScore = 0;
		this.scoresChecked = 0f;
		this.perfects = 0;
		this.golds = 0;
		this.complete = false;
		this.allPerfects = false;
		this.gold = false;
		this.rankText.text = "";
		this.rankImage.color = Color.white;
		this.rankImage.sprite = this.rankSpriteOriginal;
		this.secretMission = false;
		base.GetComponent<Image>().color = this.defaultColor;
		LevelSelectPanel[] componentsInChildren = base.GetComponentsInChildren<LevelSelectPanel>(true);
		SecretMissionPanel secretMissionPanel = this.secretMissionPanel;
		if (secretMissionPanel != null)
		{
			secretMissionPanel.GotEnabled();
		}
		LevelSelectPanel[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].CheckScore();
		}
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x0006CC6C File Offset: 0x0006AE6C
	public void AddScore(int score, bool perfect = false)
	{
		this.Setup();
		if (this.golds < this.levelAmount)
		{
			base.GetComponent<Image>().color = this.defaultColor;
		}
		this.scoresChecked += 1f;
		this.totalScore += (float)score;
		if (perfect)
		{
			this.perfects++;
		}
		if (this.scoresChecked == (float)this.levelAmount)
		{
			this.complete = true;
			if (this.perfects == this.levelAmount)
			{
				this.rankText.text = "<color=#FFFFFF>P</color>";
				this.rankImage.color = new Color(1f, 0.686f, 0f, 1f);
				this.rankImage.sprite = this.rankSpriteOnP;
				this.allPerfects = true;
				this.trueScore = Mathf.RoundToInt(this.totalScore / (float)this.levelAmount);
				return;
			}
			this.trueScore = Mathf.RoundToInt(this.totalScore / (float)this.levelAmount);
			float num = this.totalScore / (float)this.levelAmount;
			Debug.Log("True Score: " + this.trueScore.ToString() + ". Real score: " + num.ToString());
			switch (this.trueScore)
			{
			case 1:
				this.rankText.text = "<color=#4CFF00>C</color>";
				break;
			case 2:
				this.rankText.text = "<color=#FFD800>B</color>";
				break;
			case 3:
				this.rankText.text = "<color=#FF6A00>A</color>";
				break;
			case 4:
			case 5:
			case 6:
				this.rankText.text = "<color=#FF0000>S</color>";
				break;
			default:
				this.rankText.text = "<color=#0094FF>D</color>";
				break;
			}
			this.rankImage.color = Color.white;
			this.rankImage.sprite = this.rankSpriteOriginal;
		}
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x0006CE50 File Offset: 0x0006B050
	public void Gold()
	{
		this.golds++;
		if (this.golds == this.levelAmount && this.levelAmount != 0 && (this.noSecretMission || this.secretMission))
		{
			base.GetComponent<Image>().color = new Color(1f, 0.686f, 0f, 0.75f);
			this.gold = true;
		}
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x0006CEBC File Offset: 0x0006B0BC
	public void SecretMissionDone()
	{
		this.secretMission = true;
		if (this.golds == this.levelAmount && this.secretMission)
		{
			base.GetComponent<Image>().color = new Color(1f, 0.686f, 0f, 0.75f);
		}
	}

	// Token: 0x0400136C RID: 4972
	public SecretMissionPanel secretMissionPanel;

	// Token: 0x0400136D RID: 4973
	public int layerNumber;

	// Token: 0x0400136E RID: 4974
	public int levelAmount;

	// Token: 0x0400136F RID: 4975
	private float totalScore;

	// Token: 0x04001370 RID: 4976
	private float scoresChecked;

	// Token: 0x04001371 RID: 4977
	private int perfects;

	// Token: 0x04001372 RID: 4978
	public int golds;

	// Token: 0x04001373 RID: 4979
	private bool secretMission;

	// Token: 0x04001374 RID: 4980
	[HideInInspector]
	public TMP_Text rankText;

	// Token: 0x04001375 RID: 4981
	[HideInInspector]
	public Image rankImage;

	// Token: 0x04001376 RID: 4982
	[HideInInspector]
	public Sprite rankSpriteOriginal;

	// Token: 0x04001377 RID: 4983
	public Sprite rankSpriteOnP;

	// Token: 0x04001378 RID: 4984
	public bool gold;

	// Token: 0x04001379 RID: 4985
	public bool allPerfects;

	// Token: 0x0400137A RID: 4986
	public int trueScore;

	// Token: 0x0400137B RID: 4987
	public bool complete;

	// Token: 0x0400137C RID: 4988
	public bool noSecretMission;

	// Token: 0x0400137D RID: 4989
	[HideInInspector]
	public LevelSelectLeaderboard[] childLeaderboards;

	// Token: 0x0400137E RID: 4990
	private Color defaultColor = new Color(0f, 0f, 0f, 0.35f);
}
