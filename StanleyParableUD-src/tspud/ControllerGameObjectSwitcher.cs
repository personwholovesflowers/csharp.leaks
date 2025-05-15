using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class ControllerGameObjectSwitcher : MonoBehaviour
{
	// Token: 0x060003EF RID: 1007 RVA: 0x00018C7D File Offset: 0x00016E7D
	private void Awake()
	{
		Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged += this.Instance_OnInputDeviceTypeChanged;
		this.Instance_OnInputDeviceTypeChanged(Singleton<GameMaster>.Instance.InputDeviceType);
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x00018CA5 File Offset: 0x00016EA5
	private void OnDestroy()
	{
		if (Singleton<GameMaster>.Instance != null)
		{
			Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged -= this.Instance_OnInputDeviceTypeChanged;
		}
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x00018CCC File Offset: 0x00016ECC
	private void Instance_OnInputDeviceTypeChanged(GameMaster.InputDevice type)
	{
		if (base.enabled)
		{
			if (this.matchTarget)
			{
				this.matchTarget.SetActive(type == this.inputDeviceToMatch);
			}
			if (this.nonMatchTarget)
			{
				this.nonMatchTarget.SetActive(type != this.inputDeviceToMatch);
			}
		}
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x00005444 File Offset: 0x00003644
	private void Update()
	{
	}

	// Token: 0x040003E7 RID: 999
	[Header("Sets active/notactive on match, and opposite on not match")]
	public GameMaster.InputDevice inputDeviceToMatch;

	// Token: 0x040003E8 RID: 1000
	public GameObject matchTarget;

	// Token: 0x040003E9 RID: 1001
	public GameObject nonMatchTarget;
}
