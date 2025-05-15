using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000328 RID: 808
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class OutdoorLightMaster : MonoSingleton<OutdoorLightMaster>
{
	// Token: 0x060012B9 RID: 4793 RVA: 0x00095110 File Offset: 0x00093310
	private void Start()
	{
		Light[] componentsInChildren = base.GetComponentsInChildren<Light>(true);
		this.outdoorLights.AddRange(componentsInChildren);
		if (this.extraLights != null)
		{
			this.outdoorLights.AddRange(this.extraLights);
		}
		if (this.outdoorLights.Count != 0)
		{
			this.normalMask = 16777216;
			this.normalMask |= 33554432;
			LayerMask layerMask = 8192;
			this.playerMask = this.normalMask | layerMask;
		}
		foreach (Light light in this.outdoorLights)
		{
			if (this.inverse && (!this.waitForFirstDoorOpen || this.firstDoorOpened))
			{
				light.cullingMask = this.playerMask;
			}
			else
			{
				light.cullingMask = this.normalMask;
			}
			if (this.waitForFirstDoorOpen)
			{
				light.enabled = false;
			}
		}
		if (this.skyboxAnimation == SkyboxAnimation.Wobble)
		{
			this.skyboxDefaultRotation = RenderSettings.skybox.GetFloat("_Rotation");
			this.skyboxRotation = this.skyboxDefaultRotation;
		}
		if (this.activateWhenOutside != null)
		{
			GameObject[] array = this.activateWhenOutside;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(this.inverse && (!this.waitForFirstDoorOpen || this.firstDoorOpened));
			}
		}
		for (int j = 0; j < this.muffleWhenIndoors.Count; j++)
		{
			this.muffleGoals.Add(this.muffleWhenIndoors[j].cutoffFrequency);
		}
		this.muffleSounds = this.inverse && this.waitForFirstDoorOpen && !this.firstDoorOpened;
		this.currentMuffle = (float)(this.muffleSounds ? 1 : 0);
		this.UpdateMuffle();
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x0009531C File Offset: 0x0009351C
	private void Update()
	{
		if (this.skyboxAnimation != SkyboxAnimation.None && RenderSettings.skybox)
		{
			if (!this.tempSkybox)
			{
				this.UpdateSkyboxMaterial();
			}
			else
			{
				if (this.skyboxAnimation == SkyboxAnimation.Rotate)
				{
					this.skyboxRotation += Time.deltaTime;
					if (this.skyboxRotation >= 360f)
					{
						this.skyboxRotation -= 360f;
					}
				}
				else if (this.skyboxAnimation == SkyboxAnimation.Wobble)
				{
					if (this.skyboxRotation > this.skyboxDefaultRotation + 2.5f && this.skyboxWobbleSpeed > -1f)
					{
						this.skyboxWobbleSpeed = Mathf.MoveTowards(this.skyboxWobbleSpeed, -1f, Time.deltaTime / 2f);
					}
					else if (this.skyboxRotation < this.skyboxDefaultRotation - 2.5f && this.skyboxWobbleSpeed < 1f)
					{
						this.skyboxWobbleSpeed = Mathf.MoveTowards(this.skyboxWobbleSpeed, 1f, Time.deltaTime / 2f);
					}
					this.skyboxRotation += Time.deltaTime * 0.5f * this.skyboxWobbleSpeed;
				}
				RenderSettings.skybox.SetFloat("_Rotation", this.skyboxRotation);
			}
		}
		if ((this.muffleSounds && this.currentMuffle != 1f) || (!this.muffleSounds && this.currentMuffle != 0f))
		{
			this.currentMuffle = Mathf.MoveTowards(this.currentMuffle, (float)(this.muffleSounds ? 1 : 0), Time.deltaTime * 3f);
			this.UpdateMuffle();
		}
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x000954BC File Offset: 0x000936BC
	public void AddRequest()
	{
		this.requests++;
		if (this.requests == 1)
		{
			foreach (Light light in this.outdoorLights)
			{
				light.cullingMask = (this.inverse ? this.normalMask : this.playerMask);
			}
			GameObject[] array = this.activateWhenOutside;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(!this.inverse);
			}
			this.muffleSounds = (this.muffleWhenOutdoors ? (!this.inverse) : this.inverse);
		}
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x00095588 File Offset: 0x00093788
	public void RemoveRequest()
	{
		this.requests--;
		if (this.requests == 0)
		{
			foreach (Light light in this.outdoorLights)
			{
				light.cullingMask = (this.inverse ? this.playerMask : this.normalMask);
			}
			GameObject[] array = this.activateWhenOutside;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(this.inverse);
			}
			this.muffleSounds = (this.muffleWhenOutdoors ? this.inverse : (!this.inverse));
		}
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x00095650 File Offset: 0x00093850
	public void FirstDoorOpen()
	{
		if (!this.firstDoorOpened)
		{
			this.firstDoorOpened = true;
			if (this.waitForFirstDoorOpen)
			{
				foreach (Light light in this.outdoorLights)
				{
					if (this.inverse && this.requests <= 0)
					{
						light.cullingMask = this.playerMask;
					}
					light.enabled = true;
				}
			}
			if (this.inverse && this.waitForFirstDoorOpen && this.requests <= 0)
			{
				if (this.activateWhenOutside != null)
				{
					GameObject[] array = this.activateWhenOutside;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].SetActive(true);
					}
				}
				this.muffleSounds = (this.muffleWhenOutdoors ? this.inverse : (!this.inverse));
			}
		}
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x00095740 File Offset: 0x00093940
	public void UpdateSkyboxMaterial()
	{
		if (!this.skyboxMaterial)
		{
			this.skyboxMaterial = RenderSettings.skybox;
		}
		this.tempSkybox = new Material(this.skyboxMaterial);
		RenderSettings.skybox = this.tempSkybox;
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x00095776 File Offset: 0x00093976
	public void ForceMuffle(float target)
	{
		this.currentMuffle = Mathf.Clamp(target, 0f, 1f);
		this.UpdateMuffle();
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x00095794 File Offset: 0x00093994
	private void UpdateMuffle()
	{
		for (int i = 0; i < this.muffleWhenIndoors.Count; i++)
		{
			if (!(this.muffleWhenIndoors[i] == null))
			{
				this.muffleWhenIndoors[i].enabled = this.currentMuffle != 0f;
				this.muffleWhenIndoors[i].cutoffFrequency = Mathf.Lerp(5000f, this.muffleGoals[i], this.currentMuffle);
			}
		}
	}

	// Token: 0x0400199E RID: 6558
	public SkyboxAnimation skyboxAnimation = SkyboxAnimation.Rotate;

	// Token: 0x0400199F RID: 6559
	private float skyboxRotation;

	// Token: 0x040019A0 RID: 6560
	private float skyboxWobbleSpeed = 1f;

	// Token: 0x040019A1 RID: 6561
	private float skyboxDefaultRotation;

	// Token: 0x040019A2 RID: 6562
	public bool inverse;

	// Token: 0x040019A3 RID: 6563
	private List<Light> outdoorLights = new List<Light>();

	// Token: 0x040019A4 RID: 6564
	public Light[] extraLights;

	// Token: 0x040019A5 RID: 6565
	public GameObject[] activateWhenOutside;

	// Token: 0x040019A6 RID: 6566
	[HideInInspector]
	public LayerMask normalMask;

	// Token: 0x040019A7 RID: 6567
	[HideInInspector]
	public LayerMask playerMask;

	// Token: 0x040019A8 RID: 6568
	private int requests;

	// Token: 0x040019A9 RID: 6569
	private bool firstDoorOpened;

	// Token: 0x040019AA RID: 6570
	public bool waitForFirstDoorOpen = true;

	// Token: 0x040019AB RID: 6571
	private Material skyboxMaterial;

	// Token: 0x040019AC RID: 6572
	private Material tempSkybox;

	// Token: 0x040019AD RID: 6573
	public List<AudioLowPassFilter> muffleWhenIndoors = new List<AudioLowPassFilter>();

	// Token: 0x040019AE RID: 6574
	private List<float> muffleGoals = new List<float>();

	// Token: 0x040019AF RID: 6575
	private bool muffleSounds;

	// Token: 0x040019B0 RID: 6576
	private float currentMuffle;

	// Token: 0x040019B1 RID: 6577
	public bool muffleWhenOutdoors;

	// Token: 0x040019B2 RID: 6578
	[HideInInspector]
	public List<Collider> outdoorsZonesCheckerable = new List<Collider>();
}
