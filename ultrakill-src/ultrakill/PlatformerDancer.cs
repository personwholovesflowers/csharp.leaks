using System;
using UnityEngine;

// Token: 0x0200033C RID: 828
public class PlatformerDancer : MonoBehaviour
{
	// Token: 0x060012FE RID: 4862 RVA: 0x00097210 File Offset: 0x00095410
	public void Whoosh(float pitch)
	{
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		this.aud.pitch = pitch + Random.Range(-0.05f, 0.05f);
		this.aud.Play();
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x00097260 File Offset: 0x00095460
	public void DanceEnd()
	{
		MonoSingleton<PlatformerMovement>.Instance.transform.position = base.transform.position;
		MonoSingleton<PlatformerMovement>.Instance.gameObject.SetActive(true);
		base.gameObject.SetActive(false);
		MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=orange>CLASH MODE</color> CHEAT UNLOCKED", "", "", 0, false, false, true);
	}

	// Token: 0x04001A1C RID: 6684
	private AudioSource aud;
}
