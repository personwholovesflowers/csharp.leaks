using System;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class FishCooker : MonoBehaviour
{
	// Token: 0x060009AB RID: 2475 RVA: 0x00043149 File Offset: 0x00041349
	private void Awake()
	{
		if (this.unusable)
		{
			this.timeSinceLastError = 0f;
		}
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00043164 File Offset: 0x00041364
	private void OnTriggerEnter(Collider other)
	{
		FishObjectReference fishObjectReference;
		if (!other.TryGetComponent<FishObjectReference>(out fishObjectReference))
		{
			return;
		}
		if (this.unusable)
		{
			if (this.timeSinceLastError > 2f)
			{
				this.timeSinceLastError = 0f;
				MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("Too small for this fish.\n:^(", "", "", 0, false, false, true);
			}
			return;
		}
		if (fishObjectReference.fishObject == this.cookedFish || fishObjectReference.fishObject == this.failedFish)
		{
			return;
		}
		bool flag = MonoSingleton<FishManager>.Instance.recognizedFishes[this.cookedFish];
		GameObject gameObject = FishingRodWeapon.CreateFishPickup(this.fishPickupTemplate, fishObjectReference.fishObject.canBeCooked ? this.cookedFish : this.failedFish, false, false);
		if (!fishObjectReference.fishObject.canBeCooked)
		{
			MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("Cooking failed.", "", "", 0, false, false, true);
		}
		gameObject.transform.SetPositionAndRotation(base.transform.position, Quaternion.identity);
		gameObject.GetComponent<Rigidbody>().velocity = (MonoSingleton<NewMovement>.Instance.transform.position - base.transform.position).normalized * 18f + Vector3.up * 10f;
		Object.Instantiate<GameObject>(this.cookedSound, base.transform.position, Quaternion.identity);
		if (this.cookedParticles)
		{
			Object.Instantiate<GameObject>(this.cookedParticles, base.transform.position, Quaternion.identity);
		}
		Object.Destroy(fishObjectReference.gameObject);
	}

	// Token: 0x04000C92 RID: 3218
	[SerializeField]
	private bool unusable;

	// Token: 0x04000C93 RID: 3219
	private TimeSince timeSinceLastError;

	// Token: 0x04000C94 RID: 3220
	[SerializeField]
	private ItemIdentifier fishPickupTemplate;

	// Token: 0x04000C95 RID: 3221
	[SerializeField]
	private FishObject cookedFish;

	// Token: 0x04000C96 RID: 3222
	[SerializeField]
	private FishObject failedFish;

	// Token: 0x04000C97 RID: 3223
	[SerializeField]
	private GameObject cookedSound;

	// Token: 0x04000C98 RID: 3224
	[SerializeField]
	private GameObject cookedParticles;
}
