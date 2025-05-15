using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000186 RID: 390
public class WaveSetter : MonoBehaviour
{
	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x0003291A File Offset: 0x00030B1A
	// (set) Token: 0x06000790 RID: 1936 RVA: 0x00032924 File Offset: 0x00030B24
	public ButtonState state
	{
		get
		{
			return this._state;
		}
		set
		{
			this._state = value;
			this.button.interactable = this.state == ButtonState.Unselected;
			this.shopButton.deactivated = this.state != ButtonState.Unselected;
			this.shopButton.failure = this.state == ButtonState.Locked;
			this.buttonGraphic.color = ((this.state == ButtonState.Locked) ? Color.red : Color.white);
			this.buttonText.color = ((this.state == ButtonState.Locked) ? Color.red : Color.white);
		}
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x000329B7 File Offset: 0x00030BB7
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
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x000329F6 File Offset: 0x00030BF6
	private void Select()
	{
		if (this._state == ButtonState.Locked)
		{
			return;
		}
		this.wm.SetCurrentWave(this.wave);
	}

	// Token: 0x040009CE RID: 2510
	public int wave;

	// Token: 0x040009CF RID: 2511
	private WaveMenu wm;

	// Token: 0x040009D0 RID: 2512
	private ButtonState _state;

	// Token: 0x040009D1 RID: 2513
	private ShopButton shopButton;

	// Token: 0x040009D2 RID: 2514
	private bool prepared;

	// Token: 0x040009D3 RID: 2515
	private Button button;

	// Token: 0x040009D4 RID: 2516
	[SerializeField]
	private Image buttonGraphic;

	// Token: 0x040009D5 RID: 2517
	[SerializeField]
	private TMP_Text buttonText;
}
