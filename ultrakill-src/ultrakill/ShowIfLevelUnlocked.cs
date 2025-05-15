using System;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class ShowIfLevelUnlocked : MonoBehaviour
{
	// Token: 0x06001705 RID: 5893 RVA: 0x000BB018 File Offset: 0x000B9218
	private void OnEnable()
	{
		RankData rank = GameProgressSaver.GetRank(this.missionNumber, true);
		GameObject[] array = this.objectsToHide;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(rank != null);
		}
	}

	// Token: 0x04002020 RID: 8224
	public int missionNumber;

	// Token: 0x04002021 RID: 8225
	public GameObject[] objectsToHide;
}
