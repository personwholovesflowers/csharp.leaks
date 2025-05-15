using System;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class EndlessPrefabAnimator : MonoBehaviour
{
	// Token: 0x0600076C RID: 1900 RVA: 0x00031D0C File Offset: 0x0002FF0C
	public void Start()
	{
		if (!this.pooledId)
		{
			this.pooledId = base.GetComponent<CyberPooledPrefab>();
		}
		this.origPos = base.transform.position;
		if (!this.reverseOnly)
		{
			base.transform.position = this.origPos - Vector3.up * 20f;
			this.moving = true;
		}
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00031D78 File Offset: 0x0002FF78
	private void Update()
	{
		if (this.moving)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.origPos, Time.deltaTime * 2f + 5f * Vector3.Distance(base.transform.position, this.origPos) * Time.deltaTime);
			if (base.transform.position == this.origPos)
			{
				this.moving = false;
				this.eg = base.GetComponentInParent<EndlessGrid>();
				this.eg.OnePrefabDone();
				return;
			}
		}
		else if (this.reverse)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.origPos - Vector3.up * 20f, Time.deltaTime * 2f + 5f * Vector3.Distance(base.transform.position, this.origPos) * Time.deltaTime);
			if (base.transform.position == this.origPos - Vector3.up * 20f)
			{
				if (this.pooledId)
				{
					base.gameObject.SetActive(false);
					return;
				}
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x0400099A RID: 2458
	private Vector3 origPos;

	// Token: 0x0400099B RID: 2459
	private bool moving;

	// Token: 0x0400099C RID: 2460
	public bool reverse;

	// Token: 0x0400099D RID: 2461
	public bool reverseOnly;

	// Token: 0x0400099E RID: 2462
	private EndlessGrid eg;

	// Token: 0x0400099F RID: 2463
	private CyberPooledPrefab pooledId;
}
