using System;
using UnityEngine;

namespace TFHC_Shader_Samples
{
	// Token: 0x020002FC RID: 764
	public class highlightAnimated : MonoBehaviour
	{
		// Token: 0x060013DC RID: 5084 RVA: 0x0006923A File Offset: 0x0006743A
		private void Start()
		{
			this.mat = base.GetComponent<Renderer>().material;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0006924D File Offset: 0x0006744D
		private void OnMouseEnter()
		{
			this.switchhighlighted(true);
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00069256 File Offset: 0x00067456
		private void OnMouseExit()
		{
			this.switchhighlighted(false);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0006925F File Offset: 0x0006745F
		private void switchhighlighted(bool highlighted)
		{
			this.mat.SetFloat("_Highlighted", highlighted ? 1f : 0f);
		}

		// Token: 0x04000F8A RID: 3978
		private Material mat;
	}
}
