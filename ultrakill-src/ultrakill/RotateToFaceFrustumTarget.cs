using System;
using UnityEngine;

// Token: 0x02000395 RID: 917
internal class RotateToFaceFrustumTarget : MonoBehaviour
{
	// Token: 0x06001516 RID: 5398 RVA: 0x000ACD60 File Offset: 0x000AAF60
	private void Update()
	{
		Quaternion quaternion = (base.transform.parent ? base.transform.parent.rotation : Quaternion.identity);
		if (this.targeter && this.targeter.isActiveAndEnabled && CameraFrustumTargeter.isEnabled && this.targeter.CurrentTarget)
		{
			quaternion = Quaternion.LookRotation(this.targeter.CurrentTarget.bounds.center - base.transform.position);
		}
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * this.maxDegreesDelta);
	}

	// Token: 0x04001D57 RID: 7511
	[SerializeField]
	private CameraFrustumTargeter targeter;

	// Token: 0x04001D58 RID: 7512
	[SerializeField]
	private float maxDegreesDelta;
}
