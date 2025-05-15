using System;
using UnityEngine;

// Token: 0x02000488 RID: 1160
public class Torch : MonoBehaviour
{
	// Token: 0x06001A8E RID: 6798 RVA: 0x000DA841 File Offset: 0x000D8A41
	private void Start()
	{
		this.torchLight = base.GetComponentInChildren<Light>();
		this.originalPos = this.torchLight.transform.localPosition;
		this.itid = base.GetComponent<ItemIdentifier>();
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x000DA874 File Offset: 0x000D8A74
	private void Update()
	{
		if (this.pickedUp && this.torchLight && !this.itid.hooked)
		{
			this.torchLight.transform.position = MonoSingleton<PlayerTracker>.Instance.GetTarget().position;
			return;
		}
		if (this.torchLight.transform.localPosition != this.originalPos)
		{
			this.torchLight.transform.localPosition = this.originalPos;
		}
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000DA8F8 File Offset: 0x000D8AF8
	public void HitWith(GameObject target)
	{
		Flammable component = target.gameObject.GetComponent<Flammable>();
		if (component != null && !component.enemyOnly)
		{
			component.Burn(4f, false);
		}
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000C246A File Offset: 0x000C066A
	public void HitSurface(RaycastHit hit)
	{
		MonoSingleton<StainVoxelManager>.Instance.TryIgniteAt(hit.point, 3);
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x000DA92E File Offset: 0x000D8B2E
	public void PickUp()
	{
		this.pickedUp = true;
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x000DA937 File Offset: 0x000D8B37
	public void PutDown()
	{
		this.pickedUp = false;
		this.torchLight.transform.localPosition = this.originalPos;
	}

	// Token: 0x04002541 RID: 9537
	private Light torchLight;

	// Token: 0x04002542 RID: 9538
	private bool pickedUp;

	// Token: 0x04002543 RID: 9539
	private Vector3 originalPos;

	// Token: 0x04002544 RID: 9540
	private ItemIdentifier itid;
}
