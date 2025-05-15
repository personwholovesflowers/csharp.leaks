using System;
using UnityEngine;

// Token: 0x02000489 RID: 1161
public class TrailRendererAutoScaler : MonoBehaviour
{
	// Token: 0x06001A95 RID: 6805 RVA: 0x000DA956 File Offset: 0x000D8B56
	private void Awake()
	{
		this.tr = base.GetComponent<TrailRenderer>();
		if (this.setDefaultSizeOnAwake)
		{
			this.defaultSize = base.transform.localScale;
		}
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x000DA97D File Offset: 0x000D8B7D
	private void Update()
	{
		this.tr.widthMultiplier = this.SizeAverage(base.transform.localScale) / this.SizeAverage(this.defaultSize);
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x000DA9A8 File Offset: 0x000D8BA8
	private float SizeAverage(Vector3 size)
	{
		return (size.x + size.y + size.z) / 3f;
	}

	// Token: 0x04002545 RID: 9541
	private TrailRenderer tr;

	// Token: 0x04002546 RID: 9542
	[SerializeField]
	private bool setDefaultSizeOnAwake = true;

	// Token: 0x04002547 RID: 9543
	[SerializeField]
	private Vector3 defaultSize = Vector3.one;
}
