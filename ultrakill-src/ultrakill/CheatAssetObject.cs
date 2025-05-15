using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000AA RID: 170
[CreateAssetMenu(fileName = "Cheat Assets", menuName = "ULTRAKILL/Cheat Asset DB")]
public class CheatAssetObject : ScriptableObject
{
	// Token: 0x04000440 RID: 1088
	public new string name;

	// Token: 0x04000441 RID: 1089
	[Header("Cheats")]
	[FormerlySerializedAs("GenericIcon")]
	public Sprite genericCheatIcon;

	// Token: 0x04000442 RID: 1090
	[FormerlySerializedAs("Icons")]
	public CheatAssetObject.KeyIcon[] cheatIcons;

	// Token: 0x04000443 RID: 1091
	[Header("Sandbox")]
	public Sprite genericSandboxToolIcon;

	// Token: 0x04000444 RID: 1092
	[FormerlySerializedAs("sandboxToolIcons")]
	public CheatAssetObject.KeyIcon[] sandboxMenuIcons;

	// Token: 0x04000445 RID: 1093
	public CheatAssetObject.KeyIcon[] sandboxArmHoloIcons;

	// Token: 0x020000AB RID: 171
	[Serializable]
	public struct KeyIcon
	{
		// Token: 0x04000446 RID: 1094
		public string key;

		// Token: 0x04000447 RID: 1095
		public Sprite sprite;
	}
}
