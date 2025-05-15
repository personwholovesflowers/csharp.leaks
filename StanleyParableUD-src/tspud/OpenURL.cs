using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class OpenURL : MonoBehaviour
{
	// Token: 0x0600078F RID: 1935 RVA: 0x00026961 File Offset: 0x00024B61
	public void Open()
	{
		Application.OpenURL(this.url);
	}

	// Token: 0x040007B3 RID: 1971
	[SerializeField]
	private string url;
}
