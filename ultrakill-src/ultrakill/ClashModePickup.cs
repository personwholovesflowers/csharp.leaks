using System;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class ClashModePickup : MonoBehaviour
{
	// Token: 0x060003DD RID: 989 RVA: 0x00018A78 File Offset: 0x00016C78
	private void OnTriggerEnter(Collider other)
	{
		if (this.activated)
		{
			return;
		}
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && MonoSingleton<PlatformerMovement>.Instance && other.gameObject == MonoSingleton<PlatformerMovement>.Instance.gameObject)
		{
			this.activated = true;
			this.Activate();
			return;
		}
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS && MonoSingleton<NewMovement>.Instance && other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
		{
			MonoSingleton<PlayerTracker>.Instance.ChangeToPlatformer();
		}
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00018B08 File Offset: 0x00016D08
	private void Activate()
	{
		MonoSingleton<PlatformerMovement>.Instance.gameObject.SetActive(false);
		MonoSingleton<PlatformerMovement>.Instance.SnapCamera(new Vector3(0f, 5f, -5.5f), new Vector3(20f, 0f, 0f));
		MonoSingleton<PlatformerMovement>.Instance.platformerCamera.position = this.dancer.transform.position + new Vector3(0f, 5f, -5.5f);
		this.dancer.SetActive(true);
		GameProgressSaver.SetClashModeUnlocked(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x040004BB RID: 1211
	private bool activated;

	// Token: 0x040004BC RID: 1212
	[SerializeField]
	private GameObject dancer;
}
