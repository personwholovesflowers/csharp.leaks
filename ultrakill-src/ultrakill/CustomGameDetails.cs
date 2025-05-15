using System;

// Token: 0x0200002D RID: 45
public class CustomGameDetails
{
	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000115 RID: 277 RVA: 0x000064EF File Offset: 0x000046EF
	public string campaignId
	{
		get
		{
			CampaignJson campaignJson = this.campaign;
			if (campaignJson == null)
			{
				return null;
			}
			return campaignJson.uuid;
		}
	}

	// Token: 0x040000CD RID: 205
	public string uniqueIdentifier;

	// Token: 0x040000CE RID: 206
	public int levelNumber;

	// Token: 0x040000CF RID: 207
	public ulong? workshopId;

	// Token: 0x040000D0 RID: 208
	public CampaignJson campaign;
}
