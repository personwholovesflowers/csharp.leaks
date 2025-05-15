using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C8 RID: 456
[CreateAssetMenu(fileName = "Data", menuName = "Sound List")]
public class SoundList : ScriptableObject
{
	// Token: 0x04000A64 RID: 2660
	public List<AudioClip> sounds = new List<AudioClip>();
}
