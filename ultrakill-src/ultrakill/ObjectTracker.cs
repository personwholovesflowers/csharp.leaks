using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200031F RID: 799
public class ObjectTracker : MonoSingleton<ObjectTracker>
{
	// Token: 0x06001268 RID: 4712 RVA: 0x00093D2D File Offset: 0x00091F2D
	public void AddGrenade(Grenade gren)
	{
		if (!this.grenadeList.Contains(gren))
		{
			this.grenadeList.Add(gren);
		}
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x00093D49 File Offset: 0x00091F49
	public void AddCannonball(Cannonball cb)
	{
		if (!this.cannonballList.Contains(cb))
		{
			this.cannonballList.Add(cb);
		}
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x00093D65 File Offset: 0x00091F65
	public void AddLandmine(Landmine lm)
	{
		if (!this.landmineList.Contains(lm))
		{
			this.landmineList.Add(lm);
		}
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x00093D81 File Offset: 0x00091F81
	public void AddMagnet(Magnet mag)
	{
		if (!this.magnetList.Contains(mag))
		{
			this.magnetList.Add(mag);
		}
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x00093D9D File Offset: 0x00091F9D
	public void AddZappable(Zappable zap)
	{
		if (!this.zappablesList.Contains(zap))
		{
			this.zappablesList.Add(zap);
		}
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x00093DB9 File Offset: 0x00091FB9
	public void RemoveGrenade(Grenade gren)
	{
		if (this.grenadeList.Contains(gren))
		{
			this.grenadeList.Remove(gren);
		}
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x00093DD6 File Offset: 0x00091FD6
	public void RemoveCannonball(Cannonball cb)
	{
		if (this.cannonballList.Contains(cb))
		{
			this.cannonballList.Remove(cb);
		}
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x00093DF3 File Offset: 0x00091FF3
	public void RemoveLandmine(Landmine lm)
	{
		if (this.landmineList.Contains(lm))
		{
			this.landmineList.Remove(lm);
		}
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x00093E10 File Offset: 0x00092010
	public void RemoveMagnet(Magnet mag)
	{
		if (this.magnetList.Contains(mag))
		{
			this.magnetList.Remove(mag);
		}
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x00093E2D File Offset: 0x0009202D
	public void RemoveZappable(Zappable zap)
	{
		if (this.zappablesList.Contains(zap))
		{
			this.zappablesList.Remove(zap);
		}
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x00093E4C File Offset: 0x0009204C
	public Grenade GetGrenade(Transform tf)
	{
		for (int i = this.grenadeList.Count - 1; i >= 0; i--)
		{
			if (this.grenadeList[i] != null && this.grenadeList[i].transform == tf)
			{
				return this.grenadeList[i];
			}
		}
		return null;
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x00093EAC File Offset: 0x000920AC
	public Cannonball GetCannonball(Transform tf)
	{
		for (int i = this.cannonballList.Count - 1; i >= 0; i--)
		{
			if (this.cannonballList[i] != null && this.cannonballList[i].transform == tf)
			{
				return this.cannonballList[i];
			}
		}
		return null;
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x00093F0C File Offset: 0x0009210C
	public Landmine GetLandmine(Transform tf)
	{
		for (int i = this.landmineList.Count - 1; i >= 0; i--)
		{
			if (this.landmineList[i] != null && this.landmineList[i].transform == tf)
			{
				return this.landmineList[i];
			}
		}
		return null;
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x00093F6C File Offset: 0x0009216C
	public bool HasTransform(Transform tf)
	{
		for (int i = this.grenadeList.Count - 1; i >= 0; i--)
		{
			if (this.grenadeList[i] != null && this.grenadeList[i].transform == tf)
			{
				return true;
			}
		}
		for (int j = this.cannonballList.Count - 1; j >= 0; j--)
		{
			if (this.cannonballList[j] != null && this.cannonballList[j].transform == tf)
			{
				return true;
			}
		}
		for (int k = this.landmineList.Count - 1; k >= 0; k--)
		{
			if (this.landmineList[k] != null && this.landmineList[k].transform == tf)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x0001BEDA File Offset: 0x0001A0DA
	private void Start()
	{
		base.Invoke("SlowUpdate", 30f);
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x00094050 File Offset: 0x00092250
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 30f);
		for (int i = this.grenadeList.Count - 1; i >= 0; i--)
		{
			if (this.grenadeList[i] == null)
			{
				this.grenadeList.RemoveAt(i);
			}
		}
		for (int j = this.cannonballList.Count - 1; j >= 0; j--)
		{
			if (this.cannonballList[j] == null)
			{
				this.cannonballList.RemoveAt(j);
			}
		}
		for (int k = this.landmineList.Count - 1; k >= 0; k--)
		{
			if (this.landmineList[k] == null)
			{
				this.landmineList.RemoveAt(k);
			}
		}
		for (int l = this.magnetList.Count - 1; l >= 0; l--)
		{
			if (this.magnetList[l] == null)
			{
				this.magnetList.RemoveAt(l);
			}
		}
		for (int m = this.zappablesList.Count - 1; m >= 0; m--)
		{
			if (this.zappablesList[m] == null)
			{
				this.zappablesList.RemoveAt(m);
			}
		}
	}

	// Token: 0x04001967 RID: 6503
	public List<Grenade> grenadeList = new List<Grenade>();

	// Token: 0x04001968 RID: 6504
	public List<Cannonball> cannonballList = new List<Cannonball>();

	// Token: 0x04001969 RID: 6505
	public List<Landmine> landmineList = new List<Landmine>();

	// Token: 0x0400196A RID: 6506
	public List<Magnet> magnetList = new List<Magnet>();

	// Token: 0x0400196B RID: 6507
	public List<Zappable> zappablesList = new List<Zappable>();
}
