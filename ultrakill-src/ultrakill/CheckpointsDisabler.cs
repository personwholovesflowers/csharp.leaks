using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public class CheckpointsDisabler : MonoBehaviour
{
	// Token: 0x060003D8 RID: 984 RVA: 0x000189E2 File Offset: 0x00016BE2
	private void Start()
	{
		if (!this.activated)
		{
			this.activated = true;
			MonoSingleton<CheckPointsController>.Instance.DisableCheckpoints();
		}
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x000189E2 File Offset: 0x00016BE2
	private void OnEnable()
	{
		if (!this.activated)
		{
			this.activated = true;
			MonoSingleton<CheckPointsController>.Instance.DisableCheckpoints();
		}
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00018A00 File Offset: 0x00016C00
	private void OnDisable()
	{
		if (this.activated && base.gameObject.scene.isLoaded)
		{
			this.activated = false;
			MonoSingleton<CheckPointsController>.Instance.EnableCheckpoints();
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00018A3C File Offset: 0x00016C3C
	private void OnDestroy()
	{
		if (this.activated && base.gameObject.scene.isLoaded)
		{
			this.activated = false;
			MonoSingleton<CheckPointsController>.Instance.EnableCheckpoints();
		}
	}

	// Token: 0x040004BA RID: 1210
	private bool activated;
}
