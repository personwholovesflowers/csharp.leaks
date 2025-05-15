using System;

// Token: 0x0200015F RID: 351
public interface IPlatformPlayerPrefs
{
	// Token: 0x06000840 RID: 2112
	void DeleteAll();

	// Token: 0x06000841 RID: 2113
	void DeleteKey(string key);

	// Token: 0x06000842 RID: 2114
	float GetFloat(string key);

	// Token: 0x06000843 RID: 2115
	float GetFloat(string key, float defaultValue);

	// Token: 0x06000844 RID: 2116
	int GetInt(string key);

	// Token: 0x06000845 RID: 2117
	int GetInt(string key, int defaultValue);

	// Token: 0x06000846 RID: 2118
	string GetString(string key);

	// Token: 0x06000847 RID: 2119
	string GetString(string key, string defaultValue);

	// Token: 0x06000848 RID: 2120
	bool HasKey(string key);

	// Token: 0x06000849 RID: 2121
	void Save();

	// Token: 0x0600084A RID: 2122
	void SetFloat(string key, float value);

	// Token: 0x0600084B RID: 2123
	void SetInt(string key, int value);

	// Token: 0x0600084C RID: 2124
	void SetString(string key, string value);
}
