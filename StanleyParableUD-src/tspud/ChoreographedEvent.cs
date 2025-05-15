using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200009B RID: 155
[Serializable]
public class ChoreographedEvent
{
	// Token: 0x060003CB RID: 971 RVA: 0x00018704 File Offset: 0x00016904
	public void PreloadAudioClips()
	{
		this.usePreloadedClips = true;
		string assetBundleFilenameFromVoiceClip = ChoreoMaster.GetAssetBundleFilenameFromVoiceClip(this.Clip, PlatformSettings.GetCurrentRunningPlatform(), false);
		if (ChoreoMaster.VOICEASSETBUNDLE == null || !ChoreoMaster.VOICEASSETBUNDLE.Contains(assetBundleFilenameFromVoiceClip))
		{
			return;
		}
		this.preloadedClip = ChoreoMaster.VOICEASSETBUNDLE.LoadAsset<AudioClip>(assetBundleFilenameFromVoiceClip);
		if (this.Clip.HasBucketClip)
		{
			string assetBundleFilenameFromVoiceClip2 = ChoreoMaster.GetAssetBundleFilenameFromVoiceClip(this.Clip, PlatformSettings.GetCurrentRunningPlatform(), true);
			if (!ChoreoMaster.VOICEASSETBUNDLE.Contains(assetBundleFilenameFromVoiceClip2))
			{
				return;
			}
			this.preloadedClipBucket = ChoreoMaster.VOICEASSETBUNDLE.LoadAsset<AudioClip>(assetBundleFilenameFromVoiceClip2);
		}
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00018794 File Offset: 0x00016994
	public void StartPreloadOfDynamicClip(MonoBehaviour behaviour)
	{
		if (this.usePreloadedClips)
		{
			return;
		}
		if (this.loadDynamicRoutine != null)
		{
			behaviour.StopCoroutine(this.loadDynamicRoutine);
		}
		this.loadDynamicRoutine = behaviour.StartCoroutine(this.PreloadDynamicClipAsync());
	}

	// Token: 0x060003CD RID: 973 RVA: 0x000187C5 File Offset: 0x000169C5
	private IEnumerator PreloadDynamicClipAsync()
	{
		string assetBundleFilenameFromVoiceClip = ChoreoMaster.GetAssetBundleFilenameFromVoiceClip(this.Clip, PlatformSettings.GetCurrentRunningPlatform(), BucketController.HASBUCKET);
		if (ChoreoMaster.VOICEASSETBUNDLE == null || !ChoreoMaster.VOICEASSETBUNDLE.Contains(assetBundleFilenameFromVoiceClip))
		{
			this.loadDynamicRoutine = null;
			yield break;
		}
		AssetBundleRequest clipRequest = ChoreoMaster.VOICEASSETBUNDLE.LoadAssetAsync<AudioClip>(assetBundleFilenameFromVoiceClip);
		clipRequest.allowSceneActivation = true;
		while (!clipRequest.isDone)
		{
			yield return null;
		}
		this.dynamicClip = clipRequest.asset as AudioClip;
		this.loadDynamicRoutine = null;
		yield break;
	}

	// Token: 0x060003CE RID: 974 RVA: 0x000187D4 File Offset: 0x000169D4
	public AudioClip GetAudioClip()
	{
		if (!this.usePreloadedClips)
		{
			return this.dynamicClip;
		}
		if (!BucketController.HASBUCKET || !(this.preloadedClipBucket != null))
		{
			return this.preloadedClip;
		}
		return this.preloadedClipBucket;
	}

	// Token: 0x040003BD RID: 957
	public VoiceClip Clip;

	// Token: 0x040003BE RID: 958
	[HideInInspector]
	public ChoreographedScene owner;

	// Token: 0x040003BF RID: 959
	private AudioClip preloadedClip;

	// Token: 0x040003C0 RID: 960
	private AudioClip preloadedClipBucket;

	// Token: 0x040003C1 RID: 961
	private AudioClip dynamicClip;

	// Token: 0x040003C2 RID: 962
	private bool usePreloadedClips;

	// Token: 0x040003C3 RID: 963
	private Coroutine loadDynamicRoutine;
}
