using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000146 RID: 326
public class PauseEvents : MonoBehaviour
{
	// Token: 0x0600079B RID: 1947 RVA: 0x00026B09 File Offset: 0x00024D09
	private void Awake()
	{
		GameMaster.OnPause += this.OnPause;
		GameMaster.OnResume += this.OnResume;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x00026B2D File Offset: 0x00024D2D
	private void OnPause()
	{
		if (this.active)
		{
			this.OnPauseEvent.Invoke();
		}
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x00026B42 File Offset: 0x00024D42
	private void OnResume()
	{
		if (this.active)
		{
			this.OnResumeEvent.Invoke();
		}
	}

	// Token: 0x040007BE RID: 1982
	[SerializeField]
	private bool active = true;

	// Token: 0x040007BF RID: 1983
	[SerializeField]
	private UnityEvent OnPauseEvent;

	// Token: 0x040007C0 RID: 1984
	[SerializeField]
	private UnityEvent OnResumeEvent;
}
