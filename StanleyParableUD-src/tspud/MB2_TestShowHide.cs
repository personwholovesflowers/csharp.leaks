using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class MB2_TestShowHide : MonoBehaviour
{
	// Token: 0x060002C0 RID: 704 RVA: 0x00011BC4 File Offset: 0x0000FDC4
	private void Update()
	{
		if (Time.frameCount == 100)
		{
			this.mb.ShowHide(null, this.objs);
			this.mb.ApplyShowHide();
			Debug.Log("should have disappeared");
		}
		if (Time.frameCount == 200)
		{
			this.mb.ShowHide(this.objs, null);
			this.mb.ApplyShowHide();
			Debug.Log("should show");
		}
	}

	// Token: 0x040002A8 RID: 680
	public MB3_MeshBaker mb;

	// Token: 0x040002A9 RID: 681
	public GameObject[] objs;
}
