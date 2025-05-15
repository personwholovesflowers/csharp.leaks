using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[AddComponentMenu("")]
public class AmplifyColorTriggerProxy : AmplifyColorTriggerProxyBase
{
	// Token: 0x06000030 RID: 48 RVA: 0x00003B30 File Offset: 0x00001D30
	private void Start()
	{
		this.sphereCollider = base.GetComponent<SphereCollider>();
		this.sphereCollider.radius = 0.01f;
		this.sphereCollider.isTrigger = true;
		this.rigidBody = base.GetComponent<Rigidbody>();
		this.rigidBody.useGravity = false;
		this.rigidBody.isKinematic = true;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003B89 File Offset: 0x00001D89
	private void LateUpdate()
	{
		base.transform.position = this.Reference.position;
		base.transform.rotation = this.Reference.rotation;
	}

	// Token: 0x04000050 RID: 80
	private SphereCollider sphereCollider;

	// Token: 0x04000051 RID: 81
	private Rigidbody rigidBody;
}
