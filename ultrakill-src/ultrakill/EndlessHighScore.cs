using System;
using TMPro;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class EndlessHighScore : MonoBehaviour
{
	// Token: 0x0600076A RID: 1898 RVA: 0x00031C84 File Offset: 0x0002FE84
	private void OnEnable()
	{
		if (!this.text)
		{
			this.text = base.GetComponent<TMP_Text>();
		}
		if (!this.text)
		{
			return;
		}
		int num = (int)Mathf.Floor(GameProgressSaver.GetBestCyber().preciseWavesByDifficulty[MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 2)]);
		if (num <= 0)
		{
			this.text.text = "";
			return;
		}
		this.text.text = num.ToString() ?? "";
	}

	// Token: 0x04000999 RID: 2457
	private TMP_Text text;
}
