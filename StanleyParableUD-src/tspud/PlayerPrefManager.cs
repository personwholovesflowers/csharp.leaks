using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000169 RID: 361
public class PlayerPrefManager : MonoBehaviour
{
	// Token: 0x06000868 RID: 2152 RVA: 0x00028047 File Offset: 0x00026247
	private void Awake()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor)
		{
			this.SimulateKeyExists = false;
			this.SimulateIndexMatch = false;
		}
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00028066 File Offset: 0x00026266
	public void DeleteContinueKeys()
	{
		PlatformPlayerPrefs.DeleteKey("ContinuePoint");
		PlatformPlayerPrefs.Save();
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00028077 File Offset: 0x00026277
	public void DeleteAllSinglePlaythroughKeys()
	{
		PlatformPlayerPrefs.DeleteKey("PressEnding1");
		PlatformPlayerPrefs.DeleteKey("PressEnding2");
		PlatformPlayerPrefs.Save();
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00028092 File Offset: 0x00026292
	public void DeleteKey()
	{
		PlatformPlayerPrefs.DeleteKey(this.Key);
		PlatformPlayerPrefs.Save();
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x000280A4 File Offset: 0x000262A4
	public void SetKey()
	{
		PlatformPlayerPrefs.SetInt(this.Key, 1);
		PlatformPlayerPrefs.Save();
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x000280B7 File Offset: 0x000262B7
	public void SetKeyIntValue(int value)
	{
		PlatformPlayerPrefs.SetInt(this.Key, value);
		PlatformPlayerPrefs.Save();
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x000280CC File Offset: 0x000262CC
	public void AdvanceIntValue()
	{
		if (PlatformPlayerPrefs.HasKey(this.Key))
		{
			int @int = PlatformPlayerPrefs.GetInt(this.Key);
			PlatformPlayerPrefs.SetInt(this.Key, @int + 1);
			PlatformPlayerPrefs.Save();
			return;
		}
		PlatformPlayerPrefs.SetInt(this.Key, 1);
		PlatformPlayerPrefs.Save();
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x00028117 File Offset: 0x00026317
	public void InvokeEvents()
	{
		if (this.SimulateKeyExists || PlatformPlayerPrefs.HasKey(this.Key))
		{
			this.OnPlayerPrefsHaveKey.Invoke();
			return;
		}
		this.OnPlayerPrefsDoNotHaveKey.Invoke();
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x00028148 File Offset: 0x00026348
	public void InvokeIndexEvents()
	{
		if (this.SimulateIndexMatch || (PlatformPlayerPrefs.HasKey(this.Key) && PlatformPlayerPrefs.GetInt(this.Key) == this.RelevantIndex))
		{
			this.OnIndexMatch.Invoke();
			return;
		}
		this.OnIndexDoNotMatch.Invoke();
	}

	// Token: 0x0400083D RID: 2109
	public string Key = "empty";

	// Token: 0x0400083E RID: 2110
	public UnityEvent OnPlayerPrefsHaveKey;

	// Token: 0x0400083F RID: 2111
	public UnityEvent OnPlayerPrefsDoNotHaveKey;

	// Token: 0x04000840 RID: 2112
	[Space]
	public int RelevantIndex;

	// Token: 0x04000841 RID: 2113
	public UnityEvent OnIndexMatch;

	// Token: 0x04000842 RID: 2114
	public UnityEvent OnIndexDoNotMatch;

	// Token: 0x04000843 RID: 2115
	public bool SimulateIndexMatch;

	// Token: 0x04000844 RID: 2116
	public bool SimulateKeyExists;
}
