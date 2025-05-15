using System;
using UnityEngine;

// Token: 0x020003BC RID: 956
public class SandboxGhostEffect : MonoBehaviour
{
	// Token: 0x060015CA RID: 5578 RVA: 0x000B11F6 File Offset: 0x000AF3F6
	private void Awake()
	{
		this.originalScale = base.transform.localScale;
		base.transform.localScale *= 1.3f;
		this.timeSinceStart = 0f;
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x000B1234 File Offset: 0x000AF434
	private void Update()
	{
		base.transform.localScale = Vector3.Lerp(this.originalScale * 1.3f, this.originalScale, this.timeSinceStart / 0.2f);
		if (this.timeSinceStart >= 0.2f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04001E03 RID: 7683
	public Collider targetCollider;

	// Token: 0x04001E04 RID: 7684
	private const float scaleMulti = 1.3f;

	// Token: 0x04001E05 RID: 7685
	private const float duration = 0.2f;

	// Token: 0x04001E06 RID: 7686
	private Vector3 originalScale;

	// Token: 0x04001E07 RID: 7687
	private TimeSince timeSinceStart;
}
