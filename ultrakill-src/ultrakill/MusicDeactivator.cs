using System;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class MusicDeactivator : MonoBehaviour
{
	// Token: 0x060011B9 RID: 4537 RVA: 0x0008A244 File Offset: 0x00088444
	private void OnEnable()
	{
		MonoSingleton<MusicManager>.Instance.StopMusic();
		if (this.forceOff)
		{
			MonoSingleton<MusicManager>.Instance.forcedOff = true;
		}
		if (this.oneTime)
		{
			Object.Destroy(this);
		}
	}

	// Token: 0x0400182C RID: 6188
	public bool oneTime;

	// Token: 0x0400182D RID: 6189
	public bool forceOff;
}
