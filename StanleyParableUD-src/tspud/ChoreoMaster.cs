using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Token: 0x02000092 RID: 146
public class ChoreoMaster : Singleton<ChoreoMaster>
{
	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000380 RID: 896 RVA: 0x000171DF File Offset: 0x000153DF
	public static SubtitleUI SubtitleUIInstance
	{
		get
		{
			return Singleton<ChoreoMaster>.Instance.subtitlesUI;
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x000171EC File Offset: 0x000153EC
	public static string GetAssetBundleFilenameFromVoiceClip(VoiceClip voiceClip, StanleyPlatform platform, bool hasBucket = false)
	{
		string voiceAudioClipBaseName = voiceClip.GetVoiceAudioClipBaseName(platform, hasBucket);
		return "voice_" + voiceAudioClipBaseName;
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00017210 File Offset: 0x00015410
	protected override void Awake()
	{
		base.Awake();
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
		CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
		CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
		if (this != Singleton<ChoreoMaster>.Instance)
		{
			Object.DestroyImmediate(base.gameObject);
			return;
		}
		this.CacheAvailableDubs();
		this.OnAudioDubChange("default");
		this.source = base.GetComponent<AudioSource>();
		if (!this.source)
		{
			this.source = base.gameObject.AddComponent<AudioSource>();
		}
		this.soundscapeSource1 = this.soundscapeChild.AddComponent<AudioSource>();
		this.soundscapeSource2 = this.soundscapeChild.AddComponent<AudioSource>();
		this.soundscapeSource1.bypassReverbZones = true;
		this.soundscapeSource2.bypassReverbZones = true;
		this.soundscapeSource1.outputAudioMixerGroup = this.ambienceMixer;
		this.soundscapeSource2.outputAudioMixerGroup = this.ambienceMixer;
		GameMaster.OnPause += this.Pause;
		GameMaster.OnResume += this.Resume;
		this.currentContentWarningConfigurable.Init();
		GameMaster.OnPrepareLoadingLevel += this.OnPrepareLoadingLevel;
		this.FindSoundScapes();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0001735A File Offset: 0x0001555A
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.FindSoundScapes();
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00017364 File Offset: 0x00015564
	private void FindSoundScapes()
	{
		this.scapes.Clear();
		this.currentSoundscapeClip = null;
		foreach (Soundscape soundscape in Object.FindObjectsOfType<Soundscape>())
		{
			this.RegisterSoundscape(soundscape);
		}
	}

	// Token: 0x06000385 RID: 901 RVA: 0x000173A2 File Offset: 0x000155A2
	private void OnPrepareLoadingLevel()
	{
		this.currentContentWarningConfigurable.SetValue("");
	}

	// Token: 0x06000386 RID: 902 RVA: 0x000173B4 File Offset: 0x000155B4
	private void OnDisable()
	{
		GameMaster.OnPause -= this.Pause;
		GameMaster.OnResume -= this.Resume;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x000173D8 File Offset: 0x000155D8
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x000173EC File Offset: 0x000155EC
	private void CacheAvailableDubs()
	{
		string manifestAssetBundleName = ChoreoMaster.GetManifestAssetBundleName();
		if (!File.Exists(Path.Combine(Application.streamingAssetsPath, manifestAssetBundleName)))
		{
			return;
		}
		string[] allAssetBundles = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, manifestAssetBundleName)).LoadAsset<AssetBundleManifest>("assetbundlemanifest").GetAllAssetBundles();
		int num = 0;
		foreach (string text in allAssetBundles)
		{
			if (text.Contains("voice"))
			{
				ChoreoMaster.voiceVariants.Add(text);
				string text2 = text.Split(new string[] { "." }, StringSplitOptions.None)[1].ToLower();
				ChoreoMaster.languageCodeAssetBundleDictionary.Add(text2, num);
				num++;
			}
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00017491 File Offset: 0x00015691
	public static string GetManifestAssetBundleName()
	{
		return "StreamingAssets";
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00017498 File Offset: 0x00015698
	private void OnAudioDubChange(string languageCode)
	{
		if (ChoreoMaster.VOICEASSETBUNDLE != null)
		{
			ChoreoMaster.VOICEASSETBUNDLE.Unload(true);
		}
		int num = 0;
		languageCode = languageCode.ToLower();
		if (ChoreoMaster.languageCodeAssetBundleDictionary.TryGetValue(languageCode, out num))
		{
			string text = ChoreoMaster.voiceVariants[num];
			ChoreoMaster.VOICEASSETBUNDLE = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, text));
			ChoreoMaster.CURRENTLANGUAGECODEVOICE = languageCode;
			return;
		}
		foreach (KeyValuePair<string, int> keyValuePair in ChoreoMaster.languageCodeAssetBundleDictionary)
		{
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0001753C File Offset: 0x0001573C
	private void Update()
	{
		if (Singleton<GameMaster>.Instance != null && Singleton<GameMaster>.Instance.closedCaptionsOn)
		{
			this.canvas.planeDistance = 0.054f;
			this.canvas.targetDisplay = 0;
		}
		else
		{
			this.canvas.planeDistance = -1f;
			this.canvas.targetDisplay = 1;
		}
		if (this.currentEvent.Clip != null && this.waitRoutine == null)
		{
			if (this.currentEvent.owner != null)
			{
				this.currentEvent.owner.FinishedEvent(this.currentEvent);
			}
			this.CheckAvailableEvents();
		}
		if (this.waitRoutine == null)
		{
			this.subtitlesUI.HideSubtitlesWithFade();
		}
		if (this.scapes.Count > 0)
		{
			int num = -1;
			float num2 = float.PositiveInfinity;
			for (int i = 0; i < this.scapes.Count; i++)
			{
				float num3;
				if (this.scapes[i].WithhinDistance(out num3) && num3 < num2)
				{
					num2 = num3;
					num = i;
				}
			}
			if (num != -1 && this.scapes[num].clip != this.currentSoundscapeClip)
			{
				this.currentSoundscapeClip = this.scapes[num].clip;
				if (this.scapeChangeCoroutine != null)
				{
					base.StopCoroutine(this.scapeChangeCoroutine);
				}
				if (this.soundscapeToUse == 1)
				{
					this.scapeChangeCoroutine = this.ChangeSoundscape(this.scapes[num], this.soundscapeSource1, this.soundscapeSource2);
					base.StartCoroutine(this.scapeChangeCoroutine);
					this.soundscapeToUse = 2;
					return;
				}
				this.scapeChangeCoroutine = this.ChangeSoundscape(this.scapes[num], this.soundscapeSource2, this.soundscapeSource1);
				base.StartCoroutine(this.scapeChangeCoroutine);
				this.soundscapeToUse = 1;
			}
		}
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00017712 File Offset: 0x00015912
	private void Pause()
	{
		this.source.Pause();
		this.soundscapeSource1.Pause();
		this.soundscapeSource2.Pause();
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00017735 File Offset: 0x00015935
	private void Resume()
	{
		this.source.UnPause();
		this.soundscapeSource1.UnPause();
		this.soundscapeSource2.UnPause();
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00017758 File Offset: 0x00015958
	private void CheckAvailableEvents()
	{
		if (this.upcomingEvents.Count > 0)
		{
			this.currentEvent = this.upcomingEvents[0];
			this.upcomingEvents.RemoveAt(0);
			this.PlayClip(this.currentEvent);
			return;
		}
		if (this.currentEvent.owner != null)
		{
			this.currentEvent.owner.FinishedEvent(this.currentEvent);
		}
		this.currentEvent = new ChoreographedEvent();
		if (this.waitRoutine != null)
		{
			base.StopCoroutine(this.waitRoutine);
		}
		this.subtitlesUI.HideSubtitlesWithFade();
	}

	// Token: 0x0600038F RID: 911 RVA: 0x000177F4 File Offset: 0x000159F4
	private void PlayClip(ChoreographedEvent choreoEvent)
	{
		if (this.waitRoutine != null)
		{
			base.StopCoroutine(this.waitRoutine);
		}
		AudioClip audioClip = choreoEvent.GetAudioClip();
		this.source.clip = audioClip;
		if (audioClip != null)
		{
			this.source.Play();
		}
		this.waitRoutine = base.StartCoroutine(this.WaitForClip(choreoEvent.Clip, audioClip));
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00017855 File Offset: 0x00015A55
	private IEnumerator WaitForClip(VoiceClip voiceClip, AudioClip audioClip)
	{
		float clipDuration = 1.5f;
		if (audioClip != null)
		{
			clipDuration = audioClip.length;
		}
		this.subtitlesUI.NewSubtitle(voiceClip.GetVoiceAudioClipBaseName(PlatformSettings.GetCurrentRunningPlatform(), BucketController.HASBUCKET), clipDuration);
		float timer = 0f;
		while (timer < clipDuration)
		{
			timer += Time.deltaTime * ChoreoMaster.GameSpeed;
			yield return null;
		}
		this.source.clip = null;
		this.waitRoutine = null;
		this.CheckAvailableEvents();
		yield break;
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00017874 File Offset: 0x00015A74
	public void BeginEvents(List<ChoreographedEvent> events, ChoreographedScene.InteruptBehaviour behaviour, bool spawnflag49)
	{
		List<ChoreographedEvent> list = new List<ChoreographedEvent>(events);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].StartPreloadOfDynamicClip(this);
		}
		if (behaviour == ChoreographedScene.InteruptBehaviour.StartImmediately || spawnflag49)
		{
			if (list.Count > 0)
			{
				this.source.Stop();
				this.currentEvent = list[0];
				this.upcomingEvents = list;
				this.CheckAvailableEvents();
				return;
			}
			this.upcomingEvents.Clear();
			return;
		}
		else if (behaviour == ChoreographedScene.InteruptBehaviour.CancelAtNextEvent)
		{
			if (list.Count > 0)
			{
				for (int j = 0; j < this.upcomingEvents.Count; j++)
				{
					if (this.upcomingEvents[j].owner != null)
					{
						this.upcomingEvents[j].owner.Cancelled();
					}
				}
				this.upcomingEvents = list;
				this.CheckAvailableEvents();
				return;
			}
			this.upcomingEvents.Clear();
			return;
		}
		else
		{
			if (behaviour == ChoreographedScene.InteruptBehaviour.InterruptAtNextEvent)
			{
				for (int k = 0; k < this.upcomingEvents.Count; k++)
				{
					if (this.upcomingEvents[k].owner != null)
					{
						this.upcomingEvents[k].owner.Cancelled();
					}
				}
				this.upcomingEvents.InsertRange(0, list);
				this.CheckAvailableEvents();
				return;
			}
			if (behaviour == ChoreographedScene.InteruptBehaviour.WaitForFinish)
			{
				for (int l = 0; l < list.Count; l++)
				{
					this.upcomingEvents.Add(list[l]);
				}
				this.CheckAvailableEvents();
			}
			return;
		}
	}

	// Token: 0x06000392 RID: 914 RVA: 0x000179E4 File Offset: 0x00015BE4
	public void DropAll()
	{
		this.source.Stop();
		if (this.waitRoutine != null)
		{
			base.StopCoroutine(this.waitRoutine);
		}
		this.currentEvent = new ChoreographedEvent();
		this.upcomingEvents.Clear();
		this.subtitlesUI.HideSubtitlesImmediate();
		this.CheckAvailableEvents();
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00017A38 File Offset: 0x00015C38
	public void DropOwnedEvents(ChoreographedScene owner)
	{
		for (int i = this.upcomingEvents.Count - 1; i >= 0; i--)
		{
			if (this.upcomingEvents[i].owner == owner)
			{
				this.upcomingEvents.RemoveAt(i);
			}
		}
		if (this.currentEvent.owner == owner)
		{
			this.CheckAvailableEvents();
		}
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00017A9C File Offset: 0x00015C9C
	public void NearbySoundscape(AudioClip clip, float distance, float pitch, float volume, float fadetime)
	{
		ChoreoMaster.Scape scape = new ChoreoMaster.Scape();
		scape.clip = clip;
		scape.volume = volume;
		scape.pitch = pitch;
		scape.fadetime = fadetime;
		this.scapes.Add(scape);
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00017ADC File Offset: 0x00015CDC
	private void RegisterSoundscape(Soundscape scape)
	{
		ChoreoMaster.Scape scape2 = new ChoreoMaster.Scape();
		scape2.clip = scape.clip;
		scape2.position = scape.gameObject.transform.position;
		scape2.radius = scape.radius;
		scape2.volume = scape.volume;
		scape2.pitch = scape.pitch;
		scape2.fadetime = scape.fadetime;
		scape2.playerTransform = StanleyController.Instance.transform;
		this.scapes.Add(scape2);
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00017B5D File Offset: 0x00015D5D
	private IEnumerator ChangeSoundscape(ChoreoMaster.Scape scape, AudioSource inSource, AudioSource outSource)
	{
		fint32 startTime = Time.realtimeSinceStartup;
		fint32 endTime = startTime + scape.fadetime;
		inSource.clip = scape.clip;
		inSource.pitch = scape.pitch;
		inSource.loop = true;
		inSource.volume = 0f;
		inSource.Play();
		float inTargetVol = scape.volume;
		float outStartVol = outSource.volume;
		while (Time.realtimeSinceStartup < endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Time.realtimeSinceStartup);
			inSource.volume = num * inTargetVol;
			outSource.volume = (1f - num) * outStartVol;
			yield return new WaitForEndOfFrame();
		}
		inSource.volume = inTargetVol;
		outSource.Stop();
		yield break;
	}

	// Token: 0x04000372 RID: 882
	public static AssetBundle VOICEASSETBUNDLE;

	// Token: 0x04000373 RID: 883
	public static string CURRENTLANGUAGECODEVOICE = "default";

	// Token: 0x04000374 RID: 884
	public static string CURRENTLANGUAGECODETEXT = "en";

	// Token: 0x04000375 RID: 885
	public static readonly List<string> voiceVariants = new List<string>();

	// Token: 0x04000376 RID: 886
	public static readonly Dictionary<string, int> languageCodeAssetBundleDictionary = new Dictionary<string, int>();

	// Token: 0x04000377 RID: 887
	public static float GameSpeed = 1f;

	// Token: 0x04000378 RID: 888
	[SerializeField]
	private AudioSource source;

	// Token: 0x04000379 RID: 889
	private ChoreographedEvent currentEvent = new ChoreographedEvent();

	// Token: 0x0400037A RID: 890
	private List<ChoreographedEvent> upcomingEvents = new List<ChoreographedEvent>();

	// Token: 0x0400037B RID: 891
	private Coroutine waitRoutine;

	// Token: 0x0400037C RID: 892
	private IEnumerator scapeChangeCoroutine;

	// Token: 0x0400037D RID: 893
	public GameObject soundscapeChild;

	// Token: 0x0400037E RID: 894
	private AudioSource soundscapeSource1;

	// Token: 0x0400037F RID: 895
	private AudioSource soundscapeSource2;

	// Token: 0x04000380 RID: 896
	public AudioMixerGroup ambienceMixer;

	// Token: 0x04000381 RID: 897
	public Canvas canvas;

	// Token: 0x04000382 RID: 898
	[SerializeField]
	private AudioClip currentSoundscapeClip;

	// Token: 0x04000383 RID: 899
	[SerializeField]
	private List<ChoreoMaster.Scape> scapes = new List<ChoreoMaster.Scape>();

	// Token: 0x04000384 RID: 900
	private int soundscapeToUse = 1;

	// Token: 0x04000385 RID: 901
	[SerializeField]
	private SubtitleUI subtitlesUI;

	// Token: 0x04000386 RID: 902
	[Header("Content Warning")]
	public StringConfigurable currentContentWarningConfigurable;

	// Token: 0x02000390 RID: 912
	[Serializable]
	private class Scape
	{
		// Token: 0x0600166C RID: 5740 RVA: 0x00076C6D File Offset: 0x00074E6D
		public bool WithhinDistance(out float distance)
		{
			distance = Vector3.Distance(this.position, this.playerTransform.position);
			return distance <= this.radius;
		}

		// Token: 0x04001304 RID: 4868
		public AudioClip clip;

		// Token: 0x04001305 RID: 4869
		public float radius;

		// Token: 0x04001306 RID: 4870
		public Vector3 position;

		// Token: 0x04001307 RID: 4871
		public float volume;

		// Token: 0x04001308 RID: 4872
		public float pitch;

		// Token: 0x04001309 RID: 4873
		public float fadetime;

		// Token: 0x0400130A RID: 4874
		public Transform playerTransform;
	}
}
