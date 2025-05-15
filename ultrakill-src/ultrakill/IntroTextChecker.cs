using System;
using UnityEngine;

// Token: 0x02000283 RID: 643
public class IntroTextChecker : MonoBehaviour
{
	// Token: 0x06000E4B RID: 3659 RVA: 0x0006A4B1 File Offset: 0x000686B1
	private void Awake()
	{
		if (GameProgressSaver.GetIntro())
		{
			this.secondTime.SetActive(true);
		}
		else
		{
			this.firstTime.SetActive(true);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x040012BB RID: 4795
	public GameObject firstTime;

	// Token: 0x040012BC RID: 4796
	public GameObject secondTime;
}
