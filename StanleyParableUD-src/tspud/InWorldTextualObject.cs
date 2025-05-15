using System;
using TMPro;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class InWorldTextualObject : MonoBehaviour
{
	// Token: 0x060005BA RID: 1466 RVA: 0x0001FEC0 File Offset: 0x0001E0C0
	public void OnEnable()
	{
		InWorldLabelManager.Instance.RegisterObject(this);
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0001FECD File Offset: 0x0001E0CD
	public void OnDisable()
	{
		if (InWorldLabelManager.Instance != null)
		{
			InWorldLabelManager.Instance.DeregisterObject(this);
		}
	}

	// Token: 0x040005F7 RID: 1527
	public string localizationTerm;

	// Token: 0x040005F8 RID: 1528
	public Color labelColor = Color.white;

	// Token: 0x040005F9 RID: 1529
	public float activationRadius = 10f;

	// Token: 0x040005FA RID: 1530
	public HorizontalAlignmentOptions horizontalAlignment = HorizontalAlignmentOptions.Center;

	// Token: 0x040005FB RID: 1531
	[Header("Debug, ignore these")]
	public float currentRadius_DEBUG = -1f;

	// Token: 0x040005FC RID: 1532
	public float currentAngleFromCenter_DEBUG = -1f;

	// Token: 0x040005FD RID: 1533
	public Collider obstruction_DEBUG;

	// Token: 0x040005FE RID: 1534
	public Vector2 normalizedPosition_DEBUG;
}
