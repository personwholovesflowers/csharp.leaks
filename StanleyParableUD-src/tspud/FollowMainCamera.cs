using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class FollowMainCamera : MonoBehaviour
{
	// Token: 0x060004F1 RID: 1265 RVA: 0x0001CAEF File Offset: 0x0001ACEF
	private void LateUpdate()
	{
		base.transform.position = StanleyController.Instance.cam.transform.position;
		base.transform.rotation = StanleyController.Instance.cam.transform.rotation;
	}
}
