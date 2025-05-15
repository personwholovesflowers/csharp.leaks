using System;
using UnityEngine;

// Token: 0x02000244 RID: 580
public class HardDamageForcer : MonoBehaviour
{
	// Token: 0x06000CB7 RID: 3255 RVA: 0x0005E337 File Offset: 0x0005C537
	private void Start()
	{
		if (!this.activated && this.activateOnEnable)
		{
			this.On();
		}
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x0005E337 File Offset: 0x0005C537
	private void OnEnable()
	{
		if (!this.activated && this.activateOnEnable)
		{
			this.On();
		}
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x0005E34F File Offset: 0x0005C54F
	private void Update()
	{
		if (!this.activated)
		{
			return;
		}
		MonoSingleton<NewMovement>.Instance.ForceAntiHP(99f, true, true, false, true);
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x0005E36D File Offset: 0x0005C56D
	public void On()
	{
		this.activated = true;
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x0005E376 File Offset: 0x0005C576
	public void Off()
	{
		this.activated = false;
	}

	// Token: 0x040010E0 RID: 4320
	[HideInInspector]
	private bool activated;

	// Token: 0x040010E1 RID: 4321
	public bool activateOnEnable = true;
}
