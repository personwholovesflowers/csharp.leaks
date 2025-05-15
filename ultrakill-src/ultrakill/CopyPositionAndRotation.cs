using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000EB RID: 235
[DefaultExecutionOrder(-2147483648)]
public sealed class CopyPositionAndRotation : MonoBehaviour
{
	// Token: 0x06000495 RID: 1173 RVA: 0x0001FB84 File Offset: 0x0001DD84
	private void Start()
	{
		if (this.m_Target == null)
		{
			return;
		}
		this._initialPositionOffset = base.transform.position - this.m_Target.position;
		this._initialRotationOffset = Quaternion.Inverse(this.m_Target.rotation) * base.transform.rotation;
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000496 RID: 1174 RVA: 0x0001FBE7 File Offset: 0x0001DDE7
	// (set) Token: 0x06000497 RID: 1175 RVA: 0x0001FBEF File Offset: 0x0001DDEF
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

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001FBF8 File Offset: 0x0001DDF8
	// (set) Token: 0x06000499 RID: 1177 RVA: 0x0001FC00 File Offset: 0x0001DE00
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

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001FC09 File Offset: 0x0001DE09
	// (set) Token: 0x0600049B RID: 1179 RVA: 0x0001FC11 File Offset: 0x0001DE11
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

	// Token: 0x0600049C RID: 1180 RVA: 0x0001FC1C File Offset: 0x0001DE1C
	private void LateUpdate()
	{
		if (this.useRelative)
		{
			if (this.m_CopyPosition)
			{
				base.transform.position = this.m_Target.position + this._initialPositionOffset;
			}
			if (this.m_CopyRotation)
			{
				base.transform.rotation = this.m_Target.rotation * this._initialRotationOffset;
			}
			return;
		}
		if (this.m_CopyRotation)
		{
			base.transform.rotation = this.m_Target.rotation;
		}
		if (this.m_CopyPosition)
		{
			base.transform.position = this.m_Target.position;
		}
	}

	// Token: 0x0400063F RID: 1599
	[SerializeField]
	[FormerlySerializedAs("target")]
	private Transform m_Target;

	// Token: 0x04000640 RID: 1600
	[SerializeField]
	[FormerlySerializedAs("copyRotation")]
	private bool m_CopyRotation = true;

	// Token: 0x04000641 RID: 1601
	[SerializeField]
	[FormerlySerializedAs("copyPosition")]
	private bool m_CopyPosition = true;

	// Token: 0x04000642 RID: 1602
	public bool useRelative;

	// Token: 0x04000643 RID: 1603
	private Vector3 _initialPositionOffset;

	// Token: 0x04000644 RID: 1604
	private Quaternion _initialRotationOffset;
}
