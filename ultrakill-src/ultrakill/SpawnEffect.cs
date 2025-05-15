using System;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public class SpawnEffect : MonoBehaviour
{
	// Token: 0x060017F0 RID: 6128 RVA: 0x000C32CC File Offset: 0x000C14CC
	private void Start()
	{
		if (MonoSingleton<PrefsManager>.Instance.GetBoolLocal("simpleSpawns", false))
		{
			this.simple = true;
		}
		this.aud = base.GetComponent<AudioSource>();
		this.aud.pitch = Random.Range(this.pitch - 0.1f, this.pitch + 0.1f);
		this.aud.Play();
		this.bubble = base.transform.GetChild(0);
		if (!this.simple)
		{
			this.light = base.GetComponentInChildren<Light>();
			this.light.enabled = true;
			base.GetComponent<ParticleSystem>().Play();
		}
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x000C3370 File Offset: 0x000C1570
	private void Update()
	{
		if (this.bubble.localScale.x > 0f)
		{
			this.bubble.localScale = this.bubble.localScale - Vector3.one * 2f * Time.deltaTime;
		}
		else
		{
			this.bubble.localScale = Vector3.zero;
		}
		if (!this.simple && this.light != null && this.light.range > 0f)
		{
			this.light.range -= Time.deltaTime * 50f;
		}
	}

	// Token: 0x04002196 RID: 8598
	private AudioSource aud;

	// Token: 0x04002197 RID: 8599
	private Transform bubble;

	// Token: 0x04002198 RID: 8600
	private Light light;

	// Token: 0x04002199 RID: 8601
	public float pitch;

	// Token: 0x0400219A RID: 8602
	private bool simple;
}
