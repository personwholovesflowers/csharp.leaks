using System;
using UnityEngine;

// Token: 0x020002D2 RID: 722
public class LightOff : MonoBehaviour
{
	// Token: 0x06000FB4 RID: 4020 RVA: 0x00074FCD File Offset: 0x000731CD
	private void Awake()
	{
		this.aud = base.GetComponentsInChildren<AudioSource>();
		this.light = base.GetComponentInChildren<Light>();
		this.otherLight = this.otherLamp.GetComponent<Light>();
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x00074FF8 File Offset: 0x000731F8
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			AudioSource[] array = this.aud;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Play();
			}
			if (this.light != null)
			{
				this.light.enabled = false;
			}
			if (this.otherLight != null)
			{
				this.otherLight.intensity = this.oLIntensity;
			}
		}
	}

	// Token: 0x04001530 RID: 5424
	private Light light;

	// Token: 0x04001531 RID: 5425
	private AudioSource[] aud;

	// Token: 0x04001532 RID: 5426
	public GameObject otherLamp;

	// Token: 0x04001533 RID: 5427
	private Light otherLight;

	// Token: 0x04001534 RID: 5428
	public float oLIntensity;
}
