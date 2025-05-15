using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
[ExecuteInEditMode]
public class PotentialCullItem : MonoBehaviour
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001B3FC File Offset: 0x000195FC
	public GameObject TargetGameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x0400047E RID: 1150
	public bool definitelyCulled;
}
