using System;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class ForceStayCrouchedTrigger : MonoBehaviour
{
	// Token: 0x0600095B RID: 2395 RVA: 0x0002C284 File Offset: 0x0002A484
	private void OnTriggerEnter(Collider col)
	{
		if (!col.CompareTag("Player"))
		{
			return;
		}
		if (this.forceCrouchRegardless)
		{
			StanleyController.Instance.ForceCrouched = true;
			return;
		}
		StanleyController.Instance.ForceStayCrouched = true;
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0002C284 File Offset: 0x0002A484
	private void OnTriggerStay(Collider col)
	{
		if (!col.CompareTag("Player"))
		{
			return;
		}
		if (this.forceCrouchRegardless)
		{
			StanleyController.Instance.ForceCrouched = true;
			return;
		}
		StanleyController.Instance.ForceStayCrouched = true;
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0002C2B3 File Offset: 0x0002A4B3
	private void OnTriggerExit(Collider col)
	{
		if (!col.CompareTag("Player"))
		{
			return;
		}
		if (this.forceCrouchRegardless)
		{
			StanleyController.Instance.ForceCrouched = false;
			return;
		}
		StanleyController.Instance.ForceStayCrouched = false;
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0002C2E2 File Offset: 0x0002A4E2
	private void OnDestroy()
	{
		if (StanleyController.Instance != null)
		{
			StanleyController.Instance.ForceCrouched = false;
			StanleyController.Instance.ForceStayCrouched = false;
		}
	}

	// Token: 0x0400093B RID: 2363
	public bool forceCrouchRegardless;
}
