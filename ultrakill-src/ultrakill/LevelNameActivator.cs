using System;
using UnityEngine;

// Token: 0x020002BF RID: 703
public class LevelNameActivator : MonoBehaviour
{
	// Token: 0x06000F2B RID: 3883 RVA: 0x0007000E File Offset: 0x0006E20E
	private void Start()
	{
		this.col = base.GetComponent<Collider>();
		if (this.col == null || !this.col.isTrigger)
		{
			this.GoTime();
			return;
		}
		this.activateOnCollision = true;
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x00070045 File Offset: 0x0006E245
	private void OnTriggerEnter(Collider other)
	{
		if (this.activateOnCollision && other.gameObject.CompareTag("Player"))
		{
			this.GoTime();
		}
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x00070067 File Offset: 0x0006E267
	private void GoTime()
	{
		MonoSingleton<LevelNamePopup>.Instance.NameAppear();
		Object.Destroy(this);
	}

	// Token: 0x04001454 RID: 5204
	private Collider col;

	// Token: 0x04001455 RID: 5205
	private bool activateOnCollision;
}
