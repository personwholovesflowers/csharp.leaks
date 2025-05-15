using System;
using System.Collections.Generic;

// Token: 0x020000C0 RID: 192
public class CheckPointsController : MonoSingleton<CheckPointsController>
{
	// Token: 0x060003D1 RID: 977 RVA: 0x00018780 File Offset: 0x00016980
	public void DisableCheckpoints()
	{
		if (this.requests == 0)
		{
			foreach (CheckPoint checkPoint in this.cps)
			{
				checkPoint.forceOff = true;
			}
			foreach (ShopZone shopZone in this.shops)
			{
				shopZone.ForceOff();
			}
		}
		this.requests++;
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00018828 File Offset: 0x00016A28
	public void EnableCheckpoints()
	{
		this.requests--;
		if (this.requests <= 0)
		{
			foreach (CheckPoint checkPoint in this.cps)
			{
				checkPoint.forceOff = false;
				checkPoint.ReactivationEffect();
			}
			foreach (ShopZone shopZone in this.shops)
			{
				shopZone.StopForceOff();
			}
			this.requests = 0;
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x000188DC File Offset: 0x00016ADC
	public void AddCheckpoint(CheckPoint cp)
	{
		if (this.cps.Contains(cp))
		{
			return;
		}
		this.cps.Add(cp);
		if (this.requests > 0)
		{
			cp.forceOff = true;
			return;
		}
		if (cp.forceOff)
		{
			cp.forceOff = false;
			cp.ReactivationEffect();
		}
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0001892A File Offset: 0x00016B2A
	public void RemoveCheckpoint(CheckPoint cp)
	{
		if (!this.cps.Contains(cp))
		{
			return;
		}
		this.cps.Remove(cp);
		if (cp.forceOff)
		{
			cp.forceOff = false;
			cp.ReactivationEffect();
		}
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001895D File Offset: 0x00016B5D
	public void AddShop(ShopZone shop)
	{
		if (this.shops.Contains(shop))
		{
			return;
		}
		this.shops.Add(shop);
		if (this.requests > 0)
		{
			shop.ForceOff();
			return;
		}
		if (shop.forcedOff)
		{
			shop.StopForceOff();
		}
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x00018998 File Offset: 0x00016B98
	public void RemoveShop(ShopZone shop)
	{
		if (!this.shops.Contains(shop))
		{
			return;
		}
		this.shops.Remove(shop);
		if (shop.forcedOff)
		{
			shop.StopForceOff();
		}
	}

	// Token: 0x040004B7 RID: 1207
	private int requests;

	// Token: 0x040004B8 RID: 1208
	public List<CheckPoint> cps = new List<CheckPoint>();

	// Token: 0x040004B9 RID: 1209
	private List<ShopZone> shops = new List<ShopZone>();
}
