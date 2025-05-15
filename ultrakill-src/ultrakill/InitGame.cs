using System;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class InitGame : MonoBehaviour
{
	// Token: 0x06000D8A RID: 3466 RVA: 0x0006685C File Offset: 0x00064A5C
	private void Awake()
	{
		if (InitGame.hasInitialized)
		{
			return;
		}
		int intLocal = MonoSingleton<PrefsManager>.Instance.GetIntLocal("resolutionWidth", -1);
		int intLocal2 = MonoSingleton<PrefsManager>.Instance.GetIntLocal("resolutionHeight", -1);
		bool boolLocal = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("fullscreen", false);
		if (intLocal != -1 && intLocal2 != -1)
		{
			Screen.SetResolution(intLocal, intLocal2, boolLocal);
		}
		else
		{
			Resolution currentResolution = Screen.currentResolution;
			Screen.SetResolution(currentResolution.width, currentResolution.height, boolLocal);
		}
		InitGame.hasInitialized = true;
	}

	// Token: 0x04001210 RID: 4624
	public static bool hasInitialized;
}
