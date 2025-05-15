using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class AddForce : MonoBehaviour
{
	// Token: 0x060000F0 RID: 240 RVA: 0x00005F2D File Offset: 0x0000412D
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.Push();
		}
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00005F3D File Offset: 0x0000413D
	private void SetValues()
	{
		if (this.valuesSet)
		{
			return;
		}
		this.valuesSet = true;
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00005F5C File Offset: 0x0000415C
	private void Push()
	{
		if (this.oneTime && this.beenActivated)
		{
			return;
		}
		if (!this.valuesSet)
		{
			this.SetValues();
		}
		if (this.relative)
		{
			this.rb.AddRelativeForce(this.force, ForceMode.VelocityChange);
		}
		else
		{
			this.rb.AddForce(this.force, ForceMode.VelocityChange);
		}
		this.beenActivated = true;
	}

	// Token: 0x0400009E RID: 158
	private Rigidbody rb;

	// Token: 0x0400009F RID: 159
	private bool valuesSet;

	// Token: 0x040000A0 RID: 160
	public Vector3 force;

	// Token: 0x040000A1 RID: 161
	public bool relative;

	// Token: 0x040000A2 RID: 162
	public bool onEnable;

	// Token: 0x040000A3 RID: 163
	public bool oneTime;

	// Token: 0x040000A4 RID: 164
	private bool beenActivated;
}
