using System;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class PolymorphicFieldAttribute : PropertyAttribute
{
	// Token: 0x060006B8 RID: 1720 RVA: 0x0002D0F4 File Offset: 0x0002B2F4
	public PolymorphicFieldAttribute(Type baseType)
	{
		this.baseType = baseType;
	}

	// Token: 0x040008D2 RID: 2258
	public Type baseType;
}
