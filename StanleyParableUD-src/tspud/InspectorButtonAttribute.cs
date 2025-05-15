using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
[AttributeUsage(AttributeTargets.Field)]
public class InspectorButtonAttribute : PropertyAttribute
{
	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00020031 File Offset: 0x0001E231
	// (set) Token: 0x060005C8 RID: 1480 RVA: 0x00020039 File Offset: 0x0001E239
	public float ButtonWidth
	{
		get
		{
			return this._buttonWidth;
		}
		set
		{
			this._buttonWidth = value;
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x00020042 File Offset: 0x0001E242
	public InspectorButtonAttribute(string MethodName, string DisplayName = null)
	{
		this.MethodName = MethodName;
		this.DisplayName = ((DisplayName == null) ? MethodName : DisplayName);
	}

	// Token: 0x04000604 RID: 1540
	public static float kDefaultButtonWidth;

	// Token: 0x04000605 RID: 1541
	public readonly string MethodName;

	// Token: 0x04000606 RID: 1542
	public readonly string DisplayName;

	// Token: 0x04000607 RID: 1543
	private float _buttonWidth = InspectorButtonAttribute.kDefaultButtonWidth;
}
