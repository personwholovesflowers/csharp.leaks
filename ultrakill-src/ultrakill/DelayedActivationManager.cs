using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class DelayedActivationManager : MonoSingleton<DelayedActivationManager>
{
	// Token: 0x060004FB RID: 1275 RVA: 0x00021E08 File Offset: 0x00020008
	private void Update()
	{
		if (this.activateCountdowns.Count == 0)
		{
			return;
		}
		for (int i = this.activateCountdowns.Count - 1; i >= 0; i--)
		{
			if (this.toActivate[i] == null)
			{
				this.toActivate.RemoveAt(i);
				this.activateCountdowns.RemoveAt(i);
			}
			else
			{
				this.activateCountdowns[i] = Mathf.MoveTowards(this.activateCountdowns[i], 0f, Time.deltaTime);
				if (this.activateCountdowns[i] == 0f)
				{
					this.toActivate[i].SetActive(true);
					this.toActivate.RemoveAt(i);
					this.activateCountdowns.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00021ED3 File Offset: 0x000200D3
	public void Add(GameObject target, float time)
	{
		this.toActivate.Add(target);
		this.activateCountdowns.Add(time);
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00021EF0 File Offset: 0x000200F0
	public void Remove(GameObject target)
	{
		if (this.toActivate.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.toActivate.Count; i++)
		{
			if (this.toActivate[i] == target)
			{
				this.toActivate.RemoveAt(i);
				this.activateCountdowns.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x040006EC RID: 1772
	private List<GameObject> toActivate = new List<GameObject>();

	// Token: 0x040006ED RID: 1773
	private List<float> activateCountdowns = new List<float>();
}
