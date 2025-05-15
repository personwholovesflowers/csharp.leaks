using System;
using UnityEngine;

// Token: 0x0200044B RID: 1099
public class StaminaFail : MonoBehaviour
{
	// Token: 0x060018E6 RID: 6374 RVA: 0x000CA01C File Offset: 0x000C821C
	private void Start()
	{
		StaminaMeter[] array = Object.FindObjectsOfType<StaminaMeter>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Flash(true);
		}
	}
}
