using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class CustomCampaign : CustomGameContent
{
	// Token: 0x0600012E RID: 302 RVA: 0x000069B8 File Offset: 0x00004BB8
	public CustomCampaign(string path)
	{
		if (path.EndsWith("campaign.json"))
		{
			path = path.Substring(0, path.Length - "campaign.json".Length);
		}
		this.lastModified = File.GetLastWriteTime(path);
		this.path = path;
		if (!File.Exists(Path.Combine(path, "campaign.json")))
		{
			Debug.Log(path);
			Debug.LogError("Campaign is missing campaign.json");
			this.name = "<color=red>Loading Failed!</color>";
			this.levels = Array.Empty<CampaignLevel>();
			this.valid = false;
			return;
		}
		string text = File.ReadAllText(Path.Combine(path, "campaign.json"));
		Debug.Log(text);
		CampaignJson campaignJson = JsonConvert.DeserializeObject<CampaignJson>(text);
		this.json = campaignJson;
		this.name = campaignJson.name;
		this.uniqueId = campaignJson.uuid;
		this.levelCount = campaignJson.levels.Length;
		this.levels = campaignJson.levels;
		this.author = campaignJson.author;
		this.valid = true;
		campaignJson.path = path;
		if (File.Exists(Path.Combine(path, "thumbnail.png")))
		{
			byte[] array = File.ReadAllBytes(Path.Combine(path, "thumbnail.png"));
			this.thumbnail = new Texture2D(640, 480)
			{
				filterMode = FilterMode.Point
			};
			this.thumbnail.LoadImage(array);
		}
	}

	// Token: 0x040000F5 RID: 245
	public int levelCount;

	// Token: 0x040000F6 RID: 246
	public CampaignLevel[] levels;

	// Token: 0x040000F7 RID: 247
	public bool valid;

	// Token: 0x040000F8 RID: 248
	public CampaignJson json;
}
