using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class TextureToggle : HammerEntity
{
	// Token: 0x06000A20 RID: 2592 RVA: 0x0002FC64 File Offset: 0x0002DE64
	private void OnValidate()
	{
		InfoOverlay[] array = Object.FindObjectsOfType<InfoOverlay>();
		List<InfoOverlay> list = new List<InfoOverlay>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name == this.target)
			{
				list.Add(array[i]);
			}
		}
		if (list.Count > 0)
		{
			this.targetObjects = list.ToArray();
		}
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0002FCC0 File Offset: 0x0002DEC0
	public void Input_IncrementTextureIndex()
	{
		for (int i = 0; i < this.targetObjects.Length; i++)
		{
			if (this.targetObjects[i] != null)
			{
				this.targetObjects[i].IncrementTextureIndex();
			}
		}
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0002FD00 File Offset: 0x0002DF00
	public void Input_SetTextureIndex(float floatdex)
	{
		int num = Mathf.RoundToInt(floatdex);
		for (int i = 0; i < this.targetObjects.Length; i++)
		{
			if (this.targetObjects[i] != null)
			{
				this.targetObjects[i].SetTextureIndex(num);
			}
		}
	}

	// Token: 0x04000A1B RID: 2587
	public string target = "";

	// Token: 0x04000A1C RID: 2588
	public InfoOverlay[] targetObjects;
}
