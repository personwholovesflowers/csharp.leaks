using System;
using System.Collections.Generic;
using Malee.List;
using UnityEngine;

// Token: 0x0200002A RID: 42
[CreateAssetMenu(fileName = "New Release Map", menuName = "Release Map")]
public class ReleaseMap : ScriptableObject
{
	// Token: 0x04000162 RID: 354
	[Reorderable]
	public ReleaseMap.ReleaseScenes Scenes;

	// Token: 0x04000163 RID: 355
	public string RootFolder = "Project";

	// Token: 0x04000164 RID: 356
	public List<ReleaseBundle> Bundles = new List<ReleaseBundle>();

	// Token: 0x02000356 RID: 854
	[Serializable]
	public class ReleaseScene
	{
		// Token: 0x0400120C RID: 4620
		public string SceneName;

		// Token: 0x0400120D RID: 4621
		public SceneAssetSet Set;
	}

	// Token: 0x02000357 RID: 855
	[Serializable]
	public class ReleaseScenes : ReorderableArray<ReleaseMap.ReleaseScene>
	{
	}
}
