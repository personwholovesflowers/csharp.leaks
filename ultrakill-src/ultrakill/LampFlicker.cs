using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class LampFlicker : MonoBehaviour
{
	// Token: 0x06001D9C RID: 7580 RVA: 0x000F77B7 File Offset: 0x000F59B7
	private void Awake()
	{
		this.thisLight = base.GetComponent<Light>();
		this.baseIntensity = this.thisLight.intensity;
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x000F77D6 File Offset: 0x000F59D6
	private void OnEnable()
	{
		base.StartCoroutine(this.FlickerLamp());
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x000F77E5 File Offset: 0x000F59E5
	private IEnumerator FlickerLamp()
	{
		for (;;)
		{
			this.thisLight.intensity = this.baseIntensity * Random.Range(this.randomMin, this.randomMax);
			yield return new WaitForSeconds(Random.Range(this.randomSpeedMin, this.randomSpeedMax));
		}
		yield break;
	}

	// Token: 0x040029F5 RID: 10741
	public float randomSpeedMin;

	// Token: 0x040029F6 RID: 10742
	public float randomSpeedMax;

	// Token: 0x040029F7 RID: 10743
	public float randomMin;

	// Token: 0x040029F8 RID: 10744
	public float randomMax;

	// Token: 0x040029F9 RID: 10745
	private float baseIntensity;

	// Token: 0x040029FA RID: 10746
	private Light thisLight;
}
