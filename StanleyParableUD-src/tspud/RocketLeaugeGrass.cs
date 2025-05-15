using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class RocketLeaugeGrass : MonoBehaviour
{
	// Token: 0x060008D1 RID: 2257 RVA: 0x00005444 File Offset: 0x00003644
	private void Start()
	{
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0002A144 File Offset: 0x00028344
	[ContextMenu("Do Update Step")]
	private void Update()
	{
		if (this.followTarget != null)
		{
			Vector3 position = this.followTarget.transform.position;
			position.x -= Mathf.Repeat(position.x, this.step);
			position.z -= Mathf.Repeat(position.z, this.step);
			position.y = base.transform.position.y;
			base.transform.position = position;
		}
	}

	// Token: 0x0400089F RID: 2207
	public string followTargetTag;

	// Token: 0x040008A0 RID: 2208
	private GameObject followTarget;

	// Token: 0x040008A1 RID: 2209
	public float step = 0.5f;
}
