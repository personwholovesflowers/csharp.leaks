using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000505 RID: 1285
public class Cork : MonoBehaviour
{
	// Token: 0x06001D5E RID: 7518 RVA: 0x000F62F9 File Offset: 0x000F44F9
	private void Start()
	{
		this.basePos = base.transform.position;
		this.rb = base.GetComponent<Rigidbody>();
		this.floater = base.GetComponent<FloatOnWater>();
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x000F6324 File Offset: 0x000F4524
	private void Update()
	{
		if (!this.insideSuckZone)
		{
			this.StopWiggle();
		}
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x000F6334 File Offset: 0x000F4534
	public void StartWiggle()
	{
		if (this.disallowWiggle)
		{
			return;
		}
		if (this.crt == null)
		{
			this.crt = base.StartCoroutine(this.Wiggle());
		}
	}

	// Token: 0x06001D61 RID: 7521 RVA: 0x000F6359 File Offset: 0x000F4559
	public void StopWiggle()
	{
		if (this.disallowWiggle)
		{
			return;
		}
		base.transform.position = this.basePos;
		if (this.crt != null)
		{
			base.StopCoroutine(this.crt);
		}
		this.crt = null;
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x000F6390 File Offset: 0x000F4590
	private IEnumerator Wiggle()
	{
		this.wiggleTimer = 0f;
		while (this.wiggleTimer < this.wiggleTime)
		{
			base.transform.position = this.basePos + Random.onUnitSphere * this.wiggleStrength;
			this.wiggleTimer += Time.deltaTime;
			yield return null;
		}
		this.rb.isKinematic = false;
		this.rb.AddForce(Vector3.up);
		this.floater.enabled = true;
		this.disallowWiggle = true;
		yield return new WaitForSeconds(1f);
		this.vortex.SetActive(true);
		this.tinter.isDraining = true;
		Object.Destroy(this);
		yield break;
	}

	// Token: 0x0400299C RID: 10652
	public float wiggleTime = 2f;

	// Token: 0x0400299D RID: 10653
	public float wiggleStrength = 1f;

	// Token: 0x0400299E RID: 10654
	public GameObject vortex;

	// Token: 0x0400299F RID: 10655
	public Pond tinter;

	// Token: 0x040029A0 RID: 10656
	public bool insideSuckZone;

	// Token: 0x040029A1 RID: 10657
	private Vector3 basePos;

	// Token: 0x040029A2 RID: 10658
	private float wiggleTimer;

	// Token: 0x040029A3 RID: 10659
	private Rigidbody rb;

	// Token: 0x040029A4 RID: 10660
	private Coroutine crt;

	// Token: 0x040029A5 RID: 10661
	private FloatOnWater floater;

	// Token: 0x040029A6 RID: 10662
	private bool disallowWiggle;
}
