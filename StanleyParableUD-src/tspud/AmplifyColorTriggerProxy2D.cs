using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[AddComponentMenu("")]
public class AmplifyColorTriggerProxy2D : AmplifyColorTriggerProxyBase
{
	// Token: 0x06000033 RID: 51 RVA: 0x00003BC0 File Offset: 0x00001DC0
	private void Start()
	{
		this.circleCollider = base.GetComponent<CircleCollider2D>();
		this.circleCollider.radius = 0.01f;
		this.circleCollider.isTrigger = true;
		this.rigidBody = base.GetComponent<Rigidbody2D>();
		this.rigidBody.gravityScale = 0f;
		this.rigidBody.isKinematic = true;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003B89 File Offset: 0x00001D89
	private void LateUpdate()
	{
		base.transform.position = this.Reference.position;
		base.transform.rotation = this.Reference.rotation;
	}

	// Token: 0x04000052 RID: 82
	private CircleCollider2D circleCollider;

	// Token: 0x04000053 RID: 83
	private Rigidbody2D rigidBody;
}
