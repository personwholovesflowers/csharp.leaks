using System;
using UnityEngine;

// Token: 0x020003B0 RID: 944
[Serializable]
public class SavedQuaternion
{
	// Token: 0x06001586 RID: 5510 RVA: 0x000AE849 File Offset: 0x000ACA49
	public SavedQuaternion(Quaternion quaternion)
	{
		this.x = quaternion.x;
		this.y = quaternion.y;
		this.z = quaternion.z;
		this.w = quaternion.w;
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x000AE881 File Offset: 0x000ACA81
	public Quaternion ToQuaternion()
	{
		return new Quaternion(this.x, this.y, this.z, this.w);
	}

	// Token: 0x04001DD2 RID: 7634
	public float x;

	// Token: 0x04001DD3 RID: 7635
	public float y;

	// Token: 0x04001DD4 RID: 7636
	public float z;

	// Token: 0x04001DD5 RID: 7637
	public float w;
}
