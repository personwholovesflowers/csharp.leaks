using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class ChangeLayer : MonoBehaviour
{
	// Token: 0x0600034C RID: 844 RVA: 0x00014BE7 File Offset: 0x00012DE7
	private void Start()
	{
		if (this.activateOnEnable)
		{
			base.Invoke("Change", this.delay);
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00014C02 File Offset: 0x00012E02
	public void Change()
	{
		this.Change(this.layer);
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00014C10 File Offset: 0x00012E10
	public void Change(int targetLayer)
	{
		if (this.oneTime && this.activated)
		{
			return;
		}
		this.activated = true;
		if (this.target == null)
		{
			this.target = base.gameObject;
		}
		if (!this.includeChildren)
		{
			base.gameObject.layer = targetLayer;
			return;
		}
		Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = targetLayer;
		}
	}

	// Token: 0x04000427 RID: 1063
	public GameObject target;

	// Token: 0x04000428 RID: 1064
	public int layer;

	// Token: 0x04000429 RID: 1065
	public float delay;

	// Token: 0x0400042A RID: 1066
	public bool includeChildren;

	// Token: 0x0400042B RID: 1067
	public bool oneTime = true;

	// Token: 0x0400042C RID: 1068
	[HideInInspector]
	public bool activated;

	// Token: 0x0400042D RID: 1069
	public bool activateOnEnable = true;
}
