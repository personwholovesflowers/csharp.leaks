using System;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class EndlessStairs : MonoBehaviour
{
	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x0600076F RID: 1903 RVA: 0x00031ED8 File Offset: 0x000300D8
	public MeshRenderer PrimaryMeshRenderer
	{
		get
		{
			return this.primaryMeshRenderer;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000770 RID: 1904 RVA: 0x00031EE0 File Offset: 0x000300E0
	public MeshRenderer SecondaryMeshRenderer
	{
		get
		{
			return this.secondaryMeshRenderer;
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000771 RID: 1905 RVA: 0x00031EE8 File Offset: 0x000300E8
	public MeshFilter PrimaryMeshFilter
	{
		get
		{
			return this.primaryMeshFilter;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000772 RID: 1906 RVA: 0x00031EF0 File Offset: 0x000300F0
	public MeshFilter SecondaryMeshFilter
	{
		get
		{
			return this.secondaryMeshFilter;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000773 RID: 1907 RVA: 0x00031EF8 File Offset: 0x000300F8
	public bool ActivateFirst
	{
		get
		{
			return this.activateFirst;
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000774 RID: 1908 RVA: 0x00031F00 File Offset: 0x00030100
	public bool ActivateSecond
	{
		get
		{
			return this.activateSecond;
		}
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x00031F08 File Offset: 0x00030108
	private void Start()
	{
		this.lmask = 16777216;
		this.primaryStairs = base.transform.GetChild(0);
		this.secondaryStairs = base.transform.GetChild(1);
		if (this.RayCastCheck(base.transform.forward, 1f))
		{
			if (!this.RayCastCheck(base.transform.forward * -1f, 1f))
			{
				this.activateFirst = true;
				this.primaryStairs.forward = base.transform.forward;
			}
		}
		else if (this.RayCastCheck(base.transform.forward * -1f, 1f))
		{
			this.activateFirst = true;
			this.primaryStairs.forward = base.transform.forward * -1f;
		}
		if (this.RayCastCheck(base.transform.right, 1f))
		{
			if (!this.RayCastCheck(base.transform.right * -1f, 1f))
			{
				this.activateSecond = true;
				this.secondaryStairs.forward = base.transform.right;
			}
		}
		else if (this.RayCastCheck(base.transform.right * -1f, 1f))
		{
			this.activateSecond = true;
			this.secondaryStairs.forward = base.transform.right * -1f;
		}
		if (this.activateFirst && this.RayCastCheck(this.primaryStairs.forward, 4f))
		{
			this.primaryStairs.localScale = new Vector3(1f, 2f, 1f);
		}
		if (this.activateSecond && this.RayCastCheck(this.secondaryStairs.forward, 4f))
		{
			this.secondaryStairs.localScale = new Vector3(1f, 2f, 1f);
		}
		if (this.primaryStairs.localScale.y == 2f && this.activateSecond && this.secondaryStairs.localScale.y == 1f)
		{
			this.activateFirst = false;
		}
		else if (this.secondaryStairs.localScale.y == 2f && this.activateFirst && this.primaryStairs.localScale.y == 1f)
		{
			this.activateSecond = false;
		}
		base.Invoke("ActivationTime", 0.1f);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00032198 File Offset: 0x00030398
	private bool RayCastCheck(Vector3 direction, float height = 1f)
	{
		return Physics.Raycast(base.transform.position + Vector3.up * height, direction, 3f, this.lmask) && !Physics.Raycast(base.transform.position + Vector3.up * 6f, direction, 3f, this.lmask);
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00032214 File Offset: 0x00030414
	private void ActivationTime()
	{
		this.moving = true;
		if (this.activateFirst)
		{
			this.primaryStairs.position = base.transform.position - Vector3.up * 5f;
			this.primaryStairs.gameObject.SetActive(true);
		}
		if (this.activateSecond)
		{
			this.secondaryStairs.position = base.transform.position - Vector3.up * 5f;
			this.secondaryStairs.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x000322B0 File Offset: 0x000304B0
	private void Update()
	{
		if (this.moving)
		{
			if (this.activateFirst)
			{
				this.primaryStairs.position = Vector3.MoveTowards(this.primaryStairs.position, base.transform.position, Time.deltaTime * 2f + 5f * Vector3.Distance(this.primaryStairs.position, base.transform.position) * Time.deltaTime);
			}
			if (this.activateSecond)
			{
				this.secondaryStairs.position = Vector3.MoveTowards(this.secondaryStairs.position, base.transform.position, Time.deltaTime * 2f + 5f * Vector3.Distance(this.secondaryStairs.position, base.transform.position) * Time.deltaTime);
			}
			if ((!this.activateFirst || this.primaryStairs.position == base.transform.position) && (!this.activateSecond || this.secondaryStairs.position == base.transform.position))
			{
				this.moving = false;
				base.GetComponentInParent<EndlessGrid>().OnePrefabDone();
			}
		}
	}

	// Token: 0x040009A0 RID: 2464
	[SerializeField]
	private MeshRenderer primaryMeshRenderer;

	// Token: 0x040009A1 RID: 2465
	[SerializeField]
	private MeshRenderer secondaryMeshRenderer;

	// Token: 0x040009A2 RID: 2466
	[SerializeField]
	private MeshFilter primaryMeshFilter;

	// Token: 0x040009A3 RID: 2467
	[SerializeField]
	private MeshFilter secondaryMeshFilter;

	// Token: 0x040009A4 RID: 2468
	private Transform primaryStairs;

	// Token: 0x040009A5 RID: 2469
	private Transform secondaryStairs;

	// Token: 0x040009A6 RID: 2470
	private LayerMask lmask;

	// Token: 0x040009A7 RID: 2471
	private bool activateFirst;

	// Token: 0x040009A8 RID: 2472
	private bool activateSecond;

	// Token: 0x040009A9 RID: 2473
	private bool moving;
}
