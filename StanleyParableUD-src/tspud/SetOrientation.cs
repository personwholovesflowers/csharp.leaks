using System;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class SetOrientation : MonoBehaviour
{
	// Token: 0x0600091F RID: 2335 RVA: 0x0002B2DF File Offset: 0x000294DF
	public void MoveAndRotateToHere(Transform t)
	{
		t.position = base.transform.position;
		t.rotation = base.transform.rotation;
	}
}
