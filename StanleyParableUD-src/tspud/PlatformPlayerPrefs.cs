using System;

// Token: 0x02000150 RID: 336
public static class PlatformPlayerPrefs
{
	// Token: 0x060007D7 RID: 2007 RVA: 0x00027592 File Offset: 0x00025792
	public static void Init(IPlatformPlayerPrefs platformPlayerPrefs)
	{
		PlatformPlayerPrefs.playerPrefs = platformPlayerPrefs;
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0002759A File Offset: 0x0002579A
	public static void DeleteAll()
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs != null)
		{
			platformPlayerPrefs.DeleteAll();
		}
		IPlatformPlayerPrefs platformPlayerPrefs2 = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs2 == null)
		{
			return;
		}
		platformPlayerPrefs2.Save();
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x000275BB File Offset: 0x000257BB
	public static void DeleteKey(string key)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs != null)
		{
			platformPlayerPrefs.DeleteKey(key);
		}
		IPlatformPlayerPrefs platformPlayerPrefs2 = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs2 == null)
		{
			return;
		}
		platformPlayerPrefs2.Save();
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x000275DD File Offset: 0x000257DD
	public static float GetFloat(string key)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs == null)
		{
			return 0f;
		}
		return platformPlayerPrefs.GetFloat(key);
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x000275F4 File Offset: 0x000257F4
	public static float GetFloat(string key, float defaultValue)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs == null)
		{
			return defaultValue;
		}
		return platformPlayerPrefs.GetFloat(key, defaultValue);
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00027608 File Offset: 0x00025808
	public static int GetInt(string key)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs == null)
		{
			return 0;
		}
		return platformPlayerPrefs.GetInt(key);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x0002761B File Offset: 0x0002581B
	public static int GetInt(string key, int defaultValue)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs == null)
		{
			return defaultValue;
		}
		return platformPlayerPrefs.GetInt(key, defaultValue);
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x0002762F File Offset: 0x0002582F
	public static string GetString(string key)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs == null)
		{
			return null;
		}
		return platformPlayerPrefs.GetString(key);
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00027642 File Offset: 0x00025842
	public static string GetString(string key, string defaultValue)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs == null)
		{
			return null;
		}
		return platformPlayerPrefs.GetString(key, defaultValue);
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00027656 File Offset: 0x00025856
	public static bool HasKey(string key)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		return platformPlayerPrefs != null && platformPlayerPrefs.HasKey(key);
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00027669 File Offset: 0x00025869
	public static void Save()
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs == null)
		{
			return;
		}
		platformPlayerPrefs.Save();
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0002767A File Offset: 0x0002587A
	public static void SetFloat(string key, float value)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs != null)
		{
			platformPlayerPrefs.SetFloat(key, value);
		}
		IPlatformPlayerPrefs platformPlayerPrefs2 = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs2 == null)
		{
			return;
		}
		platformPlayerPrefs2.Save();
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0002769D File Offset: 0x0002589D
	public static void SetInt(string key, int value)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs != null)
		{
			platformPlayerPrefs.SetInt(key, value);
		}
		IPlatformPlayerPrefs platformPlayerPrefs2 = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs2 == null)
		{
			return;
		}
		platformPlayerPrefs2.Save();
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x000276C0 File Offset: 0x000258C0
	public static void SetString(string key, string value)
	{
		IPlatformPlayerPrefs platformPlayerPrefs = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs != null)
		{
			platformPlayerPrefs.SetString(key, value);
		}
		IPlatformPlayerPrefs platformPlayerPrefs2 = PlatformPlayerPrefs.playerPrefs;
		if (platformPlayerPrefs2 == null)
		{
			return;
		}
		platformPlayerPrefs2.Save();
	}

	// Token: 0x040007ED RID: 2029
	private static IPlatformPlayerPrefs playerPrefs;
}
