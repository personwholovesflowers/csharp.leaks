using System;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class NonRendererFootstepType : MonoBehaviour
{
	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000963 RID: 2403 RVA: 0x0002C43A File Offset: 0x0002A63A
	public StanleyData.FootstepSounds FootstepType
	{
		get
		{
			return this.footstepType;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06000964 RID: 2404 RVA: 0x0002C442 File Offset: 0x0002A642
	public bool ForceOverrideMaterial
	{
		get
		{
			return this.forceOverrideMaterial;
		}
	}

	// Token: 0x04000945 RID: 2373
	[SerializeField]
	private StanleyData.FootstepSounds footstepType;

	// Token: 0x04000946 RID: 2374
	[SerializeField]
	private bool forceOverrideMaterial;
}
