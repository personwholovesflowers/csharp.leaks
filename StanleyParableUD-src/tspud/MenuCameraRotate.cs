using System;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class MenuCameraRotate : MonoBehaviour
{
	// Token: 0x06000733 RID: 1843 RVA: 0x0002581A File Offset: 0x00023A1A
	private void Awake()
	{
		this.seed = Random.value;
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00025828 File Offset: 0x00023A28
	private void Update()
	{
		base.transform.localRotation = Quaternion.Euler(new Vector3(this.maximumAngularShake.x * (Mathf.PerlinNoise(this.seed + 3f, Time.time * this.frequency) * 2f - 1f), this.maximumAngularShake.y * (Mathf.PerlinNoise(this.seed + 4f, Time.time * this.frequency) * 2f - 1f), this.maximumAngularShake.z * (Mathf.PerlinNoise(this.seed + 5f, Time.time * this.frequency) * 2f - 1f)) * this.intensity);
	}

	// Token: 0x0400075A RID: 1882
	public float frequency = 0.1f;

	// Token: 0x0400075B RID: 1883
	public float intensity = 0.5f;

	// Token: 0x0400075C RID: 1884
	private float seed;

	// Token: 0x0400075D RID: 1885
	private Vector3 maximumAngularShake = Vector3.one * 2f;
}
