using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class Smear : MonoBehaviour
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000057 RID: 87 RVA: 0x00004A1B File Offset: 0x00002C1B
	// (set) Token: 0x06000058 RID: 88 RVA: 0x00004A23 File Offset: 0x00002C23
	private Material InstancedMaterial
	{
		get
		{
			return this.m_instancedMaterial;
		}
		set
		{
			this.m_instancedMaterial = value;
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00004A2C File Offset: 0x00002C2C
	private void Start()
	{
		this.InstancedMaterial = this.Renderer.material;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00004A40 File Offset: 0x00002C40
	private void LateUpdate()
	{
		if (this.m_recentPositions.Count > this.FramesBufferSize)
		{
			this.InstancedMaterial.SetVector("_PrevPosition", this.m_recentPositions.Dequeue());
		}
		this.InstancedMaterial.SetVector("_Position", base.transform.position);
		this.m_recentPositions.Enqueue(base.transform.position);
	}

	// Token: 0x04000079 RID: 121
	private Queue<Vector3> m_recentPositions = new Queue<Vector3>();

	// Token: 0x0400007A RID: 122
	public int FramesBufferSize;

	// Token: 0x0400007B RID: 123
	public Renderer Renderer;

	// Token: 0x0400007C RID: 124
	private Material m_instancedMaterial;
}
