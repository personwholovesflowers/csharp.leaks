using System;
using UnityEngine;

// Token: 0x0200028D RID: 653
public class ItemTrigger : MonoBehaviour
{
	// Token: 0x06000E6B RID: 3691 RVA: 0x0006B82C File Offset: 0x00069A2C
	private void OnDisable()
	{
		this.requests = 0;
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x0006B838 File Offset: 0x00069A38
	private void OnTriggerEnter(Collider other)
	{
		if (this.oneTime && this.activated)
		{
			return;
		}
		ItemIdentifier itemIdentifier;
		if ((this.dontRequireItemLayer || other.gameObject.layer == 22) && (other.attachedRigidbody ? other.attachedRigidbody.TryGetComponent<ItemIdentifier>(out itemIdentifier) : other.TryGetComponent<ItemIdentifier>(out itemIdentifier)) && itemIdentifier.itemType == this.targetType)
		{
			if (this.requests == 0)
			{
				this.activated = true;
				UltrakillEvent ultrakillEvent = this.onEvent;
				if (ultrakillEvent != null)
				{
					ultrakillEvent.Invoke(base.gameObject.name);
				}
				if (this.destroyActivator)
				{
					Object.Destroy(itemIdentifier.gameObject);
				}
				else if (this.disableActivator)
				{
					itemIdentifier.gameObject.SetActive(false);
				}
			}
			if (!this.destroyActivator && !this.disableActivator)
			{
				this.requests++;
			}
		}
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x0006B91C File Offset: 0x00069B1C
	private void OnTriggerExit(Collider other)
	{
		if (this.requests <= 0)
		{
			return;
		}
		ItemIdentifier itemIdentifier;
		if ((this.dontRequireItemLayer || other.gameObject.layer == 22) && (other.attachedRigidbody ? other.attachedRigidbody.TryGetComponent<ItemIdentifier>(out itemIdentifier) : other.TryGetComponent<ItemIdentifier>(out itemIdentifier)) && itemIdentifier.itemType == this.targetType)
		{
			this.requests--;
			if (this.requests == 0 && this.disableOnExit)
			{
				UltrakillEvent ultrakillEvent = this.onEvent;
				if (ultrakillEvent != null)
				{
					ultrakillEvent.Revert();
				}
				if (this.destroyActivator)
				{
					Object.Destroy(itemIdentifier.gameObject);
					return;
				}
				if (this.disableActivator)
				{
					itemIdentifier.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x04001327 RID: 4903
	public ItemType targetType;

	// Token: 0x04001328 RID: 4904
	public bool oneTime;

	// Token: 0x04001329 RID: 4905
	private bool activated;

	// Token: 0x0400132A RID: 4906
	public bool disableOnExit;

	// Token: 0x0400132B RID: 4907
	public bool disableActivator;

	// Token: 0x0400132C RID: 4908
	public bool destroyActivator;

	// Token: 0x0400132D RID: 4909
	public bool dontRequireItemLayer;

	// Token: 0x0400132E RID: 4910
	public UltrakillEvent onEvent;

	// Token: 0x0400132F RID: 4911
	private int requests;
}
