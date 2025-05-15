using System;
using TMPro;
using UnityEngine;

// Token: 0x020004C2 RID: 1218
public class WeaponOrderController : MonoBehaviour
{
	// Token: 0x06001BEC RID: 7148 RVA: 0x000E7F00 File Offset: 0x000E6100
	private void Start()
	{
		this.ResetValues();
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x000E7F00 File Offset: 0x000E6100
	private void OnEnable()
	{
		this.ResetValues();
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x000E7F08 File Offset: 0x000E6108
	public void ChangeOrderNumber(int additive)
	{
		int num = this.currentOrderNumber + additive;
		if (num > 0 && num < 4)
		{
			for (int i = 0; i < this.variationOrder.Length; i++)
			{
				if ((int)(this.variationOrder[i] - '0') == num)
				{
					this.variationOrder = this.variationOrder.Replace(this.variationOrder[i], this.variationOrder[this.variationNumber]);
				}
			}
			this.variationOrder = this.variationOrder.Remove(this.variationNumber, 1);
			this.variationOrder = this.variationOrder.Insert(this.variationNumber, num.ToString());
			MonoSingleton<PrefsManager>.Instance.SetString("weapon." + this.variationName + ".order", this.variationOrder);
			WeaponOrderController[] componentsInChildren = base.transform.parent.parent.parent.GetComponentsInChildren<WeaponOrderController>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].ResetValues();
			}
			GunSetter gunSetter = Object.FindObjectOfType<GunSetter>();
			if (gunSetter == null)
			{
				return;
			}
			gunSetter.ResetWeapons(false);
		}
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x000E8020 File Offset: 0x000E6220
	public void ResetValues()
	{
		if (!this.text)
		{
			this.text = base.GetComponentInChildren<TMP_Text>();
		}
		if (this.revolver)
		{
			this.variationOrder = MonoSingleton<PrefsManager>.Instance.GetString("weapon." + this.variationName + ".order", "1324");
		}
		else
		{
			this.variationOrder = MonoSingleton<PrefsManager>.Instance.GetString("weapon." + this.variationName + ".order", "1234");
		}
		this.currentOrderNumber = (int)(this.variationOrder[this.variationNumber] - '0');
		this.text.text = this.variationOrder[this.variationNumber].ToString() ?? "";
		Debug.Log("Order in WeaponOrderController: " + this.variationOrder);
	}

	// Token: 0x0400275F RID: 10079
	private TMP_Text text;

	// Token: 0x04002760 RID: 10080
	public int variationNumber;

	// Token: 0x04002761 RID: 10081
	public string variationName;

	// Token: 0x04002762 RID: 10082
	private string variationOrder;

	// Token: 0x04002763 RID: 10083
	private int currentOrderNumber;

	// Token: 0x04002764 RID: 10084
	public bool revolver;
}
