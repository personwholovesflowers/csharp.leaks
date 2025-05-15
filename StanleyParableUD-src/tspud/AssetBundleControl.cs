using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000024 RID: 36
public class AssetBundleControl : MonoBehaviour
{
	// Token: 0x060000CF RID: 207 RVA: 0x000083CC File Offset: 0x000065CC
	private void Awake()
	{
		foreach (ReleaseMap releaseMap in this.releaseMaps)
		{
			foreach (ReleaseBundle releaseBundle in releaseMap.Bundles)
			{
				AssetBundleControl.allListedAssetBundles.Add(releaseBundle.name);
			}
			foreach (ReleaseMap.ReleaseScene releaseScene in releaseMap.Scenes)
			{
				AssetBundleControl.releaseScenes.Add(releaseScene);
			}
		}
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00008490 File Offset: 0x00006690
	private static AssetBundle LoadAssetBundle(string bundleName)
	{
		string text = Path.Combine(Application.streamingAssetsPath, bundleName);
		if (!File.Exists(text))
		{
			return null;
		}
		AssetBundle assetBundle;
		if (AssetBundleControl.loadIntoMemory)
		{
			assetBundle = AssetBundle.LoadFromMemory(File.ReadAllBytes(text));
		}
		else
		{
			assetBundle = AssetBundle.LoadFromFile(text);
		}
		return assetBundle;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000084D2 File Offset: 0x000066D2
	private static IEnumerator LoadAssetBundleAsync(string bundleName, Action<AssetBundle> callbackBundle, Action<float> callbackLoadProgress = null)
	{
		string text = Path.Combine(Application.streamingAssetsPath, bundleName.ToLower());
		if (!File.Exists(text))
		{
			yield break;
		}
		if (AssetBundleControl.loadIntoMemory)
		{
			byte[] array = File.ReadAllBytes(text);
			AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromMemoryAsync(array);
			while (!bundleRequest.isDone)
			{
				if (callbackLoadProgress != null)
				{
					callbackLoadProgress(bundleRequest.progress);
				}
				yield return null;
			}
			if (callbackLoadProgress != null)
			{
				callbackLoadProgress(1f);
			}
			if (callbackBundle != null && bundleRequest.assetBundle != null)
			{
				callbackBundle(bundleRequest.assetBundle);
			}
			bundleRequest = null;
		}
		else
		{
			AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(text);
			while (!bundleRequest.isDone)
			{
				if (callbackLoadProgress != null)
				{
					callbackLoadProgress(bundleRequest.progress);
				}
				yield return null;
			}
			if (callbackLoadProgress != null)
			{
				callbackLoadProgress(1f);
			}
			if (callbackBundle != null && bundleRequest.assetBundle != null)
			{
				callbackBundle(bundleRequest.assetBundle);
			}
			bundleRequest = null;
		}
		yield break;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x000084EF File Offset: 0x000066EF
	private static void SceneInBuildIndexOrAssetBundle(string sceneName, ref bool inBuildIndex, ref bool inAssetBundles)
	{
		inBuildIndex = Application.CanStreamedLevelBeLoaded(sceneName);
		inAssetBundles = File.Exists(Path.Combine(Application.streamingAssetsPath, sceneName));
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x0000850C File Offset: 0x0000670C
	private static bool ReleaseSceneAvailable(string sceneName, out ReleaseMap.ReleaseScene releaseScene)
	{
		for (int i = 0; i < AssetBundleControl.releaseScenes.Count; i++)
		{
			ReleaseMap.ReleaseScene releaseScene2 = AssetBundleControl.releaseScenes[i];
			if (releaseScene2.SceneName.ToLower().Equals(sceneName.ToLower()))
			{
				releaseScene = releaseScene2;
				return true;
			}
		}
		releaseScene = null;
		return false;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0000855C File Offset: 0x0000675C
	public static bool ChangeScene(string sceneName, string loadingSceneName, MonoBehaviour behaviour)
	{
		ReleaseMap.ReleaseScene releaseScene = null;
		sceneName = sceneName.ToLower();
		return AssetBundleControl.ReleaseSceneAvailable(sceneName, out releaseScene) && AssetBundleControl.ChangeScene(releaseScene, loadingSceneName, behaviour);
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00008588 File Offset: 0x00006788
	public static bool ChangeScene(ReleaseMap.ReleaseScene releaseScene, string loadingSceneName, MonoBehaviour behaviour)
	{
		bool flag = false;
		bool flag2 = false;
		string text = releaseScene.SceneName.ToLower();
		loadingSceneName = loadingSceneName.ToLower();
		AssetBundleControl.SceneInBuildIndexOrAssetBundle(text, ref flag, ref flag2);
		if (!flag && !flag2)
		{
			return false;
		}
		if (flag)
		{
			if (AssetBundleControl.changeSceneRoutine == null)
			{
				AssetBundleControl.changeSceneRoutine = behaviour.StartCoroutine(AssetBundleControl.LoadSceneInBuildIndex(text, loadingSceneName, true));
				return true;
			}
			return false;
		}
		else
		{
			if (!flag2)
			{
				return false;
			}
			if (AssetBundleControl.changeSceneRoutine == null)
			{
				AssetBundleControl.changeSceneRoutine = behaviour.StartCoroutine(AssetBundleControl.LoadSceneFromAssetBundle(releaseScene, behaviour, true, loadingSceneName));
				return true;
			}
			return false;
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00008603 File Offset: 0x00006803
	private static IEnumerator LoadSceneFromAssetBundle(ReleaseMap.ReleaseScene releaseScene, MonoBehaviour behaviour, bool useLoadingScreen = false, string loadingSceneName = "")
	{
		if (useLoadingScreen)
		{
			if (loadingSceneName != "" && AssetBundleControl.loadingSceneAssetBundle == null)
			{
				AssetBundleControl.loadingSceneAssetBundle = AssetBundleControl.LoadAssetBundle(loadingSceneName);
				if (AssetBundleControl.loadingSceneAssetBundle != null)
				{
					AsyncOperation loadingSceneAsync = AssetBundleControl.GatherAndActivateSceneFromBundle(AssetBundleControl.loadingSceneAssetBundle, ref AssetBundleControl.currentLoadingSceneName);
					while (loadingSceneAsync != null && !loadingSceneAsync.isDone)
					{
						yield return null;
					}
					loadingSceneAsync = null;
				}
			}
			else if (loadingSceneName != "" && AssetBundleControl.loadingSceneAssetBundle != null)
			{
				AsyncOperation loadingSceneAsync = AssetBundleControl.GatherAndActivateSceneFromBundle(AssetBundleControl.loadingSceneAssetBundle, ref AssetBundleControl.currentLoadingSceneName);
				while (loadingSceneAsync != null && !loadingSceneAsync.isDone)
				{
					yield return null;
				}
				loadingSceneAsync = null;
			}
		}
		if (AssetBundleControl.OnScenePreLoad != null)
		{
			AssetBundleControl.OnScenePreLoad();
		}
		string sceneBundleName = releaseScene.SceneName.ToLower();
		AssetBundleControl.currentGameSceneName != sceneBundleName;
		bool changeSetDataRequired = AssetBundleControl.currentAssetSet != releaseScene.Set;
		bool flag = true;
		AssetBundleControl.currentGameSceneName != "";
		if (flag)
		{
			if (AssetBundleControl.currentSceneDataBundle != null)
			{
				AssetBundleControl.currentSceneDataBundle.Unload(true);
			}
			if (AssetBundleControl.currentSceneBundle != null)
			{
				AssetBundleControl.currentSceneBundle.Unload(true);
			}
			for (int i = 0; i < AssetBundleControl.currentSceneExclusiveAssetBundles.Count; i++)
			{
				AssetBundleControl.currentSceneExclusiveAssetBundles[i].Unload(true);
			}
			AssetBundleControl.currentSceneExclusiveAssetBundles.Clear();
			if (changeSetDataRequired)
			{
				for (int j = 0; j < AssetBundleControl.currentSetAssetBundles.Count; j++)
				{
					AssetBundleControl.currentSetAssetBundles[j].Unload(true);
				}
				AssetBundleControl.currentSetAssetBundles.Clear();
			}
			AssetBundleControl.currentAssetSet = releaseScene.Set;
			string text = sceneBundleName + "_sceneassets";
			if (File.Exists(Path.Combine(Application.streamingAssetsPath, text)))
			{
				yield return behaviour.StartCoroutine(AssetBundleControl.LoadAssetBundleAsync(text, delegate(AssetBundle newBundle)
				{
					if (newBundle != null)
					{
						AssetBundleControl.currentSceneDataBundle = newBundle;
					}
				}, delegate(float progress)
				{
					AssetBundleControl.BroadcastLoadProgressUpdate(progress, 0f, 0.2f);
				}));
			}
			else
			{
				AssetBundleControl.currentSceneDataBundle = null;
			}
			AssetBundleControl.<>c__DisplayClass33_0 CS$<>8__locals1 = new AssetBundleControl.<>c__DisplayClass33_0();
			CS$<>8__locals1.i = 0;
			while (CS$<>8__locals1.i < AssetBundleControl.allListedAssetBundles.Count)
			{
				string text2 = AssetBundleControl.allListedAssetBundles[CS$<>8__locals1.i];
				string text3 = sceneBundleName + "_exclusive_" + text2 + ".default";
				yield return behaviour.StartCoroutine(AssetBundleControl.LoadAssetBundleAsync(text3, delegate(AssetBundle newBundle)
				{
					if (newBundle != null)
					{
						AssetBundleControl.currentSceneExclusiveAssetBundles.Add(newBundle);
					}
				}, delegate(float progress)
				{
					AssetBundleControl.BroadcastLoadProgressUpdate((float)(CS$<>8__locals1.i / AssetBundleControl.allListedAssetBundles.Count), 0.2f, 0.4f);
				}));
				int num = CS$<>8__locals1.i;
				CS$<>8__locals1.i = num + 1;
			}
			CS$<>8__locals1 = null;
			if (changeSetDataRequired && AssetBundleControl.currentAssetSet != null)
			{
				AssetBundleControl.<>c__DisplayClass33_1 CS$<>8__locals2 = new AssetBundleControl.<>c__DisplayClass33_1();
				CS$<>8__locals2.i = 0;
				while (CS$<>8__locals2.i < AssetBundleControl.LoadedDefaultBundles.Count)
				{
					string name = AssetBundleControl.LoadedDefaultBundles[CS$<>8__locals2.i].name;
					int num2 = name.IndexOf(".");
					if (num2 != -1)
					{
						string text4 = name.Insert(num2, "_" + AssetBundleControl.currentAssetSet.name);
						yield return behaviour.StartCoroutine(AssetBundleControl.LoadAssetBundleAsync(text4, delegate(AssetBundle newBundle)
						{
							if (newBundle != null)
							{
								AssetBundleControl.currentSetAssetBundles.Add(newBundle);
							}
						}, delegate(float progress)
						{
							AssetBundleControl.BroadcastLoadProgressUpdate((float)(CS$<>8__locals2.i / AssetBundleControl.allListedAssetBundles.Count), 0.4f, 0.6f);
						}));
					}
					int num = CS$<>8__locals2.i;
					CS$<>8__locals2.i = num + 1;
				}
				CS$<>8__locals2 = null;
			}
			yield return behaviour.StartCoroutine(AssetBundleControl.LoadAssetBundleAsync(sceneBundleName, delegate(AssetBundle newBundle)
			{
				if (newBundle != null)
				{
					AssetBundleControl.currentSceneBundle = newBundle;
				}
			}, delegate(float progress)
			{
				AssetBundleControl.BroadcastLoadProgressUpdate(progress, 0.6f, 0.8f);
			}));
			AssetBundleControl.BroadcastLoadProgressUpdate(0.9f, 0f, 1f);
		}
		AsyncOperation async = AssetBundleControl.GatherAndActivateSceneFromBundle(AssetBundleControl.currentSceneBundle, ref AssetBundleControl.currentGameSceneName);
		async.allowSceneActivation = false;
		while (async != null && !async.isDone)
		{
			AssetBundleControl.BroadcastLoadProgressUpdate(async.progress, 0.8f, 0.95f);
			if (async.progress >= 0.9f)
			{
				AssetBundleControl.BroadcastLoadProgressUpdate(1f, 0f, 1f);
				async.allowSceneActivation = true;
			}
			yield return null;
		}
		AssetBundleControl.BroadcastLoadProgressUpdate(1f, 0f, 1f);
		if (AssetBundleControl.OnSceneReady != null)
		{
			AssetBundleControl.OnSceneReady();
		}
		AssetBundleControl.changeSceneRoutine = null;
		yield break;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00008627 File Offset: 0x00006827
	private static AsyncOperation GatherAndActivateSceneFromBundle(AssetBundle bundle, ref string referenceString)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(bundle.GetAllScenePaths()[0]);
		referenceString = bundle.name;
		return SceneManager.LoadSceneAsync(fileNameWithoutExtension);
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00008643 File Offset: 0x00006843
	private static IEnumerator LoadSceneInBuildIndex(string mapName, string loadingSceneName, bool waitMinTime = true)
	{
		float time = Time.time;
		AsyncOperation loadlevelOperation = null;
		AsyncOperation unloadOperation = null;
		if (loadingSceneName != "")
		{
			loadlevelOperation = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
			yield return null;
			do
			{
				yield return null;
			}
			while (!loadlevelOperation.isDone);
			yield return null;
		}
		if (AssetBundleControl.currentGameSceneName == "")
		{
			AssetBundleControl.currentGameSceneName = SceneManager.GetActiveScene().name;
		}
		if (AssetBundleControl.currentGameSceneName != "")
		{
			unloadOperation = SceneManager.UnloadSceneAsync(AssetBundleControl.currentGameSceneName);
			while (unloadOperation != null && !unloadOperation.isDone)
			{
				yield return null;
			}
			yield return null;
		}
		AssetBundleControl.BroadcastLoadProgressUpdate(0.5f, 0f, 1f);
		yield return null;
		if (AssetBundleControl.OnScenePreLoad != null)
		{
			AssetBundleControl.OnScenePreLoad();
		}
		loadlevelOperation = SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive);
		loadlevelOperation.allowSceneActivation = false;
		yield return null;
		yield return null;
		do
		{
			AssetBundleControl.BroadcastLoadProgressUpdate(0.5f + loadlevelOperation.progress * 0.5f, 0f, 1f);
			yield return null;
		}
		while (loadlevelOperation.progress < 0.9f);
		AssetBundleControl.BroadcastLoadProgressUpdate(1f, 0f, 1f);
		loadlevelOperation.allowSceneActivation = true;
		do
		{
			yield return null;
		}
		while (!loadlevelOperation.isDone);
		if (loadingSceneName != "")
		{
			unloadOperation = SceneManager.UnloadSceneAsync(loadingSceneName);
			while (!unloadOperation.isDone)
			{
				yield return null;
			}
		}
		AssetBundleControl.currentGameSceneName = mapName;
		if (AssetBundleControl.OnSceneReady != null)
		{
			AssetBundleControl.OnSceneReady();
		}
		AssetBundleControl.changeSceneRoutine = null;
		yield break;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00008659 File Offset: 0x00006859
	private void Start()
	{
		if (this.autoStart)
		{
			this.StartLoadBundles();
		}
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00008669 File Offset: 0x00006869
	public void StartLoadBundles()
	{
		if (this.loadingBundles != null)
		{
			return;
		}
		this.loadingBundles = base.StartCoroutine(this.LoadBundles());
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00008686 File Offset: 0x00006886
	private IEnumerator LoadBundles()
	{
		Object.DontDestroyOnLoad(this);
		RuntimePlatform runtimePlatform = (this.forcePlatform ? this.customPlatform : Application.platform);
		AssetBundleControl.loadIntoMemory = this.platformConfiguration.LoadIntoMemory(runtimePlatform);
		List<string> assetBundlesToLoad = new List<string>();
		for (int j = 0; j < this.releaseMaps.Length; j++)
		{
			ReleaseMap releaseMap = this.releaseMaps[j];
			for (int k = 0; k < releaseMap.Bundles.Count; k++)
			{
				ReleaseBundle releaseBundle = releaseMap.Bundles[k];
				string text = releaseBundle.name.ToLower();
				BundleConfiguration bundleConfiguration;
				if (this.platformConfiguration.OverrideExists(runtimePlatform, releaseBundle, out bundleConfiguration))
				{
					switch (bundleConfiguration.IncludeOption)
					{
					case BundleConfiguration.IncludeOptions.UseDefault:
						assetBundlesToLoad.Add(text + ".default");
						break;
					case BundleConfiguration.IncludeOptions.UseVariant:
						assetBundlesToLoad.Add(text + "." + bundleConfiguration.GetCustomVariant());
						break;
					}
				}
				else
				{
					assetBundlesToLoad.Add(text + ".default");
				}
			}
		}
		string text2 = Path.Combine(Application.streamingAssetsPath, "universal_sceneassets");
		if (AssetBundleControl.loadIntoMemory)
		{
			yield return base.StartCoroutine(this.LoadAssetBundleIntoMemory(text2, AssetBundleControl.LoadedDefaultBundles));
		}
		else
		{
			yield return base.StartCoroutine(this.LoadAssetBundleFromFile(text2, AssetBundleControl.LoadedDefaultBundles));
		}
		int num;
		for (int i = 0; i < assetBundlesToLoad.Count; i = num + 1)
		{
			string text3 = Path.Combine(Application.streamingAssetsPath, assetBundlesToLoad[i]);
			if (File.Exists(text3))
			{
				if (AssetBundleControl.loadIntoMemory)
				{
					yield return base.StartCoroutine(this.LoadAssetBundleIntoMemory(text3, AssetBundleControl.LoadedDefaultBundles));
				}
				else
				{
					yield return base.StartCoroutine(this.LoadAssetBundleFromFile(text3, AssetBundleControl.LoadedDefaultBundles));
				}
				AssetBundleControl.BroadcastLoadProgressUpdate((float)i / (float)assetBundlesToLoad.Count, 0f, 1f);
			}
			num = i;
		}
		if (this.cancelLoadScene)
		{
			yield break;
		}
		if (this.debugLoadScene)
		{
			AssetBundleControl.ChangeScene(this.debugSceneName, "LoadingScene_UD_MASTER", this);
			yield break;
		}
		if (AssetBundleControl.releaseScenes.Count > 0)
		{
			string sceneName = AssetBundleControl.releaseScenes[0].SceneName;
			this.loadingBundles = null;
			AssetBundleControl.ChangeScene(sceneName, "LoadingScene_UD_MASTER", this);
			yield break;
		}
		yield break;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00008695 File Offset: 0x00006895
	private IEnumerator LoadAssetBundleFromFile(string bundlePath, List<AssetBundle> referenceList)
	{
		if (!File.Exists(bundlePath))
		{
			yield break;
		}
		AssetBundleCreateRequest resultAssetBundle = AssetBundle.LoadFromFileAsync(bundlePath);
		yield return new WaitWhile(() => !resultAssetBundle.isDone);
		referenceList.Add(resultAssetBundle.assetBundle);
		yield return null;
		yield break;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x000086AB File Offset: 0x000068AB
	private IEnumerator LoadAssetBundleIntoMemory(string bundlePath, List<AssetBundle> referenceList)
	{
		if (!File.Exists(bundlePath))
		{
			yield break;
		}
		byte[] array = null;
		try
		{
			array = File.ReadAllBytes(bundlePath);
		}
		catch (OutOfMemoryException)
		{
			yield break;
		}
		AssetBundleCreateRequest resultAssetBundle = AssetBundle.LoadFromMemoryAsync(array);
		yield return new WaitWhile(() => !resultAssetBundle.isDone);
		referenceList.Add(resultAssetBundle.assetBundle);
		yield return null;
		yield break;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x000086C1 File Offset: 0x000068C1
	private static void BroadcastLoadProgressUpdate(float progress, float from = 0f, float to = 1f)
	{
		progress = Mathf.Lerp(from, to, progress);
		if (AssetBundleControl.OnUpdateLoadProgress != null)
		{
			AssetBundleControl.OnUpdateLoadProgress(progress);
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000086E0 File Offset: 0x000068E0
	private void UnloadTest()
	{
		AssetBundle.UnloadAllAssetBundles(true);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000086F3 File Offset: 0x000068F3
	private void Reload()
	{
		SceneManager.LoadScene("init");
	}

	// Token: 0x0400013B RID: 315
	public static Action<float> OnUpdateLoadProgress;

	// Token: 0x0400013C RID: 316
	public static Action OnScenePreLoad;

	// Token: 0x0400013D RID: 317
	public static Action OnSceneReady;

	// Token: 0x0400013E RID: 318
	private static List<AssetBundle> LoadedDefaultBundles = new List<AssetBundle>();

	// Token: 0x0400013F RID: 319
	private static List<string> allListedAssetBundles = new List<string>();

	// Token: 0x04000140 RID: 320
	private static string currentGameSceneName = "";

	// Token: 0x04000141 RID: 321
	private static string currentLoadingSceneName = "";

	// Token: 0x04000142 RID: 322
	private static SceneAssetSet currentAssetSet = null;

	// Token: 0x04000143 RID: 323
	private static AssetBundle currentSceneBundle;

	// Token: 0x04000144 RID: 324
	private static AssetBundle currentSceneDataBundle;

	// Token: 0x04000145 RID: 325
	private static List<AssetBundle> currentSceneExclusiveAssetBundles = new List<AssetBundle>();

	// Token: 0x04000146 RID: 326
	private static List<AssetBundle> currentSetAssetBundles = new List<AssetBundle>();

	// Token: 0x04000147 RID: 327
	private static AssetBundle loadingSceneAssetBundle;

	// Token: 0x04000148 RID: 328
	private static bool loadIntoMemory = false;

	// Token: 0x04000149 RID: 329
	private static Coroutine changeSceneRoutine;

	// Token: 0x0400014A RID: 330
	[SerializeField]
	private bool debugLoadScene;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	private string debugSceneName;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private ReleaseMap[] releaseMaps;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private PlatformConfigurations platformConfiguration;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	private bool autoStart;

	// Token: 0x0400014F RID: 335
	private Coroutine loadingBundles;

	// Token: 0x04000150 RID: 336
	private List<AssetBundle> activeSceneBundles = new List<AssetBundle>();

	// Token: 0x04000151 RID: 337
	[Space]
	[Header("DEBUG")]
	[SerializeField]
	private bool cancelLoadScene;

	// Token: 0x04000152 RID: 338
	[SerializeField]
	private bool forcePlatform;

	// Token: 0x04000153 RID: 339
	[SerializeField]
	private RuntimePlatform customPlatform;

	// Token: 0x04000154 RID: 340
	private static List<ReleaseMap.ReleaseScene> releaseScenes = new List<ReleaseMap.ReleaseScene>();
}
