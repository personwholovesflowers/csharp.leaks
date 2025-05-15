using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public class WaterObject
{
	// Token: 0x04002707 RID: 9991
	public GameObject rbGO;

	// Token: 0x04002708 RID: 9992
	public Rigidbody Rb;

	// Token: 0x04002709 RID: 9993
	public Collider Col;

	// Token: 0x0400270A RID: 9994
	public bool IsUWC;

	// Token: 0x0400270B RID: 9995
	public bool IsPlayer;

	// Token: 0x0400270C RID: 9996
	public bool IsEnemy;

	// Token: 0x0400270D RID: 9997
	public int Layer;

	// Token: 0x0400270E RID: 9998
	public EnemyIdentifier EID;

	// Token: 0x0400270F RID: 9999
	public Vector3 EnterVelocity;

	// Token: 0x04002710 RID: 10000
	public GameObject BubbleEffect;

	// Token: 0x04002711 RID: 10001
	public Transform ContinuousSplashEffect;

	// Token: 0x04002712 RID: 10002
	public AudioSource[] AudioSources;

	// Token: 0x04002713 RID: 10003
	public List<AudioLowPassFilter> LowPassFilters = new List<AudioLowPassFilter>();
}
