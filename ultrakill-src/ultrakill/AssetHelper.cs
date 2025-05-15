using System;
using System.Collections;
using System.Collections.Generic;
using plog;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000028 RID: 40
[ConfigureSingleton(SingletonFlags.NoAutoInstance | SingletonFlags.PersistAutoInstance | SingletonFlags.DestroyDuplicates)]
public class AssetHelper : MonoSingleton<AssetHelper>
{
	// Token: 0x060000FE RID: 254 RVA: 0x000060F2 File Offset: 0x000042F2
	protected override void OnEnable()
	{
		base.OnEnable();
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00006108 File Offset: 0x00004308
	public static GameObject LoadPrefab(string address)
	{
		if (MonoSingleton<AssetHelper>.Instance.prefabCache.ContainsKey(address))
		{
			if (!(MonoSingleton<AssetHelper>.Instance.prefabCache[address] == null) && !MonoSingleton<AssetHelper>.Instance.prefabCache[address].Equals(null))
			{
				return MonoSingleton<AssetHelper>.Instance.prefabCache[address];
			}
			MonoSingleton<AssetHelper>.Instance.prefabCache.Remove(address);
		}
		GameObject gameObject = Addressables.LoadAssetAsync<GameObject>(address).WaitForCompletion();
		MonoSingleton<AssetHelper>.Instance.prefabCache.Add(address, gameObject);
		return gameObject;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x0000619C File Offset: 0x0000439C
	public static GameObject LoadPrefab(AssetReference reference)
	{
		if (reference == null || reference.Equals(null) || reference.RuntimeKey == null || !reference.RuntimeKeyIsValid())
		{
			AssetHelper.Log.Warning(string.Format("Missing asset reference.\nRuntime key: {0}", reference.RuntimeKey), null, null, null);
			return null;
		}
		string text = reference.RuntimeKey.ToString();
		if (MonoSingleton<AssetHelper>.Instance.prefabCache.ContainsKey(text))
		{
			if (!(MonoSingleton<AssetHelper>.Instance.prefabCache[text] == null) && !MonoSingleton<AssetHelper>.Instance.prefabCache[text].Equals(null))
			{
				return MonoSingleton<AssetHelper>.Instance.prefabCache[text];
			}
			MonoSingleton<AssetHelper>.Instance.prefabCache.Remove(text);
		}
		GameObject gameObject = reference.LoadAssetAsync<GameObject>().WaitForCompletion();
		MonoSingleton<AssetHelper>.Instance.prefabCache.Add(text, gameObject);
		return gameObject;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00006277 File Offset: 0x00004477
	public static void SpawnPrefabAsync(string prefab, Vector3 position, Quaternion rotation)
	{
		MonoSingleton<AssetHelper>.Instance.StartCoroutine(MonoSingleton<AssetHelper>.Instance.LoadPrefab(prefab, position, rotation));
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00006291 File Offset: 0x00004491
	public IEnumerator LoadPrefab(string prefab, Vector3 position, Quaternion rotation)
	{
		AsyncOperationHandle<GameObject> loadOperation = Addressables.LoadAssetAsync<GameObject>(prefab);
		yield return loadOperation;
		Object.Instantiate<GameObject>(loadOperation.Result, position, rotation);
		yield break;
	}

	// Token: 0x040000BA RID: 186
	private static readonly global::plog.Logger Log = new global::plog.Logger("AssetHelper");

	// Token: 0x040000BB RID: 187
	private Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();
}
