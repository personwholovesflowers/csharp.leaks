using System;
using UnityEngine;

// Token: 0x0200041D RID: 1053
public class SoundStopper : MonoBehaviour
{
	// Token: 0x060017DE RID: 6110 RVA: 0x000C307F File Offset: 0x000C127F
	private void Start()
	{
		this.toStop.Stop();
	}

	// Token: 0x04002151 RID: 8529
	public AudioSource toStop;
}
