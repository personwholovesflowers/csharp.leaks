using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000129 RID: 297
public class SettingsResetPrompt : MonoBehaviour
{
	// Token: 0x06000703 RID: 1795 RVA: 0x00024EA6 File Offset: 0x000230A6
	public void ShowPrompt(bool visible)
	{
		this.promptShowing = visible;
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00024EAF File Offset: 0x000230AF
	public void CallNo()
	{
		UnityEvent unityEvent = this.noCalled;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00024EC1 File Offset: 0x000230C1
	public void CallYes()
	{
		UnityEvent unityEvent = this.yesCalled;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00024ED3 File Offset: 0x000230D3
	private void Update()
	{
		if (this.promptShowing && Singleton<GameMaster>.Instance.stanleyActions.MenuBack.WasPressed)
		{
			this.CallNo();
		}
	}

	// Token: 0x04000740 RID: 1856
	public GameObject[] toDisableOnPrompt;

	// Token: 0x04000741 RID: 1857
	public GameObject resetAllPrompt;

	// Token: 0x04000742 RID: 1858
	private bool promptShowing;

	// Token: 0x04000743 RID: 1859
	public UnityEvent noCalled;

	// Token: 0x04000744 RID: 1860
	public UnityEvent yesCalled;
}
