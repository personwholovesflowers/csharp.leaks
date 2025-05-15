using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002C6 RID: 710
public class LevelSelectPanel : MonoBehaviour
{
	// Token: 0x06000F50 RID: 3920 RVA: 0x00070794 File Offset: 0x0006E994
	private void Setup()
	{
		if (this.ls == null)
		{
			this.ls = base.transform.parent.GetComponent<LayerSelect>();
		}
		if (this.ls == null && base.transform.parent.parent != null)
		{
			this.ls = base.transform.parent.parent.GetComponent<LayerSelect>();
		}
		if (this.origSprite == null)
		{
			this.origSprite = base.transform.Find("Image").GetComponent<Image>().sprite;
		}
		if (this.unfilledPanel == null && this.challengeIcon != null)
		{
			this.unfilledPanel = this.challengeIcon.sprite;
		}
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00070864 File Offset: 0x0006EA64
	public void CheckScore()
	{
		this.Setup();
		this.rectTransform = base.GetComponent<RectTransform>();
		if (this.levelNumber == 666)
		{
			this.tempInt = GameProgressSaver.GetPrime(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0), this.levelNumberInLayer);
		}
		else if (this.levelNumber == 100)
		{
			this.tempInt = GameProgressSaver.GetEncoreProgress(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0));
		}
		else
		{
			this.tempInt = GameProgressSaver.GetProgress(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0));
		}
		int num = this.levelNumber;
		if (this.levelNumber == 666 || this.levelNumber == 100)
		{
			num += this.levelNumberInLayer - 1;
		}
		this.origName = GetMissionName.GetMission(num);
		if ((this.levelNumber == 666 && this.tempInt == 0) || (this.levelNumber == 100 && this.tempInt < this.levelNumberInLayer - 1) || (this.levelNumber != 666 && this.levelNumber != 100 && this.tempInt < this.levelNumber) || this.forceOff)
		{
			string text = this.ls.layerNumber.ToString();
			if (this.ls.layerNumber == 666)
			{
				text = "P";
			}
			if (this.ls.layerNumber == 100)
			{
				base.transform.Find("Name").GetComponent<TMP_Text>().text = (this.levelNumberInLayer - 1).ToString() + "-E: ???";
			}
			else
			{
				base.transform.Find("Name").GetComponent<TMP_Text>().text = text + "-" + this.levelNumberInLayer.ToString() + ": ???";
			}
			base.transform.Find("Image").GetComponent<Image>().sprite = this.lockedSprite;
			base.GetComponent<Button>().enabled = false;
			this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, this.collapsedHeight);
			this.leaderboardPanel.SetActive(false);
		}
		else
		{
			bool flag;
			if (this.tempInt == this.levelNumber || (this.levelNumber == 100 && this.tempInt == this.levelNumberInLayer - 1) || (this.levelNumber == 666 && this.tempInt == 1))
			{
				flag = false;
				base.transform.Find("Image").GetComponent<Image>().sprite = this.unlockedSprite;
				base.transform.Find("Name").GetComponent<TMP_Text>().text = (this.levelNumberInLayer - 1).ToString() + "-E: ???";
			}
			else
			{
				flag = true;
				base.transform.Find("Image").GetComponent<Image>().sprite = this.origSprite;
			}
			if (this.levelNumber != 100 || this.tempInt != this.levelNumberInLayer - 1)
			{
				base.transform.Find("Name").GetComponent<TMP_Text>().text = this.origName;
			}
			base.GetComponent<Button>().enabled = true;
			if (this.challengeIcon != null)
			{
				if (this.challengeChecker == null)
				{
					this.challengeChecker = this.challengeIcon.transform.Find("EventTrigger").gameObject;
				}
				if (this.tempInt > this.levelNumber)
				{
					this.challengeChecker.SetActive(true);
				}
			}
			if (GameStateManager.ShowLeaderboards && flag)
			{
				this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, this.expandedHeight);
				this.leaderboardPanel.SetActive(true);
			}
			else
			{
				this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, this.collapsedHeight);
				this.leaderboardPanel.SetActive(false);
			}
		}
		RankData rank = GameProgressSaver.GetRank(num, false);
		if (rank == null)
		{
			Debug.Log("Didn't Find Level " + this.levelNumber.ToString() + " Data");
			Image component = base.transform.Find("Stats").Find("Rank").GetComponent<Image>();
			component.color = Color.white;
			component.sprite = this.unfilledPanel;
			component.GetComponentInChildren<TMP_Text>().text = "";
			this.allSecrets = false;
			foreach (Image image in this.secretIcons)
			{
				image.enabled = true;
				image.sprite = this.unfilledPanel;
			}
			return;
		}
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		if (rank.levelNumber == this.levelNumber || ((this.levelNumber == 666 || this.levelNumber == 100) && rank.levelNumber == this.levelNumber + this.levelNumberInLayer - 1))
		{
			TMP_Text componentInChildren = base.transform.Find("Stats").Find("Rank").GetComponentInChildren<TMP_Text>();
			if (rank.ranks[@int] == 12 && (rank.majorAssists == null || !rank.majorAssists[@int]))
			{
				componentInChildren.text = "<color=#FFFFFF>P</color>";
				Image component2 = componentInChildren.transform.parent.GetComponent<Image>();
				component2.color = new Color(1f, 0.686f, 0f, 1f);
				component2.sprite = this.filledPanel;
				this.ls.AddScore(4, true);
			}
			else if (rank.majorAssists != null && rank.majorAssists[@int])
			{
				if (rank.ranks[@int] < 0)
				{
					componentInChildren.text = "";
				}
				else
				{
					switch (rank.ranks[@int])
					{
					case 1:
						componentInChildren.text = "C";
						this.ls.AddScore(1, false);
						break;
					case 2:
						componentInChildren.text = "B";
						this.ls.AddScore(2, false);
						break;
					case 3:
						componentInChildren.text = "A";
						this.ls.AddScore(3, false);
						break;
					case 4:
					case 5:
					case 6:
						this.ls.AddScore(4, false);
						componentInChildren.text = "S";
						break;
					default:
						this.ls.AddScore(0, false);
						componentInChildren.text = "D";
						break;
					}
					Image component3 = componentInChildren.transform.parent.GetComponent<Image>();
					component3.color = new Color(0.3f, 0.6f, 0.9f, 1f);
					component3.sprite = this.filledPanel;
				}
			}
			else if (rank.ranks[@int] < 0)
			{
				componentInChildren.text = "";
				Image component4 = componentInChildren.transform.parent.GetComponent<Image>();
				component4.color = Color.white;
				component4.sprite = this.unfilledPanel;
			}
			else
			{
				switch (rank.ranks[@int])
				{
				case 1:
					componentInChildren.text = "<color=#4CFF00>C</color>";
					this.ls.AddScore(1, false);
					break;
				case 2:
					componentInChildren.text = "<color=#FFD800>B</color>";
					this.ls.AddScore(2, false);
					break;
				case 3:
					componentInChildren.text = "<color=#FF6A00>A</color>";
					this.ls.AddScore(3, false);
					break;
				case 4:
				case 5:
				case 6:
					this.ls.AddScore(4, false);
					componentInChildren.text = "<color=#FF0000>S</color>";
					break;
				default:
					this.ls.AddScore(0, false);
					componentInChildren.text = "<color=#0094FF>D</color>";
					break;
				}
				Image component5 = componentInChildren.transform.parent.GetComponent<Image>();
				component5.color = Color.white;
				component5.sprite = this.unfilledPanel;
			}
			if (rank.secretsAmount > 0)
			{
				this.allSecrets = true;
				for (int j = 0; j < 5; j++)
				{
					if (j < rank.secretsAmount && rank.secretsFound[j])
					{
						this.secretIcons[j].sprite = this.filledPanel;
					}
					else
					{
						this.allSecrets = false;
						this.secretIcons[j].sprite = this.unfilledPanel;
					}
				}
			}
			else
			{
				Image[] array = this.secretIcons;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = false;
				}
			}
			if (this.challengeIcon)
			{
				if (rank.challenge)
				{
					this.challengeIcon.sprite = this.filledPanel;
					TMP_Text componentInChildren2 = this.challengeIcon.GetComponentInChildren<TMP_Text>();
					componentInChildren2.text = "C O M P L E T E";
					if (rank.ranks[@int] == 12 && (this.allSecrets || rank.secretsAmount == 0))
					{
						componentInChildren2.color = new Color(0.6f, 0.4f, 0f, 1f);
					}
					else
					{
						componentInChildren2.color = Color.black;
					}
				}
				else
				{
					this.challengeIcon.sprite = this.unfilledPanel;
					TMP_Text componentInChildren3 = this.challengeIcon.GetComponentInChildren<TMP_Text>();
					componentInChildren3.text = "C H A L L E N G E";
					componentInChildren3.color = Color.white;
				}
			}
		}
		else
		{
			Debug.Log("Error in finding " + this.levelNumber.ToString() + " Data");
			Image component6 = base.transform.Find("Stats").Find("Rank").GetComponent<Image>();
			component6.color = Color.white;
			component6.sprite = this.unfilledPanel;
			component6.GetComponentInChildren<TMP_Text>().text = "";
			this.allSecrets = false;
			foreach (Image image2 in this.secretIcons)
			{
				image2.enabled = true;
				image2.sprite = this.unfilledPanel;
			}
		}
		if ((rank.challenge || !this.challengeIcon) && rank.ranks[@int] == 12 && (this.allSecrets || rank.secretsAmount == 0))
		{
			this.ls.Gold();
			base.GetComponent<Image>().color = new Color(1f, 0.686f, 0f, 0.75f);
			return;
		}
		base.GetComponent<Image>().color = this.defaultColor;
	}

	// Token: 0x04001477 RID: 5239
	public float collapsedHeight = 260f;

	// Token: 0x04001478 RID: 5240
	public float expandedHeight = 400f;

	// Token: 0x04001479 RID: 5241
	public GameObject leaderboardPanel;

	// Token: 0x0400147A RID: 5242
	private RectTransform rectTransform;

	// Token: 0x0400147B RID: 5243
	public int levelNumber;

	// Token: 0x0400147C RID: 5244
	public int levelNumberInLayer;

	// Token: 0x0400147D RID: 5245
	private bool allSecrets;

	// Token: 0x0400147E RID: 5246
	public Sprite lockedSprite;

	// Token: 0x0400147F RID: 5247
	public Sprite unlockedSprite;

	// Token: 0x04001480 RID: 5248
	private Sprite origSprite;

	// Token: 0x04001481 RID: 5249
	public Image[] secretIcons;

	// Token: 0x04001482 RID: 5250
	public Image challengeIcon;

	// Token: 0x04001483 RID: 5251
	private int tempInt;

	// Token: 0x04001484 RID: 5252
	private string origName;

	// Token: 0x04001485 RID: 5253
	public Sprite unfilledPanel;

	// Token: 0x04001486 RID: 5254
	public Sprite filledPanel;

	// Token: 0x04001487 RID: 5255
	private LayerSelect ls;

	// Token: 0x04001488 RID: 5256
	private GameObject challengeChecker;

	// Token: 0x04001489 RID: 5257
	public bool forceOff;

	// Token: 0x0400148A RID: 5258
	private Color defaultColor = new Color(0f, 0f, 0f, 0.35f);
}
