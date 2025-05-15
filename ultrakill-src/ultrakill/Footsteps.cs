using System;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class Footsteps : MonoBehaviour
{
	// Token: 0x06000A66 RID: 2662 RVA: 0x0004947A File Offset: 0x0004767A
	private void Awake()
	{
		if (this.dontInstantiate)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00049490 File Offset: 0x00047690
	public void Footstep()
	{
		if (this.dontInstantiate)
		{
			int num = Random.Range(0, this.steps.Length);
			if (this.steps.Length > 1 && num == this.previousStep)
			{
				num = (num + 1) % this.steps.Length;
			}
			this.aud.clip = this.steps[num];
			this.aud.pitch = Random.Range(0.9f, 1.1f);
			this.aud.Play();
			return;
		}
		Object.Instantiate<GameObject>(this.footstep, base.transform.position, base.transform.rotation);
	}

	// Token: 0x04000DD3 RID: 3539
	public bool dontInstantiate;

	// Token: 0x04000DD4 RID: 3540
	public GameObject footstep;

	// Token: 0x04000DD5 RID: 3541
	private AudioSource aud;

	// Token: 0x04000DD6 RID: 3542
	public AudioClip[] steps;

	// Token: 0x04000DD7 RID: 3543
	private int previousStep;
}
