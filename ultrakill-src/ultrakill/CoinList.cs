using System;
using System.Collections.Generic;

// Token: 0x020000CA RID: 202
public class CoinList : MonoSingleton<CoinList>
{
	// Token: 0x06000404 RID: 1028 RVA: 0x0001BEA1 File Offset: 0x0001A0A1
	public void AddCoin(Coin coin)
	{
		if (!this.revolverCoinsList.Contains(coin))
		{
			this.revolverCoinsList.Add(coin);
		}
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0001BEBD File Offset: 0x0001A0BD
	public void RemoveCoin(Coin coin)
	{
		if (this.revolverCoinsList.Contains(coin))
		{
			this.revolverCoinsList.Remove(coin);
		}
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0001BEDA File Offset: 0x0001A0DA
	private void Start()
	{
		base.Invoke("SlowUpdate", 30f);
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0001BEEC File Offset: 0x0001A0EC
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 30f);
		for (int i = this.revolverCoinsList.Count - 1; i >= 0; i--)
		{
			if (this.revolverCoinsList[i] == null)
			{
				this.revolverCoinsList.RemoveAt(i);
			}
		}
	}

	// Token: 0x040004F7 RID: 1271
	public List<Coin> revolverCoinsList = new List<Coin>();
}
