using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class CubemapRotation : MonoBehaviour
{
	// Token: 0x060003FB RID: 1019 RVA: 0x00018F14 File Offset: 0x00017114
	private void Update()
	{
		this.yRot = base.transform.eulerAngles.y;
		Quaternion quaternion = Quaternion.Euler(0f, this.yRot, 0f);
		Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, quaternion, new Vector3(1f, 1f, 1f));
		base.GetComponent<Renderer>().material.SetMatrix("_Rotation", matrix4x);
	}

	// Token: 0x040003F5 RID: 1013
	[SerializeField]
	private float yRot = 60f;
}
