using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
[AttributeUsage(AttributeTargets.Field)]
public class DynamicLabelAttribute : PropertyAttribute
{
	// Token: 0x060005C5 RID: 1477 RVA: 0x00020013 File Offset: 0x0001E213
	public DynamicLabelAttribute(string MethodName)
	{
		this.MethodName = MethodName;
	}

	// Token: 0x04000602 RID: 1538
	public readonly string MethodName;
}
