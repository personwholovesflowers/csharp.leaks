using System;
using UnityEngine;

// Token: 0x02000163 RID: 355
[CreateAssetMenu(fileName = "New Cursor Profile", menuName = "Stanley/Cursor Profile")]
public class CursorProfile : ScriptableObject
{
	// Token: 0x06000852 RID: 2130 RVA: 0x00027CFF File Offset: 0x00025EFF
	public void SetCursorToThis()
	{
		CursorController.Instance.SetCustomCursor(this);
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00027D0C File Offset: 0x00025F0C
	public void ResetCursor()
	{
		CursorController.Instance.ResetToDefault();
	}

	// Token: 0x0400082C RID: 2092
	public Texture2D cursorTexture;

	// Token: 0x0400082D RID: 2093
	public Vector2 hotspot;
}
