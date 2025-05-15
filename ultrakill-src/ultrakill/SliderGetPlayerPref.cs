using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200040D RID: 1037
public class SliderGetPlayerPref : MonoBehaviour
{
	// Token: 0x060017AE RID: 6062 RVA: 0x000C254C File Offset: 0x000C074C
	private void Awake()
	{
		if (this.isFloat)
		{
			base.GetComponent<Slider>().value = PlayerPrefs.GetFloat(this.playerPref, this.defaultFloat);
			return;
		}
		base.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt(this.playerPref, this.defaultInt);
	}

	// Token: 0x04002107 RID: 8455
	public bool isFloat;

	// Token: 0x04002108 RID: 8456
	public string playerPref;

	// Token: 0x04002109 RID: 8457
	public float defaultFloat;

	// Token: 0x0400210A RID: 8458
	public int defaultInt;
}
