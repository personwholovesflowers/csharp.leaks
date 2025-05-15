using System;
using UnityEngine;

// Token: 0x020004C8 RID: 1224
public class WeaponTrail : MonoBehaviour
{
	// Token: 0x06001C03 RID: 7171 RVA: 0x000E8907 File Offset: 0x000E6B07
	private void Awake()
	{
		if (!this.trailTemplate)
		{
			this.trailTemplate = base.transform.GetChild(0).gameObject;
			this.trailTemplate.SetActive(false);
		}
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x000E893C File Offset: 0x000E6B3C
	public void AddTrail()
	{
		if (!this.trailTemplate)
		{
			this.trailTemplate = base.transform.GetChild(0).gameObject;
			this.trailTemplate.SetActive(false);
		}
		if (!this.currentTrail)
		{
			this.currentTrail = Object.Instantiate<GameObject>(this.trailTemplate, base.transform);
			this.currentTrail.SetActive(true);
		}
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x000E89A9 File Offset: 0x000E6BA9
	public void RemoveTrail()
	{
		if (this.currentTrail)
		{
			this.currentTrail.AddComponent<RemoveOnTime>().time = 5f;
			this.currentTrail.transform.parent = null;
			this.currentTrail = null;
		}
	}

	// Token: 0x0400278C RID: 10124
	private GameObject trailTemplate;

	// Token: 0x0400278D RID: 10125
	private GameObject currentTrail;
}
