using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class FakeRoomActivator : MonoBehaviour
{
	// Token: 0x06000919 RID: 2329 RVA: 0x0003C4A3 File Offset: 0x0003A6A3
	private void OnEnable()
	{
		this.fake.SetActive(false);
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x0003C4B1 File Offset: 0x0003A6B1
	private void OnDisable()
	{
		if (this.fake)
		{
			this.fake.SetActive(true);
		}
	}

	// Token: 0x04000B8A RID: 2954
	public GameObject fake;
}
