using System;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class MixerVolumeDip : MonoBehaviour
{
	// Token: 0x06001165 RID: 4453 RVA: 0x00087F8A File Offset: 0x0008618A
	private void OnEnable()
	{
		if (!this.dipped)
		{
			this.dipped = true;
			if (MonoSingleton<AudioMixerController>.Instance)
			{
				MonoSingleton<AudioMixerController>.Instance.TemporaryDip(this.targetAmount);
			}
		}
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x00087FB7 File Offset: 0x000861B7
	private void OnDisable()
	{
		if (this.dipped)
		{
			this.dipped = false;
			if (MonoSingleton<AudioMixerController>.Instance)
			{
				MonoSingleton<AudioMixerController>.Instance.TemporaryDip(0f);
			}
		}
	}

	// Token: 0x040017B5 RID: 6069
	private bool dipped;

	// Token: 0x040017B6 RID: 6070
	public float targetAmount = -5f;
}
