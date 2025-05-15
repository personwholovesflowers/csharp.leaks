using System;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class SetCursor : MonoBehaviour
{
	// Token: 0x06000AD9 RID: 2777 RVA: 0x0003359F File Offset: 0x0003179F
	private void Update()
	{
		Cursor.SetCursor(this.cursorTexture, this.hotSpot, this.cursorMode);
	}

	// Token: 0x04000AD2 RID: 2770
	public Texture2D cursorTexture;

	// Token: 0x04000AD3 RID: 2771
	public CursorMode cursorMode = CursorMode.ForceSoftware;

	// Token: 0x04000AD4 RID: 2772
	public Vector2 hotSpot = Vector2.zero;
}
