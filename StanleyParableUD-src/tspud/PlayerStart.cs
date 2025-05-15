using System;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class PlayerStart : HammerEntity
{
	// Token: 0x06000970 RID: 2416 RVA: 0x0002C5AC File Offset: 0x0002A7AC
	private void Start()
	{
		this.Respawn();
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0002C5B4 File Offset: 0x0002A7B4
	public void Respawn()
	{
		StanleyController.TeleportType teleportType = StanleyController.TeleportType.PlayerStartMaster;
		if (!this.isMaster)
		{
			teleportType = StanleyController.TeleportType.PlayerStart;
		}
		StanleyController.Instance.Teleport(teleportType, base.transform.position, -base.transform.up, true, false, true, base.transform);
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0002C5FC File Offset: 0x0002A7FC
	private void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		float num = 0.64f;
		for (int i = 0; i < 10; i++)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(Vector3.forward * Mathf.Lerp(num * 0.75f, num, (float)i / 9f), Vector3.down * 0.25f);
		}
		Gizmos.color = Color.green;
		Gizmos.DrawCube(Vector3.zero + Vector3.forward * (num / 2f), new Vector3(0.05f, 0.05f, num));
	}

	// Token: 0x0400094E RID: 2382
	public bool isMaster;
}
