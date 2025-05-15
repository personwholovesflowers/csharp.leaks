using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003F7 RID: 1015
public class ShopMother : MonoBehaviour
{
	// Token: 0x060016C2 RID: 5826 RVA: 0x000B64F0 File Offset: 0x000B46F0
	private void Start()
	{
		foreach (ShopZone shopZone in Object.FindObjectsOfType<ShopZone>())
		{
			if (shopZone.gameObject != base.gameObject)
			{
				this.shop = shopZone;
			}
		}
		if (this.shop != null)
		{
			this.origDailyTip = this.shop.transform.GetChild(1).GetChild(4).GetChild(0)
				.GetChild(0)
				.GetChild(1)
				.GetComponent<Text>();
			this.dailyTip.text = this.origDailyTip.text;
		}
		this.menu = base.transform.GetChild(0).gameObject;
		this.cc = MonoSingleton<CameraController>.Instance;
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x000B65A9 File Offset: 0x000B47A9
	private void Update()
	{
		if (MonoSingleton<InputManager>.Instance.InputSource.Pause.WasPerformedThisFrame && this.menu.activeSelf)
		{
			this.TurnOff();
		}
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x000B65D4 File Offset: 0x000B47D4
	public void TurnOn()
	{
		if (!this.menu.activeSelf)
		{
			this.menu.SetActive(true);
			this.cc.enabled = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x000B6607 File Offset: 0x000B4807
	public void TurnOff()
	{
		if (this.menu.activeSelf)
		{
			this.menu.SetActive(false);
			this.cc.enabled = true;
		}
	}

	// Token: 0x04001F83 RID: 8067
	private ShopZone shop;

	// Token: 0x04001F84 RID: 8068
	public Text dailyTip;

	// Token: 0x04001F85 RID: 8069
	private Text origDailyTip;

	// Token: 0x04001F86 RID: 8070
	private GameObject menu;

	// Token: 0x04001F87 RID: 8071
	private CameraController cc;
}
