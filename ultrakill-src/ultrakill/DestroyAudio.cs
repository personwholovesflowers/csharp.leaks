using System;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class DestroyAudio : MonoBehaviour
{
	// Token: 0x06000501 RID: 1281 RVA: 0x00021FA3 File Offset: 0x000201A3
	private void Start()
	{
		base.Invoke("Delet", this.time);
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00021FB6 File Offset: 0x000201B6
	private void Delet()
	{
		Object.Destroy(base.GetComponent<AudioSource>());
		Object.Destroy(this);
	}

	// Token: 0x040006EF RID: 1775
	public float time;
}
