using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class MB3_BatchPrefabBaker : MonoBehaviour
{
	// Token: 0x0400027E RID: 638
	public MB3_BatchPrefabBaker.MB3_PrefabBakerRow[] prefabRows;

	// Token: 0x0400027F RID: 639
	public string outputPrefabFolder;

	// Token: 0x02000375 RID: 885
	[Serializable]
	public class MB3_PrefabBakerRow
	{
		// Token: 0x04001289 RID: 4745
		public GameObject sourcePrefab;

		// Token: 0x0400128A RID: 4746
		public GameObject resultPrefab;
	}
}
