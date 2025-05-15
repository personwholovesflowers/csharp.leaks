using System;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class EnvSpark : HammerEntity
{
	// Token: 0x06000435 RID: 1077 RVA: 0x000197F4 File Offset: 0x000179F4
	private string GuessSparkMagnitude()
	{
		string text;
		if (this.sparkParticle == null)
		{
			text = "PS is null";
		}
		else if (this.sparkParticle.main.startSizeMultiplier == 0.015f)
		{
			text = "small";
		}
		else
		{
			text = "not small";
			if (this.sparkParticle.main.startLifetime.constantMin == 1.5f)
			{
				text = "medium";
			}
			else if (this.sparkParticle.main.startLifetime.constantMin == 2f)
			{
				text = "huge";
			}
		}
		return "From old PS this looks like a: " + text + " spark";
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x000198A7 File Offset: 0x00017AA7
	private void Awake()
	{
		this.sparkInstance = this.InstantiateSparkEffect();
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x000198B8 File Offset: 0x00017AB8
	[ContextMenu("InstantiateSparkEffect")]
	private SparkFX InstantiateSparkEffect()
	{
		SparkEffectData sparkEffectData = Singleton<GameMaster>.Instance.sparkEffectData;
		SparkFX sparkFX;
		switch (this.magnitude)
		{
		case EnvSpark.SparkMagnitude.Small:
			sparkFX = sparkEffectData.smallParticleEffect;
			goto IL_0050;
		case EnvSpark.SparkMagnitude.Large:
			sparkFX = sparkEffectData.largeParticleEffect;
			goto IL_0050;
		case EnvSpark.SparkMagnitude.Huge:
			sparkFX = sparkEffectData.hugeParticleEffect;
			goto IL_0050;
		}
		sparkFX = sparkEffectData.mediumParticleEffect;
		IL_0050:
		GameObject gameObject = Object.Instantiate<GameObject>(sparkFX.gameObject);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject.GetComponent<SparkFX>();
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00019958 File Offset: 0x00017B58
	public void Input_SparkOnce()
	{
		this.sparkInstance.particleSystemFX.Play();
		this.sparkInstance.audioSource.clip = this.sparkInstance.audioClipSet[Random.Range(0, this.sparkInstance.audioClipSet.Length)];
		this.sparkInstance.audioSource.Play();
	}

	// Token: 0x0400042A RID: 1066
	[DynamicLabel("GuessSparkMagnitude")]
	public EnvSpark.SparkMagnitude magnitude;

	// Token: 0x0400042B RID: 1067
	[HideInInspector]
	public ParticleSystem sparkParticle;

	// Token: 0x0400042C RID: 1068
	private SparkFX sparkInstance;

	// Token: 0x0200039C RID: 924
	public enum SparkMagnitude
	{
		// Token: 0x0400133D RID: 4925
		UndefindedDefaultsMedium,
		// Token: 0x0400133E RID: 4926
		Small,
		// Token: 0x0400133F RID: 4927
		Medium,
		// Token: 0x04001340 RID: 4928
		Large,
		// Token: 0x04001341 RID: 4929
		Huge
	}
}
