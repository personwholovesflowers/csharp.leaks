using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200050B RID: 1291
public class GhostMode : MonoBehaviour
{
	// Token: 0x06001D85 RID: 7557 RVA: 0x000F704C File Offset: 0x000F524C
	private void Start()
	{
		this.lightsToDim.AddRange(this.insideLightsGroup.GetComponentsInChildren<Light>());
		this.lightsToDim.AddRange(this.otherLights);
		this.defaultRayTint = this.godRays.sharedMaterial.GetColor("_TintColor");
		this.defaultAmbientColor = RenderSettings.ambientLight;
		foreach (Light light in this.lightsToDim)
		{
			this.defaultIntensities.Add(light.intensity);
		}
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x000F70F8 File Offset: 0x000F52F8
	public void StartGhostMode()
	{
		if (this.crt == null)
		{
			this.crt = base.StartCoroutine(this.RunGhostMode());
		}
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x000F7114 File Offset: 0x000F5314
	private IEnumerator RunGhostMode()
	{
		this.ghostLights.SetActive(true);
		this.duplicateGhosts = Object.Instantiate<GameObject>(this.ghostGroup, this.ghostGroup.transform.position, this.ghostGroup.transform.rotation);
		this.ghostDrones = this.duplicateGhosts.GetComponentsInChildren<GhostDrone>().ToList<GhostDrone>();
		this.duplicateGhosts.SetActive(true);
		this.isInGhostMode = true;
		float time = 0f;
		MaterialPropertyBlock props = new MaterialPropertyBlock();
		while (time < 1f)
		{
			time += Time.deltaTime;
			for (int i = 0; i < this.lightsToDim.Count; i++)
			{
				this.lightsToDim[i].intensity = Mathf.Lerp(this.defaultIntensities[i], 0f, time);
			}
			RenderSettings.ambientLight = Color.Lerp(this.defaultAmbientColor, this.dimmedAmbientColor, time);
			Color color = Color.Lerp(this.defaultRayTint, this.defaultRayTint * 0.25f, time);
			props.SetColor("_TintColor", color);
			this.godRays.SetPropertyBlock(props);
			yield return null;
		}
		while (this.ghostDrones.Count > 0)
		{
			for (int j = this.ghostDrones.Count - 1; j >= 0; j--)
			{
				GhostDrone ghostDrone = this.ghostDrones[j];
				if (ghostDrone == null)
				{
					this.ghostDrones.Remove(ghostDrone);
				}
			}
			yield return null;
		}
		base.StartCoroutine(this.EndGhostMode());
		yield break;
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x000F7123 File Offset: 0x000F5323
	private IEnumerator EndGhostMode()
	{
		float time = 0f;
		MaterialPropertyBlock props = new MaterialPropertyBlock();
		while (time < 1f)
		{
			time += Time.deltaTime;
			for (int i = 0; i < this.lightsToDim.Count; i++)
			{
				this.lightsToDim[i].intensity = Mathf.Lerp(0f, this.defaultIntensities[i], time);
			}
			RenderSettings.ambientLight = Color.Lerp(this.dimmedAmbientColor, this.defaultAmbientColor, time);
			Color color = Color.Lerp(this.defaultRayTint * 0.25f, this.defaultRayTint, time);
			props.SetColor("_TintColor", color);
			this.godRays.SetPropertyBlock(props);
			yield return null;
		}
		GameProgressSaver.SetGhostDroneModeUnlocked(true);
		MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=orange>DRONE HAUNTING</color> CHEAT UNLOCKED", "", "", 0, false, false, true);
		this.onFinish.Invoke("");
		yield break;
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x000F7134 File Offset: 0x000F5334
	public void ResetOnRespawn()
	{
		if (this.isInGhostMode)
		{
			this.ghostLights.SetActive(false);
			if (this.crt != null)
			{
				base.StopCoroutine(this.crt);
			}
			this.crt = null;
			for (int i = 0; i < this.lightsToDim.Count; i++)
			{
				this.lightsToDim[i].intensity = this.defaultIntensities[i];
			}
			RenderSettings.ambientLight = this.defaultAmbientColor;
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			materialPropertyBlock.SetColor("_TintColor", this.defaultRayTint);
			this.godRays.SetPropertyBlock(materialPropertyBlock);
			foreach (GhostDrone ghostDrone in this.ghostDrones)
			{
				if (ghostDrone != null)
				{
					Object.Destroy(ghostDrone.gameObject);
				}
			}
		}
		this.isInGhostMode = false;
	}

	// Token: 0x040029D9 RID: 10713
	public GameObject ghostGroup;

	// Token: 0x040029DA RID: 10714
	public GameObject ghostLights;

	// Token: 0x040029DB RID: 10715
	private GameObject duplicateGhosts;

	// Token: 0x040029DC RID: 10716
	private List<GhostDrone> ghostDrones;

	// Token: 0x040029DD RID: 10717
	private bool isInGhostMode;

	// Token: 0x040029DE RID: 10718
	public GameObject insideLightsGroup;

	// Token: 0x040029DF RID: 10719
	public Light[] otherLights;

	// Token: 0x040029E0 RID: 10720
	private List<Light> lightsToDim = new List<Light>();

	// Token: 0x040029E1 RID: 10721
	private List<float> defaultIntensities = new List<float>();

	// Token: 0x040029E2 RID: 10722
	private Coroutine crt;

	// Token: 0x040029E3 RID: 10723
	private Color defaultAmbientColor;

	// Token: 0x040029E4 RID: 10724
	public Color dimmedAmbientColor;

	// Token: 0x040029E5 RID: 10725
	public Renderer godRays;

	// Token: 0x040029E6 RID: 10726
	private Color defaultRayTint;

	// Token: 0x040029E7 RID: 10727
	public UltrakillEvent onFinish;
}
