using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020001CE RID: 462
[RequireComponent(typeof(AudioSource))]
public class VoiceLoadAssetBundle : MonoBehaviour
{
	// Token: 0x06000A9B RID: 2715 RVA: 0x00031780 File Offset: 0x0002F980
	private void Start()
	{
		this.audioSource = base.GetComponent<AudioSource>();
		string[] allAssetBundlesWithVariant = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "Switch")).LoadAsset<AssetBundleManifest>("assetbundlemanifest").GetAllAssetBundlesWithVariant();
		for (int i = 0; i < allAssetBundlesWithVariant.Length; i++)
		{
			string text = allAssetBundlesWithVariant[i];
			if (text.Contains("voice"))
			{
				this.voiceVariants.Add(text);
				string text2 = text.Split(new string[] { "." }, StringSplitOptions.None)[1].ToLower();
				this.languageCodeAssetBundleDictionary.Add(text2, i);
			}
		}
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00031812 File Offset: 0x0002FA12
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.LoadAndPlay();
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00031824 File Offset: 0x0002FA24
	private void LoadAndPlay()
	{
		if (this.currentVoiceBundle != null)
		{
			this.currentVoiceBundle.Unload(true);
		}
		int num = 0;
		this.wantedLanguageCode = this.wantedLanguageCode.ToLower();
		if (this.languageCodeAssetBundleDictionary.TryGetValue(this.wantedLanguageCode, out num))
		{
			string text = this.voiceVariants[num];
			this.currentVoiceBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, text));
			string text2 = this.audioClipToLoad + "_" + this.wantedLanguageCode;
			AudioClip audioClip = this.currentVoiceBundle.LoadAsset<AudioClip>(text2);
			this.audioSource.clip = audioClip;
			this.audioSource.Play();
		}
	}

	// Token: 0x04000A81 RID: 2689
	private AudioSource audioSource;

	// Token: 0x04000A82 RID: 2690
	[SerializeField]
	private string wantedLanguageCode = "en";

	// Token: 0x04000A83 RID: 2691
	[SerializeField]
	private string audioClipToLoad = "";

	// Token: 0x04000A84 RID: 2692
	[SerializeField]
	private List<string> voiceVariants = new List<string>();

	// Token: 0x04000A85 RID: 2693
	[SerializeField]
	private Dictionary<string, int> languageCodeAssetBundleDictionary = new Dictionary<string, int>();

	// Token: 0x04000A86 RID: 2694
	[SerializeField]
	private AssetBundle currentVoiceBundle;
}
