using System;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000058 RID: 88
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class WorkshopMapEndLinks : MonoSingleton<WorkshopMapEndLinks>
{
	// Token: 0x060001AF RID: 431 RVA: 0x00008C5C File Offset: 0x00006E5C
	public void Show()
	{
		AdditionalMapDetails instance = MonoSingleton<AdditionalMapDetails>.Instance;
		bool flag = instance && instance.hasAuthorLinks;
		this.container.SetActive(flag);
		if (flag)
		{
			foreach (AuthorLink authorLink in instance.authorLinks)
			{
				this.baseRow.Instantiate(authorLink.platform.ToString(), authorLink.username, authorLink.displayName, WorkshopMapEndLinks.GetColor(authorLink.platform), WorkshopMapEndLinks.GetLink(authorLink.platform, authorLink.username), authorLink.description);
			}
			GameStateManager.Instance.RegisterState(new GameState("workshop-map-credits", this.container)
			{
				cursorLock = LockMode.Unlock
			});
		}
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00008D20 File Offset: 0x00006F20
	public static string GetLink(LinkPlatform platform, string username)
	{
		username = username.Replace(" ", "");
		username = username.Replace(".", "");
		username = Regex.Replace(username, "[^a-zA-Z0-9_]", "");
		switch (platform)
		{
		case LinkPlatform.YouTube:
			if (!username.StartsWith("@"))
			{
				username = "@" + username;
			}
			return "https://www.youtube.com/" + username;
		case LinkPlatform.Twitter:
			return "https://twitter.com/" + username;
		case LinkPlatform.Twitch:
			return "https://twitch.tv/" + username;
		case LinkPlatform.Steam:
			return "https://steamcommunity.com/id/" + username;
		case LinkPlatform.SoundCloud:
			return "https://soundcloud.com/" + username;
		case LinkPlatform.Bandcamp:
			return "https://" + username + ".bandcamp.com";
		case LinkPlatform.KoFi:
			return "https://ko-fi.com/" + username;
		case LinkPlatform.Patreon:
			return "https://www.patreon.com/" + username;
		case LinkPlatform.PayPalMe:
			return "https://www.paypal.me/" + username;
		default:
			return string.Empty;
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00008E24 File Offset: 0x00007024
	public static Color GetColor(LinkPlatform platform)
	{
		switch (platform)
		{
		case LinkPlatform.YouTube:
			return new Color(1f, 0.4392157f, 0.4392157f);
		case LinkPlatform.Twitter:
			return new Color(0.627451f, 0.85490197f, 1f);
		case LinkPlatform.Twitch:
			return new Color(0.5647059f, 0.34901962f, 0.9647059f);
		case LinkPlatform.Steam:
			return new Color(0.6039216f, 0.64705884f, 0.81960785f);
		case LinkPlatform.SoundCloud:
			return new Color(1f, 0.7f, 0.47f);
		case LinkPlatform.Bandcamp:
			return new Color(0.6f, 0.97f, 1f);
		case LinkPlatform.KoFi:
			return new Color(0.65f, 0.95f, 1f);
		case LinkPlatform.Patreon:
			return new Color(1f, 0.6f, 0.44f);
		case LinkPlatform.PayPalMe:
			return new Color(0.63f, 0.77f, 1f);
		default:
			return Color.white;
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00008F22 File Offset: 0x00007122
	public void Continue()
	{
		MonoSingleton<AdditionalMapDetails>.Instance.hasAuthorLinks = false;
		this.container.SetActive(false);
		MonoSingleton<FinalRank>.Instance.LevelChange(false);
	}

	// Token: 0x040001C1 RID: 449
	[SerializeField]
	private AuthorLinkRow baseRow;

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	private GameObject container;
}
