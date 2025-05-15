using System;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class SetWidthAndHeight : MonoBehaviour
{
	// Token: 0x06000923 RID: 2339 RVA: 0x0002B35B File Offset: 0x0002955B
	public void SetWidthHeight(float val)
	{
		this.target.sizeDelta = new Vector2(val, val);
	}

	// Token: 0x040008F7 RID: 2295
	[SerializeField]
	private RectTransform target;
}
