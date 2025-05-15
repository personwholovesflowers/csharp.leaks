using System;
using System.Collections.Generic;

// Token: 0x0200016A RID: 362
[Serializable]
public class SoundtrackFolder
{
	// Token: 0x06000712 RID: 1810 RVA: 0x0002E26D File Offset: 0x0002C46D
	public SoundtrackFolder(string name, List<AssetReferenceSoundtrackSong> songs)
	{
		this.name = name;
		this.songs = songs;
	}

	// Token: 0x0400090E RID: 2318
	public string name;

	// Token: 0x0400090F RID: 2319
	public List<AssetReferenceSoundtrackSong> songs;
}
