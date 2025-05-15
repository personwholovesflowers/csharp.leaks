using System;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class SpiderLegsController : MonoBehaviour
{
	// Token: 0x06001837 RID: 6199 RVA: 0x000C5FB0 File Offset: 0x000C41B0
	private void Start()
	{
		this.spiderBody = base.transform.parent.GetChild(0).gameObject;
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x000C5FD0 File Offset: 0x000C41D0
	private void Update()
	{
		this.bodyRotV = this.spiderBody.transform.rotation.eulerAngles;
		this.bodyRotQ.eulerAngles = new Vector3(0f, this.bodyRotV.y, 0f);
		base.transform.SetPositionAndRotation(this.spiderBody.transform.position, this.bodyRotQ);
	}

	// Token: 0x040021FF RID: 8703
	private GameObject spiderBody;

	// Token: 0x04002200 RID: 8704
	private Vector3 bodyRotV;

	// Token: 0x04002201 RID: 8705
	private Quaternion bodyRotQ;
}
