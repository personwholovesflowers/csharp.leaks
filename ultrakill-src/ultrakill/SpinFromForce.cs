using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class SpinFromForce : MonoBehaviour
{
	// Token: 0x06001DB6 RID: 7606 RVA: 0x000F7CE4 File Offset: 0x000F5EE4
	private void Update()
	{
		this.angularVelocity *= 1f - this.angularDrag;
		Vector3 vector = base.transform.TransformDirection(this.rotationAxis);
		Quaternion quaternion = Quaternion.AngleAxis(Vector3.Dot(this.angularVelocity, vector) * Time.deltaTime, vector);
		base.transform.rotation = quaternion * base.transform.rotation;
	}

	// Token: 0x06001DB7 RID: 7607 RVA: 0x000F7D58 File Offset: 0x000F5F58
	public void AddSpin(ref List<ParticleCollisionEvent> pEvents)
	{
		foreach (ParticleCollisionEvent particleCollisionEvent in pEvents)
		{
			Vector3 vector = Vector3.Cross(particleCollisionEvent.intersection - base.transform.position, particleCollisionEvent.velocity);
			this.angularVelocity += vector / this.mass;
			this.angularVelocity = Vector3.Project(this.angularVelocity, base.transform.TransformDirection(this.rotationAxis));
		}
	}

	// Token: 0x04002A15 RID: 10773
	public Vector3 rotationAxis = Vector3.up;

	// Token: 0x04002A16 RID: 10774
	public float angularDrag = 0.01f;

	// Token: 0x04002A17 RID: 10775
	public float mass = 1f;

	// Token: 0x04002A18 RID: 10776
	private Vector3 angularVelocity = Vector3.zero;
}
