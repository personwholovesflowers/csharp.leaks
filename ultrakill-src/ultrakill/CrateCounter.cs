using System;
using System.Globalization;
using System.Threading;
using TMPro;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class CrateCounter : MonoSingleton<CrateCounter>
{
	// Token: 0x060004B3 RID: 1203 RVA: 0x00020108 File Offset: 0x0001E308
	private void Start()
	{
		this.UpdateDisplay();
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00020110 File Offset: 0x0001E310
	public void AddCrate()
	{
		this.currentCrates++;
		this.unsavedCrates++;
		this.UpdateDisplay();
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00020134 File Offset: 0x0001E334
	public void AddCoin()
	{
		this.currentCoins++;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00020144 File Offset: 0x0001E344
	public void SaveStuff()
	{
		this.unsavedCrates = 0;
		this.savedCoins += this.currentCoins;
		this.currentCoins = 0;
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00020168 File Offset: 0x0001E368
	public void CoinsToPoints()
	{
		if (this.savedCoins > 0)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			GameProgressSaver.AddMoney(this.savedCoins * 100);
			MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage(string.Concat(new string[]
			{
				"<color=grey>TRANSACTION COMPLETE:</color> ",
				this.savedCoins.ToString(),
				" COINS <color=orange>=></color> ",
				StatsManager.DivideMoney(this.savedCoins * 100),
				"<color=orange>P</color>"
			}), "", "", 0, false, false, true);
			this.savedCoins = 0;
		}
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00020204 File Offset: 0x0001E404
	public void ResetUnsavedStuff()
	{
		this.currentCrates -= this.unsavedCrates;
		this.unsavedCrates = 0;
		this.currentCoins = 0;
		this.UpdateDisplay();
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00020230 File Offset: 0x0001E430
	private void UpdateDisplay()
	{
		if (this.display)
		{
			this.display.text = this.currentCrates.ToString() + " / " + this.crateAmount.ToString();
		}
		if (this.crateAmount == 0)
		{
			return;
		}
		if (this.success || this.currentCrates < this.crateAmount)
		{
			if (this.success && this.currentCrates < this.crateAmount)
			{
				this.success = false;
				UltrakillEvent ultrakillEvent = this.onAllCratesGet;
				if (ultrakillEvent == null)
				{
					return;
				}
				ultrakillEvent.Revert();
			}
			return;
		}
		this.success = true;
		UltrakillEvent ultrakillEvent2 = this.onAllCratesGet;
		if (ultrakillEvent2 == null)
		{
			return;
		}
		ultrakillEvent2.Invoke("");
	}

	// Token: 0x0400065D RID: 1629
	public int crateAmount;

	// Token: 0x0400065E RID: 1630
	private int currentCrates;

	// Token: 0x0400065F RID: 1631
	private int unsavedCrates;

	// Token: 0x04000660 RID: 1632
	[SerializeField]
	private TMP_Text display;

	// Token: 0x04000661 RID: 1633
	private int currentCoins;

	// Token: 0x04000662 RID: 1634
	private int savedCoins;

	// Token: 0x04000663 RID: 1635
	private bool success;

	// Token: 0x04000664 RID: 1636
	public UltrakillEvent onAllCratesGet;
}
