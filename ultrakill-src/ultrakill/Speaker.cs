using System;
using UnityEngine;

// Token: 0x0200042A RID: 1066
public class Speaker : MonoBehaviour
{
	// Token: 0x06001802 RID: 6146 RVA: 0x000C38C4 File Offset: 0x000C1AC4
	private void Start()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		component.clip = this.sounds[Random.Range(0, this.sounds.Length)];
		component.Play();
		component.time = Random.Range(0f, component.clip.length);
	}

	// Token: 0x040021A7 RID: 8615
	public AudioClip[] sounds;
}
