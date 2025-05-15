using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class PortalTraveller : MonoBehaviour
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060001CB RID: 459 RVA: 0x0000CBE3 File Offset: 0x0000ADE3
	// (set) Token: 0x060001CC RID: 460 RVA: 0x0000CBEB File Offset: 0x0000ADEB
	public GameObject graphicsClone { get; set; }

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060001CD RID: 461 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
	// (set) Token: 0x060001CE RID: 462 RVA: 0x0000CBFC File Offset: 0x0000ADFC
	public Vector3 previousOffsetFromPortal { get; set; }

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060001CF RID: 463 RVA: 0x0000CC05 File Offset: 0x0000AE05
	// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000CC0D File Offset: 0x0000AE0D
	public Material[] originalMaterials { get; set; }

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000CC16 File Offset: 0x0000AE16
	// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000CC1E File Offset: 0x0000AE1E
	public Material[] cloneMaterials { get; set; }

	// Token: 0x060001D3 RID: 467 RVA: 0x0000CC27 File Offset: 0x0000AE27
	public virtual void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
	{
		base.transform.position = pos;
		base.transform.rotation = rot;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000CC44 File Offset: 0x0000AE44
	public virtual void EnterPortalThreshold()
	{
		if (!this.InstantiateClone)
		{
			return;
		}
		if (this.graphicsClone == null)
		{
			this.graphicsClone = Object.Instantiate<GameObject>(this.graphicsObject);
			this.graphicsClone.transform.parent = this.graphicsObject.transform.parent;
			this.graphicsClone.transform.localScale = this.graphicsObject.transform.localScale;
			this.originalMaterials = this.GetMaterials(this.graphicsObject);
			this.cloneMaterials = this.GetMaterials(this.graphicsClone);
			return;
		}
		this.graphicsClone.SetActive(true);
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000CCEC File Offset: 0x0000AEEC
	public virtual void ExitPortalThreshold()
	{
		if (!this.InstantiateClone)
		{
			return;
		}
		this.graphicsClone.SetActive(false);
		for (int i = 0; i < this.originalMaterials.Length; i++)
		{
			this.originalMaterials[i].SetVector("sliceNormal", Vector3.zero);
		}
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000CD40 File Offset: 0x0000AF40
	public void SetSliceOffsetDst(float dst, bool clone)
	{
		if (!this.InstantiateClone)
		{
			return;
		}
		for (int i = 0; i < this.originalMaterials.Length; i++)
		{
			if (clone)
			{
				this.cloneMaterials[i].SetFloat("sliceOffsetDst", dst);
			}
			else
			{
				this.originalMaterials[i].SetFloat("sliceOffsetDst", dst);
			}
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000CD94 File Offset: 0x0000AF94
	private Material[] GetMaterials(GameObject g)
	{
		MeshRenderer[] componentsInChildren = g.GetComponentsInChildren<MeshRenderer>();
		List<Material> list = new List<Material>();
		MeshRenderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (Material material in array[i].materials)
			{
				list.Add(material);
			}
		}
		return list.ToArray();
	}

	// Token: 0x04000212 RID: 530
	public GameObject graphicsObject;

	// Token: 0x04000217 RID: 535
	public bool InstantiateClone;
}
