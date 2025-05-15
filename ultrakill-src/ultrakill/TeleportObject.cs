using System;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class TeleportObject : MonoBehaviour
{
	// Token: 0x06001A28 RID: 6696 RVA: 0x000D7E0A File Offset: 0x000D600A
	public void Teleport(Transform target)
	{
		target.position = this.position;
	}

	// Token: 0x040024A2 RID: 9378
	public Vector3 position;
}
