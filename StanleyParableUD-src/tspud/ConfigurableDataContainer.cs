using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000030 RID: 48
[Serializable]
public class ConfigurableDataContainer
{
	// Token: 0x060000FB RID: 251 RVA: 0x0000898E File Offset: 0x00006B8E
	public static ConfigurableDataContainer LoadContainer(string fileName)
	{
		if (PlatformPlayerPrefs.HasKey(fileName))
		{
			return JsonUtility.FromJson<ConfigurableDataContainer>(PlatformPlayerPrefs.GetString(fileName));
		}
		return new ConfigurableDataContainer();
	}

	// Token: 0x060000FC RID: 252 RVA: 0x000089AC File Offset: 0x00006BAC
	public LiveData GetSavedDataFromContainer(string key)
	{
		for (int i = 0; i < this.saveDataCache.Count; i++)
		{
			if (this.saveDataCache[i].key.Equals(key))
			{
				return this.saveDataCache[i];
			}
		}
		return null;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000089F8 File Offset: 0x00006BF8
	public void UpdateSaveDataCache(LiveData data)
	{
		bool flag = true;
		for (int i = 0; i < this.saveDataCache.Count; i++)
		{
			if (data.key.Equals(this.saveDataCache[i].key))
			{
				this.saveDataCache[i] = data;
				flag = false;
				break;
			}
		}
		if (flag)
		{
			this.saveDataCache.Add(data);
		}
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00008A5C File Offset: 0x00006C5C
	public void DeleteEntry(string key)
	{
		for (int i = 0; i < this.saveDataCache.Count; i++)
		{
			if (this.saveDataCache[i].key.Equals(key))
			{
				this.saveDataCache.RemoveAt(i);
			}
		}
		this.SaveToPlatformPrefs("data", true);
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00008AB0 File Offset: 0x00006CB0
	public void SaveToPlatformPrefs(string fileName, bool forceSave = false)
	{
		if (ConfigurableDataContainer.OnSaveValues != null)
		{
			ConfigurableDataContainer.OnSaveValues();
		}
		if (forceSave || Configurable.Dirty)
		{
			string text = JsonUtility.ToJson(this, true);
			PlatformPlayerPrefs.SetString(fileName, text);
			PlatformPlayerPrefs.Save();
			Configurable.Dirty = false;
		}
	}

	// Token: 0x0400016E RID: 366
	public static Action OnSaveValues;

	// Token: 0x0400016F RID: 367
	public static Action OnResetValues;

	// Token: 0x04000170 RID: 368
	public List<LiveData> saveDataCache = new List<LiveData>();
}
