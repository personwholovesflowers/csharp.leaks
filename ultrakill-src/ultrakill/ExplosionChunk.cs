using System;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class ExplosionChunk : MonoBehaviour
{
	// Token: 0x06000904 RID: 2308 RVA: 0x0003BE9E File Offset: 0x0003A09E
	private void Start()
	{
		base.Invoke("Gone", 3f);
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0003BEB0 File Offset: 0x0003A0B0
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.done && LayerMaskDefaults.IsMatchingLayer(collision.gameObject.layer, LMD.Environment))
		{
			this.done = true;
			this.rb = base.GetComponent<Rigidbody>();
			base.GetComponent<TrailRenderer>().emitting = false;
			base.Invoke("Gone", 1f);
		}
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0000A719 File Offset: 0x00008919
	private void Gone()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000B75 RID: 2933
	private bool done;

	// Token: 0x04000B76 RID: 2934
	private Rigidbody rb;
}
