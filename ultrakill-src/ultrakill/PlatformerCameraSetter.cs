using System;
using UnityEngine;

// Token: 0x0200033A RID: 826
public class PlatformerCameraSetter : MonoBehaviour
{
	// Token: 0x060012F9 RID: 4857 RVA: 0x00097098 File Offset: 0x00095298
	private void OnTriggerEnter(Collider other)
	{
		if (MonoSingleton<PlayerTracker>.Instance.playerType != PlayerType.Platformer || MonoSingleton<PlatformerMovement>.Instance == null)
		{
			return;
		}
		if (other.gameObject == MonoSingleton<PlatformerMovement>.Instance.gameObject)
		{
			MonoSingleton<PlatformerMovement>.Instance.cameraTargets.Add(new CameraTargetInfo(this.position, this.rotation, base.gameObject));
		}
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x00097100 File Offset: 0x00095300
	private void OnTriggerExit(Collider other)
	{
		if (MonoSingleton<PlayerTracker>.Instance.playerType != PlayerType.Platformer)
		{
			return;
		}
		if (other.gameObject == MonoSingleton<PlatformerMovement>.Instance.gameObject && MonoSingleton<PlatformerMovement>.Instance.cameraTargets.Count > 0)
		{
			for (int i = MonoSingleton<PlatformerMovement>.Instance.cameraTargets.Count - 1; i >= 0; i--)
			{
				if (MonoSingleton<PlatformerMovement>.Instance.cameraTargets[i] != null && MonoSingleton<PlatformerMovement>.Instance.cameraTargets[i].caller == base.gameObject)
				{
					MonoSingleton<PlatformerMovement>.Instance.cameraTargets.RemoveAt(i);
					return;
				}
			}
		}
	}

	// Token: 0x04001A18 RID: 6680
	public Vector3 position = new Vector3(0f, 7f, -5.5f);

	// Token: 0x04001A19 RID: 6681
	public Vector3 rotation = new Vector3(20f, 0f, 0f);
}
