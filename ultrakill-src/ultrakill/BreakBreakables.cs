using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class BreakBreakables : MonoBehaviour
{
	// Token: 0x060002E5 RID: 741 RVA: 0x00011231 File Offset: 0x0000F431
	private void Start()
	{
		if (!base.TryGetComponent<Collider>(out this.col) && this.breakables.Length != 0)
		{
			this.Break();
		}
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00011250 File Offset: 0x0000F450
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && this.breakables.Length != 0)
		{
			this.Break();
		}
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00011274 File Offset: 0x0000F474
	private void Break()
	{
		if (this.i >= this.breakables.Length)
		{
			Object.Destroy(this);
			return;
		}
		if (this.breakables[this.i] != null)
		{
			this.breakables[this.i].Break();
		}
		this.i++;
		if (this.delay != 0f)
		{
			base.Invoke("Break", this.delay);
			return;
		}
		this.Break();
	}

	// Token: 0x04000385 RID: 901
	public Breakable[] breakables;

	// Token: 0x04000386 RID: 902
	public float delay;

	// Token: 0x04000387 RID: 903
	private int i;

	// Token: 0x04000388 RID: 904
	private Collider col;
}
