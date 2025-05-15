using System;
using UnityEngine;

// Token: 0x020003C9 RID: 969
public class ScatterRigidbodies : MonoBehaviour
{
	// Token: 0x06001608 RID: 5640 RVA: 0x000B2560 File Offset: 0x000B0760
	private void Start()
	{
		this.rbs = base.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in this.rbs)
		{
			rigidbody.AddForce(Random.Range(this.minForce.x, this.maxForce.x), Random.Range(this.minForce.y, this.maxForce.y), Random.Range(this.minForce.z, this.maxForce.z), ForceMode.VelocityChange);
			rigidbody.AddTorque(Random.Range(-this.rotationForce, this.rotationForce), Random.Range(-this.rotationForce, this.rotationForce), Random.Range(-this.rotationForce, this.rotationForce), ForceMode.VelocityChange);
		}
	}

	// Token: 0x04001E55 RID: 7765
	private Rigidbody[] rbs;

	// Token: 0x04001E56 RID: 7766
	public Vector3 minForce;

	// Token: 0x04001E57 RID: 7767
	public Vector3 maxForce;

	// Token: 0x04001E58 RID: 7768
	public float rotationForce;
}
