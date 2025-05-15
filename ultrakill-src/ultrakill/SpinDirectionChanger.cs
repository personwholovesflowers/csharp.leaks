using System;
using UnityEngine;

// Token: 0x02000435 RID: 1077
public class SpinDirectionChanger : MonoBehaviour
{
	// Token: 0x06001847 RID: 6215 RVA: 0x000C643A File Offset: 0x000C463A
	private void Start()
	{
		this.target.spinDirection = this.direction;
	}

	// Token: 0x04002215 RID: 8725
	public Spin target;

	// Token: 0x04002216 RID: 8726
	public Vector3 direction;
}
