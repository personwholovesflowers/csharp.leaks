using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000156 RID: 342
[CreateAssetMenu(menuName = "ULTRAKILL/Soundtrack Song Data")]
public class SoundtrackSong : ScriptableObject
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060006BA RID: 1722 RVA: 0x0002D10C File Offset: 0x0002B30C
	public string extraLevelBit
	{
		get
		{
			if (!(this.levelName == ""))
			{
				return "(" + this.levelName + ")";
			}
			return "";
		}
	}

	// Token: 0x040008D3 RID: 2259
	[Space]
	public Sprite icon;

	// Token: 0x040008D4 RID: 2260
	public string songName;

	// Token: 0x040008D5 RID: 2261
	public string levelName;

	// Token: 0x040008D6 RID: 2262
	[Header("Clips")]
	public AudioClip introClip;

	// Token: 0x040008D7 RID: 2263
	public List<AudioClip> clips;

	// Token: 0x040008D8 RID: 2264
	public int maxClipsIfNotRepeating = -1;

	// Token: 0x040008D9 RID: 2265
	[SerializeReference]
	[PolymorphicField(typeof(UnlockCondition))]
	public List<UnlockCondition> conditions;
}
