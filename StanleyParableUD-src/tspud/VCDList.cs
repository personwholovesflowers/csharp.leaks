using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C9 RID: 457
[CreateAssetMenu(fileName = "Data", menuName = "VCD List")]
public class VCDList : ScriptableObject
{
	// Token: 0x06000A74 RID: 2676 RVA: 0x00031250 File Offset: 0x0002F450
	public VCDList()
	{
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0003126E File Offset: 0x0002F46E
	public VCDList(VCDList original)
	{
		this.scripts = original.scripts;
		this.text = original.text;
	}

	// Token: 0x04000A65 RID: 2661
	public List<string> scripts = new List<string>();

	// Token: 0x04000A66 RID: 2662
	public List<string> text = new List<string>();
}
