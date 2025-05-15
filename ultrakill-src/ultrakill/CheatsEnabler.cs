using System;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class CheatsEnabler : MonoBehaviour
{
	// Token: 0x060003B1 RID: 945 RVA: 0x00016E25 File Offset: 0x00015025
	private void Start()
	{
		MonoSingleton<CheatsController>.Instance.ActivateCheats();
	}
}
