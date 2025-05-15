using System;
using Nest.Components;
using UnityEngine;

// Token: 0x020000E0 RID: 224
[Serializable]
public struct HammerConnection
{
	// Token: 0x040005B6 RID: 1462
	public string recipient;

	// Token: 0x040005B7 RID: 1463
	public Outputs output;

	// Token: 0x040005B8 RID: 1464
	public Inputs input;

	// Token: 0x040005B9 RID: 1465
	public NestInput nestInput;

	// Token: 0x040005BA RID: 1466
	public GameObject recipientObject;

	// Token: 0x040005BB RID: 1467
	public string parameter;

	// Token: 0x040005BC RID: 1468
	public float delay;

	// Token: 0x040005BD RID: 1469
	public bool onceOnly;
}
