using System;
using UnityEngine;

// Token: 0x020004CB RID: 1227
public class WebButton : MonoBehaviour
{
	// Token: 0x06001C11 RID: 7185 RVA: 0x000E9233 File Offset: 0x000E7433
	public void OpenURL()
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		Application.OpenURL(this.url);
	}

	// Token: 0x0400279B RID: 10139
	public string url;
}
