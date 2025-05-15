using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000238 RID: 568
public class GroundCheckEnemy : MonoBehaviour
{
	// Token: 0x06000C2E RID: 3118 RVA: 0x00056DC0 File Offset: 0x00054FC0
	private void Start()
	{
		this.col = base.GetComponent<Collider>();
		if (this.col)
		{
			this.col.enabled = false;
			this.col.enabled = true;
		}
		this.CheckCols();
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x00056DF9 File Offset: 0x00054FF9
	private void OnEnable()
	{
		this.cols.Clear();
		this.toRemove.Clear();
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x00056E14 File Offset: 0x00055014
	public static bool TaggedAsStandable(GameObject obj)
	{
		return obj.CompareTag("Floor") || obj.CompareTag("Wall") || obj.CompareTag("GlassFloor") || obj.CompareTag("Moving") || obj.CompareTag("Breakable") || obj.CompareTag("SoftFloor");
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00056E70 File Offset: 0x00055070
	private void UpdateGrounds()
	{
		bool flag = true;
		for (int i = 0; i < this.cols.Count; i++)
		{
			if (!(this.cols[i] != null) || !this.cols[i].CompareTag("SoftFloor"))
			{
				flag = false;
				break;
			}
		}
		this.fallSuppressed = flag;
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00056ECC File Offset: 0x000550CC
	private void FixedUpdate()
	{
		this.waitForPhysicsTick = false;
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x00056ED8 File Offset: 0x000550D8
	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("Slippery") && (LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) || other.gameObject.layer == 16) && ((other.gameObject.layer != 16 && this.dontCheckTags) || GroundCheckEnemy.TaggedAsStandable(other.gameObject)))
		{
			if (this.toIgnore.Contains(other))
			{
				return;
			}
			this.touchingGround = true;
			this.cols.Add(other);
			this.UpdateGrounds();
			if (this.forcedOff <= 0)
			{
				this.onGround = this.touchingGround;
			}
		}
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x00056F80 File Offset: 0x00055180
	private void OnTriggerExit(Collider other)
	{
		if (!other.gameObject.CompareTag("Slippery") && (LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) || other.gameObject.layer == 16) && ((other.gameObject.layer != 16 && this.dontCheckTags) || GroundCheckEnemy.TaggedAsStandable(other.gameObject)) && this.cols.Contains(other))
		{
			this.cols.Remove(other);
			this.UpdateGrounds();
		}
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00057008 File Offset: 0x00055208
	private void CheckCols()
	{
		base.Invoke("CheckCols", 0.1f);
		if (base.transform.up.y < 0.25f)
		{
			this.touchingGround = false;
			this.onGround = false;
			this.waitForPhysicsTick = true;
			return;
		}
		if (this.waitForPhysicsTick)
		{
			return;
		}
		this.CheckColsOnce();
		if (this.forcedOff > 0)
		{
			this.onGround = false;
			return;
		}
		this.onGround = this.touchingGround;
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x00057080 File Offset: 0x00055280
	private void CheckColsOnce()
	{
		bool flag = false;
		for (int i = this.cols.Count - 1; i >= 0; i--)
		{
			Collider collider = this.cols[i];
			if (collider == null || this.toIgnore.Contains(collider) || !collider.enabled)
			{
				this.cols.RemoveAt(i);
			}
			else
			{
				GameObject gameObject = collider.gameObject;
				if (!gameObject.activeInHierarchy)
				{
					this.cols.RemoveAt(i);
				}
				else if (!gameObject.CompareTag("Slippery") && (LayerMaskDefaults.IsMatchingLayer(gameObject.layer, LMD.Environment) || gameObject.layer == 16) && ((gameObject.layer != 16 && this.dontCheckTags) || GroundCheckEnemy.TaggedAsStandable(gameObject)))
				{
					flag = true;
				}
			}
		}
		this.touchingGround = flag;
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x0005714C File Offset: 0x0005534C
	public void ForceOff()
	{
		this.forcedOff++;
		this.onGround = false;
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x00057163 File Offset: 0x00055363
	public void StopForceOff()
	{
		this.forcedOff--;
		if (this.forcedOff <= 0)
		{
			this.onGround = this.touchingGround;
		}
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x00057188 File Offset: 0x00055388
	public Vector3 ClosestPoint()
	{
		this.CheckColsOnce();
		if (this.cols.Count > 0)
		{
			Vector3 vector = base.transform.position;
			float num = 999f;
			foreach (Collider collider in this.cols)
			{
				Vector3 vector2 = collider.ClosestPoint(base.transform.position);
				float num2 = Vector3.SqrMagnitude(vector2 - base.transform.position);
				if (num2 < num && !this.toIgnore.Contains(collider))
				{
					vector = vector2;
					num = num2;
				}
			}
			return vector;
		}
		return base.transform.position;
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x00057250 File Offset: 0x00055450
	public float DistanceToGround()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.col.bounds.center, Vector3.down, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
		{
			return raycastHit.distance - this.col.bounds.extents.y;
		}
		return float.PositiveInfinity;
	}

	// Token: 0x04001004 RID: 4100
	public bool onGround;

	// Token: 0x04001005 RID: 4101
	public bool fallSuppressed;

	// Token: 0x04001006 RID: 4102
	public bool touchingGround;

	// Token: 0x04001007 RID: 4103
	public List<Collider> cols = new List<Collider>();

	// Token: 0x04001008 RID: 4104
	private List<Collider> toRemove = new List<Collider>();

	// Token: 0x04001009 RID: 4105
	public bool dontCheckTags;

	// Token: 0x0400100A RID: 4106
	[HideInInspector]
	public int forcedOff;

	// Token: 0x0400100B RID: 4107
	public List<Collider> toIgnore = new List<Collider>();

	// Token: 0x0400100C RID: 4108
	private Collider col;

	// Token: 0x0400100D RID: 4109
	private bool waitForPhysicsTick;
}
