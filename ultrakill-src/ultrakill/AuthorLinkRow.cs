using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200002A RID: 42
public class AuthorLinkRow : MonoBehaviour
{
	// Token: 0x0600010B RID: 267 RVA: 0x00006368 File Offset: 0x00004568
	public void Instantiate(string platform, string username, string displayName, Color platformColor, string targetURL, string descriptionText = "")
	{
		AuthorLinkRow authorLinkRow = Object.Instantiate<AuthorLinkRow>(this, base.transform.parent, false);
		authorLinkRow.platformName.text = platform;
		authorLinkRow.platformName.color = platformColor;
		authorLinkRow.platformUsername.text = username;
		authorLinkRow.platformDisplayName.text = displayName;
		authorLinkRow.description.text = descriptionText;
		authorLinkRow.url = targetURL;
		authorLinkRow.gameObject.SetActive(true);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x000063D8 File Offset: 0x000045D8
	public void OnClick()
	{
		Application.OpenURL(this.url);
	}

	// Token: 0x040000C2 RID: 194
	public Text platformName;

	// Token: 0x040000C3 RID: 195
	public Text platformUsername;

	// Token: 0x040000C4 RID: 196
	public Text platformDisplayName;

	// Token: 0x040000C5 RID: 197
	public Text description;

	// Token: 0x040000C6 RID: 198
	private string url;
}
