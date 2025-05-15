using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class FindCanvasAndRefresh : MonoBehaviour
{
	// Token: 0x0600024D RID: 589 RVA: 0x000104DB File Offset: 0x0000E6DB
	public void FindCanvasAndForceRefresh()
	{
		Canvas.ForceUpdateCanvases();
	}
}
