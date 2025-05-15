using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class DisablePowerUp : MonoBehaviour
{
	// Token: 0x06000526 RID: 1318 RVA: 0x000225C3 File Offset: 0x000207C3
	private void Start()
	{
		if (MonoSingleton<PowerUpMeter>.Instance.juice > 0f && !InfinitePowerUps.Enabled)
		{
			MonoSingleton<PowerUpMeter>.Instance.EndPowerUp();
		}
	}
}
