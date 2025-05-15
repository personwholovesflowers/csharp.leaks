using System;
using UnityEngine;

// Token: 0x0200028F RID: 655
public class KeepInBounds : MonoBehaviour
{
	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0006BD70 File Offset: 0x00069F70
	private Vector3 CurrentPosition
	{
		get
		{
			if (!this.useColliderCenter)
			{
				return base.transform.position;
			}
			return this.col.bounds.center;
		}
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0006BDA4 File Offset: 0x00069FA4
	private void Awake()
	{
		if (this.useColliderCenter)
		{
			this.col = base.GetComponent<Collider>();
			if (this.col == null)
			{
				Debug.LogWarning("Unfortunately, the Collider component is missing while useColliderCenter is true. Switching to fallback transform.position tracking", base.gameObject);
				this.useColliderCenter = false;
			}
		}
		this.previousTracedPosition = this.CurrentPosition;
		this.previousRealPosition = base.transform.position;
		this.lmask = LayerMaskDefaults.Get(LMD.Environment);
		if (this.includePlayerOnly)
		{
			this.lmask |= 262144;
		}
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0006BE37 File Offset: 0x0006A037
	private void Update()
	{
		if (this.updateMode != KeepInBounds.UpdateMode.Update)
		{
			return;
		}
		this.ValidateMove();
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0006BE49 File Offset: 0x0006A049
	private void FixedUpdate()
	{
		if (this.updateMode != KeepInBounds.UpdateMode.FixedUpdate)
		{
			return;
		}
		this.ValidateMove();
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0006BE5B File Offset: 0x0006A05B
	private void LateUpdate()
	{
		if (this.updateMode != KeepInBounds.UpdateMode.LateUpdate)
		{
			return;
		}
		this.ValidateMove();
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x0006BE6D File Offset: 0x0006A06D
	public void ForceApproveNewPosition()
	{
		this.previousTracedPosition = this.CurrentPosition;
		this.previousRealPosition = base.transform.position;
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x0006BE8C File Offset: 0x0006A08C
	public void ValidateMove()
	{
		Vector3 position = base.transform.position;
		if (this.maxConsideredDistance != 0f && Vector3.Distance(this.previousTracedPosition, this.CurrentPosition) > this.maxConsideredDistance)
		{
			this.previousTracedPosition = this.CurrentPosition;
			this.previousRealPosition = position;
			return;
		}
		if (this.CastCheck())
		{
			this.ApplyCorrectedPosition(this.previousRealPosition);
			return;
		}
		this.previousTracedPosition = this.CurrentPosition;
		this.previousRealPosition = base.transform.position;
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0006BF14 File Offset: 0x0006A114
	private bool CastCheck()
	{
		RaycastHit raycastHit;
		return Physics.Linecast(this.previousTracedPosition, this.CurrentPosition, out raycastHit, this.lmask, QueryTriggerInteraction.Ignore);
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x0006BF40 File Offset: 0x0006A140
	private void ApplyCorrectedPosition(Vector3 position)
	{
		if (this.rb != null)
		{
			this.rb.position = position;
			return;
		}
		base.transform.position = position;
	}

	// Token: 0x04001337 RID: 4919
	[SerializeField]
	private bool useColliderCenter;

	// Token: 0x04001338 RID: 4920
	[SerializeField]
	private float maxConsideredDistance;

	// Token: 0x04001339 RID: 4921
	[SerializeField]
	private KeepInBounds.UpdateMode updateMode = KeepInBounds.UpdateMode.Update;

	// Token: 0x0400133A RID: 4922
	private Vector3 previousTracedPosition;

	// Token: 0x0400133B RID: 4923
	private Vector3 previousRealPosition;

	// Token: 0x0400133C RID: 4924
	private Collider col;

	// Token: 0x0400133D RID: 4925
	private Rigidbody rb;

	// Token: 0x0400133E RID: 4926
	[SerializeField]
	private bool includePlayerOnly;

	// Token: 0x0400133F RID: 4927
	private LayerMask lmask;

	// Token: 0x02000290 RID: 656
	[Serializable]
	private enum UpdateMode
	{
		// Token: 0x04001341 RID: 4929
		None,
		// Token: 0x04001342 RID: 4930
		Update,
		// Token: 0x04001343 RID: 4931
		FixedUpdate,
		// Token: 0x04001344 RID: 4932
		LateUpdate
	}
}
