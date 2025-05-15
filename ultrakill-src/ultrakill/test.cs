using System;
using UnityEngine;

// Token: 0x02000476 RID: 1142
public class test : MonoBehaviour
{
	// Token: 0x06001A2E RID: 6702 RVA: 0x000D8277 File Offset: 0x000D6477
	private void FixedUpdate()
	{
		this.FixedUpdatesPerFrame += 1f;
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x000D828B File Offset: 0x000D648B
	private void Update()
	{
		Debug.Log("FixedUpdatesPerFrame: " + this.FixedUpdatesPerFrame.ToString());
		this.FixedUpdatesPerFrame = 0f;
	}

	// Token: 0x040024B3 RID: 9395
	private float FixedUpdatesPerFrame;
}
