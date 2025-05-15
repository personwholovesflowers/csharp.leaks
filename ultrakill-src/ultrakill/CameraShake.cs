using System;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public class CameraShake : MonoBehaviour
{
	// Token: 0x06000320 RID: 800 RVA: 0x00012ED6 File Offset: 0x000110D6
	private void Start()
	{
		MonoSingleton<CameraController>.Instance.CameraShake(this.amount);
		Object.Destroy(this);
	}

	// Token: 0x040003DE RID: 990
	public float amount;
}
