using System;
using UnityEngine;

// Token: 0x02000224 RID: 548
public class GearCheckEnabler : MonoBehaviour
{
	// Token: 0x06000BCE RID: 3022 RVA: 0x00052CA0 File Offset: 0x00050EA0
	private void Start()
	{
		if (GameProgressSaver.CheckGear(this.gear) > 0 && (!this.checkForFullIntro || PlayerPrefs.GetInt("FullIntro", 0) == 0))
		{
			GameObject[] array = this.toActivate;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			array = this.toDisactivate;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}
	}

	// Token: 0x04000F65 RID: 3941
	public string gear;

	// Token: 0x04000F66 RID: 3942
	public GameObject[] toActivate;

	// Token: 0x04000F67 RID: 3943
	public GameObject[] toDisactivate;

	// Token: 0x04000F68 RID: 3944
	public bool checkForFullIntro;
}
