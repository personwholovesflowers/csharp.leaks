using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200001C RID: 28
public class ActivateOnController : MonoBehaviour
{
	// Token: 0x060000E5 RID: 229 RVA: 0x00005CFC File Offset: 0x00003EFC
	private void Start()
	{
		this.Check();
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00005CFC File Offset: 0x00003EFC
	private void Update()
	{
		this.Check();
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00005D04 File Offset: 0x00003F04
	private void Check()
	{
		if (this.oneTime && this.doneOnce)
		{
			return;
		}
		if ((!this.doneOnce || this.activated) && (Gamepad.current == null || !Gamepad.current.enabled))
		{
			this.activated = false;
			UltrakillEvent ultrakillEvent = this.onController;
			if (ultrakillEvent != null)
			{
				ultrakillEvent.Revert();
			}
			UltrakillEvent ultrakillEvent2 = this.onNoController;
			if (ultrakillEvent2 != null)
			{
				ultrakillEvent2.Invoke("");
			}
		}
		else if (!this.activated && Gamepad.current != null && Gamepad.current.enabled)
		{
			this.activated = true;
			UltrakillEvent ultrakillEvent3 = this.onController;
			if (ultrakillEvent3 != null)
			{
				ultrakillEvent3.Invoke("");
			}
			UltrakillEvent ultrakillEvent4 = this.onNoController;
			if (ultrakillEvent4 != null)
			{
				ultrakillEvent4.Revert();
			}
		}
		this.doneOnce = true;
	}

	// Token: 0x0400008F RID: 143
	public bool oneTime;

	// Token: 0x04000090 RID: 144
	private bool doneOnce;

	// Token: 0x04000091 RID: 145
	public UltrakillEvent onController;

	// Token: 0x04000092 RID: 146
	public UltrakillEvent onNoController;

	// Token: 0x04000093 RID: 147
	private bool activated;
}
