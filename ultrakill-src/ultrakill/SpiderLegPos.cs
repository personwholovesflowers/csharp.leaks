using System;
using UnityEngine;

// Token: 0x02000431 RID: 1073
public class SpiderLegPos : MonoBehaviour
{
	// Token: 0x06001833 RID: 6195 RVA: 0x000C5D88 File Offset: 0x000C3F88
	private void Start()
	{
		this.MoveLeg();
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x000C5D90 File Offset: 0x000C3F90
	private void Update()
	{
		if (this.movingLeg)
		{
			this.childLeg.transform.position = Vector3.MoveTowards(this.childLeg.transform.position, base.transform.position, Time.deltaTime * (20f * Vector3.Distance(base.transform.position, this.childLeg.transform.position) + 0.1f));
			if (this.childLeg.transform.position == base.transform.position)
			{
				this.movingLeg = false;
				return;
			}
		}
		else if (Vector3.Distance(base.transform.position, this.childLeg.transform.position) > 3f)
		{
			this.MoveLeg();
		}
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x000C5E64 File Offset: 0x000C4064
	private void MoveLeg()
	{
		bool flag = false;
		if (!this.backLeg)
		{
			if (Physics.Raycast(base.transform.position, base.transform.up * -1f + (base.transform.forward + base.transform.right * -1f) * Random.Range(-1f, 2f), out this.hit, 35f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				flag = true;
			}
		}
		else if (Physics.Raycast(base.transform.position, base.transform.up * -1f + (base.transform.forward + base.transform.right) * -1f * Random.Range(-1f, 2f), out this.hit, 35f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			flag = true;
		}
		if (flag && this.hit.transform != null)
		{
			this.sl.target = this.hit.point;
			this.movingLeg = true;
		}
	}

	// Token: 0x040021FA RID: 8698
	public GameObject childLeg;

	// Token: 0x040021FB RID: 8699
	public SpiderLeg sl;

	// Token: 0x040021FC RID: 8700
	private bool movingLeg;

	// Token: 0x040021FD RID: 8701
	private RaycastHit hit;

	// Token: 0x040021FE RID: 8702
	public bool backLeg;
}
