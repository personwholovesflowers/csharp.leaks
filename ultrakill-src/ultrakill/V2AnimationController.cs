using System;
using UnityEngine;

// Token: 0x02000497 RID: 1175
public class V2AnimationController : MonoBehaviour
{
	// Token: 0x06001B13 RID: 6931 RVA: 0x000E1602 File Offset: 0x000DF802
	private void Start()
	{
		this.v2 = base.GetComponentInParent<V2>();
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x000E1610 File Offset: 0x000DF810
	public void IntroEnd()
	{
		this.v2.IntroEnd();
	}

	// Token: 0x06001B15 RID: 6933 RVA: 0x000E161D File Offset: 0x000DF81D
	public void StareAtPlayer()
	{
		this.v2.StareAtPlayer();
	}

	// Token: 0x06001B16 RID: 6934 RVA: 0x000E162A File Offset: 0x000DF82A
	public void BeginEscape()
	{
		this.v2.BeginEscape();
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x000E1637 File Offset: 0x000DF837
	public void WingsOpen()
	{
		this.v2.SwitchPattern(0);
	}

	// Token: 0x0400263B RID: 9787
	private V2 v2;
}
