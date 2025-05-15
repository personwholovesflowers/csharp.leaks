using System;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
public class GetPlayerTagTest : MonoBehaviour
{
	// Token: 0x06001B74 RID: 7028 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void Start()
	{
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x000E3E44 File Offset: 0x000E2044
	private void Update()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		MonoBehaviour.print(array.Length);
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			MonoBehaviour.print(array2[i].gameObject);
		}
	}
}
