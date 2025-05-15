using System;
using System.Collections;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

// Token: 0x020002A7 RID: 679
public class LevelEndLeaderboard : MonoBehaviour
{
	// Token: 0x06000ECC RID: 3788 RVA: 0x0006E37B File Offset: 0x0006C57B
	private void Start()
	{
		this.keyboardControlScheme = MonoSingleton<InputManager>.Instance.InputSource.Actions.KeyboardMouseScheme;
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x0006E398 File Offset: 0x0006C598
	private void OnEnable()
	{
		if (SceneHelper.IsPlayingCustom || !SceneHelper.CurrentScene.StartsWith("Level ") || !GameStateManager.ShowLeaderboards || !MonoSingleton<PrefsManager>.Instance.GetBool("levelLeaderboards", false) || MonoSingleton<StatsManager>.Instance.firstPlayThrough)
		{
			base.gameObject.SetActive(false);
			return;
		}
		Debug.Log("Fetching level leaderboards for " + SceneHelper.CurrentScene);
		this.displayPRank = MonoSingleton<StatsManager>.Instance.rankScore == 12;
		base.StartCoroutine(this.Fetch(SceneHelper.CurrentScene));
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x0006E429 File Offset: 0x0006C629
	private IEnumerator Fetch(string levelName)
	{
		if (string.IsNullOrEmpty(levelName))
		{
			yield break;
		}
		this.ResetEntries();
		this.loadingPanel.SetActive(true);
		this.leaderboardType.text = (this.displayPRank ? "P RANK" : "ANY RANK");
		Task<LeaderboardEntry[]> entryTask = MonoSingleton<LeaderboardController>.Instance.GetLevelScores(levelName, this.displayPRank);
		while (!entryTask.IsCompleted)
		{
			yield return null;
		}
		if (entryTask.Result == null)
		{
			yield break;
		}
		foreach (LeaderboardEntry leaderboardEntry in entryTask.Result)
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
		this.loadingPanel.SetActive(false);
		this.container.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0006E440 File Offset: 0x0006C640
	private void ResetEntries()
	{
		foreach (object obj in this.container)
		{
			Transform transform = (Transform)obj;
			if (!(transform == this.template.transform))
			{
				Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0006E4B0 File Offset: 0x0006C6B0
	private void Update()
	{
		InputBinding? inputBinding = null;
		if (MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad)
		{
			if (MonoSingleton<InputManager>.Instance.InputSource.NextWeapon.Action.bindings.Count <= 0)
			{
				goto IL_01CD;
			}
			using (ReadOnlyArray<InputBinding>.Enumerator enumerator = MonoSingleton<InputManager>.Instance.InputSource.NextWeapon.Action.bindings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InputBinding inputBinding2 = enumerator.Current;
					if (!inputBinding2.groups.Contains(this.keyboardControlScheme.bindingGroup))
					{
						inputBinding = new InputBinding?(inputBinding2);
					}
				}
				goto IL_01CD;
			}
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.PreviousVariation.Action.bindings.Count > 0)
		{
			foreach (InputBinding inputBinding3 in MonoSingleton<InputManager>.Instance.InputSource.PreviousVariation.Action.bindings)
			{
				if (inputBinding3.groups.Contains(this.keyboardControlScheme.bindingGroup))
				{
					inputBinding = new InputBinding?(inputBinding3);
				}
			}
		}
		if (inputBinding == null && MonoSingleton<InputManager>.Instance.InputSource.LastWeapon.Action.bindings.Count > 0)
		{
			foreach (InputBinding inputBinding4 in MonoSingleton<InputManager>.Instance.InputSource.LastWeapon.Action.bindings)
			{
				if (inputBinding4.groups.Contains(this.keyboardControlScheme.bindingGroup))
				{
					inputBinding = new InputBinding?(inputBinding4);
				}
			}
		}
		IL_01CD:
		if (inputBinding == null)
		{
			this.switchTypeInput.text = "<color=white>[<color=orange>NO BINDING</color>]</color>";
			return;
		}
		this.switchTypeInput.text = "<color=white>[<color=orange>" + ((inputBinding != null) ? inputBinding.GetValueOrDefault().ToDisplayString((InputBinding.DisplayStringOptions)0, null).ToUpper() : null) + "</color>]</color>";
		if (MonoSingleton<InputManager>.Instance.InputSource.NextWeapon.WasPerformedThisFrame || MonoSingleton<InputManager>.Instance.InputSource.LastWeapon.WasPerformedThisFrame || MonoSingleton<InputManager>.Instance.InputSource.PreviousVariation.WasPerformedThisFrame)
		{
			this.displayPRank = !this.displayPRank;
			base.StopAllCoroutines();
			base.StartCoroutine(this.Fetch(SceneHelper.CurrentScene));
		}
	}

	// Token: 0x040013D3 RID: 5075
	[SerializeField]
	private GameObject template;

	// Token: 0x040013D4 RID: 5076
	[SerializeField]
	private TMP_Text templateUsername;

	// Token: 0x040013D5 RID: 5077
	[SerializeField]
	private TMP_Text templateTime;

	// Token: 0x040013D6 RID: 5078
	[SerializeField]
	private TMP_Text templateDifficulty;

	// Token: 0x040013D7 RID: 5079
	[Space]
	[SerializeField]
	private Transform container;

	// Token: 0x040013D8 RID: 5080
	[SerializeField]
	private TMP_Text leaderboardType;

	// Token: 0x040013D9 RID: 5081
	[SerializeField]
	private TMP_Text switchTypeInput;

	// Token: 0x040013DA RID: 5082
	[Space]
	[SerializeField]
	private GameObject loadingPanel;

	// Token: 0x040013DB RID: 5083
	private bool displayPRank;

	// Token: 0x040013DC RID: 5084
	private InputControlScheme keyboardControlScheme;

	// Token: 0x040013DD RID: 5085
	private const string LeftBracket = "<color=white>[";

	// Token: 0x040013DE RID: 5086
	private const string RightBracket = "]</color>";
}
