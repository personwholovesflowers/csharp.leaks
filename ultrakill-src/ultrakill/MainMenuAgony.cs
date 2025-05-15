using System;
using System.Collections;
using System.Linq;
using plog;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class MainMenuAgony : MonoBehaviour
{
	// Token: 0x0600011E RID: 286 RVA: 0x00006824 File Offset: 0x00004A24
	private void Awake()
	{
		MainMenuAgony.isAgonyOpen = false;
		GameObject[] array = this.agonyMenus;
		for (int i = 0; i < array.Length; i++)
		{
			Object.DestroyImmediate(array[i]);
		}
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void Start()
	{
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00006854 File Offset: 0x00004A54
	private IEnumerator CloseMainMenuDelayed()
	{
		yield return null;
		this.mainMenu.SetActive(false);
		yield break;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00006864 File Offset: 0x00004A64
	private void Update()
	{
		bool flag = MainMenuAgony.isAgonyOpen;
		this.normalLights.SetActive(!flag);
		this.agonyLights.SetActive(flag);
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00006892 File Offset: 0x00004A92
	public void OpenAgony(bool restore = false)
	{
		MainMenuAgony.isAgonyOpen = true;
		if (restore)
		{
			this.agonyMenus.Last<GameObject>().SetActive(true);
		}
		else
		{
			this.agonyMenus.First<GameObject>().SetActive(true);
		}
		this.mainMenu.SetActive(false);
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000068D0 File Offset: 0x00004AD0
	public void CloseAgony()
	{
		MainMenuAgony.isAgonyOpen = false;
		GameObject[] array = this.agonyMenus;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		this.mainMenu.SetActive(true);
	}

	// Token: 0x040000E4 RID: 228
	private static readonly global::plog.Logger Log = new global::plog.Logger("MainMenuAgony");

	// Token: 0x040000E5 RID: 229
	public static bool isAgonyOpen = true;

	// Token: 0x040000E6 RID: 230
	[SerializeField]
	private GameObject agonyButton;

	// Token: 0x040000E7 RID: 231
	[Space]
	[SerializeField]
	private GameObject normalLights;

	// Token: 0x040000E8 RID: 232
	[SerializeField]
	private GameObject agonyLights;

	// Token: 0x040000E9 RID: 233
	[Space]
	[SerializeField]
	private GameObject[] agonyMenus;

	// Token: 0x040000EA RID: 234
	[SerializeField]
	private GameObject mainMenu;
}
