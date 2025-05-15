using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class EnemyCooldowns : MonoSingleton<EnemyCooldowns>
{
	// Token: 0x0600079F RID: 1951 RVA: 0x00032B98 File Offset: 0x00030D98
	private void Start()
	{
		this.SlowUpdate();
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00032BA0 File Offset: 0x00030DA0
	private void Update()
	{
		if (this.virtueCooldown > 0f)
		{
			this.virtueCooldown = Mathf.MoveTowards(this.virtueCooldown, 0f, Time.deltaTime);
		}
		if (this.ferrymanCooldown > 0f)
		{
			this.ferrymanCooldown = Mathf.MoveTowards(this.ferrymanCooldown, 0f, Time.deltaTime);
		}
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x00032C00 File Offset: 0x00030E00
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 10f);
		for (int i = this.currentVirtues.Count - 1; i >= 0; i--)
		{
			if (this.currentVirtues[i] == null || !this.currentVirtues[i].gameObject.activeInHierarchy)
			{
				this.currentVirtues.RemoveAt(i);
			}
		}
		for (int j = this.ferrymen.Count - 1; j >= 0; j--)
		{
			if (this.ferrymen[j] == null || !this.ferrymen[j].gameObject.activeInHierarchy)
			{
				this.ferrymen.RemoveAt(j);
			}
		}
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00032CBD File Offset: 0x00030EBD
	public void AddVirtue(Drone drn)
	{
		if (this.currentVirtues.Count <= 0 || !this.currentVirtues.Contains(drn))
		{
			this.currentVirtues.Add(drn);
		}
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x00032CE7 File Offset: 0x00030EE7
	public void RemoveVirtue(Drone drn)
	{
		if (this.currentVirtues.Count > 0 && this.currentVirtues.Contains(drn))
		{
			this.currentVirtues.Remove(drn);
		}
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x00032D12 File Offset: 0x00030F12
	public void AddFerryman(Ferryman fm)
	{
		if (this.ferrymen.Count <= 0 || !this.ferrymen.Contains(fm))
		{
			this.ferrymen.Add(fm);
		}
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x00032D3C File Offset: 0x00030F3C
	public void RemoveFerryman(Ferryman fm)
	{
		if (this.ferrymen.Count > 0 && this.ferrymen.Contains(fm))
		{
			this.ferrymen.Remove(fm);
		}
	}

	// Token: 0x040009E8 RID: 2536
	public float virtueCooldown;

	// Token: 0x040009E9 RID: 2537
	public float ferrymanCooldown;

	// Token: 0x040009EA RID: 2538
	public List<Drone> currentVirtues = new List<Drone>();

	// Token: 0x040009EB RID: 2539
	public List<Ferryman> ferrymen = new List<Ferryman>();
}
