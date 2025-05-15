using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class RotateAround : MonoBehaviour
{
	// Token: 0x060004B5 RID: 1205 RVA: 0x0001B70B File Offset: 0x0001990B
	private void Start()
	{
		this.m_TargetPosition = this.Target.position;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0001B71E File Offset: 0x0001991E
	private void Update()
	{
		base.transform.RotateAround(this.m_TargetPosition, Vector3.up, Time.deltaTime * this.Speed);
	}

	// Token: 0x04000485 RID: 1157
	public Transform Target;

	// Token: 0x04000486 RID: 1158
	public float Speed = 1f;

	// Token: 0x04000487 RID: 1159
	protected Vector3 m_TargetPosition;
}
