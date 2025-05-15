using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
[RequireComponent(typeof(Canvas))]
public class CanvasOrdering : MonoBehaviour
{
	// Token: 0x060003C8 RID: 968 RVA: 0x000186B2 File Offset: 0x000168B2
	private void Awake()
	{
		this.canvas = base.GetComponent<Canvas>();
		this.canvas.planeDistance = this.defaultPlaneDistance;
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x000186D1 File Offset: 0x000168D1
	private void Update()
	{
		if (this.canvas.worldCamera != StanleyController.Instance.currentCam)
		{
			this.canvas.worldCamera = StanleyController.Instance.currentCam;
		}
	}

	// Token: 0x040003BB RID: 955
	private Canvas canvas;

	// Token: 0x040003BC RID: 956
	public float defaultPlaneDistance;
}
