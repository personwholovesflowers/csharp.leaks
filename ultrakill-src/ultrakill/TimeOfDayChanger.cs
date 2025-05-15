using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000483 RID: 1155
public class TimeOfDayChanger : MonoBehaviour
{
	// Token: 0x06001A7B RID: 6779 RVA: 0x000D9DE0 File Offset: 0x000D7FE0
	private void OnEnable()
	{
		Collider collider;
		Rigidbody rigidbody;
		if (!base.TryGetComponent<Collider>(out collider) && !base.TryGetComponent<Rigidbody>(out rigidbody))
		{
			this.colliderless = true;
		}
		if (!this.dontActivateOnEnable && this.colliderless)
		{
			this.Activate();
		}
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x000D9E1E File Offset: 0x000D801E
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform == MonoSingleton<NewMovement>.Instance.transform)
		{
			this.Activate();
		}
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x000D9E40 File Offset: 0x000D8040
	public void Activate()
	{
		if (this.oneTime && this.activated)
		{
			return;
		}
		this.activated = true;
		if (this.newLights.Length != 0)
		{
			for (int i = 0; i < this.newLights.Length; i++)
			{
				if (!this.newLights[i])
				{
					this.origIntensities.Add(0f);
				}
				else
				{
					this.origIntensities.Add(this.newLights[i].intensity);
					if (this.oldLights.Length != 0)
					{
						this.newLights[i].intensity = 0f;
					}
					this.newLights[i].enabled = true;
				}
			}
		}
		if (this.oldLights.Length != 0)
		{
			foreach (Light light in this.oldLights)
			{
				if (!light)
				{
					this.orgOldIntensities.Add(0f);
				}
				else
				{
					this.orgOldIntensities.Add(light.intensity);
				}
			}
		}
		if (this.newSkybox && this.newLights.Length != 0)
		{
			this.oldSkyboxTemp = new Material(RenderSettings.skybox);
			RenderSettings.skybox = this.oldSkyboxTemp;
		}
		this.originalFogColor = RenderSettings.fogColor;
		this.originalFogStart = RenderSettings.fogStartDistance;
		this.originalFogEnd = RenderSettings.fogEndDistance;
		this.originalAmbientColor = RenderSettings.ambientLight;
		if (RenderSettings.skybox && this.newLights.Length != 0 && RenderSettings.skybox.HasProperty(TimeOfDayChanger.Tint))
		{
			this.originalSkyboxTint = RenderSettings.skybox.GetColor("_Tint");
		}
		this.transitionState = 0f;
		this.allDone = false;
		this.allOff = false;
		if (!this.musicWaitsUntilChange)
		{
			if (this.toBattleMusic)
			{
				MonoSingleton<MusicManager>.Instance.ArenaMusicStart();
				return;
			}
			if (this.toBossMusic)
			{
				MonoSingleton<MusicManager>.Instance.PlayBossMusic();
			}
		}
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000DA010 File Offset: 0x000D8210
	private void ChangeMaterials()
	{
		foreach (MeshRenderer meshRenderer in Object.FindObjectsOfType<MeshRenderer>())
		{
			if (meshRenderer.sharedMaterial == this.oldWalls)
			{
				meshRenderer.material = this.newWalls;
			}
			else if (meshRenderer.sharedMaterial == this.oldSky)
			{
				meshRenderer.material = this.newSky;
			}
		}
		if (this.musicWaitsUntilChange)
		{
			if (this.toBattleMusic)
			{
				MonoSingleton<MusicManager>.Instance.ArenaMusicStart();
			}
			else if (this.toBossMusic)
			{
				MonoSingleton<MusicManager>.Instance.PlayBossMusic();
			}
		}
		if (this.newSkybox)
		{
			this.newSkyboxTemp = new Material(this.newSkybox);
			RenderSettings.skybox = this.newSkyboxTemp;
			if (this.newLights.Length != 0 && RenderSettings.skybox.HasProperty(TimeOfDayChanger.Tint))
			{
				this.skyboxColor = RenderSettings.skybox.GetColor("_Tint");
				RenderSettings.skybox.SetColor("_Tint", Color.black);
			}
		}
		if (this.revertValuesOnFinish)
		{
			for (int j = 0; j < this.oldLights.Length; j++)
			{
				if (this.oldLights[j])
				{
					this.oldLights[j].transform.parent.gameObject.SetActive(false);
					this.oldLights[j].intensity = this.orgOldIntensities[j];
				}
			}
		}
		UnityEvent unityEvent = this.onMaterialChange;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000DA180 File Offset: 0x000D8380
	private void Update()
	{
		if (!this.allDone)
		{
			this.transitionState += Time.deltaTime * this.speedMultiplier;
			RenderSettings.fogColor = Color.Lerp(this.originalFogColor, this.fogColor, this.transitionState / 2f);
			if (this.overrideFogSettings)
			{
				RenderSettings.fogStartDistance = Mathf.Lerp(this.originalFogStart, this.fogStart, this.transitionState / 2f);
				RenderSettings.fogEndDistance = Mathf.Lerp(this.originalFogEnd, this.fogEnd, this.transitionState / 2f);
			}
			RenderSettings.ambientLight = Color.Lerp(this.originalAmbientColor, this.ambientLightingColor, this.transitionState / 2f);
			if (this.sunSprite)
			{
				this.sunSprite.color = Color.Lerp(this.sunSprite.color, this.sunSpriteColor, this.transitionState / 2f);
			}
			if (!this.allOff)
			{
				bool flag = true;
				for (int i = 0; i < this.oldLights.Length; i++)
				{
					if (this.oldLights[i])
					{
						Light light = this.oldLights[i];
						if (light.intensity != 0f)
						{
							light.intensity = Mathf.MoveTowards(light.intensity, 0f, Time.deltaTime * this.orgOldIntensities[i] * this.speedMultiplier);
							if (light.intensity != 0f)
							{
								flag = false;
							}
						}
					}
				}
				if (this.newSkybox && this.newLights.Length != 0 && RenderSettings.skybox.HasProperty(TimeOfDayChanger.Tint))
				{
					RenderSettings.skybox.SetColor(TimeOfDayChanger.Tint, Color.Lerp(this.originalSkyboxTint, Color.black, this.transitionState));
				}
				if (flag)
				{
					this.allOff = true;
					this.ChangeMaterials();
					return;
				}
			}
			else if (this.newLights.Length != 0)
			{
				bool flag2 = true;
				for (int j = 0; j < this.newLights.Length; j++)
				{
					if (this.newLights[j] && this.newLights[j].intensity != this.origIntensities[j])
					{
						this.newLights[j].intensity = Mathf.MoveTowards(this.newLights[j].intensity, this.origIntensities[j], Time.deltaTime * this.origIntensities[j] * this.speedMultiplier);
						if (this.newLights[j].intensity != this.origIntensities[j])
						{
							flag2 = false;
						}
					}
				}
				if (this.newSkybox && RenderSettings.skybox.HasProperty(TimeOfDayChanger.Tint))
				{
					RenderSettings.skybox.SetColor(TimeOfDayChanger.Tint, Color.Lerp(Color.black, this.skyboxColor, this.transitionState - 1f));
				}
				if (flag2)
				{
					if (this.newSkybox && RenderSettings.skybox.HasProperty(TimeOfDayChanger.Tint))
					{
						RenderSettings.skybox.SetColor(TimeOfDayChanger.Tint, this.skyboxColor);
					}
					RenderSettings.fogColor = this.fogColor;
					if (this.overrideFogSettings)
					{
						RenderSettings.fogStartDistance = this.fogStart;
						RenderSettings.fogEndDistance = this.fogEnd;
					}
					RenderSettings.ambientLight = this.ambientLightingColor;
					this.allDone = true;
					return;
				}
				RenderSettings.fogColor = Color.Lerp(this.originalFogColor, this.fogColor, this.transitionState / 2f);
				if (this.overrideFogSettings)
				{
					RenderSettings.fogStartDistance = Mathf.Lerp(this.originalFogStart, this.fogStart, this.transitionState / 2f);
					RenderSettings.fogEndDistance = Mathf.Lerp(this.originalFogEnd, this.fogEnd, this.transitionState / 2f);
				}
				RenderSettings.ambientLight = Color.Lerp(this.originalAmbientColor, this.ambientLightingColor, this.transitionState / 2f);
				return;
			}
			else
			{
				RenderSettings.fogColor = this.fogColor;
				if (this.overrideFogSettings)
				{
					RenderSettings.fogStartDistance = this.fogStart;
					RenderSettings.fogEndDistance = this.fogEnd;
				}
				RenderSettings.ambientLight = this.ambientLightingColor;
				this.allDone = true;
			}
		}
	}

	// Token: 0x0400250C RID: 9484
	public float speedMultiplier = 1f;

	// Token: 0x0400250D RID: 9485
	private bool allOff;

	// Token: 0x0400250E RID: 9486
	private bool allDone = true;

	// Token: 0x0400250F RID: 9487
	public bool oneTime;

	// Token: 0x04002510 RID: 9488
	public bool dontActivateOnEnable;

	// Token: 0x04002511 RID: 9489
	private bool activated;

	// Token: 0x04002512 RID: 9490
	private bool colliderless;

	// Token: 0x04002513 RID: 9491
	public Light[] oldLights;

	// Token: 0x04002514 RID: 9492
	public Light[] newLights;

	// Token: 0x04002515 RID: 9493
	private List<float> orgOldIntensities = new List<float>();

	// Token: 0x04002516 RID: 9494
	private List<float> origIntensities = new List<float>();

	// Token: 0x04002517 RID: 9495
	public Material oldWalls;

	// Token: 0x04002518 RID: 9496
	public Material oldSky;

	// Token: 0x04002519 RID: 9497
	public Material newWalls;

	// Token: 0x0400251A RID: 9498
	public Material newSky;

	// Token: 0x0400251B RID: 9499
	public bool toBattleMusic;

	// Token: 0x0400251C RID: 9500
	public bool toBossMusic;

	// Token: 0x0400251D RID: 9501
	public bool musicWaitsUntilChange;

	// Token: 0x0400251E RID: 9502
	public bool revertValuesOnFinish;

	// Token: 0x0400251F RID: 9503
	public Material newSkybox;

	// Token: 0x04002520 RID: 9504
	private Color skyboxColor;

	// Token: 0x04002521 RID: 9505
	private Material oldSkyboxTemp;

	// Token: 0x04002522 RID: 9506
	private Material newSkyboxTemp;

	// Token: 0x04002523 RID: 9507
	public SpriteRenderer sunSprite;

	// Token: 0x04002524 RID: 9508
	public Color sunSpriteColor;

	// Token: 0x04002525 RID: 9509
	[Header("Fog")]
	public Color fogColor;

	// Token: 0x04002526 RID: 9510
	public bool overrideFogSettings;

	// Token: 0x04002527 RID: 9511
	public float fogStart = 450f;

	// Token: 0x04002528 RID: 9512
	public float fogEnd = 600f;

	// Token: 0x04002529 RID: 9513
	[Header("Lighting")]
	public Color ambientLightingColor;

	// Token: 0x0400252A RID: 9514
	[Header("Events")]
	public UnityEvent onMaterialChange;

	// Token: 0x0400252B RID: 9515
	private Color originalFogColor;

	// Token: 0x0400252C RID: 9516
	private float originalFogStart;

	// Token: 0x0400252D RID: 9517
	private float originalFogEnd;

	// Token: 0x0400252E RID: 9518
	private Color originalSkyboxTint;

	// Token: 0x0400252F RID: 9519
	private Color originalAmbientColor;

	// Token: 0x04002530 RID: 9520
	private float transitionState;

	// Token: 0x04002531 RID: 9521
	private static readonly int Tint = Shader.PropertyToID("_Tint");
}
