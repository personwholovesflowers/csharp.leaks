using System;
using UnityEngine;

// Token: 0x0200022B RID: 555
public class GlassBreaker : MonoBehaviour
{
	// Token: 0x06000BE3 RID: 3043 RVA: 0x0005396E File Offset: 0x00051B6E
	private void Start()
	{
		base.GetComponentInParent<Glass>().Shatter();
		Object.Destroy(this);
	}
}
