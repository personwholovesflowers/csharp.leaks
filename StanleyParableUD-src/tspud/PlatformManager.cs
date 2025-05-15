using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200014F RID: 335
public class PlatformManager : MonoBehaviour
{
	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060007D0 RID: 2000 RVA: 0x00027443 File Offset: 0x00025643
	public static bool UseLowerFPSVideos
	{
		get
		{
			return PlatformManager.platformManager.UseLowerFPSVideos;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002744F File Offset: 0x0002564F
	public static PlatformManager Instance
	{
		get
		{
			if (PlatformManager.instance == null)
			{
				PlatformManager.instance = Object.FindObjectOfType<PlatformManager>();
				if (PlatformManager.instance == null)
				{
					PlatformManager.instance = new GameObject("PlatformManager").AddComponent<PlatformManager>();
				}
			}
			return PlatformManager.instance;
		}
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00027490 File Offset: 0x00025690
	public void Init()
	{
		if (!PlatformManager.isInitialized)
		{
			PlatformManager.platformManager = new PCManager();
			PlatformPlayerPrefsHelper.saveSystemInitialized = (Action)Delegate.Combine(PlatformPlayerPrefsHelper.saveSystemInitialized, new Action(this.OnSaveSystemInitialized));
			PlatformManager.platformManager.Init();
			PlatformAchievements.InitPlatformAchievements(PlatformManager.platformManager.Achievements);
			PlatformGamepad.InitPlatformGamepad(PlatformManager.platformManager.Gamepad);
			PlatformRichPresence.InitPlatformRichPresence(PlatformManager.platformManager.RichPresence);
			PlatformManager.isInitialized = true;
			Object.DontDestroyOnLoad(PlatformManager.instance.gameObject);
		}
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0002751A File Offset: 0x0002571A
	private void Awake()
	{
		if (PlatformManager.instance != null && PlatformManager.instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		PlatformManager.instance = this;
		this.Init();
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0002754E File Offset: 0x0002574E
	private void OnSaveSystemInitialized()
	{
		UnityEvent unityEvent = this.saveSystemInitializedEvent;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		PlatformPlayerPrefsHelper.saveSystemInitialized = (Action)Delegate.Remove(PlatformPlayerPrefsHelper.saveSystemInitialized, new Action(this.OnSaveSystemInitialized));
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00027581 File Offset: 0x00025781
	private void Update()
	{
		IPlatformManager platformManager = PlatformManager.platformManager;
		if (platformManager == null)
		{
			return;
		}
		platformManager.PlatformManagerUpdate();
	}

	// Token: 0x040007E8 RID: 2024
	public UnityEvent saveSystemInitializedEvent;

	// Token: 0x040007E9 RID: 2025
	public static bool UseLowEndConfiguration;

	// Token: 0x040007EA RID: 2026
	private static IPlatformManager platformManager;

	// Token: 0x040007EB RID: 2027
	private static PlatformManager instance;

	// Token: 0x040007EC RID: 2028
	private static bool isInitialized;
}
