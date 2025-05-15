using System;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class LightDistanceFade : MonoBehaviour
{
	// Token: 0x06000F9F RID: 3999 RVA: 0x00074246 File Offset: 0x00072446
	private void Start()
	{
		this.player = MonoSingleton<CameraController>.Instance.transform;
		this.lit = base.GetComponent<Light>();
		this.maxIntensity = this.lit.intensity;
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x00074278 File Offset: 0x00072478
	private void Update()
	{
		if (this.lit && this.player)
		{
			float num = Vector3.Distance(base.transform.position, this.player.position);
			if (num >= this.maxDistance)
			{
				this.lit.enabled = false;
				return;
			}
			this.lit.enabled = true;
			if (num <= this.minDistance)
			{
				this.lit.intensity = this.maxIntensity;
				return;
			}
			float num2 = this.maxDistance - this.minDistance;
			float num3 = num - this.minDistance;
			this.lit.intensity = Mathf.Pow((Mathf.Sqrt(num2) - Mathf.Sqrt(num3)) / Mathf.Sqrt(num2), 2f) * this.maxIntensity;
		}
	}

	// Token: 0x040014FF RID: 5375
	private Transform player;

	// Token: 0x04001500 RID: 5376
	private Light lit;

	// Token: 0x04001501 RID: 5377
	private float maxIntensity;

	// Token: 0x04001502 RID: 5378
	public float minDistance;

	// Token: 0x04001503 RID: 5379
	public float maxDistance;
}
