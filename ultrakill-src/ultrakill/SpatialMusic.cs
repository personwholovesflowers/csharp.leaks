using System;
using UnityEngine;

// Token: 0x02000420 RID: 1056
public class SpatialMusic : MonoBehaviour
{
	// Token: 0x060017EB RID: 6123 RVA: 0x000C31C0 File Offset: 0x000C13C0
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.hiPass = base.GetComponent<AudioHighPassFilter>();
		this.target = MonoSingleton<CameraController>.Instance.transform;
		if (this.hiPass)
		{
			this.hiPassDefaultFrequency = this.hiPass.cutoffFrequency;
		}
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000C3214 File Offset: 0x000C1414
	private void Update()
	{
		float num = (Mathf.Clamp(Vector3.Distance(base.transform.position, this.target.position), this.minDistance, this.maxDistance) - this.minDistance) / (this.maxDistance - this.minDistance);
		this.aud.spatialBlend = num;
		if (this.hiPass)
		{
			this.hiPass.cutoffFrequency = Mathf.Lerp(0f, this.hiPassDefaultFrequency, num);
		}
	}

	// Token: 0x0400215B RID: 8539
	public float minDistance;

	// Token: 0x0400215C RID: 8540
	public float maxDistance;

	// Token: 0x0400215D RID: 8541
	private AudioHighPassFilter hiPass;

	// Token: 0x0400215E RID: 8542
	private float hiPassDefaultFrequency;

	// Token: 0x0400215F RID: 8543
	private AudioSource aud;

	// Token: 0x04002160 RID: 8544
	private Transform target;
}
