using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200022A RID: 554
public class Glass : MonoBehaviour
{
	// Token: 0x06000BDE RID: 3038 RVA: 0x00053718 File Offset: 0x00051918
	public void Shatter()
	{
		this.cols = base.GetComponentsInChildren<Collider>();
		base.gameObject.layer = 17;
		this.broken = true;
		this.glasses = base.transform.GetComponentsInChildren<Transform>();
		foreach (Transform transform in this.glasses)
		{
			if (transform.gameObject != base.gameObject)
			{
				Object.Destroy(transform.gameObject);
			}
		}
		foreach (Collider collider in this.cols)
		{
			if (!collider.isTrigger)
			{
				collider.enabled = false;
			}
		}
		using (List<GameObject>.Enumerator enumerator = this.enemies.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GroundCheckEnemy groundCheckEnemy;
				if (enumerator.Current.TryGetComponent<GroundCheckEnemy>(out groundCheckEnemy))
				{
					this.kills++;
				}
			}
		}
		BloodstainParent bloodstainParent;
		if (base.TryGetComponent<BloodstainParent>(out bloodstainParent))
		{
			Object.Destroy(bloodstainParent);
		}
		base.Invoke("BecomeObstacle", 0.5f);
		Object.Instantiate<GameObject>(this.shatterParticle, base.transform);
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x00053844 File Offset: 0x00051A44
	private void OnTriggerEnter(Collider other)
	{
		if (this.broken)
		{
			return;
		}
		if (other.gameObject.layer == 20 && !this.enemies.Contains(other.gameObject))
		{
			this.enemies.Add(other.gameObject);
		}
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x00053882 File Offset: 0x00051A82
	private void OnTriggerExit(Collider other)
	{
		if (this.broken)
		{
			return;
		}
		if (other.gameObject.layer == 20 && this.enemies.Contains(other.gameObject))
		{
			this.enemies.Remove(other.gameObject);
		}
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x000538C4 File Offset: 0x00051AC4
	private void BecomeObstacle()
	{
		NavMeshObstacle component = base.GetComponent<NavMeshObstacle>();
		if (this.wall)
		{
			component.carving = false;
			component.enabled = false;
		}
		else
		{
			component.enabled = true;
			foreach (Collider collider in this.cols)
			{
				if (collider != null && !collider.isTrigger)
				{
					collider.enabled = false;
				}
			}
		}
		if (this.kills >= 3)
		{
			StatsManager instance = MonoSingleton<StatsManager>.Instance;
			if (instance.maxGlassKills < this.kills)
			{
				instance.maxGlassKills = this.kills;
			}
		}
		base.enabled = false;
	}

	// Token: 0x04000F78 RID: 3960
	public bool broken;

	// Token: 0x04000F79 RID: 3961
	public bool wall;

	// Token: 0x04000F7A RID: 3962
	private Transform[] glasses;

	// Token: 0x04000F7B RID: 3963
	public GameObject shatterParticle;

	// Token: 0x04000F7C RID: 3964
	private int kills;

	// Token: 0x04000F7D RID: 3965
	private Collider[] cols;

	// Token: 0x04000F7E RID: 3966
	private List<GameObject> enemies = new List<GameObject>();
}
