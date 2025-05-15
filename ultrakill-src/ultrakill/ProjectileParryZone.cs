using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000361 RID: 865
public class ProjectileParryZone : MonoBehaviour
{
	// Token: 0x06001412 RID: 5138 RVA: 0x000A1648 File Offset: 0x0009F848
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14 && other.gameObject.GetComponentInChildren<Projectile>() != null)
		{
			this.projs.Add(other.gameObject);
			MeshRenderer component = other.GetComponent<MeshRenderer>();
			if (component != null && component.sharedMaterial == this.origMat)
			{
				component.material = this.newMat;
			}
		}
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000A16B8 File Offset: 0x0009F8B8
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 14 && this.projs.Contains(other.gameObject))
		{
			this.projs.Remove(other.gameObject);
			MeshRenderer component = other.GetComponent<MeshRenderer>();
			if (component != null && component.sharedMaterial == this.origMat)
			{
				component.material = this.origMat;
			}
		}
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x000A1728 File Offset: 0x0009F928
	public Projectile CheckParryZone()
	{
		Projectile projectile = null;
		float num = 100f;
		List<GameObject> list = new List<GameObject>();
		if (this.projs.Count > 0)
		{
			foreach (GameObject gameObject in this.projs)
			{
				if (gameObject != null && gameObject.activeInHierarchy && Vector3.Distance(base.transform.parent.position, gameObject.transform.position) < num)
				{
					projectile = gameObject.GetComponentInChildren<Projectile>();
					if (projectile != null && !projectile.undeflectable)
					{
						num = Vector3.Distance(base.transform.parent.position, gameObject.transform.position);
					}
					else
					{
						list.Add(gameObject);
					}
				}
				else if (gameObject == null || !gameObject.activeInHierarchy)
				{
					list.Add(gameObject);
				}
			}
		}
		if (list.Count > 0)
		{
			foreach (GameObject gameObject2 in list)
			{
				this.projs.Remove(gameObject2);
			}
		}
		if (projectile != null)
		{
			return projectile;
		}
		return null;
	}

	// Token: 0x04001B8E RID: 7054
	private List<GameObject> projs = new List<GameObject>();

	// Token: 0x04001B8F RID: 7055
	public Material origMat;

	// Token: 0x04001B90 RID: 7056
	public Material newMat;
}
