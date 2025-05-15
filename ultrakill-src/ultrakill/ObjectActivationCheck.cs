using System;
using UnityEngine;

// Token: 0x0200031A RID: 794
public class ObjectActivationCheck : MonoBehaviour
{
	// Token: 0x0600124E RID: 4686 RVA: 0x00093499 File Offset: 0x00091699
	public void StateChange(bool state)
	{
		this.readyToActivate = state;
	}

	// Token: 0x04001946 RID: 6470
	public bool readyToActivate;
}
