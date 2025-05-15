using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class EnemyIdentifierIdentifier : MonoBehaviour
{
	// Token: 0x060007F8 RID: 2040 RVA: 0x000371DB File Offset: 0x000353DB
	private void Awake()
	{
		if (!this.eid)
		{
			this.eid = base.GetComponentInParent<EnemyIdentifier>();
		}
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x000371F6 File Offset: 0x000353F6
	private void Start()
	{
		this.startPos = base.transform.position;
		this.SlowCheck();
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0003720F File Offset: 0x0003540F
	private void DestroyLimb()
	{
		this.eid.DestroyLimb(base.transform, LimbDestroyType.Destroy);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00037224 File Offset: 0x00035424
	public void SetupForHellBath()
	{
		Rigidbody rigidbody;
		if (base.TryGetComponent<Rigidbody>(out rigidbody))
		{
			rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
		}
		base.Invoke("DestroyLimbIfNotTouchedBloodAbsorber", StockMapInfo.Instance.gibRemoveTime);
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00037258 File Offset: 0x00035458
	private void DestroyLimbIfNotTouchedBloodAbsorber()
	{
		if (this.eid == null || !this.eid.dead)
		{
			return;
		}
		int num = this.bloodAbsorberCount;
		if (this.eid == base.GetComponentInParent<EnemyIdentifier>())
		{
			num = 0;
			foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in this.eid.GetComponentsInChildren<EnemyIdentifierIdentifier>())
			{
				num += enemyIdentifierIdentifier.bloodAbsorberCount;
			}
		}
		Collider collider;
		if (num <= 0 && base.TryGetComponent<Collider>(out collider))
		{
			GibDestroyer.LimbBegone(collider);
			return;
		}
		if (StockMapInfo.Instance.removeGibsWithoutAbsorbers)
		{
			base.Invoke("DestroyLimbIfNotTouchedBloodAbsorber", StockMapInfo.Instance.gibRemoveTime);
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x000372FC File Offset: 0x000354FC
	private void SlowCheck()
	{
		if (this.eid == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (base.gameObject.activeInHierarchy)
		{
			Vector3 position = base.transform.position;
			if (position.y > 0f)
			{
				position.y = this.startPos.y;
			}
			if (this.eid == null || Vector3.Distance(position, this.startPos) > 9999f || (Vector3.Distance(position, this.startPos) > 999f && this.eid.dead))
			{
				this.deactivated = true;
				MonoSingleton<FireObjectPool>.Instance.RemoveAllFiresFromObject(base.gameObject);
				base.gameObject.SetActive(false);
				base.transform.position = new Vector3(-100f, -100f, -100f);
				base.transform.localScale = Vector3.zero;
				Bloodsplatter componentInChildren = base.GetComponentInChildren<Bloodsplatter>();
				if (componentInChildren)
				{
					componentInChildren.Repool();
				}
				if (this.eid != null && !this.eid.dead)
				{
					this.eid.InstaKill();
				}
			}
		}
		if (!this.deactivated)
		{
			base.Invoke("SlowCheck", 3f);
		}
	}

	// Token: 0x04000A95 RID: 2709
	[HideInInspector]
	public EnemyIdentifier eid;

	// Token: 0x04000A96 RID: 2710
	private bool deactivated;

	// Token: 0x04000A97 RID: 2711
	private Vector3 startPos;

	// Token: 0x04000A98 RID: 2712
	public int bloodAbsorberCount;
}
