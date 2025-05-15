using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000293 RID: 659
[DefaultExecutionOrder(2147483647)]
public sealed class LateCopyPositionAndRotation : MonoBehaviour
{
	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0006C55A File Offset: 0x0006A75A
	// (set) Token: 0x06000E90 RID: 3728 RVA: 0x0006C562 File Offset: 0x0006A762
	public Transform target
	{
		get
		{
			return this.m_Target;
		}
		set
		{
			this.m_Target = value;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0006C56B File Offset: 0x0006A76B
	// (set) Token: 0x06000E92 RID: 3730 RVA: 0x0006C573 File Offset: 0x0006A773
	public bool copyRotation
	{
		get
		{
			return this.m_CopyRotation;
		}
		set
		{
			this.m_CopyRotation = value;
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000E93 RID: 3731 RVA: 0x0006C57C File Offset: 0x0006A77C
	// (set) Token: 0x06000E94 RID: 3732 RVA: 0x0006C584 File Offset: 0x0006A784
	public bool copyPosition
	{
		get
		{
			return this.m_CopyPosition;
		}
		set
		{
			this.m_CopyPosition = value;
		}
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x0006C58D File Offset: 0x0006A78D
	private void LateUpdate()
	{
		if (this.m_CopyRotation)
		{
			base.transform.rotation = this.m_Target.rotation;
		}
		if (this.m_CopyPosition)
		{
			base.transform.position = this.m_Target.position;
		}
	}

	// Token: 0x04001356 RID: 4950
	[SerializeField]
	[FormerlySerializedAs("target")]
	private Transform m_Target;

	// Token: 0x04001357 RID: 4951
	[SerializeField]
	[FormerlySerializedAs("copyRotation")]
	private bool m_CopyRotation = true;

	// Token: 0x04001358 RID: 4952
	[SerializeField]
	[FormerlySerializedAs("copyPosition")]
	private bool m_CopyPosition = true;
}
