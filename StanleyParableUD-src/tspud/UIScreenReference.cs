using System;
using UnityEngine;

// Token: 0x020001E3 RID: 483
[CreateAssetMenu(fileName = "UI Screen Reference", menuName = "")]
public class UIScreenReference : ScriptableObject
{
	// Token: 0x06000B1A RID: 2842 RVA: 0x00033C14 File Offset: 0x00031E14
	public void Call()
	{
		if (this.OnCall != null)
		{
			this.OnCall();
		}
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x00033C29 File Offset: 0x00031E29
	public void CallWithPrevious(UIScreenReference previousScreen)
	{
		this.previous = previousScreen;
		this.previous.Hide();
		if (this.OnCall != null)
		{
			this.OnCall();
		}
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00033C50 File Offset: 0x00031E50
	public void CloseAndCallPrevious()
	{
		if (this.previous != null)
		{
			this.previous.Show();
			this.previous = null;
		}
		if (this.OnClose != null)
		{
			this.OnClose();
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00033C85 File Offset: 0x00031E85
	public void Close()
	{
		if (this.OnClose != null)
		{
			this.OnClose();
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00033C9A File Offset: 0x00031E9A
	public void Hide()
	{
		if (this.OnHide != null)
		{
			this.OnHide();
		}
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x00033CAF File Offset: 0x00031EAF
	public void Show()
	{
		if (this.OnShow != null)
		{
			this.OnShow();
		}
	}

	// Token: 0x04000AF1 RID: 2801
	public Action OnCall;

	// Token: 0x04000AF2 RID: 2802
	public Action OnClose;

	// Token: 0x04000AF3 RID: 2803
	public Action OnHide;

	// Token: 0x04000AF4 RID: 2804
	public Action OnShow;

	// Token: 0x04000AF5 RID: 2805
	[NonSerialized]
	private UIScreenReference previous;
}
