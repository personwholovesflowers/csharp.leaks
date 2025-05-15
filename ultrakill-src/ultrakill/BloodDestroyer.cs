using System;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public class BloodDestroyer : MonoBehaviour, IBloodstainReceiver
{
	// Token: 0x06001B5C RID: 7004 RVA: 0x000E3613 File Offset: 0x000E1813
	private void OnEnable()
	{
		MonoSingleton<BloodsplatterManager>.Instance.bloodDestroyers++;
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x000E3627 File Offset: 0x000E1827
	private void OnDisable()
	{
		MonoSingleton<BloodsplatterManager>.Instance.bloodDestroyers--;
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x000C9FC2 File Offset: 0x000C81C2
	public bool HandleBloodstainHit(ref RaycastHit hit)
	{
		return false;
	}
}
