using System;
using System.Linq;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class BaitItem : MonoBehaviour
{
	// Token: 0x06000993 RID: 2451 RVA: 0x000427DC File Offset: 0x000409DC
	private void OnTriggerEnter(Collider other)
	{
		if (this.used)
		{
			return;
		}
		if (other.gameObject.layer != 4)
		{
			return;
		}
		Water component = other.GetComponent<Water>();
		if (component.fishDB == null)
		{
			return;
		}
		this.used = true;
		if (this.supportedWaters.Contains(component.fishDB))
		{
			Object.Instantiate<GameObject>(this.consumedPrefab, base.transform.position, Quaternion.identity);
			component.attractFish = this.attractFish;
			MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("A fish took the bait.", "", "", 0, false, false, true);
			Object.Destroy(base.gameObject);
			return;
		}
		if (this.silentFail)
		{
			return;
		}
		MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=red>This bait didn't work here!</color>", "", "", 0, false, false, true);
	}

	// Token: 0x04000C74 RID: 3188
	[SerializeField]
	private bool silentFail;

	// Token: 0x04000C75 RID: 3189
	[SerializeField]
	private GameObject consumedPrefab;

	// Token: 0x04000C76 RID: 3190
	[SerializeField]
	private FishObject[] attractFish;

	// Token: 0x04000C77 RID: 3191
	[SerializeField]
	private FishDB[] supportedWaters;

	// Token: 0x04000C78 RID: 3192
	private bool used;
}
