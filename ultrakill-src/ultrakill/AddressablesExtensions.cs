using System;
using System.Collections.Generic;
using plog;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000025 RID: 37
public static class AddressablesExtensions
{
	// Token: 0x060000FA RID: 250 RVA: 0x00006030 File Offset: 0x00004230
	public static GameObject ToAsset(this AssetReference reference)
	{
		return AssetHelper.LoadPrefab(reference);
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00006038 File Offset: 0x00004238
	public static GameObject[] ToAssets(this AssetReference[] references)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < references.Length; i++)
		{
			if (references[i] == null || !references[i].RuntimeKeyIsValid())
			{
				AddressablesExtensions.Log.Warning(string.Format("Invalid asset reference at index {0}.", i), null, null, null);
			}
			else
			{
				GameObject gameObject = references[i].ToAsset();
				if (gameObject == null || gameObject.Equals(null))
				{
					AddressablesExtensions.Log.Warning(string.Format("Failed to load asset at index {0}.\nRuntime key: {1}", i, references[i].RuntimeKey), null, null, null);
				}
				else
				{
					list.Add(gameObject);
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x040000B5 RID: 181
	private static readonly global::plog.Logger Log = new global::plog.Logger("AddressablesExtensions");
}
