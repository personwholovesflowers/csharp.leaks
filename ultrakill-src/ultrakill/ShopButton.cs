using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003F2 RID: 1010
public class ShopButton : MonoBehaviour
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060016AF RID: 5807 RVA: 0x000B6108 File Offset: 0x000B4308
	// (remove) Token: 0x060016B0 RID: 5808 RVA: 0x000B6140 File Offset: 0x000B4340
	public event Action PointerClickSuccess;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x060016B1 RID: 5809 RVA: 0x000B6178 File Offset: 0x000B4378
	// (remove) Token: 0x060016B2 RID: 5810 RVA: 0x000B61B0 File Offset: 0x000B43B0
	public event Action PointerClickFailure;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060016B3 RID: 5811 RVA: 0x000B61E8 File Offset: 0x000B43E8
	// (remove) Token: 0x060016B4 RID: 5812 RVA: 0x000B6220 File Offset: 0x000B4420
	public event Action PointerClickDeactivated;

	// Token: 0x060016B5 RID: 5813 RVA: 0x000B6255 File Offset: 0x000B4455
	private void Awake()
	{
		if (!base.TryGetComponent<ControllerPointer>(out this.pointer))
		{
			this.pointer = base.gameObject.AddComponent<ControllerPointer>();
		}
		this.pointer.OnPressed.AddListener(new UnityAction(this.OnPointerClick));
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x000B6294 File Offset: 0x000B4494
	private void OnPointerClick()
	{
		if (this.deactivated)
		{
			Action pointerClickDeactivated = this.PointerClickDeactivated;
			if (pointerClickDeactivated == null)
			{
				return;
			}
			pointerClickDeactivated();
			return;
		}
		else
		{
			if (this.failure)
			{
				if (this.failure && this.failSound != null)
				{
					Object.Instantiate<GameObject>(this.failSound, base.transform.position, Quaternion.identity);
					Action pointerClickFailure = this.PointerClickFailure;
					if (pointerClickFailure == null)
					{
						return;
					}
					pointerClickFailure();
				}
				return;
			}
			Debug.Log("OnPointerClick passed");
			GameObject[] array = this.toActivate;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			array = this.toDeactivate;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			if (this.variationInfo != null)
			{
				this.variationInfo.WeaponBought();
			}
			if (this.clickSound != null)
			{
				Object.Instantiate<GameObject>(this.clickSound, base.transform.position, Quaternion.identity);
			}
			Action pointerClickSuccess = this.PointerClickSuccess;
			if (pointerClickSuccess == null)
			{
				return;
			}
			pointerClickSuccess();
			return;
		}
	}

	// Token: 0x04001F72 RID: 8050
	public bool deactivated;

	// Token: 0x04001F73 RID: 8051
	public bool failure;

	// Token: 0x04001F74 RID: 8052
	public GameObject clickSound;

	// Token: 0x04001F75 RID: 8053
	public GameObject failSound;

	// Token: 0x04001F76 RID: 8054
	public GameObject[] toActivate;

	// Token: 0x04001F77 RID: 8055
	public GameObject[] toDeactivate;

	// Token: 0x04001F78 RID: 8056
	public VariationInfo variationInfo;

	// Token: 0x04001F79 RID: 8057
	private ControllerPointer pointer;
}
