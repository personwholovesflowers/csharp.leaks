using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class SimpleMoveExample : MonoBehaviour
{
	// Token: 0x06000054 RID: 84 RVA: 0x0000485D File Offset: 0x00002A5D
	private void Start()
	{
		this.m_originalPosition = base.transform.position;
		this.m_previous = base.transform.position;
		this.m_target = base.transform.position;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00004894 File Offset: 0x00002A94
	private void Update()
	{
		base.transform.position = Vector3.Slerp(this.m_previous, this.m_target, Time.deltaTime * this.Speed);
		this.m_previous = base.transform.position;
		if (Vector3.Distance(this.m_target, base.transform.position) < 0.1f)
		{
			this.m_target = base.transform.position + Random.onUnitSphere * Random.Range(0.7f, 4f);
			this.m_target.Set(Mathf.Clamp(this.m_target.x, this.m_originalPosition.x - this.BoundingVolume.x, this.m_originalPosition.x + this.BoundingVolume.x), Mathf.Clamp(this.m_target.y, this.m_originalPosition.y - this.BoundingVolume.y, this.m_originalPosition.y + this.BoundingVolume.y), Mathf.Clamp(this.m_target.z, this.m_originalPosition.z - this.BoundingVolume.z, this.m_originalPosition.z + this.BoundingVolume.z));
		}
	}

	// Token: 0x04000074 RID: 116
	private Vector3 m_previous;

	// Token: 0x04000075 RID: 117
	private Vector3 m_target;

	// Token: 0x04000076 RID: 118
	private Vector3 m_originalPosition;

	// Token: 0x04000077 RID: 119
	public Vector3 BoundingVolume = new Vector3(3f, 1f, 3f);

	// Token: 0x04000078 RID: 120
	public float Speed = 10f;
}
