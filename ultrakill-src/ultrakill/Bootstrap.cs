using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x0200008B RID: 139
public class Bootstrap : MonoBehaviour
{
	// Token: 0x060002AD RID: 685 RVA: 0x0000F8FC File Offset: 0x0000DAFC
	private void Start()
	{
		Debug.Log(Addressables.RuntimePath);
		if (!Debug.isDebugBuild)
		{
			Debug.Log("Disabling all non-error console messages!");
			Debug.unityLogger.filterLogType = LogType.Error;
		}
		GameBuildSettings instance = GameBuildSettings.GetInstance();
		if (!instance.noTutorial && (!GameProgressSaver.GetTutorial() || !GameProgressSaver.GetIntro()))
		{
			MonoSingleton<PrefsManager>.Instance.SetInt("weapon.arm0", 1);
			SceneHelper.LoadScene("Tutorial", true);
			return;
		}
		if (instance.startScene != null)
		{
			SceneHelper.LoadScene(instance.startScene, true);
			return;
		}
		SceneHelper.LoadScene("Intro", true);
	}
}
