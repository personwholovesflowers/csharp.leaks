using System;
using UnityEngine;
using UnityEngine.Sprites;

// Token: 0x02000014 RID: 20
[ExecuteInEditMode]
public class SpriteMaskController : MonoBehaviour
{
	// Token: 0x0600005F RID: 95 RVA: 0x00004B06 File Offset: 0x00002D06
	private void OnEnable()
	{
		this.m_spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.m_uvs = DataUtility.GetInnerUV(this.m_spriteRenderer.sprite);
		this.m_spriteRenderer.sharedMaterial.SetVector("_CustomUVS", this.m_uvs);
	}

	// Token: 0x0400007E RID: 126
	private SpriteRenderer m_spriteRenderer;

	// Token: 0x0400007F RID: 127
	private Vector4 m_uvs;
}
