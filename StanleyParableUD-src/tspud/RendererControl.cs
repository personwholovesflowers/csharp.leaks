using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000088 RID: 136
public class RendererControl : MonoBehaviour
{
	// Token: 0x06000340 RID: 832 RVA: 0x00016196 File Offset: 0x00014396
	private void Start()
	{
		if (this.disableShadowsOnStart)
		{
			this.DisableShadowCastingInChildren();
			this.DisableShadowReceivingInChildren();
		}
	}

	// Token: 0x06000341 RID: 833 RVA: 0x000161AC File Offset: 0x000143AC
	public void DisableShadowCastingInChildren()
	{
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].shadowCastingMode = ShadowCastingMode.Off;
		}
	}

	// Token: 0x06000342 RID: 834 RVA: 0x000161D8 File Offset: 0x000143D8
	public void DisableShadowReceivingInChildren()
	{
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].receiveShadows = false;
		}
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00016204 File Offset: 0x00014404
	public void SetDefaultLayer(bool status)
	{
		foreach (Transform transform in base.GetComponentsInChildren<Transform>(true))
		{
			if (status)
			{
				transform.gameObject.layer = this.defaultLayer;
			}
			else
			{
				transform.gameObject.layer = this.lowEndLayer;
			}
		}
	}

	// Token: 0x04000339 RID: 825
	[SerializeField]
	private bool disableShadowsOnStart;

	// Token: 0x0400033A RID: 826
	[SerializeField]
	private int defaultLayer;

	// Token: 0x0400033B RID: 827
	[SerializeField]
	private int lowEndLayer;
}
