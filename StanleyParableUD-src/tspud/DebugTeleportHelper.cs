using System;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class DebugTeleportHelper : MonoBehaviour
{
	// Token: 0x0600040E RID: 1038 RVA: 0x0001920B File Offset: 0x0001740B
	[ContextMenu("Teleport Stanley")]
	private void TeleportStanley()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		StanleyController.Instance.transform.position = base.transform.position;
	}
}
