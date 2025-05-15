using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class StalkerController : MonoSingleton<StalkerController>
{
	// Token: 0x060018E4 RID: 6372 RVA: 0x000C9FF5 File Offset: 0x000C81F5
	public bool CheckIfTargetTaken(Transform target)
	{
		return this.targets.Contains(target);
	}

	// Token: 0x040022C2 RID: 8898
	public List<Transform> targets = new List<Transform>();
}
