using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class PostEffectsVolume : VolumeBase
{
	// Token: 0x06000316 RID: 790 RVA: 0x0001536C File Offset: 0x0001356C
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInChildren<PostEffectsCamera>() != null && PostEffectsVolume.OnEnterVolume != null)
		{
			PostEffectsVolume.OnEnterVolume(this);
		}
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0001538E File Offset: 0x0001358E
	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInChildren<PostEffectsCamera>() != null && PostEffectsVolume.OnExitVolume != null)
		{
			PostEffectsVolume.OnExitVolume(this);
		}
	}

	// Token: 0x06000318 RID: 792 RVA: 0x000153B0 File Offset: 0x000135B0
	public override ProfileBase GetProfile()
	{
		return this.Profile;
	}

	// Token: 0x04000322 RID: 802
	public static Action<PostEffectsVolume> OnEnterVolume;

	// Token: 0x04000323 RID: 803
	public static Action<PostEffectsVolume> OnExitVolume;

	// Token: 0x04000324 RID: 804
	public PostEffectsProfile Profile;
}
