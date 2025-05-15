using System;
using UnityEngine;

// Token: 0x02000342 RID: 834
public class PlayerActivatorRelay : MonoSingleton<PlayerActivatorRelay>
{
	// Token: 0x06001335 RID: 4917 RVA: 0x0009AE13 File Offset: 0x00099013
	private void Start()
	{
		GameStateManager.Instance.RegisterState(new GameState("pit-falling", base.gameObject)
		{
			cameraInputLock = LockMode.Lock,
			cursorLock = LockMode.Lock
		});
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x0009AE40 File Offset: 0x00099040
	public void Activate()
	{
		if (this.index >= this.toActivate.Length)
		{
			return;
		}
		if (this.toActivate[this.index] == this.gunPanel)
		{
			if (!MonoSingleton<GunControl>.Instance.noWeapons && MonoSingleton<PrefsManager>.Instance.GetBool("weaponIcons", false) && !MapInfoBase.Instance.hideStockHUD)
			{
				this.gunPanel.SetActive(true);
			}
		}
		else if (this.toActivate[this.index] == this.crosshair)
		{
			this.crosshair.SetActive(true);
		}
		else if (!MapInfoBase.Instance.hideStockHUD)
		{
			this.toActivate[this.index].SetActive(true);
		}
		this.index++;
		if (this.index < this.toActivate.Length)
		{
			base.Invoke("Activate", this.delay);
		}
	}

	// Token: 0x04001A8D RID: 6797
	[SerializeField]
	private GameObject[] toActivate;

	// Token: 0x04001A8E RID: 6798
	[SerializeField]
	private GameObject gunPanel;

	// Token: 0x04001A8F RID: 6799
	[SerializeField]
	private GameObject crosshair;

	// Token: 0x04001A90 RID: 6800
	[SerializeField]
	private float delay = 0.2f;

	// Token: 0x04001A91 RID: 6801
	private int index;
}
