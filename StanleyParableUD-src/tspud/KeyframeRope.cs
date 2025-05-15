using System;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class KeyframeRope : HammerEntity
{
	// Token: 0x060005FE RID: 1534 RVA: 0x00020C0D File Offset: 0x0001EE0D
	private void Awake()
	{
		base.transform.rotation = Quaternion.identity;
		this.InitNodes();
		this.InitMesh();
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x00020C2B File Offset: 0x0001EE2B
	private void Start()
	{
		this.UpdateRope();
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x00020C34 File Offset: 0x0001EE34
	private void InitNodes()
	{
		if (this.nextKeyObj == null)
		{
			return;
		}
		int num = 10 + (int)Mathf.Pow((float)this.subdivisions, 2f);
		this.nodes = new KeyframeRope.node[num];
		this.nodes[0].position = base.transform.position;
		this.nodes[this.nodes.Length - 1].position = this.nextKeyObj.transform.position;
		for (int i = 1; i < num - 1; i++)
		{
			this.nodes[i].position = Vector3.Lerp(this.nodes[0].position, this.nodes[this.nodes.Length - 1].position, (float)i / (float)(num - 1));
		}
		this.nodeColors = new Color[num];
		for (int j = 0; j < num; j++)
		{
			this.nodeColors[j] = Color.Lerp(Color.green, Color.red, (float)j / (float)(num - 1));
		}
		this.length = Vector3.Distance(this.nodes[0].position, this.nodes[this.nodes.Length - 1].position);
		this.slackLength = this.length * Mathf.Pow(1f + this.slack, this.slackPower);
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x00020DA4 File Offset: 0x0001EFA4
	private void InitMesh()
	{
		if (this.nextKeyObj == null)
		{
			return;
		}
		this.render = base.GetComponent<MeshRenderer>();
		if (this.render == null)
		{
			this.render = base.gameObject.AddComponent<MeshRenderer>();
		}
		this.meshFilter = base.GetComponent<MeshFilter>();
		if (this.meshFilter == null)
		{
			this.meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		this.mesh = this.meshFilter.mesh;
		this.verts = new Vector3[this.nodes.Length * this.segments];
		this.tris = new int[this.nodes.Length * this.segments * 6];
		for (int i = 0; i < this.nodes.Length; i++)
		{
			for (int j = 0; j < this.segments; j++)
			{
				if (i < this.nodes.Length - 1)
				{
					int num = 0;
					if (j == this.segments - 1)
					{
						num = -this.segments;
					}
					this.tris[j * this.segments + j + i * this.segments * 6] = i * this.segments + j;
					this.tris[1 + j * this.segments + j + i * this.segments * 6] = i * this.segments + j + 1 + num;
					this.tris[2 + j * this.segments + j + i * this.segments * 6] = i * this.segments + j + this.segments;
					this.tris[3 + j * this.segments + j + i * this.segments * 6] = i * this.segments + j + 1 + num;
					this.tris[4 + j * this.segments + j + i * this.segments * 6] = i * this.segments + j + 1 + num + this.segments;
					this.tris[5 + j * this.segments + j + i * this.segments * 6] = i * this.segments + j + this.segments;
				}
			}
		}
		this.mesh.vertices = this.verts;
		this.mesh.triangles = this.tris;
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x00020FE3 File Offset: 0x0001F1E3
	private void OnBecameVisible()
	{
		this.visible = true;
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x00020FEC File Offset: 0x0001F1EC
	private void OnBecameInvisible()
	{
		this.visible = false;
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x00020FF5 File Offset: 0x0001F1F5
	private void Update()
	{
		if (!this.visible)
		{
			return;
		}
		this.UpdateRope();
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x00021008 File Offset: 0x0001F208
	private void UpdateRope()
	{
		if (this.nextKeyObj == null)
		{
			return;
		}
		this.slackLength = this.length * Mathf.Pow(1f + this.slack, this.slackPower);
		if (!this.precalculated)
		{
			this.precalculated = true;
			for (int i = 0; i < 10000; i++)
			{
				this.UpdateNodes();
			}
		}
		this.UpdateNodes();
		this.UpdateMesh();
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0002107C File Offset: 0x0001F27C
	private void UpdateNodes()
	{
		this.nodes[0].position = base.transform.position;
		this.nodes[this.nodes.Length - 1].position = this.nextKeyObj.transform.position;
		for (int i = 1; i < this.nodes.Length - 1; i++)
		{
			this.nodes[i].cachedPosition = this.nodes[i].position;
			this.nodes[i].velocity = Vector3.ClampMagnitude(this.nodes[i].velocity, this.velocityLimit);
			KeyframeRope.node[] array = this.nodes;
			int num = i;
			array[num].velocity = array[num].velocity * 0.9f;
		}
		float gameDeltaTime = Singleton<GameMaster>.Instance.GameDeltaTime;
		this.steps = Mathf.CeilToInt(Mathf.Log(80f * gameDeltaTime, 2f));
		this.steps = Mathf.Clamp(this.steps, 1, 3);
		for (int j = 0; j < this.steps; j++)
		{
			this.TimeStep(gameDeltaTime * (1f / (float)this.steps));
		}
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x000211BC File Offset: 0x0001F3BC
	private void TimeStep(float delta)
	{
		if (delta > 0.1f)
		{
			delta = 0.1f;
		}
		this.lengthPerNode = this.slackLength / ((float)this.nodes.Length * 4f);
		for (int i = 1; i < this.nodes.Length - 1; i++)
		{
			Vector3 vector = Vector3.zero;
			int num = 1;
			for (int j = -num; j <= num; j++)
			{
				if (i + j >= 0 && i + j < this.nodes.Length && j != 0)
				{
					Vector3 vector2 = this.nodes[i + j].position - this.nodes[i].position;
					Vector3 vector3 = -(this.lengthPerNode * (float)Mathf.Abs(j) - vector2.magnitude) * vector2.normalized * 1500f * this.springForceMultiplier / (float)Mathf.Abs(j);
					if (this.useSpringForceLimit)
					{
						vector3 = Vector3.ClampMagnitude(vector3, this.springForceLimit);
					}
					vector += vector3;
					if (i + j == 0 || i + j == this.nodes.Length - 1)
					{
						vector += vector3;
					}
				}
			}
			if (this.gravityToggle)
			{
				vector += Vector3.down * 9.8f * this.nodeWeight;
			}
			KeyframeRope.node[] array = this.nodes;
			int num2 = i;
			array[num2].velocity = array[num2].velocity + vector * delta * (this.useOldCalcs ? 1f : delta);
			if (this.showDebug)
			{
				int num3 = this.nodes.Length / 2;
			}
		}
		for (int k = 1; k < this.nodes.Length - 1; k++)
		{
			if (this.nodes[k].velocity.sqrMagnitude < 1E-07f)
			{
				this.nodes[k].velocity = Vector3.zero;
			}
			else
			{
				this.nodes[k].velocity = Vector3.ClampMagnitude(this.nodes[k].velocity, this.velocityLimit);
				KeyframeRope.node[] array2 = this.nodes;
				int num4 = k;
				array2[num4].position = array2[num4].position + this.nodes[k].velocity * delta;
			}
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00021434 File Offset: 0x0001F634
	private void UpdateMesh()
	{
		Vector3 position = base.transform.position;
		for (int i = 0; i < this.nodes.Length; i++)
		{
			bool flag = false;
			Vector3 vector;
			if (i == this.nodes.Length - 1)
			{
				vector = this.nodes[i].position - this.nodes[i - 1].position;
				if (this.nodes[i].position == this.nodes[i].cachedPosition && this.nodes[i - 1].position == this.nodes[i - 1].cachedPosition)
				{
					flag = true;
				}
			}
			else if (i == 0)
			{
				vector = this.nodes[i + 1].position - this.nodes[i].position;
				if (this.nodes[i].position == this.nodes[i].cachedPosition && this.nodes[i + 1].position == this.nodes[i + 1].cachedPosition)
				{
					flag = true;
				}
			}
			else
			{
				vector = this.nodes[i + 1].position - this.nodes[i].position + (this.nodes[i].position - this.nodes[i - 1].position);
				vector *= 0.5f;
				if (this.nodes[i].position == this.nodes[i].cachedPosition && this.nodes[i - 1].position == this.nodes[i - 1].cachedPosition && this.nodes[i + 1].position == this.nodes[i + 1].cachedPosition)
				{
					flag = true;
				}
			}
			if (!flag || this.firstMeshPass)
			{
				Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
				for (int j = 0; j < this.segments; j++)
				{
					Vector3 vector2 = Quaternion.AngleAxis((float)(j * (360 / this.segments)), vector) * normalized;
					this.verts[i * this.segments + j] = this.nodes[i].position - position + vector2 * this.width * 0.5f;
				}
			}
		}
		this.mesh.vertices = this.verts;
		this.mesh.RecalculateBounds();
		this.mesh.RecalculateNormals();
		this.firstMeshPass = false;
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x00021750 File Offset: 0x0001F950
	public void OnDrawGizmos()
	{
		if (this.nodes != null)
		{
			for (int i = 0; i < this.nodes.Length - 1; i++)
			{
				Gizmos.color = this.nodeColors[i];
				Gizmos.DrawLine(this.nodes[i].position, this.nodes[i + 1].position);
			}
		}
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x000217B4 File Offset: 0x0001F9B4
	public void DrawDebugRope()
	{
		if (this.nodes != null)
		{
			for (int i = 0; i < this.nodes.Length - 1; i++)
			{
				Debug.DrawLine(this.nodes[i].position, this.nodes[i + 1].position, this.nodeColors[i]);
				Debug.DrawLine(this.nodes[i].position, this.nodes[i].position + Vector3.up * 0.2f, this.nodeColors[i]);
			}
		}
	}

	// Token: 0x04000640 RID: 1600
	public bool start;

	// Token: 0x04000641 RID: 1601
	public string nextKeyName = "";

	// Token: 0x04000642 RID: 1602
	public GameObject nextKeyObj;

	// Token: 0x04000643 RID: 1603
	public float slack = 25f;

	// Token: 0x04000644 RID: 1604
	public float springForceMultiplier = 10f;

	// Token: 0x04000645 RID: 1605
	public int subdivisions = 2;

	// Token: 0x04000646 RID: 1606
	public float width = 0.02f;

	// Token: 0x04000647 RID: 1607
	public float nodeWeight = 5f;

	// Token: 0x04000648 RID: 1608
	private int steps = 1;

	// Token: 0x04000649 RID: 1609
	[SerializeField]
	private KeyframeRope.node[] nodes;

	// Token: 0x0400064A RID: 1610
	[SerializeField]
	[HideInInspector]
	private Color[] nodeColors;

	// Token: 0x0400064B RID: 1611
	private float length;

	// Token: 0x0400064C RID: 1612
	private float actualLength;

	// Token: 0x0400064D RID: 1613
	private float slackLength;

	// Token: 0x0400064E RID: 1614
	private float slackPower = 1.75f;

	// Token: 0x0400064F RID: 1615
	private float lengthPerNode;

	// Token: 0x04000650 RID: 1616
	public bool gravityToggle = true;

	// Token: 0x04000651 RID: 1617
	private float spring = 20f;

	// Token: 0x04000652 RID: 1618
	private bool precalculated;

	// Token: 0x04000653 RID: 1619
	[SerializeField]
	[HideInInspector]
	private MeshRenderer render;

	// Token: 0x04000654 RID: 1620
	[SerializeField]
	[HideInInspector]
	private MeshFilter meshFilter;

	// Token: 0x04000655 RID: 1621
	[SerializeField]
	[HideInInspector]
	private Mesh mesh;

	// Token: 0x04000656 RID: 1622
	[SerializeField]
	[HideInInspector]
	private Vector3[] verts;

	// Token: 0x04000657 RID: 1623
	[SerializeField]
	[HideInInspector]
	private int[] tris;

	// Token: 0x04000658 RID: 1624
	private int segments = 5;

	// Token: 0x04000659 RID: 1625
	private float velocityLimit = 5f;

	// Token: 0x0400065A RID: 1626
	public bool useOldCalcs;

	// Token: 0x0400065B RID: 1627
	public bool useSpringForceLimit;

	// Token: 0x0400065C RID: 1628
	public float springForceLimit = 10000f;

	// Token: 0x0400065D RID: 1629
	public bool showDebug;

	// Token: 0x0400065E RID: 1630
	private bool firstMeshPass = true;

	// Token: 0x0400065F RID: 1631
	private bool visible;

	// Token: 0x020003C6 RID: 966
	[Serializable]
	private struct node
	{
		// Token: 0x040013F2 RID: 5106
		public Vector3 position;

		// Token: 0x040013F3 RID: 5107
		public Vector3 velocity;

		// Token: 0x040013F4 RID: 5108
		public Vector3 cachedPosition;
	}
}
