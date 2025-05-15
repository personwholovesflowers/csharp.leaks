using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
[DefaultExecutionOrder(-150)]
public class PlaceholderPrefab : MonoBehaviour
{
	// Token: 0x06000139 RID: 313 RVA: 0x00006B70 File Offset: 0x00004D70
	private void Awake()
	{
		PrefabReplacer.Instance.ReplacePrefab(this);
	}

	// Token: 0x04000122 RID: 290
	public string uniqueId;
}
