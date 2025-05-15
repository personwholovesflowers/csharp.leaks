using System;
using UnityEngine;

// Token: 0x020001B7 RID: 439
[DisallowMultipleComponent]
public sealed class ParticleSystemStoppedMessage : MessageDispatcher
{
	// Token: 0x060008E9 RID: 2281 RVA: 0x0003A598 File Offset: 0x00038798
	private void OnParticleSystemStopped()
	{
		base.Handler.Invoke();
	}
}
