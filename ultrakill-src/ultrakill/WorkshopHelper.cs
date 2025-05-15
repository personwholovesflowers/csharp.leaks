using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Steamworks;
using Steamworks.Ugc;
using UnityEngine;

// Token: 0x02000055 RID: 85
public static class WorkshopHelper
{
	// Token: 0x060001A9 RID: 425 RVA: 0x0000866C File Offset: 0x0000686C
	public static async Task<Item?> GetWorkshopItemInfo(ulong itemId)
	{
		return await SteamUGC.QueryFileAsync(itemId);
	}

	// Token: 0x060001AA RID: 426 RVA: 0x000086B0 File Offset: 0x000068B0
	public static async Task<Item?> DownloadWorkshopMap(ulong itemId, [CanBeNull] Action promptForUpdate = null)
	{
		Item? item = await Item.GetAsync(itemId, 1800);
		Item? item2;
		if (item == null)
		{
			Debug.LogError("Failed to get workshop item info for " + itemId.ToString());
			item2 = null;
		}
		else
		{
			Debug.Log("Title: " + ((item != null) ? item.GetValueOrDefault().Title : null));
			Debug.Log(string.Format("IsInstalled: {0}", (item != null) ? new bool?(item.GetValueOrDefault().IsInstalled) : null));
			Debug.Log(string.Format("IsDownloading: {0}", (item != null) ? new bool?(item.GetValueOrDefault().IsDownloading) : null));
			Debug.Log(string.Format("IsDownloadPending: {0}", (item != null) ? new bool?(item.GetValueOrDefault().IsDownloadPending) : null));
			Debug.Log(string.Format("IsSubscribed: {0}", (item != null) ? new bool?(item.GetValueOrDefault().IsSubscribed) : null));
			Debug.Log(string.Format("NeedsUpdate: {0}", (item != null) ? new bool?(item.GetValueOrDefault().NeedsUpdate) : null));
			Debug.Log("Description: " + ((item != null) ? item.GetValueOrDefault().Description : null));
			if (promptForUpdate != null && ((item != null) ? new bool?(item.GetValueOrDefault().NeedsUpdate) : null).Value)
			{
				promptForUpdate();
				item2 = null;
			}
			else
			{
				if (!item.Value.IsInstalled)
				{
					Debug.Log(string.Format("Downloading workshop map {0}", itemId));
					TaskAwaiter<bool> taskAwaiter = item.Value.DownloadAsync(null, 60, default(CancellationToken)).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						Debug.LogError(string.Format("Failed to download workshop map {0}", itemId));
						return null;
					}
					Debug.Log(string.Format("Workshop map {0} downloaded successfully", itemId));
					item = await Item.GetAsync(itemId, 1800);
					if (item == null)
					{
						Debug.LogError("Failed to get workshop item info for " + itemId.ToString());
						return null;
					}
				}
				else
				{
					Debug.LogWarning(string.Format("Workshop map {0} was already downloaded", itemId));
				}
				item2 = item;
			}
		}
		return item2;
	}
}
