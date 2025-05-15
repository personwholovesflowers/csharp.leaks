using System;
using TMPro;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class DifficultySelectButton : MonoBehaviour
{
	// Token: 0x06000513 RID: 1299 RVA: 0x000221C0 File Offset: 0x000203C0
	private void Start()
	{
		this.level = GameProgressSaver.GetProgress(this.difficulty);
		if (this.level > 5)
		{
			this.level -= 5;
			this.stage = 1;
			while (this.level > 10)
			{
				this.level -= 10;
				this.stage += 3;
			}
			while (this.level > 4)
			{
				this.level -= 4;
				this.stage++;
			}
		}
		base.transform.Find("Progress").GetComponent<TMP_Text>().text = this.stage.ToString() + "-" + this.level.ToString();
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00022286 File Offset: 0x00020486
	public void SetDifficulty()
	{
		MonoSingleton<PrefsManager>.Instance.SetInt("difficulty", this.difficulty);
		Debug.Log("Set Difficulty to: " + this.difficulty.ToString());
	}

	// Token: 0x04000700 RID: 1792
	public int difficulty;

	// Token: 0x04000701 RID: 1793
	private int stage;

	// Token: 0x04000702 RID: 1794
	private int level;
}
