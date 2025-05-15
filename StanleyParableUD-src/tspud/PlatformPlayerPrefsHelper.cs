using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class PlatformPlayerPrefsHelper : MonoBehaviour
{
	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000856 RID: 2134 RVA: 0x00027D18 File Offset: 0x00025F18
	public static PlatformPlayerPrefsHelper Instance
	{
		get
		{
			if (PlatformPlayerPrefsHelper.instance == null)
			{
				PlatformPlayerPrefsHelper.instance = Object.FindObjectOfType<PlatformPlayerPrefsHelper>();
				if (PlatformPlayerPrefsHelper.instance == null)
				{
					PlatformPlayerPrefsHelper.instance = new GameObject("PlatformPlayerPrefsHelper").AddComponent<PlatformPlayerPrefsHelper>();
				}
				Object.DontDestroyOnLoad(PlatformPlayerPrefsHelper.instance.gameObject);
			}
			return PlatformPlayerPrefsHelper.instance;
		}
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00027D71 File Offset: 0x00025F71
	public void InitializePlatformPlayerPrefs(IPlatformPlayerPrefs playerPrefs)
	{
		if (!PlatformPlayerPrefsHelper.isInitialized)
		{
			PlatformPlayerPrefs.Init(playerPrefs);
			PlatformPlayerPrefsHelper.isInitialized = true;
			this.safeToSave = true;
			Action action = PlatformPlayerPrefsHelper.saveSystemInitialized;
			if (action == null)
			{
				return;
			}
			action();
		}
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00027D9C File Offset: 0x00025F9C
	public void StartTimer(int secondsToWait)
	{
		this.safeToSave = false;
		this.needToSave = false;
		base.StartCoroutine(this.SaveTimeDelay(secondsToWait));
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00027DBA File Offset: 0x00025FBA
	public IEnumerator SaveTimeDelay(int secondsToWait)
	{
		yield return new WaitForSecondsRealtime((float)secondsToWait);
		this.safeToSave = true;
		yield break;
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00027DD0 File Offset: 0x00025FD0
	public bool CanSave()
	{
		if (!this.safeToSave)
		{
			this.needToSave = true;
			return false;
		}
		return true;
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00027DE4 File Offset: 0x00025FE4
	private void Update()
	{
		if (this.safeToSave && this.needToSave)
		{
			PlatformPlayerPrefs.Save();
		}
	}

	// Token: 0x0400082E RID: 2094
	private static PlatformPlayerPrefsHelper instance;

	// Token: 0x0400082F RID: 2095
	private static bool isInitialized;

	// Token: 0x04000830 RID: 2096
	private bool safeToSave;

	// Token: 0x04000831 RID: 2097
	private bool needToSave;

	// Token: 0x04000832 RID: 2098
	public static Action saveSystemInitialized;
}
