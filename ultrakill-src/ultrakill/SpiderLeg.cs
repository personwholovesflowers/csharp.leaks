using System;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class SpiderLeg : MonoBehaviour
{
	// Token: 0x0600182D RID: 6189 RVA: 0x000C5C9C File Offset: 0x000C3E9C
	private void Start()
	{
		this.target = base.transform.position;
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x000C5CB0 File Offset: 0x000C3EB0
	private void Update()
	{
		if (base.transform.position != this.target)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.target, Time.deltaTime * 10f);
		}
	}

	// Token: 0x040021F6 RID: 8694
	public Vector3 target;
}
