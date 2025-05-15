using System;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class ItemIdentifier : MonoBehaviour
{
	// Token: 0x06000E5F RID: 3679 RVA: 0x0006B058 File Offset: 0x00069258
	public ItemIdentifier CreateCopy()
	{
		if (this == null)
		{
			return null;
		}
		ItemIdentifier itemIdentifier = Object.Instantiate<ItemIdentifier>(this);
		itemIdentifier.infiniteSource = false;
		itemIdentifier.pickedUp = false;
		itemIdentifier.beenPickedUp = false;
		itemIdentifier.hooked = false;
		return itemIdentifier;
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x0006B087 File Offset: 0x00069287
	private void PickUp()
	{
		UltrakillEvent ultrakillEvent = this.onPickUp;
		if (ultrakillEvent == null)
		{
			return;
		}
		ultrakillEvent.Invoke("");
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x0006B09E File Offset: 0x0006929E
	private void PutDown()
	{
		UltrakillEvent ultrakillEvent = this.onPutDown;
		if (ultrakillEvent == null)
		{
			return;
		}
		ultrakillEvent.Invoke("");
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x0006B0B5 File Offset: 0x000692B5
	public void ForcePutDown(ItemPlaceZone target)
	{
		if (MonoSingleton<FistControl>.Instance.currentPunch.heldItem == this)
		{
			MonoSingleton<PlayerUtilities>.Instance.PlaceHeldObject(target);
		}
	}

	// Token: 0x04001306 RID: 4870
	public bool infiniteSource;

	// Token: 0x04001307 RID: 4871
	public bool dropOnHit;

	// Token: 0x04001308 RID: 4872
	public bool pickedUp;

	// Token: 0x04001309 RID: 4873
	[HideInInspector]
	public bool beenPickedUp;

	// Token: 0x0400130A RID: 4874
	public bool reverseTransformSettings;

	// Token: 0x0400130B RID: 4875
	public Vector3 putDownPosition;

	// Token: 0x0400130C RID: 4876
	public Vector3 putDownRotation;

	// Token: 0x0400130D RID: 4877
	public Vector3 putDownScale = Vector3.one;

	// Token: 0x0400130E RID: 4878
	public GameObject pickUpSound;

	// Token: 0x0400130F RID: 4879
	public ItemType itemType;

	// Token: 0x04001310 RID: 4880
	public bool noHoldingAnimation;

	// Token: 0x04001311 RID: 4881
	[HideInInspector]
	public bool hooked;

	// Token: 0x04001312 RID: 4882
	[HideInInspector]
	public ItemPlaceZone ipz;

	// Token: 0x04001313 RID: 4883
	public UltrakillEvent onPickUp;

	// Token: 0x04001314 RID: 4884
	public UltrakillEvent onPutDown;
}
