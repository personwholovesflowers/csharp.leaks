using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class ForestMaker : MonoBehaviour
{
	// Token: 0x06000043 RID: 67 RVA: 0x000043B4 File Offset: 0x000025B4
	private void Start()
	{
		if (this.m_treePrefab == null)
		{
			return;
		}
		this.m_ground.transform.localScale = new Vector3((float)(this.m_amount * 10), 1f, (float)(this.m_amount * 5) * 1.866f);
		for (int i = -this.m_amount / 2; i <= this.m_amount / 2; i++)
		{
			for (int j = -this.m_amount / 2; j <= this.m_amount / 2; j++)
			{
				if (Random.Range(0f, 1f) <= 0.5f)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_treePrefab);
					Vector3 zero = Vector3.zero;
					zero.x = ((float)i + (float)j * 0.5f - (float)((int)(((j < 0) ? ((float)j - 1f) : ((float)j)) / 2f))) * 2f * this.m_radiusDistance;
					zero.z = (float)j * 1.866f * this.m_radiusDistance;
					gameObject.transform.position = zero;
					float num = Random.Range(1f, 1.5f);
					gameObject.transform.localScale = Vector3.one * num;
					gameObject.transform.Rotate(Random.Range(-10f, 10f), Random.Range(-180f, 180f), Random.Range(-10f, 10f));
				}
			}
		}
	}

	// Token: 0x04000063 RID: 99
	public GameObject m_treePrefab;

	// Token: 0x04000064 RID: 100
	public int m_amount;

	// Token: 0x04000065 RID: 101
	public GameObject m_ground;

	// Token: 0x04000066 RID: 102
	public float m_radiusDistance;
}
