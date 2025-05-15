using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class ColorBlindActivator : MonoBehaviour
{
	// Token: 0x06000410 RID: 1040 RVA: 0x0001C2BC File Offset: 0x0001A4BC
	private void Start()
	{
		if (this.cbss == null || this.cbss.Length == 0)
		{
			this.cbss = base.GetComponentsInChildren<ColorBlindSetter>(true);
		}
		ColorBlindSetter[] array = this.cbss;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Prepare();
		}
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0001C304 File Offset: 0x0001A504
	public void ResetToDefault()
	{
		if (this.cbss == null || this.cbss.Length == 0)
		{
			this.cbss = base.GetComponentsInChildren<ColorBlindSetter>(true);
		}
		ColorBlindSetter[] array = this.cbss;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ResetToDefault();
		}
	}

	// Token: 0x040004FA RID: 1274
	public Transform parentOfSetters;

	// Token: 0x040004FB RID: 1275
	private ColorBlindSetter[] cbss;
}
