using System;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class WeatherChangeTrigger : MonoBehaviour
{
	// Token: 0x06000AA9 RID: 2729 RVA: 0x00031A64 File Offset: 0x0002FC64
	private void Start()
	{
		this.weatherAnimator.enabled = true;
		this.weatherAnimator.SetBool("stormyTriggerEntered", false);
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x00031A83 File Offset: 0x0002FC83
	private void OnTriggerEnter()
	{
		this.weatherAnimator.SetBool("stormyTriggerEntered", true);
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00005444 File Offset: 0x00003644
	private void Update()
	{
	}

	// Token: 0x04000A8C RID: 2700
	public Animator weatherAnimator;
}
