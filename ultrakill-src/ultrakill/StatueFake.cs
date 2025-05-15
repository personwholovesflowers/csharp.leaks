using System;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class StatueFake : MonoBehaviour
{
	// Token: 0x06001939 RID: 6457 RVA: 0x000CF208 File Offset: 0x000CD408
	private void Start()
	{
		this.anim = base.GetComponentInChildren<Animator>();
		this.aud = base.GetComponentInChildren<AudioSource>();
		this.part = base.GetComponentInChildren<ParticleSystem>();
		StatueIntroChecker instance = MonoSingleton<StatueIntroChecker>.Instance;
		if (instance != null)
		{
			if (instance.beenSeen)
			{
				this.quickSpawn = true;
			}
			else if (!this.quickSpawn)
			{
				instance.beenSeen = true;
			}
		}
		if (this.quickSpawn)
		{
			this.anim.speed = 1.5f;
		}
		if (this.beingActivated)
		{
			this.Activate();
		}
	}

	// Token: 0x0600193A RID: 6458 RVA: 0x000CF290 File Offset: 0x000CD490
	public void Activate()
	{
		this.beingActivated = true;
		if (this.anim == null)
		{
			this.anim = base.GetComponentInChildren<Animator>();
		}
		if (this.quickSpawn)
		{
			this.anim.Play("Awaken", -1, 0.33f);
			return;
		}
		base.Invoke("SlowStart", 3f);
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x000CF2ED File Offset: 0x000CD4ED
	public void Crack()
	{
		if (!this.hasCracked)
		{
			this.hasCracked = true;
			UltrakillEvent ultrakillEvent = this.onFirstCrack;
			if (ultrakillEvent != null)
			{
				ultrakillEvent.Invoke("");
			}
		}
		this.aud.Play();
		this.part.Play();
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x000CF32A File Offset: 0x000CD52A
	public void Step()
	{
		Object.Instantiate<AudioSource>(this.step, base.transform.position, Quaternion.identity);
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x000CF348 File Offset: 0x000CD548
	public void Done()
	{
		UltrakillEvent ultrakillEvent = this.onComplete;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		Object.Destroy(this);
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000CF366 File Offset: 0x000CD566
	private void SlowStart()
	{
		this.anim.Play("Awaken", -1, 0f);
	}

	// Token: 0x04002364 RID: 9060
	private Animator anim;

	// Token: 0x04002365 RID: 9061
	private AudioSource aud;

	// Token: 0x04002366 RID: 9062
	private ParticleSystem part;

	// Token: 0x04002367 RID: 9063
	public AudioSource step;

	// Token: 0x04002368 RID: 9064
	public bool quickSpawn;

	// Token: 0x04002369 RID: 9065
	[HideInInspector]
	public bool beingActivated;

	// Token: 0x0400236A RID: 9066
	public UltrakillEvent onFirstCrack;

	// Token: 0x0400236B RID: 9067
	private bool hasCracked;

	// Token: 0x0400236C RID: 9068
	public UltrakillEvent onComplete;
}
