using System;
using UnityEngine;

// Token: 0x02000430 RID: 1072
public class SpiderLegLines : MonoBehaviour
{
	// Token: 0x06001830 RID: 6192 RVA: 0x000C5D01 File Offset: 0x000C3F01
	private void Start()
	{
		this.body = base.transform.parent.GetChild(0).gameObject;
		this.lr = base.GetComponent<LineRenderer>();
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x000C5D2C File Offset: 0x000C3F2C
	private void Update()
	{
		this.lr.SetPosition(0, this.body.transform.position);
		this.lr.SetPosition(1, base.transform.position);
		this.lr.SetPosition(2, this.legEnd.transform.position);
	}

	// Token: 0x040021F7 RID: 8695
	private GameObject body;

	// Token: 0x040021F8 RID: 8696
	public GameObject legEnd;

	// Token: 0x040021F9 RID: 8697
	private LineRenderer lr;
}
