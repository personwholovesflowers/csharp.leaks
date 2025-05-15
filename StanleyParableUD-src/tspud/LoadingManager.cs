using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000FE RID: 254
public class LoadingManager : MonoBehaviour
{
	// Token: 0x0600061A RID: 1562 RVA: 0x00021B08 File Offset: 0x0001FD08
	private void Awake()
	{
		GameMaster.OnUpdateLoadProgress = (Action<float>)Delegate.Combine(GameMaster.OnUpdateLoadProgress, new Action<float>(this.OnUpdateLoadProgress));
		AssetBundleControl.OnUpdateLoadProgress = (Action<float>)Delegate.Combine(AssetBundleControl.OnUpdateLoadProgress, new Action<float>(this.OnUpdateLoadProgress));
		string text = GameMaster.LoadingScreenMessage;
		this.standardVersion.enabled = false;
		this.blackVersion.enabled = false;
		this.blueVersion.enabled = false;
		this.doneyVersion.enabled = false;
		this.whiteVersion.enabled = false;
		this.minimalVersion.enabled = false;
		switch (GameMaster.LoadingScreenStyle)
		{
		case LoadingManager.LoadScreenStyle.Standard:
			this.standardVersion.enabled = true;
			break;
		case LoadingManager.LoadScreenStyle.Message:
			this.standardVersion.enabled = true;
			break;
		case LoadingManager.LoadScreenStyle.Black:
			this.blackVersion.enabled = true;
			break;
		case LoadingManager.LoadScreenStyle.Blue:
			this.blueVersion.enabled = true;
			break;
		case LoadingManager.LoadScreenStyle.DoneyWithTheFunny:
			this.doneyVersion.enabled = true;
			break;
		case LoadingManager.LoadScreenStyle.White:
			this.whiteVersion.enabled = true;
			break;
		case LoadingManager.LoadScreenStyle.Minimal:
			this.minimalVersion.enabled = true;
			break;
		}
		for (int i = 0; i < 4; i++)
		{
			text = text + " " + text;
		}
		this.leftText.text = text;
		this.rightText.text = GameMaster.LoadingScreenMessage;
		this.loadingBar.localScale = new Vector3(0f, 1f, 1f);
		this.minimalBar.fillAmount = 0f;
		if (LoadingManager.OnLoadingScreenSetupDone != null)
		{
			LoadingManager.OnLoadingScreenSetupDone();
		}
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x00005444 File Offset: 0x00003644
	private void Start()
	{
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x00021CA4 File Offset: 0x0001FEA4
	private void OnDestroy()
	{
		GameMaster.OnUpdateLoadProgress = (Action<float>)Delegate.Remove(GameMaster.OnUpdateLoadProgress, new Action<float>(this.OnUpdateLoadProgress));
		AssetBundleControl.OnUpdateLoadProgress = (Action<float>)Delegate.Remove(AssetBundleControl.OnUpdateLoadProgress, new Action<float>(this.OnUpdateLoadProgress));
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00021CF1 File Offset: 0x0001FEF1
	private void OnUpdateLoadProgress(float _progress)
	{
		this.loadingBar.localScale = new Vector3(_progress, 1f, 1f);
		this.minimalBar.fillAmount = _progress;
	}

	// Token: 0x0400066D RID: 1645
	public static Action OnLoadingScreenSetupDone;

	// Token: 0x0400066E RID: 1646
	public GameObject allElementsHolder;

	// Token: 0x0400066F RID: 1647
	public Canvas standardVersion;

	// Token: 0x04000670 RID: 1648
	public Canvas blackVersion;

	// Token: 0x04000671 RID: 1649
	public Canvas blueVersion;

	// Token: 0x04000672 RID: 1650
	public Canvas doneyVersion;

	// Token: 0x04000673 RID: 1651
	public Canvas whiteVersion;

	// Token: 0x04000674 RID: 1652
	public Canvas minimalVersion;

	// Token: 0x04000675 RID: 1653
	public Text leftText;

	// Token: 0x04000676 RID: 1654
	public Text rightText;

	// Token: 0x04000677 RID: 1655
	public RectTransform loadingBar;

	// Token: 0x04000678 RID: 1656
	public Image minimalBar;

	// Token: 0x020003C7 RID: 967
	public enum LoadScreenStyle
	{
		// Token: 0x040013F6 RID: 5110
		Standard,
		// Token: 0x040013F7 RID: 5111
		Message,
		// Token: 0x040013F8 RID: 5112
		Black,
		// Token: 0x040013F9 RID: 5113
		Blue,
		// Token: 0x040013FA RID: 5114
		DoneyWithTheFunny,
		// Token: 0x040013FB RID: 5115
		White,
		// Token: 0x040013FC RID: 5116
		Minimal
	}
}
