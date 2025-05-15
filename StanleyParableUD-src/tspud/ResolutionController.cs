using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class ResolutionController : MonoBehaviour
{
	// Token: 0x170000AB RID: 171
	// (get) Token: 0x0600089A RID: 2202 RVA: 0x000289F1 File Offset: 0x00026BF1
	// (set) Token: 0x0600089B RID: 2203 RVA: 0x000289F9 File Offset: 0x00026BF9
	public Resolution[] availableResolutions { get; private set; }

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x0600089C RID: 2204 RVA: 0x00028A02 File Offset: 0x00026C02
	// (set) Token: 0x0600089D RID: 2205 RVA: 0x00028A0A File Offset: 0x00026C0A
	public int maximumValue { get; private set; }

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x0600089E RID: 2206 RVA: 0x00028A13 File Offset: 0x00026C13
	public int SelectedResolutionIndex
	{
		get
		{
			return this.resolutionIndexConfigurable.GetIntValue();
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x0600089F RID: 2207 RVA: 0x00028A20 File Offset: 0x00026C20
	public FullScreenMode SelectedFullScreenMode
	{
		get
		{
			return ResolutionController.FullScreenModeFromIndex(this.fullscreenModeIndexConfigurable.GetIntValue());
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00028A32 File Offset: 0x00026C32
	public FullScreenMode CurrentFullScreenMode
	{
		get
		{
			return Screen.fullScreenMode;
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00028A39 File Offset: 0x00026C39
	private int CurrentFullScreenModeIndex
	{
		get
		{
			return ResolutionController.IndexFromFullScreenMode(Screen.fullScreenMode);
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00028A45 File Offset: 0x00026C45
	public Resolution CurrentResolution
	{
		get
		{
			return Screen.currentResolution;
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060008A3 RID: 2211 RVA: 0x00028A4C File Offset: 0x00026C4C
	public int CurrentResolutionIndex
	{
		get
		{
			return ResolutionController.ResolutionIndexFromResolution(this.CurrentResolution);
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00028A59 File Offset: 0x00026C59
	// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00028A60 File Offset: 0x00026C60
	public static ResolutionController Instance { get; private set; }

	// Token: 0x060008A6 RID: 2214 RVA: 0x00028A68 File Offset: 0x00026C68
	private void Awake()
	{
		ResolutionController.Instance = this;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00028A70 File Offset: 0x00026C70
	private void Start()
	{
		this.LoadFullscreenModeData();
		this.LoadResolutionData();
		int currentResolutionIndex = this.CurrentResolutionIndex;
		int selectedResolutionIndex = this.SelectedResolutionIndex;
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00028A8C File Offset: 0x00026C8C
	private void LoadResolutionData()
	{
		this.availableResolutions = Screen.resolutions;
		Debug.Log("ResolutionController::LoadResolutionData availableResolutions.Length: " + this.availableResolutions.Length);
		Debug.Log("ResolutionController::LoadResolutionData availableResolutions[] = " + string.Join("\n", new List<Resolution>(this.availableResolutions).ConvertAll<string>((Resolution x) => x.ToString())));
		this.maximumValue = this.availableResolutions.Length - 1;
		bool flag = false;
		LiveData liveData = this.resolutionIndexConfigurable.LoadOrCreateSaveData(out flag);
		this.resolutionIndexConfigurable.SetNewMinValue(0);
		this.resolutionIndexConfigurable.SetNewMaxValue(this.maximumValue);
		int num = -1;
		Debug.Log("validSaveData: " + flag.ToString());
		if (flag)
		{
			Debug.Log("Loading resolutionIndex...");
			num = liveData.IntValue;
			Debug.Log("resolutionIndex: " + num);
			if (num == -1 || num >= this.availableResolutions.Length || num < 0)
			{
				flag = false;
			}
		}
		Debug.Log("validSaveData again: " + flag.ToString());
		if (!flag)
		{
			Debug.Log("Finding resolutionIndex..., before resolutionIndex = " + num);
			num = this.CurrentResolutionIndex;
			if (num == -1)
			{
				Debug.Log("resolutionIndex was -1, setting to 0");
				num = 0;
			}
		}
		Debug.Log("resolutionIndex: " + num);
		liveData.IntValue = num;
		this.resolutionIndexConfigurable.SetNewConfiguredValue(liveData);
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00028C08 File Offset: 0x00026E08
	public void LoadFullscreenModeData()
	{
		int num = 0;
		bool flag = false;
		LiveData liveData = this.fullscreenModeIndexConfigurable.LoadOrCreateSaveData(out flag);
		if (flag)
		{
			num = liveData.IntValue;
		}
		if (!flag)
		{
			num = this.CurrentFullScreenModeIndex;
		}
		liveData.IntValue = num;
		this.fullscreenModeIndexConfigurable.SetNewConfiguredValue(liveData);
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00028C4E File Offset: 0x00026E4E
	public void ApplyResolutionAtIndicies(int i, int f)
	{
		this.ApplyResolution(this.availableResolutions[i], ResolutionController.FullScreenModeFromIndex(f));
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00028C68 File Offset: 0x00026E68
	public void ApplyResolutionAtIndex(int i, FullScreenMode fullScreenMode)
	{
		this.ApplyResolution(this.availableResolutions[i], fullScreenMode);
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x00028C7D File Offset: 0x00026E7D
	public void ApplyResolution(Resolution r, FullScreenMode fullScreenMode)
	{
		Screen.SetResolution(r.width, r.height, fullScreenMode, r.refreshRate);
		SimpleEvent simpleEvent = this.onResolutionChange;
		if (simpleEvent == null)
		{
			return;
		}
		simpleEvent.Call();
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00028CAA File Offset: 0x00026EAA
	private static int IndexFromFullScreenMode(FullScreenMode fullscreenMode)
	{
		switch (fullscreenMode)
		{
		case FullScreenMode.ExclusiveFullScreen:
			return 0;
		case FullScreenMode.FullScreenWindow:
			return 1;
		case FullScreenMode.Windowed:
			return 2;
		}
		return -1;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00028CCB File Offset: 0x00026ECB
	private static FullScreenMode FullScreenModeFromIndex(int index)
	{
		switch (index)
		{
		case 0:
			return FullScreenMode.ExclusiveFullScreen;
		case 1:
			return FullScreenMode.FullScreenWindow;
		case 2:
			return FullScreenMode.Windowed;
		default:
			return FullScreenMode.Windowed;
		}
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x00028CE8 File Offset: 0x00026EE8
	public static int ResolutionIndexFromResolution(Resolution resolution)
	{
		int num = Array.FindIndex<Resolution>(ResolutionController.Instance.availableResolutions, (Resolution x) => ResolutionController.CompareResolutions(resolution, x));
		if (num != -1)
		{
			return num;
		}
		return 0;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00028D25 File Offset: 0x00026F25
	public static bool CompareResolutions(Resolution a, Resolution b)
	{
		return a.width == b.width && a.height == b.height && Mathf.Abs(a.refreshRate - b.refreshRate) <= 1;
	}

	// Token: 0x04000868 RID: 2152
	[SerializeField]
	private IntConfigurable resolutionIndexConfigurable;

	// Token: 0x04000869 RID: 2153
	[SerializeField]
	private IntConfigurable fullscreenModeIndexConfigurable;

	// Token: 0x0400086A RID: 2154
	[SerializeField]
	private SimpleEvent onResolutionChange;

	// Token: 0x0400086C RID: 2156
	private const int minumumValue = 0;

	// Token: 0x0400086E RID: 2158
	public const bool RESOLUTION_DEBUG = true;
}
