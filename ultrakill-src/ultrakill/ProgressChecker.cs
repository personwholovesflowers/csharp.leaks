using System;
using UnityEngine;

// Token: 0x0200035E RID: 862
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class ProgressChecker : MonoSingleton<ProgressChecker>
{
	// Token: 0x06001401 RID: 5121 RVA: 0x0009FFE0 File Offset: 0x0009E1E0
	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(base.gameObject);
		if (!GameProgressSaver.GetTutorial() || !GameProgressSaver.GetIntro())
		{
			MonoSingleton<PrefsManager>.Instance.SetInt("weapon.arm0", 1);
			SceneHelper.LoadScene("Tutorial", false);
			this.continueWithoutSaving = false;
		}
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x000A002E File Offset: 0x0009E22E
	public void DisableSaving()
	{
		this.continueWithoutSaving = true;
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x000A0037 File Offset: 0x0009E237
	public void SaveLoadError(SaveLoadFailMessage.SaveLoadError error = SaveLoadFailMessage.SaveLoadError.Generic, string tempValidationError = "", Action saveRedo = null)
	{
		if (!this.continueWithoutSaving)
		{
			if (error == SaveLoadFailMessage.SaveLoadError.Generic)
			{
				this.continueWithoutSaving = true;
			}
			MonoSingleton<SaveLoadFailMessage>.Instance.ShowError(error, tempValidationError, saveRedo);
		}
	}

	// Token: 0x04001B5B RID: 7003
	public bool continueWithoutSaving;
}
