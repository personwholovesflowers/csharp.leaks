using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class Bloodstain : MonoBehaviour
{
	// Token: 0x06000294 RID: 660 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void OnDestroy()
	{
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0000EF40 File Offset: 0x0000D140
	private void Update()
	{
		if (base.transform.hasChanged)
		{
			MonoSingleton<BloodsplatterManager>.Instance.props[this.trackedIndex] = default(BloodsplatterManager.InstanceProperties);
			base.transform.hasChanged = false;
		}
	}

	// Token: 0x04000322 RID: 802
	public int trackedIndex;
}
