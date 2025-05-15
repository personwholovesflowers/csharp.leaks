using System;
using UnityEngine;

// Token: 0x02000415 RID: 1045
public class SoundChanger : MonoBehaviour
{
	// Token: 0x060017C1 RID: 6081 RVA: 0x000C2BE8 File Offset: 0x000C0DE8
	private void Start()
	{
		float num = 0f;
		if (this.keepProgress)
		{
			num = this.target.time;
		}
		this.target.clip = this.newSound;
		this.target.Play();
		if (this.keepProgress)
		{
			this.target.time = num;
		}
	}

	// Token: 0x04002137 RID: 8503
	public AudioSource target;

	// Token: 0x04002138 RID: 8504
	public AudioClip newSound;

	// Token: 0x04002139 RID: 8505
	public bool keepProgress;
}
