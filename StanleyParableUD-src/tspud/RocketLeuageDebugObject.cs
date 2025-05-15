using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class RocketLeuageDebugObject : MonoBehaviour
{
	// Token: 0x060008DD RID: 2269 RVA: 0x0002A382 File Offset: 0x00028582
	private void Start()
	{
		if (RocketLeuageDebugButtons.Instance != null)
		{
			RocketLeuageDebugButtons.Instance.RegisterObject(this.id, base.gameObject);
		}
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0002A3A7 File Offset: 0x000285A7
	[ContextMenu("Set To GameObject Name")]
	private void SetToGameObjectName()
	{
		this.id = base.gameObject.name;
	}

	// Token: 0x040008AA RID: 2218
	public string id = "None";
}
