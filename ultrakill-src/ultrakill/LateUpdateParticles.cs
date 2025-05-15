using System;
using UnityEngine;

// Token: 0x02000294 RID: 660
[DefaultExecutionOrder(11000)]
public class LateUpdateParticles : MonoBehaviour
{
	// Token: 0x06000E97 RID: 3735 RVA: 0x0006C5E1 File Offset: 0x0006A7E1
	private void Awake()
	{
		this.part = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x0006C5EF File Offset: 0x0006A7EF
	private void LateUpdate()
	{
		if (this.part.isPlaying)
		{
			this.beenStarted = true;
		}
		if (!this.beenStarted)
		{
			return;
		}
		this.part.Simulate(Time.deltaTime, true, false, false);
	}

	// Token: 0x04001359 RID: 4953
	private ParticleSystem part;

	// Token: 0x0400135A RID: 4954
	private bool beenStarted;
}
