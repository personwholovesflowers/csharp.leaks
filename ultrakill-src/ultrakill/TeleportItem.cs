using System;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public class TeleportItem : MonoBehaviour
{
	// Token: 0x06001A26 RID: 6694 RVA: 0x000D7DB8 File Offset: 0x000D5FB8
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 22)
		{
			other.transform.position = this.position;
			if (this.resetVelocity && other.attachedRigidbody)
			{
				other.attachedRigidbody.velocity = Vector3.zero;
			}
		}
	}

	// Token: 0x040024A0 RID: 9376
	public Vector3 position;

	// Token: 0x040024A1 RID: 9377
	public bool resetVelocity;
}
