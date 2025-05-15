using System;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class Flicker : MonoBehaviour
{
	// Token: 0x06000A46 RID: 2630 RVA: 0x00048728 File Offset: 0x00046928
	private void Start()
	{
		this.light = base.GetComponent<Light>();
		this.aud = base.GetComponent<AudioSource>();
		this.intensity = this.light.intensity;
		this.light.intensity = 0f;
		foreach (GameObject gameObject in this.flickerDisableObjects)
		{
			if (gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x0004879F File Offset: 0x0004699F
	private void OnDisable()
	{
		base.CancelInvoke();
		if (this.forceOnAfterDisable)
		{
			this.On();
		}
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x000487B8 File Offset: 0x000469B8
	private void OnEnable()
	{
		if (this.timeRandomizer != 0f)
		{
			base.Invoke("Flickering", this.delay + Random.Range(-this.timeRandomizer, this.timeRandomizer));
			return;
		}
		base.Invoke("Flickering", this.delay);
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00048808 File Offset: 0x00046A08
	private void Flickering()
	{
		if (this.light.intensity == 0f)
		{
			this.light.intensity = this.intensity + Random.Range(-this.intensityRandomizer, this.intensityRandomizer);
			if (this.aud != null && base.gameObject.activeInHierarchy)
			{
				this.aud.Play();
			}
			if (this.quickFlicker)
			{
				base.Invoke("Off", 0.1f);
			}
		}
		else
		{
			this.light.intensity = 0f;
			if (this.aud != null && this.stopAudio && base.gameObject.activeInHierarchy)
			{
				this.aud.Stop();
			}
		}
		if (!this.onlyOnce)
		{
			if (this.timeRandomizer != 0f)
			{
				base.Invoke("Flickering", this.delay + Random.Range(-this.timeRandomizer, this.timeRandomizer));
			}
			else
			{
				base.Invoke("Flickering", this.delay);
			}
		}
		foreach (GameObject gameObject in this.flickerDisableObjects)
		{
			if (gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00048948 File Offset: 0x00046B48
	private void On()
	{
		this.light.intensity = this.intensity;
		if (this.aud != null && base.gameObject.activeInHierarchy)
		{
			this.aud.Play();
		}
		GameObject[] array = this.flickerDisableObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x000489AC File Offset: 0x00046BAC
	private void Off()
	{
		this.light.intensity = 0f;
		if (this.aud != null && this.stopAudio && base.gameObject.activeInHierarchy)
		{
			this.aud.Stop();
		}
		GameObject[] array = this.flickerDisableObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x04000D96 RID: 3478
	private Light light;

	// Token: 0x04000D97 RID: 3479
	public float delay;

	// Token: 0x04000D98 RID: 3480
	private AudioSource aud;

	// Token: 0x04000D99 RID: 3481
	private float intensity;

	// Token: 0x04000D9A RID: 3482
	public bool onlyOnce;

	// Token: 0x04000D9B RID: 3483
	public bool quickFlicker;

	// Token: 0x04000D9C RID: 3484
	public float intensityRandomizer;

	// Token: 0x04000D9D RID: 3485
	public float timeRandomizer;

	// Token: 0x04000D9E RID: 3486
	public bool stopAudio;

	// Token: 0x04000D9F RID: 3487
	public bool forceOnAfterDisable;

	// Token: 0x04000DA0 RID: 3488
	public GameObject[] flickerDisableObjects;
}
