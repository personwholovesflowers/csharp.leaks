using System;
using ScriptableObjects;
using UnityEngine;

// Token: 0x0200003A RID: 58
public abstract class MapInfoBase : MonoBehaviour
{
	// Token: 0x06000135 RID: 309 RVA: 0x00006B2B File Offset: 0x00004D2B
	internal virtual void Awake()
	{
		if (!MapInfoBase.Instance)
		{
			MapInfoBase.Instance = this;
		}
	}

	// Token: 0x04000116 RID: 278
	public static MapInfoBase Instance;

	// Token: 0x04000117 RID: 279
	public string layerName = "LAYER /// NUMBER";

	// Token: 0x04000118 RID: 280
	public string levelName = "LEVEL NAME";

	// Token: 0x04000119 RID: 281
	public bool sandboxTools;

	// Token: 0x0400011A RID: 282
	public bool hideStockHUD;

	// Token: 0x0400011B RID: 283
	public bool replaceCheckpointButtonWithSkip;

	// Token: 0x0400011C RID: 284
	public bool forceUpdateEnemyRenderers;

	// Token: 0x0400011D RID: 285
	public bool continuousGibCollisions;

	// Token: 0x0400011E RID: 286
	public bool removeGibsWithoutAbsorbers;

	// Token: 0x0400011F RID: 287
	public float gibRemoveTime = 5f;

	// Token: 0x04000120 RID: 288
	public TipOfTheDay tipOfTheDay;
}
