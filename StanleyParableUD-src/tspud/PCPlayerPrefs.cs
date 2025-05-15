using System;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class PCPlayerPrefs : IPlatformPlayerPrefs
{
	// Token: 0x06000819 RID: 2073 RVA: 0x00027C69 File Offset: 0x00025E69
	public PCPlayerPrefs()
	{
		if (!this.isInitialized)
		{
			PlatformPlayerPrefsHelper.Instance.InitializePlatformPlayerPrefs(this);
			this.isInitialized = true;
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00027C8B File Offset: 0x00025E8B
	public void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00027C92 File Offset: 0x00025E92
	public void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00027C9A File Offset: 0x00025E9A
	public float GetFloat(string key)
	{
		return PlayerPrefs.GetFloat(key);
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00027CA2 File Offset: 0x00025EA2
	public float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00027CAB File Offset: 0x00025EAB
	public int GetInt(string key)
	{
		return PlayerPrefs.GetInt(key);
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00027CB3 File Offset: 0x00025EB3
	public int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00027CBC File Offset: 0x00025EBC
	public string GetString(string key)
	{
		return PlayerPrefs.GetString(key);
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00027CC4 File Offset: 0x00025EC4
	public string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00027CCD File Offset: 0x00025ECD
	public bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00027CD5 File Offset: 0x00025ED5
	public void Save()
	{
		PlayerPrefs.Save();
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x00027CDC File Offset: 0x00025EDC
	public void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x00027CE5 File Offset: 0x00025EE5
	public void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00027CEE File Offset: 0x00025EEE
	public void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	// Token: 0x04000825 RID: 2085
	private bool isInitialized;
}
