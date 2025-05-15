using System;
using TMPro;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class MoneyText : MonoBehaviour
{
	// Token: 0x06001176 RID: 4470 RVA: 0x000882FE File Offset: 0x000864FE
	private void OnEnable()
	{
		this.UpdateMoney();
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x00088306 File Offset: 0x00086506
	public void UpdateMoney()
	{
		if (this.text == null)
		{
			this.text = base.GetComponent<TMP_Text>();
		}
		this.text.text = MoneyText.DivideMoney(GameProgressSaver.GetMoney()) + " <color=#FF4343>P</color>";
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x00088344 File Offset: 0x00086544
	public static string DivideMoney(int dosh)
	{
		int i = dosh;
		int j = 0;
		int num = 0;
		if (dosh > 1000000000)
		{
			return "LIKE, A LOT OF ";
		}
		while (i >= 1000)
		{
			j++;
			i -= 1000;
		}
		while (j >= 1000)
		{
			num++;
			j -= 1000;
		}
		string text;
		if (num > 0)
		{
			text = num.ToString() + ",";
			if (j < 10)
			{
				text = text + "00" + j.ToString() + ",";
			}
			else if (j < 100)
			{
				text = text + "0" + j.ToString() + ",";
			}
			else
			{
				text = text + j.ToString() + ",";
			}
			if (i < 10)
			{
				text = text + "00" + i.ToString();
			}
			else if (i < 100)
			{
				text = text + "0" + i.ToString();
			}
			else
			{
				text += i.ToString();
			}
		}
		else if (j > 0)
		{
			text = j.ToString() + ",";
			if (i < 10)
			{
				text = text + "00" + i.ToString();
			}
			else if (i < 100)
			{
				text = text + "0" + i.ToString();
			}
			else
			{
				text += i.ToString();
			}
		}
		else
		{
			text = i.ToString() ?? "";
		}
		return text;
	}

	// Token: 0x040017BB RID: 6075
	private TMP_Text text;
}
