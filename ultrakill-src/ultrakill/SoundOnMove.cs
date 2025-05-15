using System;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class SoundOnMove : MonoBehaviour
{
	// Token: 0x060017CC RID: 6092 RVA: 0x000C2D87 File Offset: 0x000C0F87
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x000C2DA4 File Offset: 0x000C0FA4
	private void Update()
	{
		if (!this.aud.isPlaying && this.rb.velocity.magnitude > this.minSpeed)
		{
			this.aud.Play();
			return;
		}
		if (this.aud.isPlaying && this.rb.velocity.magnitude <= this.minSpeed)
		{
			this.aud.Stop();
		}
	}

	// Token: 0x04002141 RID: 8513
	private AudioSource aud;

	// Token: 0x04002142 RID: 8514
	private Rigidbody rb;

	// Token: 0x04002143 RID: 8515
	public float minSpeed;
}
