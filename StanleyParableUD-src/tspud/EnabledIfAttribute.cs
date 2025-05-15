using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
[AttributeUsage(AttributeTargets.Field)]
public class EnabledIfAttribute : PropertyAttribute
{
	// Token: 0x060005C6 RID: 1478 RVA: 0x00020022 File Offset: 0x0001E222
	public EnabledIfAttribute(string MethodName)
	{
		this.MethodName = MethodName;
	}

	// Token: 0x04000603 RID: 1539
	public readonly string MethodName;
}
