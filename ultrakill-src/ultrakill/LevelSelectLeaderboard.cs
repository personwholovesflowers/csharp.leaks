using System;
using System.Collections;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020002A9 RID: 681
public class LevelSelectLeaderboard : MonoBehaviour
{
	// Token: 0x06000ED8 RID: 3800 RVA: 0x0006E9A8 File Offset: 0x0006CBA8
	public void RefreshAnyPercent()
	{
		this.anyPercentButton.sprite = this.selectedSprite;
		this.pRankButton.sprite = this.unselectedSprite;
		this.anyPercentLabel.color = global::UnityEngine.Color.black;
		this.pRankLabel.color = global::UnityEngine.Color.white;
		this.container.gameObject.SetActive(false);
		this.scrollRectContainer.SetActive(false);
		this.loadingPanel.SetActive(true);
		this.noItemsPanel.SetActive(false);
		this.ResetEntries();
		this.pRankSelected = false;
		base.StopAllCoroutines();
		base.StartCoroutine(this.Fetch("Level " + this.layerLevelNumber));
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0006EA5C File Offset: 0x0006CC5C
	public void RefreshPRank()
	{
		this.anyPercentButton.sprite = this.unselectedSprite;
		this.pRankButton.sprite = this.selectedSprite;
		this.anyPercentLabel.color = global::UnityEngine.Color.white;
		this.pRankLabel.color = global::UnityEngine.Color.black;
		this.container.gameObject.SetActive(false);
		this.scrollRectContainer.SetActive(false);
		this.loadingPanel.SetActive(true);
		this.noItemsPanel.SetActive(false);
		this.ResetEntries();
		this.pRankSelected = true;
		base.StopAllCoroutines();
		base.StartCoroutine(this.Fetch("Level " + this.layerLevelNumber));
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0006EB10 File Offset: 0x0006CD10
	private void OnEnable()
	{
		this.RefreshAnyPercent();
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0006EB18 File Offset: 0x0006CD18
	private void ResetEntries()
	{
		foreach (object obj in this.container)
		{
			Transform transform = (Transform)obj;
			if (!(transform.gameObject == this.template))
			{
				Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0006EB88 File Offset: 0x0006CD88
	private bool IsLayerSelected()
	{
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (currentSelectedGameObject == null)
		{
			return false;
		}
		LevelSelectPanel componentInParent = currentSelectedGameObject.GetComponentInParent<LevelSelectPanel>();
		return !(componentInParent == null) && this.levelSelect == componentInParent;
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0006EBC9 File Offset: 0x0006CDC9
	private IEnumerator Fetch(string levelName)
	{
		if (string.IsNullOrEmpty(levelName))
		{
			yield break;
		}
		Task<LeaderboardEntry[]> entryTask = MonoSingleton<LeaderboardController>.Instance.GetLevelScores(levelName, this.pRankSelected);
		while (!entryTask.IsCompleted)
		{
			yield return null;
		}
		if (entryTask.Result == null)
		{
			yield break;
		}
		LeaderboardEntry[] result = entryTask.Result;
		foreach (LeaderboardEntry leaderboardEntry in result)
		{
			TMP_Text tmp_Text = this.templateUsername;
			Friend user = leaderboardEntry.User;
			tmp_Text.text = user.Name;
			int score = leaderboardEntry.Score;
			int num = score / 60000;
			float num2 = (float)(score - num * 60000) / 1000f;
			this.templateTime.text = string.Format("{0}:{1:00.000}", num, num2);
			int? num3 = null;
			if (leaderboardEntry.Details.Length != 0)
			{
				num3 = new int?(leaderboardEntry.Details[0]);
			}
			int num4 = LeaderboardProperties.Difficulties.Length;
			int? num5 = num3;
			if ((num4 <= num5.GetValueOrDefault()) & (num5 != null))
			{
				Debug.LogWarning(string.Format("Difficulty {0} is out of range for {1}", num3, levelName));
			}
			else
			{
				this.templateDifficulty.text = ((num3 == null) ? "UNKNOWN" : LeaderboardProperties.Difficulties[num3.Value].ToUpper());
				GameObject gameObject = Object.Instantiate<GameObject>(this.template, this.container);
				gameObject.SetActive(true);
				SteamController.FetchAvatar(gameObject.GetComponentInChildren<RawImage>(), leaderboardEntry.User);
			}
		}
		if (result.Length == 0)
		{
			this.noItemsPanel.SetActive(true);
		}
		this.loadingPanel.SetActive(false);
		this.container.gameObject.SetActive(true);
		this.scrollRectContainer.SetActive(true);
		yield break;
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0006EBDF File Offset: 0x0006CDDF
	private void Update()
	{
		if (!(MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad))
		{
			return;
		}
		this.UpdateLeaderboardScroll();
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0006EBFC File Offset: 0x0006CDFC
	public void SwitchLeaderboardType(bool pRank)
	{
		if (pRank)
		{
			this.RefreshPRank();
			return;
		}
		this.RefreshAnyPercent();
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x0006EC10 File Offset: 0x0006CE10
	private void UpdateLeaderboardScroll()
	{
		Vector2 vector = this.scrollSublistAction.ReadValue<Vector2>();
		if (vector == Vector2.zero)
		{
			return;
		}
		if (!this.IsLayerSelected())
		{
			return;
		}
		if (vector.y > 0f)
		{
			this.scrollRect.verticalNormalizedPosition += 0.01f * Time.deltaTime * this.controllerScrollSpeed;
			return;
		}
		if (vector.y < 0f)
		{
			this.scrollRect.verticalNormalizedPosition -= 0.01f * Time.deltaTime * this.controllerScrollSpeed;
		}
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0006ECA3 File Offset: 0x0006CEA3
	private void Start()
	{
		this.scrollSublistAction = this.inputActionAsset.FindAction("ScrollSublist", false);
		if (this.scrollSublistAction.enabled)
		{
			return;
		}
		Debug.Log("Enabling scroll sublist action");
		this.scrollSublistAction.Enable();
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0006ECDF File Offset: 0x0006CEDF
	private void Awake()
	{
		this.levelSelect = base.GetComponentInParent<LevelSelectPanel>();
	}

	// Token: 0x040013E4 RID: 5092
	public string layerLevelNumber;

	// Token: 0x040013E5 RID: 5093
	[SerializeField]
	private GameObject scrollRectContainer;

	// Token: 0x040013E6 RID: 5094
	[SerializeField]
	private GameObject template;

	// Token: 0x040013E7 RID: 5095
	[SerializeField]
	private TMP_Text templateUsername;

	// Token: 0x040013E8 RID: 5096
	[SerializeField]
	private TMP_Text templateTime;

	// Token: 0x040013E9 RID: 5097
	[SerializeField]
	private TMP_Text templateDifficulty;

	// Token: 0x040013EA RID: 5098
	[Space]
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x040013EB RID: 5099
	[SerializeField]
	private Transform container;

	// Token: 0x040013EC RID: 5100
	[Space]
	[SerializeField]
	private Sprite unselectedSprite;

	// Token: 0x040013ED RID: 5101
	[SerializeField]
	private Sprite selectedSprite;

	// Token: 0x040013EE RID: 5102
	[SerializeField]
	private global::UnityEngine.UI.Image anyPercentButton;

	// Token: 0x040013EF RID: 5103
	[SerializeField]
	private global::UnityEngine.UI.Image pRankButton;

	// Token: 0x040013F0 RID: 5104
	[SerializeField]
	private TMP_Text anyPercentLabel;

	// Token: 0x040013F1 RID: 5105
	[SerializeField]
	private TMP_Text pRankLabel;

	// Token: 0x040013F2 RID: 5106
	[Space]
	[SerializeField]
	private GameObject loadingPanel;

	// Token: 0x040013F3 RID: 5107
	[SerializeField]
	private GameObject noItemsPanel;

	// Token: 0x040013F4 RID: 5108
	[Space]
	[SerializeField]
	private InputActionAsset inputActionAsset;

	// Token: 0x040013F5 RID: 5109
	[SerializeField]
	private float controllerScrollSpeed = 250f;

	// Token: 0x040013F6 RID: 5110
	private bool pRankSelected;

	// Token: 0x040013F7 RID: 5111
	private LevelSelectPanel levelSelect;

	// Token: 0x040013F8 RID: 5112
	private InputAction scrollSublistAction;
}
