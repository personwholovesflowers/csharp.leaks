using System;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class StreetcleanerAnimations : MonoBehaviour
{
	// Token: 0x06001956 RID: 6486 RVA: 0x000D0211 File Offset: 0x000CE411
	private void Start()
	{
		this.sc = base.GetComponentInParent<Streetcleaner>();
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x000D022B File Offset: 0x000CE42B
	public void SlapOver()
	{
		this.sc.SlapOver();
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x000D0238 File Offset: 0x000CE438
	public void OverrideOver()
	{
		this.sc.OverrideOver();
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000D0245 File Offset: 0x000CE445
	public void DodgeEnd()
	{
		this.sc.DodgeEnd();
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x000D0252 File Offset: 0x000CE452
	public void StopMoving()
	{
		this.sc.StopMoving();
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x000D025F File Offset: 0x000CE45F
	public void Step()
	{
		this.aud.pitch = Random.Range(0.85f, 1.15f);
		this.aud.Play();
	}

	// Token: 0x04002391 RID: 9105
	private Streetcleaner sc;

	// Token: 0x04002392 RID: 9106
	private AudioSource aud;
}
