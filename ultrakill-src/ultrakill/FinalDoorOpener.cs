using System;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class FinalDoorOpener : MonoBehaviour
{
	// Token: 0x0600096C RID: 2412 RVA: 0x000409F0 File Offset: 0x0003EBF0
	private void Awake()
	{
		this.fd = base.GetComponentInParent<FinalDoor>();
		if (this.fd != null)
		{
			this.fd.Open();
		}
		if (this.fd != null)
		{
			this.opening = true;
			base.Invoke("GoTime", 1f);
			return;
		}
		this.GoTime();
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00040A50 File Offset: 0x0003EC50
	private void OnEnable()
	{
		if (!this.closed)
		{
			return;
		}
		if (this.fd != null)
		{
			this.fd.Open();
		}
		if (this.fd != null)
		{
			base.Invoke("GoTime", 1f);
			return;
		}
		this.GoTime();
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00040AA4 File Offset: 0x0003ECA4
	public void GoTime()
	{
		base.CancelInvoke("GoTime");
		if (this.opened)
		{
			return;
		}
		this.opening = false;
		this.opened = true;
		MonoSingleton<OnLevelStart>.Instance.StartLevel(this.startTimer, this.startMusic);
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00040AE0 File Offset: 0x0003ECE0
	public void Close()
	{
		if (!this.opened && !this.opening)
		{
			return;
		}
		this.closed = true;
		this.opened = false;
		this.opening = false;
		base.CancelInvoke("GoTime");
		if (this.fd)
		{
			this.fd.Close();
		}
	}

	// Token: 0x04000C18 RID: 3096
	public bool startTimer;

	// Token: 0x04000C19 RID: 3097
	public bool startMusic;

	// Token: 0x04000C1A RID: 3098
	private bool opened;

	// Token: 0x04000C1B RID: 3099
	private bool opening;

	// Token: 0x04000C1C RID: 3100
	private bool closed;

	// Token: 0x04000C1D RID: 3101
	private FinalDoor fd;
}
