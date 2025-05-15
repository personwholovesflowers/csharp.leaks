using System;
using UnityEngine;

// Token: 0x02000515 RID: 1301
public class SphereForce : MonoBehaviour
{
	// Token: 0x06001DB4 RID: 7604 RVA: 0x000F7C6C File Offset: 0x000F5E6C
	private void OnTriggerStay(Collider other)
	{
		Rigidbody attachedRigidbody = other.attachedRigidbody;
		if (attachedRigidbody == null)
		{
			return;
		}
		Vector3 vector = base.transform.position - attachedRigidbody.position;
		float magnitude = vector.magnitude;
		vector = vector.normalized / magnitude;
		other.attachedRigidbody.AddForce(vector * this.strength, ForceMode.Force);
	}

	// Token: 0x04002A14 RID: 10772
	public float strength = 10f;
}
