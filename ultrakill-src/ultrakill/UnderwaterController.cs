using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200048E RID: 1166
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class UnderwaterController : MonoSingleton<UnderwaterController>
{
	// Token: 0x06001ACD RID: 6861 RVA: 0x000DCA24 File Offset: 0x000DAC24
	private void OnDisable()
	{
		this.toRemove.Clear();
		foreach (Water water in this.touchingWaters)
		{
			if (water == null || !water.IsCollidingWithWater(this.col))
			{
				this.toRemove.Add(water);
			}
		}
		foreach (Water water2 in this.toRemove)
		{
			this.touchingWaters.Remove(water2);
		}
		Shader.DisableKeyword("UNDERWATER");
		if (this.inWater && this.touchingWaters.Count == 0)
		{
			this.RemoveFromWater();
		}
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x000DCB0C File Offset: 0x000DAD0C
	private void Start()
	{
		this.defaultColor = this.overlay.color;
		this.defaultColor.a = 0.3f;
		this.overlay.enabled = false;
		this.aud = this.overlay.GetComponent<AudioSource>();
		this.col = base.GetComponent<Collider>();
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x000DCB64 File Offset: 0x000DAD64
	public void EnterWater(Water enteredWater)
	{
		Shader.EnableKeyword("UNDERWATER");
		this.aud.clip = this.underWater;
		this.aud.loop = true;
		this.aud.Play();
		this.touchingWaters.Add(enteredWater);
		this.UpdateColor(enteredWater.clr);
		MonoSingleton<AudioMixerController>.Instance.IsInWater(true);
		this.inWater = true;
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x000DCBCE File Offset: 0x000DADCE
	public void OutWater(Water enteredWater)
	{
		this.touchingWaters.Remove(enteredWater);
		if (this.touchingWaters.Count == 0)
		{
			this.RemoveFromWater();
		}
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x000DCBF0 File Offset: 0x000DADF0
	private void RemoveFromWater()
	{
		Shader.DisableKeyword("UNDERWATER");
		if (this.overlay == null)
		{
			return;
		}
		Shader.SetGlobalColor("_UnderwaterOverlay", this.offColor);
		if (base.gameObject.scene.isLoaded && MonoSingleton<AudioMixerController>.Instance)
		{
			MonoSingleton<AudioMixerController>.Instance.IsInWater(false);
		}
		this.aud.clip = this.surfacing;
		this.aud.loop = false;
		this.aud.Play();
		this.inWater = false;
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x000DCC84 File Offset: 0x000DAE84
	public void UpdateColor(Color newColor)
	{
		if (newColor != new Color(0f, 0f, 0f, 0f))
		{
			newColor.a = 0.3f;
			Shader.SetGlobalColor("_UnderwaterOverlay", newColor);
			return;
		}
		Shader.SetGlobalColor("_UnderwaterOverlay", this.defaultColor);
	}

	// Token: 0x040025A5 RID: 9637
	public Image overlay;

	// Token: 0x040025A6 RID: 9638
	private Color defaultColor;

	// Token: 0x040025A7 RID: 9639
	private Color offColor;

	// Token: 0x040025A8 RID: 9640
	private HashSet<Water> touchingWaters = new HashSet<Water>();

	// Token: 0x040025A9 RID: 9641
	private AudioLowPassFilter lowPass;

	// Token: 0x040025AA RID: 9642
	public bool inWater;

	// Token: 0x040025AB RID: 9643
	private AudioSource aud;

	// Token: 0x040025AC RID: 9644
	public AudioClip underWater;

	// Token: 0x040025AD RID: 9645
	public AudioClip surfacing;

	// Token: 0x040025AE RID: 9646
	private Collider col;

	// Token: 0x040025AF RID: 9647
	private List<Water> toRemove = new List<Water>();
}
