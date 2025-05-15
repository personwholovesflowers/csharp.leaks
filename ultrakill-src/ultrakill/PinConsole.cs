using System;
using GameConsole;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020C RID: 524
public class PinConsole : MonoBehaviour
{
	// Token: 0x06000B1A RID: 2842 RVA: 0x0004FEB9 File Offset: 0x0004E0B9
	private void Awake()
	{
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x0004FEC7 File Offset: 0x0004E0C7
	public void TogglePin()
	{
		this.pinned = !this.pinned;
		MonoSingleton<Console>.Instance.pinned = this.pinned;
		this.image.color = (this.pinned ? Color.red : Color.white);
	}

	// Token: 0x04000EBF RID: 3775
	private Image image;

	// Token: 0x04000EC0 RID: 3776
	private bool pinned;
}
