using System;
using plog;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000017 RID: 23
public class PauseMenu : MonoBehaviour
{
	// Token: 0x060000C9 RID: 201 RVA: 0x00005258 File Offset: 0x00003458
	private void OnEnable()
	{
		MapInfoBase instance = MapInfoBase.Instance;
		if (instance == null)
		{
			this.checkpointButton.interactable = false;
			PauseMenu.Log.Warning("MapInfoBase.Instance is null", null, null, null);
			return;
		}
		if (!instance.replaceCheckpointButtonWithSkip)
		{
			bool flag = MonoSingleton<StatsManager>.Instance.currentCheckPoint != null;
			this.checkpointButton.interactable = flag;
			return;
		}
		if (this.nonStandardCheckpointButton)
		{
			return;
		}
		if (StockMapInfo.Instance != null && !SceneHelper.IsPlayingCustom)
		{
			this.checkpointText.text = "SKIP";
			this.checkpointButton.interactable = true;
			this.checkpointButton.onClick.RemoveAllListeners();
			this.checkpointButton.onClick.AddListener(new UnityAction(this.OnCheckpointButton));
		}
		else
		{
			this.checkpointText.text = "NOT IMPLEMENTED";
			this.checkpointButton.interactable = false;
			PauseMenu.Log.Warning("StockMapInfo is null or SceneHelper.IsPlayingCustom is true", null, null, null);
		}
		this.nonStandardCheckpointButton = true;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005358 File Offset: 0x00003558
	private void OnCheckpointButton()
	{
		StockMapInfo instance = StockMapInfo.Instance;
		if (instance == null)
		{
			return;
		}
		string nextSceneName = instance.nextSceneName;
		if (!string.IsNullOrEmpty(nextSceneName))
		{
			MonoSingleton<OptionsMenuToManager>.Instance.ChangeLevel(nextSceneName);
		}
	}

	// Token: 0x04000063 RID: 99
	private static readonly global::plog.Logger Log = new global::plog.Logger("PauseMenu");

	// Token: 0x04000064 RID: 100
	[SerializeField]
	private Button checkpointButton;

	// Token: 0x04000065 RID: 101
	[SerializeField]
	private TMP_Text checkpointText;

	// Token: 0x04000066 RID: 102
	private bool nonStandardCheckpointButton;
}
