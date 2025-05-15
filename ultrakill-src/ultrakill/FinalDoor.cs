using System;
using UnityEngine;

// Token: 0x020001CC RID: 460
public class FinalDoor : MonoBehaviour
{
	// Token: 0x06000965 RID: 2405 RVA: 0x000406E0 File Offset: 0x0003E8E0
	private void Start()
	{
		if (!this.aboutToOpen && this.doorLight)
		{
			this.doorLight.SetActive(false);
		}
		if (this.startOpen || (this.aboutToOpen && !this.opened))
		{
			this.Open();
		}
		this.allRenderers = base.GetComponentsInChildren<MeshRenderer>();
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00040738 File Offset: 0x0003E938
	public void Open()
	{
		this.aboutToOpen = true;
		MonoSingleton<MusicManager>.Instance.ArenaMusicEnd();
		base.Invoke("OpenDoors", 1f);
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		AudioSource audioSource = this.aud;
		if (audioSource != null)
		{
			audioSource.Play();
		}
		if (this.doorLight)
		{
			this.doorLight.SetActive(true);
		}
		if (this.onMaterials.Length != 0)
		{
			if (this.allRenderers == null || this.allRenderers.Length == 0)
			{
				this.allRenderers = base.GetComponentsInChildren<MeshRenderer>();
			}
			foreach (MeshRenderer meshRenderer in this.allRenderers)
			{
				int onMaterial = this.GetOnMaterial(meshRenderer);
				if (onMaterial >= 0)
				{
					meshRenderer.sharedMaterial = this.onMaterials[onMaterial];
				}
			}
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00040804 File Offset: 0x0003EA04
	public void Close()
	{
		if (!this.opened && !this.aboutToOpen)
		{
			return;
		}
		base.CancelInvoke("OpenDoors");
		AudioSource audioSource = this.aud;
		if (audioSource != null)
		{
			audioSource.Stop();
		}
		if (this.doorLight)
		{
			this.doorLight.SetActive(false);
		}
		if (this.offMaterials.Length != 0)
		{
			if (this.allRenderers == null || this.allRenderers.Length == 0)
			{
				this.allRenderers = base.GetComponentsInChildren<MeshRenderer>();
			}
			foreach (MeshRenderer meshRenderer in this.allRenderers)
			{
				int offMaterial = this.GetOffMaterial(meshRenderer);
				if (offMaterial >= 0)
				{
					meshRenderer.sharedMaterial = this.offMaterials[offMaterial];
				}
			}
		}
		if (this.opened)
		{
			Door[] array2 = this.doors;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Close(true);
			}
		}
		if (this.closingBlocker)
		{
			this.closingBlocker.SetActive(true);
		}
		this.opened = false;
		this.aboutToOpen = false;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00040900 File Offset: 0x0003EB00
	private int GetOnMaterial(MeshRenderer mr)
	{
		for (int i = 0; i < this.offMaterials.Length; i++)
		{
			if (mr.sharedMaterial.mainTexture == this.offMaterials[i].mainTexture)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00040944 File Offset: 0x0003EB44
	private int GetOffMaterial(MeshRenderer mr)
	{
		for (int i = 0; i < this.onMaterials.Length; i++)
		{
			if (mr.sharedMaterial.mainTexture == this.onMaterials[i].mainTexture)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00040988 File Offset: 0x0003EB88
	public void OpenDoors()
	{
		if (this.opened)
		{
			return;
		}
		this.opened = true;
		this.aboutToOpen = false;
		Door[] array = this.doors;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Open(false, true);
		}
		if (this.closingBlocker)
		{
			this.closingBlocker.SetActive(false);
		}
		MonoSingleton<PlayerTracker>.Instance.CheckPlayerType();
	}

	// Token: 0x04000C0E RID: 3086
	public Door[] doors;

	// Token: 0x04000C0F RID: 3087
	public GameObject doorLight;

	// Token: 0x04000C10 RID: 3088
	public bool startOpen;

	// Token: 0x04000C11 RID: 3089
	public Material[] offMaterials;

	// Token: 0x04000C12 RID: 3090
	public Material[] onMaterials;

	// Token: 0x04000C13 RID: 3091
	private MeshRenderer[] allRenderers;

	// Token: 0x04000C14 RID: 3092
	private bool opened;

	// Token: 0x04000C15 RID: 3093
	[HideInInspector]
	public bool aboutToOpen;

	// Token: 0x04000C16 RID: 3094
	private AudioSource aud;

	// Token: 0x04000C17 RID: 3095
	public GameObject closingBlocker;
}
