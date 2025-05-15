using System;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public class WeaponIdentifier : MonoBehaviour
{
	// Token: 0x06001BEA RID: 7146 RVA: 0x000E7EE6 File Offset: 0x000E60E6
	private void Start()
	{
		if (this.speedMultiplier == 0f)
		{
			this.speedMultiplier = 1f;
		}
	}

	// Token: 0x0400275B RID: 10075
	public float delay;

	// Token: 0x0400275C RID: 10076
	public float speedMultiplier;

	// Token: 0x0400275D RID: 10077
	public bool duplicate;

	// Token: 0x0400275E RID: 10078
	public Vector3 duplicateOffset;
}
