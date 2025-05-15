using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class FPSLockForSwitch : MonoBehaviour
{
	// Token: 0x06000A0F RID: 2575 RVA: 0x0002F9A3 File Offset: 0x0002DBA3
	private void Start()
	{
		if (Application.platform == RuntimePlatform.Switch)
		{
			QualitySettings.vSyncCount = 2;
		}
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00005444 File Offset: 0x00003644
	private void Update()
	{
	}
}
