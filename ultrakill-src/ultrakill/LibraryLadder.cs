using System;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class LibraryLadder : MonoBehaviour
{
	// Token: 0x06001DA6 RID: 7590 RVA: 0x000F7887 File Offset: 0x000F5A87
	private void Start()
	{
		this.rbTrans = this.rb.transform;
		this.startPos = this.rbTrans.localPosition;
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x000F78AC File Offset: 0x000F5AAC
	private void FixedUpdate()
	{
		Vector3 vector = Vector3.Scale(this.rbTrans.InverseTransformVector(this.rb.velocity), new Vector3(1f, 0f, 0f));
		this.rb.velocity = this.rbTrans.TransformVector(vector);
		Vector3 localPosition = this.rbTrans.localPosition;
		float x = this.leftClamp.localPosition.x;
		float x2 = this.rightClamp.localPosition.x;
		localPosition.x = Mathf.Clamp(localPosition.x, x, x2);
		localPosition.y = this.startPos.y;
		localPosition.z = this.startPos.z;
		this.rbTrans.localPosition = localPosition;
	}

	// Token: 0x040029FE RID: 10750
	public Rigidbody rb;

	// Token: 0x040029FF RID: 10751
	public Transform leftClamp;

	// Token: 0x04002A00 RID: 10752
	public Transform rightClamp;

	// Token: 0x04002A01 RID: 10753
	private Vector3 startPos;

	// Token: 0x04002A02 RID: 10754
	private Transform rbTrans;
}
