using System;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class MixingTransformTest : MonoBehaviour
{
	// Token: 0x06001168 RID: 4456 RVA: 0x00087FF6 File Offset: 0x000861F6
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x00088004 File Offset: 0x00086204
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F10))
		{
			this.anim.SetLayerWeight(1, 1f);
			this.anim.SetTrigger("Shoot");
		}
	}

	// Token: 0x040017B7 RID: 6071
	private Animator anim;
}
