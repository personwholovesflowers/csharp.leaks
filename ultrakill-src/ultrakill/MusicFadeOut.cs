using System;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class MusicFadeOut : MonoBehaviour
{
	// Token: 0x060011BB RID: 4539 RVA: 0x0008A274 File Offset: 0x00088474
	private void Start()
	{
		Collider collider;
		if (base.TryGetComponent<Collider>(out collider))
		{
			this.colliderless = false;
			return;
		}
		MonoSingleton<MusicManager>.Instance.off = true;
		if (this.forceOff)
		{
			MonoSingleton<MusicManager>.Instance.forcedOff = true;
		}
		if (this.oneTime)
		{
			Object.Destroy(this);
		}
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x0008A2C0 File Offset: 0x000884C0
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
		{
			MonoSingleton<MusicManager>.Instance.off = true;
			if (this.forceOff)
			{
				MonoSingleton<MusicManager>.Instance.forcedOff = true;
			}
			if (this.oneTime)
			{
				Object.Destroy(this);
			}
		}
	}

	// Token: 0x0400182E RID: 6190
	public bool forceOff;

	// Token: 0x0400182F RID: 6191
	public bool oneTime = true;

	// Token: 0x04001830 RID: 6192
	private bool colliderless = true;
}
