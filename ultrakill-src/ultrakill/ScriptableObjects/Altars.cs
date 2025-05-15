using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ScriptableObjects
{
	// Token: 0x0200054D RID: 1357
	[CreateAssetMenu(fileName = "Altars", menuName = "ULTRAKILL/Altars")]
	public class Altars : ScriptableObject
	{
		// Token: 0x04002B1C RID: 11036
		public AssetReference[] altarPrefabs;
	}
}
