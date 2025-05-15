using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class BigDoorOpener : MonoBehaviour
{
	// Token: 0x06000237 RID: 567 RVA: 0x0000BE3C File Offset: 0x0000A03C
	private void Start()
	{
		BigDoor[] array = this.bigDoors;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Open();
		}
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000BE68 File Offset: 0x0000A068
	private void OnEnable()
	{
		BigDoor[] array = this.bigDoors;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Open();
		}
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000BE94 File Offset: 0x0000A094
	private void OnDisable()
	{
		if (this.dontCloseOnDisable)
		{
			return;
		}
		BigDoor[] array = this.bigDoors;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Close();
		}
	}

	// Token: 0x0400028B RID: 651
	public BigDoor[] bigDoors;

	// Token: 0x0400028C RID: 652
	public bool dontCloseOnDisable;
}
