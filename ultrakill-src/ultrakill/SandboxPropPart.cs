using System;
using Sandbox;
using UnityEngine;

// Token: 0x020003B3 RID: 947
public class SandboxPropPart : MonoBehaviour
{
	// Token: 0x06001590 RID: 5520 RVA: 0x000AEF8D File Offset: 0x000AD18D
	private void Awake()
	{
		if (this.parent == null)
		{
			this.parent = base.GetComponentInParent<SandboxSpawnableInstance>();
		}
	}

	// Token: 0x04001DE6 RID: 7654
	public SandboxSpawnableInstance parent;
}
