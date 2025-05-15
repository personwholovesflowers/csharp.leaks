using System;
using UnityEngine;

// Token: 0x020003AF RID: 943
[Serializable]
public class SavedVector3
{
	// Token: 0x06001582 RID: 5506 RVA: 0x000AE7EC File Offset: 0x000AC9EC
	public SavedVector3(Vector3 vector3)
	{
		this.x = vector3.x;
		this.y = vector3.y;
		this.z = vector3.z;
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x000AE818 File Offset: 0x000ACA18
	public Vector3 ToVector3()
	{
		return new Vector3(this.x, this.y, this.z);
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06001584 RID: 5508 RVA: 0x000AE831 File Offset: 0x000ACA31
	public static SavedVector3 Zero
	{
		get
		{
			return new SavedVector3(Vector3.zero);
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06001585 RID: 5509 RVA: 0x000AE83D File Offset: 0x000ACA3D
	public static SavedVector3 One
	{
		get
		{
			return new SavedVector3(Vector3.one);
		}
	}

	// Token: 0x04001DCF RID: 7631
	public float x;

	// Token: 0x04001DD0 RID: 7632
	public float y;

	// Token: 0x04001DD1 RID: 7633
	public float z;
}
