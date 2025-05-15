using System;
using UnityEngine;

// Token: 0x020001E1 RID: 481
[RequireComponent(typeof(Animator))]
public class AnimatorUtility : MonoBehaviour
{
	// Token: 0x06000B03 RID: 2819 RVA: 0x00033875 File Offset: 0x00031A75
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x00033883 File Offset: 0x00031A83
	public void SetBool(bool value)
	{
		this.animator.SetBool(this.parameter, value);
	}

	// Token: 0x04000AE2 RID: 2786
	[SerializeField]
	private string parameter;

	// Token: 0x04000AE3 RID: 2787
	private Animator animator;
}
