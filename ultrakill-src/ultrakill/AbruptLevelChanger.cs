using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class AbruptLevelChanger : MonoBehaviour
{
	// Token: 0x060000CD RID: 205 RVA: 0x000053A0 File Offset: 0x000035A0
	public void AbruptChangeLevel(string levelname)
	{
		if (this.saveMission)
		{
			MonoSingleton<PreviousMissionSaver>.Instance.previousMission = MonoSingleton<StatsManager>.Instance.levelNumber;
		}
		SceneHelper.LoadScene(levelname, false);
	}

	// Token: 0x060000CE RID: 206 RVA: 0x000053C5 File Offset: 0x000035C5
	public void NormalChangeLevel(string levelname)
	{
		MonoSingleton<OptionsManager>.Instance.ChangeLevel(levelname);
	}

	// Token: 0x060000CF RID: 207 RVA: 0x000053D2 File Offset: 0x000035D2
	public void PositionChangeLevel(string levelname)
	{
		MonoSingleton<OptionsManager>.Instance.ChangeLevelWithPosition(levelname);
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x000053DF File Offset: 0x000035DF
	public void GoToLevel(int missionNumber)
	{
		SceneHelper.LoadScene(GetMissionName.GetSceneName(missionNumber), false);
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000053F0 File Offset: 0x000035F0
	public void GoToSavedLevel()
	{
		PreviousMissionSaver instance = MonoSingleton<PreviousMissionSaver>.Instance;
		if (instance != null)
		{
			int previousMission = instance.previousMission;
			Object.Destroy(instance.gameObject);
			this.GoToLevel(instance.previousMission);
			return;
		}
		this.GoToLevel(GameProgressSaver.GetProgress(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0)));
	}

	// Token: 0x04000067 RID: 103
	public bool loadingSplash;

	// Token: 0x04000068 RID: 104
	public bool saveMission;
}
