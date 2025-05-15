using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class BloodUnderwaterChecker : MonoBehaviour
{
	// Token: 0x060002A4 RID: 676 RVA: 0x0000F36F File Offset: 0x0000D56F
	private void OnEnable()
	{
		this.dzc = MonoSingleton<DryZoneController>.Instance;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0000F37C File Offset: 0x0000D57C
	private void OnTriggerEnter(Collider other)
	{
		if (this.cancelled)
		{
			return;
		}
		if (other.gameObject.layer == 4)
		{
			Vector3 position = base.transform.position;
			Vector3 vector = new Vector3(position.x, position.y + 1.5f, position.z);
			if (Vector3.Distance(other.ClosestPointOnBounds(vector), vector) < 0.5f)
			{
				if (this.dzc.dryZones != null && this.dzc.dryZones.Count > 0)
				{
					Collider[] array = Physics.OverlapSphere(position, 0.01f, 65536, QueryTriggerInteraction.Collide);
					for (int i = 0; i < array.Length; i++)
					{
						DryZone dryZone;
						if (array[i].TryGetComponent<DryZone>(out dryZone))
						{
							base.gameObject.SetActive(false);
							this.cancelled = true;
							return;
						}
					}
				}
				GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Body, true, false, false, null, false);
				if (gore)
				{
					Bloodsplatter component = base.transform.parent.GetComponent<Bloodsplatter>();
					Bloodsplatter component2 = gore.GetComponent<Bloodsplatter>();
					if (component2 && component)
					{
						component2.hpAmount = component.hpAmount;
						component2.fromExplosion = component.fromExplosion;
						if (component.ready)
						{
							component2.GetReady();
						}
					}
					gore.transform.position = base.transform.position;
					GoreZone componentInParent = base.GetComponentInParent<GoreZone>();
					if (componentInParent != null && componentInParent.goreZone != null)
					{
						gore.transform.SetParent(componentInParent.goreZone, true);
					}
					gore.SetActive(true);
					base.transform.parent.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x04000327 RID: 807
	private bool cancelled;

	// Token: 0x04000328 RID: 808
	private DryZoneController dzc;
}
