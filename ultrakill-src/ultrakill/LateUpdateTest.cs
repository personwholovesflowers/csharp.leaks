using System;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class LateUpdateTest : MonoBehaviour
{
	// Token: 0x06000E9A RID: 3738 RVA: 0x0006C621 File Offset: 0x0006A821
	private void LateUpdate()
	{
		base.transform.position = MonoSingleton<NewMovement>.Instance.transform.position;
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x0006C63D File Offset: 0x0006A83D
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Fuck");
	}
}
