using System;
using UnityEngine;

// Token: 0x0200031D RID: 797
public class ObjectBounds : MonoBehaviour
{
	// Token: 0x06001263 RID: 4707 RVA: 0x00093BEC File Offset: 0x00091DEC
	private void Start()
	{
		this.cols = base.GetComponents<Collider>();
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x00093BFC File Offset: 0x00091DFC
	private void Update()
	{
		bool flag = false;
		for (int i = this.cols.Length - 1; i >= 0; i--)
		{
			if (!(this.cols[i] == null) && Vector3.Distance(this.cols[i].ClosestPoint(this.target.position), this.target.position) <= 0.1f)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.target.position = base.transform.position;
			if (this.cancelMomentum && (this.rb || this.target.TryGetComponent<Rigidbody>(out this.rb)))
			{
				this.rb.velocity = Vector3.zero;
			}
		}
	}

	// Token: 0x04001962 RID: 6498
	public Transform target;

	// Token: 0x04001963 RID: 6499
	public bool cancelMomentum;

	// Token: 0x04001964 RID: 6500
	private Rigidbody rb;

	// Token: 0x04001965 RID: 6501
	private Collider[] cols;
}
