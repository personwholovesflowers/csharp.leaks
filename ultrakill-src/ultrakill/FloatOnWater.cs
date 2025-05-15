using System;
using UnityEngine;

// Token: 0x02000507 RID: 1287
public class FloatOnWater : MonoBehaviour
{
	// Token: 0x06001D6A RID: 7530 RVA: 0x000F64E2 File Offset: 0x000F46E2
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x000F64F0 File Offset: 0x000F46F0
	private void Update()
	{
		if (!this.isInWater)
		{
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(new Vector3(base.transform.position.x, this.waterCol.bounds.max.y + 0.1f, base.transform.position.z), Vector3.down, out raycastHit, 900f, 16, QueryTriggerInteraction.Collide))
		{
			Vector3 vector = raycastHit.point - base.transform.position;
			this.rb.AddForce(vector * this.floatiness - this.rb.velocity * this.dampen);
		}
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x000F65AC File Offset: 0x000F47AC
	private void OnTriggerEnter(Collider other)
	{
		Water componentInParent;
		if (!other.TryGetComponent<Water>(out componentInParent))
		{
			componentInParent = other.GetComponentInParent<Water>();
		}
		if (componentInParent != null)
		{
			this.isInWater = true;
			this.waterCol = other;
		}
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x000F65E1 File Offset: 0x000F47E1
	private void OnTriggerExit(Collider other)
	{
		if (other == this.waterCol)
		{
			this.waterCol = null;
			this.isInWater = false;
		}
	}

	// Token: 0x040029AA RID: 10666
	private Rigidbody rb;

	// Token: 0x040029AB RID: 10667
	private Collider waterCol;

	// Token: 0x040029AC RID: 10668
	private bool isInWater;

	// Token: 0x040029AD RID: 10669
	public float floatiness = 50f;

	// Token: 0x040029AE RID: 10670
	public float dampen = 0.9f;
}
