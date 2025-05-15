using System;
using UnityEngine;

// Token: 0x020002E4 RID: 740
public class MandaloreVoice : MonoBehaviour
{
	// Token: 0x06001012 RID: 4114 RVA: 0x0007A76B File Offset: 0x0007896B
	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x0007A779 File Offset: 0x00078979
	private void Update()
	{
		if (!this.aud.isPlaying)
		{
			this.talking = false;
		}
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x0007A78F File Offset: 0x0007898F
	public void SecondPhase()
	{
		this.aud.Stop();
		this.aud.PlayOneShot(this.secondPhase);
		this.talking = true;
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0007A7B4 File Offset: 0x000789B4
	public void ThirdPhase()
	{
		this.aud.Stop();
		this.aud.PlayOneShot(this.thirdPhase);
		this.talking = true;
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0007A7D9 File Offset: 0x000789D9
	public void FinalPhase()
	{
		this.aud.Stop();
		this.aud.PlayOneShot(this.finalPhase);
		this.talking = true;
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0007A7FE File Offset: 0x000789FE
	public void Death()
	{
		this.aud.Stop();
		this.aud.PlayOneShot(this.death);
		this.talking = true;
		this.dying = true;
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x0007A82A File Offset: 0x00078A2A
	public void Taunt(int num)
	{
		this.aud.Stop();
		if (this.taunts[num] != null)
		{
			this.aud.PlayOneShot(this.taunts[num]);
		}
		this.talking = true;
	}

	// Token: 0x040015E0 RID: 5600
	private AudioSource aud;

	// Token: 0x040015E1 RID: 5601
	public bool talking;

	// Token: 0x040015E2 RID: 5602
	public bool dying;

	// Token: 0x040015E3 RID: 5603
	public AudioClip secondPhase;

	// Token: 0x040015E4 RID: 5604
	public AudioClip thirdPhase;

	// Token: 0x040015E5 RID: 5605
	public AudioClip finalPhase;

	// Token: 0x040015E6 RID: 5606
	public AudioClip death;

	// Token: 0x040015E7 RID: 5607
	public AudioClip[] taunts;
}
