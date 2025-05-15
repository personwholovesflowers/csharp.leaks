using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023C RID: 572
public class GunColorLock : MonoBehaviour
{
	// Token: 0x06000C4C RID: 3148 RVA: 0x00057984 File Offset: 0x00055B84
	private void OnEnable()
	{
		if (this.weaponNumber == 0)
		{
			this.weaponNumber = base.GetComponentInParent<GunColorTypeGetter>().weaponNumber;
		}
		if (GameProgressSaver.HasWeaponCustomization((GameProgressSaver.WeaponCustomizationType)(this.weaponNumber - 1)))
		{
			UltrakillEvent ultrakillEvent = this.onUnlock;
			if (ultrakillEvent == null)
			{
				return;
			}
			ultrakillEvent.Invoke("");
			return;
		}
		else
		{
			if (GameProgressSaver.GetMoney() < 1000000)
			{
				this.button.interactable = false;
				this.buttonText.text = "<color=red>1,000,000 P</color>";
				this.button.GetComponent<Image>().color = Color.red;
				this.button.GetComponent<ShopButton>().failure = true;
				return;
			}
			this.button.interactable = true;
			this.buttonText.text = "1,000,000 <color=#FF4343>P</color>";
			this.button.GetComponent<Image>().color = Color.white;
			this.button.GetComponent<ShopButton>().failure = false;
			return;
		}
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00057A60 File Offset: 0x00055C60
	public void Unlock()
	{
		GameProgressSaver.AddMoney(-1000000);
		GameProgressSaver.UnlockWeaponCustomization((GameProgressSaver.WeaponCustomizationType)(this.weaponNumber - 1));
		UltrakillEvent ultrakillEvent = this.onUnlock;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		this.button.GetComponent<Image>().color = Color.white;
		base.GetComponentInParent<GunColorTypeGetter>().SetType(true);
		Object.Instantiate<GameObject>(this.buySound);
		MoneyText[] array = Object.FindObjectsOfType<MoneyText>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateMoney();
		}
		VariationInfo[] array2 = Object.FindObjectsOfType<VariationInfo>();
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].UpdateMoney();
		}
	}

	// Token: 0x04001027 RID: 4135
	private int weaponNumber;

	// Token: 0x04001028 RID: 4136
	public bool alreadyUnlocked;

	// Token: 0x04001029 RID: 4137
	public UltrakillEvent onUnlock;

	// Token: 0x0400102A RID: 4138
	public GameObject buySound;

	// Token: 0x0400102B RID: 4139
	public Button button;

	// Token: 0x0400102C RID: 4140
	public TMP_Text buttonText;
}
