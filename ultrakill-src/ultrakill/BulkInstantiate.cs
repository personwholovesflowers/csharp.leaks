using System;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class BulkInstantiate : MonoBehaviour
{
	// Token: 0x060002F2 RID: 754 RVA: 0x00011543 File Offset: 0x0000F743
	private void OnEnable()
	{
		if (this.instantiateOnEnable)
		{
			this.Instantiate();
		}
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x00011553 File Offset: 0x0000F753
	private void Start()
	{
		if (this.instantiateOnStart)
		{
			this.Instantiate();
		}
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00011563 File Offset: 0x0000F763
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(base.transform.position, base.transform.lossyScale);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0001158C File Offset: 0x0000F78C
	public void Instantiate()
	{
		for (int i = 0; i < this.count; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.source);
			Vector3 position = base.transform.position;
			Vector3 vector = base.transform.localScale / 2f;
			Vector3 vector2 = new Vector3(Random.Range(position.x - vector.x, position.x + vector.x), Random.Range(position.y - vector.y, position.y + vector.y), Random.Range(position.z - vector.z, position.z + vector.z));
			gameObject.transform.position = vector2;
			InstantiateObjectMode instantiateObjectMode = this.mode;
			if (instantiateObjectMode != InstantiateObjectMode.ForceEnable)
			{
				if (instantiateObjectMode == InstantiateObjectMode.ForceDisable)
				{
					gameObject.SetActive(false);
				}
			}
			else
			{
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000391 RID: 913
	[SerializeField]
	private int count = 1;

	// Token: 0x04000392 RID: 914
	[SerializeField]
	private bool instantiateOnEnable;

	// Token: 0x04000393 RID: 915
	[SerializeField]
	private bool instantiateOnStart = true;

	// Token: 0x04000394 RID: 916
	[SerializeField]
	private GameObject source;

	// Token: 0x04000395 RID: 917
	[SerializeField]
	private InstantiateObjectMode mode;
}
