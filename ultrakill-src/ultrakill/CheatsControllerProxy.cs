using System;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class CheatsControllerProxy : MonoBehaviour
{
	// Token: 0x06000375 RID: 885 RVA: 0x00015E2E File Offset: 0x0001402E
	private void OnEnable()
	{
		this.actualInstance = MonoSingleton<CheatsController>.Instance;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00015E3B File Offset: 0x0001403B
	private void Update()
	{
		this.actualInstance.Update();
	}

	// Token: 0x0400046B RID: 1131
	private CheatsController actualInstance;
}
