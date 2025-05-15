using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class ComponentsDatabase : MonoSingleton<ComponentsDatabase>
{
	// Token: 0x0400054F RID: 1359
	public HashSet<Transform> scrollers = new HashSet<Transform>();
}
