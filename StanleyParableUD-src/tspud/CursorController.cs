using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class CursorController : MonoBehaviour
{
	// Token: 0x17000046 RID: 70
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x00018F96 File Offset: 0x00017196
	// (set) Token: 0x060003FE RID: 1022 RVA: 0x00018F9D File Offset: 0x0001719D
	public static CursorController Instance { get; private set; }

	// Token: 0x060003FF RID: 1023 RVA: 0x00018FA5 File Offset: 0x000171A5
	public void Awake()
	{
		CursorController.Instance = this;
		this.SetCursor();
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x00018FB3 File Offset: 0x000171B3
	public void SetCustomCursor(CursorProfile profile)
	{
		this.customProfile = profile;
		this.SetCursor();
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x00018FC2 File Offset: 0x000171C2
	public void ResetToDefault()
	{
		this.customProfile = null;
		this.SetCursor();
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00018FD4 File Offset: 0x000171D4
	private void SetCursor()
	{
		if (!(this.customProfile == null))
		{
			Cursor.SetCursor(this.customProfile.cursorTexture, this.customProfile.hotspot, CursorMode.ForceSoftware);
			return;
		}
		if (this.defaultProfile == null || this.defaultProfile.cursorTexture == null)
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			return;
		}
		Cursor.SetCursor(this.defaultProfile.cursorTexture, this.defaultProfile.hotspot, CursorMode.ForceSoftware);
	}

	// Token: 0x040003F7 RID: 1015
	public CursorProfile defaultProfile;

	// Token: 0x040003F8 RID: 1016
	private CursorProfile customProfile;
}
