using System;
using System.Collections;

// Token: 0x020000DA RID: 218
public class FuncButton : HammerEntity
{
	// Token: 0x060004F9 RID: 1273 RVA: 0x0001CC3A File Offset: 0x0001AE3A
	private void Awake()
	{
		if (this.startLocked)
		{
			this.locked = true;
		}
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0001CC4B File Offset: 0x0001AE4B
	public void Input_Lock()
	{
		this.locked = true;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0001CC54 File Offset: 0x0001AE54
	public void Input_Unlock()
	{
		this.locked = false;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0001CC60 File Offset: 0x0001AE60
	public override void Use()
	{
		if (this.depressed)
		{
			return;
		}
		if (!this.locked)
		{
			base.FireOutput(Outputs.OnPressed);
			if (this.delayBeforeReset != 0f)
			{
				this.depressed = true;
				if (this.delayBeforeReset > 0f)
				{
					base.StartCoroutine(this.Depressed());
				}
			}
		}
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x0001CCB3 File Offset: 0x0001AEB3
	private IEnumerator Depressed()
	{
		yield return new WaitForGameSeconds(this.delayBeforeReset);
		this.depressed = false;
		yield break;
	}

	// Token: 0x040004CD RID: 1229
	public bool startLocked;

	// Token: 0x040004CE RID: 1230
	public bool locked;

	// Token: 0x040004CF RID: 1231
	public float delayBeforeReset = -1f;

	// Token: 0x040004D0 RID: 1232
	private bool depressed;
}
