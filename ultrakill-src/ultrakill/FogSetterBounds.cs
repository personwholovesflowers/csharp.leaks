using System;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class FogSetterBounds : MonoBehaviour
{
	// Token: 0x06000A59 RID: 2649 RVA: 0x00048EF2 File Offset: 0x000470F2
	private void Start()
	{
		this.col = base.GetComponent<Collider>();
		this.fogMaxDistance = RenderSettings.fogEndDistance - RenderSettings.fogStartDistance;
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00048F14 File Offset: 0x00047114
	private void Update()
	{
		this.closestPoint = this.col.ClosestPoint(MonoSingleton<CameraController>.Instance.transform.position);
		RenderSettings.fogStartDistance = Vector3.Distance(MonoSingleton<CameraController>.Instance.transform.position, this.closestPoint);
		RenderSettings.fogEndDistance = RenderSettings.fogStartDistance + this.fogMaxDistance;
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00048F71 File Offset: 0x00047171
	public void ChangeDistance()
	{
		this.ChangeDistance(RenderSettings.fogEndDistance - RenderSettings.fogStartDistance);
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x00048F84 File Offset: 0x00047184
	public void ChangeDistance(float min, float max)
	{
		this.ChangeDistance(max - min);
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x00048F8F File Offset: 0x0004718F
	public void ChangeDistance(float newDistance)
	{
		this.fogMaxDistance = newDistance;
	}

	// Token: 0x04000DBB RID: 3515
	private Collider col;

	// Token: 0x04000DBC RID: 3516
	private float fogMaxDistance;

	// Token: 0x04000DBD RID: 3517
	private Vector3 closestPoint;
}
