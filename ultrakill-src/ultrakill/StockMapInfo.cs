using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
[DefaultExecutionOrder(-300)]
public class StockMapInfo : MapInfoBase
{
	// Token: 0x06000189 RID: 393 RVA: 0x000080FA File Offset: 0x000062FA
	internal override void Awake()
	{
		base.Awake();
		if (StockMapInfo.Instance == null)
		{
			StockMapInfo.Instance = this;
		}
	}

	// Token: 0x04000168 RID: 360
	public new static StockMapInfo Instance;

	// Token: 0x04000169 RID: 361
	public string nextSceneName;

	// Token: 0x0400016A RID: 362
	public SerializedActivityAssets assets;
}
