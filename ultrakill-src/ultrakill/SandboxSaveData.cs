using System;
using UnityEngine;

// Token: 0x020003A7 RID: 935
[Serializable]
public class SandboxSaveData
{
	// Token: 0x04001DB6 RID: 7606
	public string MapName;

	// Token: 0x04001DB7 RID: 7607
	public string MapIdentifier;

	// Token: 0x04001DB8 RID: 7608
	public int SaveVersion = 2;

	// Token: 0x04001DB9 RID: 7609
	public string GameVersion = Application.version;

	// Token: 0x04001DBA RID: 7610
	public SavedBlock[] Blocks;

	// Token: 0x04001DBB RID: 7611
	public SavedProp[] Props;

	// Token: 0x04001DBC RID: 7612
	public SavedEnemy[] Enemies;
}
