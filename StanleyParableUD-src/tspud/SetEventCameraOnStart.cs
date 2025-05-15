using System;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class SetEventCameraOnStart : MonoBehaviour
{
	// Token: 0x06000917 RID: 2327 RVA: 0x0002B212 File Offset: 0x00029412
	private void Start()
	{
		this.targetCanvas.worldCamera = StanleyController.Instance.cam;
	}

	// Token: 0x040008EE RID: 2286
	[SerializeField]
	private Canvas targetCanvas;
}
