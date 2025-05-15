using System;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class DeathSequence : MonoBehaviour
{
	// Token: 0x060004EC RID: 1260 RVA: 0x000214DC File Offset: 0x0001F6DC
	private void OnEnable()
	{
		if (this.aud == null)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (this.tabl == null)
		{
			this.tabl = base.GetComponentInChildren<TextAppearByLines>();
		}
		if (this.sequenceOver)
		{
			return;
		}
		this.timeSinceDeath = 0f;
		MonoSingleton<TimeController>.Instance.controlPitch = false;
		MonoSingleton<TimeController>.Instance.SetAllPitch(1f);
		MonoSingleton<PostProcessV2_Handler>.Instance.DeathEffect(true);
		this.sequenceOver = false;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00021564 File Offset: 0x0001F764
	private void Update()
	{
		if (this.sequenceOver)
		{
			return;
		}
		if (this.timeSinceDeath < 2f)
		{
			float num = this.timeSinceDeath * 0.5f;
			Shader.SetGlobalFloat("_Sharpness", num);
			Shader.SetGlobalFloat("_Deathness", num);
			MonoSingleton<TimeController>.Instance.SetAllPitch(1f - this.timeSinceDeath / 2f);
			return;
		}
		this.EndSequence();
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x000215DC File Offset: 0x0001F7DC
	public void EndSequence()
	{
		this.sequenceOver = true;
		this.deathScreen.SetActive(true);
		this.aud.Stop();
		this.tabl.Stop();
		MonoSingleton<TimeController>.Instance.controlPitch = false;
		MonoSingleton<TimeController>.Instance.SetAllPitch(0f);
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x0002162C File Offset: 0x0001F82C
	private void OnDisable()
	{
		this.timeSinceDeath = 0f;
		this.sequenceOver = false;
		if (base.gameObject.scene.isLoaded)
		{
			MonoSingleton<TimeController>.Instance.controlPitch = true;
		}
		Shader.SetGlobalFloat("_Sharpness", 0f);
		Shader.SetGlobalFloat("_Deathness", 0f);
		MonoSingleton<PostProcessV2_Handler>.Instance.DeathEffect(false);
		this.deathScreen.SetActive(false);
	}

	// Token: 0x040006B1 RID: 1713
	private UnscaledTimeSince timeSinceDeath;

	// Token: 0x040006B2 RID: 1714
	[SerializeField]
	private GameObject deathScreen;

	// Token: 0x040006B3 RID: 1715
	private bool sequenceOver;

	// Token: 0x040006B4 RID: 1716
	private AudioSource aud;

	// Token: 0x040006B5 RID: 1717
	private TextAppearByLines tabl;
}
