using System;
using UnityEngine;

// Token: 0x0200033F RID: 831
public class CameraTargetInfo
{
	// Token: 0x0600132E RID: 4910 RVA: 0x0009AC7B File Offset: 0x00098E7B
	public CameraTargetInfo(Vector3 newPosition, GameObject newCaller)
	{
		this.position = newPosition;
		this.rotation = new Vector3(20f, 0f, 0f);
		this.caller = newCaller;
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x0009ACAB File Offset: 0x00098EAB
	public CameraTargetInfo(Vector3 newPosition, Vector3 newRotation, GameObject newCaller)
	{
		this.position = newPosition;
		this.rotation = newRotation;
		this.caller = newCaller;
	}

	// Token: 0x04001A84 RID: 6788
	public Vector3 position;

	// Token: 0x04001A85 RID: 6789
	public Vector3 rotation;

	// Token: 0x04001A86 RID: 6790
	public GameObject caller;
}
