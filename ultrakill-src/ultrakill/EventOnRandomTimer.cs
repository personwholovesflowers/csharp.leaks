using System;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class EventOnRandomTimer : MonoBehaviour
{
	// Token: 0x06000873 RID: 2163 RVA: 0x0003A475 File Offset: 0x00038675
	private void Start()
	{
		if (this.forceOnStart)
		{
			this.onTimer.Invoke("");
		}
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0003A48F File Offset: 0x0003868F
	private void OnEnable()
	{
		this.Randomize();
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0003A497 File Offset: 0x00038697
	private void Update()
	{
		if (this.timer > this.currentTarget)
		{
			this.onTimer.Invoke("");
			this.Randomize();
		}
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0003A4C4 File Offset: 0x000386C4
	private void Randomize()
	{
		this.timer = 0f;
		this.currentTarget = Random.Range((!this.activated && this.noMinimumOnFirst) ? 0f : this.timerMinimum, this.timerMaximum);
		this.activated = true;
	}

	// Token: 0x04000B3B RID: 2875
	public float timerMinimum;

	// Token: 0x04000B3C RID: 2876
	public float timerMaximum;

	// Token: 0x04000B3D RID: 2877
	private float currentTarget;

	// Token: 0x04000B3E RID: 2878
	private TimeSince timer;

	// Token: 0x04000B3F RID: 2879
	public bool forceOnStart;

	// Token: 0x04000B40 RID: 2880
	public bool noMinimumOnFirst;

	// Token: 0x04000B41 RID: 2881
	private bool activated;

	// Token: 0x04000B42 RID: 2882
	public UltrakillEvent onTimer;
}
