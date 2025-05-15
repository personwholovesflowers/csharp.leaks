using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200002C RID: 44
public class CampaignViewScreen : MonoBehaviour
{
	// Token: 0x06000111 RID: 273 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void Show(CustomCampaign campaign)
	{
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void Close()
	{
	}

	// Token: 0x06000113 RID: 275 RVA: 0x000064AC File Offset: 0x000046AC
	private void ResetGrid()
	{
		for (int i = 1; i < this.grid.transform.childCount; i++)
		{
			Object.Destroy(this.grid.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x040000C9 RID: 201
	[SerializeField]
	private Text campaignTitle;

	// Token: 0x040000CA RID: 202
	[SerializeField]
	private CustomLevelButton buttonTemplate;

	// Token: 0x040000CB RID: 203
	[SerializeField]
	private Texture2D placeholderThumbnail;

	// Token: 0x040000CC RID: 204
	[SerializeField]
	private GameObject grid;
}
