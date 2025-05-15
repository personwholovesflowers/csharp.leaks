using System;
using UnityEngine;

// Token: 0x02000389 RID: 905
public class ResetPosition : MonoBehaviour
{
	// Token: 0x060014CD RID: 5325 RVA: 0x000A7A3A File Offset: 0x000A5C3A
	private void Awake()
	{
		if (!this.valueSet)
		{
			this.valueSet = true;
			this.ChangeOriginalPositionAndRotation();
		}
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x000A7A51 File Offset: 0x000A5C51
	public void Activate()
	{
		base.transform.localPosition = this.originalPosition;
		base.transform.localRotation = this.originalRotation;
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x000A7A75 File Offset: 0x000A5C75
	public void ChangeOriginalPositionAndRotation()
	{
		this.ChangeOriginalPosition(base.transform.localPosition);
		this.ChangeOriginalRotation(base.transform.localRotation);
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x000A7A99 File Offset: 0x000A5C99
	public void ChangeOriginalPosition(Vector3 target)
	{
		this.originalPosition = target;
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x000A7AA2 File Offset: 0x000A5CA2
	public void ChangeOriginalRotation(Quaternion target)
	{
		this.originalRotation = target;
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x000A7AAB File Offset: 0x000A5CAB
	public void ChangeOriginalRotation(Vector3 target)
	{
		this.ChangeOriginalRotation(Quaternion.Euler(target));
	}

	// Token: 0x04001C98 RID: 7320
	[HideInInspector]
	public bool valueSet;

	// Token: 0x04001C99 RID: 7321
	[HideInInspector]
	public Vector3 originalPosition;

	// Token: 0x04001C9A RID: 7322
	[HideInInspector]
	public Quaternion originalRotation;
}
