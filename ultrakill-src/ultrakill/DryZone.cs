using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class DryZone : MonoBehaviour
{
	// Token: 0x0600059E RID: 1438 RVA: 0x000274A1 File Offset: 0x000256A1
	private void Awake()
	{
		this.dzc = MonoSingleton<DryZoneController>.Instance;
		this.dzc.dryZones.Add(this);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x000274C0 File Offset: 0x000256C0
	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody != null)
		{
			this.cols.Add(other);
			this.dzc.AddCollider(other);
		}
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x000274E9 File Offset: 0x000256E9
	private void OnTriggerExit(Collider other)
	{
		if (this.cols.Remove(other))
		{
			this.dzc.RemoveCollider(other);
		}
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00027508 File Offset: 0x00025708
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		foreach (Collider collider in this.cols)
		{
			this.dzc.RemoveCollider(collider);
		}
		this.dzc.dryZones.Remove(this);
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00027588 File Offset: 0x00025788
	private void OnEnable()
	{
		this.dzc = MonoSingleton<DryZoneController>.Instance;
		foreach (Collider collider in this.cols)
		{
			this.dzc.AddCollider(collider);
		}
		this.dzc.dryZones.Add(this);
	}

	// Token: 0x040007D2 RID: 2002
	private HashSet<Collider> cols = new HashSet<Collider>();

	// Token: 0x040007D3 RID: 2003
	private DryZoneController dzc;
}
