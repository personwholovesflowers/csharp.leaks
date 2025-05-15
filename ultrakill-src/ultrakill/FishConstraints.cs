using System;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class FishConstraints : MonoBehaviour
{
	// Token: 0x060009A8 RID: 2472 RVA: 0x00043024 File Offset: 0x00041224
	private void Awake()
	{
		if (this.restrictToColliderBounds == null || this.restrictToColliderBounds.Length == 0)
		{
			Collider[] components = base.GetComponents<BoxCollider>();
			this.restrictToColliderBounds = components;
		}
		if (this.restrictToColliderBounds == null || this.restrictToColliderBounds.Length == 0)
		{
			return;
		}
		foreach (Collider collider in this.restrictToColliderBounds)
		{
			if (!(collider == null))
			{
				Bounds bounds = this.area;
				if (this.area.size == Vector3.zero)
				{
					this.area = collider.bounds;
				}
				else
				{
					this.area.Encapsulate(collider.bounds);
				}
			}
		}
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x000430C4 File Offset: 0x000412C4
	private void OnDrawGizmos()
	{
		Bounds bounds = default(Bounds);
		if (this.restrictToColliderBounds != null)
		{
			foreach (Collider collider in this.restrictToColliderBounds)
			{
				if (!(collider == null))
				{
					if (bounds.size == Vector3.zero)
					{
						bounds = collider.bounds;
					}
					else
					{
						bounds.Encapsulate(collider.bounds);
					}
				}
			}
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(bounds.center, bounds.size);
	}

	// Token: 0x04000C90 RID: 3216
	[SerializeField]
	private Collider[] restrictToColliderBounds;

	// Token: 0x04000C91 RID: 3217
	[NonSerialized]
	public Bounds area;
}
