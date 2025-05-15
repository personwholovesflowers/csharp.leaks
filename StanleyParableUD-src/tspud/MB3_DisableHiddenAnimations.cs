using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class MB3_DisableHiddenAnimations : MonoBehaviour
{
	// Token: 0x06000269 RID: 617 RVA: 0x00010B7D File Offset: 0x0000ED7D
	private void Start()
	{
		if (base.GetComponent<SkinnedMeshRenderer>() == null)
		{
			Debug.LogError("The MB3_CullHiddenAnimations script was placed on and object " + base.name + " which has no SkinnedMeshRenderer attached");
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00010BA8 File Offset: 0x0000EDA8
	private void OnBecameVisible()
	{
		for (int i = 0; i < this.animationsToCull.Count; i++)
		{
			if (this.animationsToCull[i] != null)
			{
				this.animationsToCull[i].enabled = true;
			}
		}
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00010BF4 File Offset: 0x0000EDF4
	private void OnBecameInvisible()
	{
		for (int i = 0; i < this.animationsToCull.Count; i++)
		{
			if (this.animationsToCull[i] != null)
			{
				this.animationsToCull[i].enabled = false;
			}
		}
	}

	// Token: 0x04000285 RID: 645
	public List<Animation> animationsToCull = new List<Animation>();
}
