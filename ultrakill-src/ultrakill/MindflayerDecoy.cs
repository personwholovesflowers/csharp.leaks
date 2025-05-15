using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class MindflayerDecoy : MonoBehaviour
{
	// Token: 0x060010BA RID: 4282 RVA: 0x00080D44 File Offset: 0x0007EF44
	private void Start()
	{
		this.rends = base.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in this.rends)
		{
			if (this.enraged)
			{
				renderer.material = this.enrageMaterial;
			}
			this.mats.AddRange(renderer.materials);
		}
		this.clr = this.mats[0].color;
		if (this.fadeOverride != 0f)
		{
			this.clr.a = this.fadeOverride;
			foreach (Material material in this.mats)
			{
				material.color = this.clr;
			}
		}
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x00080E1C File Offset: 0x0007F01C
	private void Update()
	{
		if (this.clr.a > 0f)
		{
			this.clr.a = Mathf.MoveTowards(this.clr.a, 0f, Time.deltaTime * this.fadeSpeed);
			if (this.clr.a <= 0f)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			foreach (Material material in this.mats)
			{
				material.color = this.clr;
			}
		}
	}

	// Token: 0x040016CA RID: 5834
	private Renderer[] rends;

	// Token: 0x040016CB RID: 5835
	private List<Material> mats = new List<Material>();

	// Token: 0x040016CC RID: 5836
	private Color clr;

	// Token: 0x040016CD RID: 5837
	public bool enraged;

	// Token: 0x040016CE RID: 5838
	public Material enrageMaterial;

	// Token: 0x040016CF RID: 5839
	public float fadeOverride;

	// Token: 0x040016D0 RID: 5840
	public float fadeSpeed = 1f;
}
