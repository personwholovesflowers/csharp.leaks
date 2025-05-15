using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class FollowPlayer : MonoBehaviour
{
	// Token: 0x060004F3 RID: 1267 RVA: 0x0001CB2F File Offset: 0x0001AD2F
	private void LateUpdate()
	{
		base.transform.position = StanleyController.StanleyPosition + this.offset;
	}

	// Token: 0x040004CA RID: 1226
	public Vector3 offset = new Vector3(0f, 7.5f, 0f);
}
