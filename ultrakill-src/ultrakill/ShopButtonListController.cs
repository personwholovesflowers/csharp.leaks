using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003F3 RID: 1011
public class ShopButtonListController : MonoBehaviour
{
	// Token: 0x060016B8 RID: 5816 RVA: 0x000B63A4 File Offset: 0x000B45A4
	private void Start()
	{
		Button[] array = this.buttons;
		for (int i = 0; i < array.Length; i++)
		{
			Button button = array[i];
			button.GetComponent<ShopButton>().PointerClickSuccess += delegate
			{
				this.SetActiveButton(button);
			};
		}
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x000B63F8 File Offset: 0x000B45F8
	private void SetActiveButton(Button specButton)
	{
		foreach (Button button in this.buttons)
		{
			if (button == specButton)
			{
				button.interactable = false;
				ShopButton shopButton;
				if (button.TryGetComponent<ShopButton>(out shopButton))
				{
					shopButton.deactivated = true;
				}
			}
			else
			{
				button.interactable = true;
				ShopButton shopButton2;
				if (button.TryGetComponent<ShopButton>(out shopButton2))
				{
					shopButton2.deactivated = false;
				}
			}
		}
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x000B645A File Offset: 0x000B465A
	public void ResetButtons()
	{
		this.SetActiveButton(null);
	}

	// Token: 0x04001F7D RID: 8061
	[SerializeField]
	private bool resetOnEnable = true;

	// Token: 0x04001F7E RID: 8062
	[SerializeField]
	private Button[] buttons;
}
