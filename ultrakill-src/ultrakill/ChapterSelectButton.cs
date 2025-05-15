using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A9 RID: 169
public class ChapterSelectButton : MonoBehaviour
{
	// Token: 0x06000353 RID: 851 RVA: 0x00014F54 File Offset: 0x00013154
	private void Awake()
	{
		this.buttonBg = base.GetComponent<Image>();
		this.originalSprite = this.buttonBg.sprite;
		this.rankButton = this.rankText.transform.parent.GetComponent<Image>();
		this.originalRankSprite = this.rankButton.sprite;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00014FAA File Offset: 0x000131AA
	private void OnEnable()
	{
		this.CheckScore();
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00014FB4 File Offset: 0x000131B4
	private void OnDisable()
	{
		this.totalScore = 0;
		this.notComplete = false;
		this.golds = 0;
		this.allPerfects = 0;
		this.buttonBg.color = Color.white;
		this.buttonBg.sprite = this.originalSprite;
		this.rankText.text = "";
		this.rankButton.color = Color.white;
		this.rankButton.sprite = this.originalRankSprite;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00015030 File Offset: 0x00013230
	public void CheckScore()
	{
		this.totalScore = 0;
		this.notComplete = false;
		this.golds = 0;
		this.allPerfects = 0;
		if (this.buttonBg == null)
		{
			this.buttonBg = base.GetComponent<Image>();
		}
		this.buttonBg.color = Color.white;
		this.buttonBg.sprite = this.originalSprite;
		foreach (LayerSelect layerSelect in this.layersInChapter)
		{
			layerSelect.CheckScore();
			this.totalScore += layerSelect.trueScore;
			if (!layerSelect.complete)
			{
				this.notComplete = true;
			}
			if (layerSelect.allPerfects)
			{
				this.allPerfects++;
			}
			if (layerSelect.gold)
			{
				this.golds++;
			}
		}
		if (!this.notComplete)
		{
			if (this.allPerfects == this.layersInChapter.Length)
			{
				this.rankText.text = "<color=#FFFFFF>P</color>";
				this.rankButton.color = new Color(1f, 0.686f, 0f, 1f);
				this.rankButton.sprite = this.rankOnP;
				if (this.golds == this.layersInChapter.Length)
				{
					this.buttonBg.color = new Color(1f, 0.686f, 0f, 1f);
					this.buttonBg.sprite = this.buttonOnP;
					return;
				}
			}
			else
			{
				this.totalScore /= this.layersInChapter.Length;
				switch (this.totalScore)
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
				case 7:
				case 8:
				case 9:
					this.rankText.text = "<color=#FF0000>S</color>";
					break;
				default:
					this.rankText.text = "<color=#0094FF>D</color>";
					break;
				}
				this.rankButton.color = Color.white;
				this.rankButton.sprite = this.originalRankSprite;
			}
		}
	}

	// Token: 0x04000434 RID: 1076
	public LayerSelect[] layersInChapter;

	// Token: 0x04000435 RID: 1077
	public TMP_Text rankText;

	// Token: 0x04000436 RID: 1078
	private Image buttonBg;

	// Token: 0x04000437 RID: 1079
	private Sprite originalSprite;

	// Token: 0x04000438 RID: 1080
	[SerializeField]
	private Sprite buttonOnP;

	// Token: 0x04000439 RID: 1081
	private Image rankButton;

	// Token: 0x0400043A RID: 1082
	private Sprite originalRankSprite;

	// Token: 0x0400043B RID: 1083
	[SerializeField]
	private Sprite rankOnP;

	// Token: 0x0400043C RID: 1084
	public int totalScore;

	// Token: 0x0400043D RID: 1085
	public bool notComplete;

	// Token: 0x0400043E RID: 1086
	public int golds;

	// Token: 0x0400043F RID: 1087
	public int allPerfects;
}
