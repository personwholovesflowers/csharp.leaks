using System;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class EndlessCube : MonoBehaviour
{
	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000745 RID: 1861 RVA: 0x0002F267 File Offset: 0x0002D467
	public MeshRenderer MeshRenderer
	{
		get
		{
			return this.meshRenderer;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000746 RID: 1862 RVA: 0x0002F26F File Offset: 0x0002D46F
	public MeshFilter MeshFilter
	{
		get
		{
			return this.meshFilter;
		}
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0002F277 File Offset: 0x0002D477
	private void Awake()
	{
		this.tf = base.transform;
		this.eg = base.GetComponentInParent<EndlessGrid>();
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x0002F294 File Offset: 0x0002D494
	private void Update()
	{
		if (this.active)
		{
			this.tf.position = Vector3.MoveTowards(this.tf.position, this.targetPos, (Vector3.Distance(this.tf.position, this.targetPos) * 1.75f + this.speed) * Time.deltaTime);
			if (this.tf.position == this.targetPos)
			{
				this.eg.OneDone();
				this.active = false;
			}
		}
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x0002F320 File Offset: 0x0002D520
	public void SetTarget(float target)
	{
		this.targetPos = new Vector3(this.tf.position.x, target, this.tf.position.z);
		this.speed = (float)Random.Range(9, 11);
		base.Invoke("StartMoving", Random.Range(0f, 0.5f));
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x0002F383 File Offset: 0x0002D583
	private void StartMoving()
	{
		this.active = true;
	}

	// Token: 0x0400094F RID: 2383
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04000950 RID: 2384
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x04000951 RID: 2385
	public Vector2Int positionOnGrid;

	// Token: 0x04000952 RID: 2386
	public bool blockedByPrefab;

	// Token: 0x04000953 RID: 2387
	private Vector3 targetPos;

	// Token: 0x04000954 RID: 2388
	private Transform tf;

	// Token: 0x04000955 RID: 2389
	private bool active;

	// Token: 0x04000956 RID: 2390
	private float speed;

	// Token: 0x04000957 RID: 2391
	private EndlessGrid eg;
}
