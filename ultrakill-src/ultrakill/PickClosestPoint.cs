using System;
using UnityEngine;

// Token: 0x02000337 RID: 823
public class PickClosestPoint : MonoBehaviour
{
	// Token: 0x060012F1 RID: 4849 RVA: 0x00096BA9 File Offset: 0x00094DA9
	private void OnEnable()
	{
		if (this.pickOnEnable)
		{
			this.Pick();
		}
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x00096BBC File Offset: 0x00094DBC
	private void Pick()
	{
		Transform transform = null;
		float num = float.MaxValue;
		Transform transform2 = (this.closestToPlayer ? MonoSingleton<PlayerTracker>.Instance.GetPlayer() : this.customComparisonPoint);
		foreach (Transform transform3 in this.points)
		{
			float num2 = Vector3.Distance(transform3.position, transform2.position);
			if (num2 < num)
			{
				num = num2;
				transform = transform3;
			}
		}
		if (transform == null)
		{
			return;
		}
		if (this.parentTargetToClosestPoint)
		{
			this.target.SetParent(transform);
			this.target.localRotation = Quaternion.identity;
			this.target.localScale = Vector3.one;
			this.target.localPosition = Vector3.zero;
			return;
		}
		if (this.mimicRotation)
		{
			this.target.rotation = transform.rotation;
		}
		if (this.mimicScale)
		{
			this.target.localScale = transform.localScale;
		}
		if (this.mimicPosition)
		{
			this.target.position = transform.position;
		}
	}

	// Token: 0x040019FA RID: 6650
	public Transform target;

	// Token: 0x040019FB RID: 6651
	public Transform[] points;

	// Token: 0x040019FC RID: 6652
	public Transform customComparisonPoint;

	// Token: 0x040019FD RID: 6653
	[SerializeField]
	private bool pickOnEnable = true;

	// Token: 0x040019FE RID: 6654
	[SerializeField]
	private bool parentTargetToClosestPoint = true;

	// Token: 0x040019FF RID: 6655
	[SerializeField]
	private bool mimicRotation = true;

	// Token: 0x04001A00 RID: 6656
	[SerializeField]
	private bool mimicPosition = true;

	// Token: 0x04001A01 RID: 6657
	[SerializeField]
	private bool mimicScale = true;

	// Token: 0x04001A02 RID: 6658
	[SerializeField]
	private bool closestToPlayer = true;
}
