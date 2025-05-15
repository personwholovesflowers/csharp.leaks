using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
[CreateAssetMenu(menuName = "ULTRAKILL/Spawnable Object List")]
public class SpawnableObjectsDatabase : ScriptableObject
{
	// Token: 0x0400218F RID: 8591
	public SpawnableObject[] enemies;

	// Token: 0x04002190 RID: 8592
	public SpawnableObject[] objects;

	// Token: 0x04002191 RID: 8593
	public SpawnableObject[] sandboxTools;

	// Token: 0x04002192 RID: 8594
	public SpawnableObject[] sandboxObjects;

	// Token: 0x04002193 RID: 8595
	public SpawnableObject[] specialSandbox;

	// Token: 0x04002194 RID: 8596
	public SpawnableObject[] unlockables;

	// Token: 0x04002195 RID: 8597
	public SpawnableObject[] debug;
}
