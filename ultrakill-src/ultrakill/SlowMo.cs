using System;
using UnityEngine;

// Token: 0x02000413 RID: 1043
public class SlowMo : MonoBehaviour
{
	// Token: 0x060017BA RID: 6074 RVA: 0x000C2A51 File Offset: 0x000C0C51
	private void OnEnable()
	{
		MonoSingleton<TimeController>.Instance.SlowDown(this.amount);
		Object.Destroy(this);
	}

	// Token: 0x04002133 RID: 8499
	public float amount;
}
