using System;
using UnityEngine;

// Token: 0x02000196 RID: 406
[CreateAssetMenu(fileName = "New Sound Collection", menuName = "Stanley/Sound Collection")]
public class SoundCollection : ScriptableObject
{
	// Token: 0x0600094C RID: 2380 RVA: 0x0002BA1F File Offset: 0x00029C1F
	public AudioClip GetRandomClip()
	{
		if (this.Sounds.Length == 0)
		{
			return null;
		}
		return this.Sounds[Random.Range(0, this.Sounds.Length)];
	}

	// Token: 0x04000922 RID: 2338
	[SerializeField]
	private AudioClip[] Sounds;
}
