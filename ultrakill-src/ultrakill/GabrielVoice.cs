using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000207 RID: 519
public class GabrielVoice : MonoBehaviour
{
	// Token: 0x06000AF8 RID: 2808 RVA: 0x0004F648 File Offset: 0x0004D848
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0004F656 File Offset: 0x0004D856
	private void Update()
	{
		if (this.aud && !this.aud.isPlaying)
		{
			this.priority = 0;
		}
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0004F67C File Offset: 0x0004D87C
	public void Hurt()
	{
		if (this.priority <= 0)
		{
			if (this.hurt.Length > 1)
			{
				this.aud.clip = this.hurt[Random.Range(0, this.hurt.Length)];
			}
			else
			{
				this.aud.clip = this.hurt[0];
			}
			this.aud.volume = 0.75f;
			this.aud.Play();
		}
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0004F6F0 File Offset: 0x0004D8F0
	public void BigHurt()
	{
		if (this.priority <= 2)
		{
			this.priority = 2;
			if (this.bigHurt.Length > 1)
			{
				this.aud.clip = this.bigHurt[Random.Range(0, this.bigHurt.Length)];
			}
			else
			{
				this.aud.clip = this.bigHurt[0];
			}
			this.aud.volume = 1f;
			this.aud.Play();
		}
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0004F768 File Offset: 0x0004D968
	public void PhaseChange()
	{
		if (this.priority <= 3)
		{
			this.priority = 3;
			this.aud.clip = this.phaseChange;
			this.aud.volume = 1f;
			this.aud.Play();
			MonoSingleton<SubtitleController>.Instance.DisplaySubtitle(this.phaseChangeSubtitle, null, false);
		}
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0004F7C4 File Offset: 0x0004D9C4
	public void Taunt()
	{
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (this.secondPhase)
		{
			this.TauntNow(this.tauntSecondPhase, this.tauntsSecondPhase);
			return;
		}
		this.TauntNow(this.taunt, this.taunts);
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0004F818 File Offset: 0x0004DA18
	private void TauntNow(AudioClip[] clips, string[] subs)
	{
		if (this.priority <= 1)
		{
			this.priority = 1;
			if (clips.Length > 1)
			{
				int num = Random.Range(0, clips.Length);
				if (this.usedTaunts.Contains(num))
				{
					for (int i = 0; i < clips.Length; i++)
					{
						if (!this.usedTaunts.Contains(i))
						{
							num = i;
							break;
						}
					}
				}
				this.aud.clip = clips[num];
				if (this.usedTaunts.Count == clips.Length - 1)
				{
					this.usedTaunts.Clear();
				}
				this.usedTaunts.Add(num);
				if (subs[num] != "")
				{
					MonoSingleton<SubtitleController>.Instance.DisplaySubtitle(subs[num], null, false);
				}
			}
			else
			{
				this.aud.clip = clips[0];
			}
			this.aud.volume = 0.85f;
			this.aud.Play();
		}
	}

	// Token: 0x04000EA2 RID: 3746
	private AudioSource aud;

	// Token: 0x04000EA3 RID: 3747
	public AudioClip[] hurt;

	// Token: 0x04000EA4 RID: 3748
	public AudioClip[] bigHurt;

	// Token: 0x04000EA5 RID: 3749
	public AudioClip phaseChange;

	// Token: 0x04000EA6 RID: 3750
	public string phaseChangeSubtitle;

	// Token: 0x04000EA7 RID: 3751
	public AudioClip[] taunt;

	// Token: 0x04000EA8 RID: 3752
	[SerializeField]
	private string[] taunts;

	// Token: 0x04000EA9 RID: 3753
	public bool secondPhase;

	// Token: 0x04000EAA RID: 3754
	public AudioClip[] tauntSecondPhase;

	// Token: 0x04000EAB RID: 3755
	[SerializeField]
	private string[] tauntsSecondPhase;

	// Token: 0x04000EAC RID: 3756
	private List<int> usedTaunts = new List<int>();

	// Token: 0x04000EAD RID: 3757
	private int priority;
}
