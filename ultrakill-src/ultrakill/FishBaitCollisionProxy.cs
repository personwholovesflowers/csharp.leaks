using System;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class FishBaitCollisionProxy : MonoBehaviour
{
	// Token: 0x060009A4 RID: 2468 RVA: 0x00042F8A File Offset: 0x0004118A
	private void OnTriggerExit(Collider other)
	{
		this.fishBait.OnTriggerExit(other);
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00042F98 File Offset: 0x00041198
	private void OnCollisionEnter(Collision collision)
	{
		this.fishBait.OnCollisionEnter(collision);
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00042FA8 File Offset: 0x000411A8
	private void Update()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.lastPosition, base.transform.position - this.lastPosition, out raycastHit, Vector3.Distance(this.lastPosition, base.transform.position), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
		{
			Debug.Log("Out of water due to col proxy raycast");
			this.fishBait.OutOfWater();
		}
		this.lastPosition = base.transform.position;
	}

	// Token: 0x04000C8E RID: 3214
	[SerializeField]
	private FishBait fishBait;

	// Token: 0x04000C8F RID: 3215
	private Vector3 lastPosition;
}
