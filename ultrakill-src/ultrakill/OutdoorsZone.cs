using System;
using UnityEngine;

// Token: 0x0200032A RID: 810
public class OutdoorsZone : MonoBehaviour
{
	// Token: 0x060012C6 RID: 4806 RVA: 0x00095A60 File Offset: 0x00093C60
	private void Start()
	{
		if (!MonoSingleton<OutdoorLightMaster>.Instance || this.ignoreCheckers)
		{
			return;
		}
		Rigidbody rigidbody;
		if (base.TryGetComponent<Rigidbody>(out rigidbody))
		{
			foreach (Collider collider in base.GetComponentsInChildren<Collider>())
			{
				if (collider.attachedRigidbody && collider.attachedRigidbody == rigidbody)
				{
					MonoSingleton<OutdoorLightMaster>.Instance.outdoorsZonesCheckerable.Add(collider);
				}
			}
			return;
		}
		Collider collider2;
		if (base.TryGetComponent<Collider>(out collider2) && MonoSingleton<OutdoorLightMaster>.Instance && !MonoSingleton<OutdoorLightMaster>.Instance.outdoorsZonesCheckerable.Contains(collider2))
		{
			MonoSingleton<OutdoorLightMaster>.Instance.outdoorsZonesCheckerable.Add(collider2);
		}
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x00095B0C File Offset: 0x00093D0C
	private void OnDisable()
	{
		if (MonoSingleton<OutdoorLightMaster>.Instance && this.hasRequested > 0)
		{
			for (int i = this.hasRequested; i > 0; i--)
			{
				MonoSingleton<OutdoorLightMaster>.Instance.RemoveRequest();
			}
			this.hasRequested = 0;
		}
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x00095B50 File Offset: 0x00093D50
	private void OnTriggerEnter(Collider other)
	{
		if (MonoSingleton<OutdoorLightMaster>.Instance && other.gameObject.CompareTag("Player"))
		{
			if (this.hasRequested == 0)
			{
				MonoSingleton<OutdoorLightMaster>.Instance.AddRequest();
			}
			this.hasRequested++;
		}
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x00095B90 File Offset: 0x00093D90
	private void OnTriggerExit(Collider other)
	{
		if (MonoSingleton<OutdoorLightMaster>.Instance && other.gameObject.CompareTag("Player"))
		{
			if (this.hasRequested == 1)
			{
				MonoSingleton<OutdoorLightMaster>.Instance.RemoveRequest();
			}
			this.hasRequested--;
		}
	}

	// Token: 0x040019B9 RID: 6585
	private int hasRequested;

	// Token: 0x040019BA RID: 6586
	public bool ignoreCheckers;
}
