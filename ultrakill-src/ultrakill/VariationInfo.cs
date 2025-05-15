using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000498 RID: 1176
public class VariationInfo : MonoBehaviour
{
	// Token: 0x06001B19 RID: 6937 RVA: 0x000E1648 File Offset: 0x000DF848
	private void Start()
	{
		this.player = MonoSingleton<NewMovement>.Instance.gameObject;
		this.buttonText = this.buyButton.GetComponentInChildren<TMP_Text>();
		this.buyButton.variationInfo = this;
		if (GameProgressSaver.CheckGear(this.weaponName) > 0)
		{
			this.alreadyOwned = true;
		}
		this.UpdateMoney();
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x000E169D File Offset: 0x000DF89D
	private void OnEnable()
	{
		this.UpdateMoney();
	}

	// Token: 0x06001B1B RID: 6939 RVA: 0x000E16A8 File Offset: 0x000DF8A8
	public void UpdateMoney()
	{
		this.money = GameProgressSaver.GetMoney();
		MoneyText[] array = Object.FindObjectsOfType<MoneyText>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateMoney();
		}
		if (!this.alreadyOwned && this.cost < 0 && GameProgressSaver.CheckGear(this.weaponName) > 0)
		{
			this.alreadyOwned = true;
		}
		if (!this.alreadyOwned)
		{
			if (this.cost < 0)
			{
				this.costText.text = "<color=red>Unavailable</color>";
				if (this.buttonText == null)
				{
					this.buttonText = this.buyButton.GetComponentInChildren<TMP_Text>();
				}
				this.buttonText.text = this.costText.text;
				this.buyButton.failure = true;
				this.buyButton.GetComponent<Button>().interactable = false;
				this.buyButton.GetComponent<Image>().color = Color.red;
				ShopButton shopButton;
				if (base.TryGetComponent<ShopButton>(out shopButton))
				{
					shopButton.failure = true;
				}
			}
			else if (this.cost > this.money)
			{
				this.costText.text = "<color=red>" + MoneyText.DivideMoney(this.cost) + " P</color>";
				if (this.buttonText == null)
				{
					this.buttonText = this.buyButton.GetComponentInChildren<TMP_Text>();
				}
				this.buttonText.text = this.costText.text;
				this.buyButton.failure = true;
				this.buyButton.GetComponent<Button>().interactable = false;
				this.buyButton.GetComponent<Image>().color = Color.red;
			}
			else
			{
				this.costText.text = MoneyText.DivideMoney(this.cost) + " <color=#FF4343>P</color>";
				if (this.buttonText == null)
				{
					this.buttonText = this.buyButton.GetComponentInChildren<TMP_Text>();
				}
				this.buttonText.text = this.costText.text;
				this.buyButton.failure = false;
				this.buyButton.GetComponent<Button>().interactable = true;
				this.buyButton.GetComponent<Image>().color = Color.white;
			}
			this.equipButtons.SetActive(false);
			return;
		}
		this.costText.text = "Already Owned";
		if (this.buttonText == null)
		{
			this.buttonText = this.buyButton.GetComponentInChildren<TMP_Text>();
		}
		this.buttonText.text = this.costText.text;
		this.buyButton.failure = true;
		this.buyButton.GetComponent<Button>().interactable = false;
		this.buyButton.GetComponent<Image>().color = Color.white;
		this.equipButtons.SetActive(true);
		this.equipStatusButton.interactable = true;
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("weapon." + this.weaponName, 1);
		if (@int == 2 && GameProgressSaver.CheckGear(this.weaponName.Substring(0, this.weaponName.Length - 1) + "alt") > 0)
		{
			this.equipStatus = 2;
		}
		else if (@int > 0)
		{
			this.equipStatus = 1;
		}
		else
		{
			this.equipStatus = 0;
		}
		if (this.orderButtons)
		{
			if (this.equipStatus != 0)
			{
				this.orderButtons.SetActive(true);
				this.icon.rectTransform.anchoredPosition = new Vector2(25f, 0f);
				this.icon.rectTransform.sizeDelta = new Vector2(75f, 75f);
			}
			else
			{
				this.orderButtons.SetActive(false);
				this.icon.rectTransform.anchoredPosition = new Vector2(0f, 0f);
				this.icon.rectTransform.sizeDelta = new Vector2(100f, 100f);
			}
		}
		this.SetEquipStatusText(this.equipStatus);
		ShopButton shopButton2;
		if (this.cost < 0 && base.TryGetComponent<ShopButton>(out shopButton2))
		{
			shopButton2.failure = false;
		}
	}

	// Token: 0x06001B1C RID: 6940 RVA: 0x000E1AAC File Offset: 0x000DFCAC
	public void WeaponBought()
	{
		this.alreadyOwned = true;
		Object.Instantiate<GameObject>(this.buySound);
		GameProgressSaver.AddMoney(this.cost * -1);
		GameProgressSaver.AddGear(this.weaponName);
		MonoSingleton<PrefsManager>.Instance.SetInt("weapon." + this.weaponName, 1);
		this.UpdateMoney();
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
		if (PlayerPrefs.GetInt("FirVar", 1) == 1)
		{
			base.GetComponentInParent<ShopZone>().firstVariationBuy = true;
		}
		if (this.gs == null)
		{
			this.gs = this.player.GetComponentInChildren<GunSetter>();
		}
		this.gs.ResetWeapons(false);
		this.gs.ForceWeapon(this.weaponName);
		this.gs.gunc.NoWeapon();
		if (this.fc == null)
		{
			this.fc = this.player.GetComponentInChildren<FistControl>();
		}
		this.fc.ResetFists();
		this.drawer.Play("Open", 0, 0f);
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x000E1BE4 File Offset: 0x000DFDE4
	public void ChangeEquipment(int value)
	{
		int num = this.equipStatus;
		if (value > 0)
		{
			num++;
		}
		else
		{
			num--;
		}
		int num2 = num;
		if (num < 0)
		{
			if (GameProgressSaver.CheckGear(this.weaponName.Substring(0, this.weaponName.Length - 1) + "alt") > 0)
			{
				num2 = 2;
			}
			else
			{
				num2 = 1;
			}
		}
		else if (num == 2)
		{
			if (GameProgressSaver.CheckGear(this.weaponName.Substring(0, this.weaponName.Length - 1) + "alt") > 0)
			{
				num2 = 2;
			}
			else
			{
				num2 = 0;
			}
		}
		else if (num > 2)
		{
			num2 = 0;
		}
		this.equipStatus = num2;
		this.SetEquipStatusText(this.equipStatus);
		MonoSingleton<PrefsManager>.Instance.SetInt("weapon." + this.weaponName, num2);
		if (this.orderButtons)
		{
			if (this.equipStatus != 0)
			{
				this.orderButtons.SetActive(true);
				this.icon.rectTransform.anchoredPosition = new Vector2(25f, 0f);
				this.icon.rectTransform.sizeDelta = new Vector2(75f, 75f);
			}
			else
			{
				this.orderButtons.SetActive(false);
				this.icon.rectTransform.anchoredPosition = new Vector2(0f, 0f);
				this.icon.rectTransform.sizeDelta = new Vector2(100f, 100f);
			}
		}
		if (this.gs == null)
		{
			this.gs = this.player.GetComponentInChildren<GunSetter>();
		}
		this.gs.ResetWeapons(false);
		if (this.fc == null)
		{
			this.fc = this.player.GetComponentInChildren<FistControl>();
		}
		this.fc.ResetFists();
		this.drawer.Play("Open", 0, 0f);
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x000E1DC4 File Offset: 0x000DFFC4
	private void SetEquipStatusText(int equipStatus)
	{
		switch (equipStatus)
		{
		case 0:
			this.equipText.SetText("Unequipped", true);
			this.equipText.color = Color.gray;
			return;
		case 1:
			this.equipText.SetText("Equipped", true);
			this.equipText.color = Color.white;
			return;
		case 2:
			this.equipText.SetText("Alternate", true);
			this.equipText.color = new Color(1f, 0.3f, 0.3f);
			return;
		default:
			return;
		}
	}

	// Token: 0x0400263C RID: 9788
	public GameObject varPage;

	// Token: 0x0400263D RID: 9789
	private int money;

	// Token: 0x0400263E RID: 9790
	public int cost;

	// Token: 0x0400263F RID: 9791
	public TMP_Text costText;

	// Token: 0x04002640 RID: 9792
	public ShopButton buyButton;

	// Token: 0x04002641 RID: 9793
	private TMP_Text buttonText;

	// Token: 0x04002642 RID: 9794
	public GameObject buySound;

	// Token: 0x04002643 RID: 9795
	public Image icon;

	// Token: 0x04002644 RID: 9796
	public TMP_Text equipText;

	// Token: 0x04002645 RID: 9797
	public GameObject equipButtons;

	// Token: 0x04002646 RID: 9798
	private int equipStatus;

	// Token: 0x04002647 RID: 9799
	public Button equipStatusButton;

	// Token: 0x04002648 RID: 9800
	public bool alreadyOwned;

	// Token: 0x04002649 RID: 9801
	public string weaponName;

	// Token: 0x0400264A RID: 9802
	private GunSetter gs;

	// Token: 0x0400264B RID: 9803
	private FistControl fc;

	// Token: 0x0400264C RID: 9804
	private GameObject player;

	// Token: 0x0400264D RID: 9805
	public GameObject orderButtons;

	// Token: 0x0400264E RID: 9806
	[SerializeField]
	private Animator drawer;
}
