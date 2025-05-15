using System;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class StatueActivator : MonoBehaviour
{
	// Token: 0x06001913 RID: 6419 RVA: 0x000CD39A File Offset: 0x000CB59A
	private void Start()
	{
		base.transform.parent.GetComponentInChildren<StatueFake>().Activate();
		Object.Destroy(this);
	}
}
