using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
public class WallCheck : MonoBehaviour
{
	// Token: 0x06001BB7 RID: 7095 RVA: 0x000E5F52 File Offset: 0x000E4152
	private void Update()
	{
		if (this.onWall)
		{
			this.onWall = this.CheckForCols();
		}
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x000E5F68 File Offset: 0x000E4168
	private void OnTriggerEnter(Collider other)
	{
		if ((LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) || other.gameObject.layer == 11) && !other.isTrigger && !other.gameObject.CompareTag("Slippery"))
		{
			this.onWall = true;
			this.cols.Add(other);
		}
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x000E5FC4 File Offset: 0x000E41C4
	private void OnTriggerExit(Collider other)
	{
		if ((LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) || other.gameObject.layer == 17 || other.gameObject.layer == 11) && this.cols.Contains(other))
		{
			this.cols.Remove(other);
		}
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x000E6020 File Offset: 0x000E4220
	public bool CheckForCols()
	{
		bool flag = false;
		this.poc = Vector3.zero;
		float num = 100f;
		if (this.cols.Count > 1)
		{
			using (List<Collider>.Enumerator enumerator = this.cols.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Collider collider = enumerator.Current;
					CustomGroundProperties customGroundProperties;
					if (collider != null && collider.enabled && collider.gameObject.activeInHierarchy && LayerMaskDefaults.IsMatchingLayer(collider.gameObject.layer, LMD.Environment) && !collider.isTrigger && !collider.gameObject.CompareTag("Slippery") && (!collider.TryGetComponent<CustomGroundProperties>(out customGroundProperties) || customGroundProperties.canWallJump))
					{
						Vector3 vector = ColliderUtility.FindClosestPoint(collider, base.transform.position, true);
						if (Vector3.Distance(vector, base.transform.position) < num && Vector3.Distance(vector, base.transform.position) < 5f)
						{
							num = Vector3.Distance(vector, base.transform.position);
							this.poc = vector;
							this.currentCollider = collider;
							flag = true;
						}
						else if (Vector3.Distance(vector, base.transform.position) >= 5f)
						{
							this.colsToDelete.Add(collider);
						}
					}
					else
					{
						this.colsToDelete.Add(collider);
					}
				}
				goto IL_02A1;
			}
		}
		CustomGroundProperties customGroundProperties2;
		if (this.cols.Count == 1 && this.cols[0] != null && this.cols[0].enabled && this.cols[0].gameObject.activeInHierarchy && (!this.cols[0].TryGetComponent<CustomGroundProperties>(out customGroundProperties2) || customGroundProperties2.canWallJump))
		{
			Vector3 vector2 = ColliderUtility.FindClosestPoint(this.cols[0], base.transform.position, true);
			if (Vector3.Distance(vector2, base.transform.position) < 5f)
			{
				this.poc = vector2;
			}
			this.currentCollider = this.cols[0];
			flag = true;
		}
		else if (this.cols.Count == 1 && (this.cols[0] == null || Vector3.Distance(ColliderUtility.FindClosestPoint(this.cols[0], base.transform.position, true), base.transform.position) < 5f))
		{
			this.colsToDelete.Add(this.cols[0]);
		}
		IL_02A1:
		if (this.colsToDelete.Count > 0)
		{
			foreach (Collider collider2 in this.colsToDelete)
			{
				if (this.cols.Contains(collider2))
				{
					this.cols.Remove(collider2);
				}
			}
		}
		this.colsToDelete.Clear();
		return flag;
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x000E636C File Offset: 0x000E456C
	public bool CheckForEnemyCols()
	{
		if (MonoSingleton<NewMovement>.Instance.ridingRocket)
		{
			return false;
		}
		Collider[] array = Physics.OverlapSphere(base.transform.position, 2.5f, 4096, QueryTriggerInteraction.Collide);
		if (array.Length != 0)
		{
			foreach (Collider collider in array)
			{
				if (collider != null && collider.enabled && collider.gameObject.activeInHierarchy && Vector3.Distance(base.transform.position, collider.transform.position) < 40f)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04002702 RID: 9986
	public bool onWall;

	// Token: 0x04002703 RID: 9987
	public Vector3 poc;

	// Token: 0x04002704 RID: 9988
	public List<Collider> cols = new List<Collider>();

	// Token: 0x04002705 RID: 9989
	private List<Collider> colsToDelete = new List<Collider>();

	// Token: 0x04002706 RID: 9990
	public Collider currentCollider;
}
