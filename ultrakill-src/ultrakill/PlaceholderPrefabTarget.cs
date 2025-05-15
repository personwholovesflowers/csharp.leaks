using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
[Serializable]
public class PlaceholderPrefabTarget
{
	// Token: 0x04000124 RID: 292
	public bool delayedSwap = true;

	// Token: 0x04000125 RID: 293
	public string uniqueId;

	// Token: 0x04000126 RID: 294
	[HideInInspector]
	public GameObject actualPrefab;

	// Token: 0x04000127 RID: 295
	public string assetPath;
}
