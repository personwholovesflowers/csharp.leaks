using System;
using UnityEngine;

// Token: 0x0200040B RID: 1035
public class Skull : MonoBehaviour
{
	// Token: 0x060017A2 RID: 6050 RVA: 0x000C2332 File Offset: 0x000C0532
	private void Awake()
	{
		this.lit = base.GetComponent<Light>();
		this.origRange = this.lit.range;
		this.aud = base.GetComponent<AudioSource>();
		this.mod = base.GetComponentInChildren<ModifyMaterial>();
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000C236C File Offset: 0x000C056C
	private void Update()
	{
		if (this.litTime > 0f)
		{
			this.litTime = Mathf.MoveTowards(this.litTime, 0f, Time.deltaTime);
			return;
		}
		if (this.lit.range > this.origRange)
		{
			this.lit.range = Mathf.MoveTowards(this.lit.range, this.origRange, Time.deltaTime * 5f);
		}
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x000C23E4 File Offset: 0x000C05E4
	public void PunchWith()
	{
		if (this.lit.range == this.origRange)
		{
			this.litTime = 1f;
			this.lit.range = this.origRange * 2.5f;
			this.aud.Play();
		}
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x000C2434 File Offset: 0x000C0634
	public void HitWith(GameObject target)
	{
		Flammable component = target.gameObject.GetComponent<Flammable>();
		if (component != null && !component.enemyOnly)
		{
			component.Burn(4f, false);
		}
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x000C246A File Offset: 0x000C066A
	public void HitSurface(RaycastHit hit)
	{
		MonoSingleton<StainVoxelManager>.Instance.TryIgniteAt(hit.point, 3);
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x000C247F File Offset: 0x000C067F
	public void PutDown()
	{
		if (this.lit.enabled)
		{
			this.aud.Stop();
		}
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x000C2499 File Offset: 0x000C0699
	public void OnCorrectUse()
	{
		this.lit.enabled = false;
		this.mod.ChangeEmissionIntensity(1f);
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x000C24B7 File Offset: 0x000C06B7
	public void OffCorrectUse()
	{
		this.lit.enabled = true;
		this.mod.ChangeEmissionIntensity(0f);
	}

	// Token: 0x040020FD RID: 8445
	private Light lit;

	// Token: 0x040020FE RID: 8446
	private float origRange;

	// Token: 0x040020FF RID: 8447
	private float litTime;

	// Token: 0x04002100 RID: 8448
	private AudioSource aud;

	// Token: 0x04002101 RID: 8449
	private ModifyMaterial mod;
}
