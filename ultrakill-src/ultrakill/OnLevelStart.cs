using System;
using UnityEngine;

// Token: 0x02000322 RID: 802
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class OnLevelStart : MonoSingleton<OnLevelStart>
{
	// Token: 0x0600127C RID: 4732 RVA: 0x00094288 File Offset: 0x00092488
	protected override void Awake()
	{
		base.Awake();
		if (this.hideFogUntilStart && RenderSettings.fog)
		{
			RenderSettings.fog = false;
			this.fogHidden = true;
		}
		base.transform.parent = null;
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x000942B8 File Offset: 0x000924B8
	public void StartLevel(bool startTimer = true, bool startMusic = true)
	{
		if (this.activated)
		{
			return;
		}
		this.activated = true;
		MonoSingleton<PlayerTracker>.Instance.LevelStart();
		this.onStart.Invoke("");
		OutdoorLightMaster instance = MonoSingleton<OutdoorLightMaster>.Instance;
		if (instance != null)
		{
			instance.FirstDoorOpen();
		}
		MonoSingleton<StatsManager>.Instance.levelStarted = true;
		if (startTimer)
		{
			MonoSingleton<StatsManager>.Instance.StartTimer();
		}
		if (startMusic)
		{
			MonoSingleton<MusicManager>.Instance.StartMusic();
		}
		if (this.fogHidden)
		{
			this.fogHidden = false;
			RenderSettings.fog = true;
		}
		if (this.levelNameOnStart)
		{
			MonoSingleton<LevelNamePopup>.Instance.NameAppearDelayed(1f);
		}
		DisableOnLevelStart[] array = Object.FindObjectsOfType<DisableOnLevelStart>(true);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x0400196E RID: 6510
	public UltrakillEvent onStart;

	// Token: 0x0400196F RID: 6511
	private bool activated;

	// Token: 0x04001970 RID: 6512
	public bool hideFogUntilStart = true;

	// Token: 0x04001971 RID: 6513
	private bool fogHidden;

	// Token: 0x04001972 RID: 6514
	public bool levelNameOnStart = true;
}
