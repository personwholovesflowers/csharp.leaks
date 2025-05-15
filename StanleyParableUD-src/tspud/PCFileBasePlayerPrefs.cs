using System;

// Token: 0x02000154 RID: 340
public class PCFileBasePlayerPrefs : IPlatformPlayerPrefs
{
	// Token: 0x060007EC RID: 2028 RVA: 0x00027838 File Offset: 0x00025A38
	public PCFileBasePlayerPrefs()
	{
		if (!this.isInitialized)
		{
			PlatformPlayerPrefsHelper.Instance.InitializePlatformPlayerPrefs(this);
			this.isInitialized = true;
			FBPP.Start(new FBPPConfig
			{
				SaveFileName = "tspud-savedata.txt",
				AutoSaveData = false,
				ScrambleSaveData = true,
				EncryptionSecret = "saRpmZ6mMgZpmcojUkvkyGEQGez9YkWoXZfJdRc9ZmmJrCzfM8JksVxQfQK8uBBs"
			});
		}
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00027893 File Offset: 0x00025A93
	public void DeleteAll()
	{
		FBPP.DeleteAll();
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x0002789A File Offset: 0x00025A9A
	public void DeleteKey(string key)
	{
		FBPP.DeleteKey(key);
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x000278A2 File Offset: 0x00025AA2
	public float GetFloat(string key)
	{
		return FBPP.GetFloat(key, 0f);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x000278AF File Offset: 0x00025AAF
	public float GetFloat(string key, float defaultValue)
	{
		return FBPP.GetFloat(key, defaultValue);
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x000278B8 File Offset: 0x00025AB8
	public int GetInt(string key)
	{
		return FBPP.GetInt(key, 0);
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x000278C1 File Offset: 0x00025AC1
	public int GetInt(string key, int defaultValue)
	{
		return FBPP.GetInt(key, defaultValue);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000278CA File Offset: 0x00025ACA
	public string GetString(string key)
	{
		return FBPP.GetString(key, "");
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x000278D7 File Offset: 0x00025AD7
	public string GetString(string key, string defaultValue)
	{
		return FBPP.GetString(key, defaultValue);
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x000278E0 File Offset: 0x00025AE0
	public bool HasKey(string key)
	{
		return FBPP.HasKey(key);
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x000278E8 File Offset: 0x00025AE8
	public void Save()
	{
		FBPP.Save();
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x000278EF File Offset: 0x00025AEF
	public void SetFloat(string key, float value)
	{
		FBPP.SetFloat(key, value);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x000278F8 File Offset: 0x00025AF8
	public void SetInt(string key, int value)
	{
		FBPP.SetInt(key, value);
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00027901 File Offset: 0x00025B01
	public void SetString(string key, string value)
	{
		FBPP.SetString(key, value);
	}

	// Token: 0x04000817 RID: 2071
	private bool isInitialized;
}
