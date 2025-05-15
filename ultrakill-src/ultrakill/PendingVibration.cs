using System;
using UnityEngine;

// Token: 0x0200039C RID: 924
public class PendingVibration
{
	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06001541 RID: 5441 RVA: 0x000ADA4F File Offset: 0x000ABC4F
	public float Duration
	{
		get
		{
			return MonoSingleton<RumbleManager>.Instance.ResolveDuration(this.key);
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06001542 RID: 5442 RVA: 0x000ADA61 File Offset: 0x000ABC61
	public float Intensity
	{
		get
		{
			return Mathf.Clamp01(MonoSingleton<RumbleManager>.Instance.ResolveIntensity(this.key) * this.intensityMultiplier);
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06001543 RID: 5443 RVA: 0x000ADA7F File Offset: 0x000ABC7F
	public bool IsFinished
	{
		get
		{
			return this.timeSinceStart >= this.Duration;
		}
	}

	// Token: 0x04001D7B RID: 7547
	public TimeSince timeSinceStart;

	// Token: 0x04001D7C RID: 7548
	public RumbleKey key;

	// Token: 0x04001D7D RID: 7549
	public float intensityMultiplier = 1f;

	// Token: 0x04001D7E RID: 7550
	public bool isTracking;

	// Token: 0x04001D7F RID: 7551
	public GameObject trackedObject;
}
