using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class DryZoneController : MonoSingleton<DryZoneController>
{
	// Token: 0x060005A4 RID: 1444 RVA: 0x00027614 File Offset: 0x00025814
	public void AddCollider(Collider other)
	{
		int num;
		if (!this.colliderCalls.TryGetValue(other, out num))
		{
			this.colliderCalls.Add(other, 1);
			if (this.waters.Count <= 0)
			{
				return;
			}
			using (HashSet<Water>.Enumerator enumerator = this.waters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Water water = enumerator.Current;
					water.EnterDryZone(other);
				}
				return;
			}
		}
		Dictionary<Collider, int> dictionary = this.colliderCalls;
		num = dictionary[other];
		dictionary[other] = num + 1;
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x000276AC File Offset: 0x000258AC
	public void RemoveCollider(Collider other)
	{
		int num;
		if (!this.colliderCalls.TryGetValue(other, out num))
		{
			return;
		}
		if (num > 1)
		{
			Dictionary<Collider, int> dictionary = this.colliderCalls;
			int num2 = dictionary[other];
			dictionary[other] = num2 - 1;
			return;
		}
		this.colliderCalls.Remove(other);
		if (this.waters.Count > 0)
		{
			foreach (Water water in this.waters)
			{
				water.ExitDryZone(other);
			}
		}
	}

	// Token: 0x040007D4 RID: 2004
	public HashSet<Water> waters = new HashSet<Water>();

	// Token: 0x040007D5 RID: 2005
	public Dictionary<Collider, int> colliderCalls = new Dictionary<Collider, int>();

	// Token: 0x040007D6 RID: 2006
	public HashSet<DryZone> dryZones = new HashSet<DryZone>();
}
