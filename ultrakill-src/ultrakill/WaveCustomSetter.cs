using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000182 RID: 386
public class WaveCustomSetter : MonoBehaviour
{
	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x0600077C RID: 1916 RVA: 0x000323E6 File Offset: 0x000305E6
	// (set) Token: 0x0600077D RID: 1917 RVA: 0x000323F0 File Offset: 0x000305F0
	public int wave
	{
		get
		{
			return this._wave;
		}
		set
		{
			this._wave = value;
			this.buttonText.SetText(this.wave.ToString(), true);
			this.UpdateChangeButtons();
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x0600077E RID: 1918 RVA: 0x00032424 File Offset: 0x00030624
	// (set) Token: 0x0600077F RID: 1919 RVA: 0x0003242C File Offset: 0x0003062C
	public WaveCustomSetter.ButtonState state
	{
		get
		{
			return this._state;
		}
		set
		{
			this._state = value;
			this.button.interactable = this.state == WaveCustomSetter.ButtonState.Unselected;
			this.shopButton.deactivated = this.state == WaveCustomSetter.ButtonState.Selected;
			this.UpdateChangeButtons();
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00032464 File Offset: 0x00030664
	private void Awake()
	{
		this.button = base.GetComponent<Button>();
		this.wm = base.GetComponentInParent<WaveMenu>();
		if (base.TryGetComponent<ShopButton>(out this.shopButton))
		{
			this.shopButton.PointerClickSuccess += delegate
			{
				this.Select();
			};
		}
		this.increaseShopButton.PointerClickSuccess += delegate
		{
			this.IncreaseWave();
		};
		this.decreaseShopButton.PointerClickSuccess += delegate
		{
			this.DecreaseWave();
		};
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x000324DC File Offset: 0x000306DC
	public void IncreaseWave()
	{
		if (this.wm.highestWave < 60)
		{
			return;
		}
		if (this.wave + this.waveChangeAmount <= this.wm.highestWave / 2)
		{
			this.wave += this.waveChangeAmount;
		}
		this.Select();
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0003252E File Offset: 0x0003072E
	public void DecreaseWave()
	{
		if (this.wm.highestWave < 60)
		{
			return;
		}
		if (this.wave - this.waveChangeAmount >= 30)
		{
			this.wave -= this.waveChangeAmount;
		}
		this.Select();
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0003256C File Offset: 0x0003076C
	private void UpdateChangeButtons()
	{
		bool flag = this.wave + this.waveChangeAmount <= this.wm.highestWave / 2;
		this.increaseButton.interactable = flag;
		this.increaseShopButton.deactivated = !flag;
		this.increaseShopButton.failure = !flag;
		this.increaseArrow.color = (flag ? Color.white : Color.gray);
		bool flag2 = this.wave - this.waveChangeAmount >= 30;
		this.decreaseButton.interactable = flag2;
		this.decreaseShopButton.deactivated = !flag2;
		this.decreaseShopButton.failure = !flag2;
		this.decreaseArrow.color = (flag2 ? Color.white : Color.gray);
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00032636 File Offset: 0x00030836
	private void Select()
	{
		this.wm.SetCurrentWave(this.wave);
	}

	// Token: 0x040009B4 RID: 2484
	private int _wave;

	// Token: 0x040009B5 RID: 2485
	public int waveChangeAmount;

	// Token: 0x040009B6 RID: 2486
	private WaveMenu wm;

	// Token: 0x040009B7 RID: 2487
	private WaveCustomSetter.ButtonState _state;

	// Token: 0x040009B8 RID: 2488
	private ShopButton shopButton;

	// Token: 0x040009B9 RID: 2489
	private bool prepared;

	// Token: 0x040009BA RID: 2490
	private Button button;

	// Token: 0x040009BB RID: 2491
	[SerializeField]
	private Image buttonGraphic;

	// Token: 0x040009BC RID: 2492
	[SerializeField]
	private TMP_Text buttonText;

	// Token: 0x040009BD RID: 2493
	[SerializeField]
	private Button increaseButton;

	// Token: 0x040009BE RID: 2494
	[SerializeField]
	private Button decreaseButton;

	// Token: 0x040009BF RID: 2495
	[SerializeField]
	private Image increaseArrow;

	// Token: 0x040009C0 RID: 2496
	[SerializeField]
	private Image decreaseArrow;

	// Token: 0x040009C1 RID: 2497
	[Space]
	[SerializeField]
	private ShopButton increaseShopButton;

	// Token: 0x040009C2 RID: 2498
	[SerializeField]
	private ShopButton decreaseShopButton;

	// Token: 0x02000183 RID: 387
	public enum ButtonState
	{
		// Token: 0x040009C4 RID: 2500
		Selected,
		// Token: 0x040009C5 RID: 2501
		Unselected
	}
}
