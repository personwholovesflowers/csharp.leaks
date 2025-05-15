using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class MenuFakePauseHotkey : MonoBehaviour
{
	// Token: 0x06000736 RID: 1846 RVA: 0x00005444 File Offset: 0x00003644
	private void Start()
	{
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00025927 File Offset: 0x00023B27
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			MonoBehaviour.print("fake pause!");
			this.enableThis.SetActive(true);
			this.disableThis.SetActive(false);
		}
	}

	// Token: 0x0400075E RID: 1886
	public GameObject enableThis;

	// Token: 0x0400075F RID: 1887
	public GameObject disableThis;
}
