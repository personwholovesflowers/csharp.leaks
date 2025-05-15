using System;
using UnityEngine;

// Token: 0x020000FB RID: 251
[RequireComponent(typeof(Light))]
public class LightControl : MonoBehaviour
{
	// Token: 0x0600060F RID: 1551 RVA: 0x00021960 File Offset: 0x0001FB60
	private void Awake()
	{
		this.cachedLight = base.GetComponent<Light>();
		this.cachedShadows = this.cachedLight.shadows;
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0002197F File Offset: 0x0001FB7F
	public void SetRange(float range)
	{
		this.cachedLight.range = range;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0002198D File Offset: 0x0001FB8D
	public void UpdateRange(bool status)
	{
		this.cachedLight.range = (status ? this.defaultRange : this.lowEndRange);
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x000219AB File Offset: 0x0001FBAB
	public void UpdateMask(bool status)
	{
		this.cachedLight.cullingMask = (status ? this.defaultLayerMask : this.lowEndMask);
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x000219CE File Offset: 0x0001FBCE
	public void SetShadows(bool status)
	{
		if (status)
		{
			this.cachedLight.shadows = this.cachedShadows;
			return;
		}
		this.cachedLight.shadows = LightShadows.None;
	}

	// Token: 0x04000662 RID: 1634
	private Light cachedLight;

	// Token: 0x04000663 RID: 1635
	[SerializeField]
	private float defaultRange = 5f;

	// Token: 0x04000664 RID: 1636
	[SerializeField]
	private float lowEndRange = 3f;

	// Token: 0x04000665 RID: 1637
	[SerializeField]
	private LayerMask defaultLayerMask;

	// Token: 0x04000666 RID: 1638
	[SerializeField]
	private LayerMask lowEndMask;

	// Token: 0x04000667 RID: 1639
	private LightShadows cachedShadows;
}
