using System;
using UnityEngine;

// Token: 0x02000134 RID: 308
[CreateAssetMenu]
public class EmbeddedSceneInfo : ScriptableObject
{
	// Token: 0x04000815 RID: 2069
	[Tooltip("Special scenes cannot be normally loaded by the console.")]
	public string[] specialScenes;

	// Token: 0x04000816 RID: 2070
	public string[] ranklessScenes;

	// Token: 0x04000817 RID: 2071
	public IntermissionRelation[] intermissions;
}
