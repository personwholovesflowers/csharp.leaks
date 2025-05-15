using System;
using UnityEngine;

// Token: 0x020001CD RID: 461
[CreateAssetMenu(fileName = "New Voice Clip", menuName = "Stanley/New Voice Clip")]
public class VoiceClip : ScriptableObject
{
	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000A88 RID: 2696 RVA: 0x000315BD File Offset: 0x0002F7BD
	// (set) Token: 0x06000A89 RID: 2697 RVA: 0x000315C5 File Offset: 0x0002F7C5
	public string DEBUG_TAG_WindowsPlayer { get; private set; }

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000A8A RID: 2698 RVA: 0x000315CE File Offset: 0x0002F7CE
	// (set) Token: 0x06000A8B RID: 2699 RVA: 0x000315D6 File Offset: 0x0002F7D6
	public string DEBUG_TAG_PS4 { get; private set; }

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000A8C RID: 2700 RVA: 0x000315DF File Offset: 0x0002F7DF
	// (set) Token: 0x06000A8D RID: 2701 RVA: 0x000315E7 File Offset: 0x0002F7E7
	public string DEBUG_TAG_PS5 { get; private set; }

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000A8E RID: 2702 RVA: 0x000315F0 File Offset: 0x0002F7F0
	// (set) Token: 0x06000A8F RID: 2703 RVA: 0x000315F8 File Offset: 0x0002F7F8
	public string DEBUG_TAG_XBOX360 { get; private set; }

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00031601 File Offset: 0x0002F801
	// (set) Token: 0x06000A91 RID: 2705 RVA: 0x00031609 File Offset: 0x0002F809
	public string DEBUG_TAG_XboxOne { get; private set; }

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00031612 File Offset: 0x0002F812
	// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0003161A File Offset: 0x0002F81A
	public string DEBUG_TAG_Switch { get; private set; }

	// Token: 0x06000A94 RID: 2708 RVA: 0x00031624 File Offset: 0x0002F824
	private void OnValidate()
	{
		this.DEBUG_TAG_WindowsPlayer = this.GetVoiceAudioClipBaseName(RuntimePlatform.WindowsPlayer, false);
		this.DEBUG_TAG_PS4 = this.GetVoiceAudioClipBaseName(PlatformSettings.GetStanleyPlatform(RuntimePlatform.PS4), false);
		this.DEBUG_TAG_PS5 = this.GetVoiceAudioClipBaseName(PlatformSettings.GetStanleyPlatform(RuntimePlatform.PS5), false);
		this.DEBUG_TAG_XBOX360 = this.GetVoiceAudioClipBaseName(PlatformSettings.GetStanleyPlatform(RuntimePlatform.XBOX360), false);
		this.DEBUG_TAG_XboxOne = this.GetVoiceAudioClipBaseName(PlatformSettings.GetStanleyPlatform(RuntimePlatform.XboxOne), false);
		this.DEBUG_TAG_Switch = this.GetVoiceAudioClipBaseName(PlatformSettings.GetStanleyPlatform(RuntimePlatform.Switch), false);
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000A95 RID: 2709 RVA: 0x000316A3 File Offset: 0x0002F8A3
	public string AudioClipFolderName
	{
		get
		{
			if (!(this.AudioClipFolder == ""))
			{
				return this.AudioClipFolder + "/";
			}
			return "";
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x000316CD File Offset: 0x0002F8CD
	private void OnEnable()
	{
		if (this.AudioClipBasename.Contains("_EN"))
		{
			this.AudioClipBasename = this.AudioClipBasename.Replace("_EN", "");
		}
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x000316FC File Offset: 0x0002F8FC
	public string GetVoiceAudioClipBaseName(RuntimePlatform runtimeplatform, bool useBucketIfAvailable)
	{
		return LocalizationTagTools.GetVoiceAudioClipBaseName(this.AudioClipBasename, PlatformSettings.GetStanleyPlatform(runtimeplatform), this.PlatformVariations, useBucketIfAvailable, this.HasBucketClip);
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0003171C File Offset: 0x0002F91C
	public string GetVoiceAudioClipBaseName(StanleyPlatform platform, bool useBucketIfAvailable)
	{
		return LocalizationTagTools.GetVoiceAudioClipBaseName(this.AudioClipBasename, platform, this.PlatformVariations, useBucketIfAvailable, this.HasBucketClip);
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00031737 File Offset: 0x0002F937
	public string GetVoiceAudioClipBaseName(bool useBucketIfAvailable)
	{
		return LocalizationTagTools.GetVoiceAudioClipBaseName(this.AudioClipBasename, PlatformSettings.GetCurrentRunningPlatform(), this.PlatformVariations, useBucketIfAvailable, this.HasBucketClip);
	}

	// Token: 0x04000A7D RID: 2685
	[InspectorButton("PingAudioClip", "Ping Audio Clip", ButtonWidth = 300f)]
	public string AudioClipBasename = "";

	// Token: 0x04000A7E RID: 2686
	[SerializeField]
	private string AudioClipFolder = "";

	// Token: 0x04000A7F RID: 2687
	public bool HasBucketClip;

	// Token: 0x04000A80 RID: 2688
	public PlatformTag[] PlatformVariations = new PlatformTag[0];
}
