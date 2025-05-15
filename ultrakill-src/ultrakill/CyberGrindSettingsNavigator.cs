using System;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class CyberGrindSettingsNavigator : MonoBehaviour
{
	// Token: 0x06000734 RID: 1844 RVA: 0x0002EFE8 File Offset: 0x0002D1E8
	public void GoTo(GameObject panel)
	{
		foreach (GameObject gameObject in this.allPanels)
		{
			gameObject.SetActive(gameObject == panel);
		}
		this.tipPanel.SetActive(false);
		this.menu.SetActive(true);
		panel.SetActive(true);
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x0002F038 File Offset: 0x0002D238
	public void GoToNoMenu(GameObject panel)
	{
		this.GoTo(panel);
		this.menu.SetActive(false);
	}

	// Token: 0x0400093D RID: 2365
	public GameObject menu;

	// Token: 0x0400093E RID: 2366
	public GameObject tipPanel;

	// Token: 0x0400093F RID: 2367
	public GameObject[] allPanels;
}
